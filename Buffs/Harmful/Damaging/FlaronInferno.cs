using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ProjectOmneriaTerraria.Buffs.Harmful.Damaging
{
	public class FlaronInferno : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flaron Inferno");
			Description.SetDefault("Your body is being consumed by flames. Lose 10% of your max HP every second.");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<FlaronInfernoNPC>().flaronInferno = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<FlaronInfernoPlayer>().flaronInferno = true;
		}
	}
	public class FlaronInfernoNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool flaronInferno;
		public override void ResetEffects(NPC npc)
		{
			flaronInferno = false;
		}
		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (flaronInferno)
			{
				
				if (npc.boss)
				{
					//Bosses are weaker to the flame
					npc.lifeRegen -= npc.lifeMax / 50;
				}
				else
				{
					//Normal enemies are stronger to the flame
					npc.lifeRegen -= npc.lifeMax / 10;
				}
			}
		}
	}
	public class FlaronInfernoPlayer : ModPlayer
	{
		public bool flaronInferno;
		public override void ResetEffects()
		{
			flaronInferno = false;
		}
		public override void UpdateLifeRegen()
		{
			if (flaronInferno)
			{
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}
				Player.lifeRegen -= Player.statLifeMax2 / 100;
			}
		}
	}
}
