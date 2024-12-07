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
						if (string.IsNullOrEmpty(deserializedContentPack.Format))
						{
							ModEntry.Monitor.Log($"Failed to load content pack ({contentPack.Manifest.Name} {contentPack.Manifest.Version}): The 'Format' property is missing.", LogLevel.Error);
							continue;
						}
						if (!deserializedContentPack.Format.Equals("1.0.0"))
						{
							ModEntry.Monitor.Log($"Failed to load content pack ({contentPack.Manifest.Name} {contentPack.Manifest.Version}): The format {deserializedContentPack.Format} is not supported.", LogLevel.Error);
							continue;
						}
						if (deserializedContentPack.Stations is not null)
						{
							for (int i = 0; i < deserializedContentPack.Stations.Count; i++)
							{
								deserializedContentPack.Stations[i].ContentPack = deserializedContentPack;
								if (string.IsNullOrWhiteSpace(deserializedContentPack.Stations[i].Id))
								{
									deserializedContentPack.Stations[i].Id = $"{contentPack.Manifest.UniqueID}_Station_{i}";
								}
								else if (!deserializedContentPack.Stations[i].Id.StartsWith(contentPack.Manifest.UniqueID))
								{
									ModEntry.Monitor.Log($"Failed to add station: The 'Id' property ({deserializedContentPack.Stations[i].Id}) must start with the content pack manifest's UniqueID ({contentPack.Manifest.UniqueID}).", LogLevel.Error);
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
				catch (Exception e)
				{
					ModEntry.Monitor.Log($"Failed to parse content pack ({contentPack.Manifest.Name} {contentPack.Manifest.Version}): {e.Message}", LogLevel.Error);
				}
			}
		}
	}
}
