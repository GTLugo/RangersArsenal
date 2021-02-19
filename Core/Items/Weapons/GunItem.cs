using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SoundType = Terraria.ModLoader.SoundType;

namespace RangersArsenal.Core.Items.Weapons //Such namescape
{
    // made into a class for default values. not sure if this is worth the overhead.
    public class GunSettings
    {
        public int              useTime          = 5;
        public int              useAnimationTime = 10;
        public int              useDelay         = 0;
        public LegacySoundStyle useSound         = SoundID.Item11;
        public LegacySoundStyle altUseSound      = SoundID.Item11;
        public float            spreadAngle      = 1f;
        public float            bulletSpeed      = 14f;
        
        // bools
        public bool isFullAuto          = true;
        public bool isBurstFire         = false;
        public bool isFiresExtraRockets = false;
        public bool isRevolver          = false;

        // ammo changes
        public int   convertedBulletType = ProjectileID.Bullet;
        public float ammoSaveChance      = 0f;
        public int   burstCount          = 3;
        
        // rocket
        public int rocketProjectileType = ProjectileID.Bullet;
        public int burstsBetweenRockets = 0;
        public int rocketDamage         = 25;
    }
    
    public abstract class GunItem : ModItem
    {
        private GunSettings _gunSettings;
        protected virtual GunSettings GunSettings => new GunSettings();

        /*protected virtual GunSettings GetGunSettings()
        {
            // these settings will vary from gun-to-gun
            /*return new GunSettings() {
                useTime          = 5,
                useAnimationTime = 10,
                useDelay         = 0,
                useSound         = SoundID.Item11,
                altUseSound      = SoundID.Item11,
                spreadAngle      = 1f,
                bulletSpeed      = 14f,
                
                isFullAuto          = true,
                isBurstFire         = false,
                isFiresExtraRockets = false,
                isRevolver          = false,
                
                convertedBulletType = ProjectileID.Bullet,
                ammoSaveChance      = 0f,
                burstCount          = 3,
                
                rocketProjectileType = mod.ProjectileType("SpikeBall"),
                burstsBetweenRockets = 1,
                rocketDamage         = 25,
            };#1#
            return new GunSettings();
        }*/

        // OVERRIDES
        public override void SetDefaults()
        {
            /*
             * EXAMPLE OF WHAT NEEDS TO BE IN SetDefaults()
             * item.damage     = 10;
             * item.crit       = 6;
             * item.knockBack  = 1;
             * 
             * item.width  = 60;
             * item.height = 20;
             * item.scale  = 0.85f;
             * 
             * item.value        = Item.sellPrice(0, 0, 1, 0);
             * item.rare         = ItemRarityID.White;
             */
            _gunSettings = GunSettings;
            SetGunDefaults();
            SetGunSettings(_gunSettings);
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
            item.useTime      = settings.useTime;
            item.useAnimation = CalculateUseAnimation(settings.useAnimationTime);
            item.reuseDelay   = settings.useDelay;
            item.autoReuse    = settings.isFullAuto;
            item.UseSound     = settings.useSound;
            item.shootSpeed   = settings.bulletSpeed;
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
            if (type == ProjectileID.Bullet) type = _gunSettings.convertedBulletType;

            // inaccuracy
            float spreadAngle = _gunSettings.spreadAngle;
            if (_gunSettings.isRevolver && player.altFunctionUse == 2) {
                spreadAngle = 15;
                knockBack   =  item.knockBack * 2;
            }
            var perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(spreadAngle));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;


            // extra rocket that fires only during first burst
            if (_gunSettings.isBurstFire && !(player.itemAnimation < item.useAnimation - 2)) {
                if (_gunSettings.isFiresExtraRockets && modPlayer.numBullets == _gunSettings.burstsBetweenRockets) {
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
                        _gunSettings.rocketProjectileType,
                        _gunSettings.rocketDamage,
                        5,
                        player.whoAmI
                    );
                }

                // fires every couple bursts
                if (modPlayer.numBullets < _gunSettings.burstsBetweenRockets)
                    modPlayer.numBullets++;
                else
                    modPlayer.numBullets = 0;
            }

            return true;
        }

        public override bool ConsumeAmmo(Player player)
        {
            // ammo saving
            bool wontSaveAmmo = Main.rand.NextFloat() < 1f - _gunSettings.ammoSaveChance;

            // burst fire
            if (_gunSettings.isBurstFire) {
                bool willUseAmmoInBurst = !(player.itemAnimation < item.useAnimation - 2);
                return willUseAmmoInBurst && wontSaveAmmo;
            }
            
            return wontSaveAmmo;
        }

        public override bool CanUseItem(Player player)
        {
            if (!_gunSettings.isRevolver) return base.CanUseItem(player);

            // revolver fan-the-hammer mode
            if (player.altFunctionUse == 2) {
                int altUseTime = (int)(_gunSettings.useTime / 2.5f);
                item.useAnimation = altUseTime * 6;
                item.useTime      = altUseTime;
                item.reuseDelay   = 90;
                item.UseSound     = _gunSettings.altUseSound;
            } else {
                item.useAnimation = _gunSettings.useAnimationTime;
                item.useTime      = _gunSettings.useTime;
                item.reuseDelay   = _gunSettings.useDelay;
                item.UseSound     = _gunSettings.useSound;
            }

            return base.CanUseItem(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            // enable revolver's fan-the-hammer mode
            return _gunSettings.isRevolver;
        }
        
        private int CalculateUseAnimation(int defaultTime) {
            return _gunSettings.isBurstFire ? item.useTime * _gunSettings.burstCount : defaultTime;
        }
    }
}