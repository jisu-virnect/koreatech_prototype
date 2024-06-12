using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine.UI;

public class go_Install : MonoBehaviour
{
    public Install install;

    private GameObject img_Focus;
    private Image img_Check;
    private Image img_Index;
    private TMP_Text tmp_Index;
    private TMP_Text tmp_Summary;

    private string check_Off = nameof(check_Off);
    private string check_On = nameof(check_On);
    private Color color_off = new Color(.6f, .6f, .6f, 1f);
    private Color color_on = new Color(.2f, .4f, 1f, 1f);


    private void Awake()
    {
        GetComponent();
    }
    private void GetComponent()
    {
        img_Focus = gameObject.SearchGameObject(nameof(img_Focus));

        img_Check = gameObject.Search<Image>(nameof(img_Check));
        img_Index = gameObject.Search<Image>(nameof(img_Index));
        tmp_Index = gameObject.Search<TMP_Text>(nameof(tmp_Index));
        tmp_Summary = gameObject.Search<TMP_Text>(nameof(tmp_Summary));
    }

    public void InitData(Install install)
    {
        this.install = install;
        tmp_Index.text = (install.index+1).ToString("00");
        tmp_Summary.text = install.title;
        SetData(SEQUENCE_STATE.BEFORE);
    }
    public void SetData(SEQUENCE_STATE sEQUENCE_STATE)
    {
        switch (sEQUENCE_STATE)
        {
            case SEQUENCE_STATE.BEFORE:
                img_Check.sprite = ResourceManager.instance.LoadDataSprite(check_Off);
                img_Index.color = color_off;
                img_Focus.SetActive(false);
                tmp_Summary.color = Define.color_black_153;
                break;
            case SEQUENCE_STATE.FOCUS:
                img_Check.sprite = ResourceManager.instance.LoadDataSprite(check_Off);
                img_Index.color = color_off;
                img_Focus.SetActive(true);
                tmp_Summary.color = Define.color_black_153;
                break;
            case SEQUENCE_STATE.AFTER:
                img_Check.sprite = ResourceManager.instance.LoadDataSprite(check_On);
                img_Index.color = color_on;
                img_Focus.SetActive(false);
                tmp_Summary.color = Define.color_black_0;
                break;
            default:
                break;
        }
    }
}