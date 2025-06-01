using System.Collections.Generic;

public static class GameData
{
    public static int levelsCompleted = 0;
    public static double mostDamage = -1;
    public static int money = 2147483600; //2147483647
    public static List<Item> cakes = new List<Item>();
    public enum Rarity
    {
        common,
        uncommon,
        rare
    }
}