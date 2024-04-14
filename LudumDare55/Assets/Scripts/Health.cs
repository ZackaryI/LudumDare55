using System;

public class Health
{
    float health;
    float curHealth;
    Action doAtDeath;
    Action doAtDamage;

    public Health(float health, Action doAtDeath, Action doAtDamage, float bonus, float currentHealth = 0) 
    {
        this.health = health + bonus;

        if (currentHealth > 0)
        {
            curHealth = currentHealth;
        }
        else 
        {
            curHealth = health;
        }

        this.doAtDeath = doAtDeath;
        this.doAtDamage = doAtDamage;
    }
    public void TakeDamage(float dmg) 
    {
        health -= dmg;
        doAtDamage();

        if (health <= 0) 
        {
            doAtDeath();
        }
    }
    public void Heal(float amount) 
    {
        curHealth = curHealth + amount < health ? curHealth + amount : health;
    }
    public float GetHealth() { return curHealth; }
}
