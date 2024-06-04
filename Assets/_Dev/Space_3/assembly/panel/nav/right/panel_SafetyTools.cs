using Cinemachine;
using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class panel_SafetyTools : panel_Base
{
    private TMP_Text tmp_Index;
    private TMP_Text tmp_Title;
    private TMP_Text tmp_Content;

    private Image img_Content;

    private Button btn_Confirm;

    private List<SafetyTools> safetyToolies;
    private int index;

    protected override void Awake()
    {
        base.Awake();
        tmp_Index = gameObject.Search<TMP_Text>(nameof(tmp_Index));
        tmp_Title = gameObject.Search<TMP_Text>(nameof(tmp_Title));
        tmp_Content = gameObject.Search<TMP_Text>(nameof(tmp_Content));

        img_Content = gameObject.Search<Image>(nameof(img_Content));

        btn_Confirm = gameObject.Search<Button>(nameof(btn_Confirm));
        btn_Confirm.onClick.AddListener(OnClick_Confirm);

        safetyToolies = DBManager.instance.SafetyToolies;
    }


    private void OnClick_Confirm()
    {
        SoundManager.instance.PlayEffect(eAudioClips.effect_click);

        if (index == safetyToolies.Count-1)
        {
            DestroyTargetOutlines();
            Close();

            SoundManager.instance.PlayVoice(eAudioClips.voice_1_2_modal_end);

            popup_Success popup_Success = UIManager.instance.OpenPopup<popup_Success>();
            popup_Success.SetData(new packet_popup_Basic("완료", "[작업 안전]을 완료하였습니다.\n원하는 구역으로 이동하여 체험을 다시 진행할 수 있습니다."));
            popup_Success.SetAction(() =>
            {
                Space_3.instance.Control_VirtualCamera();
                Space_3.instance.Control_PlayerMovement(true);
                UIManager.instance.ClosePanel<panel_PlanMap>();
                UIManager.instance.GetPanel<panel_TopNavigation>().ResetStep();
                UIManager.instance.OpenPanel<panel_TriggerMenu>();
            });
        }
        else
        {
            NextStep();
        }
    }

    Dictionary<HumanBodyBones, GameObject> targetOutlines = new Dictionary<HumanBodyBones, GameObject>();

    public void ResetStep()
    {
        Space_3.instance.Control_VirtualCamera(eVirtualCameraState.none);
        Space_3.instance.Control_PlayerMovement(false);
        DestroyTargetOutlines();
        SetTargetOutlines();
        
        index = -1;
        prevSafetyTools = null;
        NextStep();

    }

    private void DestroyTargetOutlines()
    {
        var v = targetOutlines.Values;
        foreach (var item in v)
        {
            Destroy(item);
        }
        targetOutlines.Clear();
    }

    float far = 0.05f;
    float zoom = 0.4f;

    private void SetTargetOutlines()
    {
        for (int i = 0; i < safetyToolies.Count; i++)
        {
            SafetyTools safetyTools = safetyToolies[i];
            HumanBodyBones HumanBodyBones = (HumanBodyBones)safetyTools.targetTransform;

            if (!targetOutlines.ContainsKey(HumanBodyBones))
            {
                Transform targetTransform = SpatialBridge.actorService.localActor.avatar.GetAvatarBoneTransform(HumanBodyBones);

                if (targetTransform != null)
                {
                    GameObject go_Outline = Instantiate(ResourceManager.instance.LoadData<GameObject>(nameof(go_Outline)));

                    Util.lossyscale(go_Outline.transform, targetTransform, far);

                    if (!targetOutlines.ContainsKey(HumanBodyBones))
                    {
                        targetOutlines.Add(HumanBodyBones, go_Outline);
                    }
                }
            }
        }
    }

    private SafetyTools prevSafetyTools = null;

    private void NextStep()
    {

        HumanBodyBones HumanBodyBones;
        //기존 아웃라인포인트 릴리즈
        if (prevSafetyTools != null)
        {
            HumanBodyBones = (HumanBodyBones)prevSafetyTools.targetTransform;
            if (targetOutlines.ContainsKey(HumanBodyBones))
            {
                GameObject prevTargetOutline = targetOutlines[HumanBodyBones];
                Util.lossyscale(prevTargetOutline.transform, prevTargetOutline.transform.parent, far);
            }
        }
        index++;
        prevSafetyTools = safetyToolies[index];
        SoundManager.instance.PlayVoice(Util.String2Enum<eAudioClips>("voice_1_2_modal_" + (prevSafetyTools.index+1).ToString()));

        //아웃라인포인트 잡아주기
        HumanBodyBones = (HumanBodyBones)prevSafetyTools.targetTransform;
        if (targetOutlines.ContainsKey(HumanBodyBones))
        {
            GameObject targetOutline = targetOutlines[HumanBodyBones];
            Util.lossyscale(targetOutline.transform, targetOutline.transform.parent, zoom);

            //카메라 포커싱
            //SpatialBridge.cameraService.SetTargetOverride(targetOutline.transform, SpatialCameraMode.Actor);
            //Space_3.instance.Control_PlayerMovement(false);
            Outline(targetOutline.transform, 0.2f, zoom);
            CinemachineVirtualCamera virtualCamera = Space_3.instance.Control_VirtualCamera(Util.String2Enum<eVirtualCameraState>("vcam_" + prevSafetyTools.title));
            StartCoroutine(SetVirtualCamera(virtualCamera, targetOutline.transform.parent));
        }

        tmp_Index.text = $"{prevSafetyTools.index + 1} / {safetyToolies.Count}";
        tmp_Title.text = prevSafetyTools.title;

        img_Content.sprite = ResourceManager.instance.LoadDataSprite(prevSafetyTools.content_image);

        tmp_Content.text = prevSafetyTools.content;
    }

    private IEnumerator SetVirtualCamera(CinemachineVirtualCamera virtualCamera, Transform target)
    {                    
        virtualCamera.gameObject.transform.position = target.position + Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized * 1f + Vector3.up * 0.5f;
        
        while (virtualCamera.enabled)
        {
            virtualCamera.transform.LookAt(target);
            yield return null;
        }
    }

    Coroutine co_Outline = null;
    void Outline(Transform outline, float st, float en)
    {
        if (co_Outline != null)
        {
            StopCoroutine(co_Outline);
        }
        co_Outline = StartCoroutine(Co_Outline(outline,st,en));
    }

    private IEnumerator Co_Outline(Transform outline, float st, float en)
    {
        float curTime;
        float durTime;
        while (true)
        {
            curTime = 0f;
            durTime = 0.2f;
            while (curTime < 1f)
            {
                Util.lossyscale(outline, outline.parent, Mathf.Lerp(st, en, EasingFunction.EaseOutCirc(0f, 1f, curTime += Time.deltaTime / durTime)));
                yield return null;
            }
            curTime = 0f;
            durTime = 0.4f;
            while (curTime < 1f)
            {
                Util.lossyscale(outline, outline.parent, Mathf.Lerp(en, st, EasingFunction.EaseInCirc(0f,1f, curTime += Time.deltaTime / durTime)));
                yield return null;
            }
            yield return null;
        }
    }
}
