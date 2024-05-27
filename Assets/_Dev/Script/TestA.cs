using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestA : MonoBehaviour
{
    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        materials = meshRenderer.materials;
    }
    Material[] materials;

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
            coroutine = StartCoroutine(Co_Translate(materials, Util.BlendMode.Opaque));

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(Co_Translate(materials, Util.BlendMode.Transparent));
        }
    }

    Coroutine coroutine = null;
    IEnumerator Co_Translate(Material[] materials, Util.BlendMode Transparent)
    {
        switch (Transparent)
        {
            case Util.BlendMode.Opaque:
                for (int i = 0; i < materials.Length; i++)
                {
                    Util.changeRenderMode(materials[i], Util.BlendMode.Opaque);
                    materials[i].color = Color.white;
                }
                break;
            case Util.BlendMode.Cutout:
                break;
            case Util.BlendMode.Fade:
                break;
            case Util.BlendMode.Transparent:
                for (int i = 0; i < materials.Length; i++)
                {
                    Util.changeRenderMode(materials[i], Util.BlendMode.Transparent);
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
                            material.color = Color.Lerp(Color.white, Color.white - Color.black, curTime);
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
                            material.color = Color.Lerp(Color.white - Color.black, Color.white, curTime);
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
