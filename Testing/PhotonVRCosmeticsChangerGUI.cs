#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Photon.Pun;
using Photon.VR.Cosmetics;

namespace Photon.VR.Testing
{
    [CustomEditor(typeof(PhotonVRCosmeticsChanger))]
    public class PhotonVRCosmeticsChangerGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            if (PhotonNetwork.InRoom)
            {
                base.OnInspectorGUI();
                PhotonVRCosmeticsChanger manager = (PhotonVRCosmeticsChanger)target;
                if (GUILayout.Button("Change"))
                    manager.ChangeCosmetics(manager.Cosmetics);
            }
            else
            {
                GUILayout.Label("Not in a room");
            }
        }
    }
}
#endif
