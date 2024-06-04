using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.UI;

public class panel_Check_Environment : panel_Base
{
    public int remainCheckEnvironment { get; set; }
    private GameObject go_Check_Environment;
    private ScrollRect sview_;

    protected override void Awake()
    {
        base.Awake();
        go_Check_Environment = ResourceManager.instance.LoadData<GameObject>(nameof(go_Check_Environment));
        sview_ = gameObject.Search<ScrollRect>(nameof(sview_));
    }

    /// <summary>
    /// 기본 프리팹 데이터
    /// </summary>
    private void SetData()
    {
        Util.DestroyChildrenGameObject(sview_.content);
        List<CheckEnvironment> checkEnvironments = DBManager.instance.CheckEnvironments;
        for (int i = 0; i < checkEnvironments.Count; i++)
        {
            GameObject go = Instantiate(go_Check_Environment, sview_.content);
            go.GetComponent<go_Check_Environment>().SetData(checkEnvironments[i]);
        }
    }

    public override void Open(Action act = null)
    {
        base.Open();

        remainCheckEnvironment = 0;

        SetData();

        //캐릭터 이동, 카메라 제어
        Space_3.instance.Control_PlayerMovement(false);
        Space_3.instance.Control_VirtualCamera(eVirtualCameraState.vcam_before);

        //ui
        packet_mapdata_root packet_Mapdata_Root = new packet_mapdata_root();
        packet_Mapdata_Root.title = "비계작업안전 시공도서";
        packet_Mapdata_Root.packet_mapdatas = new packet_mapdata[] { new packet_mapdata("평면도", "plan1"), new packet_mapdata("입면도", "plan2") };
        UIManager.instance.OpenPanel<panel_PlanMap>(Define.before).SetData(packet_Mapdata_Root);

        SoundManager.instance.PlayVoice(eAudioClips.voice_1_1_toast_guide);
        UIManager.instance.ShowToast<toast_Basic>("화면 우측에서 작업현장 적합성을 확인합니다.")
            .SetData(new packet_toast_basic(eToastColor.blue, eToastIcon.toast_idle));

    }
    protected override IEnumerator Action_Opening()
    {
        yield return null;
        sview_.verticalScrollbar.value = 0;
    }

}
