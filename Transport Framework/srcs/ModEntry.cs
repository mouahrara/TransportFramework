using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewModdingAPI;
using TransportFramework.Api;
using TransportFramework.Classes;
using TransportFramework.Handlers;
using TransportFramework.Patches;
using TransportFramework.Utilities;

namespace TransportFramework
{
	/// <summary>The mod entry point.</summary>
	internal sealed class ModEntry : Mod
	{
		// Shared static helpers
		internal static new IModHelper	Helper		{ get; private set; }
		internal static new IMonitor	Monitor		{ get; private set; }
		internal static new IManifest	ModManifest	{ get; private set; }

		public static ModConfig					Config;
		public static List<Station>				Stations = new();
		public static IEnumerable<Station>		CurrentLocationStations = null;
		public static IEnumerable<Station>		OnDayStartStations = null;
		public static IEnumerable<Station>		OnInteractStations = null;
		public static IEnumerable<SSprite>		OnDayStartSprites = null;
		public static IEnumerable<SSprite>		OnLocationChangeSprites = null;
		public static IEnumerable<SSCondition>	OnDayStartSpriteConditions = null;
		public static IEnumerable<SSCondition>	OnLocationChangeSpriteConditions = null;
		public static IEnumerable<SCondition>	OnDayStartConditions = null;
		public static IEnumerable<SCondition>	OnInteractConditions = null;

		public override void Entry(IModHelper helper)
		{
			Helper = base.Helper;
			Monitor = base.Monitor;
			ModManifest = base.ModManifest;

			// Load Harmony patches
			try
			{
				Harmony harmony = new(ModManifest.UniqueID);

				// Apply patches
				Game1Patch.Apply(harmony);
				GameLocationPatch.Apply(harmony);
				BusPatch.Apply(harmony);
				BoatPatch.Apply(harmony);
				MinecartPatch.Apply(harmony);
				ParrotExpressPatch.Apply(harmony);
				EventPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				Monitor.Log($"Issue with Harmony patching: {e}", LogLevel.Error);
				return;
			}

			// Subscribe to events
			Helper.Events.GameLoop.GameLaunched += GameLaunchedHandler.Apply;
			Helper.Events.GameLoop.SaveLoaded += SaveLoadedHandler.Apply;
			Helper.Events.GameLoop.DayStarted += DayStartedHandler.Apply;
			Helper.Events.GameLoop.UpdateTicked += UpdateTickedHandler.Apply;
			Helper.Events.GameLoop.ReturnedToTitle += ReturnedToTitleHandler.Apply;
			Helper.Events.Content.AssetRequested += AssetRequestedHandler.Apply;
		}

		public override object GetApi()
		{
			return new TransportFrameworkApi();
		}
	}
}
