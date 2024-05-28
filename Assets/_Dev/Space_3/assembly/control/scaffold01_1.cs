using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using UnityEngine.Rendering;
public class scaffold01_1 : MonoBehaviour
{
    public Dictionary<eBuildScaffold, GameObject> objList { get; private set; } = new Dictionary<eBuildScaffold, GameObject>();
    public Dictionary<eBuildScaffold, Vector3> transformList { get; private set; } = new Dictionary<eBuildScaffold, Vector3>();

    private Coroutine coroutine_RenderMode;
    private Coroutine coroutine_Position;

    private void Awake()
    {
        string[] objNames =  Util.Enum2StringArray<eBuildScaffold>();
        for (int i = 0; i < objNames.Length; i++)
        {
            string objName = objNames[i];
            eBuildScaffold enumName = Util.String2Enum<eBuildScaffold>(objName);
            GameObject obj = gameObject.Search(objName).gameObject;

            objList.Add(enumName, obj);
            transformList.Add(enumName, obj.transform.position);
        }
        Action_ResetObjects();
    }

    /// <summary>
    /// 전체 오브젝트 비활성화
    /// </summary>
    public void Action_ResetObjects()
    {
        foreach (var item in objList.Values)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// 오브젝트 블링크
    /// </summary>
    /// <param name="eBuildScaffold"></param>
    /// <param name="blendMode"></param>
    /// <param name="sequence"></param>
    public void Action_scaffold_RenderMode(eBuildScaffold eBuildScaffold, BlendMode blendMode, Sequence sequence)
    {
        if (coroutine_RenderMode != null)
        {
            StopCoroutine(coroutine_RenderMode);
        }
        Material[] materials = objList[eBuildScaffold].GetComponentInChildren<MeshRenderer>().materials;
        coroutine_RenderMode = StartCoroutine(Co_Change_Material(materials, blendMode, sequence));
    }

    /// <summary>
    /// 오브젝트 플로팅
    /// </summary>
    /// <param name="eBuildScaffold"></param>
    /// <param name="blendMode"></param>
    /// <param name="sequence"></param>
    public void Action_scaffold_Position(eBuildScaffold eBuildScaffold, BlendMode blendMode, Sequence sequence)
    {
        if (coroutine_Position != null)
        {
            StopCoroutine(coroutine_Position);
        }
        coroutine_Position = StartCoroutine(Co_Change_Position(eBuildScaffold, blendMode, sequence));
    }

    public void Action_scaffold_Active(eBuildScaffold eBuildScaffold, bool active)
    {
        objList[eBuildScaffold].SetActive(active);
    }

    /// <summary>
    /// 오브젝트 블링크
    /// </summary>
    /// <param name="materials"></param>
    /// <param name="Transparent"></param>
    /// <param name="sequence"></param>
    /// <returns></returns>
    IEnumerator Co_Change_Material(Material[] materials, BlendMode Transparent, Sequence sequence)
    {
        float curTime;
        float durTime = 0.5f;

        switch (Transparent)
        {
            case BlendMode.Opaque:

                for (int i = 0; i < materials.Length; i++)
                {
                    Util.ChangeRenderMode(materials[i], Util.String2Enum<BlendMode>(sequence.renderMode));
                    materials[i].color = Color.white;
                }
                break;
            case BlendMode.Transparent:
                for (int i = 0; i < materials.Length; i++)
                {
                    Util.ChangeRenderMode(materials[i], BlendMode.Transparent);
                }
                while (true)
                {
                    curTime = 0f;
                    while (curTime < 1f)
                    {
                        curTime += Time.deltaTime / durTime;
                        for (int i = 0; i < materials.Length; i++)
                        {
                            Material material = materials[i];
                            material.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, curTime));
                        }
                        yield return null;
                    }
                    curTime = 0f;
                    while (curTime < 1f)
                    {
                        curTime += Time.deltaTime / durTime;
                        for (int i = 0; i < materials.Length; i++)
                        {
                            Material material = materials[i];
                            material.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, curTime));
                        }
                        yield return null;
                    }
                }
            default:
                break;
        }
        yield return null;
    }

    /// <summary>
    /// 오브젝트 플로팅
    /// </summary>
    /// <param name="eBuildScaffold"></param>
    /// <param name="Transparent"></param>
    /// <param name="sequence"></param>
    /// <returns></returns>
    IEnumerator Co_Change_Position(eBuildScaffold eBuildScaffold, BlendMode Transparent, Sequence sequence)
    {
        float curTime;
        float durTime = 0.5f;
        Vector3 upRange = Vector3.up * 0.2f;
        Vector3 oriPos = transformList[eBuildScaffold];
        Vector3 upPos = oriPos + upRange;

        switch (Transparent)
        {
            case BlendMode.Opaque:
                objList[eBuildScaffold].transform.position = transformList[eBuildScaffold];
                break;
            case BlendMode.Transparent:
                while (true)
                {
                    curTime = 0f;
                    while (curTime < 1f)
                    {
                        curTime += Time.deltaTime / durTime;
                        objList[eBuildScaffold].transform.position = Vector3.Lerp(oriPos, upPos, curTime);
                        yield return null;
                    }
                    curTime = 0f;
                    while (curTime < 1f)
                    {
                        curTime += Time.deltaTime / durTime;
                        objList[eBuildScaffold].transform.position = Vector3.Lerp(upPos, oriPos, curTime);
                        yield return null;
                    }
                }
            default:
                break;
        }
        yield return null;
    }
}
