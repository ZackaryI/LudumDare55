public class ItemBonuses
{
    float damageBonus;
    float speedBonus;
    float hpBonus;
    float summonCapacityBonus;

    public ItemBonuses()
    {
        damageBonus = 0;
        speedBonus = 0;
        hpBonus = 0;
        summonCapacityBonus = 0;
    }
    public void UpdateValues(float damageBonus, float speedBonus, float hpBonus, float summonCapacityBonus) 
    {
        this.damageBonus += damageBonus;
        this.speedBonus += speedBonus;
        this.hpBonus += hpBonus;
        this.summonCapacityBonus += summonCapacityBonus;
    }
    public float DamageBonus
    {
        get { return damageBonus; }
        set { damageBonus = value; }
    }
    public float SpeedBonus
    {
        get { return speedBonus; }
        set { speedBonus = value; }
    }
    public float HpBonus
    {
        get { return hpBonus; }
        set { hpBonus = value; }
    }
    public float SummonCapacityBonus
    {
        get { return summonCapacityBonus; }
        set { summonCapacityBonus = value; }
    }
}