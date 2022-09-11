using System;
using System.Linq;
using System.Collections.Generic;

namespace RPGStatsMaker;

public enum SelfType
{
    Self, Minion
}

public enum SourceType
{
    Hit, Spell, Any
}

public enum DamageType
{
    Fire, Cold, Lightning, Chaos, Physics, All
}

public enum IncreasType
{
    Added, Increase, IncreaseTotal
}

public class DamageStat : Stat
{
    public DamageType DamageType { get; }

    public DamageStat(double value = 0, 
        SelfType body = SelfType.Self,
        SourceType source = SourceType.Any,
        DamageType damage = DamageType.All,
        IncreasType increas = IncreasType.Increase) : base(value, body, source, increas)
    {
        DamageType = damage;
    }

    public override double GetSum(List<Stat> stats)
    {
        var l = stats.Where(x => x is DamageStat).Cast<DamageStat>()
            .Where(x => (x.DamageType == DamageType || x.DamageType == DamageType.All) && 
                        (x.Source == Source || x.Source == SourceType.Any) && 
                        x.Self == Self &&
                        x.IncreasType == IncreasType);

        return l.Sum(x => x.Value);
    }

    public override string ToString()
    {
        return IncreasType + " " + Value + " " + DamageType + " damage to " + Source;
    }
}

public class CooldownReductionStat : Stat
{
    public CooldownReductionStat(double value = 0, 
        SelfType body = SelfType.Self,
        SourceType source = SourceType.Any) : base(value, body, source, IncreasType.Increase, true)
    { }
    
    public override double GetSum(List<Stat> stats)
    {
        var l = stats.Where(x => x is CooldownReductionStat).Cast<CooldownReductionStat>()
            .Where(x => (x.Source == Source || x.Source == SourceType.Any) && 
                        x.Self == Self && 
                        x.IncreasType == IncreasType);

        return l.Sum(x => x.Value);
    }
}

public class CastSpeedStat : Stat
{
    public CastSpeedStat(double value = 0, 
        SelfType body = SelfType.Self,
        SourceType source = SourceType.Any) : base(value, body, source, IncreasType.Increase, true)
    { }
    
    public override double GetSum(List<Stat> stats)
    {
        var l = stats.Where(x => x is CastSpeedStat).Cast<CastSpeedStat>()
            .Where(x => (x.Source == Source || x.Source == SourceType.Any) && 
                        x.Self == Self && 
                        x.IncreasType == IncreasType);

        return l.Sum(x => x.Value);
    }
}

public abstract class Stat
{
    public SelfType Self { get; }
    public SourceType Source { get; }
    public IncreasType IncreasType { get; }
    public double Value { get; }

    public Stat(double value = 0, SelfType body = SelfType.Self,
        SourceType source = SourceType.Any,
        IncreasType increas = IncreasType.Increase, 
        bool clampValue = false)
    {
        Self = body;
        Source = source;
        IncreasType = increas;
        if (clampValue) value = Math.Clamp(value, 0, 100);
        Value = value;
    }

    public virtual double GetSum(List<Stat> stats)
    {
        double result = 0;
        foreach (var stat in stats)
        {
            result += stat.Value;
        }

        return result;
    }
}