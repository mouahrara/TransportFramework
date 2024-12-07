using StardewModdingAPI;
using StardewModdingAPI.Events;
using TransportFramework.Utilities;

namespace TransportFramework.Handlers
{
	internal static class UpdateTickedHandler
	{
		/// <inheritdoc cref="IGameLoopEvents.UpdateTicked"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, UpdateTickedEventArgs e)
		{
			if (!Context.IsWorldReady)
				return;

			// Update mouse cursor
			MouseCursorUtility.Update();

			if (!Context.CanPlayerMove)
				return;

			// Handle touchActions
			TouchActionsUtility.Handle();
		}
	}
}
