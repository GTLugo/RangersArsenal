using Microsoft.Xna.Framework.Graphics;
using RangersArsenal.ThirdParty;
using Terraria;
using Terraria.UI;

namespace RangersArsenal.Core
{
    public class ExtraAccessorySlots : UIState
    {
        private int _slotsPositionX;
        private int _slotsPositionY;

        public CustomItemSlot[] Slots { get; set; }

        public bool Visible => Main.playerInventory;

        protected int GetOffset()
        {
            return -50;
        }

        protected bool IsUnlocked(int slotIndex)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<RAPlayer>();
            return modPlayer.extraSlots > slotIndex;
        }

        public override void OnInitialize()
        {
            // add a texture to display when the accessory slot is empty
            var emptyTexture = new CroppedTexture2D(
                RangersArsenal.instance.GetTexture("Core/AccessoryIcon"),
                CustomItemSlot.DefaultColors.EmptyTexture
            );
            Slots = new CustomItemSlot[RangersArsenal.maxExtraSlots];

            for (int i = 0; i < Slots.Length; ++i) {
                Slots[i] = new CustomItemSlot(ItemSlot.Context.EquipAccessory, 0.85f) {
                    IsValidItem  = item => item.type > 0, // what do you want in the slot?
                    EmptyTexture = emptyTexture,
                    HoverText    = "Accessory" // try to describe what will go into the slot
                };

                Append(Slots[i]);
            }
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            _slotsPositionX = (int)(Main.screenWidth * 0.85f - 100f * Main.UIScale);
            _slotsPositionY = Main.screenHeight - 50;

            // slotsPositionX = (int)(100);
            // slotsPositionY = (int)(500);
            for (int i = 0; i < Slots.Length; ++i)
                if (IsUnlocked(i)) {
                    Slots[i].Left.Set(_slotsPositionX + GetOffset() * i, 0);
                    Slots[i].Top.Set(_slotsPositionY, 0);
                }

            base.DrawSelf(spriteBatch);
        }
    }
}