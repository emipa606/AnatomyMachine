using Verse;

namespace LingMod;

public class KeyValuePairsA(BodyPartRecord Key, bool value)
{
    public BodyPartRecord Key { get; } = Key;

    public bool Value { get; private set; } = value;

    public void SetValue(bool value)
    {
        Value = value;
    }
}