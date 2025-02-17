using StardewModdingAPI.Events;
using TransportFramework.Utilities;

namespace TransportFramework.Handlers
{
	internal static class SaveLoadedHandler
	{
		/// <inheritdoc cref="IGameLoopEvents.SaveLoaded"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, SaveLoadedEventArgs e)
		{
			// Remove invalid stations
			StationsUtility.RemoveInvalidStationsSaveLoaded();

			// Localize translatable station fields
			StationsUtility.Localize();

			// Generate update enumerables
			StationsUtility.GenerateUpdateEnumerables();

			// Update stations, sprites and conditions OnSaveLoad
			StationsUtility.UpdateOnSaveLoaded();
		}
	}
}
