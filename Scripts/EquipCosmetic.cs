using UnityEngine;
using Photon.VR;

public class EquipCosmetic : MonoBehaviour
{
    public string Tag = "-Hand tag-";
    public string Type = "-Cosmetic type-";
    public string Cosmetic = "-Cosmetic name-";

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tag)
        {
            PhotonVRManager.SetCosmetic(Type, Cosmetic);
        }
    }
}
