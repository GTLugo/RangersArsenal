using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.NPCs
{
    public class RAGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.HeadlessHorseman && Main.pumpkinMoon && NPC.waveNumber > 10) {
                int chance = NPC.waveNumber - 8;
                if (Main.expertMode) chance++;

                if (Main.rand.Next(50) < chance)
                    Item.NewItem(
                        (int)npc.position.X,
                        (int)npc.position.Y,
                        npc.width,
                        npc.height,
                        mod.ItemType("PumpkinAR")
                    );
            }

            if (npc.type == NPCID.Golem)
                if (Main.expertMode)
                    Item.NewItem(
                        (int)npc.position.X,
                        (int)npc.position.Y,
                        npc.width,
                        npc.height,
                        mod.ItemType("SerumResearch")
                    );
        }
    }
}