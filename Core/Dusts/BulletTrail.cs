using Terraria;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Dusts
{
    internal class BulletTrail : ModDust
    {
        public override void SetDefaults()
        {
            Dust.CloneDust(75);
        }

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
        }
    }
}