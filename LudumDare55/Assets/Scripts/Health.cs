using System;
using UnityEngine.UI;

public class Health
{
    float health;
    float curHealth;
    Action doAtDeath;
    Action doAtDamage;
    Slider healthbar;

    public Health(float health, Action doAtDeath, Action doAtDamage, float bonus, Slider healthbar, float currentHealth = 0) 
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
        this.healthbar = healthbar;

        healthbar.value = curHealth / health;
    }
    public void TakeDamage(float dmg) 
    {
        curHealth -= dmg;
        healthbar.value = curHealth / health;

        doAtDamage();

        if (curHealth <= 0) 
        {
            doAtDeath();
        }
    }
    public void Heal(float amount) 
    {
        curHealth = curHealth + amount < health ? curHealth + amount : health;

        healthbar.value = curHealth / health;
    }
 
    public float GetHealth() { return curHealth; }
}
