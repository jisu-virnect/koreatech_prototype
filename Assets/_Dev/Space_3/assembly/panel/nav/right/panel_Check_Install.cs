using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class panel_Check_Install : panel_Base
{
    private List<CheckInstall> checkInstalls;

    private GameObject go_Check_Install;
    private Dictionary<int, go_Check_Install> go_Check_Installs = new Dictionary<int, go_Check_Install>();
    private RectTransform content;

    private GameObject scaffold;
    private scaffold01_1 scaffold01_1;

    private Button btn_Checked;
    private Image btn_Image;
    private Image img_Checked;
    private TMP_Text tmp_Checked;

    ScrollRect sview_Check_Install;
    protected override void Awake()
    {
        base.Awake();
        //ui
        checkInstalls = DBManager.instance.CheckInstalls;

        go_Check_Install = ResourceManager.instance.LoadData<GameObject>(nameof(go_Check_Install));
        sview_Check_Install = gameObject.Search<ScrollRect>(nameof(sview_Check_Install));
        content = sview_Check_Install.content;

        //gameobject
        scaffold = ResourceManager.instance.LoadData<GameObject>(nameof(scaffold));
        scaffold01_1 = scaffold.GetComponent<scaffold01_1>();

        //component
        btn_Checked = gameObject.Search<Button>(nameof(btn_Checked));
        btn_Checked.onClick.AddListener(OnClick_Check_Install);
        btn_Image = btn_Checked.targetGraphic as Image;
        img_Checked = gameObject.Search<Image>(nameof(img_Checked));
        tmp_Checked = gameObject.Search<TMP_Text>(nameof(tmp_Checked));

        //trigger
        GameObject triggerCheck = ResourceManager.instance.LoadData<GameObject>(nameof(triggerCheck));
        var v = triggerCheck.GetComponentsInChildren<SpatialTriggerEvent>(true);
        foreach (var item in v)
        {
            int Index = int.Parse(item.name.Split("_")[1]);
            item.onEnterEvent.unityEvent.AddListener(() => OnTriggerEnter_Check_Install(Index));
            item.onExitEvent.unityEvent.AddListener(() => OnTriggerExit_Check_Install(Index));
        }
    }
    protected override IEnumerator Action_Opening()
    {
        yield return null;
        sview_Check_Install.verticalScrollbar.value = 0;
    }

    private int remainCheckCount; //남은 체크 카운트

    public override void Open(Action act = null)
    {
        base.Open(act);
        scaffold01_1.Action_ObjectsColliderEnable(true);

        ResetData();
        InitData();
    }

    public override void Close(Action act = null)
    {
        base.Close(act);
        scaffold01_1.Action_ResetObjects();
    }

    /// <summary>
    /// 기존 데이터 삭제
    /// </summary>
    private void ResetData()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        go_Check_Installs.Clear();
        remainCheckCount = 0;
        img_Checked.gameObject.SetActive(false);
    }

    private void InitData()
    {
        for (int i = 0; i < checkInstalls.Count; i++)
        {
            GameObject go = Instantiate(go_Check_Install, content);
            go_Check_Install script = go.GetComponent<go_Check_Install>();

            CheckInstall checkInstall = checkInstalls[i];
            script.SetData(checkInstall);

            go_Check_Installs.Add(checkInstall.index, script);

            remainCheckCount += checkInstall.isChecked ? 0 : 1;
        }
    }

    private void OnTriggerEnter_Check_Install(int index)
    {
        go_Check_Install go_Check_Install = go_Check_Installs[index];

        CheckInstall checkInstall = go_Check_Install.GetData();
        if (checkInstall.isChecked)
        {
            return;
        }
        IsTriggered_Button(checkInstall.index, true);
        ResourceManager.instance.LoadData<GameObject>("outline_" + index).SetActive(true);
    }
    private void OnTriggerExit_Check_Install(int index)
    {
        go_Check_Install go_Check_Install = go_Check_Installs[index];

        CheckInstall checkInstall = go_Check_Install.GetData();
        if (checkInstall.isChecked)
        {
            return;
        }
        IsTriggered_Button(checkInstall.index, false);
        ResourceManager.instance.LoadData<GameObject>("outline_" + index).SetActive(false);
    }
    private int triggeredIndex;

    /// <summary>
    ///  트리거 되었을 때 버튼 상태 변경
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isChecked"></param>
    public void IsTriggered_Button(int index, bool isChecked)
    {
        triggeredIndex = index;

        if (isChecked)
        {
            btn_Checked.interactable = true;
            btn_Image.sprite = ResourceManager.instance.LoadDataSprite("btn_370_enabled");
            tmp_Checked.text = (index + 1).ToString("00") + "번 항목 점검하기";
        }
        else
        {
            btn_Checked.interactable = false;
            btn_Image.sprite = ResourceManager.instance.LoadDataSprite("btn_370_disabled");
            tmp_Checked.text = "점검하기";
        }

    }

    /// <summary>
    /// 체크버튼 클릭
    /// </summary>
    public void OnClick_Check_Install()
    {
        if(remainCheckCount > 0)
        {
            //오브젝트 체크 갱신
            go_Check_Install go_Check_Install = go_Check_Installs[triggeredIndex];
            CheckInstall checkInstall = go_Check_Install.GetData();
            checkInstall.isChecked = true;
            go_Check_Install.SetData(checkInstall);

            //버튼 갱신
            IsTriggered_Button(triggeredIndex, false);
            ResourceManager.instance.LoadData<GameObject>("outline_" + triggeredIndex).SetActive(false);

            //횟수 차감
            remainCheckCount--;
            if(remainCheckCount == 0)//마지막 클릭이라면?
            {
                btn_Checked.interactable = false;
                btn_Image.sprite = ResourceManager.instance.LoadDataSprite("btn_370_finished");
                tmp_Checked.text = "점검완료";
                img_Checked.gameObject.SetActive(true);
            }
        }
    }
}
