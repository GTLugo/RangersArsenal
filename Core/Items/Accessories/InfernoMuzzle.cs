using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Accessories
{
    public class InfernoMuzzle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "Bullets instantly hit their targets"
              + "\n12% increased ranged damage"
              + "\n(Visibility toggles bullet acceleration)"
            );
        }

        public override void SetDefaults()
        {
            item.width     = 22;
            item.height    = 30;
            item.value     = 200000;
            item.rare      = 4;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();
            player.rangedDamage += 0.12f;
            modPlayer.hasMuzzle =  true;
            if (!hideVisual) modPlayer.areBulletsAccelerated = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "MeteorMuzzle");
            recipe.AddIngredient(ItemID.RangerEmblem);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}