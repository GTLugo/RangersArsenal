using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SoundType = Terraria.ModLoader.SoundType;

namespace RangersArsenal.Core.Items.Weapons //Such namescape
{
    public struct GunSettings
    {
        public LegacySoundStyle useSound;
        public float            spreadAngle;
        
        // bools
        public bool isFullAuto;
        public bool isBurstFire;
        public bool isFiresExtraRockets;

        // ammo changes
        public int   convertedBulletType;
        public float ammoSaveChance;
        public int   burstCount;
        
        // rocket
        public int rocketProjectileType;
        public int burstsBetweenRockets;
        public int rocketDamage;
    }
    
    public abstract class GunItem : ModItem
    {
        protected GunSettings gunSettings;
        
        protected virtual GunSettings GetGunSettings()
        {
            // these settings will vary from gun-to-gun
            return new GunSettings() {
                useSound             = SoundID.Item11,
                spreadAngle          = 1f,
                isFullAuto           = true,
                isBurstFire          = false,
                isFiresExtraRockets  = false,
                convertedBulletType  = ProjectileID.Bullet,
                ammoSaveChance       = 0f,
                burstCount           = 3,
                rocketProjectileType = mod.ProjectileType("SpikeBall"),
                burstsBetweenRockets = 1,
                rocketDamage         = 25,
            };
        }
        
        // OVERRIDES
        public override void SetDefaults()
        {
            /*
            item.damage     = 10;
            item.crit       = 6;
            item.knockBack  = 1;
            item.shootSpeed = 12f;
            
            item.width  = 60;
            item.height = 20;
            item.scale  = 0.85f;
            
            item.useTime      = 3;
            item.useAnimation = 10; // only necessary for non-burst guns
            item.reuseDelay   = 14;
            
            item.value        = Item.sellPrice(0, 0, 1, 0);
            item.rare         = ItemRarityID.White;
            */
            
            SetGunDefaults();
            gunSettings = GetGunSettings();
            SetGunSettings(gunSettings);
        }

        private void SetGunDefaults()
        {
            // these settings shouldn't change for any guns
            item.ranged    = true;
            item.noMelee   = true;

            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ProjectileID.Bullet; // I agree with ExampleMod. I have no fucking clue why guns use purification powder as the item.shoot sometimes...
            item.useAmmo = AmmoID.Bullet;
        }

        private void SetGunSettings(GunSettings settings)
        {
            item.autoReuse    = settings.isFullAuto;
            item.useAnimation = CalculateUseAnimation(item.useAnimation);
            item.UseSound     = settings.useSound;
        }

        public override bool Shoot(
            Player      player,
            ref Vector2 position,
            ref float   speedX,
            ref float   speedY,
            ref int     type,
            ref int     damage,
            ref float   knockBack
        )
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();
            
            // converts bullets
            if (type == ProjectileID.Bullet) type = gunSettings.convertedBulletType;

            // inaccuracy
            var perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(gunSettings.spreadAngle));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;


            // extra rocket that fires only during first burst
            if (gunSettings.isBurstFire && !(player.itemAnimation < item.useAnimation - 2)) {
                if (gunSettings.isFiresExtraRockets && modPlayer.numBullets == gunSettings.burstsBetweenRockets) {
                    var perturbedSpeed2 =
                        new Vector2(speedX * 2f, speedY * 2f).RotatedByRandom(MathHelper.ToRadians(5f));
                    speedX = perturbedSpeed2.X;
                    speedY = perturbedSpeed2.Y;
                    var perturbedSpeed3 = new Vector2(speedX, speedY);

                    // muzzle offset
                    var adjustedPosition = position;
                    var muzzleOffset     = Vector2.Normalize(new Vector2(speedX, speedY)) * (player.itemWidth + 1);
                    if (Collision.CanHit(adjustedPosition, 0, 0, adjustedPosition + muzzleOffset, 0, 0))
                        adjustedPosition += muzzleOffset;
                    Projectile.NewProjectile(
                        adjustedPosition.X,
                        adjustedPosition.Y,
                        perturbedSpeed3.X,
                        perturbedSpeed3.Y,
                        gunSettings.rocketProjectileType,
                        gunSettings.rocketDamage,
                        5,
                        player.whoAmI
                    );
                }

                // fires every couple bursts
                if (modPlayer.numBullets < gunSettings.burstsBetweenRockets)
                    modPlayer.numBullets++;
                else
                    modPlayer.numBullets = 0;
            }

            return true;
        }

        public override bool ConsumeAmmo(Player player)
        {
            bool wontSaveAmmo = Main.rand.NextFloat() < 1f - gunSettings.ammoSaveChance;

            if (gunSettings.isBurstFire) {
                bool willUseAmmoInBurst = !(player.itemAnimation < item.useAnimation - 2);
                return willUseAmmoInBurst && wontSaveAmmo;
            }
            return wontSaveAmmo;
        }
        
        private int CalculateUseAnimation(int defaultTime) {
            return gunSettings.isBurstFire ? item.useTime * gunSettings.burstCount : defaultTime;
        }
    }
}