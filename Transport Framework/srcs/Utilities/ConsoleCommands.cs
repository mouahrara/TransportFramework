using System.Collections.Generic;
using System.Linq;
using System.Text;
using StardewModdingAPI;
using StardewValley;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class ConsoleCommandsUtility
	{
		internal static void Register()
		{
			// Stations
			ModEntry.Helper.ConsoleCommands.Add("tf_summary", "This command lists stations.\n\nUsage: tf_summary [manifestsUniqueId = \"all\"] [location = \"all\"] [network = \"all\"]\nLists stations that match the provided command arguments. The \"all\" keyword matches all stations.\n- manifestsUniqueId: The unique ID of a mod's manifest.\n- location: The name of a location.\n- network: The name of a network.", TF_summary);
			ModEntry.Helper.ConsoleCommands.Add("tf_show", "This command shows detailed information about a station.\n\nUsage: tf_show [stationId = \"all\"]\nShows the detailed information about the station that matches the provided command argument. The \"all\" keyword shows detailed information about all stations.\n- stationId : The station identifier.", TF_show);
			ModEntry.Helper.ConsoleCommands.Add("tf_update", "This command updates a station and displays whether its conditions are satisfied.\n\nUsage: tf_update [stationId = \"all\"]\nUpdates the station that matches the provided command argument. The \"all\" keyword updates all stations.\n- stationId: The station identifier.", TF_update);

			// Templates
			ModEntry.Helper.ConsoleCommands.Add("tf_template_summary", "This command lists templates.\n\nUsage: tf_template_summary [manifestsUniqueId = \"all\"]\nLists templates that match the provided command arguments. The \"all\" keyword matches all templates.\n- manifestsUniqueId: The unique ID of a mod's manifest.", TF_template_summary);
			ModEntry.Helper.ConsoleCommands.Add("tf_template_show", "This command shows detailed information about a template.\n\nUsage: tf_template_show [templateId = \"all\"]\nShows the detailed information about the template that matches the provided command argument. The \"all\" keyword shows detailed information about all templates.\n- templateId : The template identifier.", TF_template_show);
		}

		// Stations
		private static void TF_summary(string command, string[] args)
		{
			if (!ArgUtility.TryGetOptional(args, 0, out string manifestsUniqueId, out string error, "all") || !ArgUtility.TryGetOptional(args, 1, out string location, out error, "all") || !ArgUtility.TryGetOptional(args, 2, out string network, out error, "all"))
			{
				ModEntry.Monitor.Log(error, LogLevel.Error);
				return;
			}

			StringBuilder summary = new();
			IEnumerable<Station> filteredStations = ModEntry.Stations.Where(s => (manifestsUniqueId.ToLower().Equals("all") || s.ContentPack.Manifest.UniqueID.Equals(manifestsUniqueId)) && (location.ToLower().Equals("all") || s.Location.Equals(location)) && (network.ToLower().Equals("all") || s.Network.Equals(network)));
			int count = filteredStations.Count();

			if (count == 0)
			{
				summary.Append("No station found.\n");
			}
			else
			{
				string lastManifestUniqueId = null;

				summary.Append($"List of {count} stations found:\n");
				foreach (Station station in filteredStations)
				{
					if (!station.ContentPack.Manifest.UniqueID.Equals(lastManifestUniqueId))
					{
						summary.Append($"\n{station.ContentPack.Manifest.UniqueID}\n");
						lastManifestUniqueId = station.ContentPack.Manifest.UniqueID;
					}
					summary.Append($"- {station.Id}\n");
				}
			}
			ModEntry.Monitor.Log(summary.ToString(), LogLevel.Info);
		}

		private static void TF_show(string command, string[] args)
		{
			if (!ArgUtility.TryGetOptional(args, 0, out string stationId, out string error, "all"))
			{
				ModEntry.Monitor.Log(error, LogLevel.Error);
				return;
			}

			StringBuilder summary = new();
			IEnumerable<Station> filteredStations = !stationId.ToLower().Equals("all") ? ModEntry.Stations.Where(s => s.Id.Equals(stationId)) : ModEntry.Stations;
			int count = filteredStations.Count();

			if (count == 0)
			{
				summary.Append("No station found.\n");
			}
			else
			{
				summary.Append($"List of {count} stations found:\n");
				foreach (Station station in filteredStations)
				{
					StringBuilderUtility.AppendStation(summary, station.ContentPack.Manifest.UniqueID, station);
				}
			}
			ModEntry.Monitor.Log(summary.ToString(), LogLevel.Info);
		}

		private static void TF_update(string command, string[] args)
		{
			if (!Context.IsWorldReady)
			{
				ModEntry.Monitor.Log("You must load a save to use this command.", LogLevel.Error);
				return;
			}
			if (!ArgUtility.TryGetOptional(args, 0, out string stationId, out string error, "all"))
			{
				ModEntry.Monitor.Log(error, LogLevel.Error);
				return;
			}

			StringBuilder summary = new();
			string lastManifestUniqueId = null;
			IEnumerable<Station> filteredByStationId = !stationId.ToLower().Equals("all") ? ModEntry.Stations.Where(s => s.Id.Equals(stationId)) : ModEntry.Stations;
			int count = filteredByStationId.Count();

			if (count == 0)
			{
				summary.Append("No station found.\n");
			}
			else
			{
				summary.Append($"List of {count} stations found:\n");
				foreach (Station station in filteredByStationId)
				{
					if (!station.ContentPack.Manifest.UniqueID.Equals(lastManifestUniqueId))
					{
						summary.Append($"\n{station.ContentPack.Manifest.UniqueID}\n");
						lastManifestUniqueId = station.ContentPack.Manifest.UniqueID;
					}
					StationsUtility.UpdateOnSaveLoad(station);
					summary.Append($"- {station.Id}: {station.ConditionsCache}\n");
				}
			}
			ModEntry.Monitor.Log(summary.ToString(), LogLevel.Info);
		}

		// Templates
		private static void TF_template_summary(string command, string[] args)
		{
			if (!ArgUtility.TryGetOptional(args, 0, out string manifestsUniqueId, out string error, "all"))
			{
				ModEntry.Monitor.Log(error, LogLevel.Error);
				return;
			}

			StringBuilder summary = new();
			IEnumerable<Template> filteredTemplates = ModEntry.Templates.Where(t => manifestsUniqueId.ToLower().Equals("all") || t.ContentPack.Manifest.UniqueID.Equals(manifestsUniqueId));
			int count = filteredTemplates.Count();

			if (count == 0)
			{
				summary.Append("No template found.\n");
			}
			else
			{
				string lastManifestUniqueId = null;

				summary.Append($"List of {count} templates found:\n");
				foreach (Template template in filteredTemplates)
				{
					if (!template.ContentPack.Manifest.UniqueID.Equals(lastManifestUniqueId))
					{
						summary.Append($"\n{template.ContentPack.Manifest.UniqueID}\n");
						lastManifestUniqueId = template.ContentPack.Manifest.UniqueID;
					}
					summary.Append($"- {template.Id}\n");
				}
			}
			ModEntry.Monitor.Log(summary.ToString(), LogLevel.Info);
		}

		private static void TF_template_show(string command, string[] args)
		{
			if (!ArgUtility.TryGetOptional(args, 0, out string templateId, out string error, "all"))
			{
				ModEntry.Monitor.Log(error, LogLevel.Error);
				return;
			}

			StringBuilder summary = new();
			IEnumerable<Template> filteredTemplates = !templateId.ToLower().Equals("all") ? ModEntry.Templates.Where(t => t.Id.Equals(templateId)) : ModEntry.Templates;
			int count = filteredTemplates.Count();

			if (count == 0)
			{
				summary.Append("No station found.\n");
			}
			else
			{
				summary.Append($"List of {count} stations found:\n");
				foreach (Template template in filteredTemplates)
				{
					StringBuilderUtility.AppendTemplate(summary, template.ContentPack.Manifest.UniqueID, template);
				}
			}
			ModEntry.Monitor.Log(summary.ToString(), LogLevel.Info);
		}
	}
}
