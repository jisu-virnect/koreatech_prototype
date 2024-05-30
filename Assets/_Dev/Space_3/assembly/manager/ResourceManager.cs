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

    public T LoadData<T>(string resourceName) where T : Object
    {
        if (resourcesDic.ContainsKey(resourceName))
        {
            return resourcesDic[resourceName] as T;
        }
        else
        {
            Debug.Log("데이터가 없습니다.");
            return null;
        }
        
    }

}
