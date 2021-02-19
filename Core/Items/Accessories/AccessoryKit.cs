using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Accessories
{
    public class AccessoryKit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Illegal Accessory Kit");
            Tooltip.SetDefault(
                "Increases ranged accuracy dramatically"
                //+ "\nDecreases ranged damage by 4%"
              + "\nBullets instantly hit their targets"
              + "\n(Visibility toggles bullet acceleration and accuracy)"
            );
        }

        public override void SetDefaults()
        {
            item.width     = 26;
            item.height    = 22;
            item.value     = 50000;
            item.rare      = 8;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();
            //player.rangedDamage -= 0.04f;
            modPlayer.hasGrip   = true;
            modPlayer.hasMuzzle = true;

            if (!hideVisual) {
                modPlayer.areBulletsAccurate    = true;
                modPlayer.areBulletsAccelerated = true;
            }

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "VerticalGrip");
            recipe.AddIngredient(ItemID.IllegalGunParts);
            recipe.AddIngredient(mod, "MeteorMuzzle");
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}