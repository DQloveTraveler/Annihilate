
public class SelectingRideable
{
    public enum RideableCharacter
    {
        Null = 0, Dragon = 1, Griffon = 2, FatIceDragon = 3, Golem = 4
    }

    public static RideableCharacter Value { get; private set; }

    public static RideableCharacter NewValue { get; private set; }

    public static void TrySet(RideableCharacter RC)
    {
        NewValue = RC;
    }

    public static void Set(RideableCharacter RC)
    {
        Value = RC;
    }

    public static bool IsDragon => Value == RideableCharacter.Dragon;

    public static bool IsGriffon => Value == RideableCharacter.Griffon;

    public static bool IsFatIceDragon => Value == RideableCharacter.FatIceDragon;

    public static bool IsGolem => Value == RideableCharacter.Golem;

}
