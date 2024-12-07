using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using TransportFramework.Classes;
using TransportFramework.Utilities;

namespace TransportFramework.Patches
{
	internal class ParrotExpressPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(ParrotPlatform), nameof(ParrotPlatform.Activate)),
				prefix: new HarmonyMethod(typeof(ParrotExpressPatch), nameof(ParrotPlatformActivatePrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(ParrotUpgradePerch), nameof(ParrotUpgradePerch.ApplyUpgrade)),
				postfix: new HarmonyMethod(typeof(ParrotExpressPatch), nameof(ParrotUpgradePerchApplyUpgradePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(IslandNorth), "resetLocalState"),
				postfix: new HarmonyMethod(typeof(ParrotExpressPatch), nameof(ResetLocalStatePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(IslandWest), "resetLocalState"),
				postfix: new HarmonyMethod(typeof(ParrotExpressPatch), nameof(ResetLocalStatePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(IslandEast), "resetLocalState"),
				postfix: new HarmonyMethod(typeof(ParrotExpressPatch), nameof(ResetLocalStatePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(IslandSouth), "resetLocalState"),
				postfix: new HarmonyMethod(typeof(ParrotExpressPatch), nameof(ResetLocalStatePostfix))
			);
		}

		private static bool ParrotPlatformActivatePrefix()
		{
			return false;
		}

		private static void ParrotUpgradePerchApplyUpgradePostfix(ParrotUpgradePerch __instance)
		{
			if (__instance.locationRef.Value is IslandWest && __instance.tilePosition.Value == new Point(72, 10))
			{
				if (ModEntry.CurrentLocationStations is not null)
				{
					foreach (Station station in ModEntry.CurrentLocationStations)
					{
						if (station.Network.Equals("ParrotExpress"))
						{
							StationsUtility.UpdateSpriteConditions(ModEntry.OnLocationChangeSpriteConditions, station);
							StationsUtility.UpdateSprites(ModEntry.OnLocationChangeSprites, station);
						}
					}
				}
				typeof(GameLocation).GetMethod("resetLocalState", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Game1.currentLocation, null);
			}
		}

		private static void ResetLocalStatePostfix(IslandLocation __instance)
		{
			if (__instance.parrotPlatforms is not null)
			{
				foreach (ParrotPlatform parrotPlatform in __instance.parrotPlatforms)
				{
					parrotPlatform.position = new Vector2(float.MinValue, float.MinValue);
				}
			}
		}
	}
}
