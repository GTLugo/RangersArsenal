using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Projectiles
{
    public class BioticDart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Biotic Dart");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type]     = 0;
            //RAGlobalProjectile.whitelistedTypes.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Bullet);
            projectile.ignoreWater  = false;
            projectile.penetrate    = 1;
            projectile.timeLeft     = 60 * 10;
            projectile.friendly     = true;
            projectile.hostile      = false;
            projectile.extraUpdates = 1;
            aiType                  = ProjectileID.Bullet;
        }

        public override bool PreKill(int timeLeft)
        {
            projectile.type = ProjectileID.Bullet;
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            var owner         = Main.player[projectile.owner];
            int healingAmount = 2;
            owner.statLife += healingAmount;
            owner.HealEffect(healingAmount);
        }
    }
}