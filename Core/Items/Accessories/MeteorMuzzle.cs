using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Accessories
{
    public class MeteorMuzzle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "Bullets instantly hit their targets"
              + "\nBullets burn struck enemies"
              + "\n(Visibility toggles bullet acceleration)"
              + "\n'Zoom!'"
            );
        }

        public override void SetDefaults()
        {
            item.width     = 22;
            item.height    = 30;
            item.value     = 50000;
            item.rare      = 2;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.bulletDamage *= 1.10f;
            var modPlayer = player.GetModPlayer<RAPlayer>();
            modPlayer.hasMuzzle = true;
            if (!hideVisual) modPlayer.areBulletsAccelerated = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}