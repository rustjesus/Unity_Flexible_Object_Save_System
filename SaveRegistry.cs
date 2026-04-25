using System.Collections.Generic;

public static class SaveRegistry
{
    private static readonly HashSet<Saveable> saveables = new HashSet<Saveable>();

    public static void Register(Saveable s)
    {
        if (s != null)
            saveables.Add(s);
    }

    public static void Unregister(Saveable s)
    {
        if (s != null)
            saveables.Remove(s);
    }

    public static IEnumerable<Saveable> GetAll()
    {
        return saveables;
    }

    public static void Clear()
    {
        saveables.Clear();
    }
}