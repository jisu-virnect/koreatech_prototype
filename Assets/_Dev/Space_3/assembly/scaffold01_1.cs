using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
public class scaffold01_1 : MonoBehaviour
{
    public Dictionary<eBuildScaffold, GameObject> objList { get; private set; } = new Dictionary<eBuildScaffold, GameObject>();
    private void Awake()
    {
        string[] objNames =  Util.Enum2StringArray<eBuildScaffold>();
        for (int i = 0; i < objNames.Length; i++)
        {
            string objName = objNames[i];
            objList.Add(Util.String2Enum<eBuildScaffold>(objName), gameObject.Search(objName).gameObject);
        }
        Action_ResetObjs();
    }

    public void Action_ResetObjs()
    {
        foreach (var item in objList.Values)
        {
            item.SetActive(false);
        }
    }

    public void Action_scaffold01_1(eBuildScaffold eBuildScaffold)
    {
        objList[eBuildScaffold].SetActive(true);
    }
}
