using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Materials.Beskar
{
    internal class BeskarBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Beskar Bar");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.width    = 30;
            item.height   = 24;
            item.maxStack = 99;
            item.value    = Item.sellPrice(0, 15, 50);
            item.rare     = 8;
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "BeskarOre", 5);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}