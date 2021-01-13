/**
 * Interface providing variables for damage amount and damage type.
 * Should be implemented by every Object that is supposed to deal damage.
 */
public interface IDamageDealer
{
    int damage
    {
        get;
        set;
    }

    DamageType damageType
    {
        get;
        set;
    }

    DamageSource damageSource
    {
        get;
        set;
    }

}
