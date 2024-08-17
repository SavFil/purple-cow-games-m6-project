using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class CutSceneMenu : Menu
{
    public static CutSceneMenu Instance { get; private set; }

    [Header("CUTSCENE SETTINGS")]
    public Image skipImage;
    bool canInteractSkip;

    //TESTING RESOLUTION\
    public Text res;

    public Image blockOne;
    public Image blockTwo;
    public Image blockThree;


    [Header("FADE CANVAS")]
    public CanvasGroup canvasGroup = null;
    public float duration = .5f;

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 MainMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("MainMenu Created!");

        //TESTING RESOLUTION\
        res.text = Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height + " / " + Screen.currentResolution.refreshRate;

    }



    private void Update()
    {
        if (!ROOT.activeInHierarchy) return;
        Skip();
    }



    void Skip()
    {
        if (InputManager.Instance.playerState[0].shoot && skipImage.fillAmount <= 1 && skipImage.fillAmount > 0&& canInteractSkip)
        {
            skipImage.fillAmount -= .01f;
        }
        else
        {
            skipImage.fillAmount += .01f;
        }
        if (skipImage.fillAmount <= 0)
        {
            GameManager.Instance.StartGame();
        }
    }

    //TESTING RESOLUTION\

    void SetResolution1920()
    {
        GameManager.Instance.SetResolution(GameManager.Instance.resolutionFHD);
    }
    void SetResolution3840()
    {
        GameManager.Instance.SetResolution(GameManager.Instance.resolution4K);
    }



    public IEnumerator CutsceneSequence()
    {
        yield return new WaitForSeconds(1);
        SetImageAlphaOne(blockOne.rectTransform);
        while (blockOne.color.a < 1)
        {
            yield return null;
        }

        skipImage.gameObject.SetActive(true);
        canInteractSkip=true;
        yield return new WaitForSeconds(1);

        SetImageAlphaOne(blockTwo.rectTransform);
        while (blockTwo.color.a < 1)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        SetImageAlphaOne(blockThree.rectTransform);
        while (blockThree.color.a < 1)
        {
            yield return null;
        }
    }

    void SetImageAlphaOne(RectTransform block)
    {
        LeanTween.alpha(block, 1, 1).setEase(LeanTweenType.linear);
    }


}