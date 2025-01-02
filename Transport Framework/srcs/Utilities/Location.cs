using StardewValley;

namespace TransportFramework.Utilities
{
	internal class LocationUtility
	{
		public static GameLocation GetLocationFromName(string locationName)
		{
			return GetActivePassiveFestivalLocationRequest(locationName)?.Location ?? Game1.getLocationFromName(locationName);
		}

		public static LocationRequest GetLocationRequest(string locationName, bool isStructure = false)
		{
			return GetActivePassiveFestivalLocationRequest(locationName, isStructure) ?? Game1.getLocationRequest(locationName, isStructure);
		}

		private static LocationRequest GetActivePassiveFestivalLocationRequest(string locationName, bool isStructure = false)
		{
			foreach (string activePassiveFestival in Game1.netWorldState.Value.ActivePassiveFestivals)
			{
				if (Utility.TryGetPassiveFestivalData(activePassiveFestival, out var data) && Game1.dayOfMonth >= data.StartDay && Game1.dayOfMonth <= data.EndDay && data.Season == Game1.season && data.MapReplacements != null && data.MapReplacements.TryGetValue(locationName, out var value))
				{
					return Game1.getLocationRequest(value, isStructure);
				}
			}
			return null;
		}
	}
}
