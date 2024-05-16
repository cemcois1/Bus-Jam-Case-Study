using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = System.Random;
using RandomProducer = UnityEngine.Random;

public static class StaticMethods
{
    #region Debug

    public static void DebugRed(this string message)
    {
        Debug.Log(message.Red());
    }

    public static void DebugGreen(this string message)
    {
        Debug.Log("<color=green>" + message + "</color>");
    }

    public static void DebugBlue(this string message)
    {
        Debug.Log("<color=blue>" + message + "</color>");
    }

    public static void DebugCyan(this string message)
    {
        Debug.Log("<color=cyan>" + message + "</color>");
    }

    public static void DebugYellow(this string message)
    {
        Debug.Log("<color=yellow>" + message + "</color>");
    }

    #endregion

    #region DebugColors

    public static string Red(this string message)
    {
        return "<color=red>" + message + "</color>";
    }

    public static string Green(this string message)
    {
        return "<color=green>" + message + "</color>";
    }

    public static string Blue(this string message)
    {
        return "<color=blue>" + message + "</color>";
    }

    public static string Cyan(this string message)
    {
        return "<color=cyan>" + message + "</color>";
    }

    public static string Yellow(this string message)
    {
        return "<color=yellow>" + message + "</color>";
    }

    #endregion

    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null)
        {
            throw new ArgumentNullException(nameof(enumerable));
        }

        // note: creating a Random instance each call may not be correct for you,
        // consider a thread-safe static instance
        var enumerableList = enumerable.ToList();
        if (enumerableList.Count > 0)
        {
            return enumerableList[RandomProducer.Range(0, enumerableList.Count)];
        }

        return default(T);
    }
    
    //liste içerisinden random birden fazla eleman döndürsün
    public static List<T> Random<T>(this List<T> enumerable, int count)
    {
        if (enumerable == null)
        {
            throw new ArgumentNullException(nameof(enumerable));
        }

        // Use a shared Random instance (consider making it static)
        Random random = new Random();

        var enumerableList = enumerable.ToList();
        if (enumerableList.Count > 0)
        {
            var randomList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                randomList.Add(enumerableList[random.Next(0, enumerableList.Count)]);
            }

            return randomList;
        }

        return default(List<T>);
    }
    
    public static T FindComponentInParents<T>(this Transform currentTransform) where T : Component
    {
        var component = currentTransform.GetComponentInChildren<T>(true);

        if (component != null)
        {
            return component;
        }

        if (currentTransform.parent == null)
        {
            // Üst ebeveyn yoksa veya bileşen bulunamazsa null döndür.
            var sameTypeList = GameObject.FindObjectsByType(typeof(T), FindObjectsSortMode.None).FirstOrDefault();
            
            return (T)sameTypeList;
        }

        // Üst ebeveyni kontrol etmek için işlemi tekrarla.
        return FindComponentInParents<T>(currentTransform.parent);
    }

    public static Component FindComponentInParents(this Transform currentTransform, Type type,
        string attributeContainInName="")
    {

        
        
        var component = currentTransform.GetComponentsInChildren(type, true)
            .FirstOrDefault(x=> x.gameObject.name.Contains(attributeContainInName));

        if (component != null)
        {
            Debug.Log(attributeContainInName);
            return component;
        }

        if (currentTransform.parent == null)
        {
            //findinScene ile ara bulursan atama yap
            var sameTypeList = GameObject.FindObjectsByType(type, FindObjectsSortMode.None)
                .FirstOrDefault(x => ((Component)x).gameObject.name.Contains(attributeContainInName));


            // Üst ebeveyn yoksa veya bileşen bulunamazsa null döndür.
            return (Component)sameTypeList;
            
            return null;
        }

        // Üst ebeveyni kontrol etmek için işlemi tekrarla.
        return FindComponentInParents(currentTransform.parent, type,attributeContainInName);
    }

    public static float Abs(this float Number)
    {
        if (Number < 0)
        {
            return -Number;
        }

        return Number;
    }

    public static void CloseAllChilds(this GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public static TweenerCore<float, float, FloatOptions> ClosePanel(this CanvasGroup canvasGroup,float duration = .35f)
    {
        canvasGroup.DOKill(true);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        return canvasGroup.DOFade(0, duration).OnComplete(() => canvasGroup.gameObject.SetActive(false));
    }

    public static TweenerCore<float, float, FloatOptions> OpenPanel(this CanvasGroup canvasGroup,float duration = .35f)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.DOKill(true);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        return canvasGroup.DOFade(1, duration);
    }


    public static IEnumerator MakeActionWithDelay(Action action, float delay)
    {
        float tmp = 0;
        yield return new WaitForSeconds(delay);
        /*
        while (tmp < delay)
        {
            tmp += Time.deltaTime;
            yield return null;
        }*/

        action?.Invoke();
        yield return null;
    }

    public static Vector3 NormalizeOnXZ(this Vector3 v1)
    {
        var v1xz = new Vector3(v1.x, 0, v1.z).normalized;
        return v1xz;
    }

    public static float DistanceOnXZ(this Vector3 v1, Vector3 v2)
    {
        var v1xz = new Vector3(v1.x, 0, v1.z);
        var v2xz = new Vector3(v2.x, 0, v2.z);
        return Vector3.Distance(v1xz, v2xz);
    }

    public static float MaggnatudeOnXZ(this Vector3 v1)
    {
        var v1xz = new Vector3(v1.x, 0, v1.z);

        return v1xz.magnitude;
    }

    public static Transform DisableRigidbody(this Transform rigidbodyTransform)
    {
        Rigidbody rigidBody = rigidbodyTransform.GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        rigidBody.detectCollisions = false;
        return rigidbodyTransform;
    }

    public static void ResetVelocity(this Rigidbody rigidbodyTransform)
    {
        rigidbodyTransform.velocity = Vector3.zero;
        rigidbodyTransform.angularVelocity = Vector3.zero;
    }

    public static Transform EnableRigidbody(this Transform rigidbodyTransform)
    {
        Rigidbody rigidBody = rigidbodyTransform.GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        rigidBody.detectCollisions = true;
        return rigidbodyTransform;
    }

    public static Coroutine MakeAction(this MonoBehaviour monoBehaviour, Action action, float delay)
    {
        monoBehaviour.StartCoroutine(MakeActionWithDelay(action, delay));
        return null;
    }

    public static void MakeShineEffect(this Material material, string ColorName = "_Emission",
        Action onShiningFinished = null, float duration = 1f)
    {
        material.EnableKeyword("_EMISSION");
        DOTween.To(() => material.GetColor(ColorName), value => material.SetColor(ColorName, value),
                Color.white, duration * 0.3f)
            .From(Color.black).OnComplete(() =>
            {
                DOTween.To(() => material.GetColor(ColorName), value => material.SetColor(ColorName, value),
                    Color.black, duration * .7f).OnComplete(() => { onShiningFinished?.Invoke(); });
            });
    }

    public static IEnumerator LerpPositionIEnumerator(Vector3 startpos, Vector3 endpos, float duration)
    {
        float t = 0;
        while (t <= 1)
        {
            startpos += Vector3.Lerp(startpos, endpos, t);
            t += Time.deltaTime / duration;
            yield return null;
        }

        startpos = endpos;
        yield return null;
    }

    public static Vector3 GetLerpedValue(float min, float max, float current, Vector3 startVector, Vector3 endVector)
    {
        var currentValue = Mathf.Clamp(current, min, max);

        var ilerlemeMiktarı = (currentValue - min) / Mathf.Abs((max - min));
        return Vector3.Lerp(startVector, endVector, ilerlemeMiktarı);
    }

    public static float GetLerpedValue(float min, float max, float current, float startValue, float endValue)
    {
        var currentValue = Mathf.Clamp(current, min, max);

        var ilerlemeMiktarı = (currentValue - min) / Mathf.Abs((max - min));
        return Mathf.Lerp(startValue, endValue, ilerlemeMiktarı);
    }
}