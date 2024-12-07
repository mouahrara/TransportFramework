using System.Reflection;
using StardewModdingAPI;
using StardewValley;
using TransportFramework.Classes;
using TransportFramework.Utilities;

namespace TransportFramework.Api
{
	public class TransportFrameworkApi : ITransportFrameworkApi
	{
		public IStation GetStation(string id)
		{
			foreach (Station station in ModEntry.Stations)
			{
				if (station.Id.Equals(id))
				{
					return new StationAdapter(station);
				}
			}
			return null;
		}

		public bool SetStation(string id, IStation IStation)
		{
			if (!Context.IsWorldReady)
			{
				ModEntry.Monitor.Log($"Failed to set station (Id: {IStation.Id}): No save loaded.", LogLevel.Error);
				return false;
			}
			for (int i = 0; i < ModEntry.Stations.Count; i++)
			{
				if (ModEntry.Stations[i].Id.Equals(id))
				{
					if (IStation is not null)
					{
						return AddOrSet(IStation, i);
					}
					else
					{
						return RemoveStation(i);
					}
				}
			}
			return AddOrSet(IStation);
		}

		private static bool AddOrSet(IStation IStation, int index = -1)
		{
			Station station = ((StationAdapter)IStation).Station;

			if (StationsUtility.IsStationValidGameLaunched(station) && StationsUtility.IsStationValidSaveLoaded(station))
			{
				StationsUtility.ComputeProperties(station);
				StationsUtility.Localize(station);
				StationsUtility.GenerateUpdateEnumerables();
				if (index < 0)
				{
					ModEntry.Stations.Add(station);
				}
				else
				{
					ModEntry.Stations[index] = station;
				}
				ResetLocalStateIfNeeded(station.Location);
				return true;
			}
			return false;
		}

		private static bool RemoveStation(int index)
		{
			if (0 <= index && index < ModEntry.Stations.Count)
			{
				string location = ModEntry.Stations[index].Location;

				ModEntry.Stations.RemoveAt(index);
				ResetLocalStateIfNeeded(location);
				return true;
			}
			return false;
		}

		private static void ResetLocalStateIfNeeded(string location)
		{
			if (Game1.currentLocation?.Name == location)
			{
				typeof(GameLocation).GetMethod("resetLocalState", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Game1.currentLocation, null);
			}
		}

		public bool UpdateStation(string id, ITransportFrameworkApi.TypeMask typeMask = ITransportFrameworkApi.TypeMask.All, ITransportFrameworkApi.UpdateMask updateMask = ITransportFrameworkApi.UpdateMask.All)
		{
			foreach (Station station in ModEntry.Stations)
			{
				if (station.Id.Equals(id))
				{
					if (typeMask.HasFlag(ITransportFrameworkApi.TypeMask.StationCondition))
					{
						if (updateMask.HasFlag(ITransportFrameworkApi.UpdateMask.OnDayStart))
						{
							StationsUtility.UpdateConditions(ModEntry.OnDayStartConditions, station);
							StationsUtility.UpdateStations(ModEntry.OnDayStartStations, station);
						}
						if (updateMask.HasFlag(ITransportFrameworkApi.UpdateMask.OnInteract))
						{
							StationsUtility.UpdateConditions(ModEntry.OnInteractConditions, station);
							StationsUtility.UpdateStations(ModEntry.OnInteractStations, station);
						}
					}
					if (typeMask.HasFlag(ITransportFrameworkApi.TypeMask.SpriteCondition))
					{
						if (updateMask.HasFlag(ITransportFrameworkApi.UpdateMask.OnDayStart))
						{
							StationsUtility.UpdateSpriteConditions(ModEntry.OnDayStartSpriteConditions, station);
							StationsUtility.UpdateSprites(ModEntry.OnDayStartSprites, station);
						}
						if (updateMask.HasFlag(ITransportFrameworkApi.UpdateMask.OnLocationChange))
						{
							StationsUtility.UpdateSpriteConditions(ModEntry.OnLocationChangeSpriteConditions, station);
							StationsUtility.UpdateSprites(ModEntry.OnLocationChangeSprites, station);
						}
					}
					return true;
				}
			}
			return false;
		}
	}
}
