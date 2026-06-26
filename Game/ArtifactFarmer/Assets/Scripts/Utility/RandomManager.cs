using System;

public static class RandomManager 
{
    private static System.Random random;
    private static int currentSeed;

    public static void Init(int? seed = null)
    {
        currentSeed = seed ?? Environment.TickCount;
        random = new System.Random(currentSeed);
    }

    public static int CurrentSeed => currentSeed;

    public static bool RollChance(float chance)
    {
        return random.NextDouble() * 100 < chance;
    }

    public static int RangeInt(int min, int max)
    {
        return random.Next(min, max);
    }

    public static float RangeFloat(float min, float max)
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }

}
