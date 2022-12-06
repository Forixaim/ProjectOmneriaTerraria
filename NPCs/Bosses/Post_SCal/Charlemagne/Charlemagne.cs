using Microsoft.Xna.Framework;
using ProjectOmneriaTerraria.Buffs.Harmful.Damaging;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace ProjectOmneriaTerraria.NPCs.Bosses.Post_SCal.Charlemagne
{
	[AutoloadBossHead]
	internal class Charlemagne : ModNPC
	{
		private UInt16 seconds = 0; //Maximim seconds is 65,535
		private UInt16 minutes = 0; //Maximum minutes is 65,535
		private UInt16 hours = 0; //Maximum hours is 65,535
		private UInt16 days = 0; //Maximum days is 65,535
		private UInt16 years = 0; //Maximum years is 65,535
		private UInt64 century = 0; //Maximum centuries is 18,446,744,073,709,551,615

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charlemagne the Eternal Emberlight Empress");
		}

		public override void SetDefaults()
		{
			NPC.boss = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.damage = 20;
			NPC.defense = 12;
			NPC.lifeMax = 5000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 1f;
			//makes NPC have a damage reduction of 90% when hit
			NPC.takenDamageMultiplier = 0.1f;
			//Custom AI
			NPC.aiStyle = -1;
			NPC.ai[1] = 0f;
			//Reflect Projectiles
			NPC.reflectsProjectiles = true;
			//NPC is immune to all debuffs
			//Call a loop to check for all debuffs and make them immune.
			for (int i = 0; i < NPC.buffImmune.Length - 1; i++)
			{
				NPC.buffImmune[i] = true;
				if (NPC.buffImmune[i] == NPC.buffImmune[BuffID.Endurance])
				{
					NPC.buffImmune[i] = false;
					NPC.AddBuff(BuffID.Endurance, Int32.MaxValue);
				}
			}
			if (!Main.dedServ)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CosmicInferno");
			}
		}
		public override bool PreAI()
		{
			//Get the player
			Player player = Main.player[NPC.target];
			//Inflict CosmicRules until the boss is dead
			player.AddBuff(ModContent.BuffType<Buffs.Harmful.Disabling.CosmicRules>(), Int32.MaxValue);
			return true;
		}
		public override void AI()
		{
			//Boss will use multiple ai[] values to determine time and actions
			//Invunerable at the start of the fight, gives a bit of dialogue then begins, 
			Player player = Main.player[NPC.target];
			NPC.dontTakeDamage = true;
			NPC.ai[0] += 1f;
			NPC.ai[1] += 1f;
			
			//disable immunity after a minute
			if (NPC.ai[0] >= 3600f)
			{
				NPC.dontTakeDamage = false;
			}
			//if NPC.ai[1] is 60, increment seconds by 1
			if (NPC.ai[1] >= 60f)
			{
				seconds++;
				NPC.ai[1] = 0f;
			}
			//if seconds is 60, increment minutes by 1
			if (seconds >= 60)
			{
				minutes++;
				seconds = 0;
			}
			//if minutes is 60, increment hours by 1
			if (minutes >= 60)
			{
				hours++;
				minutes = 0;
			}
			//if hours is 24, increment days by 1
			if (hours >= 24)
			{
				days++;
				hours = 0;
			}
			//if days is 365, increment years by 1
			if (days >= 365)
			{
				years++;
				days = 0;
			}
			//if years is 100, increment centuries by 1
			if (years >= 100)
			{
				century++;
				years = 0;
			}
			//Broadcast a system message
			if (NPC.ai[0] == 1f)
			{
				Main.NewText("So, I'm going to give you about a minute to get ready, take your time.", 255, 0, 0);
			}
			//One minute has passed, begin the introduction message.
			if (minutes == 1 && seconds == 0 && NPC.ai[1] == 1f && hours == 0)
			{
				Main.NewText("I am Charlemagne, and welcome to the trial of the blaze.", 255, 0, 0);
			}
			//Every 5 seconds after the minute has passed
			if (seconds == 5 && minutes == 1 && NPC.ai[1] == 1f && hours == 0)
			{
				Main.NewText("I will be your opponent in this trial, and I will not hold back.", 255, 0, 0);
			}
			if (seconds == 10 && minutes == 1 && NPC.ai[1] == 1f && hours == 0)
			{
				Main.NewText("Let's get a few rules up and running so we can make this a fair fight.", 255, 0, 0);
			}
			if (seconds == 15 && minutes == 1 && NPC.ai[1] == 1f && hours == 0)
			{
				Main.NewText("First, you are prohibited from flying.", 255, 0, 0);
			}
			if (seconds == 20 && minutes == 1 && NPC.ai[1] == 1f && hours == 0)
			{
				Main.NewText("Second, you are prohibited from using any form of projectile.", 255, 0, 0);
			}
			if (seconds == 25 && minutes == 1 && NPC.ai[1] == 1f && hours == 0)
			{
				Main.NewText("Third, have fun.", 255, 0, 0);
			}
			if (seconds == 30 && minutes == 1 && NPC.ai[1] == 1f && hours == 0)
			{
				Main.NewText("Now, let's get started.", 255, 0, 0);
			}
			
			//
		}
		public override void PostAI()
		{
			//Get the player
			Player player = Main.player[NPC.target];
			//If the player is dead, despawn the NPC and respawn the NPC back as CosmonianCharlemagne
			if (!player.active || player.dead)
			{
				NPC.TargetClosest(true);
				player = Main.player[NPC.target];
				if (!player.active || player.dead)
				{
					NPC.active = false;
					//spawn CosmonianCharlemagne
					//Charlemagne for iEntitySource
					var EntitySource = NPC.GetSource_Death();
					NPC.NewNPC(EntitySource, (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<NPCs.TownNPCs.CosmonianCharlemagne>());
					//Grab the WorldValues and add 1 to the number of times the player has died to Charlemagne
					var worldValues = ModContent.GetInstance<World.WorldValues>();
					worldValues.CharleDeaths++;
				}
			}
		}
		//detects if the npc is hit
		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			NPC.dontTakeDamage = true;
			//Remove the CosmicRules debuff
			player.ClearBuff(ModContent.BuffType<Buffs.Harmful.Disabling.CosmicRules>());
			
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			//Does triple damage to the player
			target.AddBuff(ModContent.BuffType<FlaronInferno>(), 180);
		}
	}
}
