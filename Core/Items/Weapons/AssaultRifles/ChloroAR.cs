using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Weapons.AssaultRifles //Such namescape
{
    public class ChloroAR : GunItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pitcher's Pitfall");
            Tooltip.SetDefault(
                "Four round burst"
              + "\nOnly the first shot consumes ammo"
              + "\nFires an additional explosive flower"
              + "\nShoots a powerful, high velocity bullet"
            );
        }

        public override void SetDefaults()
        {
            item.damage     = 23;
            item.crit       = 6;
            item.knockBack  = 1;
            item.shootSpeed = 13f;
            
            item.width  = 66;
            item.height = 28;
            item.scale  = 1f;
            
            item.useTime      = 3;
            item.useAnimation = 12;
            item.reuseDelay   = 14;
            
            item.value = 165000;
            item.rare  = ItemRarityID.Yellow;
            
            base.SetDefaults();
        }

        protected override GunSettings GetGunSettings()
        {
            return new GunSettings() {
                useSound             = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Item_31_4"),
                spreadAngle          = 1f,
                isFullAuto           = true,
                isBurstFire          = true,
                isFiresExtraRockets  = true,
                convertedBulletType  = ProjectileID.BulletHighVelocity,
                ammoSaveChance       = 0f,
                burstCount           = 4,
                rocketProjectileType = mod.ProjectileType("SpikeBall"),
                burstsBetweenRockets = 1,
                rocketDamage         = 20,
            };
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }
    }
}