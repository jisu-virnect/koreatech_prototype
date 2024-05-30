using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpatialSys.UnitySDK;
using UnityEngine.UI;
using System.Threading.Tasks;

public static partial class Util
{
    public static Sprite Tex2Sprite(Texture2D _tex)
    {
        return Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), new Vector2(0.5f, 0.5f));
    }

    // JSON 배열을 파싱하는 헬퍼 메서드
    public static T[] FromJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    public static List<T> FromJsonList<T>(string json)
    {
        return new List<T>(FromJsonArray<T>(json));
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

    /// <summary>
    /// 딕셔너리의 벨류의 리스트에 데이터 추가
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
    public static void DicQueue<T1, T2>(ref Dictionary<T1, Queue<T2>> dic, T1 key, T2 data)
    {
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, new Queue<T2>());
        }
        dic[key].Enqueue(data);
    }



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
    public static Transform Search(this GameObject _obj, string _name)
    {
        return Search(_obj.transform, _name);
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




    #region enum 관련

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
                standardShaderMaterial.renderQueue = 3000;
                standardShaderMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                standardShaderMaterial.DisableKeyword("_SURFACE_TYPE_OPAQUE");
                break;
        }
    }
}

