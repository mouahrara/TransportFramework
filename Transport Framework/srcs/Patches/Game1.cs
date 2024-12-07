using System;
using HarmonyLib;
using StardewValley;
using TransportFramework.Utilities;

namespace TransportFramework.Patches
{
	internal class Game1Patch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(Game1), nameof(Game1.setRichPresence), new Type[] { typeof(string), typeof(object) }),
				postfix: new HarmonyMethod(typeof(Game1Patch), nameof(SetRichPresencePostfix))
			);
		}

		private static void SetRichPresencePostfix(string friendlyName)
		{
			if (friendlyName != "location")
				return;

			// Generate current location enumerable
			StationsUtility.GenerateCurrentLocationEnumerable();

			// Update stations, sprites and conditions OnLocationChange
			StationsUtility.UpdateOnLocationChange();
		}
	}
}
