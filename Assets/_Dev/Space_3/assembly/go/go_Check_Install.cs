using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class go_Check_Install : MonoBehaviour
{
    private TMP_Text tmp_Index;
    private TMP_Text tmp_Summary;
    private Image img_Suitable;
    private Image img_Unsuitable;

    private void Awake()
    {
        tmp_Index = gameObject.Search<TMP_Text>(nameof(tmp_Index));
        tmp_Summary = gameObject.Search<TMP_Text>(nameof(tmp_Summary));
        img_Suitable = gameObject.Search<Image>(nameof(img_Suitable));
        img_Unsuitable = gameObject.Search<Image>(nameof(img_Unsuitable));
    }
    private CheckInstall _checkInstall;
    private CheckInstall checkInstall
    {
        get => _checkInstall;
        set 
        {
            _checkInstall = value;

            tmp_Index.text = (value.index + 1).ToString("00");
            tmp_Summary.text = value.summary;

            img_Suitable.gameObject.SetActive(value.isSuitable);
            img_Unsuitable.gameObject.SetActive(!value.isSuitable);

            img_Suitable.sprite = ResourceManager.instance.LoadDataSprite(value.isChecked ? Define.check_On : Define.check_Off);
            img_Unsuitable.sprite = ResourceManager.instance.LoadDataSprite(value.isChecked ? Define.check_On : Define.check_Off);
        }
    }

    public void SetData(CheckInstall checkInstall)
    {
        this.checkInstall = checkInstall;
    }
    public CheckInstall GetData()
    {
        return checkInstall;
    }

    public void UpdateData()
    {
        img_Suitable.sprite = ResourceManager.instance.LoadDataSprite(Define.check_On);
        img_Unsuitable.sprite = ResourceManager.instance.LoadDataSprite(Define.check_On);
    }
}
