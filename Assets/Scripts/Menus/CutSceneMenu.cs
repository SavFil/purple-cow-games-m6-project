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

    [Header("--TESTING RESOLUTION")]
    //TESTING RESOLUTION\
    public Text res;
    [Header("--IMAGE BLOCKS")]
    public Image blockOneImage;
    public Image blockTwoImage;
    public Image blockThreeImage;
    public Material blockOneMat;
    public Material blockTwoMat;
    public Material blockThreeMat;

    [Header("--FADE CANVAS")]
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


        skipText.text = "Skip";


    }



    private void Update()
    {
        if (!ROOT.activeInHierarchy) return;
        Skip();
    }



    void Skip()
    {
        if (InputManager.Instance.playerState[0].shoot && skipImage.fillAmount >= 0 && canInteractSkip)
        {
            skipImage.fillAmount += .01f;
        }
        else
        {
            skipImage.fillAmount -= .01f;
        }
        if (skipImage.fillAmount >= 1)
        {
            ResetSaturation();
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

        yield return StartCoroutine(SetImageAlphaOne(blockOneMat,1));


        skipContainer.SetActive(true);
        canInteractSkip = true;


        yield return StartCoroutine(SetImageAlphaOne(blockTwoMat, 1));


        yield return StartCoroutine(SetImageAlphaOne(blockThreeMat, 1));



        yield return StartCoroutine(SetMaterialSaturation(blockOneMat, 1));

        yield return StartCoroutine(SetMaterialSaturation(blockTwoMat, 1));

        yield return StartCoroutine(SetMaterialSaturation(blockThreeMat, 1));

        yield return new WaitForSeconds(.5f);
        skipText.text = "Play";
    }

    IEnumerator SetImageAlphaOne(Material material, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        float changeOpacity = 0;
        while (material.GetFloat("_Opacity") < 1)
        {
            changeOpacity += .01f;
            material.SetFloat("_Opacity", changeOpacity);
            if (material.GetFloat("_Opacity") > 1) yield break;
            yield return null;
        }
    }

    IEnumerator SetMaterialSaturation(Material material,float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        float changeSaturation = 0;
        while (material.GetFloat("_Saturation") < 1)
        {
            changeSaturation += .01f;
            material.SetFloat("_Saturation", changeSaturation);
            if (material.GetFloat("_Saturation") > 1) yield break;
            yield return null;
        }
    }

    void ResetSaturation()
    {
        blockOneMat.SetFloat("_Saturation", 0);
        blockTwoMat.SetFloat("_Saturation", 0);
        blockThreeMat.SetFloat("_Saturation", 0);

        blockOneMat.SetFloat("_Opacity", 0);
        blockTwoMat.SetFloat("_Opacity", 0);
        blockThreeMat.SetFloat("_Opacity", 0);
    }
}