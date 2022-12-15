
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ProjectOmneriaTerraria.Projectiles
{
	internal class DM_Lance : ModProjectile
	{
		//This projectile isn't supposed to be fired from a player but from a Town NPC
		//Flies straight forward, piercing 4 enemies, and sticking to the last enemy hit
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Matter Lance");
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			//Set the Projectile damage to be 1.5x the damage of the NPC's attack
			Projectile.damage = (int)(Main.npc[Projectile.owner].damage * 1.5f);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			//Check if the projectile hits an enemy

		}


		public override void Kill(int timeLeft)
		{
			//Leave an explosion
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position, Vector2.Zero, ProjectileID.DaybreakExplosion, Projectile.damage, Projectile.knockBack, Projectile.owner);
		}
	}
}
