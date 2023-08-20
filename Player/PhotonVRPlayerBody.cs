using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonVRPlayerBody : MonoBehaviour
{
    public Transform Head;
    public float Offset;

    private void Start()
    {
        transform.position = new Vector3(Head.position.x, Head.position.y + Offset, Head.position.z);
    }
    private void Update()
    {
        transform.eulerAngles = new Vector3(0, Head.eulerAngles.y, 0);
        transform.position = new Vector3(Head.position.x, transform.position.y, Head.position.z);
    }
}
