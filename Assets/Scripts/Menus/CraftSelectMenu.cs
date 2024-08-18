using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CraftSelectMenu : Menu
{
    public static CraftSelectMenu Instance { get; private set; }


    [Header("FADE CANVAS")]
    public CanvasGroup canvasGroup = null;
    private float duration = .5f;

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 CraftSelectMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("CraftSelectMenu Created!");
    }

    public void OnPlayButton()
    {
        StartCoroutine(FadeCutscene());
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }



    IEnumerator FadeCutscene()
    {
        canvasGroup.interactable = false;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.LeanAlpha(0, duration);
            yield return null;
        }
        TurnOff(false);

        yield return new WaitForSeconds(duration);
        CutSceneMenu.Instance.TurnOn(this);
        while (CutSceneMenu.Instance.canvasGroup.alpha < 1)
        {
            CutSceneMenu.Instance.canvasGroup.LeanAlpha(1, CutSceneMenu.Instance.duration);
            yield return null;
        }

        StartCoroutine(CutSceneMenu.Instance.CutsceneSequence());
    }
}