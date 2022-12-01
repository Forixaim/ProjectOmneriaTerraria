using IL.Terraria.Localization;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

//Use tabs instead of spaces

namespace ProjectOmneriaTerraria.NPCs.TownNPCs
{
	public class CosmonianCharlemagne : ModNPC
	{
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
			Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0;
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
			AnimationType = NPCID.Guide;
			NPC.immortal = true;
		}
		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedBoss1)
			{
				return true;
			}
			return false;
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
			int CalamitasInt;
			int FabInt;
			int PermafrostInt;
			int MutantInt;
			int DevianttInt;
			ModBuff EridaniBlessing;
			ModBuff AsphodeneBlessing;
			
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
			}
			//checks if a player has a buff
			if (StarsAbove != null)
			{
				//Grab the world name
				string worldName = Main.worldName;
				chat.Add(player.name + ", we need to talk. Seems you have established a connection to another world. The realm of the starfarers have been connected with " + worldName + ". Now we have newer journeys through the stars, and newer threats to vanquish.");
				EridaniBlessing = StarsAbove.Find<ModBuff>("EridaniBlessing");
				AsphodeneBlessing = StarsAbove.Find<ModBuff>("AsphodeneBlessing");
				//Saves it as a variable
				if (player.HasBuff(EridaniBlessing.Type))
				{
					chat.Add("Eridani? Ah yes her! Something I know about her is that... yes, we are similar but we are significantly different. I highly dont recommend treating starfarers and cosmonians as the same.");
				}
				else if (player.HasBuff(AsphodeneBlessing.Type))
				{
					chat.Add("Yes, Asphodene is another starfarer. So here's a little thing between you and me... I serve no connection with the starfarers, nor I have anything to do with them. I am a cosmonian, and we are different completely.");
				}
			}
			chat.Add("Chat isn't done yet, be patient.");
			return chat;
		}
	}
}