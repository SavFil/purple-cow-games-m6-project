using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    CraftData craftData = new CraftData();
    Vector3 newPosition = new Vector3();
    private void FixedUpdate()
    {
        if (InputManager.Instance)
        {
            craftData.positionX += InputManager.Instance.playerState[0].movement.x;
            craftData.positionY += InputManager.Instance.playerState[0].movement.y;
            newPosition.x =(int)craftData.positionX;
            newPosition.y = (int)craftData.positionY;
            gameObject.transform.position = newPosition;
        }
    }
}

public class CraftData
{
    public float positionX;
    public float positionY;
}
