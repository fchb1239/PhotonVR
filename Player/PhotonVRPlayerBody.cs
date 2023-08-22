using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonVRPlayerBody : MonoBehaviour
{
    public Transform Head;
    [Tooltip("Enable this if you want the body to line up with the default gorilla locomotion hitbox")]
    public bool GorillaLocomotion;

    private void Start() {
        transform.parent = Head;
        if (GorillaLocomotion){
            transform.localScale = new Vector3(0,-0.24f,-0.06f);
        }
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, Head.eulerAngles.y, 0);
    }
}
