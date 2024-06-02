using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

public class go_Section : MonoBehaviour
{
    private MeshRenderer mr_Wireframe;
    private MeshRenderer[] mr_Zones;

    private void Start()
    {
        InitTriggerEvent();
        //SetAnimation(false);
        SetTriggerZone(1f, 0f);
        SetTriggerWireframe(new Color(0.3f, 1f, 0.3f, 0.5f), new Color(0.3f, 1f, 0.3f, 0f));
    }
    /// <summary>
    /// 트리거이벤트 초기화
    /// </summary>
    private void InitTriggerEvent()
    {
        SpatialTriggerEvent triggerEvent = gameObject.Search<SpatialTriggerEvent>(nameof(triggerEvent));
        triggerEvent.onEnterEvent.unityEvent.AddListener(() => OnTriggerEnter_Spatial(name));
        triggerEvent.onExitEvent.unityEvent.AddListener(() => OnTriggerExit_Spatial(name));
        
        GameObject go_Zone = gameObject.SearchGameObject(nameof(go_Zone));
        mr_Zones = go_Zone.GetComponentsInChildren<MeshRenderer>();
        mr_Wireframe = gameObject.Search<MeshRenderer>(nameof(mr_Wireframe));
    }

    private void OnTriggerEnter_Spatial(string name)
    {
        SetTriggerWireframe(new Color(0.3f, 1f, 0.3f, 0f), new Color(0.3f, 1f, 0.3f, 0.5f));
        SetTriggerZone(0f, 1f);
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs_Space>(name);
#if UNITY_EDITOR
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs_Space.before:
               Space_3.instance.Trigger_BeforeZone();
                break;
            case RemoteEventSubIDs_Space.install:
                Space_3.instance.Trigger_InstallZone();
                break;
            case RemoteEventSubIDs_Space.after:
                Space_3.instance.Trigger_AfterZone();
                break;
            default:
                break;
        }
#else
        SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.SpaceState, new object[] { remoteEventSubIDs.ToString() });
#endif
    }

    private void OnTriggerExit_Spatial(string name)
    {
        SetTriggerWireframe(new Color(0.3f, 1f, 0.3f, 0.5f), new Color(0.3f, 1f, 0.3f, 0f));
        SetTriggerZone(1f, 0f);
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs_Space>(name);
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs_Space.install:
            case RemoteEventSubIDs_Space.before:
            case RemoteEventSubIDs_Space.after:
#if UNITY_EDITOR
                Space_3.instance.Trigger_World();
#else
                SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.SpaceState, new object[] { RemoteEventSubIDs.world.ToString() });
#endif
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 트리거이벤트 존 애니메이션
    /// </summary>
    /// <param name="st"></param>
    /// <param name="en"></param>
    private void SetTriggerZone(float st, float en)
    {
        for (int i = 0; i < mr_Zones.Length; i++)
        {
            MeshRenderer go_Zone = mr_Zones[i];
            Util.ShaderFade_Float(go_Zone, "_Line2InvertedThickness", st, en);
        }
    }

    /// <summary>
    /// 트리거이벤트 와이어프레임 애니메이션
    /// </summary>
    /// <param name="st"></param>
    /// <param name="en"></param>
    private void SetTriggerWireframe(Color st, Color en)
    {
        Util.ShaderFade_Color(mr_Wireframe, "_TintColor", st, en);
    }


    ///// <summary>
    ///// 폴리곤그리드 애니메이션
    ///// </summary>
    ///// <param name="active"></param>
    //private void SetAnimation(bool active)
    //{
    //    if (active)
    //    {
    //        animator.Play(Define.polygonGrid_Glow_Appear, -1, 0);
    //    }
    //    else
    //    {
    //        animator.Play(Define.polygonGrid_Glow_DisAppear, -1, 0);
    //    }
    //}

}
