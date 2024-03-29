using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.NPCs.Bosses.SunGod
{
    public class SunMinion : ModNPC
    {
        private float rotationCount;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sun Minion");
        }

        public override void SetDefaults()
        {
            npc.width                     = 34;
            npc.height                    = 34;
            npc.damage                    = 24;
            npc.defense                   = 25;
            npc.lifeMax                   = 5000;
            npc.value                     = 60f;
            npc.knockBackResist           = 0.5f;
            npc.aiStyle                   = 5;
            npc.lavaImmune                = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.noGravity                 = true;
            npc.noTileCollide             = true;
            npc.HitSound                  = SoundID.NPCHit30;
            npc.DeathSound                = SoundID.NPCDeath33;
            aiType                        = NPCID.Probe;
        }

        public float Rotate(float angle)
        {
            rotationCount += angle;
            return (float)Math.Asin(Math.Sin(rotationCount)) * 2;
        }

        private void DrawCorona(SpriteBatch spriteBatch, Color drawColor)
        {
            var corona = mod.GetTexture("NPCs/Bosses/SunGod/SunMinionCorona");
            spriteBatch.Draw(
                corona,
                new Vector2(
                    npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                    npc.position.Y - Main.screenPosition.Y + npc.height * 0.5f
                ),
                new Rectangle(0, 0, corona.Width, corona.Height),
                drawColor,
                Rotate(0.01f),
                corona.Size() * 0.5f,
                npc.scale,
                SpriteEffects.None,
                0f
            );
            spriteBatch.Draw(
                corona,
                new Vector2(
                    npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                    npc.position.Y - Main.screenPosition.Y + npc.height * 0.5f
                ),
                new Rectangle(0, 0, corona.Width, corona.Height),
                drawColor,
                Rotate(0.01f) * -1f,
                corona.Size() * 0.5f,
                npc.scale,
                SpriteEffects.None,
                0f
            );
        }

        private void DrawBody(SpriteBatch spriteBatch, Color drawColor)
        {
            var body = mod.GetTexture("NPCs/Bosses/SunGod/SunMinionBody");

            spriteBatch.Draw(
                body,
                new Vector2(
                    npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                    npc.position.Y - Main.screenPosition.Y + npc.height * 0.5f
                ),
                new Rectangle(0, 0, body.Width, body.Height),
                drawColor,
                npc.rotation,
                body.Size() * 0.5f,
                npc.scale,
                SpriteEffects.None,
                0f
            );
        }

        // Custom rendering //
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            DrawCorona(spriteBatch, Color.White);
            DrawBody(spriteBatch, Color.White);
            return false;
        }
    }
}