using Terraria;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Accessories
{
    public class BumpStock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "Converts all ranged weapons to auto-fire"
                //+ "\nDecreases ranged damage by 10%"
            );
        }

        public override void SetDefaults()
        {
            item.width     = 30;
            item.height    = 30;
            item.value     = 75000;
            item.rare      = 7;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.rangedDamage -= 0.10f;
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<RAPlayer>().hasStock = true;
        }
    }
}