using System;
using System.Reflection;
using HarmonyLib;
using Microsoft.Xna.Framework;
using xTile.Dimensions;
using StardewValley;
using StardewValley.Locations;
using Rectangle = xTile.Dimensions.Rectangle;

namespace TransportFramework.Patches
{
	internal class BusPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(BusStop), nameof(BusStop.checkAction), new Type[] { typeof(Location), typeof(Rectangle), typeof(Farmer) }),
				prefix: new HarmonyMethod(typeof(BusPatch), nameof(BusStopCheckActionPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Desert), nameof(Desert.performTouchAction), new Type[] { typeof(string[]), typeof(Vector2) }),
				prefix: new HarmonyMethod(typeof(BusPatch), nameof(DesertPerformTouchActionPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(BusStop), "resetLocalState"),
				prefix: new HarmonyMethod(typeof(BusPatch), nameof(ResetLocalStatePrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(BusStop), "resetLocalState"),
				postfix: new HarmonyMethod(typeof(BusPatch), nameof(ResetLocalStatePostfixBusStop))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Desert), "resetLocalState"),
				prefix: new HarmonyMethod(typeof(BusPatch), nameof(ResetLocalStatePrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Desert), "resetLocalState"),
				postfix: new HarmonyMethod(typeof(BusPatch), nameof(ResetLocalStatePostfixDesert))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(DesertFestival), "resetLocalState"),
				prefix: new HarmonyMethod(typeof(BusPatch), nameof(ResetLocalStatePrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(DesertFestival), "resetLocalState"),
				postfix: new HarmonyMethod(typeof(BusPatch), nameof(ResetLocalStatePostfixDesert))
			);
		}

		private static bool BusStopCheckActionPrefix(BusStop __instance, Location tileLocation, Rectangle viewport, Farmer who, ref bool __result)
		{
			switch (__instance.getTileIndexAt(tileLocation, "Buildings"))
			{
				case 1057:
				{
					__result = ((Func<Location, Rectangle, Farmer, bool>)Activator.CreateInstance(typeof(Func<Location, Rectangle, Farmer, bool>), __instance, typeof(GameLocation).GetMethod(nameof(GameLocation.checkAction), BindingFlags.Public | BindingFlags.Instance).MethodHandle.GetFunctionPointer()))(tileLocation, viewport, who);
					return false;
				}
				default:
					break;
			}
			return true;
		}

		private static bool DesertPerformTouchActionPrefix(string[] action)
		{
			if (ArgUtility.Get(action, 0) == "DesertBus")
			{
				return false;
			}
			return true;
		}

		private static void ResetLocalStatePrefix(GameLocation __instance, out string __state)
		{
			__state = Game1.player.previousLocationName;
			Game1.player.previousLocationName = __instance.Name;
		}

		private static void ResetLocalStatePostfixBusStop(GameLocation __instance, string __state)
		{
			FieldInfo fieldInfoBusPosition = typeof(BusStop).GetField("busPosition", BindingFlags.Public | BindingFlags.Instance);
			FieldInfo fieldInfoBusDoor = typeof(BusStop).GetField("busDoor", BindingFlags.NonPublic | BindingFlags.Instance);

			ResetLocalStatePostfix(__instance, __state, fieldInfoBusPosition, fieldInfoBusDoor);
		}

		private static void ResetLocalStatePostfixDesert(GameLocation __instance, string __state)
		{
			FieldInfo fieldInfoBusPosition = typeof(Desert).GetField("busPosition", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo fieldInfoBusDoor = typeof(Desert).GetField("busDoor", BindingFlags.NonPublic | BindingFlags.Instance);

			ResetLocalStatePostfix(__instance, __state, fieldInfoBusPosition, fieldInfoBusDoor);
		}

		private static void ResetLocalStatePostfix(GameLocation __instance, string __state, FieldInfo fieldInfoBusPosition, FieldInfo fieldInfoBusDoor)
		{
			if (fieldInfoBusPosition is not null && fieldInfoBusDoor is not null)
			{
				fieldInfoBusPosition.SetValue(__instance, new Vector2(float.MinValue, float.MinValue));
				fieldInfoBusDoor.SetValue(__instance, new TemporaryAnimatedSprite());
			}
			Game1.player.previousLocationName = __state;
		}
	}
}
