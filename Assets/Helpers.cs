using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class Helpers
{
    public static IEnumerator WrapWithCallback(this MonoBehaviour self, IEnumerator coroutine, Action callback)
    {
        yield return self.StartCoroutine(coroutine);
        callback.Invoke();
    }

    public static IEnumerator ParallelCoroutines(this MonoBehaviour self, IEnumerable<IEnumerator> coroutines)
    {
        var count = 0;
        Action callback = () => count--;

        foreach (var coroutine in coroutines)
        {
            count++;
            self.StartCoroutine( self.WrapWithCallback( coroutine, callback ) );
        }
        while (count > 0)
        {
            yield return null;
        }
    }

    public static IEnumerator SerialCoroutines(this MonoBehaviour self, IEnumerable<IEnumerator> coroutines)
    {
        foreach (var coroutine in coroutines)
        {
            yield return self.StartCoroutine(coroutine);
        }
    }

    public static T RandomElement<T>(this T[] self)
    {
        return self[Random.Range(0, self.Length)];
    }

    public static T RandomElement<T>( this IList<T> self )
    {
        return self[Random.Range( 0, self.Count )];
    }

    public static T PopRandomElement<T>( this IList<T> self )
    {
        var i = Random.Range(0, self.Count);
        var elem = self[i];
        self.RemoveAt(i);
        return elem;
    }
}
