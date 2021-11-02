using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using RangersArsenal.Core;
using RangersArsenal.Core.Dusts;
using RangersArsenal.Core.Items.Accessories.Symbiote;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace RangersArsenal
{
    internal class RangersArsenal : Mod
    {
        internal static RangersArsenal instance;
        
        public const int maxExtraSlots = 3;
        private UserInterface _accessorySlotUI;
        public  ExtraAccessorySlots extraAccessorySlots;

        public RangersArsenal()
        {
            Properties = new ModProperties {
                Autoload = true, AutoloadGores = true, AutoloadSounds = true
            };
        }


        public override void Load()
        {
            instance = ModContent.GetInstance<RangersArsenal>();

            if (!Main.dedServ) {
                // you can only display the ui to the local player -- prevent an error message!
                // Add certain equip textures
                AddEquipTexture(
                    new SymbioteHead(),
                    null,
                    EquipType.Head,
                    "SymbioteHead",
                    "RangersArsenal/Core/Items/Accessories/Symbiote/Symbiote_Head"
                );
                AddEquipTexture(
                    new SymbioteBody(),
                    null,
                    EquipType.Body,
                    "SymbioteBody",
                    "RangersArsenal/Core/Items/Accessories/Symbiote/Symbiote_Body",
                    "RangersArsenal/Core/Items/Accessories/Symbiote/Symbiote_Arms",
                    "RangersArsenal/Core/Items/Accessories/Symbiote/Symbiote_FemaleBody"
                );
                AddEquipTexture(
                    new SymbioteLegs(),
                    null,
                    EquipType.Legs,
                    "SymbioteLegs",
                    "RangersArsenal/Core/Items/Accessories/Symbiote/Symbiote_Legs"
                );
                AddDust("BulletTrail", new BulletTrail(), "RangersArsenal/Core/Dusts/BulletTrail");

                _accessorySlotUI    = new UserInterface();
                extraAccessorySlots = new ExtraAccessorySlots();
                extraAccessorySlots.Activate();
                _accessorySlotUI.SetState(extraAccessorySlots);

                /*_golemSlotUI = new UserInterface();
                golemSlot       = new GolemSlot();
                golemSlot.Activate();
                _golemSlotUI.SetState(golemSlot);
                
                _moonlordSlotUI = new UserInterface();
                moonlordSlot    = new MoonlordSlot();
                moonlordSlot.Activate();
                _moonlordSlotUI.SetState(moonlordSlot);*/
            }

            base.Load();
        }

        // make sure the ui can draw
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            // this will draw on the same layer as the inventory
            int inventoryLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));

            if (inventoryLayer != -1)
                layers.Insert(
                    inventoryLayer,
                    new LegacyGameInterfaceLayer(
                        "RangersArsenal: Golem Slot UI",
                        () => {
                            for (int i = 0; i < maxExtraSlots; ++i)
                                if (extraAccessorySlots.Visible)
                                    _accessorySlotUI.Draw(Main.spriteBatch, new GameTime());

                            return true;
                        },
                        InterfaceScaleType.UI
                    )
                );
            /*layers.Insert(
                    inventoryLayer,
                    new LegacyGameInterfaceLayer(
                        "RangersArsenal: Moon Lord Slot UI",
                        () => {
                            if (moonlordSlot.Visible) _moonlordSlotUI.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI
                    )
                );*/
        }

        public override void PostSetupContent()
        {
            /*var bossChecklist = ModLoader.GetMod("BossChecklist");
            //List<int> CollectionItemIDs = ;
            //List<int> LootItemIDs = ;
            bossChecklist?.Call(
                "AddBoss",
                12.5f,
                ModContent.NPCType<SunGod>(),
                this,
                "Sun God",
                (Func<bool>)(() => RAWorld.downedSunGod),
                ModContent.ItemType<MayanCalendar>(),
                new List<int>(),
                new List<int> {
                    ModContent.ItemType<GoldenGun>(),
                    ModContent.ItemType<SunPowerSeed>(),
                    ModContent.ItemType<BeskarBar>(),
                    ModContent.ItemType<BeskarOre>()
                },
                "Craft a Mayan calendar at the Lihzahrd Altar and use it at night to summon the Sun God",
                "The Sun God reigns supreme",
                "RangersArsenal/NPCs/Bosses/SunGod/SunGod2",
                "RangersArsenal/NPCs/Bosses/SunGod/SunGod_Head_Boss"
            );*/
            Projectile.maxAI += 3;
        }

        //public override void HandlePacket(BinaryReader reader, int whoAmI) {
        //  ModNetHandler.HandlePacket(reader, whoAmI);
        //}

        public override void AddRecipeGroups()
        {
            var goldRecipeGroup = new RecipeGroup(
                () => Language.GetTextValue("LegacyMisc.37") + " Gold Bar",
                ItemID.GoldBar,
                ItemID.PlatinumBar
            );
            RecipeGroup.RegisterGroup("RangersArsenal:GoldBar", goldRecipeGroup);
            var evilChunkRecipeGroup = new RecipeGroup(
                () => Language.GetTextValue("LegacyMisc.37") + " Evil Chunk",
                ItemID.RottenChunk,
                ItemID.Vertebrae
            );
            RecipeGroup.RegisterGroup("RangersArsenal:EvilChunk", evilChunkRecipeGroup);
            var evilAccRecipeGroup = new RecipeGroup(
                () => Language.GetTextValue("LegacyMisc.37") + " Evil Accessory",
                ItemID.WormScarf,
                ItemID.BrainOfConfusion
            );
            RecipeGroup.RegisterGroup("RangersArsenal:EvilAccessory", evilAccRecipeGroup);
        }

        public override void AddRecipes()
        {
            var newLeather = new ModRecipe(this);
            newLeather.AddRecipeGroup("RangersArsenal:EvilChunk", 3);
            newLeather.AddTile(TileID.WorkBenches);
            newLeather.SetResult(ItemID.Leather);
            newLeather.AddRecipe();

            var finder = new RecipeFinder();
            finder.AddIngredient(ItemID.RottenChunk, 5);
            finder.AddTile(TileID.WorkBenches);
            finder.SetResult(ItemID.Leather);
            var recipe2 = finder.FindExactRecipe();
            if (recipe2 == null) return;
            var editor = new RecipeEditor(recipe2);
            editor.DeleteRecipe();
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var msgType = (RAModMessageType)reader.ReadByte();

            switch (msgType) {
                case RAModMessageType.RAPlayerSyncPlayer:
                    byte playernumber = reader.ReadByte();
                    var  modPlayer    = Main.player[playernumber].GetModPlayer<RAPlayer>();
                    int  serums       = reader.ReadInt32();
                    int  extraSlots   = reader.ReadInt32();
                    int  numBullets   = reader.ReadInt32();
                    modPlayer.serums     = serums;
                    modPlayer.extraSlots = extraSlots;
                    modPlayer.numBullets = numBullets;
                    // SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
                    break;
            }

            //ModNetHandler.HandlePacket(reader, whoAmI);
        }
    }

    internal enum RAModMessageType : byte
    {
        RAPlayerSyncPlayer
    }

    internal class ModNetHandler
    {
        // When a lot of handlers are added, it might be wise to automate
        // creation of them
        public const    byte                     bulletTrailType = 2;
        internal static BulletTrailPacketHandler bulletTrail     = new BulletTrailPacketHandler(bulletTrailType);

        public static void HandlePacket(BinaryReader r, int fromWho)
        {
            switch (r.ReadByte()) {
                case bulletTrailType:
                    bulletTrail.HandlePacket(r, fromWho);
                    break;
            }
        }
    }

    internal abstract class PacketHandler
    {
        protected PacketHandler(byte handlerType)
        {
            HandlerType = handlerType;
        }

        internal byte HandlerType { get; set; }

        public abstract void HandlePacket(BinaryReader reader, int fromWho);

        protected ModPacket GetPacket(byte packetType, int fromWho)
        {
            var p = RangersArsenal.instance.GetPacket();
            p.Write(HandlerType);
            p.Write(packetType);
            if (Main.netMode == NetmodeID.Server) p.Write((byte)fromWho);
            return p;
        }
    }

    internal class BulletTrailPacketHandler : PacketHandler
    {
        public const byte SyncProjectile = 2;

        public BulletTrailPacketHandler(byte handlerType) : base(handlerType) { }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            //throw new NotImplementedException();
            switch (reader.ReadByte()) {
                case SyncProjectile:
                    ReceiveProjectile(reader, fromWho);
                    break;
            }
        }

        public void SendProjectile(int toWho, int fromWho, int projectile, int trail, int iter)
        {
            var packet = GetPacket(SyncProjectile, fromWho);
            packet.Write(projectile);
            packet.Write(trail);
            packet.Write(iter);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveProjectile(BinaryReader reader, int fromWho)
        {
            int projectile = reader.ReadInt32();
            int trail      = reader.ReadInt32();
            int iter       = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server) {
                SendProjectile(-1, fromWho, projectile, trail, iter);
            } else {
                var targetProj = Main.projectile[projectile];
                var dust       = Main.dust[trail];

                var projectilePosition = targetProj.position;
                projectilePosition -= targetProj.velocity * (iter * 0.25f);

                Main.dust[trail].position = projectilePosition;
                //Main.dust[trail].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                Main.dust[trail].velocity  *= 0.2f;
                Main.dust[trail].noGravity =  true;
                targetProj.netUpdate       =  true;
            }
        }
    }
}