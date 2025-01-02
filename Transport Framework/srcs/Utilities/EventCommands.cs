using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Audio;
using StardewValley.Objects;
using StardewValley.Pathfinding;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class EventCommandsUtility
	{
		private static readonly PerScreen<SEvent>	sevent = new(() => null);

		internal static SEvent @SEvent
		{
			get => sevent.Value;
			set => sevent.Value = value;
		}

		// Actors
		private static readonly PerScreen<(Character, int)>															actorsMoveTo = new(() => new());
		private static readonly PerScreen<Dictionary<Character, (TemporaryAnimatedSprite, Vector2, string, bool)>>	actorsSyncWithTemporarySprite = new(() => new());

		private static (Character, int) ActorsMoveTo
		{
			get => actorsMoveTo.Value;
			set => actorsMoveTo.Value = value;
		}

		private static Dictionary<Character, (TemporaryAnimatedSprite, Vector2, string, bool)> ActorsSyncWithTemporarySprite
		{
			get => actorsSyncWithTemporarySprite.Value;
			set => actorsSyncWithTemporarySprite.Value = value;
		}

		// Temporary sprites
		private static readonly PerScreen<TemporaryAnimatedSpriteList>																												temporarySprites = new(() => new());
		private static readonly PerScreen<Dictionary<TemporaryAnimatedSprite, float>>																								temporarySpritesTemporaryAnimatedSpriteIntervalChange = new(() => new());
		private static readonly PerScreen<Dictionary<TemporaryAnimatedSprite, float>>																								temporarySpritesTemporaryAnimatedSpriteIntervalVariation = new(() => new());
		private static readonly PerScreen<Dictionary<TemporaryAnimatedSprite, float>>																								temporarySpritesTemporaryAnimatedSpriteRotationChangeChange = new(() => new());
		private static readonly PerScreen<Dictionary<TemporaryAnimatedSprite, bool>>																								temporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero = new(() => new());
		private static readonly PerScreen<Dictionary<TemporaryAnimatedSprite, (Vector2, Vector2, Vector2, Vector2, Func<double, double>, Func<double, double>, string, float)>>		temporarySpritesTemporaryAnimatedSpriteSway = new(() => new());
		private static readonly PerScreen<List<(TemporaryAnimatedSprite, string, int)>>																								temporarySpritesTemporaryAnimatedSpriteFrameSound = new(() => new());
		private static readonly PerScreen<List<(TemporaryAnimatedSprite, TemporaryAnimatedSprite, string, float, string, float, int, bool, Event, string[], EventContext, int)>>	temporarySpritesChangeTemporaryAnimatedSprite = new(() => new());
		private static readonly PerScreen<List<(TemporaryAnimatedSprite, Rectangle, bool, bool)>>																					temporarySpritesDestroyObjectsOnCollision = new(() => new());
		private static readonly PerScreen<List<(FarmerSprite.AnimationFrame, int, Rectangle, Vector2, float, Color, float, float, TemporaryAnimatedSprite, Vector2)>>				temporaryFarmerSprites = new(() => new());

		public static TemporaryAnimatedSpriteList TemporarySprites
		{
			get => temporarySprites.Value;
			set => temporarySprites.Value = value;
		}

		private static Dictionary<TemporaryAnimatedSprite, float> TemporarySpritesTemporaryAnimatedSpriteIntervalChange
		{
			get => temporarySpritesTemporaryAnimatedSpriteIntervalChange.Value;
			set => temporarySpritesTemporaryAnimatedSpriteIntervalChange.Value = value;
		}

		private static Dictionary<TemporaryAnimatedSprite, float> TemporarySpritesTemporaryAnimatedSpriteIntervalVariation
		{
			get => temporarySpritesTemporaryAnimatedSpriteIntervalVariation.Value;
			set => temporarySpritesTemporaryAnimatedSpriteIntervalVariation.Value = value;
		}

		private static Dictionary<TemporaryAnimatedSprite, float> TemporarySpritesTemporaryAnimatedSpriteRotationChangeChange
		{
			get => temporarySpritesTemporaryAnimatedSpriteRotationChangeChange.Value;
			set => temporarySpritesTemporaryAnimatedSpriteRotationChangeChange.Value = value;
		}

		private static Dictionary<TemporaryAnimatedSprite, bool> TemporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero
		{
			get => temporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero.Value;
			set => temporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero.Value = value;
		}

		private static Dictionary<TemporaryAnimatedSprite, (Vector2, Vector2, Vector2, Vector2, Func<double, double>, Func<double, double>, string, float)> TemporarySpritesTemporaryAnimatedSpriteSway
		{
			get => temporarySpritesTemporaryAnimatedSpriteSway.Value;
			set => temporarySpritesTemporaryAnimatedSpriteSway.Value = value;
		}

		private static List<(TemporaryAnimatedSprite, string, int)> TemporarySpritesTemporaryAnimatedSpriteFrameSound
		{
			get => temporarySpritesTemporaryAnimatedSpriteFrameSound.Value;
			set => temporarySpritesTemporaryAnimatedSpriteFrameSound.Value = value;
		}

		private static List<(TemporaryAnimatedSprite, TemporaryAnimatedSprite, string, float, string, float, int, bool, Event, string[], EventContext, int)> TemporarySpritesChangeTemporaryAnimatedSprite
		{
			get => temporarySpritesChangeTemporaryAnimatedSprite.Value;
			set => temporarySpritesChangeTemporaryAnimatedSprite.Value = value;
		}

		private static List<(TemporaryAnimatedSprite, Rectangle, bool, bool)> TemporarySpritesDestroyObjectsOnCollision
		{
			get => temporarySpritesDestroyObjectsOnCollision.Value;
			set => temporarySpritesDestroyObjectsOnCollision.Value = value;
		}

		public static List<(FarmerSprite.AnimationFrame, int, Rectangle, Vector2, float, Color, float, float, TemporaryAnimatedSprite, Vector2)> TemporaryFarmerSprites
		{
			get => temporaryFarmerSprites.Value;
			set => temporaryFarmerSprites.Value = value;
		}

		// Sounds
		private static readonly PerScreen<List<ICue>>								sounds = new(() => new());
		private static readonly PerScreen<Dictionary<ICue, float>>					soundsVolumeChange = new(() => new());
		private static readonly PerScreen<Dictionary<ICue, (int, int, int, bool)>>	soundsPitchChange = new(() => new());

		public static List<ICue> Sounds
		{
			get => sounds.Value;
			set => sounds.Value = value;
		}

		public static Dictionary<ICue, float> SoundsVolumeChange
		{
			get => soundsVolumeChange.Value;
			set => soundsVolumeChange.Value = value;
		}

		public static Dictionary<ICue, (int, int, int, bool)> SoundsPitchChange
		{
			get => soundsPitchChange.Value;
			set => soundsPitchChange.Value = value;
		}

		// Location specific commands
		private static readonly PerScreen<(TemporaryAnimatedSpriteList, TemporaryAnimatedSpriteList, TemporaryAnimatedSpriteList)>	locationSpecificCommandDrawParrotExpressLines = new(() => new());

		private static (TemporaryAnimatedSpriteList, TemporaryAnimatedSpriteList, TemporaryAnimatedSpriteList) LocationSpecificCommandDrawParrotExpressLines
		{
			get => locationSpecificCommandDrawParrotExpressLines.Value;
			set => locationSpecificCommandDrawParrotExpressLines.Value = value;
		}

		// Start event
		public static void StartEvent(SEvent @event, Event gameEvent)
		{
			if (Game1.currentLocation is not null)
			{
				SEvent = @event;
				gameEvent.onEventFinished += () => SEvent = null;
				Game1.currentLocation.currentEvent = gameEvent;
				Game1.eventUp = true;
				if (@event.Station.Sprites is not null)
				{
					foreach (SSprite sprite in @event.Station.Sprites)
					{
						if (Game1.currentLocation.TemporarySprites is not null)
						{
							foreach (TemporaryAnimatedSprite temporaryAnimatedSprite in Game1.currentLocation.TemporarySprites)
							{
								if (sprite.Data.InternalTextureName is not null && sprite.Data.InternalTextureName.Equals(temporaryAnimatedSprite.textureName) && sprite.Data.SourceRectangle.X == temporaryAnimatedSprite.sourceRectStartingPos.X && sprite.Data.SourceRectangle.Y == temporaryAnimatedSprite.sourceRectStartingPos.Y && sprite.Data.SourceRectangle.Width == temporaryAnimatedSprite.sourceRect.Width && sprite.Data.SourceRectangle.Height == temporaryAnimatedSprite.sourceRect.Height && sprite.Data.ComputedPosition == temporaryAnimatedSprite.position)
								{
									TemporarySprites.Add(temporaryAnimatedSprite);
									if (sprite.CollisionBoxes is not null)
									{
										foreach (Rectangle collisionBox in sprite.ComputedCollisionBoxes)
										{
											TemporarySpritesDestroyObjectsOnCollision.Add((temporaryAnimatedSprite, collisionBox, true, true));
										}
									}
									break;
								}
							}
						}
					}
					if (TemporarySprites.Count > 0)
					{
						gameEvent.onEventFinished += gameEvent.onEventFinished?.GetInvocationList().Contains(TemporarySprites.Clear) != true ? TemporarySprites.Clear : null;
					}
				}
			}
		}

		public static void Register()
		{
			// Event properties
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_hideActiveObject", HideActiveObject);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_showActiveObject", ShowActiveObject);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_hideGroundObjects", HideGroundObjects);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_showGroundObjects", ShowGroundObjects);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_hideWorldCharacters", HideWorldCharacters);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_showWorldCharacters", ShowWorldCharacters);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_ignoreObjectCollisions", IgnoreObjectCollisions);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_considerObjectCollisions", ConsiderObjectCollisions);

			// Actors
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_moveTo", MoveTo);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_hideActor", HideActor);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_showActor", ShowActor);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_disableShadowOffset", DisableShadowOffset);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_enableShadowOffset", EnableShadowOffset);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_beginSyncWithTemporarySprite", BeginSyncWithTemporarySprite);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_endSyncWithTemporarySprite", EndSyncWithTemporarySprite);

			// Temporary sprites
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_temporaryAnimatedSprite", TemporaryAnimatedSprite);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_changeTemporaryAnimatedSprite", ChangeTemporaryAnimatedSprite);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_removeTemporarySprites", RemoveTemporarySprites);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_removeTemporarySpritesRange", RemoveTemporarySpritesRange);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_destroyObjectsOnCollision", DestroyObjectsOnCollision);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_preserveObjectsOnCollision", PreserveObjectsOnCollision);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_temporaryFarmerSprite", TemporaryFarmerSprite);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_removeTemporaryFarmerSprites", RemoveTemporaryFarmerSprites);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_removeTemporaryFarmerSpritesRange", RemoveTemporaryFarmerSpritesRange);

			// Sounds
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_playSound", PlaySound);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_changeSound", ChangeSound);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_stopSounds", StopSounds);
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_stopSoundsRange", StopSoundsRange);

			// Control flow
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_query", Query);

			// Location specific commands
			Event.RegisterCommand($"{ModEntry.ModManifest.UniqueID}_locationSpecificCommand_draw_ParrotExpress_lines", LocationSpecificCommand_draw_ParrotExpress_lines);
		}

		// Event properties
		private static void HideActiveObject(Event @event, string[] args, EventContext context)
		{
			DisplayActiveObject(@event, false);
		}

		private static void ShowActiveObject(Event @event, string[] args, EventContext context)
		{
			DisplayActiveObject(@event, true);
		}

		private static void DisplayActiveObject(Event @event, bool show)
		{
			@event.showActiveObject = show;
			@event.CurrentCommand++;
		}

		private static void HideGroundObjects(Event @event, string[] args, EventContext context)
		{
			DisplayGroundObjects(@event, false);
		}

		private static void ShowGroundObjects(Event @event, string[] args, EventContext context)
		{
			DisplayGroundObjects(@event, true);
		}

		private static void DisplayGroundObjects(Event @event, bool show)
		{
			@event.showGroundObjects = show;
			@event.CurrentCommand++;
		}

		private static void HideWorldCharacters(Event @event, string[] args, EventContext context)
		{
			DisplayWorldCharacters(@event, false);
		}

		private static void ShowWorldCharacters(Event @event, string[] args, EventContext context)
		{
			DisplayWorldCharacters(@event, true);
		}

		private static void DisplayWorldCharacters(Event @event, bool show)
		{
			@event.showWorldCharacters = show;
			@event.CurrentCommand++;
		}

		private static void IgnoreObjectCollisions(Event @event, string[] args, EventContext context)
		{
			HandleObjectCollisions(@event, true);
		}

		private static void ConsiderObjectCollisions(Event @event, string[] args, EventContext context)
		{
			HandleObjectCollisions(@event, false);
		}

		private static void HandleObjectCollisions(Event @event, bool ignore)
		{
			@event.ignoreObjectCollisions = ignore;
			@event.CurrentCommand++;
		}

		// Actors
		private static void MoveTo(Event @event, string[] args, EventContext context)
		{
			if (args.Length < 5)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected mandatory fields [actor x y direction] followed by optional fields [added-speed duration-limit]");
				return;
			}
			if (!TryGetActor(@event, args, 1, out Character actor, out string error) || !ArgUtility.TryGetVector2(args, 2, out Vector2 tile, out error, integerOnly: true) || !ArgUtility.TryGetDirection(args, 4, out int direction, out error) || !ArgUtility.TryGetOptionalInt(args, 5, out int addedSpeed, out error, 0) || !ArgUtility.TryGetOptionalInt(args, 6, out int durationLimit, out error, int.MaxValue))
			{
				context.LogErrorAndSkip(error);
				return;
			}

			Character a = ActorsMoveTo.Item1;
			int dl = ActorsMoveTo.Item2;

			if (a is null)
			{
				actor.controller = new PathFindController(actor, context.Location, new Point((int)tile.X, (int)tile.Y), direction, (character, location) => MoveToEndBehavior(@event, (int)tile.X, (int)tile.Y, direction));
				if (actor is Farmer farmer)
				{
					farmer.controller.allowPlayerPathingInEvent = true;
					if (addedSpeed > 0)
					{
						farmer.canOnlyWalk = false;
						farmer.setRunning(true, true);
					}
					if (farmer.mount is not null)
					{
						farmer.mount.farmerPassesThrough = true;
					}
				}
				actor.addedSpeed = addedSpeed;
				ActorsMoveTo = (actor, durationLimit);
				@event.onEventFinished += () => MoveToEndBehavior(@event, (int)tile.X, (int)tile.Y, direction);
			}
			else
			{
				ActorsMoveTo = (a, dl - (int)Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds);
				if (dl < 0 || a.controller is null && 3000 < durationLimit - dl || a.controller is not null && 3000 < a.controller.timerSinceLastCheckPoint)
				{
					MoveToEndBehavior(@event, (int)tile.X, (int)tile.Y, direction);
				}
			}
		}

		private static void MoveToEndBehavior(Event @event, int x, int y, int direction)
		{
			Character actor = ActorsMoveTo.Item1;

			if (actor is not null)
			{
				actor.addedSpeed = 0;
				if (actor is Farmer)
				{
					Farmer farmer = actor as Farmer;

					if (farmer.mount is not null)
					{
						farmer.mount.farmerPassesThrough = false;
					}
					farmer.setRunning(false, true);
					farmer.canOnlyWalk = true;
				}
				actor.controller = null;
				actor.setTileLocation(new Vector2(x, y));
				actor.faceDirection(direction);
				actor.Halt();
				ActorsMoveTo = new();
			}
			if (@event is not null)
			{
				@event.CurrentCommand++;
			}
		}

		private static void HideActor(Event @event, string[] args, EventContext context)
		{
			DisplayActor(@event, args, context, false);
		}

		private static void ShowActor(Event @event, string[] args, EventContext context)
		{
			DisplayActor(@event, args, context, true);
		}

		private static void DisplayActor(Event @event, string[] args, EventContext context, bool display)
		{
			if (args.Length < 2)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected a mandatory [actor] field");
				return;
			}
			if (!TryGetActor(@event, args, 1, out Character actor, out string error))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			actor.shouldShadowBeOffset = !display;
			actor.drawOffset = display ? new Vector2(0, 0) : new Vector2(float.MinValue, float.MinValue);
			@event.CurrentCommand++;
		}

		private static void DisableShadowOffset(Event @event, string[] args, EventContext context)
		{
			SetShadowOffset(@event, args, context, false);
		}

		private static void EnableShadowOffset(Event @event, string[] args, EventContext context)
		{
			SetShadowOffset(@event, args, context, true);
		}

		private static void SetShadowOffset(Event @event, string[] args, EventContext context, bool value)
		{
			if (args.Length < 2)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected a mandatory [actor] field");
				return;
			}
			if (!TryGetActor(@event, args, 1, out Character actor, out string error))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			actor.shouldShadowBeOffset = value;
			@event.CurrentCommand++;
		}

		private static void BeginSyncWithTemporarySprite(Event @event, string[] args, EventContext context)
		{
			if (args.Length < 2)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected a mandatory [actor] field followed by optional fields [index type]");
				return;
			}
			if (!TryGetActor(@event, args, 1, out Character actor, out string error) || !ArgUtility.TryGetOptionalInt(args, 2, out int index, out error, -1) || !ArgUtility.TryGetOptional(args, 3, out string type, out error, "position"))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (ActorsSyncWithTemporarySprite.Any(s => s.Key.Equals(actor)))
			{
				context.LogErrorAndSkip($"actor {actor.Name} is already synced with a temporary sprite");
				return;
			}
			if (index < 0)
			{
				index += TemporarySprites.Count;
			}
			if (TemporarySprites.Count == 0)
			{
				context.LogErrorAndSkip($"temporary sprite list is empty");
				return;
			}
			if (!(0 <= index && index < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {index} is out of range");
				return;
			}
			type = type.ToLower();
			if (!type.Equals("position") && !type.Equals("offset"))
			{
				context.LogErrorAndSkip($"The type '{type}' must be one of 'position' or 'offset'");
				return;
			}
			ActorsSyncWithTemporarySprite.Add(actor, (TemporarySprites[index], TemporarySprites[index].Position, type, actor.shouldShadowBeOffset));
			if (ActorsSyncWithTemporarySprite[actor].Item3.Equals("offset"))
			{
				actor.shouldShadowBeOffset = true;
			}
			@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(ActorsSyncWithTemporarySprite.Clear) != true ? ActorsSyncWithTemporarySprite.Clear : null;
			@event.CurrentCommand++;
		}

		private static void EndSyncWithTemporarySprite(Event @event, string[] args, EventContext context)
		{
			if (args.Length < 2)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected a mandatory [actor] field");
				return;
			}
			if (!TryGetActor(@event, args, 1, out Character actor, out string error))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (!ActorsSyncWithTemporarySprite.Any(s => s.Key.Equals(actor)))
			{
				context.LogErrorAndSkip($"actor {actor.Name} is not synced with any temporary sprite");
				return;
			}
			if (ActorsSyncWithTemporarySprite[actor].Item3.Equals("offset"))
			{
				actor.shouldShadowBeOffset = ActorsSyncWithTemporarySprite[actor].Item4;
			}
			ActorsSyncWithTemporarySprite.Remove(actor);
			@event.CurrentCommand++;
		}

		public static void ActorsSyncWithTemporarySpriteUpdate()
		{
			if (ActorsSyncWithTemporarySprite is not null)
			{
				foreach (KeyValuePair<Character, (TemporaryAnimatedSprite, Vector2, string, bool)> entry in ActorsSyncWithTemporarySprite)
				{
					Character actor = entry.Key;
					TemporaryAnimatedSprite sprite = entry.Value.Item1;
					Vector2 oldSpritePosition = entry.Value.Item2;
					string type = entry.Value.Item3;
					bool shouldShadowBeOffset = entry.Value.Item4;

					if (type.Equals("position"))
					{
						actor.Position += sprite.Position - oldSpritePosition;
					}
					else
					{
						actor.drawOffset += sprite.Position - oldSpritePosition;
					}
					ActorsSyncWithTemporarySprite[actor] = (sprite, sprite.Position, type, shouldShadowBeOffset);
				}
			}
		}

		private static bool TryGetActor(Event @event, string[] array, int index, out Character value, out string error)
		{
			if (!ArgUtility.TryGet(array, index, out string actor, out error))
			{
				value = null;
				return false;
			}

			Character character = @event.IsFarmerActorId(actor, out int farmerNumber) ? @event.GetFarmerActor(farmerNumber) : @event.getActorByName(actor);

			if (character is null)
			{
				error = "no actor found with name '" + actor + "'";
				value = null;
				return false;
			}
			value = character;
			return true;
		}

		// Temporary sprites
		private static void TemporaryAnimatedSprite(Event @event, string[] args, EventContext context)
		{
			if (args.Length < 20)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected mandatory fields [textureName, sourceRect, animationInterval, animationLength, numberOfLoops, position, flicker, flipped, layerDepth, alphaFade, color, scale, scaleChange, rotation, rotationChange] followed by an optional [flags] field");
				return;
			}
			if (!ArgUtility.TryGet(args, 1, out string textureName, out string error) || !ArgUtility.TryGetRectangle(args, 2, out Rectangle sourceRect, out error) || !ArgUtility.TryGetFloat(args, 6, out float animationInterval, out error) || !ArgUtility.TryGetInt(args, 7, out int animationLength, out error) || !ArgUtility.TryGetInt(args, 8, out int numberOfLoops, out error) || !ArgUtility.TryGetVector2(args, 9, out Vector2 position, out error, integerOnly: false) || !ArgUtility.TryGetBool(args, 11, out bool flicker, out error) || !ArgUtility.TryGetBool(args, 12, out bool flipped, out error) || !ArgUtility.TryGetFloat(args, 13, out float layerDepth, out error) || !ArgUtility.TryGetFloat(args, 14, out float alphaFade, out error) || !TryGetColor(args, 15, out Color color, out error) || !ArgUtility.TryGetFloat(args, 16, out float scale, out error) || !ArgUtility.TryGetFloat(args, 17, out float scaleChange, out error) || !ArgUtility.TryGetFloat(args, 18, out float rotation, out error) || !ArgUtility.TryGetFloat(args, 19, out float rotationChange, out error))
			{
				context.LogErrorAndSkip(error);
				return;
			}

			TemporaryAnimatedSprite temporaryAnimatedSprite = new(textureName, sourceRect, animationInterval, animationLength, numberOfLoops, @event.OffsetPosition(position * Game1.tileSize), flicker, flipped, @event.OffsetPosition(new Vector2(0f, layerDepth) * Game1.tileSize).Y / 10000f, alphaFade, color, scale * Game1.pixelZoom, scaleChange, rotation, rotationChange);

			SetTemporaryAnimatedSpriteFlags(@event, args, context, 20, temporaryAnimatedSprite);
			context.Location.TemporarySprites.Add(temporaryAnimatedSprite);
			TemporarySprites.Add(temporaryAnimatedSprite);
			@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporarySprites.Clear) != true ? TemporarySprites.Clear : null;
			@event.CurrentCommand++;
		}

		private static void ChangeTemporaryAnimatedSprite(Event @event, string[] args, EventContext context)
		{
			if (args.Length < 9)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected mandatory fields [indexOfTemporaryAnimatedSpriteToChange, indexOfTemporaryAnimatedSpriteToCompare, operatorX, x, operatorY, y, delay, flags]");
				return;
			}
			if (!ArgUtility.TryGetInt(args, 1, out int indexOfTemporaryAnimatedSpriteToChange, out string error) || !ArgUtility.TryGetInt(args, 2, out int indexOfTemporaryAnimatedSpriteToCompare, out error))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (indexOfTemporaryAnimatedSpriteToChange < 0)
			{
				indexOfTemporaryAnimatedSpriteToChange += TemporarySprites.Count;
			}
			if (indexOfTemporaryAnimatedSpriteToCompare < 0)
			{
				indexOfTemporaryAnimatedSpriteToCompare += TemporarySprites.Count;
			}
			if (TemporarySprites.Count == 0)
			{
				context.LogErrorAndSkip($"temporary sprite list is empty");
				return;
			}
			if (!(0 <= indexOfTemporaryAnimatedSpriteToChange && indexOfTemporaryAnimatedSpriteToChange < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {indexOfTemporaryAnimatedSpriteToChange} is out of range");
				return;
			}
			if (!(0 <= indexOfTemporaryAnimatedSpriteToCompare && indexOfTemporaryAnimatedSpriteToCompare < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {indexOfTemporaryAnimatedSpriteToCompare} is out of range");
				return;
			}
			if (!ArgUtility.TryGet(args, 3, out string operatorX, out error) || (!operatorX.ToLower().Equals("none") && !operatorX.Equals("<") && !operatorX.Equals("<=") &&!operatorX.Equals("!=") &&!operatorX.Equals("==") && !operatorX.Equals(">") && !operatorX.Equals(">=")))
			{
				context.LogErrorAndSkip(error ?? $"The operator '{operatorX}' must be one of 'none', '<', '<=', '!=', '==', '>' or '>='");
				return;
			}
			if (!ArgUtility.TryGetFloat(args, 4, out float x, out error))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (!ArgUtility.TryGet(args, 5, out string operatorY, out error) || (!operatorX.ToLower().Equals("none") && !operatorX.Equals("<") && !operatorX.Equals("<=") &&!operatorX.Equals("!=") &&!operatorX.Equals("==") && !operatorX.Equals(">") && !operatorX.Equals(">=")))
			{
				context.LogErrorAndSkip(error ?? $"The operator '{operatorY}' must be one of 'none', '<', '<=', '!=', '==', '>' or '>='");
				return;
			}
			if (!ArgUtility.TryGetFloat(args, 6, out float y, out error))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (!ArgUtility.TryGetInt(args, 7, out int delay, out error))
			{
				context.LogErrorAndSkip(error);
				return;
			}

			const int flagsIndex = 8;
			TemporaryAnimatedSprite temporaryAnimatedSpriteToChange = TemporarySprites[indexOfTemporaryAnimatedSpriteToChange];
			TemporaryAnimatedSprite temporaryAnimatedSpriteToCompare = TemporarySprites[indexOfTemporaryAnimatedSpriteToCompare];

			TemporarySpritesChangeTemporaryAnimatedSprite.Add((temporaryAnimatedSpriteToChange, temporaryAnimatedSpriteToCompare, operatorX, x, operatorY, y, delay, false, @event, args, context, flagsIndex));
			@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporarySpritesChangeTemporaryAnimatedSprite.Clear) != true ? TemporarySpritesChangeTemporaryAnimatedSprite.Clear : null;
			@event.CurrentCommand++;
		}

		private static void SetTemporaryAnimatedSpriteFlags(Event @event, string[] args, EventContext context, int start_index, TemporaryAnimatedSprite temporaryAnimatedSprite)
		{
			for (int i = start_index; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "texture_name":
					{
						if (!ArgUtility.TryGet(args, i + 1, out string value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.textureName = value;
						i += 1;
						break;
					}
					case "source_rectangle":
					{
						if (!ArgUtility.TryGetRectangle(args, i + 1, out Rectangle value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.sourceRect = value;
						i += 4;
						break;
					}
					case "interval":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.interval = value;
						i += 1;
						break;
					}
					case "interval_change":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						TemporarySpritesTemporaryAnimatedSpriteIntervalChange.Add(temporaryAnimatedSprite, value);
						@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporarySpritesTemporaryAnimatedSpriteIntervalChange.Clear) != true ? TemporarySpritesTemporaryAnimatedSpriteIntervalChange.Clear : null;
						i += 1;
						break;
					}
					case "interval_variation":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						TemporarySpritesTemporaryAnimatedSpriteIntervalVariation.Add(temporaryAnimatedSprite, value);
						@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporarySpritesTemporaryAnimatedSpriteIntervalVariation.Clear) != true ? TemporarySpritesTemporaryAnimatedSpriteIntervalVariation.Clear : null;
						i += 1;
						break;
					}
					case "animation_length":
					{
						if (!ArgUtility.TryGetInt(args, i + 1, out int value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.animationLength = value;
						i += 1;
						break;
					}
					case "number_of_loops":
					{
						if (!ArgUtility.TryGetInt(args, i + 1, out int value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.totalNumberOfLoops = value;
						i += 1;
						break;
					}
					case "position":
					{
						if (!ArgUtility.TryGetVector2(args, i + 1, out Vector2 value, out string error, integerOnly: false) )
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.position = @event.OffsetPosition(value * Game1.tileSize);
						i += 2;
						break;
					}
					case "flicker":
					{
						if (!ArgUtility.TryGetBool(args, i + 1, out bool value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.flicker = value;
						i += 1;
						break;
					}
					case "flip":
					{
						if (!ArgUtility.TryGetBool(args, i + 1, out bool value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.flipped = value;
						i += 1;
						break;
					}
					case "vertical_flip":
					{
						if (!ArgUtility.TryGetBool(args, i + 1, out bool value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.verticalFlipped = value;
						i += 1;
						break;
					}
					case "layer_depth":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.layerDepth = @event.OffsetPosition(new Vector2(0f, value) * Game1.tileSize).Y / 10000f;
						i += 1;
						break;
					}
					case "alpha_fade":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.alphaFade = value;
						i += 1;
						break;
					}
					case "color":
					{
						if (!ArgUtility.TryGet(args, i + 1, out string value, out string error))
						{
							context.LogError(error);
							break;
						}

						Color? color = Utility.StringToColor(value);

						if (color.HasValue)
						{
							temporaryAnimatedSprite.color = color.Value;
						}
						else
						{
							context.LogError($"index {i + 1} has value '{value}', which can't be parsed as a color");
						}
						i += 1;
						break;
					}
					case "time_based_motion":
					{
						if (!ArgUtility.TryGetBool(args, i + 1, out bool value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.timeBasedMotion = value;
						i += 1;
						break;
					}
					case "motion":
					{
						if (!ArgUtility.TryGetVector2(args, i + 1, out Vector2 value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.motion = value;
						i += 2;
						break;
					}
					case "acceleration":
					{
						if (!ArgUtility.TryGetVector2(args, i + 1, out Vector2 value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.acceleration = value;
						i += 2;
						break;
					}
					case "acceleration_change":
					{
						if (!ArgUtility.TryGetVector2(args, i + 1, out Vector2 value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.accelerationChange = value;
						i += 2;
						break;
					}
					case "stop_accelerating_when_velocity_is_zero":
					{
						if (!ArgUtility.TryGetBool(args, i + 1, out bool value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.stopAcceleratingWhenVelocityIsZero = value;
						i += 1;
						break;
					}
					case "scale":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.scale = value * Game1.pixelZoom;
						i += 1;
						break;
					}
					case "scale_change":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.scaleChange = value;
						i += 1;
						break;
					}
					case "scale_change_change":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.scaleChangeChange = value;
						i += 1;
						break;
					}
					case "rotation":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.rotation = value;
						i += 1;
						break;
					}
					case "rotation_change":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.rotationChange = value;
						i += 1;
						break;
					}
					case "rotation_change_change":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						TemporarySpritesTemporaryAnimatedSpriteRotationChangeChange.Add(temporaryAnimatedSprite, value);
						@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporarySpritesTemporaryAnimatedSpriteRotationChangeChange.Clear) != true ? TemporarySpritesTemporaryAnimatedSpriteRotationChangeChange.Clear : null;
						i += 1;
						break;
					}
					case "stop_rotating_when_velocity_is_zero":
					{
						if (!ArgUtility.TryGetBool(args, i + 1, out bool value, out string error))
						{
							context.LogError(error);
							break;
						}
						TemporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero.Add(temporaryAnimatedSprite, value);
						@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero.Clear) != true ? TemporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero.Clear : null;
						i += 1;
						break;
					}
					case "shake_intensity":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.shakeIntensity = value;
						i += 1;
						break;
					}
					case "shake_intensity_change":
					{
						if (!ArgUtility.TryGetFloat(args, i + 1, out float value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.shakeIntensityChange = value;
						i += 1;
						break;
					}
					case "sway":
					{
						if (!ArgUtility.TryGetVector2(args, i + 1, out Vector2 startingVector, out string error) || !ArgUtility.TryGetVector2(args, i + 3, out Vector2 endingVector, out error) || !ArgUtility.TryGetVector2(args, i + 5, out Vector2 multiplierVector, out error) || !ArgUtility.TryGetVector2(args, i + 7, out Vector2 speedVector, out error) || !ArgUtility.TryGet(args, i + 9, out string functionXAsString, out error) || !ArgUtility.TryGet(args, i + 10, out string functionYAsString, out error) || !ArgUtility.TryGet(args, i + 11, out string axisAsString, out error) || !ArgUtility.TryGet(args, i + 12, out string swayOffsetAsString, out error))
						{
							context.LogError(error);
							break;
						}

						Func<double, double> functionX;
						Func<double, double> functionY;
						string axis;
						float swayOffset;

						functionXAsString = functionXAsString.ToLower();
						if (functionXAsString.Equals("default") || functionXAsString.Equals("sin"))
						{
							functionX = Math.Sin;
						}
						else if (functionXAsString.Equals("cos"))
						{
							functionX = Math.Cos;
						}
						else
						{
							context.LogError($"The function x '{functionXAsString}' must be one of 'default', 'sin' or 'cos'");
							break;
						}
						functionYAsString = functionYAsString.ToLower();
						if (functionYAsString.Equals("sin"))
						{
							functionY = Math.Sin;
						}
						else if (functionYAsString.Equals("default") || functionYAsString.Equals("cos"))
						{
							functionY = Math.Cos;
						}
						else
						{
							context.LogError($"The function y '{functionYAsString}' must be one of 'default', 'sin' or 'cos'");
							break;
						}
						axisAsString = axisAsString.ToLower();
						if (axisAsString.Equals("x") || axisAsString.Equals("y") || axisAsString.Equals("xy"))
						{
							axis = axisAsString;
						}
						else if (axisAsString.Equals("default"))
						{
							axis = "xy";
						}
						else
						{
							context.LogError($"The axis '{axisAsString}' must be one of 'default', 'x', 'y' or 'xy'");
							break;
						}
						swayOffsetAsString = swayOffsetAsString.ToLower();
						if (swayOffsetAsString.Equals("default"))
						{
							swayOffset = Utility.RandomFloat(0f, 100f);
						}
						else if (!ArgUtility.TryGetFloat(new string[] {swayOffsetAsString}, 0, out swayOffset, out error) )
						{
							context.LogError($"The sway offset '{axisAsString}' must be must be 'default' or a float number");
							break;
						}

						if ((axis.Equals("x") || axis.Equals("xy")) && endingVector.X == startingVector.X)
						{
							context.LogError($"The x coordinate of the start point cannot be equal to the x coordinate of the end point with the {axisAsString} axis");
							break;
						}
						if ((axis.Equals("y") || axis.Equals("xy")) && endingVector.Y == startingVector.Y)
						{
							context.LogError($"The y coordinate of the start point cannot be equal to the y coordinate of the end point with the {axisAsString} axis");
							break;
						}
						TemporarySpritesTemporaryAnimatedSpriteSway.Add(temporaryAnimatedSprite, (startingVector * Game1.tileSize, endingVector * Game1.tileSize, multiplierVector, speedVector, functionX, functionY, axis, swayOffset));
						@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporarySpritesTemporaryAnimatedSpriteSway.Clear) != true ? TemporarySpritesTemporaryAnimatedSpriteSway.Clear : null;
						i += 12;
						break;
					}
					case "light":
					{
						if (!ArgUtility.TryGet(args, i + 1, out string lightColor, out string error) || !ArgUtility.TryGetFloat(args, i + 2, out float lightRadius, out error))
						{
							context.LogError(error);
							break;
						}

						Color? color = Utility.StringToColor(lightColor);

						if (color.HasValue)
						{
							temporaryAnimatedSprite.lightId = $"{SEvent.Station.Id}_Sprite_{SEvent.Station.Sprites.Count + @event.CurrentCommand}";
							temporaryAnimatedSprite.lightcolor = Utility.getOppositeColor(color.Value);
							temporaryAnimatedSprite.lightRadius = lightRadius;
						}
						else
						{
							context.LogError($"index {i + 1} has value '{lightColor}', which can't be parsed as a color");
						}
						i += 2;
						break;
					}
					case "tick_before_animation_start":
					{
						if (!ArgUtility.TryGetInt(args, i + 1, out int value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.ticksBeforeAnimationStart = value;
						i += 1;
						break;
					}
					case "delay_before_animation_start":
					{
						if (!ArgUtility.TryGetInt(args, i + 1, out int value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.delayBeforeAnimationStart = value;
						i += 1;
						break;
					}
					case "start_sound":
					{
						if (!ArgUtility.TryGet(args, i + 1, out string value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.startSound = value;
						i += 1;
						break;
					}
					case "end_sound":
					{
						if (!ArgUtility.TryGet(args, i + 1, out string value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.endSound = value;
						i += 1;
						break;
					}
					case "frame_sound":
					{
						if (!ArgUtility.TryGet(args, i + 1, out string sound, out string error) || !ArgUtility.TryGetInt(args, i + 2, out int index, out error))
						{
							context.LogError(error);
							break;
						}
						TemporarySpritesTemporaryAnimatedSpriteFrameSound.Add((temporaryAnimatedSprite, sound, index));
						@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporarySpritesTemporaryAnimatedSpriteFrameSound.Clear) != true ? TemporarySpritesTemporaryAnimatedSpriteFrameSound.Clear : null;
						i += 2;
						break;
					}
					case "ping_pong":
					{
						if (!ArgUtility.TryGetBool(args, i + 1, out bool value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.pingPong = value;
						i += 1;
						break;
					}
					case "hold_last_frame":
					{
						if (!ArgUtility.TryGetBool(args, i + 1, out bool value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.holdLastFrame = value;
						i += 1;
						break;
					}
					case "draw_above_always_front":
					{
						if (!ArgUtility.TryGetBool(args, i + 1, out bool value, out string error))
						{
							context.LogError(error);
							break;
						}
						temporaryAnimatedSprite.drawAboveAlwaysFront = value;
						i += 1;
						break;
					}
					default:
						context.LogError("unknown option '" + args[i] + "'");
						break;
				}
			}
		}

		public static void TemporarySpritesTemporaryAnimatedSpriteIntervalChangeUpdate()
		{
			if (TemporarySpritesTemporaryAnimatedSpriteIntervalChange is not null)
			{
				foreach (KeyValuePair<TemporaryAnimatedSprite, float> entry in TemporarySpritesTemporaryAnimatedSpriteIntervalChange)
				{
					TemporaryAnimatedSprite sprite = entry.Key;
					float interval_change = entry.Value;

					sprite.interval += interval_change * ((!sprite.timeBasedMotion) ? 1 : Game1.currentGameTime.ElapsedGameTime.Milliseconds);
				}
			}
		}

		public static void TemporarySpritesTemporaryAnimatedSpriteIntervalVariationUpdate()
		{
			if (TemporarySpritesTemporaryAnimatedSpriteIntervalVariation is not null)
			{
				foreach (KeyValuePair<TemporaryAnimatedSprite, float> entry in TemporarySpritesTemporaryAnimatedSpriteIntervalVariation)
				{
					TemporaryAnimatedSprite sprite = entry.Key;
					float interval_variation = entry.Value;

					sprite.timer += Utility.RandomFloat(-interval_variation, interval_variation) * ((!sprite.timeBasedMotion) ? 1 : Game1.currentGameTime.ElapsedGameTime.Milliseconds);
				}
			}
		}

		public static void TemporarySpritesTemporaryAnimatedSpriteRotationChangeChangeUpdate()
		{
			if (TemporarySpritesTemporaryAnimatedSpriteRotationChangeChange is not null)
			{
				foreach (KeyValuePair<TemporaryAnimatedSprite, float> entry in TemporarySpritesTemporaryAnimatedSpriteRotationChangeChange)
				{
					TemporaryAnimatedSprite sprite = entry.Key;
					float rotation_change_change = entry.Value;

					sprite.rotationChange += rotation_change_change * ((!sprite.timeBasedMotion) ? 1 : Game1.currentGameTime.ElapsedGameTime.Milliseconds);
				}
			}
		}

		public static void TemporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZeroUpdate()
		{
			if (TemporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero is not null)
			{
				TemporaryAnimatedSpriteList spritesToRemove = new();

				foreach (KeyValuePair<TemporaryAnimatedSprite, bool> entry in TemporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero)
				{
					TemporaryAnimatedSprite sprite = entry.Key;
					bool stop_rotating_when_velocity_is_zero = entry.Value;

					if (TemporarySpritesTemporaryAnimatedSpriteRotationChangeChange.ContainsKey(sprite))
					{
						float value1 = sprite.rotationChange;
						float value2 = sprite.rotationChange + TemporarySpritesTemporaryAnimatedSpriteRotationChangeChange[sprite] * ((!sprite.timeBasedMotion) ? 1 : Game1.currentGameTime.ElapsedGameTime.Milliseconds);

						if (stop_rotating_when_velocity_is_zero && Math.Sign(value1) != Math.Sign(value2))
						{
							sprite.rotationChange = 0f;
							TemporarySpritesTemporaryAnimatedSpriteRotationChangeChange.Remove(sprite);
							spritesToRemove.Add(sprite);
						}
					}
				}
				foreach (TemporaryAnimatedSprite sprite in spritesToRemove)
				{
					TemporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZero.Remove(sprite);
				}
			}
		}

		public static void TemporarySpritesTemporaryAnimatedSpriteSwayUpdate()
		{
			if (TemporarySpritesTemporaryAnimatedSpriteSway is not null)
			{
				foreach (KeyValuePair<TemporaryAnimatedSprite, (Vector2, Vector2, Vector2, Vector2, Func<double, double>, Func<double, double>, string, float)> entry in TemporarySpritesTemporaryAnimatedSpriteSway)
				{
					TemporaryAnimatedSprite sprite = entry.Key;
					Vector2 startingVector = entry.Value.Item1;
					Vector2 endingVector = entry.Value.Item2;
					Vector2 multiplierVector = entry.Value.Item3;
					Vector2 speedVector = entry.Value.Item4;
					Func<double, double> functionX = entry.Value.Item5;
					Func<double, double> functionY = entry.Value.Item6;
					string axis = entry.Value.Item7;
					float swayOffset = entry.Value.Item8;
					float lerpX = axis.Equals("x") || axis.Equals("xy") ? Math.Clamp((sprite.position.X - startingVector.X) / (endingVector.X - startingVector.X), 0, 1) : 0;
					float lerpY = axis.Equals("y") || axis.Equals("xy") ? Math.Clamp((sprite.position.Y - startingVector.Y) / (endingVector.Y - startingVector.Y), 0, 1) : 0;
					float lerp = axis.Equals("x") ? lerpX : axis.Equals("y") ? lerpY : (lerpX + lerpY) / 2f;
					float x = (float)functionX(Game1.currentGameTime.TotalGameTime.TotalSeconds * speedVector.X + (double)swayOffset) * lerp * multiplierVector.X * Game1.pixelZoom;
					float y = (float)functionY(Game1.currentGameTime.TotalGameTime.TotalSeconds * speedVector.Y + (double)swayOffset) * lerp * multiplierVector.Y * Game1.pixelZoom;

					sprite.position += new Vector2(x, y);
				}
			}
		}

		public static void TemporarySpritesTemporaryAnimatedSpriteFrameSoundUpdate()
		{
			if (TemporarySpritesTemporaryAnimatedSpriteFrameSound is not null)
			{
				for (int i = 0; i < TemporarySpritesTemporaryAnimatedSpriteFrameSound.Count; i++)
				{
					TemporaryAnimatedSprite sprite = TemporarySpritesTemporaryAnimatedSpriteFrameSound[i].Item1;
					string sound = TemporarySpritesTemporaryAnimatedSpriteFrameSound[i].Item2;
					int index = TemporarySpritesTemporaryAnimatedSpriteFrameSound[i].Item3;

					if (sprite.timer == 0 && sprite.currentParentTileIndex == index)
					{
						Game1.playSound(sound);
					}
				}
			}
		}

		public static void TemporarySpritesChangeTemporaryAnimatedSpriteUpdate()
		{
			if (TemporarySpritesChangeTemporaryAnimatedSprite is not null)
			{
				for (int i = 0; i < TemporarySpritesChangeTemporaryAnimatedSprite.Count; i++)
				{
					TemporaryAnimatedSprite temporaryAnimatedSpriteToChange = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item1;
					TemporaryAnimatedSprite temporaryAnimatedSpriteToCompare = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item2;
					string operatorX = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item3;
					float x = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item4 * Game1.tileSize;
					string operatorY = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item5;
					float y = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item6 * Game1.tileSize;
					int delay = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item7;
					bool conditions = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item8;
					Event @event = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item9;
					string[] args = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item10;
					EventContext context = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item11;
					int start_index = TemporarySpritesChangeTemporaryAnimatedSprite[i].Item12;

					if (!conditions)
					{
						bool conditionX = true;
						bool conditionY = true;

						if (operatorX.Equals("<") && !(temporaryAnimatedSpriteToCompare.position.X < x))
						{
							conditionX = false;
						}
						else if (operatorX.Equals("<=") && !(temporaryAnimatedSpriteToCompare.position.X <= x))
						{
							conditionX = false;
						}
						else if (operatorX.Equals("!=") && !(temporaryAnimatedSpriteToCompare.position.X != x))
						{
							conditionX = false;
						}
						else if (operatorX.Equals("==") && !(temporaryAnimatedSpriteToCompare.position.X == x))
						{
							conditionX = false;
						}
						else if (operatorX.Equals(">") && !(temporaryAnimatedSpriteToCompare.position.X > x))
						{
							conditionX = false;
						}
						else if (operatorX.Equals(">=") && !(temporaryAnimatedSpriteToCompare.position.X >= x))
						{
							conditionX = false;
						}
						if (operatorY.Equals("<") && !(temporaryAnimatedSpriteToCompare.position.Y < y))
						{
							conditionY = false;
						}
						else if (operatorY.Equals("<=") && !(temporaryAnimatedSpriteToCompare.position.Y <= y))
						{
							conditionY = false;
						}
						else if (operatorY.Equals("!=") && !(temporaryAnimatedSpriteToCompare.position.Y != y))
						{
							conditionY = false;
						}
						else if (operatorY.Equals("==") && !(temporaryAnimatedSpriteToCompare.position.Y == y))
						{
							conditionY = false;
						}
						else if (operatorY.Equals(">") && !(temporaryAnimatedSpriteToCompare.position.Y > y))
						{
							conditionY = false;
						}
						else if (operatorY.Equals(">=") && !(temporaryAnimatedSpriteToCompare.position.Y >= y))
						{
							conditionY = false;
						}
						conditions = conditionX && conditionY;
					}

					if (conditions)
					{
						delay -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
						if (delay <= 0)
						{
							SetTemporaryAnimatedSpriteFlags(@event, args, context, start_index, temporaryAnimatedSpriteToChange);
							TemporarySpritesChangeTemporaryAnimatedSprite.RemoveAt(i--);
						}
						else
						{
							TemporarySpritesChangeTemporaryAnimatedSprite[i] = (temporaryAnimatedSpriteToChange, temporaryAnimatedSpriteToCompare, operatorX, x, operatorY, y, delay, conditions, @event, args, context, start_index);
						}
					}
				}
			}
		}

		private static void RemoveTemporarySprites(Event @event, string[] args, EventContext context)
		{
			Rectangle sourceRect = Rectangle.Empty;

			if (!ArgUtility.TryGetOptional(args, 1, out string textureName, out string error, null) || args.Length > 2 && !ArgUtility.TryGetRectangle(args, 2, out sourceRect, out error))
			{
				context.LogErrorAndSkip(error);
				return;
			}

			TemporaryAnimatedSpriteList spritesToRemove = new();
			string LanguageString = LocalizedContentManager.CurrentLanguageString;
			string InternalPrefix = $"SMAPI/{SEvent.Station.ContentPack.Manifest.UniqueID.ToLower()}/";
			string textureNameNormalized = textureName?.Replace('\\', '/');

			if (!string.IsNullOrEmpty(LanguageString))
			{
				textureNameNormalized = textureNameNormalized?.Replace($".{LanguageString}", string.Empty);
			}
			foreach (TemporaryAnimatedSprite sprite in TemporarySprites)
			{
				string spriteTextureNameNormalized = sprite?.textureName?.Replace('\\', '/');

				if (!string.IsNullOrEmpty(LanguageString))
				{
					spriteTextureNameNormalized = spriteTextureNameNormalized?.Replace($".{LanguageString}", string.Empty);
				}
				if (string.IsNullOrWhiteSpace(textureName) || (spriteTextureNameNormalized is not null && (spriteTextureNameNormalized.Equals(textureNameNormalized) || spriteTextureNameNormalized.Equals($"{InternalPrefix}{textureNameNormalized}"))))
				{
					if (sourceRect.IsEmpty || sprite.sourceRect == sourceRect)
					{
						spritesToRemove.Add(sprite);
					}
				}
			}
			foreach (TemporaryAnimatedSprite sprite in spritesToRemove)
			{
				context.Location.TemporarySprites.Remove(sprite);
				TemporarySprites.Remove(sprite);
			}
			@event.CurrentCommand++;
		}

		private static void RemoveTemporarySpritesRange(Event @event, string[] args, EventContext context)
		{
			if (!ArgUtility.TryGetOptionalInt(args, 1, out int index, out string error, -1) || !ArgUtility.TryGetOptionalInt(args, 2, out int range, out error, 1))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (index < 0)
			{
				index += TemporarySprites.Count;
			}
			if (TemporarySprites.Count == 0)
			{
				context.LogErrorAndSkip($"temporary sprite list is empty");
				return;
			}
			if (!(0 <= index && index < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {index} is out of range");
				return;
			}
			if (!(0 <= index + range - 1 && index + range - 1 < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {index + range - 1} is out of range");
				return;
			}

			TemporaryAnimatedSpriteList spritesToRemove = new();

			for (int i = 0; i < range; i++)
			{
				spritesToRemove.Add(TemporarySprites[index + i]);
			}
			foreach (TemporaryAnimatedSprite sprite in spritesToRemove)
			{
				context.Location.TemporarySprites.Remove(sprite);
				TemporarySprites.Remove(sprite);
			}
			@event.CurrentCommand++;
		}

		private static void DestroyObjectsOnCollision(Event @event, string[] args, EventContext context)
		{
			if (!ArgUtility.TryGetOptionalInt(args, 1, out int index, out string error, -1))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (index < 0)
			{
				index += TemporarySprites.Count;
			}
			if (TemporarySprites.Count == 0)
			{
				context.LogErrorAndSkip($"temporary sprite list is empty");
				return;
			}
			if (!(0 <= index && index < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {index} is out of range");
				return;
			}
			if (!ArgUtility.TryGetOptionalFloat(args, 2, out float x, out error, 0f) || !ArgUtility.TryGetOptionalFloat(args, 3, out float y, out error, 0f) || !ArgUtility.TryGetOptionalFloat(args, 4, out float width, out error, TemporarySprites[index].sourceRect.Width * Game1.pixelZoom / Game1.tileSize) || !ArgUtility.TryGetOptionalFloat(args, 5, out float height, out error, TemporarySprites[index].sourceRect.Height * Game1.pixelZoom / Game1.tileSize) || !ArgUtility.TryGetOptionalBool(args, 6, out bool showDestroyedObject, out error, true) || !ArgUtility.TryGetOptionalBool(args, 7, out bool playSound, out error, true))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			TemporarySpritesDestroyObjectsOnCollision.Add((TemporarySprites[index], new Rectangle((int)(x * Game1.tileSize), (int)(y * Game1.tileSize), (int)(width * Game1.tileSize), (int)(height * Game1.tileSize)), showDestroyedObject, playSound));
			@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporarySpritesDestroyObjectsOnCollision.Clear) != true ? TemporarySpritesDestroyObjectsOnCollision.Clear : null;
			@event.CurrentCommand++;
		}

		private static void PreserveObjectsOnCollision(Event @event, string[] args, EventContext context)
		{
			if (!ArgUtility.TryGetOptionalInt(args, 1, out int index, out string error, -1))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (index < 0)
			{
				index += TemporarySprites.Count;
			}
			if (TemporarySprites.Count == 0)
			{
				context.LogErrorAndSkip($"temporary sprite list is empty");
				return;
			}
			if (!(0 <= index && index < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {index} is out of range");
				return;
			}
			for (int i = 0; i < TemporarySpritesDestroyObjectsOnCollision.Count; i++)
			{
				if (TemporarySpritesDestroyObjectsOnCollision[i].Item1.Equals(TemporarySprites[index]))
				{
					TemporarySpritesDestroyObjectsOnCollision.RemoveAt(i--);
				}
			}
			@event.CurrentCommand++;
		}

		public static void TemporarySpritesDestroyObjectsOnCollisionUpdate(GameLocation location)
		{
			if (TemporarySpritesDestroyObjectsOnCollision is not null && ModEntry.Config.AllowObjectDestruction)
			{
				for (int i = 0; i < TemporarySpritesDestroyObjectsOnCollision.Count; i++)
				{
					TemporaryAnimatedSprite sprite = TemporarySpritesDestroyObjectsOnCollision[i].Item1;
					Rectangle collisionBox = TemporarySpritesDestroyObjectsOnCollision[i].Item2;
					bool showDestroyedObject = TemporarySpritesDestroyObjectsOnCollision[i].Item3;
					bool playSound = TemporarySpritesDestroyObjectsOnCollision[i].Item4;
					Rectangle absoluteCollisionBox = new((int)sprite.Position.X + (int)(collisionBox.X * sprite.scale / Game1.pixelZoom), (int)sprite.Position.Y + (int)(collisionBox.Y * sprite.scale / Game1.pixelZoom), (int)(collisionBox.Width * sprite.scale / Game1.pixelZoom), (int)(collisionBox.Height * sprite.scale / Game1.pixelZoom));

					foreach (StardewValley.Object @object in location.Objects.Values.ToArray())
					{
						if (@object is not null && !@object.isPassable())
						{
							Rectangle objectRect = new((int)(@object.TileLocation.X * Game1.tileSize), (int)(@object.TileLocation.Y * Game1.tileSize), Game1.tileSize, Game1.tileSize);

							if (absoluteCollisionBox.Intersects(objectRect))
							{
								location.characterDestroyObjectWithinRectangle(objectRect, showDestroyedObject: showDestroyedObject);
								if (playSound && @object is not Chest)
								{
									Game1.sounds.PlayLocal("boulderBreak", location, @object.TileLocation, null, SoundContext.Default, out _);
								}
							}
						}
					}
				}
			}
		}

		private static void TemporaryFarmerSprite(Event @event, string[] args, EventContext context)
		{
			if (args.Length < 8)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected mandatory fields [frameIndex, sourceRect, position] followed by optional fields [facingDirection, flipped, hideSecondaryArm, hideArms, layerDepth, color, scale, rotation, offset, spriteIndex]");
				return;
			}

			Color color = Color.White;
			Vector2 offset = Vector2.Zero;

			if (!ArgUtility.TryGetInt(args, 1, out int frameIndex, out string error) || !ArgUtility.TryGetRectangle(args, 2, out Rectangle sourceRect, out error) || !ArgUtility.TryGetVector2(args, 6, out Vector2 position, out error, integerOnly: false) || !ArgUtility.TryGetOptionalDirection(args, 8, out int facingDirection, out error, Game1.player.FacingDirection) || !ArgUtility.TryGetOptionalBool(args, 9, out bool flipped, out error, false) || !ArgUtility.TryGetOptionalBool(args, 10, out bool hideSecondaryArm, out error, true) || !ArgUtility.TryGetOptionalBool(args, 11, out bool hideArms, out error, false) || !ArgUtility.TryGetOptionalFloat(args, 12, out float layerDepth, out error, -1f) || args.Length > 13 && !TryGetColor(args, 13, out color, out error) || !ArgUtility.TryGetOptionalFloat(args, 14, out float scale, out error, 1f) || !ArgUtility.TryGetOptionalFloat(args, 15, out float rotation, out error, 0f) || args.Length > 16 && !ArgUtility.TryGetVector2(args, 16, out offset, out error, integerOnly: true) || !ArgUtility.TryGetOptionalInt(args, 18, out int spriteIndex, out error, -1))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			position = @event.OffsetPosition(position * Game1.tileSize);
			if (layerDepth == -1)
			{
				layerDepth = position.Y + (sourceRect.Y * scale * Game1.pixelZoom / Game1.tileSize);
			}
			layerDepth = @event.OffsetPosition(new Vector2(0f, layerDepth) * Game1.tileSize).Y / 10000f;
			if (spriteIndex < 0)
			{
				spriteIndex += TemporarySprites.Count;
			}
			if (TemporarySprites.Count == 0)
			{
				context.LogErrorAndSkip($"temporary sprite list is empty");
				return;
			}
			if (!(0 <= spriteIndex && spriteIndex < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {spriteIndex} is out of range");
				return;
			}
			Game1.player.faceDirection(facingDirection);
			TemporaryFarmerSprites.Add((new FarmerSprite.AnimationFrame(frameIndex, 99999, (int)offset.Y, !hideSecondaryArm, flipped, null, null, (int)offset.X, hideArms), frameIndex, sourceRect, position, layerDepth, color, rotation, scale, TemporarySprites[spriteIndex], TemporarySprites[spriteIndex].Position));
			@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(TemporaryFarmerSprites.Clear) != true ? TemporaryFarmerSprites.Clear : null;
			@event.CurrentCommand++;
		}

		public static void TemporaryFarmerSpritesDraw(SpriteBatch b)
		{
			if (TemporaryFarmerSprites is not null)
			{
				for (int i = 0; i < TemporaryFarmerSprites.Count; i++)
				{
					FarmerSprite.AnimationFrame farmerSprite = TemporaryFarmerSprites[i].Item1;
					int frameIndex = TemporaryFarmerSprites[i].Item2;
					Rectangle sourceRect = TemporaryFarmerSprites[i].Item3;
					Vector2 position = TemporaryFarmerSprites[i].Item4;
					float layerDepth = TemporaryFarmerSprites[i].Item5;
					Color color = TemporaryFarmerSprites[i].Item6;
					float rotation = TemporaryFarmerSprites[i].Item7;
					float scale = TemporaryFarmerSprites[i].Item8;
					TemporaryAnimatedSprite sprite = TemporaryFarmerSprites[i].Item9;
					Vector2 oldSpritePosition = TemporaryFarmerSprites[i].Item10;

					position += sprite.Position - oldSpritePosition;
					TemporaryFarmerSprites[i] = (farmerSprite, frameIndex, sourceRect, position, layerDepth, color, rotation, scale, sprite, sprite.Position);
					Game1.player.FarmerRenderer.draw(b, farmerSprite, frameIndex, sourceRect, Game1.GlobalToLocal(position), Vector2.Zero, layerDepth, color, rotation, scale, Game1.player);
				}
			}
		}

		private static void RemoveTemporaryFarmerSprites(Event @event, string[] args, EventContext context)
		{
			Rectangle sourceRect = Rectangle.Empty;

			if (!ArgUtility.TryGetOptionalInt(args, 1, out int frameIndex, out string error, -1) || args.Length > 2 && !ArgUtility.TryGetRectangle(args, 2, out sourceRect, out error))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			for (int i = 0; i < TemporaryFarmerSprites.Count; i++)
			{
				if (frameIndex == -1 || TemporaryFarmerSprites[i].Item2 == frameIndex)
				{
					if (sourceRect.IsEmpty || TemporaryFarmerSprites[i].Item3 == sourceRect)
					{
						TemporaryFarmerSprites.RemoveAt(i--);
					}
				}
			}
			@event.CurrentCommand++;
		}

		private static void RemoveTemporaryFarmerSpritesRange(Event @event, string[] args, EventContext context)
		{
			if (!ArgUtility.TryGetOptionalInt(args, 1, out int index, out string error, -1) || !ArgUtility.TryGetOptionalInt(args, 2, out int range, out error, 1))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (index < 0)
			{
				index += TemporaryFarmerSprites.Count;
			}
			if (TemporaryFarmerSprites.Count == 0)
			{
				context.LogErrorAndSkip($"temporary sprite list is empty");
				return;
			}
			if (!(0 <= index && index < TemporaryFarmerSprites.Count))
			{
				context.LogErrorAndSkip($"index {index} is out of range");
				return;
			}
			if (!(0 <= index + range - 1 && index + range - 1 < TemporaryFarmerSprites.Count))
			{
				context.LogErrorAndSkip($"index {index + range - 1} is out of range");
				return;
			}
			TemporaryFarmerSprites.RemoveRange(index, range);
			@event.CurrentCommand++;
		}

		private static bool TryGetColor(string[] array, int index, out Color value, out string error)
		{
			if (!ArgUtility.TryGet(array, index, out string colorAsString, out error))
			{
				value = Color.White;
				return false;
			}

			Color? color = Utility.StringToColor(colorAsString);

			if (color.HasValue)
			{
				value = color.Value;
				return true;
			}
			else
			{
				value = Color.White;
				error = $"index {index} has value '{colorAsString}', which can't be parsed as a color";
				return false;
			}
		}

		// Sounds
		private static void PlaySound(Event @event, string[] args, EventContext context)
		{
			Vector2 position = new(float.MinValue, float.MinValue);

			if (args.Length < 2)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected a mandatory [name] field followed by optional fields [volume, volumeChange, pitch, pitchChange, pitchMin, pitchMax, loopPitch, position, soundContext]");
				return;
			}
			if (!ArgUtility.TryGet(args, 1, out string name, out string error) || !ArgUtility.TryGetOptionalFloat(args, 2, out float volume, out error, 100f) || !ArgUtility.TryGetOptionalFloat(args, 3, out float volumeChange, out error, 0f) || !ArgUtility.TryGetOptionalInt(args, 4, out int pitch, out error, 1200) || !ArgUtility.TryGetOptionalInt(args, 5, out int pitchChange, out error, 0) || !ArgUtility.TryGetOptionalInt(args, 6, out int pitchMin, out error, 0) || !ArgUtility.TryGetOptionalInt(args, 7, out int pitchMax, out error, 2400) || !ArgUtility.TryGetOptionalBool(args, 8, out bool loopPitch, out error, true) || args.Length > 9 && !ArgUtility.TryGetVector2(args, 9, out position, out error) || !ArgUtility.TryGetOptionalEnum(args, 11, out SoundContext soundContext, out error, SoundContext.Default))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			Game1.sounds.PlayLocal(name, context.Location, position != new Vector2(float.MinValue, float.MinValue) ? position : null, pitch, soundContext, out ICue cue);
			cue.SetVariable("Volume", volume);
			if (volumeChange != 0f)
			{
				SoundsVolumeChange.Add(cue, volumeChange);
				@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(SoundsVolumeChange.Clear) != true ? SoundsVolumeChange.Clear : null;
			}
			if (pitchChange != 0f)
			{
				SoundsPitchChange.Add(cue, (pitchChange, pitchMin, pitchMax, loopPitch));
				@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(SoundsPitchChange.Clear) != true ? SoundsPitchChange.Clear : null;
			}
			Sounds.Add(cue);
			@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(SoundsClear) != true ? SoundsClear : null;
			@event.CurrentCommand++;
		}

		public static void SoundsVolumeChangeUpdate()
		{
			if (SoundsVolumeChange is not null)
			{
				foreach (KeyValuePair<ICue, float> entry in SoundsVolumeChange)
				{
					ICue cue = entry.Key;
					float volumeChange = entry.Value;
					float newVolume = cue.GetVariable("Volume") + volumeChange;

					cue.SetVariable("Volume", newVolume);
				}
			}
		}

		public static void SoundsPitchChangeUpdate()
		{
			if (SoundsPitchChange is not null)
			{
				foreach (KeyValuePair<ICue, (int, int, int, bool)> entry in SoundsPitchChange)
				{
					ICue cue = entry.Key;
					int pitchChange = entry.Value.Item1;
					int pitchMin = entry.Value.Item2;
					int pitchMax = entry.Value.Item3;
					bool loopPitch = entry.Value.Item4;
					float newPitch = cue.GetVariable("Pitch") + pitchChange;

					if (newPitch < pitchMin || pitchMax < newPitch)
					{
						if (loopPitch)
						{
							newPitch = pitchMin + Math.Abs((newPitch - pitchMin) % (pitchMax - pitchMin + 1));
						}
						else
						{
							newPitch = (newPitch < pitchMin) ? pitchMin : pitchMax;
						}
					}
					cue.SetVariable("Pitch", newPitch);
				}
			}
		}

		private static void ChangeSound(Event @event, string[] args, EventContext context)
		{
			if (args.Length < 2)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected a mandatory [index] field followed by optional fields [volume, volumeChange, pitch, pitchChange, pitchMin, pitchMax, loopPitch]");
				return;
			}
			if (!ArgUtility.TryGetInt(args, 1, out int index, out string error) || !ArgUtility.TryGetOptional(args, 2, out string volumeString, out error, null) || !ArgUtility.TryGetOptionalFloat(args, 2, out float volume, out error, 100f) && volumeString != "none" || !ArgUtility.TryGetOptional(args, 3, out string volumeChangeString, out error, null) || !ArgUtility.TryGetOptionalFloat(args, 3, out float volumeChange, out error, 0f) && volumeChangeString != "none" || !ArgUtility.TryGetOptional(args, 4, out string pitchString, out error, null) || !ArgUtility.TryGetOptionalInt(args, 4, out int pitch, out error, 1200) && pitchString != "none" || !ArgUtility.TryGetOptional(args, 5, out string pitchChangeString, out error, null) || !ArgUtility.TryGetOptionalInt(args, 5, out int pitchChange, out error, 0) && pitchChangeString != "none" || !ArgUtility.TryGetOptional(args, 6, out string pitchMinString, out error, null) || !ArgUtility.TryGetOptionalInt(args, 6, out int pitchMin, out error, 0) && pitchMinString != "none" || !ArgUtility.TryGetOptional(args, 7, out string pitchMaxString, out error, null) || !ArgUtility.TryGetOptionalInt(args, 7, out int pitchMax, out error, 2400) && pitchMaxString != "none" || !ArgUtility.TryGetOptional(args, 8, out string loopPitchString, out error, null) || !ArgUtility.TryGetOptionalBool(args, 8, out bool loopPitch, out error, true) && loopPitchString != "none")
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (index < 0)
			{
				index += Sounds.Count;
			}
			if (Sounds.Count == 0)
			{
				context.LogErrorAndSkip($"sound list is empty");
				return;
			}
			if (!(0 <= index && index < Sounds.Count))
			{
				context.LogErrorAndSkip($"index {index} is out of range");
				return;
			}
			if (volumeString != "none")
			{
				Sounds[index].SetVariable("Volume", volume);
			}
			if (volumeChangeString != "none" && SoundsVolumeChange.ContainsKey(Sounds[index]))
			{
				SoundsVolumeChange[Sounds[index]] = volumeChange;
			}
			if (pitchString != "none")
			{
				Sounds[index].SetVariable("Pitch", pitch);
			}
			if (SoundsPitchChange.ContainsKey(Sounds[index]))
			{
				int newPitchChange = pitchChangeString != "none" ? pitchChange : SoundsPitchChange[Sounds[index]].Item1;
				int newPitchMin = pitchMinString != "none" ? pitchMin : SoundsPitchChange[Sounds[index]].Item2;
				int newPitchMax = pitchMaxString != "none" ? pitchMax : SoundsPitchChange[Sounds[index]].Item3;
				bool newLoopPitch = loopPitchString != "none" ? loopPitch : SoundsPitchChange[Sounds[index]].Item4;

				SoundsPitchChange[Sounds[index]] = (newPitchChange, newPitchMin, newPitchMax, newLoopPitch);
			}
			@event.CurrentCommand++;
		}

		private static void StopSounds(Event @event, string[] args, EventContext context)
		{
			if (!ArgUtility.TryGetOptional(args, 1, out string name, out string error, null) || !ArgUtility.TryGetOptionalEnum(args, 2, out AudioStopOptions audioStopOptions, out error, AudioStopOptions.Immediate))
			{
				context.LogErrorAndSkip(error);
				return;
			}

			List<ICue> soundsToRemove = new();

			foreach (ICue cue in Sounds)
			{
				if (string.IsNullOrWhiteSpace(name) || (cue.Name is not null && cue.Name.Equals(name)))
				{
					soundsToRemove.Add(cue);
				}
			}
			foreach (ICue cue in soundsToRemove)
			{
				cue.Stop(audioStopOptions);
				Sounds.Remove(cue);
			}
			@event.CurrentCommand++;
		}

		private static void StopSoundsRange(Event @event, string[] args, EventContext context)
		{
			if (!ArgUtility.TryGetOptionalInt(args, 1, out int index, out string error, -1) || !ArgUtility.TryGetOptionalInt(args, 2, out int range, out error, 1) || !ArgUtility.TryGetOptionalEnum(args, 3, out AudioStopOptions audioStopOptions, out error, AudioStopOptions.Immediate))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (index < 0)
			{
				index += Sounds.Count;
			}
			if (Sounds.Count == 0)
			{
				context.LogErrorAndSkip($"sound list is empty");
				return;
			}
			if (!(0 <= index && index < Sounds.Count))
			{
				context.LogErrorAndSkip($"index {index} is out of range");
				return;
			}
			if (!(0 <= index + range - 1 && index + range - 1 < Sounds.Count))
			{
				context.LogErrorAndSkip($"index {index + range - 1} is out of range");
				return;
			}

			List<ICue> soundsToRemove = new();

			for (int i = 0; i < range; i++)
			{
				soundsToRemove.Add(Sounds[index + i]);
			}
			foreach (ICue cue in soundsToRemove)
			{
				cue.Stop(audioStopOptions);
				Sounds.Remove(cue);
			}
			@event.CurrentCommand++;
		}

		private static void SoundsClear()
		{
			foreach (ICue cue in Sounds)
			{
				cue.Stop(AudioStopOptions.Immediate);
			}
			Sounds.Clear();
		}

		// Control flow
		private static void Query(Event @event, string[] args, EventContext context)
		{
			if (args.Length < 3)
			{
				context.LogErrorAndSkip("invalid number of arguments. Expected mandatory fields [query, n1] followed by an optional [n2] field");
				return;
			}
			if (!ArgUtility.TryGet(args, 1, out string query, out string error) || !ArgUtility.TryGetInt(args, 2, out int n1, out error) || !ArgUtility.TryGetOptionalInt(args, 3, out int n2, out error, 0))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (GameStateQuery.CheckConditions(query))
			{
				if (n2 > 0)
				{
					List<string> eventCommands = @event.eventCommands.ToList();

					eventCommands.RemoveRange(@event.currentCommand + n1 + 1, n2);
					@event.eventCommands = eventCommands.ToArray();
				}
			}
			else
			{
				@event.currentCommand += n1;
			}
			@event.CurrentCommand++;
		}

		// Location specific commands
		private static void LocationSpecificCommand_draw_ParrotExpress_lines(Event @event, string[] args, EventContext context)
		{
			if (!ArgUtility.TryGetOptionalInt(args, 1, out int platformIndex, out string error, -3) || !ArgUtility.TryGetOptionalInt(args, 2, out int parrotLeftIndex, out error, -2) || !ArgUtility.TryGetOptionalInt(args, 3, out int parrotRightIndex, out error, -1))
			{
				context.LogErrorAndSkip(error);
				return;
			}
			if (platformIndex < 0)
			{
				platformIndex += TemporarySprites.Count;
			}
			if (parrotLeftIndex < 0)
			{
				parrotLeftIndex += TemporarySprites.Count;
			}
			if (parrotRightIndex < 0)
			{
				parrotRightIndex += TemporarySprites.Count;
			}
			if (TemporarySprites.Count == 0)
			{
				context.LogErrorAndSkip($"temporary sprite list is empty");
				return;
			}
			if (!(platformIndex < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {platformIndex} is out of range");
				return;
			}
			if (!(parrotLeftIndex < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {parrotLeftIndex} is out of range");
				return;
			}
			if (!(parrotRightIndex < TemporarySprites.Count))
			{
				context.LogErrorAndSkip($"index {parrotRightIndex} is out of range");
				return;
			}
			LocationSpecificCommandDrawParrotExpressLines = (
			new TemporaryAnimatedSpriteList()
			{
				TemporarySprites[platformIndex],
				TemporarySprites[parrotLeftIndex],
				TemporarySprites[parrotRightIndex],
			},
			new TemporaryAnimatedSpriteList()
			{
				new("LooseSprites\\ParrotPlatform", new Rectangle(22, 74, 4, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, false, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0),
				new("LooseSprites\\ParrotPlatform", new Rectangle(0, 73, 16, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, false, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0),
				new("LooseSprites\\ParrotPlatform", new Rectangle(0, 73, 16, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, false, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0),
				new("LooseSprites\\ParrotPlatform", new Rectangle(0, 73, 16, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, false, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0),
				new("LooseSprites\\ParrotPlatform", new Rectangle(0, 73, 16, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, false, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0)
			},
			new TemporaryAnimatedSpriteList()
			{
				new("LooseSprites\\ParrotPlatform", new Rectangle(22, 74, 4, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, true, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0),
				new("LooseSprites\\ParrotPlatform", new Rectangle(0, 73, 16, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, true, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0),
				new("LooseSprites\\ParrotPlatform", new Rectangle(0, 73, 16, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, true, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0),
				new("LooseSprites\\ParrotPlatform", new Rectangle(0, 73, 16, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, true, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0),
				new("LooseSprites\\ParrotPlatform", new Rectangle(0, 73, 16, 4), 999999, 1, 1, new Vector2(float.MinValue, float.MinValue), false, true, 0f, 0, Color.White, Game1.pixelZoom, 0, 0, 0)
			});
			context.Location.temporarySprites.AddRange(LocationSpecificCommandDrawParrotExpressLines.Item2);
			context.Location.temporarySprites.AddRange(LocationSpecificCommandDrawParrotExpressLines.Item3);
			TemporarySprites.AddRange(LocationSpecificCommandDrawParrotExpressLines.Item2);
			TemporarySprites.AddRange(LocationSpecificCommandDrawParrotExpressLines.Item3);
			@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(LocationSpecificCommandDrawParrotExpressLinesClear) != true ? LocationSpecificCommandDrawParrotExpressLinesClear : null;
			@event.CurrentCommand++;
		}

		public static void LocationSpecificCommandDrawParrotExpressLinesUpdate()
		{
			if (LocationSpecificCommandDrawParrotExpressLines.Item1 is not null)
			{
				const int pointSpriteSize = 16;
				const int pointCount = 4;
				float layerDepth = (LocationSpecificCommandDrawParrotExpressLines.Item1[0].position.Y + 0.05f) / 10000f + 0.01f;
				Vector2 startPoint1 = LocationSpecificCommandDrawParrotExpressLines.Item1[0].position + new Vector2(57, 0);
				Vector2 startPoint2 = LocationSpecificCommandDrawParrotExpressLines.Item1[0].position + new Vector2(134, 0);
				Vector2 endPoint1 = LocationSpecificCommandDrawParrotExpressLines.Item1[1].position + new Vector2(57, 66);
				Vector2 endPoint2 = LocationSpecificCommandDrawParrotExpressLines.Item1[2].position + new Vector2(38, 66);
				Vector2 direction1 = Vector2.Normalize(endPoint1 - startPoint1);
				Vector2 direction2 = Vector2.Normalize(endPoint2 - startPoint2);
				float distance1 = Vector2.Distance(startPoint1, endPoint1);
				float distance2 = Vector2.Distance(startPoint2, endPoint2);
				float scale1 = distance1 / pointSpriteSize / Game1.pixelZoom;
				float scale2 = distance2 / pointSpriteSize / Game1.pixelZoom;
				float rotation1 = MathF.Atan2(direction1.Y, direction1.X);
				float rotation2 = MathF.Atan2(direction2.Y, direction2.X);
				Vector2 offset1 = (new Vector2(-MathHelper.Lerp(0, pointSpriteSize, (float)Math.Clamp(direction1.X + 0.5, 0, 1)), -MathHelper.Lerp(pointSpriteSize, 0, (float)Math.Clamp(-direction1.Y + 0.5, 0, 1))) + new Vector2(-pointSpriteSize / 8, -pointSpriteSize / 2)) * scale1;
				Vector2 offset2 = (new Vector2(-MathHelper.Lerp(0, pointSpriteSize, (float)Math.Clamp(direction2.X + 0.5, 0, 1)), -MathHelper.Lerp(pointSpriteSize, 0, (float)Math.Clamp(-direction2.Y + 0.5, 0, 1))) + new Vector2(pointSpriteSize / 8, -pointSpriteSize / 2)) * scale2;

				LocationSpecificCommandDrawParrotExpressLines.Item2[0].position = startPoint1 + new Vector2(-pointSpriteSize / 2, -pointSpriteSize / 2) + new Vector2(1, 0);
				LocationSpecificCommandDrawParrotExpressLines.Item2[0].layerDepth = layerDepth;
				LocationSpecificCommandDrawParrotExpressLines.Item2[0].drawAboveAlwaysFront = LocationSpecificCommandDrawParrotExpressLines.Item1[1].drawAboveAlwaysFront;
				LocationSpecificCommandDrawParrotExpressLines.Item3[0].position = startPoint2 + new Vector2(-pointSpriteSize / 2, -pointSpriteSize / 2);
				LocationSpecificCommandDrawParrotExpressLines.Item3[0].layerDepth = layerDepth;
				LocationSpecificCommandDrawParrotExpressLines.Item3[0].drawAboveAlwaysFront = LocationSpecificCommandDrawParrotExpressLines.Item1[2].drawAboveAlwaysFront;
				for (int i = 1; i <= pointCount; i++)
				{
					LocationSpecificCommandDrawParrotExpressLines.Item2[i].position = startPoint1 + direction1 * ((i - 1) * (distance1 / pointCount)) + offset1;
					LocationSpecificCommandDrawParrotExpressLines.Item2[i].rotation = rotation1;
					LocationSpecificCommandDrawParrotExpressLines.Item2[i].scale = scale1;
					LocationSpecificCommandDrawParrotExpressLines.Item2[i].layerDepth = layerDepth + i * 1E-06f;
					LocationSpecificCommandDrawParrotExpressLines.Item2[i].drawAboveAlwaysFront = LocationSpecificCommandDrawParrotExpressLines.Item1[1].drawAboveAlwaysFront;
					LocationSpecificCommandDrawParrotExpressLines.Item3[i].position = startPoint2 + direction2 * ((i - 1) * (distance2 / pointCount)) + offset2;
					LocationSpecificCommandDrawParrotExpressLines.Item3[i].rotation = rotation2;
					LocationSpecificCommandDrawParrotExpressLines.Item3[i].scale = scale2;
					LocationSpecificCommandDrawParrotExpressLines.Item3[i].layerDepth = layerDepth + i * 1E-06f;
					LocationSpecificCommandDrawParrotExpressLines.Item3[i].drawAboveAlwaysFront = LocationSpecificCommandDrawParrotExpressLines.Item1[2].drawAboveAlwaysFront;
				}
			}
		}

		private static void LocationSpecificCommandDrawParrotExpressLinesClear()
		{
			LocationSpecificCommandDrawParrotExpressLines = new();
		}
	}
}
