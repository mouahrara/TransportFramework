using StardewModdingAPI.Events;
using TransportFramework.Utilities;

namespace TransportFramework.Handlers
{
	internal static class GameLaunchedHandler
	{
		/// <inheritdoc cref="IGameLoopEvents.GameLaunched"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, GameLaunchedEventArgs e)
		{
			// Register console commands
			ConsoleCommandsUtility.Register();

			// Register tokens
			TokensUtility.Register();

			// Register queries
			QueriesUtility.Register();

			// Register event commands
			EventCommandsUtility.Register();

			// Initialize GMCM
			GMCMUtility.Initialize();

			// Subscribe to the event
			ModEntry.Helper.Events.GameLoop.UpdateTicked += ApplyAfterOneTick;
		}

		internal static void ApplyAfterOneTick(object sender, UpdateTickedEventArgs e)
		{
			// Unsubscribe from the event
			ModEntry.Helper.Events.GameLoop.UpdateTicked -= ApplyAfterOneTick;

			// Subscribe to the event
			ModEntry.Helper.Events.GameLoop.UpdateTicked += ApplyAfterTwoTicks;
		}

		internal static void ApplyAfterTwoTicks(object sender, UpdateTickedEventArgs e)
		{
			// Initialize station list with base game stations
			StationsUtility.Initialize();

			// Initialize content packs
			ContentPacksUtility.Initialize();

			// Set station references in conditions and events
			StationsUtility.SetStationReferences();

			// Remove invalid stations
			StationsUtility.RemoveInvalidStationsGameLaunched();

			// Compute properties
			StationsUtility.ComputeProperties();

			// Unsubscribe from the event
			ModEntry.Helper.Events.GameLoop.UpdateTicked -= ApplyAfterTwoTicks;
		}
	}
}
