using System.Collections.Generic;

public class ResistanceProfile
{
    private readonly Dictionary<AttackType, float> _resistances;

    public ResistanceProfile(Dictionary<AttackType, float> resistances)
    {
        _resistances = resistances;
    }

    public float GetResistance(AttackType type)
    {
        return _resistances.TryGetValue(type, out var resistance) ? resistance : 0f;
    }
}
