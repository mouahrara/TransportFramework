using StardewModdingAPI;
using StardewModdingAPI.Events;
using TransportFramework.Utilities;

namespace TransportFramework.Handlers
{
	internal static class DayStartedHandler
	{
		/// <inheritdoc cref="IGameLoopEvents.DayStarted"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, DayStartedEventArgs e)
		{
			if (!Context.IsWorldReady)
				return;

			// Update stations, sprites and conditions OnDayStart
			StationsUtility.UpdateOnDayStart();
		}
	}
}
