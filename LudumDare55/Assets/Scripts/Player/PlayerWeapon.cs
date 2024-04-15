
using UnityEngine;

public class PlayerWeapon
{
    WeaponAttributes weaponAttributes;
    Transform playerPosition;
    PlayerAttributes playerAttributes;
    AudioSource audioSource;
    public PlayerWeapon(WeaponAttributes weaponAttributes, Transform playerPosition, PlayerAttributes playerAttributes, AudioSource audioSource) 
    {
        this.weaponAttributes = weaponAttributes;
        this.playerPosition = playerPosition;
        this.playerAttributes = playerAttributes;
        this.audioSource = audioSource;
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

        AudioSource tempAudio = Object.Instantiate(audioSource, playerPosition.position, Quaternion.identity);
        tempAudio.clip = playerAttributes.swordSwing[Random.Range(0, playerAttributes.swordSwing.Length - 1)];
        tempAudio.Play();
    }
    void RangeAttack(float bonus, Projectile ammo) 
    {
        Projectile spawnedProjectile = Object.Instantiate(ammo, playerPosition.position, playerPosition.rotation);
        spawnedProjectile.projectileDamage = weaponAttributes.damage + bonus;
        spawnedProjectile.enemyProjectile = false;
        spawnedProjectile.playerProjectile = true;
        spawnedProjectile.GetComponent<EnemyDamageOnEnter>().damage = 0;

        Vector2 target = playerPosition.position + playerPosition.transform.up * 5;
        GameObject tempTarget = new();
        tempTarget.transform.position = target;
        spawnedProjectile.target = tempTarget.transform;
        Object.Destroy(tempTarget);
    }
}
