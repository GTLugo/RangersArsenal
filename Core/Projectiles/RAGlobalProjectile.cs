using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Projectiles
{
    public class RAGlobalProjectile : GlobalProjectile
    {
        public static List<int> shootTypes = new List<int> {
            ProjectileID.PurificationPowder, // for whatever reason, a lot of guns have this as their shoot
            ProjectileID.Bullet,
            ProjectileID.Xenopopper
        };

        public static List<int> whitelistedTypes = new List<int> {
            ProjectileID.Bullet,
            ProjectileID.ExplosiveBullet,
            ProjectileID.BulletHighVelocity,
            ProjectileID.ChlorophyteBullet,
            ProjectileID.CrystalBullet,
            ProjectileID.CursedBullet,
            ProjectileID.GoldenBullet,
            ProjectileID.IchorBullet,
            ProjectileID.MoonlordBullet,
            ProjectileID.NanoBullet,
            ProjectileID.PartyBullet,
            ProjectileID.VenomBullet,
            ProjectileID.MeteorShot
        };

        public static List<int> blacklistedTypes = new List<int> {
            ProjectileID.BlackBolt, ProjectileID.Xenopopper
        };

        //private static readonly int MuzzleSmokeAI        = Projectile.maxAI - 3;
        private static readonly int ProjectileLifetimeAI = Projectile.maxAI - 2;
        private static readonly int BulletBouncedAI      = Projectile.maxAI - 1;

        private static bool IsPlayersBullet(Projectile projectile, Player player)
        {
            // fallback onto the bullet list for testing if gun
            return IsPlayerProjectile(projectile)
                && (HoldingGun(player) || whitelistedTypes.Contains(projectile.type))
                && !blacklistedTypes.Contains(projectile.type);
        }

        private static bool HoldingGun(Player player)
        {
            return shootTypes.Contains(player.HeldItem.shoot);
        }

        private static bool IsPlayerProjectile(Projectile projectile)
        {
            return projectile.ranged && projectile.friendly && !projectile.npcProj;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.owner == Main.myPlayer && IsPlayersBullet(projectile, Main.player[Main.myPlayer])) {
                var owner     = Main.player[Main.myPlayer];
                var modPlayer = owner.GetModPlayer<RAPlayer>();
                //Test for bullet types
                if (modPlayer.hasMuzzle) //Test for muzzle and ranged projectile
                    target.AddBuff(BuffID.OnFire, 60 * 8);
                /*if (modPlayer.hasMag && HoldingGun(owner)) //Test for muzzle and ranged projectile
                    target.AddBuff(BuffID.Daybreak, 60 * 12);*/
            }

            base.OnHitNPC(projectile, target, damage, knockback, crit);
        }

        public override void AI(Projectile projectile)
        {
            var owner     = Main.player[projectile.owner];
            var modPlayer = owner.GetModPlayer<RAPlayer>();

            if (IsPlayerProjectile(projectile)) {
                //Main.NewText((HoldingGun(owner) ? "THIS IS A GUN " : "not gun ") + (IsPlayersBullet(projectile, owner) ? "FIRING A BULLET" : "not bullet"));
                //Main.NewText(owner.HeldItem.shoot);
            }

            //var heldItem  = owner.inventory[owner.selectedItem];

            if (IsPlayersBullet(projectile, owner)) {
                // PERFECT BULLET ACCURACY SECTION
                if (projectile.owner == Main.myPlayer) {
                    var cursor       = (Main.MouseWorld - owner.Center).SafeNormalize(Vector2.UnitX);
                    var projVelocity = cursor * projectile.velocity.Length();

                    //Test for grip and test that the bullet just spawned
                    if (modPlayer.areBulletsAccurate) // Stuff to occur only on first spawn
                        if (CompareFloats(projectile.localAI[ProjectileLifetimeAI], 0f)) {
                            //float angle = projectile.velocity.ToRotation().AngleTowards(projVelocity.ToRotation(), 30);
                            //if (projectile.velocity.ToRotation() > projVelocity.RotatedBy(Math.PI / 12).ToRotation())/*projectile velocity rotation is greater than threshold*/{
                            //Vector2 perturbedSpeed = projVelocity.RotatedByRandom(MathHelper.ToRadians(1f));
                            projectile.velocity.X = projVelocity.X;
                            projectile.velocity.Y = projVelocity.Y;
                            projectile.netUpdate  = true;
                            //}
                        }
                }

                // HITSCAN BULLET ACCELERATION SECTION
                if (modPlayer.areBulletsAccelerated) {
                    var projectileDirection = Vector2.Normalize(projectile.velocity);
                    var barrelDirection     = Vector2.UnitX.RotatedBy(owner.itemRotation) * owner.direction;
                    var holdoutOffset = owner.HeldItem.modItem?.HoldoutOffset().GetValueOrDefault(Vector2.Zero)
                                     ?? Vector2.Zero;
                    //Main.NewText("Rotation: " + owner.itemRotation / Math.PI + " Direction: " + owner.direction);

                    //Main.NewText("Holdout Offset: " + holdoutOffset);
                    projectile.extraUpdates = 100;
                    projectile.alpha        = 255;
                    projectile.netUpdate    = true;

                    // Stuff to occur only on first spawn
                    if (CompareFloats(projectile.localAI[ProjectileLifetimeAI], 0f)) {
                        var barrelPosition =
                                owner.Center
                              + barrelDirection * owner.itemWidth
                            //+ holdoutOffset.X * barrelDirection
                            ;
                        MuzzleFlash(projectile, barrelPosition);
                    } else {
                        HitscanBeam(projectile, owner, projectileDirection);
                    }
                }

                projectile.localAI[ProjectileLifetimeAI]++;
                projectile.netUpdate = true;
            }

            base.AI(projectile);
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            var owner     = Main.player[projectile.owner];
            var modPlayer = owner.GetModPlayer<RAPlayer>();
            //var heldItem  = owner.inventory[owner.selectedItem];

            if (IsPlayersBullet(projectile, owner) && modPlayer.areBulletsAccelerated) {
                // Does it shoot bullets?
                HitMarker(projectile);
                projectile.localAI[ProjectileLifetimeAI] = 0;
                projectile.localAI[BulletBouncedAI]      = 1;
                projectile.netUpdate                     = true;
            }

            return base.OnTileCollide(projectile, oldVelocity);
        }

        private static void MuzzleFlash(Projectile projectile, Vector2 barrelPosition)
        {
            bool hasBounced = projectile.localAI[BulletBouncedAI] > 0f;

            if (hasBounced) return;
            // MUZZLE FLASH FLAME
            SpawnFlame(barrelPosition, projectile.width, projectile.height, 3f);
        }

        private static void HitscanBeam(Projectile projectile, Player owner, Vector2 projectileDirection)
        {
            bool  hasBounced = projectile.localAI[BulletBouncedAI] > 0f;
            float scale      = 2f / (projectile.localAI[ProjectileLifetimeAI] + 1);
            var   position   = projectile.position;
            if (Collision.CanHit(position, 0, 0, position + projectileDirection * owner.itemWidth, 0, 0)
             && !hasBounced)
                position                  += projectileDirection * owner.itemWidth;
            else if (hasBounced) position += projectileDirection * -20;
            SpawnDustAlongVector(
                position, //projectile.position
                projectile.velocity,
                76,
                ChooseTeamColor(owner.team),
                scale >= 0.4f ? scale : 0.4f
            );
        }

        private static void HitMarker(Projectile projectile)
        {
            SpawnFlame(projectile.position, projectile.width, projectile.height, 1f);
        }

        private static Color ChooseTeamColor(int team)
        {
            switch (team) {
                case 0:
                    return Color.White;
                case 1:
                    return Color.Crimson;
                case 2:
                    return Color.LawnGreen;
                case 3:
                    return Color.CornflowerBlue;
                case 4:
                    return Color.Goldenrod;
                case 5:
                    return Color.Magenta;
                default:
                    return Color.White;
            }
        }

        private static void SpawnDustAlongVector(
            Vector2 position,
            Vector2 velocity,
            int     dustType,
            Color   dustColor,
            float   scale,
            int     iterations = 3
        )
        {
            for (int i = 0; i < iterations; ++i) {
                var adjPosition = position - velocity * (i * (1f / iterations));
                int trail = Dust.NewDust(
                    adjPosition,
                    1,
                    1,
                    dustType,
                    0f,
                    0f,
                    0,
                    dustColor,
                    scale
                );
                Main.dust[trail].velocity  *= 0.2f;
                Main.dust[trail].noGravity =  true;
                Main.dust[trail].noLight   =  true;
            }
        }

        private static void SpawnFlame(Vector2 position, int width, int height, float scale)
        {
            Lighting.AddLight(position, 1f, 0.40f, 0f);

            for (int i = 0; i < 4; ++i) {
                int fire = Dust.NewDust(
                    position,
                    width,
                    height,
                    DustID.Fire,
                    Scale: scale
                );
                Main.dust[fire].noGravity = true;
            }
        }

        private static bool CompareFloats(float a, float b, float tolerance = 0.1f)
        {
            if (a == b) return true; // early-out check for perfect match
            return Math.Abs(a - b) < tolerance;
        }

        /*public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox) {
            Player owner = Main.player[projectile.owner];
            if (projectile.ranged && projectile.friendly && !projectile.npcProj && projectile.owner == Main.myPlayer) {
                if (owner.GetModPlayer<RAPlayer>().hasMuzzle) //Test for muzzle and ranged projectile
                {
                    //hitbox.Inflate(5, 5);
                }
            }
            base.ModifyDamageHitbox(projectile, ref hitbox);
        }*/
    }
}