using System;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.TokenizableStrings;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class TokensUtility
	{
		internal static readonly PerScreen<Station>	station = new(() => null);

		private static Station Station
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
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_LocalizedStardewValley", LocalizedStardewValley);
			TokenParser.RegisterParser($"{ModEntry.ModManifest.UniqueID}_I18n", I18n);
		}

		private static bool LocalizedStardewValley(string[] query, out string replacement, Random random, Farmer player)
		{
			replacement = ModEntry.Helper.Translation.Get("About_Title");
			return true;
		}

		private static bool I18n(string[] query, out string replacement, Random random, Farmer player)
		{
			if (station is null)
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
	}
}
