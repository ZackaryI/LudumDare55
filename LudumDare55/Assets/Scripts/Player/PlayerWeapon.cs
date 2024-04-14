
using System;

public class PlayerWeapon
{
    WeaponAttributes weaponAttributes;
    public PlayerWeapon(WeaponAttributes weaponAttributes) 
    {
        this.weaponAttributes = weaponAttributes;
    }
    public void Attack(float bonus) 
    {
        /*
           shoots projectile
           or does melee attack
        */
        if (weaponAttributes.isMelee)
        {
            meleeAttack(bonus);
        }
        else 
        {
            RangeAttack(bonus);
        }
    }
    void meleeAttack(float bonus) 
    {
        throw new NotImplementedException();
    }
    void RangeAttack(float bonus) 
    {
        throw new NotImplementedException();
    }
}
