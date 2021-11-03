using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Content.Items.Weapons {
  public class BasicRifle : ModItem {
		public override string Texture => RangersArsenal.AssetPath + "Textures/Items/Rifle";

    public override void SetStaticDefaults() {
      Tooltip.SetDefault("Basic, no frills rifle.");
      
      CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
      // Item
      Item.width = 68;
      Item.height = 24;
      Item.scale = 0.8f;
      Item.rare = ItemRarityID.White;

      // Usage
      Item.autoReuse = false;
      Item.useTime = 10;
      Item.useAnimation = 10;
      Item.useStyle = ItemUseStyleID.Shoot;
      Item.UseSound = SoundID.Item11;

      // Stats
      Item.shootSpeed = 10f;
      Item.damage = 10;
      Item.knockBack = 10f;

      // Properties
      Item.DamageType = DamageClass.Ranged;
      Item.useAmmo = AmmoID.Bullet;
      Item.shoot = ProjectileID.PurificationPowder; // why the fuck
      Item.noMelee = true;
    }

    public override Vector2? HoldoutOffset() {
      return new Vector2(-9, 0);
    }
  }
}