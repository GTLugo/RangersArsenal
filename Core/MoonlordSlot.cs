using Microsoft.Xna.Framework.Graphics;
using RangersArsenal.ThirdParty;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace RangersArsenal.Core
{
    public class MoonlordSlot : UIState
    {
        public CustomItemSlot moonLordSlot;

        public int slotsPositionX;
        public int slotsPositionY;

        public bool Visible {
            get {
                if (!Main.dedServ) {
                    var modPlayer = Main.LocalPlayer.GetModPlayer<RAPlayer>();
                    return Main.playerInventory && modPlayer.extraSlots == 2;
                }

                return false;
            }
        }

        public override void OnInitialize()
        {
            // add a texture to display when the accessory slot is empty
            var emptyTexture = new CroppedTexture2D(
                ModContent.GetInstance<RangersArsenal>().GetTexture("Core/AccessoryIcon"),
                CustomItemSlot.DefaultColors.EmptyTexture
            );

            moonLordSlot = new CustomItemSlot(ItemSlot.Context.EquipAccessory, 0.85f) {
                IsValidItem  = item => item.type > 0, // what do you want in the slot?
                EmptyTexture = emptyTexture,
                HoverText    = "Accessory" // try to describe what will go into the slot
            };

            Append(moonLordSlot);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            slotsPositionX = (int)(Main.screenWidth * 0.85f - 110f * Main.UIScale);
            slotsPositionY = Main.screenHeight - 50;
            // slotsPositionX = (int)(100);
            // slotsPositionY = (int)(500);

            moonLordSlot.Left.Set(slotsPositionX - 50, 0);
            moonLordSlot.Top.Set(slotsPositionY, 0);
            base.DrawSelf(spriteBatch);
        }
    }
}