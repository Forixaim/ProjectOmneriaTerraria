using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using ProjectOmneriaTerraria.Biomes;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.DataStructures;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using ProjectOmneriaTerraria.Items;

//Use tabs instead of spaces

namespace ProjectOmneriaTerraria.NPCs.TownNPCs
{
	[AutoloadHead]
	public class CosmonianCharlemagne : ModNPC
	{
		private World.WorldValues LocalWorldValues = ModContent.GetInstance<World.WorldValues>();
		private static Mod CalamityMod;
		private bool CalamityModCheck = ModLoader.TryGetMod("CalamityMod", out CalamityMod);
		private static Mod StarsAbove;
		private bool StarsAboveCheck = ModLoader.TryGetMod("StarsAbove", out StarsAbove);
		private static Mod Fargos;
		private bool FargosCheck = ModLoader.TryGetMod("Fargowiltas", out Fargos);
		private static ModNPC Tsuki = null;
		private bool TsukiCheck = StarsAbove.TryFind<ModNPC>("Tsuki", out Tsuki);
		private bool BossFightBegins;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternal Emberlight Empress");
			//Use Laevateinn to defend herself
			Main.npcFrameCount[Type] = 21; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 0;
			NPCID.Sets.DangerDetectRange[Type] = 0; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0; // The type of attack the NPC does. 0 is a melee attack, 1 is a projectile attack, 2 is a magic attack, 3 is a summon attack, and 4 is a ranged attack.
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 30;
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
			NPC.damage = 1000000000;
			NPC.defense = 250000;
			NPC.lifeMax = 250000000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 1f;
			NPC.GivenName = "Charlemagne";
			NPC.stepSpeed = 2f;
			AnimationType = NPCID.Guide;
			NPC.immortal = true;
			NPC.dontTakeDamage = true;
			//Sounds.Music.CosmicInferno.ogg
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/cosmic-inferno.ogg");
		}

