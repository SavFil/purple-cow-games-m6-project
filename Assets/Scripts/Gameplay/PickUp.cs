using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        INVALID,

        Bomb,
        Coin,
        PowerUp,
        BeamUp,
        Options,
        Medal,
        Secret,
        Lives,

        NOOFPICKUPTYPES
    };

    public PickUpConfig config;
    public Vector2 position;
    public Vector2 velocity;

    private void OnEnable()
    {
        position = transform.position;
    }

    private void FixedUpdate()
    {
        //Move
        position.y -= config.fallSpeed;
        if (GameManager.Instance && GameManager.Instance.progressWindow)
        {
            float posY = position.y - GameManager.Instance.progressWindow.transform.position.y;
            if (posY <- 180) // Offscreen
            {
                GameManager.Instance.PickUpFallOffScreen(this);
                Destroy(gameObject);
                return;
            }
        }
        transform.position = position;
    }

    public void ProcessPickUp(int playerIndex, CraftData craftData)
    {
        switch (config.type)
        {
            case PickUpType.Coin:
                {
                    GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(config.coinValue);
                    break;
                }
            case PickUpType.PowerUp:
                {
                    GameManager.Instance.playerCrafts[playerIndex].PowerUp((byte)config.powerLevel);
                    break;
                }

            case PickUpType.Lives:
                {
                    GameManager.Instance.playerCrafts[playerIndex].OneUp();
                    break;
                }

            case PickUpType.Secret:
                {
                    GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(config.coinValue);
                    break;
                }

            case PickUpType.BeamUp:
                {
                    GameManager.Instance.playerCrafts[playerIndex].IncreaseBeamStrength();
                    break;
                }

            case PickUpType.Options:
                {
                    GameManager.Instance.playerCrafts[playerIndex].AddOption();
                    break;
                }

            case PickUpType.Bomb:
                {
                    GameManager.Instance.playerCrafts[playerIndex].AddBomb(config.bombPower);
                    break;
                }

            case PickUpType.Medal:
                {
                    GameManager.Instance.playerCrafts[playerIndex].AddMedal(config.medalLevel, 
                                                                            config.medalValue);
                    break;
                }

            default:
                {
                    Debug.LogError("Unprocessed config type: " + config.type);
                    break;
                }
        };

        Destroy(gameObject);
    }
}
