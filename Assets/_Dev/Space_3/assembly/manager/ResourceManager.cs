using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    public Object[] resources;
    public Dictionary<string, object> resourcesDic = new Dictionary<string, object>();

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < resources.Length; i++)
        {
            resourcesDic.Add(resources[i].name, resources[i]);
        }
    }

    /// <summary>
    /// �Ϲ� ����Ƽ ������Ʈ �ε�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public T LoadData<T>(string resourceName) where T : Object
    {
        if (resourcesDic.ContainsKey(resourceName))
        {
            return resourcesDic[resourceName] as T;
        }
        else
        {
            Debug.Log("�����Ͱ� �����ϴ�.");
            return null;
        }
    }

    /// <summary>
    /// ��������Ʈ �ε�
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public Sprite LoadDataSprite(string resourceName)
    {
        if (resourcesDic.ContainsKey(resourceName))
        {
            Texture2D texture = LoadData<Texture2D>(resourceName);
            Sprite sprite = Util.Tex2Sprite(texture);
            return sprite;
        }
        else
        {
            Debug.Log("�����Ͱ� �����ϴ�.");
            return null;
        }
    }
}
