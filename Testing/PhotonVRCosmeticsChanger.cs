using System;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.VR.Testing
{
    public class PhotonVRCosmeticsChanger : MonoBehaviour
    {
        public List<PhotonVRCosmeticTest> Cosmetics = new List<PhotonVRCosmeticTest>();

        public void ChangeCosmetics(Dictionary<string, string> Cosmetics)
        {
            PhotonVRManager.SetCosmetics(Cosmetics);
        }

        [Serializable]
        public class PhotonVRCosmeticTest
        {
            public string SlotName;
            public string Cosmetic;
        }
    }
}