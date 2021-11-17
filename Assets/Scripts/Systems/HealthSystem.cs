using System;

public class HealthSystem // health system class that stores max health and current health information
{
    public event EventHandler OnHealthChanged;

    private int health;
    private int healthMax;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth() // returns current health
    {
        return health;
    }
    public void Damage(int damageAmount) // reduces current health by damage amount
    {
        health -= damageAmount;
        if (health < 0)
            health = 0;

        OnHealthChanged?.Invoke(this, EventArgs.Empty); // invokes health changed event so healthbar can be updated
    }
    public void Heal(int healAmount) // increases current health by heal amount
    {
        health += healAmount;
        if (health > healthMax)
            health = healthMax;

        OnHealthChanged?.Invoke(this, EventArgs.Empty); // invokes health changed event so healthbar can be updated
    }
}
