using StardewValley;
using TransportFramework.Frameworks;

namespace TransportFramework.Utilities
{
	public sealed class ModConfig
	{
		internal static bool	AllowIndoorHorsebackTravel = ModEntry.Helper.ModRegistry.IsLoaded("Pathoschild.HorseFluteAnywhere");

		public bool		AllowHorsebackTravel = true;
		public bool		SkipTravelAnimations = false;
		public string	PreviousPageButtonPosition = "Top";
		public string	NextPageButtonPosition = "Bottom";
		public int		NumberOfDestinationsPerPage = 4;
		public bool		AllowObjectDestruction = true;
	}

	internal class GMCMUtility
	{
		internal static void Initialize()
		{
			ReadConfig();
			Register();
		}

		private static void ReadConfig()
		{
			ModEntry.Config = ModEntry.Helper.ReadConfig<ModConfig>();
		}

		private static void Register()
		{
			// Get Generic Mod Config Menu's API
			IGenericModConfigMenuApi gmcm = ModEntry.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

			if (gmcm is not null)
			{
				// Register mod
				gmcm.Register(
					mod: ModEntry.ModManifest,
					reset: () => ModEntry.Config = new ModConfig(),
					save: () => ModEntry.Helper.WriteConfig(ModEntry.Config)
				);

				// Main section
				gmcm.AddBoolOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.AllowHorsebackTravel.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.AllowHorsebackTravel.Tooltip", new { HorseFlute = Game1.content.LoadString("Strings\\StringsFromCSFiles:HorseFlute").ToLower() }),
					getValue: () => ModEntry.Config.AllowHorsebackTravel,
					setValue: (value) => ModEntry.Config.AllowHorsebackTravel = value
				);
				gmcm.AddBoolOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.SkipTravelAnimations.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.SkipTravelAnimations.Tooltip"),
					getValue: () => ModEntry.Config.SkipTravelAnimations,
					setValue: (value) => ModEntry.Config.SkipTravelAnimations = value
				);
				gmcm.AddTextOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.PreviousPageButtonPosition.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.PreviousPageButtonPosition.Tooltip"),
					getValue: () => ModEntry.Config.PreviousPageButtonPosition,
					setValue: (value) => ModEntry.Config.PreviousPageButtonPosition = value,
					allowedValues: new string[] { "Top", "Bottom" },
					formatAllowedValue: (string value) => ModEntry.Helper.Translation.Get("GMCM.PagePosition." + value)
				);
				gmcm.AddTextOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.NextPageButtonPosition.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.NextPageButtonPosition.Tooltip"),
					getValue: () => ModEntry.Config.NextPageButtonPosition,
					setValue: (value) => ModEntry.Config.NextPageButtonPosition = value,
					allowedValues: new string[] { "Top", "Bottom" },
					formatAllowedValue: (string value) => ModEntry.Helper.Translation.Get("GMCM.PagePosition." + value)
				);
				gmcm.AddNumberOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.NumberOfDestinationsPerPage.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.NumberOfDestinationsPerPage.Tooltip"),
					getValue: () => ModEntry.Config.NumberOfDestinationsPerPage,
					setValue: (value) => ModEntry.Config.NumberOfDestinationsPerPage = value,
					min: 4,
					max: 20
				);
				gmcm.AddBoolOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.AllowObjectDestruction.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.AllowObjectDestruction.Tooltip"),
					getValue: () => ModEntry.Config.AllowObjectDestruction,
					setValue: (value) => ModEntry.Config.AllowObjectDestruction = value
				);
			}
		}
	}
}
