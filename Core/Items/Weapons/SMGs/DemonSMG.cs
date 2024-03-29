using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Weapons.SMGs //Such namescape
{
    public class DemonSMG : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Umbra");
            Tooltip.SetDefault(
                "50% chance not to consume ammo"
              + "\n'Boop!'"
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
            return true;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .5f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 4);
        }

        public override void SetDefaults()
        {
            item.damage       = 7;
            item.crit         = 6;
            item.ranged       = true;
            item.width        = 56;
            item.height       = 30;
            item.useAnimation = 5;
            item.useTime      = 5;
            item.reuseDelay   = 0;
            item.useStyle     = 5;
            item.noMelee      = true;
            item.knockBack    = 1;
            item.value        = 16500;
            item.rare         = 3;
            item.UseSound     = SoundID.Item40; //mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Item_31_4");
            item.autoReuse    = true;
            item.shoot        = 10;
            item.shootSpeed   = 12f;
            item.scale        = 0.85f;
            item.useAmmo      = AmmoID.Bullet;
        }


        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            //recipe.AddIngredient(mod, "ChloroAR");
            recipe.AddIngredient(ItemID.Bone, 35);
            recipe.AddIngredient(ItemID.DemoniteBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}