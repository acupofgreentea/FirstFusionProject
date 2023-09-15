using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Extensions 
{
    public static Vector3 ToVector3(this Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    public static Vector2 ToVector2(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static void DelayOneFrame(this MonoBehaviour monoBehaviour, UnityAction action)
    {
        monoBehaviour.StartCoroutine(Delay());

        IEnumerator Delay()
        {
            yield return null;
            
            action?.Invoke();
        }
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static List<T> Randomize<T>(this List<T> list)
    {
        System.Random random = new System.Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        return list;
    }
}
