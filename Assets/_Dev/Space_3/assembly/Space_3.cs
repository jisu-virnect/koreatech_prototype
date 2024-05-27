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
        switch (eBuildScaffold)
        {
            case eBuildScaffold.piiler:
                break;
            case eBuildScaffold.bracing:
                break;
            case eBuildScaffold.ladder:
                break;
            case eBuildScaffold.net:
                break;
            case eBuildScaffold.band:
                break;
            case eBuildScaffold.catgut:
                break;
            case eBuildScaffold.ladder2:
                break;
            case eBuildScaffold.stump:
                break;
            case eBuildScaffold.wall_jont:
                break;
            case eBuildScaffold.work_scaffolding:
                break;
            case eBuildScaffold.stump_piller:
                break;
            case eBuildScaffold.railing:
                break;
            default:
                break;
        }
    }

}

public enum eBuildScaffold
{
    stump=0,
    piiler, //받침판
    bracing, //파이프
    ladder , //조인트
    net , //사다리
    band , //작업대
    catgut, //안전그물
    ladder2,
    wall_jont,
    //wall,
    work_scaffolding,
    stump_piller,
    railing,
}