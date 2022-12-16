using Microsoft.Xna.Framework;
using Mono.Cecil;
using ProjectOmneriaTerraria.Biomes;
using ProjectOmneriaTerraria.Projectiles;
using System;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ProjectOmneriaTerraria.NPCs.TownNPCs
{
	[AutoloadHead]
	internal class CosmonianKalish : ModNPC
	{
		
		private int _projectile; //_projectile is the projectile that the NPC will shoot 1. Dark Matter Spear 2. Dark Matter Lance 3. Dark Matter Sword
		private static Mod _calamityMod;
		private readonly bool _calamityModCheck = ModLoader.TryGetMod("CalamityMod", out _calamityMod);
		private static Mod _fargos;
		private readonly bool _fargosCheck = ModLoader.TryGetMod("Fargowiltas", out _fargos);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Emperor");
			//Reserve 5 frames for NPC's Attack animation
			Main.npcFrameCount[NPC.type] = 25;

			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 4;
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;
			//AttackType is the type of attack the NPC will use 0 is throw, 1 is shoot, 2 is magic, 3 is melee.
			NPCID.Sets.AttackType[NPC.type] = 2; //Fires Dark Matter Weapons in self defense
			NPCID.Sets.AttackTime[NPC.type] = 20;
			NPCID.Sets.AttackAverageChance[NPC.type] = 90;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;
			NPC.Happiness
				.SetBiomeAffection<SpaceBiome>(AffectionLevel.Love)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Like);
		}
		public override void SetDefaults()
		{
			NPC.height = 42;
			NPC.width = 18;
			NPC.aiStyle = 7;
			NPC.damage = 25;
			NPC.defense = 100;
			NPC.lifeMax = 10000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 1f;
			NPC.value = 10000f;
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.npcSlots = 1f;
			NPC.lavaImmune = true;
			NPC.dontTakeDamage = true;
			NPC.GivenName = "Kalish";
			AnimationType = NPCID.Wizard;
			NPC.reflectsProjectiles = true;
		}

		public override void AI()
		{
			//Emit purple light
			Lighting.AddLight(NPC.Center, Color.Purple.ToVector3() * 2f);
			if (!NPC.HasGivenName || NPC.GivenName != "Kalish Alexander Eridanus")
			{
				NPC.GivenName = "Kalish Alexander Eridanus";
			}
			//Look for the nearest hostile npc
			//CheckForHostiles();
		}

		//Shoots a variety of Dark Matter Weapons (randomly chosen list: Spear, Lance, and Sword)
		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			//Random number between 0 and 2
			//Kept as a short number for memory efficiency
			UInt16 random = (UInt16)Main.rand.Next(3);
			if (random == 0)
			{
				projType = ModContent.ProjectileType<DM_Spear>();
			}
			else if (random == 1)
			{
				projType = ModContent.ProjectileType<DM_Lance>();
			}
			else if (random == 2)
			{
				projType = ModContent.ProjectileType<DM_Sword>();
			}
			attackDelay = 1;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			//Damage will be calculated by CosmonianCharlemagne's damage modifier system
			damage = NPC.damage;
			//Simple damage modifier system
			float damageModifier = 1f;
			float weaponTypeModifier = 1f;
			float totalDamageModifier = 1f;
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
			//check the _projectile int to see what projectile is being used
			weaponTypeModifier = _projectile switch
			{
				0 => 1.5f,
				1 => 3f,
				_ => 1f
			};
			totalDamageModifier = damageModifier * weaponTypeModifier;
			//Temporarily casts the damage to a float, multiplies it by the damage modifier, then casts it back to an int
			damage = (int)(damage * totalDamageModifier);
			knockback = 0f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 0;
			randExtraCooldown = 0;
		}

		public override void TownNPCAttackMagic(ref float auraLightMultiplier)
		{
			//double it
			auraLightMultiplier *= 2f;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			//Spawns initially in hardmode
			return Main.hardMode;
		}

		//a few private methods to help with the code
		private void CheckForHostiles()
		{
			//check for hostiles nearby
			//First a scanning distance is set
			int scanDistance = NPCID.Sets.DangerDetectRange[Type];
			//Then a rectangle is created with the scanning distance
			Rectangle scanArea = new Rectangle((int)NPC.position.X - scanDistance, (int)NPC.position.Y - scanDistance, NPC.width + scanDistance * 2, NPC.height + scanDistance * 2);
			//Check if any NPCs are in the scan area
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				//If the NPC is hostile and is in the scan area, set _hostile to true
				if (Main.npc[i].active && !(Main.npc[i].friendly) && Main.npc[i].type != NPCID.TargetDummy && scanArea.Intersects(Main.npc[i].Hitbox) && !(Main.npc[i].CountsAsACritter))
				{
					//walk towards the hostile NPC
					NPC.velocity = NPC.DirectionTo(Main.npc[i].Center) * 2f;
					//When the NPC is close enough, fire a Dark Matter Spear
					if (NPC.Distance(Main.npc[i].Center) < 100f)
					{
						//Set the projectile to Dark Matter Spear
						_projectile = 0;
						NPC.target = Main.npc[i].whoAmI;
						//Fire the projectile
						ShootProjectile(Main.npc[i]);
					}
				}
			}
		}
		
		private void ShootProjectile(NPC TargetNPC)
		{
			//Create initial variables
			var entitySource = NPC.GetSource_FromAI();
			
			//First a random number is generated from 0 to 2
			int random = Main.rand.Next(3);
			
			//A if statement because a switch case doesn't work
			if (random == 0)
			{
				//If the random number is 0, set the projectile to Dark Matter Spear
				_projectile = ModContent.ProjectileType<DM_Spear>();
			}
			else if (random == 1)
			{

				_projectile = ModContent.ProjectileType<DM_Lance>();
			}
			else
			{
				_projectile = ModContent.ProjectileType<DM_Sword>();
			}
			//Then the projectile is fired
			Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(TargetNPC.Center) * 10f, _projectile, NPC.damage, NPC.type);
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();
			int Charlemagne = NPC.FindFirstNPC(ModContent.NPCType<CosmonianCharlemagne>());
			//Add chat options here
			chat.Add("Look mortal, I have nothing to do with this but there is something really weird with this planet.");
			chat.Add("Do you really think that a simple little creature that will kill me? The truth is, no one can kill me for now.");
			if (_calamityModCheck)
			{
				chat.Add("If you want any help on the creatures of calamity, I'm sorry but I cannot help.");
				if (NPC.FindFirstNPC(_calamityMod.Find<ModNPC>("WITCH").Type) >= 0)
				{
					chat.Add("The brimstone witch isn't my cup of tea, I prefer the cosmos. However, if you insist on me fighting her, rest assured that she will lose every fight.");
				}

				if (Main.player[Main.myPlayer].HasBuff(_calamityMod.Find<ModBuff>("ChibiiDoGBuff").Type))
				{
					chat.Add("This little... Nevermind, I can't remember how many times do I have to discipline a long serpent again. Anyway, be aware and stay in bright areas. There's no telling if she can stab you in the back.");
				}
			}

			if (_fargosCheck)
			{
				chat.Add("Sigh... truth to be told, I truly didn't mean that those three would come. I would ask you to not drag me into a fight with their summons.");
				if (NPC.FindFirstNPC(_fargos.Find<ModNPC>("Mutant").Type) >= 0)
				{
					chat.Add("Mortal, would you ask " + Main.npc[NPC.FindFirstNPC((_fargos.Find<ModNPC>("Mutant").Type))] + " to come talk to me, he has pissed me off way too many times on how he keeps ripping me off and overpricing the things I gave to him.");
				}
			}

			if (Charlemagne >= 0)
			{
				chat.Add("Charlemagne is my younger cousin, well as the third daughter of reality, she does have a lot of power within her. I don't recommend you treating her on the same caliber as the threats we have faced.");
			}

			//Returns a random chat option
			return chat;
		}

		//Modify the speed of the projectile
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = _projectile switch
			{
				0 => 25f,
				1 => 10f,
				_ => 5f,
			};

		}
	}
}
