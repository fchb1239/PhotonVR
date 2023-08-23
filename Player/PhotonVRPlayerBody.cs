using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonVRPlayerBody : MonoBehaviour
{
    public Transform Head;

    private void Start() {
        transform.parent = Head;
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, Head.eulerAngles.y, 0);
    }
}
