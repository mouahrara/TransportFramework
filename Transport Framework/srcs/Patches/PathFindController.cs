using System;
using HarmonyLib;
using StardewValley;
using StardewValley.Pathfinding;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace TransportFramework.Patches
{
	internal class PathFindControllerPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(PathFindController), "isPositionImpassableForNPCSchedule", new Type[] { typeof(GameLocation), typeof(int), typeof(int), typeof(Character) }),
				postfix: new HarmonyMethod(typeof(PathFindControllerPatch), nameof(IsPositionImpassableForNPCSchedulePostfix))
			);
		}

		private static void IsPositionImpassableForNPCSchedulePostfix(GameLocation loc, int x, int y, Character npc, ref bool __result)
		{
			if (!__result && loc.isCollidingPosition(new Rectangle(x * Game1.tileSize + 1, y * Game1.tileSize + 1, 62, 62), Game1.viewport, npc is Farmer, 0, glider: false, npc, pathfinding: true))
			{
				__result = true;
			}
		}
	}
}
