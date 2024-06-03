using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class go_PlanMap : MonoBehaviour
{
    private TMP_Text tmp_PlanMap;
    private Image img_PlanMap;

    private Button btn_PlanMap;
    private packet_mapdata packet_Mapdata;

    private void Awake()
    {
        btn_PlanMap = gameObject.Search<Button>(nameof(btn_PlanMap));
        btn_PlanMap.onClick.AddListener(OnClick_PlanMap);

        tmp_PlanMap = gameObject.Search<TMP_Text>(nameof(tmp_PlanMap));
        img_PlanMap = gameObject.Search<Image>(nameof(img_PlanMap));
    }
    public void SetData(packet_mapdata packet_Mapdata)
    {
        this.packet_Mapdata = packet_Mapdata;
        tmp_PlanMap.text = packet_Mapdata.mapdata;
        img_PlanMap.sprite = ResourceManager.instance.LoadDataSprite(packet_Mapdata.sprite);
    }

    private void OnClick_PlanMap()
    {
        UIManager.instance.OpenPopup<popup_InstallMapDetail>().SetData(packet_Mapdata);
    }

}
