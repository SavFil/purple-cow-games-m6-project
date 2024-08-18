using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    [SerializeField] Image SoundOnIcon;
    [SerializeField] Image SoundOffIcon;
    private bool muted = false;

    void Start()
    {
        UpdateButtonIcon();
        AudioListener.pause = muted;

    }

    public void OnButtonPress()
    {
        if (muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }

        else
        {
            muted = false;
            AudioListener.pause = false;
        }

        UpdateButtonIcon();

    }

    private void UpdateButtonIcon()
    {
        if (muted == false)
        {
            SoundOnIcon.enabled = true;
            SoundOffIcon.enabled = false;
        }

        else
        {
            SoundOnIcon.enabled = false;
            SoundOffIcon.enabled = true;

        }
    }
}