using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Content.Items.Weapons {
  public class BasicRifle : BaseGun {
		public override string Texture => RangersArsenal.AssetPath + "Textures/Items/Weapons/Rifle";

    public override Dictionary<FireMode, GunStats> Stats { get; } = new Dictionary<FireMode, GunStats>{
      {FireMode.Primary, new GunStats{
        damage    = 10,
        crit      = 5,
        knockback = 10,
        useDelay  = 10,
      }},
      {FireMode.Secondary, new GunStats{
        damage           = 10,
        crit             = 5,
        knockback        = 10,
        bulletsPerSpread = 5,
        //bulletsPerBurst  = 3,
        //useSound         = SoundID.Item31,
        spreadAngle      = 15f,
      }}
    };

    public override void SetStaticDefaults() {
      Tooltip.SetDefault("Alt-fire shoots a spread of bullets.");
      
      CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
      Item.width  = 68;
      Item.height = 24;
      Item.scale  = 0.8f;
      Item.rare   = ItemRarityID.White;
      Item.value  = Item.sellPrice(1, 0, 0, 69);

      base.SetDefaults();
    }

    public override Vector2? HoldoutOffset() => new Vector2(-9, 0);

    public override void AddRecipes() => CreateRecipe()
      .AddRecipeGroup("Wood", 15)
      .AddRecipeGroup("IronBar", 15)
      .Register();
  }
}