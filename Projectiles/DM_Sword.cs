using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ProjectOmneriaTerraria.Projectiles
{
	internal class DM_Sword : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Matter Sword");
		}
		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = ProjAIStyleID.Boomerang;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 100; //Can hit multiple enemies
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.scale = 1f;
			AIType = ProjectileID.WoodenBoomerang;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			
		}
		//Unlike the previous projectiles, the Dark Matter Sword spins while it flies
		public override void AI()
		{
			//Emit shadowflame dust
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
			//Rotate the projectile
			Projectile.rotation += 0.5f;
		}
	}
}
