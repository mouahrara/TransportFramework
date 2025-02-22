using System;
using System.Data;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.TokenizableStrings;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class TokensUtility
	{
		private static readonly PerScreen<Station>	station = new(() => null);

		internal static Station Station
		{
			get => station.Value;
			set => station.Value = value;
		}

		public static string ParseText(Station station, string text, Random random = null, TokenParserDelegate customParser = null, Farmer player = null)
		{
			string result;

			Station = station;
			result = TokenParser.ParseText(text, random, customParser, player);
			Station = null;
			return result;
		}

		public static void Register()
		{
			// Localization
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_LocalizedStardewValley", LocalizedStardewValley);
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_I18n", I18n);

			// Event scripting
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_PlayerTileX", PlayerTileX);
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_PlayerTileY", PlayerTileY);
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_StationTemplateReferenceTileX", StationTemplateReferenceTileX);
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_StationTemplateReferenceTileY", StationTemplateReferenceTileY);
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_StationLocationWidth", StationLocationWidth);
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_StationLocationHeight", StationLocationHeight);
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_QueryExpression", QueryExpression);
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_CheckGameStateQuery", CheckGameStateQuery);
		}

		// Localization
		private static bool LocalizedStardewValley(string[] query, out string replacement, Random random, Farmer player)
		{
			replacement = ModEntry.Helper.Translation.Get("About_Title");
			return true;
		}

		private static bool I18n(string[] query, out string replacement, Random random, Farmer player)
		{
			if (Station is null)
			{
				return TokenParser.LogTokenError(query, "context station not defined", out replacement);
			}
			if (!ArgUtility.TryGet(query, 1, out string key, out string error))
			{
				return TokenParser.LogTokenError(query, error, out replacement);
			}
			replacement = Station.ContentPack.Translation.Get(key);
			return true;
		}

		// Event scripting
		private static bool PlayerTileX(string[] query, out string replacement, Random random, Farmer player)
		{
			replacement = player.Tile.X.ToString();
			return true;
		}

		private static bool PlayerTileY(string[] query, out string replacement, Random random, Farmer player)
		{
			replacement = player.Tile.Y.ToString();
			return true;
		}

		private static bool StationTemplateReferenceTileX(string[] query, out string replacement, Random random, Farmer player)
		{
			if (Station is null)
			{
				return TokenParser.LogTokenError(query, "context station not defined", out replacement);
			}
			replacement = Station.Template.ReferenceTile.X.ToString();
			return true;
		}

		private static bool StationTemplateReferenceTileY(string[] query, out string replacement, Random random, Farmer player)
		{
			if (Station is null)
			{
				return TokenParser.LogTokenError(query, "context station not defined", out replacement);
			}
			replacement = Station.Template.ReferenceTile.Y.ToString();
			return true;
		}

		private static bool StationLocationWidth(string[] query, out string replacement, Random random, Farmer player)
		{
			if (Station is null)
			{
				return TokenParser.LogTokenError(query, "context station not defined", out replacement);
			}

			GameLocation location = LocationUtility.GetLocationFromName(Station.Location);

			if (location is not null)
			{
				return TokenParser.LogTokenError(query, $"context location '{Station.Location}' not found", out replacement);
			}
			replacement = location.Map.Layers[0].LayerWidth.ToString();
			return true;
		}

		private static bool StationLocationHeight(string[] query, out string replacement, Random random, Farmer player)
		{
			if (Station is null)
			{
				return TokenParser.LogTokenError(query, "context station not defined", out replacement);
			}

			GameLocation location = LocationUtility.GetLocationFromName(Station.Location);

			if (location is not null)
			{
				return TokenParser.LogTokenError(query, $"context location '{Station.Location}' not found", out replacement);
			}
			replacement = location.Map.Layers[0].LayerHeight.ToString();
			return true;
		}

		private static bool QueryExpression(string[] query, out string replacement, Random random, Farmer player)
		{
			string queryExpression = string.Join(" ", query[1..]);

			try
			{
				DataTable dataTable = new();

				replacement = dataTable.Compute(queryExpression, null).ToString();
				return true;
			}
			catch
			{
				return TokenParser.LogTokenError(query, $"{queryExpression} is not a valid query expression", out replacement);
			}
		}

		private static bool CheckGameStateQuery(string[] query, out string replacement, Random random, Farmer player)
		{
			replacement = QueriesUtility.CheckConditions(Station, string.Join(" ", query[1..])).ToString();
			return true;
		}
	}
}
