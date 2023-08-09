using System.Collections.Generic;
using UnityEngine;
using Photon.VR.Cosmetics;

namespace Photon.VR.Cosmetics
{
    public class CosmeticsManager : MonoBehaviour
    {
        public static List<string> EquippedCosmetics = new List<string>();

        private void Start()
        {
            if (PhotonVRManager.Manager != null)
            {
                PhotonVRCosmeticsData cosmeticsData = PhotonVRManager.Manager.Cosmetics;
                EquippedCosmetics = new List<string>
                {
                    cosmeticsData.Head,
                    cosmeticsData.Face,
                    cosmeticsData.Body,
                    cosmeticsData.LeftHand,
                    cosmeticsData.RightHand
                };
                EquippedCosmetics.RemoveAll(string.IsNullOrEmpty);
            }
        }
    }
}
