
public class StatModifier
{
    public float damage;
    public float healthPoint;
    public float power;
    public float moveSpeed;

    public StatModifier() {
        damage         = 0f;
        healthPoint    = 0f;
        power          = 0f;
        moveSpeed      = 0f;
    }
    public StatModifier(float damage, float healthPoint, float power, float speed) {
        this.damage         = damage;
        this.healthPoint    = healthPoint;
        this.power          = power;
        this.moveSpeed      = speed;
    }
}
