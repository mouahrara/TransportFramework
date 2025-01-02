using System.Linq;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.Tiles;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace TransportFramework.Utilities
{
	internal class MapsUtility
	{
		public static void EditBusMaps(AssetRequestedEventArgs e)
		{
			if (e.Name.IsEquivalentTo("Maps/BusStop"))
			{
				e.Edit(asset =>
				{
					IAssetDataForMap editor = asset.AsMap();

					MoveBusTilesFromBuildingsLayerToBackLayer(editor.Data, 21, 7, "outdoors", new int[] { 1054, 1056, 1082 });
				});
			}
			else if (e.Name.IsEquivalentTo("Maps/Desert") || e.Name.IsEquivalentTo("Maps/Desert-Festival"))
			{
				e.Edit(asset =>
				{
					IAssetDataForMap editor = asset.AsMap();

					MoveBusTilesFromBuildingsLayerToBackLayer(editor.Data, 17, 25, "desert-new", new int[] { 174, 190, 206 });
				});
			}
		}

		private static void	MoveBusTilesFromBuildingsLayerToBackLayer(Map map, int x, int y, string tileSheetId, int[] tileIndexes)
		{
			Layer buildingsLayer = map.GetLayer("Buildings");
			Layer backLayer = map.GetLayer("Back");

			if (buildingsLayer is not null && backLayer is not null)
			{
				for (int i = x; i < x + 8; i++)
				{
					for (int j = y; j < y + 3; j++)
					{
						Location position = new(i, j);
						Tile buildingsTile = buildingsLayer.Tiles[position];
						Tile backTile = backLayer.Tiles[position];

						if (buildingsTile is not null)
						{
							if (buildingsTile.TileSheet.Id.Equals(tileSheetId) && tileIndexes.Any(tileIndex => buildingsTile.TileIndex == tileIndex))
							{
								backLayer.Tiles[position] = buildingsLayer.Tiles[position];
								buildingsLayer.Tiles[position] = null;
							}
						}
					}
				}
			}
		}

		public static void AddMinecartTransportActions(AssetRequestedEventArgs e)
		{
			if (e.Name.IsEquivalentTo("Maps/Town"))
			{
				e.Edit(asset =>
				{
					IAssetDataForMap editor = asset.AsMap();

					AddMinecartTransportActionAtTile(editor.Data, 105, 79, "Default", "Town");
					AddMinecartTransportActionAtTile(editor.Data, 106, 79, "Default", "Town");
					AddMinecartTransportActionAtTile(editor.Data, 107, 79, "Default", "Town");
				});
			}
			else if (e.Name.IsEquivalentTo("Maps/BusStop"))
			{
				e.Edit(asset =>
				{
					IAssetDataForMap editor = asset.AsMap();

					AddMinecartTransportActionAtTile(editor.Data, 14, 3, "Default", "Bus");
					AddMinecartTransportActionAtTile(editor.Data, 15, 3, "Default", "Bus");
					AddMinecartTransportActionAtTile(editor.Data, 16, 3, "Default", "Bus");
				});
			}
			else if (e.Name.IsEquivalentTo("Maps/Mountain"))
			{
				e.Edit(asset =>
				{
					IAssetDataForMap editor = asset.AsMap();

					AddMinecartTransportActionAtTile(editor.Data, 124, 11, "Default", "Quarry");
					AddMinecartTransportActionAtTile(editor.Data, 125, 11, "Default", "Quarry");
					AddMinecartTransportActionAtTile(editor.Data, 126, 11, "Default", "Quarry");
				});
			}
		}

		private static void	AddMinecartTransportActionAtTile(Map map, int x, int y, string networkId, string excludeDestinationId)
		{
			Layer buildingsLayer = map.GetLayer("Buildings");

			if (buildingsLayer is not null && buildingsLayer.Tiles[x, y] is not null && !buildingsLayer.Tiles[x, y].Properties.ContainsKey("Action"))
			{
				buildingsLayer.Tiles[x, y].Properties.Add("Action", $"MinecartTransport {networkId} {excludeDestinationId}");
			}
		}
	}
}
