using System;
using System.Reflection;
using HarmonyLib;
using xTile.Dimensions;
using StardewValley;
using StardewValley.Locations;

namespace TransportFramework.Patches
{
	internal class BoatPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(BoatTunnel), nameof(BoatTunnel.checkAction), new Type[] { typeof(Location), typeof(Rectangle), typeof(Farmer) }),
				prefix: new HarmonyMethod(typeof(BoatPatch), nameof(BoatTunnelCheckActionPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(IslandSouth), nameof(IslandSouth.performTouchAction), new Type[] { typeof(string[]), typeof(Microsoft.Xna.Framework.Vector2) }),
				prefix: new HarmonyMethod(typeof(BoatPatch), nameof(IslandSouthPerformTouchActionPrefix))
			);
		}

		private static bool BoatTunnelCheckActionPrefix(BoatTunnel __instance, Location tileLocation, Rectangle viewport, Farmer who, ref bool __result)
		{
			switch (__instance.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Action", "Buildings"))
			{
				case "BoatTicket":
					if (Game1.MasterPlayer.hasOrWillReceiveMail("willyBoatFixed"))
					{
						__result = ((Func<Location, Rectangle, Farmer, bool>)Activator.CreateInstance(typeof(Func<Location, Rectangle, Farmer, bool>), __instance, typeof(GameLocation).GetMethod(nameof(GameLocation.checkAction), BindingFlags.Public | BindingFlags.Instance).MethodHandle.GetFunctionPointer()))(tileLocation, viewport, who);
						return false;
					}
					break;
			}
			return true;
		}

		private static bool IslandSouthPerformTouchActionPrefix(string[] action)
		{
			if (ArgUtility.Get(action, 0) == "LeaveIsland")
			{
				return false;
			}
			return true;
		}
	}
}
