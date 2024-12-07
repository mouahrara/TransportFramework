using StardewModdingAPI.Events;
using TransportFramework.Utilities;

namespace TransportFramework.Handlers
{
	internal static class AssetRequestedHandler
	{
		/// <inheritdoc cref="IContentEvents.AssetRequested"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, AssetRequestedEventArgs e)
		{
			// Edit bus maps
			MapsUtility.EditBusMaps(e);

			// Add MinecartTransport actions
			MapsUtility.AddMinecartTransportActions(e);
		}
	}
}
