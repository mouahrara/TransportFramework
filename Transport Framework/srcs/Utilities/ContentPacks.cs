using System;
using StardewModdingAPI;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class ContentPacksUtility
	{
		internal static void Initialize()
		{
			Load();
		}

		private static void Load()
		{
			foreach (IContentPack contentPack in ModEntry.Helper.ContentPacks.GetOwned())
			{
				if (!contentPack.HasFile("content.json"))
				{
					ModEntry.Monitor.Log($"{contentPack.Manifest.UniqueID} is missing the 'content.json' file.", LogLevel.Error);
					continue;
				}
				ModEntry.Monitor.Log($"Reading content pack: {contentPack.Manifest.Name} {contentPack.Manifest.Version} from {contentPack.DirectoryPath}", LogLevel.Trace);
				try
				{
					ContentPack deserializedContentPack = contentPack.ReadJsonFile<ContentPack>("content.json");

					if (deserializedContentPack is not null)
					{
						deserializedContentPack.Manifest = contentPack.Manifest;
						deserializedContentPack.ModContent = contentPack.ModContent;
						deserializedContentPack.Translation = contentPack.Translation;
						if (string.IsNullOrWhiteSpace(deserializedContentPack.Format))
						{
							ModEntry.Monitor.Log($"Failed to load content pack ({deserializedContentPack.Manifest.Name} {deserializedContentPack.Manifest.Version}): The 'Format' property is missing.", LogLevel.Error);
							continue;
						}
						switch (deserializedContentPack.Format)
						{
							case "1.0.2":
								LoadContentPackFormat_1_0_2(deserializedContentPack);
								break;
							case "1.0.1":
								LoadContentPackFormat_1_0_1(deserializedContentPack);
								UpgradeContentPackFromFormat_1_0_1(deserializedContentPack);
								break;
							case "1.0.0":
								LoadContentPackFormat_1_0_0(deserializedContentPack);
								UpgradeContentPackFromFormat_1_0_0(deserializedContentPack);
								break;
							default:
								ModEntry.Monitor.Log($"Failed to load content pack ({deserializedContentPack.Manifest.Name} {deserializedContentPack.Manifest.Version}): The format {deserializedContentPack.Format} is not supported.", LogLevel.Error);
								break;
						}
					}
				}
				catch (Exception e)
				{
					ModEntry.Monitor.Log($"Failed to parse content pack ({contentPack.Manifest.Name} {contentPack.Manifest.Version}): {e.Message}", LogLevel.Error);
				}
			}
		}

		private static void LoadContentPackFormat_1_0_2(ContentPack deserializedContentPack)
		{
			LoadContentPackFormat_1_0_1(deserializedContentPack);
		}

		private static void UpgradeContentPackFromFormat_1_0_1(ContentPack deserializedContentPack)
		{
			deserializedContentPack.Format = "1.0.2";
			if (deserializedContentPack.Templates is not null)
			{
				foreach (Template template in deserializedContentPack.Templates)
				{
					if (template.Events is not null)
					{
						foreach (SEvent @event in template.Events)
						{
							if (!string.IsNullOrEmpty(@event.Script))
							{
								@event.Script = @event.Script.Replace($"{ModEntry.ModManifest.UniqueID}_StationTileX", $"{ModEntry.ModManifest.UniqueID}_StationTemplateReferenceTileX");
								@event.Script = @event.Script.Replace($"{ModEntry.ModManifest.UniqueID}_StationTileY", $"{ModEntry.ModManifest.UniqueID}_StationTemplateReferenceTileY");
							}
						}
					}
				}
			}
			if (deserializedContentPack.Stations is not null)
			{
				foreach (Station station in deserializedContentPack.Stations)
				{
					if (!string.IsNullOrWhiteSpace(station.TemplateId))
					{
						station.Template = new()
						{
							Id = station.TemplateId
						};
						station.TemplateId = null;
					}
					if (station.Events is not null)
					{
						foreach (SEvent @event in station.Events)
						{
							if (!string.IsNullOrEmpty(@event.Script))
							{
								@event.Script = @event.Script.Replace($"{ModEntry.ModManifest.UniqueID}_StationTileX", $"{ModEntry.ModManifest.UniqueID}_StationTemplateReferenceTileX");
								@event.Script = @event.Script.Replace($"{ModEntry.ModManifest.UniqueID}_StationTileY", $"{ModEntry.ModManifest.UniqueID}_StationTemplateReferenceTileY");
							}
						}
					}
				}
			}
		}

		private static void LoadContentPackFormat_1_0_1(ContentPack deserializedContentPack)
		{
			LoadContentPackFormat_1_0_0(deserializedContentPack);
			if (deserializedContentPack.Templates is not null)
			{
				for (int i = 0; i < deserializedContentPack.Templates.Count; i++)
				{
					deserializedContentPack.Templates[i].ContentPack = deserializedContentPack;
					if (string.IsNullOrWhiteSpace(deserializedContentPack.Templates[i].Id))
					{
						deserializedContentPack.Templates[i].Id = $"{deserializedContentPack.Manifest.UniqueID}_Template_{i}";
					}
					else if (!deserializedContentPack.Templates[i].Id.StartsWith(deserializedContentPack.Manifest.UniqueID))
					{
						ModEntry.Monitor.Log($"Failed to add template: The 'Id' property ({deserializedContentPack.Templates[i].Id}) must start with the content pack manifest's UniqueID ({deserializedContentPack.Manifest.UniqueID}).", LogLevel.Error);
						continue;
					}
					else
					{
						for (int j = 0; j < i; j++)
						{
							if (deserializedContentPack.Templates[i].Id.Equals(deserializedContentPack.Templates[j].Id))
							{
								ModEntry.Monitor.Log($"Failed to add template: The template with Id '{deserializedContentPack.Templates[i].Id}' already exists.", LogLevel.Error);
								continue;
							}
						}
					}
					ModEntry.Templates.Add(deserializedContentPack.Templates[i]);
				}
			}
		}

		private static void UpgradeContentPackFromFormat_1_0_0(ContentPack deserializedContentPack)
		{
			deserializedContentPack.Format = "1.0.1";
			UpgradeContentPackFromFormat_1_0_1(deserializedContentPack);
		}

		private static void LoadContentPackFormat_1_0_0(ContentPack deserializedContentPack)
		{
			if (deserializedContentPack.Stations is not null)
			{
				for (int i = 0; i < deserializedContentPack.Stations.Count; i++)
				{
					deserializedContentPack.Stations[i].ContentPack = deserializedContentPack;
					if (string.IsNullOrWhiteSpace(deserializedContentPack.Stations[i].Id))
					{
						deserializedContentPack.Stations[i].Id = $"{deserializedContentPack.Manifest.UniqueID}_Station_{i}";
					}
					else if (!deserializedContentPack.Stations[i].Id.StartsWith(deserializedContentPack.Manifest.UniqueID))
					{
						ModEntry.Monitor.Log($"Failed to add station: The 'Id' property ({deserializedContentPack.Stations[i].Id}) must start with the content pack manifest's UniqueID ({deserializedContentPack.Manifest.UniqueID}).", LogLevel.Error);
						continue;
					}
					else
					{
						for (int j = 0; j < i; j++)
						{
							if (deserializedContentPack.Stations[i].Id.Equals(deserializedContentPack.Stations[j].Id))
							{
								ModEntry.Monitor.Log($"Failed to add station: The station with Id '{deserializedContentPack.Stations[i].Id}' already exists.", LogLevel.Error);
								continue;
							}
						}
					}
					ModEntry.Stations.Add(deserializedContentPack.Stations[i]);
				}
			}
		}
	}
}
