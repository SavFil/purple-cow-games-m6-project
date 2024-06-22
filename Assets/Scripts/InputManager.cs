using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputState[] playerState = new InputState[2];

    public ButtonMapping[] playerButtons = new ButtonMapping[2];
    public AxisMapping[] playerAxis = new AxisMapping[2];

    public KeyButtonMapping[] playerKeyButtons = new KeyButtonMapping[2];
    public KeyAxisMapping[] playerKeyAxis = new KeyAxisMapping[2];

    public int[] playerController = new int[2];
    public bool[] playerUsingKeys = new bool[2];

    public const float deadZone = 0.01f;

    private System.Array allKeyCodes = System.Enum.GetValues(typeof(KeyCode));

    private string[,] playerButtonNames =
    {
        { "J1_B1", "J1_B2", "J1_B3", "J1_B4", "J1_B5", "J1_B6", "J1_B7", "J1_B8" },
        { "J2_B1", "J2_B2", "J2_B3", "J2_B4", "J2_B5", "J2_B6", "J2_B7", "J2_B8" },
        { "J3_B1", "J3_B2", "J3_B3", "J3_B4", "J3_B5", "J3_B6", "J3_B7", "J3_B8" },
        { "J4_B1", "J4_B2", "J4_B3", "J4_B4", "J4_B5", "J4_B6", "J4_B7", "J4_B8" },
        { "J5_B1", "J5_B2", "J5_B3", "J5_B4", "J5_B5", "J5_B6", "J5_B7", "J5_B8" },
        { "J6_B1", "J6_B2", "J6_B3", "J6_B4", "J6_B5", "J6_B6", "J6_B7", "J6_B8" }
    };


    private string[,] playerAxisNames =
    {
        {"J1_Horizontal", "J1_Vertical"},
        {"J2_Horizontal", "J2_Vertical"},
        {"J3_Horizontal", "J3_Vertical"},
        {"J4_Horizontal", "J4_Vertical"},
        {"J5_Horizontal", "J5_Vertical"},
        {"J6_Horizontal", "J6_Vertical"}
    };

    public string[] oldJoysticks = null;

    public static string[] actionNames = { "Shoot", "Bomb", "Options", "Auto", "Beam", "Menu", "Extra1", "Extra2" };
    public static string[] axisNames = { "Left", "Right", "Up", "Down" };
    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 InputManager!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("InputManager Created!");

        playerController[0] = 0;
        playerController[1] = 1;
        playerUsingKeys[0] = false;
        playerUsingKeys[1] = false;

        playerState[0] = new InputState();
        playerState[1] = new InputState();
        playerButtons[0] = new ButtonMapping();
        playerButtons[1] = new ButtonMapping();
        playerAxis[0] = new AxisMapping();
        playerAxis[1] = new AxisMapping();

        playerKeyAxis[0] = new KeyAxisMapping();
        playerKeyAxis[1] = new KeyAxisMapping();
        playerKeyButtons[0] = new KeyButtonMapping();
        playerKeyButtons[1] = new KeyButtonMapping();

        oldJoysticks = Input.GetJoystickNames();

        StartCoroutine(CheckControllers());
    }

    IEnumerator CheckControllers()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);

            string[] currentJoysticks = Input.GetJoystickNames();

            for (int i = 0; i < currentJoysticks.Length; i++)
            {
                if (i < oldJoysticks.Length)
                {
                    if (currentJoysticks[i] != oldJoysticks[i])
                    {
                        if (string.IsNullOrEmpty(currentJoysticks[i])) // disconnect
                        {
                            Debug.Log("Controller " + i + " has been disconnected.");
                            if (PlayerIsUsingController(i))
                            {
                                ControllerMenu.Instance.whichPlayer = i;
                                ControllerMenu.Instance.textUI.text = "Player " + (i + 1) + " Controller unplugged!";
                                ControllerMenu.Instance.TurnOn(null);
                                // GameManager.instance.PauseGameplay();
                            }
                        }
                        else // connected
                        {
                            Debug.Log("Controller " + i + " is connected: " + currentJoysticks[i]);
                        }
                    }
                }
                else
                {
                    Debug.Log("New controller connected");
                }
            }
        }
    }

    private bool PlayerIsUsingController(int i)
    {
        if (playerController[0] == i)
            return true;
        if (GameManager.Instance.twoPlayer && playerController[1] == i)
            return true;
        return false;
    }

    void UpdatePlayerState(int playerIndex)
    {
        playerState[playerIndex].left = Input.GetKey(playerKeyAxis[playerIndex].left);
        playerState[playerIndex].right = Input.GetKey(playerKeyAxis[playerIndex].right);
        playerState[playerIndex].up = Input.GetKey(playerKeyAxis[playerIndex].up);
        playerState[playerIndex].down = Input.GetKey(playerKeyAxis[playerIndex].down);

        playerState[playerIndex].shoot = Input.GetKey(playerKeyButtons[playerIndex].shoot);
        playerState[playerIndex].bomb = Input.GetKey(playerKeyButtons[playerIndex].bomb);
        playerState[playerIndex].options = Input.GetKey(playerKeyButtons[playerIndex].options);
        playerState[playerIndex].auto = Input.GetKey(playerKeyButtons[playerIndex].auto);
        playerState[playerIndex].beam = Input.GetKey(playerKeyButtons[playerIndex].beam);
        playerState[playerIndex].menu = Input.GetKey(playerKeyButtons[playerIndex].menu);
        playerState[playerIndex].extra1 = Input.GetKey(playerKeyButtons[playerIndex].extra1);
        playerState[playerIndex].extra2 = Input.GetKey(playerKeyButtons[playerIndex].extra2);

        if (playerController[playerIndex] < 0)
        {
            return;
        }

        playerState[playerIndex].left =
            Input.GetAxisRaw(playerAxisNames[playerController[playerIndex], playerAxis[playerIndex].horizontal]) < deadZone;
        playerState[playerIndex].right =
            Input.GetAxisRaw(playerAxisNames[playerController[playerIndex], playerAxis[playerIndex].horizontal]) > deadZone;
        playerState[playerIndex].down =
            Input.GetAxisRaw(playerAxisNames[playerController[playerIndex], playerAxis[playerIndex].vertical]) < deadZone;
        playerState[playerIndex].up =
            Input.GetAxisRaw(playerAxisNames[playerController[playerIndex], playerAxis[playerIndex].vertical]) > deadZone;

        playerState[playerIndex].shoot =
            Input.GetButton(playerButtonNames[playerController[playerIndex], playerButtons[playerIndex].shoot]);
        playerState[playerIndex].bomb =
            Input.GetButton(playerButtonNames[playerController[playerIndex], playerButtons[playerIndex].bomb]);
        playerState[playerIndex].options =
            Input.GetButton(playerButtonNames[playerController[playerIndex], playerButtons[playerIndex].options]);
        playerState[playerIndex].auto =
            Input.GetButton(playerButtonNames[playerController[playerIndex], playerButtons[playerIndex].auto]);
        playerState[playerIndex].beam =
            Input.GetButton(playerButtonNames[playerController[playerIndex], playerButtons[playerIndex].beam]);
        playerState[playerIndex].menu =
            Input.GetButton(playerButtonNames[playerController[playerIndex], playerButtons[playerIndex].menu]);
        playerState[playerIndex].extra1 =
            Input.GetButton(playerButtonNames[playerController[playerIndex], playerButtons[playerIndex].extra1]);
        playerState[playerIndex].extra2 =
            Input.GetButton(playerButtonNames[playerController[playerIndex], playerButtons[playerIndex].extra2]);

    }

    private void FixedUpdate()
    {
        UpdatePlayerState(0);

        if (GameManager.Instance != null && GameManager.Instance.twoPlayer)
        {
            UpdatePlayerState(1);
        }
    }

    // Return the index of the controler pressed
    public int DetectControllerButtonPress()
    {
        int result = -1;

        for (int j = 0; j < 6; j++)
        {
            for (int b = 0; b < 8; b++)
            {
                if (Input.GetButton(playerButtonNames[j, b]))
                {
                    return j;
                }
            }
        }

        return result;
    }

    // Return the button index
    public int DetectButtonPress()
    {
        int result = -1;

        for (int j = 0; j < 6; j++)
        {
            for (int b = 0; b < 8; b++)
            {
                if (Input.GetButton(playerButtonNames[j, b]))
                {
                    return b;
                }
            }
        }

        return result;
    }

    public int DetectKeyPress()
    {
        foreach (KeyCode key in allKeyCodes)
        {
            if (Input.GetKey(key)) return (int)key;
        }

        return -1;
    }

    public bool CheckForPlayerInput(int playerIndex)
    {
        int controller = DetectControllerButtonPress();
        if (controller > -1)
        {
            SetPlayerController(playerIndex, controller);
            return true;
        }
        if (DetectKeyPress() > -1)
        {
            SetPlayerKeyboard(playerIndex);
            return true;
        }

        return false;
    }

    private void SetPlayerKeyboard(int playerIndex)
    {
        playerController[playerIndex] = -1;
        playerUsingKeys[playerIndex] = true;
        Debug.Log("Player " + playerIndex + " is set to keyboard");
    }

    private void SetPlayerController(int playerIndex, int controller)
    {
        playerController[playerIndex] = controller;
        playerUsingKeys[playerIndex] = false;
        Debug.Log("Player " + playerIndex + " is set to controller " + controller);
    }

    internal string GetButtonName(int playerIndex, int actionID)
    {
        string buttonName = "";

        switch (actionID)
        {
            case 0:
                buttonName = playerButtonNames[playerIndex, playerButtons[playerIndex].shoot];
                break;
            case 1:
                buttonName = playerButtonNames[playerIndex, playerButtons[playerIndex].bomb];
                break;
            case 2:
                buttonName = playerButtonNames[playerIndex, playerButtons[playerIndex].options];
                break;
            case 3:
                buttonName = playerButtonNames[playerIndex, playerButtons[playerIndex].auto];
                break;
            case 4:
                buttonName = playerButtonNames[playerIndex, playerButtons[playerIndex].beam];
                break;
            case 5:
                buttonName = playerButtonNames[playerIndex, playerButtons[playerIndex].menu];
                break;
            case 6:
                buttonName = playerButtonNames[playerIndex, playerButtons[playerIndex].extra1];
                break;
            case 7:
                buttonName = playerButtonNames[playerIndex, playerButtons[playerIndex].extra2];
                break;
        }

        char b = buttonName[4];
        return "Button " + b.ToString();
    }

    internal string GetKeyName(int playerIndex, int actionID)
    {
        KeyCode key = KeyCode.None;

        switch (actionID)
        {
            case 0:
                key = playerKeyButtons[playerIndex].shoot;
                break;
            case 1:
                key = playerKeyButtons[playerIndex].bomb;
                break;
            case 2:
                key = playerKeyButtons[playerIndex].options;
                break;
            case 3:
                key = playerKeyButtons[playerIndex].auto;
                break;
            case 4:
                key = playerKeyButtons[playerIndex].beam;
                break;
            case 5:
                key = playerKeyButtons[playerIndex].menu;
                break;
            case 6:
                key = playerKeyButtons[playerIndex].extra1;
                break;
            case 7:
                key = playerKeyButtons[playerIndex].extra2;
                break;
        }

        return key.ToString();
    }

    internal string GetKeyAxisName(int playerIndex, int actionID)
    {
        KeyCode key = KeyCode.None;
        switch (actionID)
        {
            case 0:
                key = playerKeyAxis[playerIndex].left;
                break;
            case 1:
                key = playerKeyAxis[playerIndex].right;
                break;
            case 2:
                key = playerKeyAxis[playerIndex].up;
                break;
            case 3:
                key = playerKeyAxis[playerIndex].down;
                break;
        }

        return key.ToString();
    }

    internal void BindPlayerAxisKey(int playerIndex, int actionID, KeyCode key)
    {
        switch (actionID)
        {
            case 0:
                playerKeyAxis[playerIndex].left = key;
                break;
            case 1:
                playerKeyAxis[playerIndex].right = key;
                break;
            case 2:
                playerKeyAxis[playerIndex].up = key;
                break;
            case 3:
                playerKeyAxis[playerIndex].down = key;
                break;
        }
    }

    internal void BindPlayerKey(int playerIndex, int actionID, KeyCode key)
    {
        switch (actionID)
        {
            case 0:
                playerKeyButtons[playerIndex].shoot = key;
                break;
            case 1:
               playerKeyButtons[playerIndex].bomb = key;
                break;
            case 2:
                playerKeyButtons[playerIndex].options = key;
                break;
            case 3:
                playerKeyButtons[playerIndex].auto = key;
                break;
            case 4:
                playerKeyButtons[playerIndex].beam = key;
                break;
            case 5:
                playerKeyButtons[playerIndex].menu = key;
                break;
            case 6:
                playerKeyButtons[playerIndex].extra1 = key;
                break;
            case 7:
                playerKeyButtons[playerIndex].extra2 = key;
                break;
        }
    }

    internal void BindPlayerButton(int playerIndex, int actionID, byte button)
    {
        switch (actionID)
        {
            case 0:
                playerButtons[playerIndex].shoot = button;
                break;
            case 1:
                playerButtons[playerIndex].bomb = button;
                break;
            case 2:
                playerButtons[playerIndex].options = button;
                break;
            case 3:
                playerButtons[playerIndex].auto = button;
                break;
            case 4:
                playerButtons[playerIndex].beam = button;
                break;
            case 5:
                playerButtons[playerIndex].menu = button;
                break;
            case 6:
                playerButtons[playerIndex].extra1 = button;
                break;
            case 7:
                playerButtons[playerIndex].extra2 = button;
                break;
        }
    }
}


public class InputState
{
    public bool left, right, up, down;
    public bool shoot, bomb, options, auto, beam, menu, extra1, extra2;
}
public class ButtonMapping
{
    public byte shoot = 0;
    public byte bomb = 1;
    public byte options = 2;
    public byte auto = 3;
    public byte beam = 4;
    public byte menu = 5;
    public byte extra1 = 6;
    public byte extra2 = 7;
}

public class AxisMapping
{
    public byte horizontal = 0;
    public byte vertical = 1;
}

public class KeyButtonMapping
{
    public KeyCode shoot = KeyCode.B;
    public KeyCode bomb = KeyCode.N;
    public KeyCode options = KeyCode.M;
    public KeyCode auto = KeyCode.Comma;
    public KeyCode beam = KeyCode.Period;
    public KeyCode menu = KeyCode.J;
    public KeyCode extra1 = KeyCode.K;
    public KeyCode extra2 = KeyCode.L;
}

public class KeyAxisMapping
{
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
}