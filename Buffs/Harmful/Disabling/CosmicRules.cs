using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ProjectOmneriaTerraria.Buffs.Harmful.Disabling
{
	internal class CosmicRules : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Rules");
			Description.SetDefault("You are now under the rules of the cosmos");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			// Make the nurse unable to remove this buff
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			// Prevents buffs that are not debuffs from being applied
			for (int i = 0; i < player.buffImmune.Length - 1; i++)
			{
				player.buffImmune[i] = false;
				if (player.buffImmune[i])
				{
					player.buffImmune[i] = true;
					player.AddBuff(BuffID.Endurance, Int32.MaxValue);
				}
			}
		}
	}
}
