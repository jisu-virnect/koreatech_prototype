using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;

public class Space_3 : MonoBehaviour
{
    public GameObject HMD;
    public TMP_Text tmp_LocalAvatarName;
    private void Update()
    {
        //LocalAvatarName();
        //LookAtCamera(HMD.transform);

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.All,true);
            SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.Chat, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.All, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpatialBridge.coreGUIService.SetCoreGUIOpen(SpatialCoreGUITypeFlags.All, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SpatialBridge.coreGUIService.SetCoreGUIOpen(SpatialCoreGUITypeFlags.All, false);
        }
    }

    private void LocalAvatarName()
    {
        var avator = SpatialBridge.actorService.localActor.avatar;
        var head = avator.GetAvatarBoneTransform(HumanBodyBones.Head);
        if (HMD != null && head != null)
        {
            HMD.transform.position = head.position + Vector3.up * 0.3f;
            if (tmp_LocalAvatarName != null)
            {
                tmp_LocalAvatarName.text = SpatialBridge.actorService.localActor.displayName;
            }
        }
    }

    private void LookAtCamera(Transform tr)
    {
        var camera = SpatialBridge.cameraService;
        Debug.Log(camera.position);
        tr.LookAt(2 * tr.position - camera.position);
    }
    eBuildScaffold eBuildScaffold;
    private void BuildScaffold()
    {
    }

}

public enum eBuildScaffold
{
    stump=0,
    piiler, //받침판
    stump_piller,
    catgut, //안전그물
    work_scaffolding,
    railing,
    wall_jont,
    ladder , //조인트
    bracing, //파이프
    net , //사다리
    //wall,
}