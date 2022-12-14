using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProjectOmneriaTerraria.Items
{
	//Shouldn't be obtained
	internal class OriginLaevateinn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flare Blade Origin Laevateinn");
			Tooltip.SetDefault("Charlemagne Halphas Flaron's personal sword, the true blade of the supernova. Deals TRIPLE damage to bosses. Shouldn't be obtained.");
		}
		public override void SetDefaults()
		{
			Item.damage = Int32.MaxValue/2;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Expert;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.crit = 100;
			Item.scale = 0.5f;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			//apply the Flaron Inferno
			target.AddBuff(ModContent.BuffType<Buffs.Harmful.Damaging.FlaronInferno>(), 1500);
		}
	}
}
