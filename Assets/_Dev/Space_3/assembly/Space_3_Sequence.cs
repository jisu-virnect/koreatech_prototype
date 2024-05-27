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

    public Sequence sequence;

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
        Space_3_SequenceManager.instance.sequenceDetail.OpenSequenceDetail(sequence);
    }

    public void SetData(Sequence sequence)
    {
        GetComponent();
        this.sequence = sequence;
        SetSequenceData(sequence);
        SetSequenceState(SEQUENCE_STATE.BEFORE);
    }

    public void SetSequenceData(Sequence sequence)
    {
        tMP_Text.text = sequence.title;
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
            case SEQUENCE_STATE.FOCUE:
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
public enum SEQUENCE_STATE
{
    BEFORE,
    FOCUE,
    AFTER,
}
