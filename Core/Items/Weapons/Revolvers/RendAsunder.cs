using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Weapons.Revolvers //Such namescape
{
    public class RendAsunder : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rend Asunder");
            Tooltip.SetDefault(
                "Right click to fire a 6-shot burst"
              + "\n'An assassin's right-hand man'"
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
            if (player.altFunctionUse == 2) {
                var perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                speedX    = perturbedSpeed.X;
                speedY    = perturbedSpeed.Y;
                knockBack = item.knockBack * 2;
                return true;
            }

            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 2);
        }

        public override void SetDefaults()
        {
            item.damage       = 22;
            item.ranged       = true;
            item.width        = 60;
            item.height       = 26;
            item.useAnimation = 20;
            item.useTime      = 20;
            item.reuseDelay   = 0;
            item.useStyle     = 5;
            item.noMelee      = true;
            item.knockBack    = 3f;
            item.value        = 50000;
            item.rare         = 2;
            item.UseSound     = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/McCree Gunshot (no reverb)");
            item.autoReuse    = false;
            item.shoot        = 10;
            item.shootSpeed   = 9f;
            item.scale        = 0.85f;
            item.useAmmo      = AmmoID.Bullet;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) {
                item.useAnimation = 36;
                item.useTime      = 6;
                item.reuseDelay   = 90;
                item.useStyle     = 5;
                item.UseSound     = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/McCree Gunshot 6-shot");
                item.damage       = 22;
                item.shoot        = 10;
                item.autoReuse    = false;
            } else {
                item.useAnimation = 20;
                item.useTime      = 20;
                item.reuseDelay   = 0;
                item.useStyle     = 5;
                item.UseSound     = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/McCree Gunshot (no reverb)");
                item.damage       = 22;
                item.shoot        = 10;
                item.autoReuse    = false;
            }

            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TheUndertaker);
            recipe.AddIngredient(ItemID.CrimtaneBar, 20);
            recipe.AddIngredient(ItemID.TissueSample, 30);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}