using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private Dictionary<eAudioMixerType, AudioSource> audioSourceDics = new Dictionary<eAudioMixerType, AudioSource>();

    public AudioClip[] audioClips;
    private Dictionary<eAudioClips, AudioClip> audioClipDics = new Dictionary<eAudioClips, AudioClip>();

    private void Awake()
    {
        instance = this;

        AudioSource[] audioSources =  transform.GetComponentsInChildren<AudioSource>(true);
        foreach (var audioSource in audioSources)
        {
            audioSourceDics.Add(Util.String2Enum<eAudioMixerType>(audioSource.name), audioSource);
        }

        foreach (var audioClip in audioClips) 
        {
            audioClipDics.Add(Util.String2Enum<eAudioClips>(audioClip.name), audioClip);
        }
    }
    public void PlayEffect(eAudioClips eAudioClip, float volumeScale = 1f)
    {
        if (audioClipDics.ContainsKey(eAudioClip))
        {
            audioSourceDics[eAudioMixerType.SoundEffects].PlayOneShot(audioClipDics[eAudioClip], volumeScale);
        }
        else
        {
            Debug.LogError($"해당 {eAudioClip} audioClip이 존재하지 않음");
        }
    }
    public void PlayVoice(eAudioClips eAudioClip, float volumeScale = 1f)
    {
        if (audioClipDics.ContainsKey(eAudioClip))
        {
            AudioSource audioSource = audioSourceDics[eAudioMixerType.Interface];
            audioSource.Stop();
            audioSource.clip = audioClipDics[eAudioClip];
            audioSource.volume = volumeScale;
            audioSource.Play();
        }
        else
        {
            Debug.LogError($"해당 {eAudioClip} audioClip이 존재하지 않음");
        }
    }
}

public enum eAudioMixerType
{
    Ambience,
    Dialogue,
    Interface,
    Master,
    Music,
    SoundEffects,
}

public enum eAudioClips
{
    //dummy
    watermark_typecast_audio_clip,

    //bgm
    bgm_factory,
    bgm_festival,

    //effect
    effect_click,
    effect_gate,
    effect_next,
    effect_prev,
    effect_screen,
    effect_success,
    effect_close,

    //voice
    voice_0_toast_guide,
    voice_1_1_toast_guide,
    voice_1_1_toast_end,
    voice_1_2_toast_guide,
    voice_1_2_modal_end,
    voice_2_toast_guide,
    voice_3_toast_guide,
    voice_t_1,
    voice_t_2,
    voice_t_3,
    voice_toast_end,
    voice_toast_guide,
    voice_1_1_modal_7,
    voice_1_1_modal_8,
    voice_1_2_modal_1,
    voice_1_2_modal_2,
    voice_1_2_modal_3,
    voice_1_2_modal_4,
}