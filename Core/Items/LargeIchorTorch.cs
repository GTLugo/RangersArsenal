using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items
{
    public class LargeIchorTorch : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Ichor Torch");
            Tooltip.SetDefault(
                "Creates torches at the cost of gel" + "\n'Unlimited torch power! Itty bitty inventory space.'"
            );
        }

        public override void SetDefaults()
        {
            item.width        = 10;
            item.height       = 12;
            item.holdStyle    = 1;
            item.noWet        = false;
            item.useTurn      = true;
            item.autoReuse    = true;
            item.useAnimation = 15;
            item.useTime      = 10;
            item.useStyle     = 1;
            item.createTile   = TileID.Torches;
            item.placeStyle   = 11;
            item.tileWand     = AmmoID.Gel;
            item.consumable   = false;
            item.rare         = 4;
            item.flame        = true;
        }

        public override void PostUpdate()
        {
            //if (!item.wet) {
            Lighting.AddLight(
                (int)((item.position.X + item.width / 2) / 16f),
                (int)((item.position.Y + item.height / 2) / 16f),
                1.3f,
                1.2f,
                1f
            );
            //}
        }

        public override void HoldStyle(Player player)
        {
            player.itemLocation = new Vector2(
                player.itemLocation.X - 10f * player.direction,
                player.itemLocation.Y + 3f
            );
            base.HoldStyle(player);
        }

        public override void HoldItem(Player player)
        {
            if (Main.rand.Next(5) == 0)
                Dust.NewDust(
                    new Vector2(
                        player.itemLocation.X + 24f * player.direction,
                        player.itemLocation.Y - 26f * player.gravDir
                    ),
                    4,
                    4,
                    DustID.Fire
                );
            var position = player.RotatedRelativePoint(
                new Vector2(
                    player.itemLocation.X + 12f * player.direction + player.velocity.X,
                    player.itemLocation.Y - 14f + player.velocity.Y
                )
            );
            Lighting.AddLight(position, 1.3f, 1.2f, 1f);
        }

        public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
        {
            dryTorch = true;
            wetTorch = true;
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Shadewood, 200);
            recipe.AddIngredient(ItemID.Gel, 300);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}