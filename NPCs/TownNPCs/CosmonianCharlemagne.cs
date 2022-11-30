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
		public Mod CalamityMod = ModLoader.GetMod("CalamityMod");
		public Mod StarsAbove = ModLoader.GetMod("StarsAbove");
		public Mod Fargos = ModLoader.GetMod("Fargowiltas");
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternal Emberlight Empress");
			//Use Laevateinn to defend herself
			Main.npcFrameCount[Type] = 4;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4;	
		}
		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
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
					chat.Add(Main.npc[DevianttInt].GivenName + " the Deviantt, can be useful, if you want to spar with her go ahead. Don't get me involved in this.")
				}
            }
			if (CalamityMod != null)
			{
				//Saves it as a variable
				Calamitas = CalamityMod.Find<ModNPC>("WITCH");
				Fab = CalamityMod.Find<ModNPC>("FAP");
				Permafrost = CalamityMod.Find<ModNPC>("DILF");
				chat.Add("Hey, could you tell " + Permafrost.NPC.GivenName + " to stay away from me, I highly recommend 40 feet. Him getting near me may be way too warm for the archmage. Especially when I'm in combat when the air can skyrocket up to 7000 degrees kelvin.");
				chat.Add(Calamitas.NPC.GivenName + "'s flames are impressive indeed, the hellfire of the abyss should not be taken lightly. But my flames of the universe goes beyond. In other terms, I am much stronger, my flames burn hotter and brighter.");
				chat.Add(Fab.NPC.GivenName + "As an empress, getting drunk often is not my cup of tea. Proper conduct is a must and therefore I never drink. If you are thinking of offering me a cup I may just ignite the alcohol within it and toss it into your face.");
				CalamitasInt = NPC.FindFirstNPC(Calamitas.Type);
				FabInt = NPC.FindFirstNPC(Fab.Type);
                PermafrostInt = NPC.FindFirstNPC(Permafrost.Type);
                if 
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
					chat.Add("Eridani? Ah yes her! Something I know about her is that yes, we are similar but we are significantly different. I highly dont recommend treating starfarers and cosmonians as the same.");
				}
				else if (player.HasBuff(AsphodeneBlessing.Type))
				{
					chat.Add("Yes, Asphodene is another starfarer. So here's a little thing between you and me... I serve no connection with the starfarers, nor I have anything to do with them. I am a cosmonian, and we are different completely.");
				}
            }
            //Checks if Calamitas is alive
            
            chat.Add("");
            return chat;
		}
	}
}