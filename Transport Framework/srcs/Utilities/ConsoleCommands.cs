using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class ConsoleCommandsUtility
	{
		internal static void Register()
		{
			ModEntry.Helper.ConsoleCommands.Add("tf_summary", "This command lists stations.\n\nUsage: tf_summary [manifestsUniqueId = \"all\"] [location = \"all\"] [network = \"all\"]\nLists stations that match the provided command arguments. The \"all\" keyword matches all stations.\n- manifestsUniqueId: The unique ID of a mod's manifest.\n- location: The name of a location.\n- network: The name of a network.", TF_summary);
			ModEntry.Helper.ConsoleCommands.Add("tf_show", "This command shows detailed information about a station.\n\nUsage: tf_show [stationId = \"all\"]\nShows the detailed information about the station that matches the provided command argument. The \"all\" keyword shows detailed information about all stations.\n- stationId : The station identifier.", TF_show);
			ModEntry.Helper.ConsoleCommands.Add("tf_update", "This command updates a station and displays whether its conditions are satisfied.\n\nUsage: tf_update [stationId = \"all\"]\nUpdates the station that matches the provided command argument. The \"all\" keyword updates all stations.\n- stationId: The station identifier.", TF_update);
		}

		private static void TF_summary(string command, string[] args)
		{
			if (!ArgUtility.TryGetOptional(args, 0, out string manifestsUniqueId, out string error, "all") || !ArgUtility.TryGetOptional(args, 1, out string location, out error, "all") || !ArgUtility.TryGetOptional(args, 2, out string network, out error, "all"))
			{
				ModEntry.Monitor.Log(error, LogLevel.Error);
				return;
			}

			StringBuilder summary = new();
			string lastManifestUniqueId = null;
			IEnumerable<Station> filteredByManifestsUniqueId = !manifestsUniqueId.ToLower().Equals("all") ? ModEntry.Stations.Where(s => s.ContentPack.Manifest.UniqueID.Equals(manifestsUniqueId)) : ModEntry.Stations;
			IEnumerable<Station> filteredByLocation = !location.ToLower().Equals("all") ? filteredByManifestsUniqueId.Where(s => s.Location.Equals(location)) : filteredByManifestsUniqueId;
			IEnumerable<Station> filteredByNetwork = !network.ToLower().Equals("all") ? filteredByLocation.Where(s => s.Network.Equals(network)) : filteredByLocation;
			int count = filteredByNetwork.Count();

			if (count == 0)
			{
				summary.Append("No station found.\n");
			}
			else
			{
				summary.Append($"List of {count} stations found:\n");
				foreach (Station station in filteredByNetwork)
				{
					if (!station.ContentPack.Manifest.UniqueID.Equals(lastManifestUniqueId))
					{
						summary.Append($"\n{station.ContentPack.Manifest.UniqueID}\n");
						lastManifestUniqueId = station.ContentPack.Manifest.UniqueID;
					}
					summary.Append($"- {station.Id}\n");
				}
			}
			ModEntry.Monitor.Log(summary.ToString(), LogLevel.Info);
		}

		private static void TF_show(string command, string[] args)
		{
			if (!ArgUtility.TryGetOptional(args, 0, out string stationId, out string error, "all"))
			{
				ModEntry.Monitor.Log(error, LogLevel.Error);
				return;
			}

			StringBuilder summary = new();
			IEnumerable<Station> filteredByStationId = !stationId.ToLower().Equals("all") ? ModEntry.Stations.Where(s => s.Id.Equals(stationId)) : ModEntry.Stations;
			int count = filteredByStationId.Count();

			if (count == 0)
			{
				summary.Append("No station found.\n");
			}
			else
			{
				int tabs = 0;

				void Append(string text)
				{
					for (int i = 0; i < tabs; i++)
					{
						summary.Append('\t');
					}
					summary.Append(text);
				}

				void AppendProperty(string name, object value)
				{
					if (name is not null)
					{
						Append($"{name}: {value ?? "null"}\n");
					}
					else
					{
						Append($"{value ?? "null"}\n");
					}
				}

				void AppendOpen(string name, object value, char character)
				{
					if (value is null)
					{
						Append($"{name}: null\n");
					}
					else
					{
						if (name is not null)
						{
							Append($"{name}:\n");
						}
						Append($"{character}\n");
						tabs++;
					}
				}

				void AppendClose(object value, char character)
				{
					if (value is not null)
					{
						tabs--;
						Append($"{character}\n");
					}
				}

				void AppendObjectOpen(string name, object value)
				{
					AppendOpen(name, value, '{');
				}

				void AppendObjectClose(object value)
				{
					AppendClose(value, '}');
				}

				void AppendArrayOpen(string name, object value)
				{
					AppendOpen(name, value, '[');
				}

				void AppendArrayClose(object value)
				{
					AppendClose(value, ']');
				}

				void AppendArrayComma()
				{
					summary.Remove(summary.Length - 1, 1);
					summary.Append(",\n");
				}

				void RemoveArrayLastComma()
				{
					summary.Remove(summary.Length - 2, 2);
					summary.Append('\n');
				}

				void AppendPoint(string name, Point point)
				{
					AppendObjectOpen(name, point);
					AppendProperty(nameof(point.X), point.X);
					AppendArrayComma();
					AppendProperty(nameof(point.Y), point.Y);
					AppendObjectClose(point);
				}

				void AppendSSprite(string name, SSprite ssprite)
				{
					AppendObjectOpen(name, ssprite);
					if (ssprite is not null)
					{
						AppendProperty(nameof(ssprite.Type), ssprite.Type);
						AppendArrayComma();
						AppendSSData(nameof(ssprite.Data), ssprite.Data);
						AppendArrayComma();
						AppendItems(nameof(ssprite.AbsoluteCollisionBoxes), ssprite.AbsoluteCollisionBoxes, AppendRectangle);
						AppendArrayComma();
						AppendItems(nameof(ssprite.ComputedCollisionBoxes), ssprite.ComputedCollisionBoxes, AppendRectangle);
						AppendArrayComma();
						AppendItems(nameof(ssprite.CollisionBoxes), ssprite.CollisionBoxes, AppendSSCollisionBox);
						AppendArrayComma();
						AppendProperty(nameof(ssprite.ConditionsCache), ssprite.ConditionsCache);
						AppendArrayComma();
						AppendItems(nameof(ssprite.Conditions), ssprite.Conditions, AppendSSCondition);
					}
					AppendObjectClose(ssprite);
				}

				void AppendSSData(string name, SSData ssdata)
				{
					AppendObjectOpen(name, ssdata);
					if (ssdata is not null)
					{
						AppendProperty(nameof(ssdata.InternalTextureName), ssdata.InternalTextureName);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.TextureName), ssdata.TextureName);
						AppendArrayComma();
						AppendVector(nameof(ssdata.ComputedPosition), ssdata.ComputedPosition);
						AppendArrayComma();
						AppendVector(nameof(ssdata.Position), ssdata.Position);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.Interval), ssdata.Interval);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.AnimationLength), ssdata.AnimationLength);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.Flicker), ssdata.Flicker);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.Flip), ssdata.Flip);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.VerticalFlip), ssdata.VerticalFlip);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.ComputedLayerDepth), ssdata.ComputedLayerDepth);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.LayerDepth), ssdata.LayerDepth);
						AppendArrayComma();
						AppendColor(nameof(ssdata.ColorAsColor), ssdata.ColorAsColor);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.Color), ssdata.Color);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.Scale), ssdata.Scale);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.Rotation), ssdata.Rotation);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.ShakeIntensity), ssdata.ShakeIntensity);
						AppendArrayComma();
						AppendSSDLight(nameof(ssdata.Light), ssdata.Light);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.PingPong), ssdata.PingPong);
						AppendArrayComma();
						AppendProperty(nameof(ssdata.DrawAboveAlwaysFront), ssdata.DrawAboveAlwaysFront);
					}
					AppendObjectClose(ssdata);
				}

				void AppendVector(string name, Vector2 vector)
				{
					AppendObjectOpen(name, vector);
					AppendProperty(nameof(vector.X), vector.X);
					AppendArrayComma();
					AppendProperty(nameof(vector.Y), vector.Y);
					AppendObjectClose(vector);
				}

				void AppendColor(string name, Color color)
				{
					AppendObjectOpen(name, color);
					AppendProperty(nameof(color.R), color.R);
					AppendArrayComma();
					AppendProperty(nameof(color.G), color.G);
					AppendArrayComma();
					AppendProperty(nameof(color.B), color.B);
					AppendArrayComma();
					AppendProperty(nameof(color.A), color.A);
					AppendObjectClose(color);
				}

				void AppendSSDLight(string name, SSDLight ssdLight)
				{
					AppendObjectOpen(name, ssdLight);
					if (ssdLight is not null)
					{
						AppendColor(nameof(ssdLight.ColorAsColor), ssdLight.ColorAsColor);
						AppendArrayComma();
						AppendProperty(nameof(ssdLight.Color), ssdLight.Color);
						AppendArrayComma();
						AppendProperty(nameof(ssdLight.Radius), ssdLight.Radius);
					}
					AppendObjectClose(ssdLight);
				}

				void AppendRectangle(string name, Rectangle rectangle)
				{
					AppendObjectOpen(name, rectangle);
					AppendProperty(nameof(rectangle.X), rectangle.X);
					AppendArrayComma();
					AppendProperty(nameof(rectangle.Y), rectangle.Y);
					AppendArrayComma();
					AppendProperty(nameof(rectangle.Width), rectangle.Width);
					AppendArrayComma();
					AppendProperty(nameof(rectangle.Height), rectangle.Height);
					AppendObjectClose(rectangle);
				}

				void AppendSSCollisionBox(string name, SSCollisionBox sscollisionBox)
				{
					AppendObjectOpen(name, sscollisionBox);
					if (sscollisionBox is not null)
					{
						AppendProperty(nameof(sscollisionBox.X), sscollisionBox.X);
						AppendArrayComma();
						AppendProperty(nameof(sscollisionBox.Y), sscollisionBox.Y);
						AppendArrayComma();
						AppendProperty(nameof(sscollisionBox.Width), sscollisionBox.Width);
						AppendArrayComma();
						AppendProperty(nameof(sscollisionBox.Height), sscollisionBox.Height);
					}
					AppendObjectClose(sscollisionBox);
				}

				void AppendSSCondition(string name, SSCondition sscondition)
				{
					AppendObjectOpen(name, sscondition);
					if (sscondition is not null)
					{
						AppendProperty(nameof(sscondition.Cache), sscondition.Cache);
						AppendArrayComma();
						AppendProperty(nameof(sscondition.Query), sscondition.Query);
						AppendArrayComma();
						AppendProperty(nameof(sscondition.Update), sscondition.Update);
					}
					AppendObjectClose(sscondition);
				}

				void AppendSCondition(string name, SCondition scondition)
				{
					AppendObjectOpen(name, scondition);
					if (scondition is not null)
					{
						AppendProperty(nameof(scondition.Cache), scondition.Cache);
						AppendArrayComma();
						AppendProperty(nameof(scondition.Query), scondition.Query);
						AppendArrayComma();
						AppendProperty(nameof(scondition.LockedMessage), scondition.LockedMessage);
						AppendArrayComma();
						AppendProperty(nameof(scondition.Update), scondition.Update);
					}
					AppendObjectClose(scondition);
				}

				void AppendSEvent(string name, SEvent sevent)
				{
					AppendObjectOpen(name, sevent);
					if (sevent is not null)
					{
						AppendProperty(nameof(sevent.Type), sevent.Type);
						AppendArrayComma();
						AppendProperty(nameof(sevent.Id), sevent.Id);
						AppendArrayComma();
						AppendProperty(nameof(sevent.Script), sevent.Script);
						AppendArrayComma();
						AppendProperty(nameof(sevent.Location), sevent.Location);
						AppendArrayComma();
						AppendSEFilter(nameof(sevent.Filter), sevent.Filter);
					}
					AppendObjectClose(sevent);
				}

				void AppendSEFilter(string name, SEFilter sefilter)
				{
					AppendObjectOpen(name, sefilter);
					if (sefilter is not null)
					{
						AppendItems(nameof(sefilter.IncludeStations), sefilter.IncludeStations, AppendProperty);
						AppendArrayComma();
						AppendItems(nameof(sefilter.ExcludeStations), sefilter.ExcludeStations, AppendProperty);
						AppendArrayComma();
						AppendProperty(nameof(sefilter.TravelCount), sefilter.TravelCount);
					}
					AppendObjectClose(sefilter);
				}

				void AppendItems<T>(string name, IEnumerable<T> items, Action<string, T> appendItem)
				{
					AppendArrayOpen(name, items);
					if (items is not null)
					{
						foreach (T item in items)
						{
							appendItem(null, item);
							AppendArrayComma();
						}
						RemoveArrayLastComma();
					}
					AppendArrayClose(items);
				}

				void AppendStation(string manifestUniqueID, Station station)
				{
					summary.Append($"\n=== {manifestUniqueID} ===\n");
					if (station is not null)
					{
						AppendProperty(nameof(station.Id), station.Id);
						AppendProperty(nameof(station.DisplayName), station.DisplayName);
						AppendProperty(nameof(station.LocalizedDisplayName), station.LocalizedDisplayName);
						AppendProperty(nameof(station.Location), station.Location);
						AppendPoint(nameof(station.Tile), station.Tile);
						AppendProperty(nameof(station.Direction), station.Direction);
						AppendProperty(nameof(station.DirectionAsInt), station.DirectionAsInt);
						AppendProperty(nameof(station.Price), station.Price);
						AppendProperty(nameof(station.Network), station.Network);
						AppendItems(nameof(station.AccessTiles), station.AccessTiles, AppendPoint);
						AppendProperty(nameof(station.ConditionsCache), station.ConditionsCache);
						AppendProperty(nameof(station.ConditionsLockedMessageCache), station.ConditionsLockedMessageCache);
						AppendItems(nameof(station.Sprites), station.Sprites, AppendSSprite);
						AppendItems(nameof(station.Conditions), station.Conditions, AppendSCondition);
						AppendItems(nameof(station.Events), station.Events, AppendSEvent);
						AppendProperty(nameof(station.Sound), station.Sound);
						AppendItems(nameof(station.Include), station.Include, AppendProperty);
						AppendItems(nameof(station.IncludeDeparture), station.IncludeDeparture, AppendProperty);
						AppendItems(nameof(station.IncludeArrival), station.IncludeArrival, AppendProperty);
						AppendItems(nameof(station.Exclude), station.Exclude, AppendProperty);
						AppendItems(nameof(station.ExcludeDeparture), station.ExcludeDeparture, AppendProperty);
						AppendItems(nameof(station.ExcludeArrival), station.ExcludeArrival, AppendProperty);
						AppendItems(nameof(station.IgnoreConditions), station.IgnoreConditions, AppendProperty);
						AppendItems(nameof(station.IgnoreConditionsDeparture), station.IgnoreConditionsDeparture, AppendProperty);
						AppendItems(nameof(station.IgnoreConditionsArrival), station.IgnoreConditionsArrival, AppendProperty);
					}
				}

				summary.Append($"List of {count} stations found:\n");
				foreach (Station station in filteredByStationId)
				{
					AppendStation(station.ContentPack.Manifest.UniqueID, station);
				}
			}
			ModEntry.Monitor.Log(summary.ToString(), LogLevel.Info);
		}

		private static void TF_update(string command, string[] args)
		{
			if (!Context.IsWorldReady)
			{
				ModEntry.Monitor.Log("You must load a save to use this command.", LogLevel.Error);
				return;
			}
			if (!ArgUtility.TryGetOptional(args, 0, out string stationId, out string error, "all"))
			{
				ModEntry.Monitor.Log(error, LogLevel.Error);
				return;
			}

			StringBuilder summary = new();
			string lastManifestUniqueId = null;
			IEnumerable<Station> filteredByStationId = !stationId.ToLower().Equals("all") ? ModEntry.Stations.Where(s => s.Id.Equals(stationId)) : ModEntry.Stations;
			int count = filteredByStationId.Count();

			if (count == 0)
			{
				summary.Append("No station found.\n");
			}
			else
			{
				summary.Append($"List of {count} stations found:\n");
				foreach (Station station in filteredByStationId)
				{
					if (!station.ContentPack.Manifest.UniqueID.Equals(lastManifestUniqueId))
					{
						summary.Append($"\n{station.ContentPack.Manifest.UniqueID}\n");
						lastManifestUniqueId = station.ContentPack.Manifest.UniqueID;
					}
					StationsUtility.UpdateOnSaveLoad(station);
					summary.Append($"- {station.Id}: {station.ConditionsCache}\n");
				}
			}
			ModEntry.Monitor.Log(summary.ToString(), LogLevel.Info);
		}
	}
}
