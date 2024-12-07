using System;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using TransportFramework.Utilities;

namespace TransportFramework.Patches
{
	internal class EventPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.UpdateWhenCurrentLocation), new Type[] { typeof(GameTime) }),
				postfix: new HarmonyMethod(typeof(EventPatch), nameof(GameLocationUpdateWhenCurrentLocationPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.draw), new Type[] { typeof(SpriteBatch) }),
				postfix: new HarmonyMethod(typeof(EventPatch), nameof(GameLocationDrawPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(TemporaryAnimatedSprite), "loadTexture"),
				prefix: new HarmonyMethod(typeof(EventPatch), nameof(TemporaryAnimatedSpriteLoadTexturePrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Event.DefaultCommands), nameof(Event.DefaultCommands.TemporarySprite), new Type[] { typeof(Event), typeof(string[]), typeof(EventContext) }),
				postfix: new HarmonyMethod(typeof(EventPatch), nameof(EventDefaultCommandsTemporarySpritePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Event.DefaultCommands), nameof(Event.DefaultCommands.TemporaryAnimatedSprite), new Type[] { typeof(Event), typeof(string[]), typeof(EventContext) }),
				postfix: new HarmonyMethod(typeof(EventPatch), nameof(EventDefaultCommandsTemporarySpritePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Event.DefaultCommands), nameof(Event.DefaultCommands.SpecificTemporarySprite), new Type[] { typeof(Event), typeof(string[]), typeof(EventContext) }),
				prefix: new HarmonyMethod(typeof(EventPatch), nameof(EventDefaultCommandsSpecificTemporarySpritePrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Event.DefaultCommands), nameof(Event.DefaultCommands.SpecificTemporarySprite), new Type[] { typeof(Event), typeof(string[]), typeof(EventContext) }),
				postfix: new HarmonyMethod(typeof(EventPatch), nameof(EventDefaultCommandsSpecificTemporarySpritePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Event.DefaultCommands), nameof(Event.DefaultCommands.RemoveSprite), new Type[] { typeof(Event), typeof(string[]), typeof(EventContext) }),
				postfix: new HarmonyMethod(typeof(EventPatch), nameof(EventDefaultCommandsRemoveSpritePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Event.DefaultCommands), nameof(Event.DefaultCommands.RemoveTemporarySprites), new Type[] { typeof(Event), typeof(string[]), typeof(EventContext) }),
				postfix: new HarmonyMethod(typeof(EventPatch), nameof(EventDefaultCommandsRemoveTemporarySpritesPostfix))
			);
		}

		private static void GameLocationUpdateWhenCurrentLocationPostfix(GameLocation __instance)
		{
			if (__instance.currentEvent is not null)
			{
				EventCommandsUtility.TemporarySpritesTemporaryAnimatedSpriteIntervalChangeUpdate();
				EventCommandsUtility.TemporarySpritesTemporaryAnimatedSpriteIntervalVariationUpdate();
				EventCommandsUtility.TemporarySpritesTemporaryAnimatedSpriteRotationChangeChangeUpdate();
				EventCommandsUtility.TemporarySpritesTemporaryAnimatedSpriteStopRotatingWhenVelocityIsZeroUpdate();
				EventCommandsUtility.TemporarySpritesTemporaryAnimatedSpriteSwayUpdate();
				EventCommandsUtility.TemporarySpritesTemporaryAnimatedSpriteFrameSoundUpdate();
				EventCommandsUtility.TemporarySpritesChangeTemporaryAnimatedSpriteUpdate();
				EventCommandsUtility.TemporarySpritesDestroyObjectsOnCollisionUpdate(__instance);
				EventCommandsUtility.ActorsSyncWithTemporarySpriteUpdate();
				EventCommandsUtility.SoundsVolumeChangeUpdate();
				EventCommandsUtility.SoundsPitchChangeUpdate();
				EventCommandsUtility.LocationSpecificCommandDrawParrotExpressLinesUpdate();
			}
		}

		private static void GameLocationDrawPostfix(GameLocation __instance, SpriteBatch b)
		{
			if (__instance.currentEvent is not null)
			{
				EventCommandsUtility.TemporaryFarmerSpritesDraw(b);
			}
		}

		private static bool TemporaryAnimatedSpriteLoadTexturePrefix(TemporaryAnimatedSprite __instance)
		{
			if (EventCommandsUtility.SEvent is not null)
			{
				try
				{
					Texture2D texture = EventCommandsUtility.SEvent.Station.ContentPack.ModContent.Load<Texture2D>(__instance.textureName);

					if (texture is not null)
					{
						__instance.texture = texture;
						return false;
					}
				}
				catch
				{
					return true;
				}
			}
			return true;
		}

		private static void EventDefaultCommandsTemporarySpritePostfix(Event @event, EventContext context)
		{
			EventCommandsUtility.TemporarySprites.Add(context.Location.TemporarySprites.Last());
			@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(EventCommandsUtility.TemporarySprites.Clear) != true ? EventCommandsUtility.TemporarySprites.Clear : null;
		}

		private static void EventDefaultCommandsSpecificTemporarySpritePrefix(EventContext context, out int __state)
		{
			__state = context.Location.TemporarySprites.Count;
		}

		private static void EventDefaultCommandsSpecificTemporarySpritePostfix(Event @event, EventContext context, int __state)
		{
			EventCommandsUtility.TemporarySprites.AddRange(context.Location.TemporarySprites.Skip(__state));
			@event.onEventFinished += @event.onEventFinished?.GetInvocationList().Contains(EventCommandsUtility.TemporarySprites.Clear) != true ? EventCommandsUtility.TemporarySprites.Clear : null;
		}

		private static void EventDefaultCommandsRemoveSpritePostfix(Event @event, string[] args)
		{
			if (!ArgUtility.TryGetVector2(args, 1, out Vector2 value, out _, integerOnly: true))
				return;

			Vector2 position = @event.OffsetPosition(value * Game1.tileSize);

			for (int i = EventCommandsUtility.TemporarySprites.Count - 1; i >= 0; i--)
			{
				if (EventCommandsUtility.TemporarySprites[i].position == position)
				{
					EventCommandsUtility.TemporarySprites.RemoveAt(i);
				}
			}
		}

		private static void EventDefaultCommandsRemoveTemporarySpritesPostfix()
		{
			EventCommandsUtility.TemporarySprites.Clear();
		}
	}
}
