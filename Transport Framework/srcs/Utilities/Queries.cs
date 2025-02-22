using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Delegates;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class QueriesUtility
	{
		private static readonly PerScreen<Station>		station = new(() => null);
		private static readonly PerScreen<SCondition>	scondition = new(() => null);

		internal static Station Station
		{
			get => station.Value;
			set => station.Value = value;
		}

		internal static SCondition SCondition
		{
			get => scondition.Value;
			set => scondition.Value = value;
		}

		public static bool CheckConditions(Station station, string queryString, GameLocation location = null, Farmer player = null, Item targetItem = null, Item inputItem = null, Random random = null, HashSet<string> ignoreQueryKeys = null)
		{
			bool result;

			Station = station;
			result = GameStateQuery.CheckConditions(queryString, location, player , targetItem, inputItem, random, ignoreQueryKeys);
			Station = null;
			return result;
		}

		public static bool CheckConditions(SCondition condition, string queryString, GameLocation location = null, Farmer player = null, Item targetItem = null, Item inputItem = null, Random random = null, HashSet<string> ignoreQueryKeys = null)
		{
			bool result;

			SCondition = condition;
			result = CheckConditions(condition.Station, queryString, location, player , targetItem, inputItem, random, ignoreQueryKeys);
			SCondition = null;
			return result;
		}

		public static bool CheckConditions(SSCondition condition, string queryString, GameLocation location = null, Farmer player = null, Item targetItem = null, Item inputItem = null, Random random = null, HashSet<string> ignoreQueryKeys = null)
		{
			return CheckConditions(new SCondition()
			{
				Station = condition.Sprite.Station,
				Query = condition.Query,
				LockedMessage = null,
				Update = condition.Update
			}, queryString, location, player , targetItem, inputItem, random, ignoreQueryKeys);
		}

		public static void Register()
		{
			GameStateQuery.Register($"{ModEntry.ModManifest.UniqueID}_CanDriveYourselfToday", CanDriveYourselfToday);
			GameStateQuery.Register($"{ModEntry.ModManifest.UniqueID}_IsCharacterAtTileForDeparture", IsCharacterAtTileForDeparture);
			GameStateQuery.Register($"{ModEntry.ModManifest.UniqueID}_IsCharacterAtTileForArrival", IsCharacterAtTileForArrival);
			GameStateQuery.Register($"{ModEntry.ModManifest.UniqueID}_IsCharacterAtTile", IsCharacterAtTile);
		}

		private static bool CanDriveYourselfToday(string[] query, GameStateQueryContext context)
		{
			return Game1.netWorldState.Value.canDriveYourselfToday.Value;
		}

		private static bool IsCharacterAtTileForDeparture(string[] query, GameStateQueryContext context)
		{
			return IsCharacterAtTile(query, context) || Game1.currentLocation != LocationUtility.GetLocationFromName(SCondition.Station.Location);
		}

		private static bool IsCharacterAtTileForArrival(string[] query, GameStateQueryContext context)
		{
			return IsCharacterAtTile(query, context) || Game1.currentLocation == LocationUtility.GetLocationFromName(SCondition.Station.Location);
		}

		private static bool IsCharacterAtTile(string[] query, GameStateQueryContext context)
		{
			if (Station is null)
			{
				return GameStateQuery.Helpers.ErrorResult(query, "context station not defined");
			}
			if (!ArgUtility.TryGet(query, 1, out string npc, out string error) || !ArgUtility.TryGetVector2(query, 2, out Vector2 tile, out error, integerOnly: true))
			{
				return RemoveCondition(query, error);
			}

			NPC character = Game1.getCharacterFromName(npc);

			if (character is null)
			{
				return RemoveCondition(query, $"{npc} not found");
			}

			GameLocation stationLocation = LocationUtility.GetLocationFromName(Station.Location);

			if (stationLocation is null)
			{
				return RemoveCondition(query, $"{Station.Location} not found");
			}
			return stationLocation.characters.Contains(character) && character.TilePoint.X == (int)tile.X && character.TilePoint.Y == (int)tile.Y;
		}

		private static bool RemoveCondition(string[] query, string error)
		{
			if (SCondition is not null)
			{
				SCondition.Query = string.Empty;
				return GameStateQuery.Helpers.ErrorResult(query, $"{error}. The condition will be ignored");
			}
			return GameStateQuery.Helpers.ErrorResult(query, error);
		}
	}
}
