using xTile.Layers;
using StardewValley;

namespace TransportFramework.Utilities
{
	internal class MouseCursorUtility
	{
		public static void Update()
		{
			if (Game1.mouseCursorTransparency <= 0.0f)
				return;

			int xTile = (Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize;
			int yTile = (Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize;

			if (ActionsUtility.GetStationAtTile(xTile, yTile) is not null)
			{
				Game1.mouseCursor = Game1.cursor_grab;
				Game1.mouseCursorTransparency = Utility.tileWithinRadiusOfPlayer(xTile, yTile, 1, Game1.player) ? 1f : 0.5f;
			}
			else
			{
				Layer frontLayer = Game1.currentLocation?.Map?.GetLayer("Front");

				if ((frontLayer is not null && frontLayer.Tiles[xTile, yTile] is not null) || ActionsUtility.IsFrontLayerOfAnyStation(xTile, yTile))
				{
					if (ActionsUtility.GetStationAtTile(xTile, yTile + 1) is not null)
					{
						Game1.mouseCursor = Game1.cursor_grab;
						Game1.mouseCursorTransparency = Utility.tileWithinRadiusOfPlayer(xTile, yTile, 1, Game1.player) ? 1f : 0.5f;
					}
				}
			}
		}
	}
}
