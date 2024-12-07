using Microsoft.Xna.Framework;
using xTile.Layers;
using StardewModdingAPI.Utilities;
using StardewValley;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class TouchActionsUtility
	{
		private static readonly PerScreen<string>	lastTouchAction = new(() => null);

		public static void Reset()
		{
			LastTouchAction = null;
		}

		public static string LastTouchAction
		{
			get => lastTouchAction.Value;
			set => lastTouchAction.Value = value;
		}

		public static void Handle()
		{
			OpenMenuIfTileIsAccessTile(Game1.player.TilePoint.X, Game1.player.TilePoint.Y);
		}

		private static bool	OpenMenuIfTileIsAccessTile(int x, int y)
		{
			Layer backLayer = Game1.currentLocation?.Map?.GetLayer("Back");
			Layer buildingsLayer = Game1.currentLocation?.Map?.GetLayer("Buildings");

			if (((backLayer is null || backLayer.Tiles[x, y] is null) && (buildingsLayer is null || buildingsLayer.Tiles[x, y] is null)) || (buildingsLayer is not null && buildingsLayer.Tiles[x, y] is not null && string.IsNullOrEmpty(Game1.currentLocation.doesTileHaveProperty(x, y, "Passable", "Buildings"))))
				return false;

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
								if (!station.Id.Equals(LastTouchAction))
								{
									LastTouchAction = station.Id;
									MenuUtility.TryToOpen(station);
									return true;
								}
								else
								{
									return false;
								}
							}
						}
					}
				}
			}
			Reset();
			return false;
		}
	}
}
