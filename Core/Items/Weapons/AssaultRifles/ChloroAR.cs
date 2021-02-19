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
            
            item.width  = 66;
            item.height = 28;
            item.scale  = 1f;
            
            item.value = 165000;
            item.rare  = ItemRarityID.Yellow;
            
            base.SetDefaults();
        }

        protected override GunSettings GunSettings =>
            new GunSettings {
                useTime              = 3,
                useSound             = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Item_31_4"),
                isBurstFire          = true,
                isFiresExtraRockets  = true,
                convertedBulletType  = ProjectileID.BulletHighVelocity,
                burstCount           = 4,
                rocketProjectileType = mod.ProjectileType("SpikeBall"),
                burstsBetweenRockets = 1,
                rocketDamage         = item.damage,
            };

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }
    }
}