
using UnityEngine;

public class PlayerWeapon
{
    WeaponAttributes weaponAttributes;
    Transform playerPosition;
    public PlayerWeapon(WeaponAttributes weaponAttributes, Transform playerPosition) 
    {
        this.weaponAttributes = weaponAttributes;
        this.playerPosition = playerPosition;
    }
    public void Attack(float bonus, Projectile ammo, DamageEnemy melee) 
    {
        if (weaponAttributes.isMelee)
        {
            MeleeAttack(bonus, melee);
        }

        if (ammo == null) 
        {
            return;
        }

        if(weaponAttributes.isMelee != true)
        {
            RangeAttack(bonus, ammo);
        }
    }
    void MeleeAttack(float bonus, DamageEnemy melee) 
    {
         DamageEnemy temp = Object.Instantiate(melee, playerPosition.position, playerPosition.rotation);
         temp.damage = weaponAttributes.damage + bonus;

    }
    void RangeAttack(float bonus, Projectile ammo) 
    {
        Projectile spawnedProjectile = Object.Instantiate(ammo, playerPosition.position, playerPosition.rotation);
        spawnedProjectile.projectileDamage = weaponAttributes.damage + bonus;
        spawnedProjectile.enemyProjectile = false;
        spawnedProjectile.playerProjectile = true;

        Vector2 target = playerPosition.position + playerPosition.transform.up * 5;
        GameObject tempTarget = new();
        tempTarget.transform.position = target;
        spawnedProjectile.target = tempTarget.transform;
        Object.Destroy(tempTarget);
    }
}
