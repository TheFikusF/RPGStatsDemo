using System.Collections.Generic;

namespace RPGStatsMaker.Stats;

public class PlayerCharacter : Character
{
    public PlayerCharacter() : base(SelfType.Self)
    { }
}

public abstract class Character
{
    public class StatsBody
    {
        public SelfType SelfType { get; }

        private List<Stat> _stats;

        internal StatsBody(SelfType selfType)
        {
            SelfType = selfType;
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
    
    private StatsBody _stats;
    public StatsBody Stats => _stats;
    public SelfType SelfType => _stats.SelfType;

    public Character(SelfType selfType)
    {
        _stats = new StatsBody(selfType);
    }
}