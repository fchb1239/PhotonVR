using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.VR.Testing
{
    public class PhotonVRColourChanger : MonoBehaviour
    {
        public Color Colour;

        public void ChangeColour(Color Colour) => PhotonVRManager.SetColour(Colour);
    }
}