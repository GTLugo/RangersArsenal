using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Weapons.SMGs //Such namescape
{
    public class GTSMG : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("G.T.S.M.G");
            Tooltip.SetDefault(
                "100% chance not to consume ammo"
              + "\n'Totaw gwobaw annihiwation! UwU'"
            );
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
            var perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2f));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            
            for (int i = 0; i < 3; ++i)
            Projectile.NewProjectile(
                position.X,
                position.Y,
                perturbedSpeed.X + 0.01f * i,
                perturbedSpeed.Y + 0.01f * i,
                type,
                20,
                5,
                player.whoAmI
            );
            return true;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 4);
        }

        public override void SetDefaults()
        {
            item.damage = 69420;
            //item.damage       = int.MaxValue / 10;
            item.crit         = 666;
            item.ranged       = true;
            item.width        = 56;
            item.height       = 30;
            item.useAnimation = 0;
            item.useTime      = 0;
            item.reuseDelay   = 0;
            item.useStyle     = 5;
            item.noMelee      = true;
            item.knockBack    = 420;
            item.value        = 69420;
            item.rare         = -11;
            item.UseSound     = SoundID.Item40; //mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Item_31_4");
            item.autoReuse    = true;
            item.shoot        = 10;
            item.shootSpeed   = 12f;
            item.scale        = 0.85f;
            item.useAmmo      = AmmoID.Bullet;
        }
    }
}