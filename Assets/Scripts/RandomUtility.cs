using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Joshua 2023/12/10

[System.Serializable]
public class ObjectPool<T>
{
    public T obj;
    [Range(0, 100)]
    public int weight;   
}

public static class RandomUtility
{
    public static object ObjectPoolCalculator<T>(List<ObjectPool<T>> list)
    {
        int combinedWeight = 0;

        foreach (ObjectPool<T> pool in list)
        {
            combinedWeight += pool.weight;
        }

        var random = Random.Range(0, combinedWeight);

        foreach (ObjectPool<T> pool in list)
        {
            random -= pool.weight;

            if (random <= 0)
            {
                return pool.obj;
            }
        }

        return null;
    }

    public static bool RandomPercentage(int percent)
    {
        int random = Random.Range(0, 100);

        if(random < percent)
        {
            return true;
        }
        return false;
    }
}
