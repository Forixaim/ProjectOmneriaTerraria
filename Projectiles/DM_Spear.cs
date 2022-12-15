using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProjectOmneriaTerraria.Projectiles
{
	internal class DM_Spear : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Matter Spear");
		}
		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 5; //Can only hit 1 enemy but hits extremely hard and flies fast
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
		//Fix the spear's rotation

		public override void AI()
		{
			//Emit shadowflame dust
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;
		}
		//When the projectile is killed, leave an explosion
		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position, Vector2.Zero, ProjectileID.DaybreakExplosion, Projectile.damage, Projectile.knockBack, Projectile.owner);
		}
	}
}
