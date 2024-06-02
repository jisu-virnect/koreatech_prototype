using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine.UI;

public class Space_3_Sequence : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Toggle toggle;
    private TMP_Text tMP_Text;
    private Button button;
    private GameObject go_Focus;

    public Install sequence;

    private void GetComponent()
    {
        canvasGroup = gameObject.GetComponentInChildren<CanvasGroup>();
        toggle = gameObject.GetComponentInChildren<Toggle>();
        tMP_Text = gameObject.GetComponentInChildren<TMP_Text>();

        button = gameObject.GetComponentInChildren<Button>();
        button.onClick.AddListener(OnClick_OpenSummary);

        go_Focus = gameObject.transform.GetChild(0).gameObject;
    }

    public void OnClick_OpenSummary()
    {
        UIManager.instance.OpenPopup<popup_InstallDetail>().SetData(sequence);
    }

    public void SetData(Install install)
    {
        GetComponent();
        this.sequence = install;
        SetInstallData(install);
    }

    public void SetInstallData(Install install)
    {
        tMP_Text.text = install.title;
        SetSequenceState(SEQUENCE_STATE.BEFORE);
    }
    public void SetSequenceState(SEQUENCE_STATE sEQUENCE_STATE)
    {
        canvasGroup.alpha = 0.5f;
        toggle.isOn = false;
        tMP_Text.text = sequence.title;
        button.interactable = false;
        go_Focus.SetActive(false);

        switch (sEQUENCE_STATE)
        {
            case SEQUENCE_STATE.BEFORE:
                break;
            case SEQUENCE_STATE.FOCUS:
                canvasGroup.alpha = 1f;
                button.interactable = true;
                go_Focus.SetActive(true);
                break;
            case SEQUENCE_STATE.AFTER:
                toggle.isOn = true;
                button.interactable = true;
                break;
            default:
                break;
        }
    }
}