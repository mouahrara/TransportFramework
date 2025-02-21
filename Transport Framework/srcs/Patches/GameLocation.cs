using System;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework;
using xTile.Dimensions;
using xTile.Layers;
using StardewValley;
using StardewValley.GameData.Machines;
using TransportFramework.Classes;
using TransportFramework.Utilities;
using Object = StardewValley.Object;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace TransportFramework.Patches
{
	internal class GameLocationPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.checkAction), new Type[] { typeof(Location), typeof(xTile.Dimensions.Rectangle), typeof(Farmer) }),
				prefix: new HarmonyMethod(typeof(GameLocationPatch), nameof(CheckActionPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), "resetLocalState"),
				postfix: new HarmonyMethod(typeof(GameLocationPatch), nameof(ResetLocalStatePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.isCollidingPosition), new Type[] { typeof(Rectangle), typeof(xTile.Dimensions.Rectangle), typeof(bool), typeof(int), typeof(bool), typeof(Character), typeof(bool), typeof(bool), typeof(bool), typeof(bool) }),
				postfix: new HarmonyMethod(typeof(GameLocationPatch), nameof(IsCollidingPositionPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.CanItemBePlacedHere), new Type[] { typeof(Vector2), typeof(bool), typeof(CollisionMask), typeof(CollisionMask), typeof(bool), typeof(bool) }),
				postfix: new HarmonyMethod(typeof(GameLocationPatch), nameof(CanItemBePlacedHerePostfix))
			);
		}

		private static bool CheckActionPrefix(GameLocation __instance, Location tileLocation, Farmer who, ref bool __result)
		{
			if (!Game1.player.isRidingHorse() || (ModEntry.Config.AllowHorsebackTravel && Game1.player.Items.ContainsId("911")))
			{
				if (ActionsUtility.OpenStationAtTile(tileLocation.X, tileLocation.Y))
				{
					__result = true;
					return false;
				}
				else
				{
					Layer frontLayer = __instance.Map?.GetLayer("Front");

					if ((frontLayer is not null && frontLayer.Tiles[tileLocation.X, tileLocation.Y] is not null) || ActionsUtility.IsFrontLayerOfAnyStation(tileLocation.X, tileLocation.Y))
					{
						if (!AnyHigherPriorityActions(__instance, tileLocation, who) && ActionsUtility.OpenStationAtTile(tileLocation.X, tileLocation.Y + 1))
						{
							__result = true;
							return false;
						}
					}
				}
			}
			return true;
		}

		private static bool AnyHigherPriorityActions(GameLocation location, Location tileLocation, Farmer who)
		{
			Vector2 vector = new(tileLocation.X, tileLocation.Y);

			if (who.CurrentItem is not null && who.CurrentItem.isPlaceable() && who.CurrentItem.canBePlacedHere(location, vector))
			{
				return true;
			}
			if (location.objects.TryGetValue(vector, out Object @object))
			{
				bool isErrorItem = ItemRegistry.GetDataOrErrorItem(@object.QualifiedItemId).IsErrorItem;

				if (@object.Type is not null || isErrorItem)
				{
					if (@object.Type == "Crafting" || @object.Type == "interactive")
					{
						MachineData machineData = @object.GetMachineData();

						if (machineData is not null && who.CurrentItem is not null)
						{
							if ((@object.heldObject.Value is not null && !machineData.AllowLoadWhenFull) || !machineData.OutputRules.Any(r => r.Triggers.Any(t => (t.RequiredItemId is not null && t.RequiredItemId.Equals(who.CurrentItem.QualifiedItemId)) || (t.RequiredTags is not null && t.RequiredTags.Any(rt => who.CurrentItem.GetContextTags().Any(ct => rt.Equals(ct)))))))
							{
								return false;
							}
						}
						return true;
					}
					else if (@object.IsSpawnedObject || isErrorItem)
					{
						if (who.couldInventoryAcceptThisItem(@object) && (!@object.questItem.Value || @object.questId.Value is null || @object.questId.Value == "0" || who.hasQuest(@object.questId.Value)))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private static void ResetLocalStatePostfix(GameLocation __instance)
		{
			if (Game1.currentLocation != __instance)
				return;

			foreach (Station station in ModEntry.Stations)
			{
				if (LocationUtility.GetLocationFromName(station.Location) == __instance)
				{
					if (station.Sprites is not null)
					{
						for (int i = 0; i < station.Sprites.Count; i++)
						{
							if (station.Sprites[i].ConditionsCache && ((station.ConditionsCache && !station.Sprites[i].Type.Equals("Locked")) || (!station.ConditionsCache && !station.Sprites[i].Type.Equals("Unlocked"))))
							{
								TemporaryAnimatedSprite temporaryAnimatedSprite = new(station.Sprites[i].Data.InternalTextureName, station.Sprites[i].Data.SourceRectangle, station.Sprites[i].Data.Interval, station.Sprites[i].Data.AnimationLength, 1, station.Sprites[i].Data.ComputedPosition, station.Sprites[i].Data.Flicker, station.Sprites[i].Data.Flip, station.Sprites[i].Data.ComputedLayerDepth, 0f, station.Sprites[i].Data.ColorAsColor, station.Sprites[i].Data.Scale * Game1.pixelZoom, 0f, station.Sprites[i].Data.Rotation, 0f)
								{
									verticalFlipped = station.Sprites[i].Data.VerticalFlip,
									shakeIntensity = station.Sprites[i].Data.ShakeIntensity,
									pingPong = station.Sprites[i].Data.PingPong,
									drawAboveAlwaysFront = station.Sprites[i].Data.DrawAboveAlwaysFront,
									destroyable = false
								};

								if (station.Sprites[i].Data.Light is not null)
								{
									temporaryAnimatedSprite.lightId = $"{station.Id}_Sprite_{i}";
									temporaryAnimatedSprite.lightcolor = Utility.getOppositeColor(station.Sprites[i].Data.Light.ColorAsColor);
									temporaryAnimatedSprite.lightRadius = station.Sprites[i].Data.Light.Radius;
								}
								__instance.TemporarySprites.Add(temporaryAnimatedSprite);
							}
						}
					}
				}
			}
		}

		private static void IsCollidingPositionPostfix(GameLocation __instance, Rectangle position, ref bool __result)
		{
			foreach (Station station in ModEntry.Stations)
			{
				if (LocationUtility.GetLocationFromName(station.Location) == __instance)
				{
					if (station.Sprites is not null)
					{
						foreach (SSprite sprite in station.Sprites)
						{
							if (sprite.ConditionsCache && ((station.ConditionsCache && !sprite.Type.Equals("Locked")) || (!station.ConditionsCache && !sprite.Type.Equals("Unlocked"))))
							{
								if (sprite.AbsoluteCollisionBoxes is not null)
								{
									foreach (Rectangle absoluteCollisionBox in sprite.AbsoluteCollisionBoxes)
									{
										if (absoluteCollisionBox.Intersects(position))
										{
											__result = true;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		private static void CanItemBePlacedHerePostfix(GameLocation __instance, Vector2 tile, bool itemIsPassable, CollisionMask collisionMask, ref bool __result)
		{
			if (!itemIsPassable && collisionMask.HasFlag(CollisionMask.LocationSpecific))
			{
				Rectangle position = new((int)tile.X * Game1.tileSize, (int)tile.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);

				foreach (Station station in ModEntry.Stations)
				{
					if (LocationUtility.GetLocationFromName(station.Location) == __instance)
					{
						if (station.Sprites is not null)
						{
							foreach (SSprite sprite in station.Sprites)
							{
								if (sprite.ConditionsCache && ((station.ConditionsCache && !sprite.Type.Equals("Locked")) || (!station.ConditionsCache && !sprite.Type.Equals("Unlocked"))))
								{
									if (sprite.AbsoluteCollisionBoxes is not null)
									{
										foreach (Rectangle absoluteCollisionBox in sprite.AbsoluteCollisionBoxes)
										{
											if (absoluteCollisionBox.Intersects(position))
											{
												__result = false;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
