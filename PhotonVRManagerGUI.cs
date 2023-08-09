#if UNITY_EDITOR
using System;
using System.Net;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

using Photon.Pun;
using UnityEditor.Experimental.SceneManagement;

namespace Photon.VR
{
    [CustomEditor(typeof(PhotonVRManager))]
    public class PhotonVRManagerGUI : Editor
    {
        private Texture2D logo;

        public override void OnInspectorGUI()
        {
            if (logo == null)
                logo = Resources.Load<Texture2D>("PhotonVR/Assets/PhotonVRWide");
            GUILayout.Label(new GUIContent() { image = logo });

            PhotonVRManager manager = (PhotonVRManager)target;

            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                manager.CheckDefaultValues();
            }

            base.OnInspectorGUI();
            GUILayout.Space(10);
            if (PhotonNetwork.IsConnected)
            {
                GUILayout.Label($"State: {PhotonVRManager.GetConnectionState()}");
                GUILayout.Label($"Ping: {PhotonNetwork.GetPing()}");
                GUILayout.Label($"Room: {(PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name : "Not in a room")}");
                if(!PhotonNetwork.InRoom && !manager.JoinRoomOnConnect)
                    if (GUILayout.Button(CreateContent("Join", "Join a random public lobby")))
                        PhotonVRManager.JoinRandomRoom("Default", 16);
            }
            else
                if (!manager.ConnectOnAwake)
                if (GUILayout.Button("Connect"))
                    PhotonVRManager.Connect();
        }

        private GUIContent CreateContent(string text, string tooltip)
        {
            return new GUIContent()
            {
                text = text,
                tooltip = tooltip
            };
        }
    }
}

#endif