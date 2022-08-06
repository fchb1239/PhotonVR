#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

using Photon.Pun;

namespace Photon.VR.Testing
{
    [CustomEditor(typeof(PhotonVRColourChanger))]
    public class PhotonVRColourChangerGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonVRColourChanger manager = (PhotonVRColourChanger)target;
                base.OnInspectorGUI();
                if (GUILayout.Button("Change"))
                    manager.ChangeColour(manager.Colour);
            }
            else
            {
                GUILayout.Label("Not in a room");
            }
        }
    }
}
#endif