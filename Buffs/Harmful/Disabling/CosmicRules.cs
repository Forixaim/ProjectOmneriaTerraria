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
			//Make the nurse unable to remove this buff
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			//Prevents buffs that 
		}
	}
}
