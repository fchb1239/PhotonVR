using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonVRPlayerBody : MonoBehaviour
{
    public Transform Head;
    public float Offset;

    private void Update()
    {
        transform.rotation = new Quaternion(0, Head.rotation.y, 0, Head.rotation.w);
        transform.position = new Vector3(Head.position.x, Head.position.y + Offset, Head.position.z);
    }
}
