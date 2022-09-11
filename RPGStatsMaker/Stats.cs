using System.Collections.Generic;
using System.Linq;

namespace RPGStatsMaker;

public class PlayerStats : Stats
{
    public PlayerStats() : base()
    {
        SelfType = SelfType.Self;
    }
}

public abstract class Stats
{
    public SelfType SelfType { get; protected set; }

    private List<Stat> _stats;

    public Stats()
    {
        _stats = new List<Stat>();
    }

    public void AddStat(Stat stat) => _stats.Add(stat);

    public void RemoveStat(Stat stat) => _stats.Remove(stat);

    public double GetDamageValue(double startValue, SourceType sourceType, DamageType damageType)
    {
        double addedDamage = new DamageStat(source: sourceType, 
            body: SelfType, 
            increas: IncreasType.Added, 
            damage: damageType).GetSum(_stats);
        
        double increaseDamage = new DamageStat(source: sourceType, 
            body: SelfType, 
            increas: IncreasType.Increase, 
            damage: damageType).GetSum(_stats);
        
        double increaseTotal = new DamageStat(source: sourceType, 
            body: SelfType, 
            increas: IncreasType.IncreaseTotal, 
            damage: damageType).GetSum(_stats);
        
        return (startValue + addedDamage) * (100 + increaseDamage) * 0.01 * (100 + increaseTotal) * 0.01;
    }
}