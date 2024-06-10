using SpatialSys.UnitySDK;
using UnityEngine;

public class go_Section : MonoBehaviour
{
    private MeshRenderer mr_Wireframe;
    private MeshRenderer[] mr_Zones;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = gameObject.Search<CanvasGroup>("Canvas_Title");
        canvasGroup.alpha = 1f;
    }

    private void Start()
    {
        InitTriggerEvent();
        //SetAnimation(false);
        SetTriggerZone(0f, 1f);
        SetTriggerWireframe(new Color(0.3f, 1f, 0.3f, 0.5f), new Color(0.3f, 1f, 0.3f, 0f));
        SetTriggerCanvasGroup(0f, 1f);


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
        SetTriggerCanvasGroup(1f, 0f);
        SetTriggerZone(1f, 0f);
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs_Space>(name);
//#if UNITY_EDITOR
        Space_3.instance.Trigger_Zone((int)remoteEventSubIDs);
//#else
//        SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.SpaceState, new object[] { remoteEventSubIDs.ToString() });
//#endif
    }

    private void OnTriggerExit_Spatial(string name)
    {
        SetTriggerWireframe(new Color(0.3f, 1f, 0.3f, 0.5f), new Color(0.3f, 1f, 0.3f, 0f));
        SetTriggerZone(0f, 1f);
        SetTriggerCanvasGroup(0f, 1f);
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs_Space>(name);
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs_Space.install:
            case RemoteEventSubIDs_Space.before:
            case RemoteEventSubIDs_Space.after:
//#if UNITY_EDITOR
                Space_3.instance.Trigger_World();
//#else
//                SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.SpaceState, new object[] { RemoteEventSubIDs_Space.world.ToString() });
//#endif
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
            Util.ShaderFade_Float(go_Zone, "_Alpha", st, en);
        }
    }

    /// <summary>
    /// 트리거이벤트 와이어프레임 애니메이션
    /// </summary>
    /// <param name="st"></param>
    /// <param name="en"></param>
    private void SetTriggerWireframe(Color st, Color en)
    {
        return;
        Util.ShaderFade_Color(mr_Wireframe, "_TintColor", st, en);
    }
    private void SetTriggerCanvasGroup(float st, float en)
    {
        Util.CanvasGroupAlpha(canvasGroup, st, en, 0.3f);
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
