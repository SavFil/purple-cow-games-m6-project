using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance&&GameManager.Instance.playerOneCraft)
        {
            //transform.position=new Vector3(transform.position.x,GameManager.Instance.playerOneCraft.transform.position.y,transform.position.z);
            transform.position = new Vector3(GameManager.Instance.playerOneCraft.transform.position.x, GameManager.Instance.playerOneCraft.transform.position.y, transform.position.z);
        }
    }
}
