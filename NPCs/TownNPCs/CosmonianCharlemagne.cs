using IL.Terraria.Localization;
using ProjectOmneriaTerraria.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;


//Use tabs instead of spaces

namespace ProjectOmneriaTerraria.NPCs.TownNPCs
{
	[AutoloadHead]
	public class CosmonianCharlemagne : ModNPC
	{
		private static bool CalamitasFought = false;
		private static bool ThingSaid = false;
		private static bool CalamitasPreFightSaid = false;
		private static bool TsukiyomiPreFightSaid = false;
		public static Mod CalamityMod;
		public bool CalamityModCheck = ModLoader.TryGetMod("CalamityMod", out CalamityMod);
		public static Mod StarsAbove;
		public bool StarsAboveCheck = ModLoader.TryGetMod("StarsAbove", out StarsAbove);
		public static Mod Fargos;
		public bool FargosCheck = ModLoader.TryGetMod("Fargowiltas", out Fargos);
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
			NPC.stepSpeed = 2f;
			AnimationType = NPCID.Guide;
			NPC.immortal = true;
			NPC.dontTakeDamage = true;
			
			if (!Main.dedServ)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/cosmic-inferno.ogg");
			}
		}

		//AI
		public override void AI()
		{
			//Always have the npc emit fire particles
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 0f, -60f, default, default, 2f);
			//
		}


		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return true;
		}
		
		public override List<string> SetNPCNameList()
		{
			return new List<string>()
			{
				"Charlemagne"
			};
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
				Calamitas = CalamityMod.Find<ModNPC>("WITCH");
				Fab = CalamityMod.Find<ModNPC>("FAP");
				Permafrost = CalamityMod.Find<ModNPC>("DILF");
				CalamitasInt = NPC.FindFirstNPC(Calamitas.Type);
				FabInt = NPC.FindFirstNPC(Fab.Type);
				PermafrostInt = NPC.FindFirstNPC(Permafrost.Type);
				//check if player has Ashes of Calamity
				if (player.HasItem(CalamityMod.Find<ModItem>("AshesofCalamity").Type) && !CalamitasPreFightSaid)
				{
					//Give the player Laevateinn
					if (!TsukiyomiPreFightSaid)
					{
						var EntitySource = NPC.GetSource_GiftOrReward();
						player.QuickSpawnItem(EntitySource, ModContent.ItemType<Laevateinn>());
					}
					
					chat.Add("Those ashes... I see. You are about to engage onto a tough fight against the witch. I wish you luck. Here's the only thing I can do, here I've forged you a replica of my blade. Despite being a replica, the blade is extremely powerful. The cosmic inferno will be extremely effective against her. Use it well.", 1000);
					CalamitasPreFightSaid = true;
				}
				if ((bool)CalamityMod.Call("GetBossDowned", "supremecalamitas") && !CalamitasFought)
				{
					
					chat.Add("You have defeated the witch, great work. I would like to personally speak to her as soon as I can. Who knows what we can get from each other.");
					CalamitasFought = true;

				}
			}
			//checks if a player has a buff
			if (StarsAbove != null)
			{
				//Grab the world name
				string worldName = Main.worldName;
				if (!ThingSaid)
				{
					chat.Add(player.name + ", we need to talk. Seems you have established a connection to another world. The realm of the starfarers have been connected with " + worldName + ". Now we have newer journeys through the stars, and newer threats to vanquish.");
					ThingSaid = true;
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
				if (player.HasItem(StarsAbove.Find<ModItem>("MnemonicSigil").Type) && !TsukiyomiPreFightSaid)
				{
					if (!CalamitasPreFightSaid)
					{
						var EntitySource = NPC.GetSource_GiftOrReward();
						player.QuickSpawnItem(EntitySource, ModContent.ItemType<Laevateinn>());
						chat.Add("So you have found the source for all that trouble, huh? I am glad you have found it. I hope you will be able to stop it. Remember, Tsukiyomi is a tough one, rather brash and also persistent. I'd hope you will give her the disclipline that she needs. Here take Laevateinn, this can help you.", 100);
					}
					else
					{
						chat.Add("So you have found the source for all that trouble, huh? I am glad you have found it. I hope you will be able to stop it. Remember, Tsukiyomi is a tough one, rather brash and also persistent. I'd hope you will give her the disclipline that she needs. Use my sword like you have used on Calamitas, it will help a lot..", 100);
					}
					TsukiyomiPreFightSaid = true;
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
			chat.Add("I'm Charlemagne, the Eternal Emberlight Empress hailing from the Teraverse Omneria. Say... I'd expect someone taller, but hey! It's a pleasure to work with you.");
			chat.Add("Since enemies and foes are unable to fight me, I'll let you take the battle, that said, if you need help, I'll be there to give you information.");
			chat.Add("I highly recommend not touching me... My body is around 2500 degrees and you will sear your entire face into oblivion if you do so.");
			return chat;
		}
	}
}