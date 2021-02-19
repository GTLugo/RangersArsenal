using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Weapons.Ammo
{
    internal class FriendlyFire : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Friendly Fire Round");
            Tooltip.SetDefault("Harms townspeople and yourself");
        }

        public override void SetDefaults()
        {
            item.damage     = 3;
            item.ranged     = true;
            item.width      = 8;
            item.height     = 8;
            item.maxStack   = 999;
            item.consumable = true;
            item.knockBack  = 1.5f;
            item.value      = 1;
            item.rare       = -1;
            item.shoot      = mod.ProjectileType("FriendlyFire");
            item.shootSpeed = 8f;
            item.ammo       = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MusketBall, 25);
            recipe.AddIngredient(ItemID.BloodWater);
            recipe.SetResult(this, 25);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MusketBall, 25);
            recipe.AddIngredient(ItemID.UnholyWater);
            recipe.SetResult(this, 25);
            recipe.AddRecipe();
        }
    }
}