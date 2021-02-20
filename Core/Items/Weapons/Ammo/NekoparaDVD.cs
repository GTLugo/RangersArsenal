using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Weapons.Ammo //Such namescape
{
    public class NekoparaDVD : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nekopara Collection");
            Tooltip.SetDefault("Can be used as bullets by guns"
                             + "'Young neko are so innocent and super cute. They're the best'");
        }

        public override void SetDefaults()
        {
            item.damage     = 69420;
            item.crit       = 666;
            item.ranged     = true;
            item.width      = 26;
            item.height     = 26;
            item.maxStack   = 1;
            item.knockBack  = 420;
            item.value      = 69420;
            item.rare       = -11;
            item.shoot      = ProjectileID.ChlorophyteBullet; //Chlorophyte Bullet Projectile ID
            item.shootSpeed = 5f;
            item.ammo       = AmmoID.Bullet;
        }
    }
}