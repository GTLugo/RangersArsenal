using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Core.Items
{
    public class RAGlobalItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();

            if (item.ranged && modPlayer.hasStock && player.altFunctionUse != 2) {
                item.autoReuse = true;
            } else if (item.ranged && !modPlayer.hasStock && player.altFunctionUse != 2) {
                var itemClone = item.Clone();
                itemClone.CloneDefaults(item.type);
                item.autoReuse = itemClone.autoReuse;
            }

            return base.CanUseItem(item, player);
        }

        public override void PickAmmo(
            Item      weapon,
            Item      ammo,
            Player    player,
            ref int   type,
            ref float speed,
            ref int   damage,
            ref float knockback
        )
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();

            if (weapon.useAmmo == AmmoID.Bullet && modPlayer.gunslingerBuff) {
                knockback = (int)((double)knockback * 1.2f);
                damage    = (int)((double)damage * 1.2f);
            }

            base.PickAmmo(weapon, ammo, player, ref type, ref speed, ref damage, ref knockback);
        }

        private static bool HoldingGun(Player player)
        {
            return player.HeldItem.shoot == 10;
        }

        /*public override bool Shoot(
            Item        item,
            Player      player,
            ref Vector2 position,
            ref float   speedX,
            ref float   speedY,
            ref int     type,
            ref int     damage,
            ref float   knockBack
        )
        {
            var modPlayer = player.GetModPlayer<RAPlayer>();
            if (modPlayer.areBulletsAccelerated && item.ranged && HoldingGun(player)) {
                // muzzle offset
                var muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * (player.itemWidth);
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                {
                    position += muzzleOffset;
                }
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }*/
    }
}