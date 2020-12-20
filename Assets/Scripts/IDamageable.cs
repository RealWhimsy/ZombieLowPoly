/**
 * Should be implemented by every Object that is supposed to take damage
 */
public interface IDamageable
{
    void TakeDamage(IDamageDealer damage);
}