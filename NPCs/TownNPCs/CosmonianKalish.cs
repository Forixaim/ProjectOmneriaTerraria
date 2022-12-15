using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProjectOmneriaTerraria.NPCs.TownNPCs
{
	[AutoloadHead]
	internal class CosmonianKalish : ModNPC
	{
		private int _projectile = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Emperor");
			//Reserve 5 frames for NPC's Attack animation
			Main.npcFrameCount[NPC.type] = 22;

			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 1;
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;
			NPCID.Sets.AttackType[NPC.type] = 0; //Fires Dark Matter Weapons in self defense
			NPCID.Sets.AttackTime[NPC.type] = 10;
			NPCID.Sets.AttackAverageChance[NPC.type] = 90;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;

		}
		public override void SetDefaults()
		{
			NPC.height = 40;
			NPC.width = 40;
			NPC.aiStyle = 7;
			NPC.damage = 250;
			NPC.defense = 100;
			NPC.lifeMax = 10000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 1f;
			NPC.value = 10000f;
			NPC.npcSlots = 1f;
			NPC.lavaImmune = true;
			NPC.dontTakeDamage = false;
			NPC.GivenName = "Kalish";
		}

		public override void AI()
		{
			//Emit purple light
			Lighting.AddLight(NPC.Center, Color.Purple.ToVector3() * 2f);
		}

		//Shoots a variety of Dark Matter Weapons (randomly chosen list: Spear, Lance, and Sword)
		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			//Random number between 0 and 2
			_projectile = Main.rand.Next(3);
			//Set the projectile type to be a Dark Matter Weapon
			switch (_projectile)
			{
				case 0:
					projType = ModContent.ProjectileType<Projectiles.DM_Spear>();
					break;
				case 1:
					projType = ModContent.ProjectileType<Projectiles.DM_Lance>();
					break;
				case 2:
					projType = ModContent.ProjectileType<Projectiles.DM_Sword>();
					break;
			}
		}
		//Modify the speed of the projectile
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			switch (_projectile)
			{
				case 0:
					multiplier = 2f;
					break;
				case 1:
					multiplier = 1.5f;
					break;
				case 2:
					multiplier = 1f;
					break;
			}
			//Always make the projectile spawn behind the NPC
			randomOffset = 2f;
			
		}
	}
}
