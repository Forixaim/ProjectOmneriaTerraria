using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectOmneriaTerraria.Biomes;
using ProjectOmneriaTerraria.Items;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace ProjectOmneriaTerraria.NPCs.TownNPCs
{

	[AutoloadHead]
	public class CosmonianCharlemagne : ModNPC
	{

		private World.WorldValues _localWorldValues = ModContent.GetInstance<World.WorldValues>();
		private static Mod _calamityMod;
		private bool _calamityModCheck = ModLoader.TryGetMod("CalamityMod", out _calamityMod);
		private static Mod _starsAbove;
		private bool _starsAboveCheck = ModLoader.TryGetMod("StarsAbove", out _starsAbove);
		private static Mod _fargos;
		private bool _fargosCheck = ModLoader.TryGetMod("Fargowiltas", out _fargos);


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternal Emberlight Empress");
			//Use Laevateinn to defend herself
			Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 100; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 3; // The type of attack the NPC does. 0 is a melee attack, 1 is a projectile attack, 2 is a magic attack, 3 is a summon attack, and 4 is a ranged attack.
			NPCID.Sets.AttackTime[Type] = 10; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 100;
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
							  // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
							  // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};
			//Happiness
			//Loves to live in space or underworld
			NPC.Happiness
				.SetBiomeAffection<UnderworldBiome>(AffectionLevel.Love)
				.SetBiomeAffection<SpaceBiome>(AffectionLevel.Like)
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Hate)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
				;
		}


		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 25;
			NPC.defense = 250;
			NPC.lifeMax = 25000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 1f;
			NPC.GivenName = "Charlemagne";
			NPC.stepSpeed = 2f;
			AnimationType = NPCID.DyeTrader;
			NPC.immortal = true;
			NPC.dontTakeDamage = true;
		}
		//AI/
		public override void AI()
		{
			//Always have the npc emit some form of light
			Lighting.AddLight(NPC.Center, Color.OrangeRed.ToVector3() * 5f);
			//
			if (!NPC.HasGivenName || NPC.GivenName != "Charlemagne Halphas Flaron")
			{
				NPC.GivenName = "Charlemagne Halphas Flaron";
			}
			//check the NPCID.sets.DangerDetectRange
			//override the code that makes the NPC run away from hostile NPCs
		}
		//Edit the weapon swung
		public override void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
		{
			//manually load the item
			Main.instance.LoadItem(ItemType<OriginLaevateinn>());
			item = TextureAssets.Item[ItemType<OriginLaevateinn>()].Value;
			itemSize = 40;
			scale = 1f;

		}
		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 60);
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = NPC.damage;
			//Simple damage modifier system
			float damageModifier = 1f;
			//if king slime is defeated, increase damage by 5%
			if (NPC.downedSlimeKing)
			{
				damageModifier += 0.05f;
			}
			//If the eye of cthulhu is defeated, increase damage by 10%
			if (NPC.downedBoss1)
			{
				damageModifier += 0.1f;
			}
			//If the eater of worlds is defeated, increase damage by 15%
			if (NPC.downedBoss2)
			{
				damageModifier += 0.15f;
			}
			//if skeletron is defeated, increase damage by 20%
			if (NPC.downedBoss3)
			{
				damageModifier += 0.2f;
			}
			//If the queen bee is defeated, increase damage by 10%
			if (NPC.downedQueenBee)
			{
				damageModifier += 0.1f;
			}
			//If the world is in hardmode, increase damage by 100%
			if (Main.hardMode)
			{
				damageModifier += 1f;
			}
			//If one of the mech bosses is defeated, increase damage by 50%
			if (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3)
			{
				damageModifier += 0.5f;
			}
			//if 2 of the mech bosses are defeated, increase damage by 100%
			if ((NPC.downedMechBoss1 && NPC.downedMechBoss2) || (NPC.downedMechBoss1 && NPC.downedMechBoss3) || (NPC.downedMechBoss2 && NPC.downedMechBoss3))
			{
				damageModifier += 1f;
			}
			//if all 3 of the mech bosses are defeated, increase damage by 200%
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				damageModifier += 2f;
			}
			//If the plantera is defeated, increase damage by 50%
			if (NPC.downedPlantBoss)
			{
				damageModifier += 0.5f;
			}
			//If the golem is defeated, increase damage by 50%
			if (NPC.downedGolemBoss)
			{
				damageModifier += 0.5f;
			}
			//if duke fishron is defeated, increase damage by 50%
			if (NPC.downedFishron)
			{
				damageModifier += 0.5f;
			}
			//if cultist is defeated, increase damage by 50%
			if (NPC.downedAncientCultist)
			{
				damageModifier += 0.5f;
			}
			//if moon lord is defeated, increase damage by 100%
			if (NPC.downedMoonlord)
			{
				damageModifier += 1f;
			}
			//if queen slime is defeated, increase damage by 50%
			if (NPC.downedQueenSlime)
			{
				damageModifier += 0.5f;
			}
			//if empress of light is defeated, increase damage by 50%
			if (NPC.downedEmpressOfLight)
			{
				damageModifier += 0.5f;
			}
			//Modded bosses
			//Calamity

			if (_calamityModCheck)
			{
				//If Desert Scourge is defeated, increase damage by 5%
				if ((bool)_calamityMod.Call("GetBossDowned", "desertscourge"))
				{
					damageModifier += 0.05f;
				}

				//If crabulon is defeated, increase damage by 10%
				if ((bool)_calamityMod.Call("GetBossDowned", "crabulon"))
				{
					damageModifier += 0.1f;
				}

				//If Perforator or Hive Mind is defeated, increase damage by 10%
				if ((bool)_calamityMod.Call("GetBossDowned", "perforator") ||
					(bool)_calamityMod.Call("GetBossDowned", "hivemind"))
				{
					damageModifier += 0.1f;
				}

				//If Slime God is defeated, increase damage by 15%
				if ((bool)_calamityMod.Call("GetBossDowned", "slimegod"))
				{
					damageModifier += 0.15f;
				}

				//Calamity Hardmode
				//If Cryogen is defeated, increase damage by 25%
				if ((bool)_calamityMod.Call("GetBossDowned", "cryogen"))
				{
					damageModifier += 0.25f;
				}

				//If Aquatic Scourge is defeated, increase damage by 25%
				if ((bool)_calamityMod.Call("GetBossDowned", "aquaticscourge"))
				{
					damageModifier += 0.25f;
				}

				//If Brimstone Elemental is defeated, increase damage by 25%
				if ((bool)_calamityMod.Call("GetBossDowned", "brimstoneelemental"))
				{
					damageModifier += 0.25f;
				}

				//If Calamitas Clone is defeated, increase damage by 50%
				if ((bool)_calamityMod.Call("GetBossDowned", "calamitas"))
				{
					damageModifier += 0.5f;
				}

				//If Anahita and Leviathan are defeated, increase damage by 50% (They are one boss in the Call Function)
				if ((bool)_calamityMod.Call("GetBossDowned", "anahitaleviathan"))
				{
					damageModifier += 0.5f;
				}

				//If Ravager is defeated, increase damage by 100%
				if ((bool)_calamityMod.Call("GetBossDowned", "ravager"))
				{
					damageModifier += 1f;
				}

				//If Plaguebringer Goliath is defeated, increase damage by 100%
				if ((bool)_calamityMod.Call("GetBossDowned", "plaguebringergoliath"))
				{
					damageModifier += 1f;
				}

			}

			//Temporarily casts the damage to a float, multiplies it by the damage modifier, then casts it back to an int
			damage = (int)(damage * damageModifier);
			knockback = 0f;
		}
		public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
		{
			itemWidth = 40;
			itemHeight = 40;
		}
		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 1;
			randExtraCooldown = 1;
		}

		//Dialogue from en-US.json
		public override string GetChat()
		{
			//Get the player
			Player player = Main.player[Main.myPlayer];
			WeightedRandom<string> chat = new WeightedRandom<string>();
			ModNPC Calamitas;
			ModNPC Fab;
			ModNPC Permafrost;
			ModNPC Mutant;
			ModNPC Deviantt;
			//An int version of the ModNPCs
			int CalamitasInt = -1;
			int FabInt = -1;
			int PermafrostInt = -1;
			int MutantInt = -1;
			int DevianttInt = -1;
			ModBuff EridaniBlessing;
			ModBuff AsphodeneBlessing;
			ModBuff EverlastingLight;


			if (_fargosCheck)
			{
				Mutant = _fargos.Find<ModNPC>("Mutant");
				Deviantt = _fargos.Find<ModNPC>("Deviantt");
				MutantInt = NPC.FindFirstNPC(Mutant.Type);
				DevianttInt = NPC.FindFirstNPC(Deviantt.Type);
				if (MutantInt >= 0 && Main.rand.NextBool(4))
				{
					chat.Add("Remember, do not take " + Main.npc[MutantInt].GivenName + " lightly. He is extremely powerful. Sure I can beat him without lifting a finger but this is your test. I can not help you cheat.");
				}
				if (DevianttInt >= 0 && Main.rand.NextBool(4))
				{
					chat.Add(Main.npc[DevianttInt].GivenName + " the Deviantt, can be useful, if you want to spar with her go ahead. Don't get me involved in this.");
				}
			}
			if (_calamityModCheck)
			{
				//Saves it as a variable
				bool CalamitasCheck = _calamityMod.TryFind<ModNPC>("WITCH", out Calamitas);
				bool FabCheck = _calamityMod.TryFind<ModNPC>("FAB", out Fab);
				bool PermafrostCheck = _calamityMod.TryFind<ModNPC>("DILF", out Permafrost);
				CalamitasInt = NPC.FindFirstNPC(Calamitas.Type);
				FabInt = NPC.FindFirstNPC(Fab.Type);
				PermafrostInt = NPC.FindFirstNPC(Permafrost.Type);
				//check if player has Ashes of Calamity/
				if (player.HasItem(_calamityMod.Find<ModItem>("AshesofCalamity").Type) && !_localWorldValues.NPCCharlemagneThingSaid2)
				{
					//Give the player Laevateinn but check to see if they already have it and check if the other NPCCharlemagneThingSaid3 is false

					if (!_localWorldValues.NPCCharlemagneThingSaid3)
					{
						var EntitySource = NPC.GetSource_GiftOrReward();
						player.QuickSpawnItem(EntitySource, ModContent.ItemType<Laevateinn>());
						chat.Add("Those ashes... I see. You are about to engage onto a tough fight against the witch. I wish you luck. Here's the only thing I can do, here I've forged you a replica of my blade. Despite being a replica, the blade is extremely powerful. The cosmic inferno will be extremely effective against her. Use it well.", 1000);
					}
					else
					{
						chat.Add("Those ashes... I see. You are about to engage onto a tough fight against the witch. I wish you luck. Use the blade I'd gave to you for your fight against Tsukiyomi, It's stronger against her.", 1000);
					}
					_localWorldValues.NPCCharlemagneThingSaid2 = true;
				}

			}
			//checks if a player has a buff
			if (_starsAboveCheck)
			{
				//Grab the world name
				string worldName = Main.worldName;
				if (!_localWorldValues.NPCCharlemagneThingSaid)
				{
					chat.Add(player.name + ", we need to talk. Seems you have established a connection to another world. The realm of the starfarers have been connected with " + worldName + ". Now we have newer journeys through the stars, and newer threats to vanquish.");
					_localWorldValues.NPCCharlemagneThingSaid = true;
				}

				EridaniBlessing = _starsAbove.Find<ModBuff>("EridaniBlessing");
				AsphodeneBlessing = _starsAbove.Find<ModBuff>("AsphodeneBlessing");
				EverlastingLight = _starsAbove.Find<ModBuff>("EverlastingLight");
				//Saves it as a variable
				if (player.HasBuff(EridaniBlessing.Type))
				{
					chat.Add("Eridani? Ah yes her! Something I know about her is that... yes, we are similar but we are significantly different. I highly dont recommend treating starfarers and cosmonians as the same.");
				}
				else if (player.HasBuff(AsphodeneBlessing.Type))
				{
					chat.Add("Yes, Asphodene is another starfarer. So here's a little thing between you and me... I serve no connection with the starfarers, nor I have anything to do with them. I am a cosmonian, and we are different completely.");
				}
				if (player.HasBuff(EverlastingLight.Type))
				{
					chat.Add("Sheesh, this place is bright. I sense that you have a major problem heading your way... Good luck! Honestly, my brother Uther makes brighter lightshows than whatever this threat may be.");
				}
				if (player.HasBuff(_starsAbove.Find<ModBuff>("SurtrTwilight").Type))
				{
					chat.Add("Ars Laevateinn, what a nice way to name it after my sword. I can see that you have the infernal blade and you will make sure you will protect it.");
				}
				if (player.HasItem(_starsAbove.Find<ModItem>("MnemonicSigil").Type) && !_localWorldValues.NPCCharlemagneThingSaid3)
				{
					if (!_localWorldValues.NPCCharlemagneThingSaid2)
					{
						var EntitySource = NPC.GetSource_GiftOrReward();
						player.QuickSpawnItem(EntitySource, ModContent.ItemType<Laevateinn>());
						chat.Add("So you have found the source for all that trouble, huh? I am glad you have found it. I hope you will be able to stop it. Remember, Tsukiyomi is a tough one, rather brash and also persistent. I'd hope you will give her the disclipline that she needs. Here take Laevateinn, this can help you.", 100);
					}
					else
					{
						chat.Add("So you have found the source for all that trouble, huh? I am glad you have found it. I hope you will be able to stop it. Remember, Tsukiyomi is a tough one, rather brash and also persistent. I'd hope you will give her the disclipline that she needs. Use my sword like you have used on Calamitas, it will help a lot..", 100);
					}
					_localWorldValues.NPCCharlemagneThingSaid3 = true;
				}
			}
			if (CalamitasInt >= 0)
			{
				chat.Add(Main.npc[CalamitasInt].GivenName + "'s flames are impressive indeed, the hellfire of the abyss should not be taken lightly. But my flames of the universe goes beyond. In other terms, I am much stronger, my flames burn hotter and brighter.");
			}
			if (FabInt >= 0)
			{
				chat.Add(Main.npc[FabInt].GivenName + "... as an empress, getting drunk often is not my cup of tea. Proper conduct is a must and therefore I never drink. If you are thinking of offering me a cup I may just ignite the alcohol within it and toss it into your face.");
			}
			if (PermafrostInt >= 0)
			{
				chat.Add("Hey, could you tell " + Main.npc[PermafrostInt].GivenName + " to stay away from me, I highly recommend 40 feet. Him getting near me may be way too warm for the archmage. Especially when I'm in combat when the air can skyrocket up to 7000 degrees kelvin.");
			}
			if (_localWorldValues.CharleFought && _localWorldValues.CharleDeaths == 1 && !_localWorldValues.CharleDefeated)
			{
				chat.Add("Yep, that's one time you actually died to me. I am not surprised, I am the toughest opponent you will face in your life. I suggest you don't get too close to me, I may just burn you to a crisp.");
			}
			if (_localWorldValues.CharleFought && _localWorldValues.CharleDeaths == 2 && !_localWorldValues.CharleDefeated)
			{
				chat.Add("You have died to me twice now. Did you learn what I just said previously? If so here's another tip, every strike i do inflicts Flaron Inferno, they drain your health by the percentage, so no amount of defense or damage reduction will help. The best tactic is to avoid me");
			}
			if (_localWorldValues.CharleFought && _localWorldValues.CharleDeaths == 3 && !_localWorldValues.CharleDefeated)
			{
				chat.Add("To die 3 times to me, honestly you have the determination to come back. I can tell that with enough practice you will be able to land a hit.");
			}
			else if (_localWorldValues.CharleFought && _localWorldValues.CharleDeaths > 3 && !_localWorldValues.CharleDefeated)
			{
				chat.Add("You have died to me " + _localWorldValues.CharleDeaths + " times now. Practice makes perfect.");
			}
			if (_localWorldValues.CharleDefeated && !_localWorldValues.NPCCharlemagneThingSaid4)
			{
				chat.Add("Well, you landed your hit. Nice work. This test is over, and now I honestly don't have anything left for you to do. I hope you have a good day.", 100);
				_localWorldValues.NPCCharlemagneThingSaid4 = true;
			}
			//If a party is active.

			chat.Add("I'm Charlemagne, the Eternal Emberlight Empress hailing from the Teraverse Omneria. Say... I'd expect someone taller, but hey! It's a pleasure to work with you.");
			chat.Add("Since enemies and foes are unable to fight me, I'll let you take the battle, that said, if you need help, I'll be there to give you information.");
			chat.Add("I highly recommend not touching me... My body is around 2500 degrees and you will sear your entire face into oblivion if you do so.");
			return chat;
		}
	}
}