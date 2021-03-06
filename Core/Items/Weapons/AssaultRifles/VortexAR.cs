using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items.Weapons.AssaultRifles //Such namescape
{
    public class VortexAR : ModItem
    {
        //public static short glowMask;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vortex Pulse Rifle");
            Tooltip.SetDefault(
                "Five round burst"
              + "\nOnly the first shot consumes ammo"
              + "\n'I love the smell of pulse munitions in the morning'"
            );
            //if (Main.netMode != NetmodeID.Server)
            //    glowMask = GlowMaskAPI.Tools.instance.AddGlowMask(mod.GetTexture("Items/Weapons/AssaultRifles/VortexAR_Glow"));
        }

        public override void PostDrawInWorld(
            SpriteBatch spriteBatch,
            Color       lightColor,
            Color       alphaColor,
            float       rotation,
            float       scale,
            int         whoAmI
        )
        {
            var texture = mod.GetTexture("Items/Weapons/AssaultRifles/VortexAR_Glow");
            spriteBatch.Draw(
                texture,
                new Vector2(
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void PostDrawInInventory(
            SpriteBatch spriteBatch,
            Vector2     position,
            Rectangle   frame,
            Color       drawColor,
            Color       itemColor,
            Vector2     origin,
            float       scale
        )
        {
            /*
                        Texture2D texture = mod.GetTexture("Items/Weapons/AssaultRifles/VortexAR_Glow");
                        spriteBatch.Draw
                        (
                            texture,
                            new Vector2
                            (
                                item.position.X - Main.screenPosition.X + item.width * 0.5f,
                                item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                            ),
                            frame,
                            Color.White,
                            rotation,
                            texture.Size() * 0.5f,
                            scale,
                            SpriteEffects.None,
                            0f
                        );
                        */
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override bool Shoot(
            Player      player,
            ref Vector2 position,
            ref float   speedX,
            ref float   speedY,
            ref int     type,
            ref int     damage,
            ref float   knockBack
        )
        {
            //Vector2 position2 = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y + 10f + player.velocity.Y), true);
            //Lighting.AddLight(position2, .5f, .7f, 1f);
            var perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(1f));

            //speedX = perturbedSpeed.X;
            //speedY = perturbedSpeed.Y;
            if (player.itemAnimation >= item.useAnimation - 2) {
                int numberProjectiles = 1; //Fires extra projectile

                for (int i = 1; i == numberProjectiles; i++) {
                    var perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5f));
                    speedX = perturbedSpeed2.X;
                    speedY = perturbedSpeed2.Y;
                    var perturbedSpeed3 = new Vector2(speedX / 3.5f, speedY / 3.5f);

                    var    rnd        = new Random();
                    double projChance = rnd.NextDouble();
                    if (projChance < 0.33)
                        Projectile.NewProjectile(
                            position.X,
                            position.Y,
                            perturbedSpeed3.X,
                            perturbedSpeed3.Y,
                            616,
                            20,
                            0,
                            player.whoAmI
                        );
                }

                //}
            }

            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }


        public override bool ConsumeAmmo(Player player)
        {
            return !(player.itemAnimation < item.useAnimation - 2);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }

        public override void SetDefaults()
        {
            item.damage       = 53;
            item.crit         = 2;
            item.ranged       = true;
            item.width        = 72;
            item.height       = 34;
            item.useAnimation = 11;
            item.useTime      = 2;
            item.reuseDelay   = 14;
            item.useStyle     = 5;
            item.noMelee      = true;
            item.knockBack    = 1;
            item.value        = 165000;
            item.rare         = 10;
            item.autoReuse    = true;
            item.shoot        = 10;
            item.shootSpeed   = 20f;
            item.useAmmo      = AmmoID.Bullet;
            item.UseSound     = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Item_31_5");
            item.scale        = .85f;
            //item.glowMask = glowMask;
            //item.noUseGraphic = true;
            //item.glowMask = (short) mod.ItemType("VortexAR_Glow");
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FragmentVortex, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}