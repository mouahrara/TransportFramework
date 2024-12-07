using System;
using System.Reflection;
using HarmonyLib;
using xTile.Dimensions;
using xTile.Tiles;
using StardewValley;
using StardewValley.Extensions;
using StardewValley.Locations;

namespace TransportFramework.Patches
{
	internal class MinecartPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(BusStop), nameof(BusStop.checkAction), new Type[] { typeof(Location), typeof(Rectangle), typeof(Farmer) }),
				prefix: new HarmonyMethod(typeof(MinecartPatch), nameof(BusStopCheckActionPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Mountain), nameof(Mountain.checkAction), new Type[] { typeof(Location), typeof(Rectangle), typeof(Farmer) }),
				prefix: new HarmonyMethod(typeof(MinecartPatch), nameof(QuarryCheckActionPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Town), nameof(Town.checkAction), new Type[] { typeof(Location), typeof(Rectangle), typeof(Farmer) }),
				prefix: new HarmonyMethod(typeof(MinecartPatch), nameof(TownCheckActionPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.checkAction), new Type[] { typeof(Location), typeof(Rectangle), typeof(Farmer) }),
				prefix: new HarmonyMethod(typeof(MinecartPatch), nameof(DefaultCheckActionPrefix))
			);
		}

		private static bool BusStopCheckActionPrefix(BusStop __instance, Location tileLocation, Rectangle viewport, Farmer who, ref bool __result)
		{
			switch (__instance.getTileIndexAt(tileLocation, "Buildings", "outdoors"))
			{
				case 958:
				case 1080:
				case 1081:
					__result = ((Func<Location, Rectangle, Farmer, bool>)Activator.CreateInstance(typeof(Func<Location, Rectangle, Farmer, bool>), __instance, typeof(GameLocation).GetMethod(nameof(GameLocation.checkAction), BindingFlags.Public | BindingFlags.Instance).MethodHandle.GetFunctionPointer()))(tileLocation, viewport, who);
					return false;
				default:
					break;
			}
			return true;
		}

		private static bool QuarryCheckActionPrefix(Mountain __instance, Location tileLocation, Rectangle viewport, Farmer who, ref bool __result)
		{
			switch (__instance.getTileIndexAt(tileLocation, "Buildings", "outdoors"))
			{
				case 958:
				case 1080:
				case 1081:
					__result = ((Func<Location, Rectangle, Farmer, bool>)Activator.CreateInstance(typeof(Func<Location, Rectangle, Farmer, bool>), __instance, typeof(GameLocation).GetMethod(nameof(GameLocation.checkAction), BindingFlags.Public | BindingFlags.Instance).MethodHandle.GetFunctionPointer()))(tileLocation, viewport, who);
					return false;
				default:
					break;
			}
			return true;
		}

		private static bool TownCheckActionPrefix(Town __instance, Location tileLocation, Rectangle viewport, Farmer who, ref bool __result)
		{
			switch (__instance.getTileIndexAt(tileLocation, "Buildings", "Landscape"))
			{
				case 958:
				case 1080:
				case 1081:
					if (!Game1.isFestival())
					{
						__result = ((Func<Location, Rectangle, Farmer, bool>)Activator.CreateInstance(typeof(Func<Location, Rectangle, Farmer, bool>), __instance, typeof(GameLocation).GetMethod(nameof(GameLocation.checkAction), BindingFlags.Public | BindingFlags.Instance).MethodHandle.GetFunctionPointer()))(tileLocation, viewport, who);
						return false;
					}
					break;
				default:
					break;
			}
			return true;
		}

		private static bool DefaultCheckActionPrefix(GameLocation __instance, Location tileLocation, Rectangle viewport)
		{
			Tile tile = __instance.map.RequireLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);

			if (tile is null || !tile.Properties.TryGetValue("Action", out string fullActionString))
			{
				fullActionString = __instance.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Action", "Buildings");
			}
			if (fullActionString is not null)
			{
				string[] action = ArgUtility.SplitBySpace(fullActionString);

				if (ArgUtility.Get(action, 0) == "MinecartTransport")
				{
					return false;
				}
			}
			return true;
		}
	}
}
