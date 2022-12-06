using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ProjectOmneriaTerraria.NPCs.Bosses.Post_SCal.Charlemagne
{
	[AutoloadBossHead]
	internal class Charlemagne : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternal Emberlight Empress");
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 1;
			NPC.boss = true;
			NPC.GivenName = "Charlemagne";
			NPC.defense = Int32.MaxValue;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.FemaleHit;
			NPC.DeathSound = SoundID.NPCDeath6;
		}

		public override void AI()
		{
			base.AI();
		}
	}
}
