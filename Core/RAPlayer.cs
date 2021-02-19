using System.Diagnostics.CodeAnalysis;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RangersArsenal.Core
{
    public class RAPlayer : ModPlayer
    {
        public const int  MaxSerums = 1;
        public       bool areBulletsAccelerated;
        public       bool areBulletsAccurate;
        public       int  extraSlots;

        public bool gunslingerBuff;
        public bool hasAmp;
        public bool hasArcReactor;
        public bool hasBeskarBreast;
        public bool hasGolemSlot;
        public bool hasGrip;
        public bool hasMag;
        public bool hasMoonLordSlot;
        public bool hasMSeed;
        public bool hasMuzzle;
        public bool hasSeed;
        public bool hasStock;

        public bool hasSymbiote;
        public bool hasSymbiotePrev;
        public int  numBullets;
        public int  serums;
        public bool symbioteBuff;
        public bool symbioteForceVanity;
        public bool symbioteHideVanity;

        //public static readonly PlayerLayer GlowLayer = new PlayerLayer("RangersArsenal", "GlowLayer", null, delegate (PlayerDrawInfo drawInfo)//layer for my glow mask
        //{
        //    Player drawPlayer = drawInfo.drawPlayer;//shortcut to the player
        //    ModPlayer modPlayer = drawPlayer.GetModPlayer<RAPlayer>();
        //    if (drawPlayer.invis || drawPlayer.dead) //if the player is invisible or dead
        //        return; //don't bother with this layer


        //    Texture2D glowTex; //container for local head texture
        //    if (/*your condition goes here*/) {

        //        glowTex = ;//GetTexture("Items/Accessories/ArcReactor/VortexAmplifier");//reference to your texture here
        //    }
        //    //  else if ()//if you want to draw a different glow mask
        //    //{
        //    //  }
        //    else {
        //        return; //do nothing when I don't want a glow mask
        //    }
        //    //calculate position to draw the sprite at
        //    Vector2 drawPosition = new Vector2((float)((int)(drawPlayer.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2f) + (float)(drawPlayer.width / 2f))),
        //        (float)((int)(drawPlayer.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f + drawPlayer.gfxOffY - drawPlayer.mount.PlayerOffset)));
        //    //draw the sprite
        //    Main.playerDrawData.Add(new DrawData(armTex, drawPosition + drawPlayer.bodyPosition + drawInfo.bodyOrigin,
        //        drawPlayer.bodyFrame, color.White, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0));
        //});

        [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        public override void clientClone(ModPlayer clientClone)
        {
            // Here we would make a backup clone of values that are only correct on the local players Player instance.
            var clone = clientClone as RAPlayer;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            // REMEMBER TO ADD PACKET READING TO RangersArsenal.cs
            var packet = mod.GetPacket();
            packet.Write((byte)RAModMessageType.RAPlayerSyncPlayer);
            packet.Write((byte)player.whoAmI);
            packet.Write(serums);
            packet.Write(extraSlots);
            packet.Write(numBullets);
            packet.Send(toWho, fromWho);
        }

        [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            // Here we would sync something like an RPG stat whenever the player changes it.
            var clone = clientPlayer as RAPlayer;
        }

        public override TagCompound Save()
        {
            return new TagCompound {
                {
                    "serums", serums
                }, {
                    "extraSlots", extraSlots
                }
            };
        }

        public override void Load(TagCompound tag)
        {
            serums     = tag.GetInt("serums");
            extraSlots = tag.GetInt("extraSlots");
        }

        /*public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            Item heldItem = player.inventory[player.selectedItem];
            if (heldItem.modItem != null)
            {
                if (heldItem.modItem.GetType() == mod.ItemType("VortexAR").GetType())
                {
                    Texture2D texture = mod.GetTexture("Items/Weapons/AssaultRifles/VortexAR_Glow");
                    Vector2 value2 = drawInfo.position + (player.itemLocation - player.position);
                    Vector2 vector10 = new Vector2((float)(Main.itemTexture[player.inventory[player.selectedItem].type].Width / 2), (float)(Main.itemTexture[player.inventory[player.selectedItem].type].Height / 2));
                    Vector2 vector11 = player.itemLocation;
                    int num107 = (int)vector11.X;

                    Vector2 origin5 = new Vector2((float)(-(float)num107), (float)(Main.itemTexture[player.inventory[player.selectedItem].type].Height / 2));

                    DrawData value = new DrawData(texture, new Vector2((float)((int)(value2.X - Main.screenPosition.X + vector10.X)), (float)((int)(value2.Y - Main.screenPosition.Y + vector10.Y))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.itemTexture[player.inventory[player.selectedItem].type].Width, Main.itemTexture[player.inventory[player.selectedItem].type].Height)), new Microsoft.Xna.Framework.Color(250, 250, 250, player.inventory[player.selectedItem].alpha), player.itemRotation, origin5, player.inventory[player.selectedItem].scale, SpriteEffects.None, 0);
                    Main.playerDrawData.Add(value);
                }
            }
            base.ModifyDrawInfo(ref drawInfo);
        }*/


        public override void PostUpdateMiscEffects()
        {
            base.PostUpdateMiscEffects();

            //player.potionDelayTime = Item.potionDelay;
            //player.restorationDelayTime = Item.restorationDelay;
            if (player.GetModPlayer<RAPlayer>().hasMSeed) {
                player.potionDelayTime      = (int)(player.potionDelayTime * (2.0 / 3.0));
                player.restorationDelayTime = (int)(player.restorationDelayTime * (2.0 / 3.0));
            }
        }

        public override void ResetEffects()
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();

            hasMuzzle             = false;
            hasGrip               = false;
            hasStock              = false;
            hasAmp                = false;
            hasArcReactor         = false;
            hasSeed               = false;
            hasMSeed              = false;
            hasMag                = false;
            hasBeskarBreast       = false;
            areBulletsAccelerated = false;
            areBulletsAccurate    = false;
            // extraSlots            = 0;
            // hasGolemSlot          = false;

            gunslingerBuff = false;

            hasSymbiotePrev = hasSymbiote; // previous state of hasSymbiote (for consistency because of order of hooks)
            hasSymbiote = false;
            symbioteBuff = false;
            symbioteHideVanity = false;
            symbioteForceVanity = false;

            if (numBullets < 0) numBullets = 0;

            // Super Soldier Serum effects: //
            if (modPlayer.serums > 0) {
                player.statLifeMax2   += serums * 100;
                player.moveSpeed      += serums * 0.1f;
                player.jumpSpeedBoost += serums * 1.2f;
                player.lifeRegen      += serums * 2;
                player.statDefense    += serums * 4;
                player.meleeSpeed     += serums * 0.1f;
                player.meleeDamage    += serums * 0.1f;
                player.meleeCrit      += serums * 2;
                player.rangedDamage   += serums * 0.1f;
                player.rangedCrit     += serums * 2;
                player.magicDamage    += serums * 0.1f;
                player.magicCrit      += serums * 2;
                player.pickSpeed      -= serums * 0.15f;
                player.minionDamage   += serums * 0.1f;
                player.minionKB       += serums * 0.5f;
                player.thrownDamage   += serums * 0.1f;
                player.thrownCrit     += serums * 2;
            }

            //if (player.GetModPlayer<RAPlayer>().symbioteBuff) {
            //}
            // player.extraAccessorySlots += modPlayer.extraSlots;

            base.ResetEffects();
        }

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (player.GetModPlayer<RAPlayer>().hasSeed || player.GetModPlayer<RAPlayer>().hasMSeed)
                healValue = (int)(1.15 * healValue);
            base.GetHealLife(item, quickHeal, ref healValue);
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo)
        {
            var modPlayer = Main.player[weapon.owner].GetModPlayer<RAPlayer>();
            //if (!base.ConsumeAmmo(weapon, ammo)) return false;
            //var rnd = new Random();


            if (modPlayer.hasMag && Main.rand.NextDouble() < 0.10) return false;

            if (modPlayer.hasBeskarBreast && Main.rand.NextDouble() < 0.30) return false;

            return base.ConsumeAmmo(weapon, ammo);
        }

        public override void UpdateVanityAccessories()
        {
            for (int n = 13; n < 18 + player.extraAccessorySlots; n++) {
                var item = player.armor[n];

                if (item.type == mod.ItemType("Symbiote")) {
                    symbioteHideVanity  = false;
                    symbioteForceVanity = true;
                }
            }
        }

        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
        {
            // Make sure this condition is the same as the condition in the Buff to remove itself. We do this here instead of in ModItem.UpdateAccessory in case we want future upgraded items to set blockyAccessory
            if (hasSymbiote) player.AddBuff(mod.BuffType("Symbiote"), 60);

            for (int i = 0; i < RangersArsenal.maxExtraSlots; ++i) {
                //if (!Main.dedServ) {
                var slot = RangersArsenal.instance.extraAccessorySlots.Slots[i];

                if (slot.Item != null) {
                    if (slot.Item.modItem != null) {
                        slot.Item.modItem.UpdateAccessory(player, !slot.ItemVisible);
                        slot.Item.modItem.UpdateEquip(player);
                    }

                    player.VanillaUpdateAccessory(
                        player.whoAmI,
                        slot.Item,
                        !slot.ItemVisible,
                        ref wallSpeedBuff,
                        ref tileSpeedBuff,
                        ref tileRangeBuff
                    );
                    player.VanillaUpdateEquip(slot.Item);
                }

                //}
            }
        }

        public override void FrameEffects()
        {
            if ((symbioteBuff || symbioteForceVanity) && !symbioteHideVanity) {
                player.legs = mod.GetEquipSlot("SymbioteLegs", EquipType.Legs);
                player.body = mod.GetEquipSlot("SymbioteBody", EquipType.Body);
                player.head = mod.GetEquipSlot("SymbioteHead", EquipType.Head);
            }
        }
    }
}