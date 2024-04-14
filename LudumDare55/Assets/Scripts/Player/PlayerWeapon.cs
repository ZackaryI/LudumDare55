
public class PlayerWeapon
{
    WeaponAttributes weaponAttributes;
    float damageBonus;
    public PlayerWeapon(WeaponAttributes weaponAttributes, float damageBonus) 
    {
        this.weaponAttributes = weaponAttributes;
        this.damageBonus = damageBonus;
    }
    public void Attack() 
    {
        /*
           shoots projectile
           or does melee attack
        */
        if (weaponAttributes.isMelee)
        {
            meleeAttack();
        }
        else 
        {
            RangeAttack();
        }
    }
    void meleeAttack() 
    {
    
    }
    void RangeAttack() 
    {
    
    }
    public void UpdateDamageBonus(float bonus) 
    {
        damageBonus = bonus;
    }
}
