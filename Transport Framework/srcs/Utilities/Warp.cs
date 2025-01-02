using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class WarpUtility
	{
		public static bool TryToWarp(Station departureStation, Station arrivalStation)
		{
			if (Game1.player.Money < arrivalStation.Price)
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_NotEnoughMoneyForTicket"));
				return false;
			}

			LocationRequest destination = Game1.getLocationRequest(arrivalStation.Location);

			if (Game1.player.IsSitting())
			{
				Game1.player.StopSitting(false);
			}
			if (Game1.player.swimming.Value)
			{
				Game1.player.swimming.Value = false;
				Game1.player.changeOutOfSwimSuit();
			}

			Horse horse = Game1.player.mount;
			bool horsebackTravel = Game1.player.isRidingHorse();

			if (horsebackTravel)
			{
				horse.dismount();
			}
			destination.OnWarp += () =>
			{
				if (horsebackTravel)
				{
					if ((destination.Location.IsOutdoors || ModConfig.AllowIndoorHorsebackTravel) && destination.Location.isTilePassable(new Vector2(arrivalStation.Tile.X, arrivalStation.Tile.Y)) && !destination.Location.IsTileOccupiedBy(new Vector2(arrivalStation.Tile.X, arrivalStation.Tile.Y), CollisionMask.All, CollisionMask.All) && destination.Location.isTilePassable(new Vector2(arrivalStation.Tile.X + 1, arrivalStation.Tile.Y)) && !destination.Location.IsTileOccupiedBy(new Vector2(arrivalStation.Tile.X + 1, arrivalStation.Tile.Y), CollisionMask.All, CollisionMask.All))
					{
						if (destination.Location.currentEvent is not null)
						{
							if (!destination.Location.currentEvent.isFestival)
							{
								destination.Location.currentEvent.onEventFinished += () => WarpHorseAndMount(horse, destination.Location, arrivalStation.Tile.X, arrivalStation.Tile.Y, arrivalStation.DirectionAsInt);
							}
						}
						else
						{
							WarpHorseAndMount(horse, destination.Location, arrivalStation.Tile.X, arrivalStation.Tile.Y, arrivalStation.DirectionAsInt);
						}
					}
				}
				Game1.player.Money -= arrivalStation.Price;
				Game1.player.stats.Increment($"{ModEntry.ModManifest.UniqueID}_TravelFrom_{departureStation.Id}", 1);
				Game1.player.stats.Increment($"{ModEntry.ModManifest.UniqueID}_TravelFrom_{departureStation.Id}_To_{arrivalStation.Id}", 1);
				TouchActionsUtility.LastTouchAction = arrivalStation.Id;
			};

			if (!ModEntry.Config.SkipTravelAnimations)
			{
				PlayTravelAnimationsAndWarpFarmer(departureStation, arrivalStation, destination);
			}
			else
			{
				if (!string.IsNullOrEmpty(arrivalStation.Sound))
				{
					Game1.playSound(arrivalStation.Sound);
				}
				Game1.warpFarmer(destination, arrivalStation.Tile.X, arrivalStation.Tile.Y, arrivalStation.DirectionAsInt);
			}
			return true;
		}

		private static void PlayTravelAnimationsAndWarpFarmer(Station departureStation, Station arrivalStation, LocationRequest locationRequest)
		{
			uint TravelCount = Game1.player.stats.Get($"{ModEntry.ModManifest.UniqueID}_TravelFrom_{departureStation.Id}") + 1;
			uint StationIdTravelCount = Game1.player.stats.Get($"{ModEntry.ModManifest.UniqueID}_TravelFrom_{departureStation.Id}_To_{arrivalStation.Id}") + 1;
			IEnumerable<SEvent> departureEvents = FilterEvents(departureStation, "Departure", arrivalStation.Id, TravelCount, StationIdTravelCount) ?? Enumerable.Empty<SEvent>();
			IEnumerable<SEvent> arrivalEvents = FilterEvents(arrivalStation, "Arrival", departureStation.Id, TravelCount, StationIdTravelCount) ?? Enumerable.Empty<SEvent>();
			List<SEvent> events = departureEvents.Concat(arrivalEvents).ToList();
			List<Event> gameEvents = new();

			for (int i = 0; i < events.Count; i++)
			{
				string eventId = events[i].Id;
				GameLocation eventLocation = Game1.getLocationFromName(events[i].Location ?? (events[i].Type.Equals("Departure") ? departureStation.Location : arrivalStation.Location));

				if (events[i].Id is not null)
				{
					if (events[i].Id.Contains('/'))
					{
						eventId = Event.SplitPreconditions(events[i].Id)[0];
					}
				}

				Event gameEvent;

				if (events[i].Script is null)
				{
					gameEvent = eventLocation.findEventById(events[i].Id, Game1.player);
				}
				else
				{
					gameEvent = new(TokensUtility.ParseText(events[i].Type.Equals("Departure") ? departureStation : arrivalStation, events[i].Script), null, eventId, Game1.player)
					{
						showActiveObject = false,
						showGroundObjects = true,
						showWorldCharacters = true,
						ignoreObjectCollisions = false,
						skippable = true
					};
				}
				gameEvents.Add(gameEvent);
			}
			if (!gameEvents.Any())
			{
				if (!string.IsNullOrEmpty(arrivalStation.Sound))
				{
					Game1.playSound(arrivalStation.Sound);
				}
				Game1.warpFarmer(locationRequest, arrivalStation.Tile.X, arrivalStation.Tile.Y, arrivalStation.DirectionAsInt);
				return;
			}
			for (int i = 0; i < gameEvents.Count; i++)
			{
				int iCapture = i;

				gameEvents[i].onEventFinished += () =>
				{
					foreach (NPC actor in gameEvents[iCapture].actors)
					{
						actor.shouldShadowBeOffset = false;
						actor.drawOffset.X = 0.0f;
						actor.drawOffset.Y = 0.0f;
					}
					foreach (Farmer farmerActor in gameEvents[iCapture].farmerActors)
					{
						farmerActor.shouldShadowBeOffset = false;
						farmerActor.drawOffset.X = 0.0f;
						farmerActor.drawOffset.Y = 0.0f;
					}
					if (!gameEvents[iCapture].skipped && iCapture < gameEvents.Count - 1)
					{
						WarpAndStartEvent(departureStation.Location, arrivalStation.Location, events.ElementAt(iCapture + 1), gameEvents[iCapture + 1], false, false);
					}
					else
					{
						if (!string.IsNullOrEmpty(arrivalStation.Sound))
						{
							Game1.playSound(arrivalStation.Sound);
						}
						PerformWarpFarmerWithoutFadeScreenToBlack(locationRequest, arrivalStation.Tile.X, arrivalStation.Tile.Y, arrivalStation.DirectionAsInt);
						GameLocation.HandleMusicChange(null, locationRequest.Location);
					}
				};
			}
			WarpAndStartEvent(departureStation.Location, arrivalStation.Location, events.First(), gameEvents[0], true, true);
		}

		private static IEnumerable<SEvent> FilterEvents(Station station, string type, string stationId, uint travelCount, uint stationIdTravelCount)
		{
			if (station.Events is not null)
			{
				IEnumerable<SEvent> filteredByTypeEvents = station.Events.Where(e => e.Type == type);
				IEnumerable<SEvent> filteredByPreconditionsEvents = filteredByTypeEvents.Where(e =>
				{
					if (!string.IsNullOrWhiteSpace(e.Id))
					{
						string eventId = Game1.getLocationFromName(e.Location ?? station.Location).checkEventPrecondition(e.Id, false);

						if (string.IsNullOrEmpty(eventId) || eventId.Equals("-1"))
						{
							return false;
						}
					}
					return true;
				});
				IEnumerable<SEvent> filteredByStationsAndTravelCountEvents = filteredByPreconditionsEvents.Where(e => (e.Filter.IncludeStations is not null || e.Filter.ExcludeStations is not null) && (e.Filter.IncludeStations is null || e.Filter.IncludeStations.Contains(stationId)) && (e.Filter.ExcludeStations is null || !e.Filter.ExcludeStations.Contains(stationId)) && e.Filter.TravelCount == stationIdTravelCount);

				if (filteredByStationsAndTravelCountEvents.Any())
				{
					return filteredByStationsAndTravelCountEvents;
				}

				IEnumerable<SEvent> filteredByTravelCountEvents = filteredByPreconditionsEvents.Where(e => e.Filter.IncludeStations is null && e.Filter.ExcludeStations is null && e.Filter.TravelCount == travelCount);

				if (filteredByTravelCountEvents.Any())
				{
					return filteredByTravelCountEvents;
				}

				IEnumerable<SEvent> filteredByStationsEvents = filteredByPreconditionsEvents.Where(e => (e.Filter.IncludeStations is not null || e.Filter.ExcludeStations is not null) && (e.Filter.IncludeStations is null || e.Filter.IncludeStations.Contains(stationId)) && (e.Filter.ExcludeStations is null || !e.Filter.ExcludeStations.Contains(stationId)) && e.Filter.TravelCount == 0);

				if (filteredByStationsEvents.Any())
				{
					return filteredByStationsEvents;
				}
				return filteredByPreconditionsEvents.Where(e => e.Filter.IncludeStations is null && e.Filter.ExcludeStations is null && e.Filter.TravelCount == 0);
			}
			return null;
		}

		private static void WarpAndStartEvent(string departureStationLocation, string arrivalStationLocation, SEvent @event, Event gameEvent, bool fadeScreenToBlack, bool isFirstEvent)
		{
			string eventLocation = @event.Location ?? (@event.Type.Equals("Departure") ? departureStationLocation : arrivalStationLocation);

			if (!isFirstEvent || Game1.currentLocation != Game1.getLocationFromName(eventLocation))
			{
				LocationRequest eventLocationRequest = Game1.getLocationRequest(eventLocation);

				eventLocationRequest.OnWarp += () =>
				{
					EventCommandsUtility.StartEvent(@event, gameEvent);
				};
				if (fadeScreenToBlack)
				{
					Game1.warpFarmer(eventLocationRequest, -1, -1, 2);
				}
				else
				{
					PerformWarpFarmerWithoutFadeScreenToBlack(eventLocationRequest, -1, -1, 2);
				}
			}
			else
			{
				EventCommandsUtility.StartEvent(@event, gameEvent);
			}
		}

		private static void PerformWarpFarmerWithoutFadeScreenToBlack(LocationRequest locationRequest, int tileX, int tileY, int facingDirectionAfterWarp)
		{
			if (locationRequest.Location is not null)
			{
				if (tileX >= locationRequest.Location.Map.Layers[0].LayerWidth - 1)
				{
					tileX--;
				}
				if (Game1.IsMasterGame)
				{
					locationRequest.Location.hostSetup();
				}
			}
			ModEntry.Monitor.Log("Warping to " + locationRequest.Name, LogLevel.Trace);
			if (Game1.player.IsSitting())
			{
				Game1.player.StopSitting(animate: false);
			}
			Game1.player.previousLocationName = (Game1.player.currentLocation != null) ? Game1.player.currentLocation.Name : "";
			Game1.locationRequest = locationRequest;
			Game1.xLocationAfterWarp = tileX;
			Game1.yLocationAfterWarp = tileY;
			typeof(Game1).GetField("_isWarping", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, true);
			Game1.facingDirectionAfterWarp = facingDirectionAfterWarp;
			Game1.setRichPresence("location", locationRequest.Name);
		}

		private static void WarpHorseAndMount(Horse horse, GameLocation location, int x, int y, int direction)
		{
			Game1.warpCharacter(horse, location, new Vector2(x, y));
			horse.faceDirection(direction);
			horse.rider = Game1.player;
			horse.rider.freezePause = 5000;
			horse.rider.synchronizedJump(6f);
			horse.rider.Halt();
			Game1.currentLocation?.playSound("dwop");
			horse.mounting.Value = true;
			horse.rider.isAnimatingMount = true;
			horse.rider.completelyStopAnimatingOrDoingAction();
		}
	}
}
