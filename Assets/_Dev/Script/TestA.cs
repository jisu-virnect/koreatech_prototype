using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestA : MonoBehaviour
{
    MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        materials = meshRenderer.materials;
    }
    Material[] materials;

    void SetMaterialTransparent(Material standardShaderMaterial)
    {
        // URP/Lit ���̴� ���
        standardShaderMaterial.SetFloat("_Surface", 1); // 0 = Opaque, 1 = Transparent
        //mat.SetInt("_Blend", (int)UnityEngine.Rendering.BlendMode.One);
        standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        standardShaderMaterial.SetInt("_ZWrite", 0);
        standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
        standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
        standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        standardShaderMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        // Alpha �� ���� (���� ����)
        Color color = standardShaderMaterial.color;
        color.a = 0f; // ���ϴ� ���� �� (0.0f���� 1.0f ����)
        standardShaderMaterial.color = color;

        // ���� Ŭ���� ��Ȱ��ȭ
        standardShaderMaterial.SetFloat("_Cutoff", 0.0f);

        // URP Ű���� ����
        standardShaderMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        standardShaderMaterial.DisableKeyword("_SURFACE_TYPE_OPAQUE");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = Color.white;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = Color.red;

            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(Co_Translate(materials, BlendMode.Opaque));

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(Co_Translate(materials, BlendMode.Transparent));
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetMaterialTransparent(meshRenderer.material);
        }
    }

    Coroutine coroutine = null;
    IEnumerator Co_Translate(Material[] materials, BlendMode Transparent)
    {
        switch (Transparent)
        {
            case BlendMode.Opaque:
                for (int i = 0; i < materials.Length; i++)
                {
                    Util.ChangeRenderMode(materials[i], BlendMode.Opaque);
                    materials[i].color = Color.white;
                }
                break;
            case BlendMode.Cutout:
                break;
            case BlendMode.Fade:
                break;
            case BlendMode.Transparent:
                //for (int i = 0; i < materials.Length; i++)
                //{
                //    Util.changeRenderMode(materials[i], Util.BlendMode.Transparent);
                //    materials[i].color = Color.white-Color.black;
                //}
                //break;
                for (int i = 0; i < materials.Length; i++)
                {
                    Util.ChangeRenderMode(materials[i], BlendMode.Transparent);
                }
                float durTime = 1f;
                float curTime = 0f;
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


}
