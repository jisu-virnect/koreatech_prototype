using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public static partial class Util
{
    #region Json - json기본기능

    /// <summary>
    /// JSON 배열을 파싱하는 헬퍼 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T[] FromJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    /// <summary>
    /// JSON 배열을 파싱하는 헬퍼 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static List<T> FromJsonList<T>(string json)
    {
        return new List<T>(FromJsonArray<T>(json));
    }

    #endregion



    #region Dictionary - 딕셔너리 하위 STL 추가,제거 기능

    /// <summary>
    /// 딕셔너리의 값인 리스트에 데이터 추가
    /// </summary>
    /// <typeparam name="T1">타입1</typeparam>
    /// <typeparam name="T2">타입2</typeparam>
    /// <param name="dic">딕셔너리</param>
    /// <param name="data">추가할 데이터</param>
    /// <param name="key">키값</param>
    public static void DicList<T1, T2>(ref Dictionary<T1, List<T2>> dic, T1 key, T2 data)
    {
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, new List<T2>());
        }
        dic[key].Add(data);
    }

    /// <summary>
    /// 딕셔너리 값인 큐에 데이터 추가
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="dic"></param>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public static void DicQueue<T1, T2>(ref Dictionary<T1, Queue<T2>> dic, T1 key, T2 data)
    {
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, new Queue<T2>());
        }
        dic[key].Enqueue(data);
    }

    #endregion



    #region Search - 이름으로 GameObject or Transform 하위의 GameObject or Component 찾기

    /// <summary>
    /// 해당 오브젝트 하위에서 특정 이름의 컴포넌트 서치
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_gameObject"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static T Search<T>(this GameObject _gameObject, string _name) where T : UnityEngine.Object
    {
        return _gameObject.transform.Search<T>(_name);
    }

    /// <summary>
    /// 해당 오브젝트 하위에서 특정 이름의 컴포넌트 서치
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_obj"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static T Search<T>(this Transform _transform, string _name) where T : UnityEngine.Object
    {
        var go = Search(_transform, _name);

        if (go == null)
        {
            return null;
        }

        if (typeof(T) == typeof(GameObject))
        {
            return go as T;
        }
        else
        {
            return go.GetComponent<T>();
        }
    }

    /// <summary>
    /// 해당 오브젝트 하위에서 특정 이름의 트랜스폼 서치
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static GameObject SearchGameObject(this GameObject _obj, string _name)
    {
        Transform trnasform = Search(_obj.transform, _name);
        return trnasform != null ? trnasform.gameObject : null;

    }

    /// <summary>
    /// 자식 트랜스폼 찾기
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static Transform Search(this Transform _target, string _name)
    {
        if (_target.name == _name) return _target;

        for (int i = 0; i < _target.childCount; ++i)
        {
            var result = Search(_target.GetChild(i), _name);

            if (result != null) return result;
        }

        return null;
    }

    /// <summary>
    /// 부모 트랜스폼 찾기
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static Transform SearchParent(this Transform _target, string _name)
    {
        if (_target.name == _name) return _target;

        if (_target.parent == null) return null;

        if (_target.parent.name == _name)
        {
            return _target.parent;
        }
        else
        {
            return SearchParent(_target.parent, _name);
        }
    }

    #endregion



    #region Enum - 이넘 의 길이, string to enum 등의 기능

    /// <summary>
    /// enum을 stringArray로 변환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string[] Enum2StringArray<T>()
    {
        return Enum.GetNames(typeof(T));
    }

    /// <summary>
    /// enum원형의 길이 구하기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int EnumLength<T>()
    {
        return Enum.GetNames(typeof(T)).Length;
    }

    /// <summary>
    /// string을 enum으로 변환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static T String2Enum<T>(string _str)
    {
        try { return (T)Enum.Parse(typeof(T), _str); }
        catch { return (T)Enum.Parse(typeof(T), "none"); }
    }

    /// <summary>
    /// enum을 string으로 변환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_enum"></param>
    /// <returns></returns>
    public static string Enum2String<T>(T _enum) where T : Enum
    {
        try { return Enum.GetName(typeof(T), _enum); }
        catch { return String.Empty; }
    }

    /// <summary>
    /// enum의 숫자를 string으로 변환
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string EnumInt2String(object obj)
    {
        return ((int)obj).ToString();
    }

    #endregion



    #region Etc - 분류하기엔 크기가 작은 기능들

    /// <summary>
    /// 고정스케일로 변경
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="parent"></param>
    /// <param name="scale"></param>
    public static void lossyscale(Transform tr, Transform parent, float scale)
    {

        tr.SetParent(null);
        tr.localScale = Vector3.one * scale;
        tr.SetParent(parent);
        tr.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 텍스쳐를 스프라이트로 변경
    /// </summary>
    /// <param name="_tex"></param>
    /// <returns></returns>
    public static Sprite Tex2Sprite(Texture2D _tex)
    {
        return Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), new Vector2(0.5f, 0.5f));
    }

    /// <summary>
    /// 컨텐츠사이즈피터 버그가 있어 레이아웃 갱신하는 함수
    /// </summary>
    /// <param name="go"></param>
    /// <param name="objName"></param>
    public static void RefreshLayout(GameObject go, string objName)
    {
        RectTransform rectTr = Search<RectTransform>(go, objName);
        if (!rectTr)
        {
            Debug.Log("리프레쉬할게없음");
            return;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTr);
    }

    /// <summary>
    /// 렌더모드 변경(Opaque,Transparent)
    /// </summary>
    /// <param name="standardShaderMaterial"></param>
    /// <param name="blendMode"></param>
    public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                standardShaderMaterial.SetFloat("_Surface", 0); // 0 = Opaque, 1 = Transparent
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = -1;
                // URP 키워드 설정
                standardShaderMaterial.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                standardShaderMaterial.EnableKeyword("_SURFACE_TYPE_OPAQUE");
                break;
            case BlendMode.Cutout:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                standardShaderMaterial.SetFloat("_Surface", 1); // 0 = Opaque, 1 = Transparent
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3001;
                standardShaderMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                standardShaderMaterial.DisableKeyword("_SURFACE_TYPE_OPAQUE");
                break;
        }
    }

    /// <summary>
    /// GetComponentsInChildren 강화버전 (비활성화 상태도 호출가능)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_target"></param>
    /// <returns></returns>
    public static List<T> GetComponentsInChildrenPlus<T>(this Transform _target) where T : Component
    {
        List<T> list = new List<T>();
        GetComponentsInChildrenPlus(_target, ref list);
        return list;
    }
    private static void GetComponentsInChildrenPlus<T>(this Transform _target, ref List<T> list) where T : Component
    {
        for (int i = 0; i < _target.childCount; ++i)
        {
            Transform childTr = _target.GetChild(i);
            if (childTr.TryGetComponent(out T t))
            {
                list.Add(t);
            }
            GetComponentsInChildrenPlus(childTr, ref list);
        }
    }


    #endregion


    #region renderer,material,shader
    //독립적으로 코루틴 돌아가게 하기 위한 dictionary
    private static Dictionary<int , Coroutine> coroutines = new Dictionary<int, Coroutine>();

    /// <summary>
    /// shader의 특정 float 값 fade 처리
    /// </summary>
    /// <param name="meshRenderer"></param>
    /// <param name="name"></param>
    /// <param name="st"></param>
    /// <param name="en"></param>
    public static void ShaderFade_Float(MeshRenderer meshRenderer, string name, float st, float en, float duration = 0.5f)
    {
        int id = meshRenderer.GetInstanceID();
        Coroutine currentCoroutine = null;

        if (coroutines.ContainsKey(id))
        {
            currentCoroutine = coroutines[id];
        }

        if (currentCoroutine != null)
        {
            Space_3.instance.StopCoroutine(currentCoroutine);
            coroutines.Remove(id);
        }

        currentCoroutine = Space_3.instance.StartCoroutine(Co_ShaderFade_Float(meshRenderer, name, st, en, duration));
        coroutines.Add(id, currentCoroutine);
    }

    /// shader의 특정 color 값 fade 처리
    public static void ShaderFade_Color(MeshRenderer meshRenderer, string name, Color st, Color en, float duration = 0.5f)
    {
        int id = meshRenderer.GetInstanceID();
        Coroutine currentCoroutine = null;

        if (coroutines.ContainsKey(id))
        {
            currentCoroutine = coroutines[id];
        }

        if (currentCoroutine != null)
        {
            Space_3.instance.StopCoroutine(currentCoroutine);
            coroutines.Remove(id);
        }

        currentCoroutine = Space_3.instance.StartCoroutine(Co_ShaderFade_Color(meshRenderer, name, st, en, duration));
        coroutines.Add(id, currentCoroutine);
    }

    private static IEnumerator Co_ShaderFade_Float(MeshRenderer meshRenderer, string name, float st, float en, float duration)
    {
        float curTime = 0;
        while (curTime < 1f)
        {
            meshRenderer.material.SetFloat(name, Mathf.Lerp(st, en, curTime += Time.deltaTime / duration));
            yield return null;
        }
    }
    private static IEnumerator Co_ShaderFade_Color(MeshRenderer meshRenderer, string name, Color st, Color en, float duration)
    {
        float curTime = 0;
        while (curTime < 1f)
        {
            meshRenderer.material.SetColor(name, Color.Lerp(st, en, curTime += Time.deltaTime / duration));
            yield return null;
        }
    }
    #endregion
}