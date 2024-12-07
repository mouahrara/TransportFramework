using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Minecarts;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class StationsUtility
	{
		public static void Initialize()
		{
			ContentPack contentPack = new()
			{
				Manifest = ModEntry.ModManifest,
				Translation = ModEntry.Helper.Translation,
				Format = "1.0.0",
				Stations = new()
				{
					new()
					{
						Id = "Bus_BusStop",
						DisplayName = "[mouahrara.TransportFramework_LocalizedStardewValley]",
						Location = "BusStop",
						Tile = new() { X = 22, Y = 10 },
						Direction = "down",
						Price = 0,
						Network = "Bus",
						AccessTiles = new List<Point>
						{
							new() { X = 17, Y = 11 }
						},
						Sprites = new List<SSprite>
						{
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\Cursors",
									SourceRectangle = { X = 288, Y = 1247, Width = 128, Height = 64 },
									Position = { X = 21f, Y = 6f },
									LayerDepth = 8.9f
								},
								CollisionBoxes = new List<SSCollisionBox>()
								{
									new() { X = 0f, Y = 1.5f, Width = 1f, Height = 2.25f },
									new() { X = 1f, Y = 1.5f, Width = 1f, Height = 1.5f },
									new() { X = 2f, Y = 1.5f, Width = 6f, Height = 2.25f }
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\Cursors",
									SourceRectangle = { X = 288, Y = 1311, Width = 16, Height = 38 },
									Position = { X = 22f, Y = 7.625f },
									LayerDepth = 9f
								}
							}
						},
						Conditions = new List<SCondition>
						{
							new()
							{
								Query = "PLAYER_HAS_MAIL Any ccVault Received",
								LockedMessage = "[LocalizedText Strings\\Locations:MineCart_OutOfOrder]",
								Update = "OnDayStart"
							},
							new()
							{
								Query = "ANY \"mouahrara.TransportFramework_CanDriveYourselfToday\" \"mouahrara.TransportFramework_IsCharacterAtTileForDeparture Pam 21 10\"",
								LockedMessage = "[LocalizedText Strings\\Locations:BusStop_NoDriver]",
								Update = "OnInteract"
							}
						},
						Events = new List<SEvent>
						{
							new()
							{
								Type = "Departure",
								Script = @"continue
										/follow
										/? 0 0 0
										/skippable
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1247 128 64 999999 1 1 21 6 false false 9.9 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1311 16 38 999999 1 1 22 7.625 false false 10.0 0 white 1 0 0 0
										/endSimultaneousCommand
										/mouahrara.TransportFramework_moveTo farmer 22 9 0 1
										/playSound stoneStep
										/mouahrara.TransportFramework_hideActor farmer
										/playMusic none
										/playSound trashcanlid
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1247 128 64 999999 1 1 21 6 false false 9.9 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1311 16 38 70 6 1 22 7.625 false false 10.0 0 white 1 0 0 0 hold_last_frame true
										/endSimultaneousCommand
										/pause 420
										/fade
										/mouahrara.TransportFramework_hideWorldCharacters
										/playSound batFlap
										/playSound busDriveOff
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1247 128 64 999999 1 1 21 6 false false 9.9 0 white 1 0 0 0 acceleration -0.1 0
										/mouahrara.TransportFramework_query mouahrara.TransportFramework_CanDriveYourselfToday 2 1
										/mouahrara.TransportFramework_temporaryFarmerSprite 117 48 608 16 32 21.0625 7.6875 3 true true false 9.99
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors2 0 0 21 41 999999 1 1 21 7.1875 false false 10.0 0 white 1 0 0 0 acceleration -0.1 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 384 1311 15 19 999999 1 1 21 8 false false 10.0 0 white 1 0 0 0 acceleration -0.1 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 368 1311 16 38 999999 1 1 22 7.625 false false 10.0 0 white 1 0 0 0 acceleration -0.1 0
										/mouahrara.TransportFramework_destroyObjectsOnCollision -3 0 1.5 8 2.25 true true
										/endSimultaneousCommand
										/pause 2400
										/globalFade 0.01
										/viewport -1000 -1000
										/end"
							},
							new()
							{
								Type = "Arrival",
								Script = @"none
										/follow
										/farmer 55 9 3
										/mouahrara.TransportFramework_hideWorldCharacters
										/mouahrara.TransportFramework_hideActor farmer
										/playSound busDriveOff
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1247 128 64 999999 1 1 54 6 false false 9.9 0 white 1 0 0 0 motion -6 0
										/mouahrara.TransportFramework_query mouahrara.TransportFramework_CanDriveYourselfToday 2 1
										/mouahrara.TransportFramework_temporaryFarmerSprite 117 48 608 16 32 54.0625 7.6875 3 true true false 9.99
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors2 0 0 21 41 999999 1 1 54 7.1875 false false 10.0 0 white 1 0 0 0 motion -6 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 384 1311 15 19 999999 1 1 54 8 false false 10.0 0 white 1 0 0 0 motion -6 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 368 1311 16 38 999999 1 1 55 7.625 false false 10.0 0 white 1 0 0 0 motion -6 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -3 < 44 none 0 0 acceleration_change 0.0000896 0 stop_accelerating_when_velocity_is_zero true
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -2 < 44 none 0 0 acceleration_change 0.0000896 0 stop_accelerating_when_velocity_is_zero true
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 < 45 none 0 0 acceleration_change 0.0000896 0 stop_accelerating_when_velocity_is_zero true
										/mouahrara.TransportFramework_destroyObjectsOnCollision -3 0 1.5 8 2.25 true true
										/mouahrara.TransportFramework_beginSyncWithTemporarySprite farmer
										/endSimultaneousCommand
										/skippable
										/pause 7600
										/globalFade 0.02
										/viewport -1000 -1000
										/end"
							}
						},
						Sound = "trashcanlid",
						IgnoreConditionsArrival = new List<string>
						{
							"all"
						}
					},
					new()
					{
						Id = "Bus_Desert",
						DisplayName = "[LocalizedText Strings\\StringsFromCSFiles:MapPage.cs.11062]",
						Location = "Desert",
						Tile = new() { X = 18, Y = 28 },
						Direction = "down",
						Price = 500,
						Network = "Bus",
						AccessTiles = new List<Point>
						{
							new() { X = 18, Y = 27 }
						},
						Sprites = new List<SSprite>
						{
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\Cursors",
									SourceRectangle = { X = 288, Y = 1247, Width = 128, Height = 64 },
									Position = { X = 17f, Y = 24f },
									LayerDepth = 26.9f
								},
								CollisionBoxes = new List<SSCollisionBox>()
								{
									new() { X = 0f, Y = 1.5f, Width = 1f, Height = 2.25f },
									new() { X = 1f, Y = 1.5f, Width = 1f, Height = 1.5f },
									new() { X = 2f, Y = 1.5f, Width = 6f, Height = 2.25f }
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\Cursors",
									SourceRectangle = { X = 288, Y = 1311, Width = 16, Height = 38 },
									Position = { X = 18f, Y = 25.625f },
									LayerDepth = 27f
								}
							}
						},
						Events = new List<SEvent>
						{
							new()
							{
								Type = "Departure",
								Script = @"continue
										/follow
										/? 0 0 0
										/skippable
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1247 128 64 999999 1 1 17 24 false false 27.9 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1311 16 38 999999 1 1 18 25.625 false false 28.0 0 white 1 0 0 0
										/endSimultaneousCommand
										/playSound stoneStep
										/mouahrara.TransportFramework_hideActor farmer
										/playMusic none
										/playSound trashcanlid
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1247 128 64 999999 1 1 17 24 false false 27.9 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1311 16 38 70 6 1 18 25.625 false false 28.0 0 white 1 0 0 0 hold_last_frame true
										/endSimultaneousCommand
										/pause 420
										/fade
										/mouahrara.TransportFramework_hideWorldCharacters
										/playSound batFlap
										/playSound busDriveOff
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1247 128 64 999999 1 1 17 24 false false 27.9 0 white 1 0 0 0 acceleration -0.1 0
										/mouahrara.TransportFramework_query mouahrara.TransportFramework_CanDriveYourselfToday 2 1
										/mouahrara.TransportFramework_temporaryFarmerSprite 117 48 608 16 32 17.0625 25.6875 3 true true false 27.99
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors2 0 0 21 41 999999 1 1 17 25.1875 false false 28.0 0 white 1 0 0 0 acceleration -0.1 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 384 1311 15 19 999999 1 1 17 26 false false 28.0 0 white 1 0 0 0 acceleration -0.1 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 368 1311 16 38 999999 1 1 18 25.625 false false 28.0 0 white 1 0 0 0 acceleration -0.1 0
										/mouahrara.TransportFramework_destroyObjectsOnCollision -3 0 1.5 8 2.25 true true
										/endSimultaneousCommand
										/pause 2400
										/globalFade 0.01
										/viewport -1000 -1000
										/end"
							},
							new()
							{
								Type = "Arrival",
								Script = @"none
										/follow
										/farmer 50 27 3
										/mouahrara.TransportFramework_hideWorldCharacters
										/mouahrara.TransportFramework_hideActor farmer
										/playSound busDriveOff
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 288 1247 128 64 999999 1 1 49 24 false false 27.9 0 white 1 0 0 0 motion -6 0 acceleration_change 0.00004606 0 stop_accelerating_when_velocity_is_zero true
										/mouahrara.TransportFramework_query mouahrara.TransportFramework_CanDriveYourselfToday 2 1
										/mouahrara.TransportFramework_temporaryFarmerSprite 117 48 608 16 32 49.0625 25.6875 3 true true false 27.99
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors2 0 0 21 41 999999 1 1 49 25.1875 false false 28.0 0 white 1 0 0 0 motion -6 0 acceleration_change 0.00004606 0 stop_accelerating_when_velocity_is_zero true
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 384 1311 15 19 999999 1 1 49 26 false false 28.0 0 white 1 0 0 0 motion -6 0 acceleration_change 0.00004606 0 stop_accelerating_when_velocity_is_zero true
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\Cursors 368 1311 16 38 999999 1 1 50 25.625 false false 28.0 0 white 1 0 0 0 motion -6 0 acceleration_change 0.00004606 0 stop_accelerating_when_velocity_is_zero true
										/mouahrara.TransportFramework_destroyObjectsOnCollision -3 0 1.5 8 2.25 true true
										/mouahrara.TransportFramework_beginSyncWithTemporarySprite farmer
										/endSimultaneousCommand
										/skippable
										/pause 8200
										/globalFade 0.02
										/viewport -1000 -1000
										/end"
							}
						},
						Sound = "trashcanlid"
					},
					new()
					{
						Id = "Boat_BoatTunnel",
						DisplayName = "[mouahrara.TransportFramework_LocalizedStardewValley]",
						Location = "BoatTunnel",
						Tile = new() { X = 6, Y = 9 },
						Direction = "down",
						Price = 0,
						Network = "Boat",
						AccessTiles = new List<Point>
						{
							new() { X = 4, Y = 9 }
						},
						Conditions = new List<SCondition>
						{
							new()
							{
								Query = "PLAYER_HAS_MAIL Any willyBoatFixed",
								Update = "OnDayStart"
							}
						},
						Events = new List<SEvent>
						{
							new()
							{
								Type = "Departure",
								Id = "-78765/",
								Script = @"none
										/follow
										/Willy 6 12 0
										/skippable
										/textAboveHead Willy ""[LocalizedText Strings\\Locations:BoatTunnel_willyText_firstRide]""
										/move Willy 0 -3 0
										/pause 500
										/locationSpecificCommand open_gate
										/viewport move 0 -1 1000
										/pause 500/move Willy 0 -2 3
										/move Willy -1 0 1
										/locationSpecificCommand path_player 6 5 2
										/move Willy 1 0 2/move Willy 0 1 2
										/pause 250/playSound clubhit
										/animate Willy false false 500 27
										/locationSpecificCommand retract_plank
										/jump Willy 4
										/pause 750
										/move Willy 0 -1 0
										/locationSpecificCommand close_gate
										/pause 200/move Willy 3 0 1
										/positionOffset Willy 0 -24
										/advancedMove Willy false 1 0 1 2000
										/locationSpecificCommand non_blocking_pause 1000
										/playerControl boatRide
										/playSound furnace
										/locationSpecificCommand animate_boat_start
										/locationSpecificCommand non_blocking_pause 1000
										/locationSpecificCommand boat_depart
										/locationSpecificCommand animate_boat_move
										/fade
										/viewport -1000 -1000
										/end",
								Filter = new()
								{
									TravelCount = 1
								}
							},
							new()
							{
								Type = "Departure",
								Id = "-78765/",
								Script = @"none
										/follow
										/Willy 6 12 0
										/skippable
										/mouahrara.TransportFramework_query ""RANDOM 0.2"" 3
										/mouahrara.TransportFramework_query ""RANDOM 0.5"" 1 1
										/textAboveHead Willy ""[LocalizedText Strings\\Locations:BoatTunnel_willyText_random0]""
										/textAboveHead Willy ""[LocalizedText Strings\\Locations:BoatTunnel_willyText_random1]""
										/move Willy 0 -3 0
										/pause 500
										/locationSpecificCommand open_gate
										/viewport move 0 -1 1000
										/pause 500
										/move Willy 0 -2 3
										/move Willy -1 0 1
										/locationSpecificCommand path_player 6 5 2
										/move Willy 1 0 2
										/move Willy 0 1 2
										/pause 250
										/playSound clubhit
										/animate Willy false false 500 27
										/locationSpecificCommand retract_plank
										/jump Willy 4
										/pause 750
										/move Willy 0 -1 0
										/locationSpecificCommand close_gate
										/pause 200
										/move Willy 3 0 1
										/positionOffset Willy 0 -24
										/advancedMove Willy false 1 0 1 2000
										/locationSpecificCommand non_blocking_pause 1000
										/playerControl boatRide
										/playSound furnace
										/locationSpecificCommand animate_boat_start
										/locationSpecificCommand non_blocking_pause 1000
										/locationSpecificCommand boat_depart
										/locationSpecificCommand animate_boat_move
										/fade
										/viewport -1000 -1000
										/end"
							}
						},
						IgnoreConditionsArrival = new List<string>
						{
							"all"
						}
					},
					new()
					{
						Id = "Boat_IslandSouth",
						DisplayName = "[LocalizedText Strings\\StringsFromCSFiles:IslandName]",
						Location = "IslandSouth",
						Tile = new() { X = 21, Y = 43 },
						Direction = "up",
						Price = 1000,
						Network = "Boat",
						AccessTiles = new List<Point>
						{
							new() { X = 19, Y = 43 }
						},
						Events = new List<SEvent>
						{
							new()
							{
								Type = "Departure",
								Id = "-157039427/",
								Script = "[LocalizedText Data\\Events\\IslandSouth:IslandDepart]"
							}
						}
					},
					new()
					{
						Id = "ParrotExpress_Volcano",
						DisplayName = "[LocalizedText Strings\\UI:ParrotPlatform_Volcano]",
						Location = "IslandNorth",
						Tile = new() { X = 60, Y = 17 },
						Direction = "down",
						Network = "ParrotExpress",
						AccessTiles = new List<Point>
						{
							new() { X = 59, Y = 15 },
							new() { X = 60, Y = 15 },
							new() { X = 61, Y = 15 }
						},
						Sprites = new List<SSprite>
						{
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 73, Width = 48, Height = 32 },
									Position = { X = 58.875f, Y = 15.375f },
									LayerDepth = 0f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 0, Y = 0, Width = 48, Height = 68 },
									Position = { X = 59f, Y = 13f },
									LayerDepth = 15f
								},
								CollisionBoxes = new List<SSCollisionBox>()
								{
									new() { X = 0f, Y = 2f, Width = 3f, Height = 0.25f },
									new() { X = 0f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 2.75f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 0f, Y = 3.75f, Width = 1f, Height = 0.25f },
									new() { X = 2f, Y = 3.75f, Width = 1f, Height = 0.25f }
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 0, Width = 48, Height = 68 },
									Position = { X = 59f, Y = 13f },
									LayerDepth = 17f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 59.2f, Y = 11.75f },
									LayerDepth = 17.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 60.325f, Y = 11.75f },
									Flip = true,
									LayerDepth = 17.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							}
						},
						Conditions = new List<SCondition>
						{
							new()
							{
								Query = "PLAYER_HAS_MAIL Any Island_UpgradeParrotPlatform",
								Update = "OnInteract"
							}
						},
						Events = new List<SEvent>
						{
							new()
							{
								Type = "Departure",
								Script = @"continue
										/follow
										/? 0 0 0
										/skippable
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 73 48 32 999999 1 1 58.875 15.375 false false 0.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 0 0 48 68 999999 1 1 59 13 false false 15.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 0 48 68 999999 1 1 59 13 false false 17.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 59.2 11.75 false false 17.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 60.325 11.75 false true 17.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/endSimultaneousCommand
										/playSound parrot
										/pause 260
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 59.2 11.75 false false 17.1 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 60.325 11.75 false true 17.1 0 white 1 0 0 0
										/endSimultaneousCommand
										/pause 240
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 59.2 11.75 false false 17.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion -0.6 -0.6 acceleration 0 -0.1 sway 60.325 11.75 60.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 60.325 11.75 false true 17.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion 0.6 -0.6 acceleration 0 -0.1 sway 60.325 11.75 60.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 9.75 0 delay_before_animation_start 1 start_sound treethud
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 9.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 9.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -5 -1 none 0 < 9.75 500 motion 0.16 0.16 scale 1 scale_change -0.01
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 9.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 9.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 9.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 9.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 9.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 9.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_beginSyncWithTemporarySprite farmer -4 offset
										/mouahrara.TransportFramework_locationSpecificCommand_draw_ParrotExpress_lines
										/endSimultaneousCommand
										/pause 2600
										/fade
										/viewport -1000 -1000
										/end"
							}
						}
					},
					new()
					{
						Id = "ParrotExpress_Archaeology",
						DisplayName = "[LocalizedText Strings\\UI:ParrotPlatform_Archaeology]",
						Location = "IslandNorth",
						Tile = new() { X = 5, Y = 49 },
						Direction = "down",
						Network = "ParrotExpress",
						AccessTiles = new List<Point>
						{
							new() { X = 4, Y = 47 },
							new() { X = 5, Y = 47 },
							new() { X = 6, Y = 47 }
						},
						Sprites = new List<SSprite>
						{
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 73, Width = 48, Height = 32 },
									Position = { X = 3.875f, Y = 47.375f },
									LayerDepth = 0f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 0, Y = 0, Width = 48, Height = 68 },
									Position = { X = 4f, Y = 45f },
									LayerDepth = 47f
								},
								CollisionBoxes = new List<SSCollisionBox>()
								{
									new() { X = 0f, Y = 2f, Width = 3f, Height = 0.25f },
									new() { X = 0f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 2.75f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 0f, Y = 3.75f, Width = 1f, Height = 0.25f },
									new() { X = 2f, Y = 3.75f, Width = 1f, Height = 0.25f }
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 0, Width = 48, Height = 68 },
									Position = { X = 4f, Y = 45f },
									LayerDepth = 49f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 4.2f, Y = 43.75f },
									LayerDepth = 49.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 5.325f, Y = 43.75f },
									Flip = true,
									LayerDepth = 49.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							}
						},
						Conditions = new List<SCondition>
						{
							new()
							{
								Query = "PLAYER_HAS_MAIL Any Island_UpgradeBridge, PLAYER_HAS_MAIL Any Island_UpgradeParrotPlatform",
								Update = "OnInteract"
							}
						},
						Events = new List<SEvent>
						{
							new()
							{
								Type = "Departure",
								Script = @"continue
										/follow
										/? 0 0 0
										/skippable
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 73 48 32 999999 1 1 3.875 47.375 false false 0.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 0 0 48 68 999999 1 1 4 45 false false 47.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 0 48 68 999999 1 1 4 45 false false 49.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 4.2 43.75 false false 49.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 5.325 43.75 false true 49.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/endSimultaneousCommand
										/playSound parrot
										/pause 260
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 4.2 43.75 false false 49.1 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 5.325 43.75 false true 49.1 0 white 1 0 0 0
										/endSimultaneousCommand
										/pause 240
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 4.2 43.75 false false 49.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion -0.6 -0.6 acceleration 0 -0.1 sway 5.325 43.75 5.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 5.325 43.75 false true 49.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion 0.6 -0.6 acceleration 0 -0.1 sway 5.325 43.75 5.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 41.75 0 delay_before_animation_start 1 start_sound treethud
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 41.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 41.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -5 -1 none 0 < 41.75 500 motion 0.16 0.16 scale 1 scale_change -0.01
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 41.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 41.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 41.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 41.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 41.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 41.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_beginSyncWithTemporarySprite farmer -4 offset
										/mouahrara.TransportFramework_locationSpecificCommand_draw_ParrotExpress_lines
										/endSimultaneousCommand
										/pause 2600
										/fade
										/viewport -1000 -1000
										/end"
							}
						}
					},
					new()
					{
						Id = "ParrotExpress_Farm",
						DisplayName = "[LocalizedText Strings\\UI:ParrotPlatform_Farm]",
						Location = "IslandWest",
						Tile = new() { X = 74, Y = 10 },
						Direction = "down",
						Network = "ParrotExpress",
						AccessTiles = new List<Point>
						{
							new() { X = 73, Y = 8 },
							new() { X = 74, Y = 8 },
							new() { X = 75, Y = 8 }
						},
						Sprites = new List<SSprite>
						{
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 73, Width = 48, Height = 32 },
									Position = { X = 72.875f, Y = 8.375f },
									LayerDepth = 0f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 0, Y = 0, Width = 48, Height = 68 },
									Position = { X = 73f, Y = 6f },
									LayerDepth = 8f
								},
								CollisionBoxes = new List<SSCollisionBox>()
								{
									new() { X = 0f, Y = 2f, Width = 3f, Height = 0.25f },
									new() { X = 0f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 2.75f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 0f, Y = 3.75f, Width = 1f, Height = 0.25f },
									new() { X = 2f, Y = 3.75f, Width = 1f, Height = 0.25f }
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 0, Width = 48, Height = 68 },
									Position = { X = 73f, Y = 6f },
									LayerDepth = 10f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 73.2f, Y = 4.75f },
									LayerDepth = 10.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 74.325f, Y = 4.75f },
									Flip = true,
									LayerDepth = 10.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							}
						},
						Conditions = new List<SCondition>
						{
							new()
							{
								Query = "PLAYER_HAS_MAIL Any Island_UpgradeParrotPlatform",
								Update = "OnInteract"
							}
						},
						Events = new List<SEvent>
						{
							new()
							{
								Type = "Departure",
								Script = @"continue
										/follow
										/? 0 0 0
										/skippable
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 73 48 32 999999 1 1 72.875 8.375 false false 0.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 0 0 48 68 999999 1 1 73 6 false false 8.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 0 48 68 999999 1 1 73 6 false false 10.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 73.2 4.75 false false 10.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 74.325 4.75 false true 10.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/endSimultaneousCommand
										/playSound parrot
										/pause 260
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 73.2 4.75 false false 10.1 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 74.325 4.75 false true 10.1 0 white 1 0 0 0
										/endSimultaneousCommand
										/pause 240
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 73.2 4.75 false false 10.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion -0.6 -0.6 acceleration 0 -0.1 sway 74.325 4.75 74.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 74.325 4.75 false true 10.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion 0.6 -0.6 acceleration 0 -0.1 sway 74.325 4.75 74.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 2.75 0 delay_before_animation_start 1 start_sound treethud
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 2.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 2.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -5 -1 none 0 < 2.75 500 motion 0.16 0.16 scale 1 scale_change -0.01
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 2.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 2.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 2.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 2.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 2.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 2.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_beginSyncWithTemporarySprite farmer -4 offset
										/mouahrara.TransportFramework_locationSpecificCommand_draw_ParrotExpress_lines
										/endSimultaneousCommand
										/pause 2600
										/fade
										/viewport -1000 -1000
										/end"
							}
						}
					},
					new()
					{
						Id = "ParrotExpress_Forest",
						DisplayName = "[LocalizedText Strings\\UI:ParrotPlatform_Forest]",
						Location = "IslandEast",
						Tile = new() { X = 28, Y = 29 },
						Direction = "down",
						Network = "ParrotExpress",
						AccessTiles = new List<Point>
						{
							new() { X = 27, Y = 27 },
							new() { X = 28, Y = 27 },
							new() { X = 29, Y = 27 }
						},
						Sprites = new List<SSprite>
						{
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 73, Width = 48, Height = 32 },
									Position = { X = 26.875f, Y = 27.375f },
									LayerDepth = 0f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 0, Y = 0, Width = 48, Height = 68 },
									Position = { X = 27f, Y = 25f },
									LayerDepth = 27f
								},
								CollisionBoxes = new List<SSCollisionBox>()
								{
									new() { X = 0f, Y = 2f, Width = 3f, Height = 0.25f },
									new() { X = 0f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 2.75f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 0f, Y = 3.75f, Width = 1f, Height = 0.25f },
									new() { X = 2f, Y = 3.75f, Width = 1f, Height = 0.25f }
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 0, Width = 48, Height = 68 },
									Position = { X = 27f, Y = 25f },
									LayerDepth = 29f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 27.2f, Y = 23.75f },
									LayerDepth = 29.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 28.325f, Y = 23.75f },
									Flip = true,
									LayerDepth = 29.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							}
						},
						Conditions = new List<SCondition>
						{
							new()
							{
								Query = "PLAYER_HAS_MAIL Any Island_UpgradeParrotPlatform",
								Update = "OnInteract"
							}
						},
						Events = new List<SEvent>
						{
							new()
							{
								Type = "Departure",
								Script = @"continue
										/follow
										/? 0 0 0
										/skippable
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 73 48 32 999999 1 1 26.875 27.375 false false 0.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 0 0 48 68 999999 1 1 27 25 false false 27.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 0 48 68 999999 1 1 27 25 false false 29.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 27.2 23.75 false false 29.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 28.325 23.75 false true 29.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/endSimultaneousCommand
										/playSound parrot
										/pause 260
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 27.2 23.75 false false 29.1 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 28.325 23.75 false true 29.1 0 white 1 0 0 0
										/endSimultaneousCommand
										/pause 240
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 27.2 23.75 false false 29.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion -0.6 -0.6 acceleration 0 -0.1 sway 28.325 23.75 28.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 28.325 23.75 false true 29.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion 0.6 -0.6 acceleration 0 -0.1 sway 28.325 23.75 28.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 21.75 0 delay_before_animation_start 1 start_sound treethud
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 21.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 21.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -5 -1 none 0 < 21.75 500 motion 0.16 0.16 scale 1 scale_change -0.01
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 21.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 21.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 21.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 21.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 21.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 21.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_beginSyncWithTemporarySprite farmer -4 offset
										/mouahrara.TransportFramework_locationSpecificCommand_draw_ParrotExpress_lines
										/endSimultaneousCommand
										/pause 2600
										/fade
										/viewport -1000 -1000
										/end"
							}
						}
					},
					new()
					{
						Id = "ParrotExpress_Docks",
						DisplayName = "[LocalizedText Strings\\UI:ParrotPlatform_Docks]",
						Location = "IslandSouth",
						Tile = new() { X = 6, Y = 32 },
						Direction = "down",
						Network = "ParrotExpress",
						AccessTiles = new List<Point>
						{
							new() { X = 5, Y = 30 },
							new() { X = 6, Y = 30 },
							new() { X = 7, Y = 30 }
						},
						Sprites = new List<SSprite>
						{
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 73, Width = 48, Height = 32 },
									Position = { X = 4.875f, Y = 30.375f },
									LayerDepth = 0f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 0, Y = 0, Width = 48, Height = 68 },
									Position = { X = 5f, Y = 28f },
									LayerDepth = 30f
								},
								CollisionBoxes = new List<SSCollisionBox>()
								{
									new() { X = 0f, Y = 2f, Width = 3f, Height = 0.25f },
									new() { X = 0f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 2.75f, Y = 2f, Width = 0.25f, Height = 2f },
									new() { X = 0f, Y = 3.75f, Width = 1f, Height = 0.25f },
									new() { X = 2f, Y = 3.75f, Width = 1f, Height = 0.25f }
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\ParrotPlatform",
									SourceRectangle = { X = 48, Y = 0, Width = 48, Height = 68 },
									Position = { X = 5f, Y = 28f },
									LayerDepth = 32f
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 5.2f, Y = 26.75f },
									LayerDepth = 32.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							},
							new()
							{
								Data = new SSData()
								{
									TextureName = "LooseSprites\\parrots",
									SourceRectangle = { X = 0, Y = 0, Width = 24, Height = 24 },
									Position = { X = 6.325f, Y = 26.75f },
									Flip = true,
									LayerDepth = 32.1f
								},
								Conditions = new List<SSCondition>
								{
									new()
									{
										Query = "WORLD_STATE_FIELD ParrotPlatformsUnlocked true",
										Update = "OnLocationChange"
									}
								}
							}
						},
						Conditions = new List<SCondition>
						{
							new()
							{
								Query = "PLAYER_HAS_MAIL Any Island_UpgradeParrotPlatform",
								Update = "OnInteract"
							}
						},
						Events = new List<SEvent>
						{
							new()
							{
								Type = "Departure",
								Script = @"continue
										/follow
										/? 0 0 0
										/skippable
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 73 48 32 999999 1 1 4.875 30.375 false false 0.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 0 0 48 68 999999 1 1 5 28 false false 30.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\ParrotPlatform 48 0 48 68 999999 1 1 5 28 false false 32.0 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 5.2 26.75 false false 32.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 130 2 1 6.325 26.75 false true 32.1 0 white 1 0 0 0 shake_intensity 2 ping_pong true
										/endSimultaneousCommand
										/playSound parrot
										/pause 260
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 5.2 26.75 false false 32.1 0 white 1 0 0 0
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 0 0 24 24 999999 1 1 6.325 26.75 false true 32.1 0 white 1 0 0 0
										/endSimultaneousCommand
										/pause 240
										/beginSimultaneousCommand
										/mouahrara.TransportFramework_removeTemporarySprites LooseSprites\parrots
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 5.2 26.75 false false 32.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion -0.6 -0.6 acceleration 0 -0.1 sway 6.325 26.75 6.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_temporaryAnimatedSprite LooseSprites\parrots 120 0 24 24 50 3 999999 6.325 26.75 false true 32.1 0 white 1 0 0 0 interval_variation 3.75 frame_sound batFlap 2 motion 0.6 -0.6 acceleration 0 -0.1 sway 6.325 26.75 6.325 0 0.25 2.5 4 16 default default y default
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 24.75 0 delay_before_animation_start 1 start_sound treethud
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 24.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 24.75 0 shake_intensity 6.6 shake_intensity_change -0.022
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -5 -1 none 0 < 24.75 500 motion 0.16 0.16 scale 1 scale_change -0.01
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -4 -1 none 0 < 24.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -3 -1 none 0 < 24.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 24.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 24.75 0 motion 0 0 acceleration 0 0
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -2 -1 none 0 < 24.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_changeTemporaryAnimatedSprite -1 -1 none 0 < 24.75 500 motion 0 0 acceleration 0 -0.1
										/mouahrara.TransportFramework_beginSyncWithTemporarySprite farmer -4 offset
										/mouahrara.TransportFramework_locationSpecificCommand_draw_ParrotExpress_lines
										/endSimultaneousCommand
										/pause 2600
										/fade
										/viewport -1000 -1000
										/end"
							}
						}
					}
				}
			};

			AddMinecartStationsToContentPack(contentPack);
			foreach (Station station in contentPack.Stations)
			{
				station.ContentPack = contentPack;
				ModEntry.Stations.Add(station);
			}
		}

		private static void AddMinecartStationsToContentPack(ContentPack contentPack)
		{
			List<KeyValuePair<string, MinecartNetworkData>> sortedMinecarts = GetSortedMinecarts();

			foreach ((string networkName, MinecartNetworkData networkData) in sortedMinecarts)
			{
				string network = networkName.Equals("Default") ? "Minecart" : networkName;
				string lockedMessage = networkData.LockedMessage ?? "[LocalizedText Strings\\Locations:MineCart_OutOfOrder]";

				foreach (MinecartDestinationData destination in networkData.Destinations)
				{
					List<SCondition> conditions = new();

					if (!string.IsNullOrWhiteSpace(networkData.UnlockCondition))
					{
						conditions.Add(new()
						{
							Query = networkData.UnlockCondition,
							LockedMessage = lockedMessage,
							Update = "OnDayStart"
						});
					}
					if (!string.IsNullOrWhiteSpace(destination.Condition))
					{
						conditions.Add(new()
						{
							Query = destination.Condition,
							LockedMessage = lockedMessage,
							Update = "OnDayStart"
						});
					}
					contentPack.Stations.Add(new Station
					{
						Id = $"{network}_{destination.Id}",
						DisplayName = destination.DisplayName,
						Location = destination.TargetLocation,
						Tile = destination.TargetTile,
						Direction = destination.TargetDirection,
						Price = destination.Price,
						Network = network,
						Conditions = conditions.Any() ? conditions : null
					});
				}
			}
		}

		private static List<KeyValuePair<string, MinecartNetworkData>> GetSortedMinecarts()
		{
			Dictionary<string, MinecartNetworkData> dictionary = DataLoader.Minecarts(Game1.content);
			List<KeyValuePair<string, MinecartNetworkData>> sortedDictionary = dictionary.OrderBy(pair => pair.Key.Equals("Default") ? string.Empty : pair.Key).ToList();

			foreach ((string networkName, MinecartNetworkData networkData) in sortedDictionary)
			{
				if (networkName.Equals("Default"))
				{
					List<string> defaultMinecartsOrder = new() { "Bus", "Quarry", "Mines", "Town" };

					networkData.Destinations = networkData.Destinations.OrderBy(d => defaultMinecartsOrder.IndexOf(d.Id) >= 0 ? defaultMinecartsOrder.IndexOf(d.Id) : int.MaxValue).ThenBy(d => d.Id).ToList();
				}
				else
				{
					networkData.Destinations = networkData.Destinations.OrderBy(d => d.Id).ToList();
				}
			}
			return sortedDictionary;
		}

		public static void SetStationReferences()
		{
			if (ModEntry.Stations is not null)
			{
				foreach (Station station in ModEntry.Stations)
				{
					if (station.Sprites is not null)
					{
						foreach (SSprite sprite in station.Sprites)
						{
							sprite.Station = station;
							if (sprite.Conditions is not null)
							{
								foreach (SSCondition condition in sprite.Conditions)
								{
									condition.Sprite = sprite;
								}
							}
						}
					}
					if (station.Conditions is not null)
					{
						foreach (SCondition condition in station.Conditions)
						{
							condition.Station = station;
						}
					}
					if (station.Events is not null)
					{
						foreach (SEvent @event in station.Events)
						{
							@event.Station = station;
						}
					}
				}
			}
		}

		public static void RemoveInvalidStationsGameLaunched()
		{
			RemoveInvalidStations(IsStationValidGameLaunched);
		}

		public static void RemoveInvalidStationsSaveLoaded()
		{
			RemoveInvalidStations(IsStationValidSaveLoaded);
		}

		private static void RemoveInvalidStations(Func<Station, bool> isStationValid)
		{
			if (ModEntry.Stations is not null)
			{
				for (int i = 0; i < ModEntry.Stations.Count; i++)
				{
					if (!isStationValid(ModEntry.Stations[i]))
					{
						ModEntry.Stations.RemoveAt(i--);
					}
				}
			}
		}

		public static bool IsStationValidGameLaunched(Station station)
		{
			if (string.IsNullOrWhiteSpace(station.Location))
			{
				ModEntry.Monitor.Log($"Failed to add station (Id: {station.Id}): The 'Location' property is missing.", LogLevel.Error);
				return false;
			}
			if (station.Tile.X == -1 && station.Tile.Y == -1)
			{
				ModEntry.Monitor.Log($"Failed to add station (Id: {station.Id}): The 'Tile' property is missing.", LogLevel.Error);
				return false;
			}
			if (string.IsNullOrWhiteSpace(station.Network))
			{
				ModEntry.Monitor.Log($"Failed to add station (Id: {station.Id}): The 'Network' property is missing.", LogLevel.Error);
				return false;
			}
			if (station.DirectionAsInt < 0 || 3 < station.DirectionAsInt)
			{
				ModEntry.Monitor.Log($"Failed to set 'Direction' for station (Id: {station.Id}): The direction ({station.Direction}) must be one of '0', '1', '2', '3', 'up', 'right', 'down' or 'left'. The default value ('down') will be used.", LogLevel.Warn);
				station.DirectionAsInt = 2;
				station.Direction = "down";
			}
			if (station.Price < 0)
			{
				ModEntry.Monitor.Log($"Failed to set 'Price' for station (Id: {station.Id}): The price ({station.Price}) must be greater than or equal to 0. The default value (0) will be used.", LogLevel.Warn);
				station.Price = 0;
			}
			if (!string.IsNullOrWhiteSpace(station.Sound) && !Game1.soundBank.Exists(station.Sound))
			{
				ModEntry.Monitor.Log($"Failed to set 'Sound' for station (Id: {station.Id}): The sound ({station.Sound}) doesn't exist. The sound will be ignored.", LogLevel.Warn);
				station.Sound = null;
			}
			if (station.Sprites is not null)
			{
				for (int i = 0; i < station.Sprites.Count; i++)
				{
					if (string.IsNullOrWhiteSpace(station.Sprites[i].Data.TextureName))
					{
						ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) sprite no.{i + 1}: The 'TextureName' property is missing. The sprite will be ignored.", LogLevel.Warn);
						station.Sprites.RemoveAt(i--);
						continue;
					}
					try
					{
						if (Game1.content.DoesAssetExist<Texture2D>(station.Sprites[i].Data.TextureName))
						{
							station.Sprites[i].Data.Texture = Game1.content.Load<Texture2D>(station.Sprites[i].Data.TextureName);
							station.Sprites[i].Data.InternalTextureName = station.Sprites[i].Data.Texture.Name;
						}
						else
						{
							station.Sprites[i].Data.InternalTextureName = station.ContentPack.ModContent.GetInternalAssetName(station.Sprites[i].Data.TextureName).BaseName;
							station.Sprites[i].Data.Texture = Game1.content.Load<Texture2D>(station.Sprites[i].Data.InternalTextureName);
						}
					}
					catch
					{
						ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) sprite no.{i + 1}: The texture ({station.Sprites[i].Data.TextureName}) doesn't exist. The sprite will be ignored.", LogLevel.Warn);
						station.Sprites.RemoveAt(i--);
						continue;
					}
					if (station.Sprites[i].Data.Position == new Vector2(-1, -1))
					{
						ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) sprite no.{i + 1}: The 'Position' property is missing. The sprite will be ignored.", LogLevel.Warn);
						station.Sprites.RemoveAt(i--);
						continue;
					}
				}
			}
			if (station.Conditions is not null)
			{
				for (int i = 0; i < station.Conditions.Count; i++)
				{
					if (string.IsNullOrWhiteSpace(station.Conditions[i].Query))
					{
						ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) condition no.{i + 1}: The 'Query' property is missing. The condition will be ignored.", LogLevel.Warn);
						station.Conditions.RemoveAt(i--);
						continue;
					}
					if (!station.Conditions[i].Update.Equals("OnDayStart") && !station.Conditions[i].Update.Equals("OnInteract"))
					{
						ModEntry.Monitor.Log($"Failed to set 'Update' for the station (Id: {station.Id}) condition no.{i + 1}: The update ({station.Conditions[i].Update}) must be one of 'OnDayStart' or 'OnInteract'. The default value ('OnDayStart') will be used.", LogLevel.Warn);
						station.Conditions[i].Update = "OnDayStart";
					}
				}
			}
			if (station.Events is not null)
			{
				for (int i = 0; i < station.Events.Count; i++)
				{
					if (!station.Events[i].Type.Equals("Departure") && !station.Events[i].Type.Equals("Arrival"))
					{
						ModEntry.Monitor.Log($"Failed to set 'Type' for the station (Id: {station.Id}) event no.{i + 1}: The type ({station.Events[i].Type}) must be one of 'Departure' or 'Arrival'. The default value ('Departure') will be used.", LogLevel.Warn);
						station.Events[i].Type = "Departure";
					}
					if (string.IsNullOrWhiteSpace(station.Events[i].Id) && string.IsNullOrWhiteSpace(station.Events[i].Script))
					{
						ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) event no.{i + 1}: The 'Id' and 'Script' properties are both missing. The event will be ignored.", LogLevel.Warn);
						station.Events.RemoveAt(i--);
						continue;
					}
					if (string.IsNullOrWhiteSpace(station.Events[i].Id))
					{
						station.Events[i].Id = $"{station.Id}_Event_{i}/";
					}
					else if (!station.ContentPack.Manifest.UniqueID.Equals(ModEntry.ModManifest.UniqueID) && !station.Events[i].Id.StartsWith(station.Id))
					{
						ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) event no.{i + 1}: The 'Id' property ({station.Events[i].Id}) must start with the Id of the station ({station.Id}).", LogLevel.Warn);
						station.Events.RemoveAt(i--);
						continue;
					}
				}
			}
			return true;
		}

		public static bool IsStationValidSaveLoaded(Station station)
		{
			GameLocation location = Game1.getLocationFromName(station.Location);

			if (location is null)
			{
				ModEntry.Monitor.Log($"Failed to add station (Id: {station.Id}): The location '{station.Location}' cannot be found.", LogLevel.Error);
				return false;
			}

			int mapWidth = location.Map.Layers[0].LayerWidth - 1;
			int mapHeight = location.Map.Layers[0].LayerHeight - 1;

			if (station.Tile.X < 0 || mapWidth < station.Tile.X || station.Tile.Y < 0 || mapHeight < station.Tile.Y)
			{
				ModEntry.Monitor.Log($"Failed to add station (Id: {station.Id}): The station coordinates ({station.Tile.X}, {station.Tile.Y}) are outside the map boundaries ({mapWidth} x {mapHeight}).", LogLevel.Error);
				return false;
			}
			if (station.AccessTiles is not null)
			{
				for (int i = 0; i < station.AccessTiles.Count; i++)
				{
					if (station.AccessTiles[i].X < 0 || mapWidth < station.AccessTiles[i].X || station.AccessTiles[i].Y < 0 || mapHeight < station.AccessTiles[i].Y)
					{
						ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) access tile no.{i + 1}: The access tile coordinates ({station.Tile.X}, {station.Tile.Y}) are outside the map boundaries ({mapWidth} x {mapHeight}).", LogLevel.Warn);
						station.AccessTiles.RemoveAt(i--);
						continue;
					}
				}
			}
			if (string.IsNullOrWhiteSpace(station.DisplayName))
			{
				station.DisplayName = location.DisplayName ?? location.Name;
			}
			if (station.Conditions is not null)
			{
				for (int i = 0; i < station.Conditions.Count; i++)
				{
					if (!string.IsNullOrWhiteSpace(station.Conditions[i].Query))
					{
						string error = GameStateQuery.Parse(station.Conditions[i].Query)[0].Error;

						if (error is not null)
						{
							ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) condition no.{i + 1}: {error}. The condition will be ignored.", LogLevel.Warn);
							station.Conditions.RemoveAt(i--);
							continue;
						}
					}
				}
			}
			if (station.Events is not null)
			{
				for (int i = 0; i < station.Events.Count; i++)
				{
					GameLocation eventLocation = Game1.getLocationFromName(station.Events[i].Location ?? station.Location);

					if (eventLocation is null)
					{
						ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) event no.{i + 1}: The event location '{station.Events[i].Location ?? station.Location}' cannot be found. The event will be ignored.", LogLevel.Warn);
						station.Events.RemoveAt(i--);
						continue;
					}
					if (!string.IsNullOrWhiteSpace(station.Events[i].Id) && string.IsNullOrWhiteSpace(station.Events[i].Script))
					{
						if (eventLocation.findEventById(station.Events[i].Id, Game1.player) is null)
						{
							ModEntry.Monitor.Log($"Failed to set station (Id: {station.Id}) event no.{i + 1}: The event with Id '{station.Events[i].Id}' cannot be found in location '{eventLocation.Name}'. The event will be ignored.", LogLevel.Warn);
							station.Events.RemoveAt(i--);
							continue;
						}
					}
				}
			}
			return true;
		}

		public static void ComputeProperties(Station targetStation = null)
		{
			if (ModEntry.Stations is not null)
			{
				foreach (Station station in ModEntry.Stations)
				{
					if (targetStation is null || station == targetStation)
					{
						if (Utility.TryParseDirection(station.Direction, out int direction))
						{
							station.DirectionAsInt = direction;
						}
						if (station.Sprites is not null)
						{
							for (int i = 0; i < station.Sprites.Count; i++)
							{
								if (station.Sprites[i].Data.SourceRectangle == Rectangle.Empty)
								{
									station.Sprites[i].Data.SourceRectangle = station.Sprites[i].Data.Texture.Bounds;
								}
								station.Sprites[i].Data.ComputedPosition = station.Sprites[i].Data.Position * Game1.tileSize;
								if (station.Sprites[i].Data.LayerDepth == -1)
								{
									station.Sprites[i].Data.ComputedLayerDepth = (station.Sprites[i].Data.ComputedPosition.Y + (station.Sprites[i].Data.Texture.Height * station.Sprites[i].Data.Scale * Game1.pixelZoom)) / 10000f;
								}
								else
								{
									station.Sprites[i].Data.ComputedLayerDepth = station.Sprites[i].Data.LayerDepth * Game1.tileSize / 10000f;
								}
								if (station.Sprites[i].Data.Color is not null)
								{
									Color? color = Utility.StringToColor(station.Sprites[i].Data.Color);

									if (color.HasValue)
									{
										station.Sprites[i].Data.ColorAsColor = color.Value;
									}
									else
									{
										ModEntry.Monitor.Log($"Failed to set 'Color' for station (Id: {station.Id}) sprite no.{i + 1}: The color '{station.Sprites[i].Data.Color}' is not valid. The default value ('White') will be used.", LogLevel.Warn);
									}
								}
								if (station.Sprites[i].Data.Light is not null)
								{
									if (station.Sprites[i].Data.Light.Color is not null)
									{
										Color? color = Utility.StringToColor(station.Sprites[i].Data.Light.Color);

										if (color.HasValue)
										{
											station.Sprites[i].Data.Light.ColorAsColor = color.Value;
										}
										else
										{
											ModEntry.Monitor.Log($"Failed to set 'Color' for station (Id: {station.Id}) sprite no.{i + 1} light: The color '{station.Sprites[i].Data.Light.Color}' is not valid. The default value ('White') will be used.", LogLevel.Warn);
										}
									}
								}
								if (station.Sprites[i].CollisionBoxes is not null)
								{
									station.Sprites[i].ComputedCollisionBoxes = new();
									for (int j = 0; j < station.Sprites[i].CollisionBoxes.Count; j++)
									{
										station.Sprites[i].ComputedCollisionBoxes.Add(new Rectangle((int)(station.Sprites[i].CollisionBoxes[j].X * Game1.tileSize), (int)(station.Sprites[i].CollisionBoxes[j].Y * Game1.tileSize), (int)(station.Sprites[i].CollisionBoxes[j].Width * Game1.tileSize), (int)(station.Sprites[i].CollisionBoxes[j].Height * Game1.tileSize)));
									}
									station.Sprites[i].AbsoluteCollisionBoxes = new();
									for (int j = 0; j < station.Sprites[i].ComputedCollisionBoxes.Count; j++)
									{
										station.Sprites[i].AbsoluteCollisionBoxes.Add(new Rectangle((int)(station.Sprites[i].Data.ComputedPosition.X + station.Sprites[i].ComputedCollisionBoxes[j].X), (int)(station.Sprites[i].Data.ComputedPosition.Y + station.Sprites[i].ComputedCollisionBoxes[j].Y), (int)station.Sprites[i].ComputedCollisionBoxes[j].Width, (int)station.Sprites[i].ComputedCollisionBoxes[j].Height));
									}
								}
							}
						}
					}
				}
			}
		}

		public static void Localize(Station targetStation = null)
		{
			if (ModEntry.Stations is not null)
			{
				foreach (Station station in ModEntry.Stations)
				{
					if (targetStation is null || station == targetStation)
					{
						station.LocalizedDisplayName = TokensUtility.ParseText(station, station.DisplayName);
					}
				}
			}
		}

		public static void GenerateCurrentLocationEnumerable()
		{
			ModEntry.CurrentLocationStations = ModEntry.Stations?.Where(s => s.Location == Game1.currentLocation?.Name) ?? Enumerable.Empty<Station>();
		}

		public static void GenerateUpdateEnumerables()
		{
			ModEntry.OnDayStartStations = ModEntry.Stations?.Where(s => s.Conditions?.Any(c => c.Update.Equals("OnDayStart")) == true);
			ModEntry.OnInteractStations = ModEntry.Stations?.Where(s => s.Conditions?.Any(c => c.Update.Equals("OnInteract")) == true);
			ModEntry.OnDayStartSprites = ModEntry.Stations?.Where(s => s.Sprites is not null)?.SelectMany(s => s.Sprites.Where(sprite => sprite.Conditions?.Any(c => c.Update.Equals("OnDayStart")) == true));
			ModEntry.OnLocationChangeSprites = ModEntry.Stations?.Where(s => s.Sprites is not null)?.SelectMany(s => s.Sprites.Where(sprite => sprite.Conditions?.Any(c => c.Update.Equals("OnLocationChange")) == true));
			ModEntry.OnDayStartSpriteConditions = ModEntry.Stations?.Where(s => s.Sprites is not null)?.SelectMany(s => s.Sprites.Where(s => s.Conditions is not null))?.SelectMany(s => s.Conditions.Where(c => c.Update.Equals("OnDayStart")));
			ModEntry.OnLocationChangeSpriteConditions = ModEntry.Stations?.Where(s => s.Sprites is not null)?.SelectMany(s => s.Sprites.Where(s => s.Conditions is not null))?.SelectMany(s => s.Conditions.Where(c => c.Update.Equals("OnLocationChange")));
			ModEntry.OnDayStartConditions = ModEntry.Stations?.Where(s => s.Conditions is not null)?.SelectMany(s => s.Conditions.Where(c => c.Update.Equals("OnDayStart")));
			ModEntry.OnInteractConditions = ModEntry.Stations?.Where(s => s.Conditions is not null)?.SelectMany(s => s.Conditions.Where(c => c.Update.Equals("OnInteract")));
		}

		public static void UpdateOnSaveLoad(Station station = null)
		{
			UpdateOnDayStart(station);
			UpdateOnLocationChange(station);
			UpdateOnInteract(station);
		}

		public static void UpdateOnDayStart(Station station = null)
		{
			UpdateSpriteConditions(ModEntry.OnDayStartSpriteConditions, station);
			UpdateSprites(ModEntry.OnDayStartSprites, station);
			UpdateConditions(ModEntry.OnDayStartConditions, station);
			UpdateStations(ModEntry.OnDayStartStations, station);
		}

		public static void UpdateOnLocationChange(Station station = null)
		{
			UpdateSpriteConditions(ModEntry.OnLocationChangeSpriteConditions, station);
			UpdateSprites(ModEntry.OnLocationChangeSprites, station);
		}

		public static void UpdateOnInteract(Station station = null)
		{
			UpdateConditions(ModEntry.OnInteractConditions, station);
			UpdateStations(ModEntry.OnInteractStations, station);
		}

		public static void UpdateStations(IEnumerable<Station> stations, Station targetStation = null)
		{
			if (stations is not null)
			{
				foreach (Station station in stations)
				{
					if (targetStation is null || station == targetStation)
					{
						bool conditions = true;

						if (station.Conditions is not null)
						{
							foreach (SCondition condition in station.Conditions)
							{
								if (!condition.Cache)
								{
									conditions = false;
									station.ConditionsLockedMessageCache = TokensUtility.ParseText(station, condition.LockedMessage);
									break;
								}
							}
						}
						station.ConditionsCache = conditions;
					}
				}
			}
		}

		public static void UpdateSprites(IEnumerable<SSprite> sprites, Station targetStation = null)
		{
			if (sprites is not null)
			{
				foreach (SSprite sprite in sprites)
				{
					if (targetStation is null || sprite.Station == targetStation)
					{
						bool conditions = true;

						if (sprite.Conditions is not null)
						{
							foreach (SSCondition condition in sprite.Conditions)
							{
								if (!condition.Cache)
								{
									conditions = false;
									break;
								}
							}
						}
						sprite.ConditionsCache = conditions;
					}
				}
			}
		}

		public static void UpdateSpriteConditions(IEnumerable<SSCondition> conditions, Station targetStation = null)
		{
			if (conditions is not null)
			{
				foreach (SSCondition condition in conditions)
				{
					if (targetStation is null || condition.Sprite.Station == targetStation)
					{
						condition.Cache = QueriesUtility.CheckConditions(condition, condition.Query);
					}
				}
			}
		}

		public static void UpdateConditions(IEnumerable<SCondition> conditions, Station targetStation = null)
		{
			if (conditions is not null)
			{
				foreach (SCondition condition in conditions)
				{
					if (targetStation is null || condition.Station == targetStation)
					{
						condition.Cache = QueriesUtility.CheckConditions(condition, condition.Query);
					}
				}
			}
		}

		public static void ClearEnumerables()
		{
			ModEntry.CurrentLocationStations = null;
			ModEntry.OnDayStartStations = null;
			ModEntry.OnInteractStations = null;
			ModEntry.OnDayStartSprites = null;
			ModEntry.OnLocationChangeSprites = null;
			ModEntry.OnDayStartSpriteConditions = null;
			ModEntry.OnLocationChangeSpriteConditions = null;
			ModEntry.OnDayStartConditions = null;
			ModEntry.OnInteractConditions = null;
		}
	}
}
