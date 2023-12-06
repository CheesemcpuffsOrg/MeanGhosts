using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        List<int> weight = new List<int>();
        List<ObjectPool<T>> objectPools = new List<ObjectPool<T>>();

        foreach (ObjectPool<T> item in list)
        {
            weight.Add(item.weight);
        }

        int maxWeight = weight.Max();

        int randomWeight = Random.Range(0, maxWeight);

        foreach (ObjectPool<T> item in list)
        {
            if (item.weight >= randomWeight) 
            { 
                objectPools.Add(item);
            }
        }

        int randomObjectPool = Random.Range(0, objectPools.Count);

        ObjectPool<T> result = objectPools[randomObjectPool];

        return result.obj;
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
