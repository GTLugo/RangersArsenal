using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RangersArsenal.Content.Items.Weapons {
  /// TODO: Major refactoring needed. Add way to easily set primary fire and alt fire settings
  // MOVE REVOLVER AND OTHER SPECIFIC CODE TO INHERITED CLASS
  public enum FireMode {
    Primary,
    Secondary,
    Bonus
  }

  public struct GunStats {
    public int   damage                 = 10;
    public int   crit                   = 5;
    public float knockback              = 10;
    public float bulletSpeed            = 14;

    public bool  isFullAuto             = false;
    public int   useTime                = 5;
    public int   useAnimationTime       = 10;
    public int   useDelay               = 0;

    public float spreadAngle            = 1;
    public int   bulletsPerBurst        = 1;
    public int   burstsBetweenEachBonus = 0;
    public float ammoSaveChance         = 0;


    public bool  convertsBullets        = false;
    public int   projectileType         = ProjectileID.Bullet;

    public LegacySoundStyle useSound    = SoundID.Item11;
  }

  public abstract class BaseGun : ModItem {
		//public override string Texture => RangersArsenal.AssetPath + "Textures/Items/Weapons/BaseGun";
    public abstract Dictionary<FireMode, GunStats> Stats { get; }
    
    public override void SetStaticDefaults() {
      CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
      SetGunData();
    }

    public void SetGunData() {
      SetGunDefaults();
      ApplyGunSettings(FireMode.Primary);
    }

    private void SetGunDefaults() {
      // these settings shouldn't change for any guns
      Item.useStyle   = ItemUseStyleID.Shoot;
      Item.DamageType = DamageClass.Ranged;
      Item.useAmmo    = AmmoID.Bullet;
      Item.shoot      = ProjectileID.PurificationPowder; // why the fuck
      Item.noMelee    = true;
    }

    private void ApplyGunSettings(FireMode mode) {
      // Stats
      Item.damage     = Stats[mode].damage;
      Item.crit       = Stats[mode].crit;
      Item.knockBack  = Stats[mode].knockback;
      Item.shootSpeed = Stats[mode].bulletSpeed;

      // Usage
      Item.useTime      = Stats[mode].useTime;
      Item.useAnimation = CalculateUseAnimation(mode);
      Item.reuseDelay   = Stats[mode].useDelay;

      Item.autoReuse    = Stats[mode].isFullAuto;
      Item.UseSound     = Stats[mode].useSound;
    }

    //public bool shouldPrimaryFire(Player player) => player.altFunctionUse == 1 && hasFireMode(FireMode.Primary);

    public bool shouldSecondaryFire(Player player) => player.altFunctionUse == 2 && hasFireMode(FireMode.Secondary);

    public bool hasFireMode(FireMode mode) => Stats.ContainsKey(mode);

    public FireMode currentFireMode(Player player) {
      if (shouldSecondaryFire(player)) {
        return FireMode.Secondary;
      } else {
        return FireMode.Primary;
      }
    }

    public override bool Shoot(Player player,
                               ProjectileSource_Item_WithAmmo source,
                               Vector2 position,
                               Vector2 velocity,
                               int type,
                               int damage,
                               float knockBack) {
      var modPlayer = player.GetModPlayer<RAPlayer>();
      
      // converts bullets
      if (type == ProjectileID.Bullet) type = _gunSettings.convertedBulletType;

      // inaccuracy
      float spreadAngle = _gunSettings.spreadAngle;
      if (_gunSettings.isRevolver && player.altFunctionUse == 2) {
          spreadAngle = 15;
          knockBack   =  item.knockBack * 2;
      }
      var perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(spreadAngle));
      speedX = perturbedSpeed.X;
      speedY = perturbedSpeed.Y;


      // extra rocket that fires only during first burst
      if (_gunSettings.isBurstFire && !(player.itemAnimation < item.useAnimation - 2)) {
          if (_gunSettings.isFiresExtraRockets && modPlayer.numBullets == _gunSettings.burstsBetweenRockets) {
              var perturbedSpeed2 =
                  new Vector2(speedX * 2f, speedY * 2f).RotatedByRandom(MathHelper.ToRadians(5f));
              speedX = perturbedSpeed2.X;
              speedY = perturbedSpeed2.Y;
              var perturbedSpeed3 = new Vector2(speedX, speedY);

              // muzzle offset
              var adjustedPosition = position;
              var muzzleOffset     = Vector2.Normalize(new Vector2(speedX, speedY)) * (player.itemWidth + 1);
              if (Collision.CanHit(adjustedPosition, 0, 0, adjustedPosition + muzzleOffset, 0, 0))
                  adjustedPosition += muzzleOffset;
              Projectile.NewProjectile(
                  adjustedPosition.X,
                  adjustedPosition.Y,
                  perturbedSpeed3.X,
                  perturbedSpeed3.Y,
                  _gunSettings.rocketProjectileType,
                  _gunSettings.rocketDamage,
                  5,
                  player.whoAmI
              );
          }

          // fires every couple bursts
          if (modPlayer.numBullets < _gunSettings.burstsBetweenRockets)
              modPlayer.numBullets++;
          else
              modPlayer.numBullets = 0;
      }

      return true;
    }

    public override bool CanConsumeAmmo(Player player) {
      bool isBurstFire = false;
      float ammoSaveChance = 0;
      ammoSaveChance = Stats[currentFireMode(player)].ammoSaveChance;

      bool wontSaveAmmo = Main.rand.NextFloat() < 1f - ammoSaveChance;
      if (isBurstFire) {
        bool willUseAmmoInBurst = !(player.itemAnimation < Item.useAnimation - 2);
        return willUseAmmoInBurst && wontSaveAmmo;
      }
      
      return wontSaveAmmo;
    }

    /// TODO: Needs to be redone to allow custom alt fire
    public override bool CanUseItem(Player player) {
      ApplyGunSettings(currentFireMode(player));
      return base.CanUseItem(player);
    }

    public override bool AltFunctionUse(Player player) => hasFireMode(FireMode.Secondary);
    
    private int CalculateUseAnimation(FireMode mode) => Stats[mode].useAnimationTime;
      //gunData_.isBurstFire ? Stats[mode].useTime * gunData_.burstCount : Stats[mode].useAnimationTime;
  }
}