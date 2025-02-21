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
				if (TryGetStationAtTileFromMapActions(buildingsLayer, x, y, out Station station))
				{
					return station;
				}
				if (buildingsLayer.Tiles[x, y] is not null && string.IsNullOrEmpty(Game1.currentLocation.doesTileHaveProperty(x, y, "Passable", "Buildings")))
				{
					if (TryGetStationAtTileFromAccessTiles(x, y, out station))
					{
						return station;
					}
				}
			}
			if (IsCollidingWithAnyStation(x, y))
			{
				if (TryGetStationAtTileFromAccessTiles(x, y, out Station station))
				{
					return station;
				}
			}
			return null;
		}

		private static bool TryGetStationAtTileFromMapActions(Layer buildingsLayer, int x, int y, out Station station)
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

						foreach (Station s in ModEntry.CurrentLocationStations)
						{
							if (s.Network.Equals(network) && (string.IsNullOrWhiteSpace(excludeDestinationId) || s.Id.Equals($"{network}_{excludeDestinationId}")))
							{
								station = s;
								return true;
							}
						}
					}
				}
			}
			station = null;
			return false;
		}

		private static bool TryGetStationAtTileFromAccessTiles(int x, int y, out Station station)
		{
			if (ModEntry.CurrentLocationStations is not null)
			{
				foreach (Station s in ModEntry.CurrentLocationStations)
				{
					if (s.AccessTiles is not null)
					{
						foreach (Point accessTile in s.AccessTiles)
						{
							if (x == accessTile.X && y == accessTile.Y)
							{
								station = s;
								return true;
							}
						}
					}
				}
			}
			station = null;
			return false;
		}

		private static bool IsCollidingWithAnyStation(int x, int y)
		{
			int absoluteX = x * Game1.tileSize;
			int absoluteY = y * Game1.tileSize;

			if (ModEntry.CurrentLocationStations is not null)
			{
				foreach (Station station in ModEntry.CurrentLocationStations)
				{
					if (station.Sprites is not null)
					{
						foreach (SSprite sprite in station.Sprites)
						{
							if (sprite.AbsoluteCollisionBoxes is not null)
							{
								foreach (Rectangle absoluteCollisionBox in sprite.AbsoluteCollisionBoxes)
								{
									if (absoluteCollisionBox.Contains(absoluteX, absoluteY))
									{
										return true;
									}
								}
							}
						}
					}
				}
			}
			return false;
		}

		public static bool IsFrontLayerOfAnyStation(int x, int y)
		{
			int absoluteX = x * Game1.tileSize;
			int absoluteY = y * Game1.tileSize;

			if (ModEntry.CurrentLocationStations is not null)
			{
				foreach (Station station in ModEntry.CurrentLocationStations)
				{
					if (station.Sprites is not null)
					{
						foreach (SSprite sprite in station.Sprites)
						{
							if (sprite.Data is not null)
							{
								Rectangle absoluteSpriteTextureRectangle = new((int)sprite.Data.ComputedPosition.X, (int)sprite.Data.ComputedPosition.Y, (int)(sprite.Data.SourceRectangle.Width * sprite.Data.Scale * Game1.pixelZoom), (int)(sprite.Data.SourceRectangle.Height * sprite.Data.Scale * Game1.pixelZoom));

								if (absoluteSpriteTextureRectangle.Contains(absoluteX, absoluteY))
								{
									if (sprite.AbsoluteCollisionBoxes is not null)
									{
										foreach (Rectangle absoluteCollisionBox in sprite.AbsoluteCollisionBoxes)
										{
											if (!absoluteCollisionBox.Contains(absoluteX, absoluteY) && absoluteCollisionBox.Contains(absoluteX, absoluteY + Game1.tileSize))
											{
												return true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
	}
}