		//AI/
		public override void AI()
		{
			//Always have the npc emit fire particles
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 0f, -60f, default, default, 2f);
			//
			if (!NPC.HasGivenName)
			{
				NPC.GivenName = "Charlemagne";
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return true;
		}
		
		
		//Make Charlemagne use her Laevateinn to defend herself
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
			
			
			if (Fargos != null)
			{
				Mutant = Fargos.Find<ModNPC>("Mutant");
				Deviantt = Fargos.Find<ModNPC>("Deviantt");
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
			if (CalamityMod != null)
			{
				//Saves it as a variable
				bool CalamitasCheck = CalamityMod.TryFind<ModNPC>("WITCH", out Calamitas);
				bool FabCheck = CalamityMod.TryFind<ModNPC>("FAB", out Fab);
				bool PermafrostCheck = CalamityMod.TryFind<ModNPC>("DILF", out Permafrost);
				CalamitasInt = NPC.FindFirstNPC(Calamitas.Type);
				FabInt = NPC.FindFirstNPC(Fab.Type);
				PermafrostInt = NPC.FindFirstNPC(Permafrost.Type);
				//check if player has Ashes of Calamity/
				if (player.HasItem(CalamityMod.Find<ModItem>("AshesofCalamity").Type) && !LocalWorldValues.NPCCharlemagneThingSaid2)
				{
					//Give the player Laevateinn but check to see if they already have it and check if the other NPCCharlemagneThingSaid3 is false
					
					if (!LocalWorldValues.NPCCharlemagneThingSaid3)
					{
						var EntitySource = NPC.GetSource_GiftOrReward();
						player.QuickSpawnItem(EntitySource, ModContent.ItemType<Laevateinn>());
						chat.Add("Those ashes... I see. You are about to engage onto a tough fight against the witch. I wish you luck. Here's the only thing I can do, here I've forged you a replica of my blade. Despite being a replica, the blade is extremely powerful. The cosmic inferno will be extremely effective against her. Use it well.", 1000);
					}
					else
					{
						chat.Add("Those ashes... I see. You are about to engage onto a tough fight against the witch. I wish you luck. Use the blade I'd gave to you for your fight against Tsukiyomi, It's stronger against her.", 1000);
					}
					LocalWorldValues.NPCCharlemagneThingSaid2 = true;
				}
			}
			//checks if a player has a buff
			if (StarsAbove != null)
			{
				//Grab the world name
				string worldName = Main.worldName;
				if (!LocalWorldValues.NPCCharlemagneThingSaid)
				{
					chat.Add(player.name + ", we need to talk. Seems you have established a connection to another world. The realm of the starfarers have been connected with " + worldName + ". Now we have newer journeys through the stars, and newer threats to vanquish.");
					LocalWorldValues.NPCCharlemagneThingSaid = true;
				}
				
				EridaniBlessing = StarsAbove.Find<ModBuff>("EridaniBlessing");
				AsphodeneBlessing = StarsAbove.Find<ModBuff>("AsphodeneBlessing");
				EverlastingLight = StarsAbove.Find<ModBuff>("EverlastingLight");
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
				if (player.HasBuff(StarsAbove.Find<ModBuff>("SurtrTwilight").Type))
				{
					chat.Add("Ars Laevateinn, what a nice way to name it after my sword. I can see that you have the infernal blade and you will make sure you will protect it.");
				}
				if (player.HasItem(StarsAbove.Find<ModItem>("MnemonicSigil").Type) && !LocalWorldValues.NPCCharlemagneThingSaid3)
				{
					if (!LocalWorldValues.NPCCharlemagneThingSaid2)
					{
						var EntitySource = NPC.GetSource_GiftOrReward();
						player.QuickSpawnItem(EntitySource, ModContent.ItemType<Laevateinn>());
						chat.Add("So you have found the source for all that trouble, huh? I am glad you have found it. I hope you will be able to stop it. Remember, Tsukiyomi is a tough one, rather brash and also persistent. I'd hope you will give her the disclipline that she needs. Here take Laevateinn, this can help you.", 100);
					}
					else
					{
						chat.Add("So you have found the source for all that trouble, huh? I am glad you have found it. I hope you will be able to stop it. Remember, Tsukiyomi is a tough one, rather brash and also persistent. I'd hope you will give her the disclipline that she needs. Use my sword like you have used on Calamitas, it will help a lot..", 100);
					}
					LocalWorldValues.NPCCharlemagneThingSaid3 = true;
				}
			}
			if (CalamitasInt >= 0 && Main.rand.NextBool(4))
			{
				chat.Add(Main.npc[CalamitasInt].GivenName + "'s flames are impressive indeed, the hellfire of the abyss should not be taken lightly. But my flames of the universe goes beyond. In other terms, I am much stronger, my flames burn hotter and brighter.");
			}
			if (FabInt >= 0 && Main.rand.NextBool(4))
			{
				chat.Add(Main.npc[FabInt].GivenName + "... as an empress, getting drunk often is not my cup of tea. Proper conduct is a must and therefore I never drink. If you are thinking of offering me a cup I may just ignite the alcohol within it and toss it into your face.");
			}
			if (PermafrostInt >= 0 && Main.rand.NextBool(4))
			{
				chat.Add("Hey, could you tell " + Main.npc[PermafrostInt].GivenName + " to stay away from me, I highly recommend 40 feet. Him getting near me may be way too warm for the archmage. Especially when I'm in combat when the air can skyrocket up to 7000 degrees kelvin.");
			}
			if (LocalWorldValues.CharleFought && LocalWorldValues.CharleDeaths == 1 && !LocalWorldValues.CharleDefeated)
			{
				chat.Add("Yep, that's one time you actually died to me. I am not surprised, I am the toughest opponent you will face in your life. I suggest you don't get too close to me, I may just burn you to a crisp.");
			}
			if (LocalWorldValues.CharleFought && LocalWorldValues.CharleDeaths == 2 && !LocalWorldValues.CharleDefeated)
			{
				chat.Add("You have died to me twice now. Did you learn what I just said previously? If so here's another tip, every strike i do inflicts Flaron Inferno, they drain your health by the percentage, so no amount of defense or damage reduction will help. The best tactic is to avoid me");
			}
			if (LocalWorldValues.CharleFought && LocalWorldValues.CharleDeaths == 3 && !LocalWorldValues.CharleDefeated)
			{
				chat.Add("To die 3 times to me, honestly you have the determination to come back. I can tell that with enough practice you will be able to land a hit.");
			}
			else if (LocalWorldValues.CharleFought && LocalWorldValues.CharleDeaths > 3 && !LocalWorldValues.CharleDefeated)
			{
				chat.Add("You have died to me " + LocalWorldValues.CharleDeaths + " times now. Practice makes perfect.");
			}
			if (LocalWorldValues.CharleDefeated && !LocalWorldValues.NPCCharlemagneThingSaid4)
			{
				chat.Add("Well, you landed your hit. Nice work. This test is over, and now I honestly don't have anything left for you to do. I hope you have a good day.", 100);
				LocalWorldValues.NPCCharlemagneThingSaid4 = true;
			}
			chat.Add("I'm Charlemagne, the Eternal Emberlight Empress hailing from the Teraverse Omneria. Say... I'd expect someone taller, but hey! It's a pleasure to work with you.");
			chat.Add("Since enemies and foes are unable to fight me, I'll let you take the battle, that said, if you need help, I'll be there to give you information.");
			chat.Add("I highly recommend not touching me... My body is around 2500 degrees and you will sear your entire face into oblivion if you do so.");
			return chat;
		}
	}
}