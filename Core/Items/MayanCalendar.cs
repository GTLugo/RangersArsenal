using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items
{
    public class MayanCalendar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Energy Disc");
            Tooltip.SetDefault("Permanently increases the number of accessory slots");
            //ItemID.Sets.SortingPriorityBossSpawns[item.type] = 12; // This helps sort inventory know this is a boss summoning item.
        }

        public override bool CanUseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();
            return !modPlayer.hasGolemSlot && modPlayer.extraSlots < RangersArsenal.maxExtraSlots;
        }

        public override bool UseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();
            modPlayer.extraSlots++;
            modPlayer.hasGolemSlot = true;
            return true;
        }

        public override void SetDefaults()
        {
            item.maxStack     = 1;
            item.width        = 34;
            item.height       = 34;
            item.value        = Item.sellPrice(0, 69, 4, 20);
            item.rare         = 8;
            item.useAnimation = 30;
            item.useTime      = 30;
            item.useStyle     = 4;
            item.consumable   = true;
        }

        /*public override bool ReforgePrice(ref int reforgePrice, ref bool canApplyDiscount)
        {
            reforgePrice = Item.sellPrice(0, 34, 4, 20);
            if (canApplyDiscount)
                reforgePrice = (int)(reforgePrice * 0.8f);
            reforgePrice = (int)(reforgePrice * 0.33f);
            return base.ReforgePrice(ref reforgePrice, ref canApplyDiscount);
        }*/

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarTabletFragment, 4);
            recipe.AddIngredient(ItemID.LihzahrdPowerCell, 4);
            recipe.AddTile(TileID.LihzahrdAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}