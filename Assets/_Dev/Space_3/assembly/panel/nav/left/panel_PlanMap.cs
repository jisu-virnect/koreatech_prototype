using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class panel_PlanMap : panel_Base
{
    private TMP_Text tmp_Title;
    private Button btn_Show;
    private Button btn_Hide;

    private GameObject go_BG;
    private packet_mapdata_root packet_Mapdata_Root;

    private GameObject go_PlanMap;

    protected override void Awake()
    {
        base.Awake();
        tmp_Title = gameObject.Search<TMP_Text>(nameof(tmp_Title));

        go_PlanMap = ResourceManager.instance.LoadData<GameObject>(nameof(go_PlanMap));
        go_BG = gameObject.SearchGameObject(nameof(go_BG));

        btn_Show = gameObject.Search<Button>(nameof(btn_Show));
        btn_Show.onClick.AddListener(OnClick_Show);

        btn_Hide = gameObject.Search<Button>(nameof(btn_Hide));
        btn_Hide.onClick.AddListener(OnClick_Hide);

        OnClick_Show();
    }
    protected override IEnumerator Action_Opening()
    {
        yield return null;
        Util.RefreshLayout(gameObject, "img_BG");
    }
    public override void SetData<T>(T t)
    {
        base.SetData(t);
        Util.DestroyChildrenGameObject(go_BG.transform);
        packet_Mapdata_Root = t as packet_mapdata_root;
        foreach (var packet_Mapdata in packet_Mapdata_Root.packet_mapdatas)
        {
            GameObject go = Instantiate(go_PlanMap, go_BG.transform);
            go_PlanMap script = go.GetComponent<go_PlanMap>();
            script.SetData(packet_Mapdata);
        }

        tmp_Title.text = packet_Mapdata_Root.title;
    }

    private void OnClick_Show()
    {
        btn_Show.gameObject.SetActive(false);
        btn_Hide.gameObject.SetActive(true);
        go_BG.SetActive(true);
    }
    private void OnClick_Hide()
    {
        btn_Show.gameObject.SetActive(true);
        btn_Hide.gameObject.SetActive(false);
        go_BG.SetActive(false);
    }
}
