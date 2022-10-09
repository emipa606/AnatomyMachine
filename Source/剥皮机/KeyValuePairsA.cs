using Verse;

namespace LingMod;

public class KeyValuePairsA
{
    public KeyValuePairsA(BodyPartRecord Key, bool value)
    {
        this.Key = Key;
        Value = value;
    }

    public BodyPartRecord Key { get; }

    public bool Value { get; private set; }

    public void SetValue(bool value)
    {
        Value = value;
    }
}