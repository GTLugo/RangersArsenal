using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Accessories
{
    public class EnchantedMag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Magazine");
            Tooltip.SetDefault(
                "10% increased ranged damage"
                //+ "\n15% increased critical strike chance"
                //+ "\nIncreased ranged accuracy"
              + "\n20% chance not to consume ammo"
              + "\nBullets instantly hit their targets"
                //+ "\nRanged attacks inflict Daybroken"
                //+ "\n(Visibility toggles bullet acceleration and accuracy)"
            );
        }

        public override void SetDefaults()
        {
            item.width     = 32;
            item.height    = 36;
            item.value     = Item.sellPrice(0, 42, 25);
            item.rare      = 9;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();

            player.rangedDamage += 0.10f;
            //player.rangedCrit   += 15;
            //modPlayer.hasMuzzle = true; //Not required anymore
            //modPlayer.hasGrip = true;
            modPlayer.hasMag = true;

            if (!hideVisual) //modPlayer.areBulletsAccurate    = true;
                modPlayer.areBulletsAccelerated = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DestroyerEmblem);
            recipe.AddIngredient(mod, "AccessoryKit");
            recipe.AddIngredient(ItemID.DD2PhoenixBow);
            recipe.AddIngredient(mod, "BeskarBar", 4);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}