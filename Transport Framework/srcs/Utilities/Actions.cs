using Microsoft.Xna.Framework;
using xTile.Layers;
using StardewValley;
using StardewValley.Extensions;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class ActionsUtility
	{
		public static bool	OpenStationAtTile(int x, int y)
		{
			Station station = GetStationAtTile(x, y);

			if (station is not null)
			{
				MenuUtility.TryToOpen(station);
				return true;
			}
			return false;
		}

		public static Station GetStationAtTile(int x, int y)
		{
			Layer buildingsLayer = Game1.currentLocation?.Map?.GetLayer("Buildings");

			if (buildingsLayer is not null)
			{
				if (buildingsLayer.Tiles[x, y] is null || !buildingsLayer.Tiles[x, y].Properties.TryGetValue("Action", out string fullActionString))
				{
					fullActionString = Game1.currentLocation.doesTileHaveProperty(x, y, "Action", "Buildings");
				}
				if (fullActionString is not null)
				{
					string[] action = ArgUtility.SplitBySpace(fullActionString);

					if (ArgUtility.Get(action, 0) == "MinecartTransport")
					{
						string networkId = ArgUtility.Get(action, 1) ?? "Default";
						string excludeDestinationId = ArgUtility.Get(action, 2);

						if (ModEntry.CurrentLocationStations is not null)
						{
							string network = networkId.Equals("Default") ? "Minecart" : networkId;

							foreach (Station station in ModEntry.CurrentLocationStations)
							{
								if (station.Network.Equals(network) && (string.IsNullOrWhiteSpace(excludeDestinationId) || station.Id.Equals($"{network}_{excludeDestinationId}")))
								{
									return station;
								}
							}
						}
					}
				}
				if (buildingsLayer.Tiles[x, y] is not null && string.IsNullOrEmpty(Game1.currentLocation.doesTileHaveProperty(x, y, "Passable", "Buildings")))
				{
					if (ModEntry.CurrentLocationStations is not null)
					{
						foreach (Station station in ModEntry.CurrentLocationStations)
						{
							if (station.AccessTiles is not null)
							{
								foreach (Point accessTile in station.AccessTiles)
								{
									if (x == accessTile.X && y == accessTile.Y)
									{
										return station;
									}
								}
							}
						}
					}
				}
			}
			return null;
		}
	}
}
