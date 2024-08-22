using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.Rendering.DebugUI;

public class CutSceneMenu : Menu
{
    public static CutSceneMenu Instance { get; private set; }



    [Header("--SKIP")]
    public Image skipImage;
    public GameObject skipContainer;
    bool canInteractSkip;
    public Text skipText;

    [Header("--IMAGE BLOCKS")]
    public Image blockOneImage;
    public Image blockTwoImage;
    public Image blockThreeImage;
    Color imageColor = new Color(1, 1, 1, 0);


    [Header("--FADE CANVAS")]
    public CanvasGroup canvasGroup = null;
    public float duration = .5f;
    public bool isCutSceneOver;


    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 MainMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("MainMenu Created!");

        ResetOpacity();
        skipText.text = "Skip";

    }



    private void Update()
    {
        if (!ROOT.activeInHierarchy) return;
        Skip();
    }



    void Skip()
    {
        if (!isCutSceneOver)
        {
            if (InputManager.Instance.playerState[0].shoot && skipImage.fillAmount >= 0 && canInteractSkip)
            {
                skipImage.fillAmount += .02f;
            }
            else
            {
                skipImage.fillAmount -= .02f;
            }
            if (skipImage.fillAmount >= 1)
            {
                StartCoroutine(StartGame());
            }
        }
    }

    IEnumerator StartGame()
    {
        isCutSceneOver=true;
        canInteractSkip = false;
        yield return StartCoroutine(FadeIn());
        GameManager.Instance.StartGame();
        Debug.Log("startgame");
    }


    public IEnumerator CutsceneSequence()
    {
        yield return StartCoroutine(SetImageAlphaOne(blockOneImage, 0));

        skipContainer.SetActive(true);
        canInteractSkip = true;

        yield return StartCoroutine(SetImageAlphaOne(blockTwoImage, 1));

        yield return StartCoroutine(SetImageAlphaOne(blockThreeImage, 1));
        
        yield return new WaitForSeconds(.5f);

        yield return StartCoroutine(StartGame());
    }

    IEnumerator SetImageAlphaOne(Image image, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        float changeOpacity = 0;
        while (image.color.a < 1)
        {
            changeOpacity += .01f;
            imageColor = new Color(1, 1, 1, changeOpacity);
            image.color = imageColor;
            if (image.color.a >= 1) yield break;
            yield return null;
        }
    }


    void ResetOpacity()
    {
        blockOneImage.color = imageColor;
        blockTwoImage.color = imageColor;
        blockThreeImage.color = imageColor;
    }


    IEnumerator FadeIn()
    {
        canvasGroup.interactable = false;
        canvasGroup.LeanAlpha(0, 1);
        while (canvasGroup.alpha > 0)
        {
            yield return null;
        }
    }
}