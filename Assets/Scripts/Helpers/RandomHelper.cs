﻿using System.Collections;
using System.Collections.Generic;

public class RandomHelper
{
    static RandomHelper instance;
    System.Random rng = new System.Random(0);

    public static RandomHelper Instance()
    {
        if (instance == null)
            instance = new RandomHelper();

        return instance;
    }

    public int GetInt(int a, int b)
    {
        return rng.Next(a, b);
    }

    public T GetItem<T>(List<T> items)
    {
        var size = items.Count;
        var ind = GetInt(0, size);
        return items[ind];
    }

    public T GetItem<T>(T[] items)
    {
        var size = items.Length;
        var ind = GetInt(0, size);
        return items[ind];
    }

    public bool GetBool(float p = 0.5f)
    {
        return rng.NextDouble() < p;
    }
}
