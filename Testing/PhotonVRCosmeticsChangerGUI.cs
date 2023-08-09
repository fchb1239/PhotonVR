#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Photon.Pun;

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
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();

                    foreach (var cosmetic in manager.Cosmetics)
                    {
                        dictionary.Add(cosmetic.SlotName, cosmetic.Cosmetic);
                    }

                    manager.ChangeCosmetics(dictionary);
                }
            }
            else
            {
                GUILayout.Label("Not in a room");
            }
        }
    }
}
#endif
