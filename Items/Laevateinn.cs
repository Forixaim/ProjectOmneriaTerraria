using rail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ProjectOmneriaTerraria.Items
{
	public class Laevateinn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flare Blade Origin Laevateinn");
			Tooltip.SetDefault("A replica of Charlemagne Halphas Flaron's personal sword, the blade of the supernova. Deals TRIPLE damage to bosses. ");
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Expert;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.crit = 100;
			Item.scale = 1.5f;
			Item.shoot = ProjectileType<Projectiles.LaevateinnProjectile>();
			Item.shootSpeed = 2.1f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SoulofNight, 10)
				.AddIngredient(ItemID.SoulofLight, 10)
				.AddIngredient(ItemID.SoulofFright, 10)
				.AddIngredient(ItemID.SoulofMight, 10)
				.AddIngredient(ItemID.SoulofSight, 10)
				.AddIngredient(ItemID.SoulofFright, 10)
				.AddIngredient(ItemID.SoulofMight, 10)
				.AddIngredient(ItemID.SoulofSight, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
		//Add the Flaron Inferno
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			//stores the target defense
			int targetDefense = target.defense;
			//Makes every hit ignore defense
			target.defense = 0;
			target.AddBuff(BuffType<Buffs.Harmful.Damaging.FlaronInferno>(), 1500);
			if (target.boss)
			{
				//Deals triple damage to bosses
				target.StrikeNPC(damage * 3, knockback, player.direction);
			}
			else if (target.townNPC)
			{
				//Attempts to heal
				target.HealEffect(50);
				target.life += 50;
			}
			else
			{
				//Deals normal damage to enemies
				target.StrikeNPC(damage, knockback, player.direction);
			}
			//Sets the target defense back to what it was
			target.defense = targetDefense;
			
		}
	}
}
