using StardewModdingAPI.Events;
using TransportFramework.Utilities;

namespace TransportFramework.Handlers
{
	internal static class ReturnedToTitleHandler
	{
		/// <inheritdoc cref="IGameLoopEvents.ReturnedToTitle"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, ReturnedToTitleEventArgs e)
		{
			// Clear enumerables
			StationsUtility.ClearEnumerables();
		}
	}
}
