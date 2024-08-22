using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlsOptionsMenu : Menu
{
    public static ControlsOptionsMenu Instance { get; private set; }

    [Header("CONTROL OPTION SETTINGS")]
    public Button[] p1_buttons = new Button[8];
    public Button[] p2_buttons = new Button[8];
    public Button[] p1_keys = new Button[12];
    public Button[] p2_keys = new Button[12];

    public GameObject bindingPanel = null;
    public TextMeshProUGUI bindText = null;
    public EventSystem eventSystem = null;

    private bool bindingKey = false;
    private bool bindingAxis = false;
    private bool bindingButton = false;

    private int actionBinding = -1;
    private int playerBinding = -1;

    private bool waiting = false;

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 ControlsOptionsMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("ControlsOptionsMenu Created!");
    }

    private void OnEnable()
    {
        UpdateButtons();
    }

    void UpdateButtons()
    {
        // joystick buttons
        for (int b = 0; b < 8; b++)
        {
            p1_buttons[b].GetComponentInChildren<TextMeshProUGUI>().text = InputManager.Instance.GetButtonName(0, b);
            p2_buttons[b].GetComponentInChildren<TextMeshProUGUI>().text = InputManager.Instance.GetButtonName(1, b);
        }


        // key "buttons"
        for (int k = 0; k < 8; k++)
        {
            p1_keys[k].GetComponentInChildren<TextMeshProUGUI>().text = InputManager.Instance.GetKeyName(0, k);
            p2_keys[k].GetComponentInChildren<TextMeshProUGUI>().text = InputManager.Instance.GetKeyName(1, k);
        }

        // key "axes"
        for (int a = 0; a < 4; a++)
        {
            p1_keys[a + 8].GetComponentInChildren<TextMeshProUGUI>().text = InputManager.Instance.GetKeyAxisName(0, a);
            p2_keys[a + 8].GetComponentInChildren<TextMeshProUGUI>().text = InputManager.Instance.GetKeyAxisName(1, a);
        }
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }

    public void BindP1Key(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindText.text = "Press a key for player 1 " + InputManager.actionNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = true;
        bindingAxis = false;
        bindingButton = false;
        playerBinding = 0;
        actionBinding = actionID;

        waiting = true;
    }
    public void BindP1AxisKey(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindText.text = "Press a key for player 1 " + InputManager.axisNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = true;
        bindingAxis = true;
        bindingButton = false;
        playerBinding = 0;
        actionBinding = actionID;

        waiting = true;
    }

    public void BindP1Button(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindText.text = "Press a button for player 1 " + InputManager.actionNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = false;
        bindingAxis = false;
        bindingButton = true;
        playerBinding = 0;
        actionBinding = actionID;

        waiting = true;
    }
    public void BindP2Key(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindText.text = "Press a key for player 2 " + InputManager.actionNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = true;
        bindingAxis = false;
        bindingButton = false;
        playerBinding = 1;
        actionBinding = actionID;

        waiting = true;
    }
    public void BindP2AxisKey(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindText.text = "Press a key for player 2 " + InputManager.axisNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = true;
        bindingAxis = true;
        bindingButton = false;
        playerBinding = 1;
        actionBinding = actionID;

        waiting = true;
    }

    public void BindP2Button(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindText.text = "Press a button for player 2 " + InputManager.actionNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = false;
        bindingAxis = false;
        bindingButton = true;
        playerBinding = 1;
        actionBinding = actionID;

        waiting = true;
    }

    private void Update()
    {
        if (waiting)
        {
            if (Input.anyKey)
            {
                return;
            }
            if (InputManager.Instance.DetectButtonPress() > -1)
            {
                return;
            }
            waiting = false;
        }
        else
        {
            if (bindingKey || bindingButton)
            {
                if (bindingKey)
                {
                    foreach (KeyCode key in KeyCode.GetValues(typeof(KeyCode)))
                    {
                        if (!key.ToString().Contains("Joystick"))
                        {
                            if (Input.GetKeyDown(key))
                            {
                                if (bindingAxis)
                                {
                                    InputManager.Instance.BindPlayerAxisKey(playerBinding, actionBinding, key);
                                }
                                else
                                {
                                    InputManager.Instance.BindPlayerKey(playerBinding, actionBinding, key);
                                }
                                CloseBindingPanel();
                            }
                        }
                    }
                }
                else if (bindingButton)
                {
                    int button = InputManager.Instance.DetectButtonPress();

                    if (button > -1)
                    {
                        InputManager.Instance.BindPlayerButton(playerBinding, actionBinding, (byte)button);
                        CloseBindingPanel();
                    }
                }
            }
        }
    }

    private void CloseBindingPanel()
    {
        bindingPanel.SetActive(false);
        bindingKey = false;
        bindingButton = false;
        eventSystem.gameObject.SetActive(true);
        UpdateButtons();
    }
}