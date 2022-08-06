using UnityEngine;
using Photon.VR.Cosmetics;

namespace Photon.VR.Testing
{
    public class PhotonVRCosmeticsChanger : MonoBehaviour
    {
        public PhotonVRCosmeticsData Cosmetics;

        public void ChangeCosmetics(PhotonVRCosmeticsData Cosmetics) => PhotonVRManager.SetCosmetics(Cosmetics);
    }

}