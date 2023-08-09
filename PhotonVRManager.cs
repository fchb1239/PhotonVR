using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.VR.Player;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;

using ExitGames.Client.Photon;
using Photon.VR.Saving;

namespace Photon.VR
{
    public class PhotonVRManager : MonoBehaviourPunCallbacks
    {
        public static PhotonVRManager Manager { get; private set; }

        [Header("Photon")]
        public string AppId;
        public string VoiceAppId;
        [Tooltip("Please read https://doc.photonengine.com/en-us/pun/current/connection-and-authentication/regions for more information")]
        public string Region = "eu";

        [Header("Player")]
        public Transform Head;
        public Transform LeftHand;
        public Transform RightHand;
        public Color Colour;
        public Dictionary<string, string> Cosmetics { get; private set; } = new Dictionary<string, string>();

        [Header("Networking")]
        public string DefaultQueue = "Default";
        public int DefaultRoomLimit = 16;

        [Header("Other")]
        // This shuold always be true
        [Tooltip("If the user shall connect when this object has awoken")]
        public bool ConnectOnAwake = true;
        [Tooltip("If the user shall join a room when they connect")]
        public bool JoinRoomOnConnect = true;

        [NonSerialized]
        public PhotonVRPlayer LocalPlayer;

        private RoomOptions options;

        private ConnectionState State = ConnectionState.Disconnected;

        private void Start()
        {
            if (Manager == null)
                Manager = this;
            else
            {
                Debug.LogError("There can't be multiple PhotonVRManagers in a scene");
                Application.Quit();
            }

            DontDestroyOnLoad(gameObject);

            if (ConnectOnAwake)
                Connect();

            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Colour")))
                Colour = JsonUtility.FromJson<Color>(PlayerPrefs.GetString("Colour"));
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Cosmetics")))
                Cosmetics = PhotonVRValueSaver.GetDictionary("Cosmetics");

        }

#if UNITY_EDITOR
        public void CheckDefaultValues()
        {
            bool b = CheckForRig(this);
            if (b)
            {
                if (string.IsNullOrEmpty(AppId))
                    AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdFusion;

                if (string.IsNullOrEmpty(VoiceAppId))
                    VoiceAppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice;

                Debug.Log("Attempted to set default values");
            }
        }

        private bool CheckForRig(PhotonVRManager manager)
        {
            GameObject[] objects = FindObjectsOfType<GameObject>();

            bool b = false;

            if (manager.Head == null)
            {
                b = true;
                foreach (GameObject obj in objects)
                {
                    if (obj.name.Contains("Camera") || obj.name.Contains("Head"))
                    {
                        manager.Head = obj.transform;
                        break;
                    }
                }
            }

            if (manager.LeftHand == null)
            {
                b = true;
                foreach (GameObject obj in objects)
                {
                    if (obj.name.Contains("Left") && (obj.name.Contains("Hand") || obj.name.Contains("Controller")))
                    {
                        manager.LeftHand = obj.transform;
                        break;
                    }
                }
            }

            if (manager.RightHand == null)
            {
                b = true;
                foreach (GameObject obj in objects)
                {
                    if (obj.name.Contains("Right") && (obj.name.Contains("Hand") || obj.name.Contains("Controller")))
                    {
                        manager.RightHand = obj.transform;
                        break;
                    }
                }
            }

            return b;
        }
#endif

        /// <summary>
        /// Connects to Photon using the specified AppId and VoiceAppId
        /// </summary>
        public static bool Connect()
        {
            if (string.IsNullOrEmpty(Manager.AppId) || string.IsNullOrEmpty(Manager.VoiceAppId))
            {
                Debug.LogError("Please input an app id");
                return false;
            }

            PhotonNetwork.AuthValues = null;

            Manager.State = ConnectionState.Connecting;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = Manager.AppId;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = Manager.VoiceAppId;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = Manager.Region;
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log($"Connecting - AppId: {PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime} VoiceAppId: {PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice}");
            return true;
        }

        /// <summary>
        /// Connects to Photon using the specified AppId and VoiceAppId with authentication
        /// </summary>
        public static bool ConnectAuthenticated(string username, string token)
        {
            if (string.IsNullOrEmpty(Manager.AppId) || string.IsNullOrEmpty(Manager.VoiceAppId))
            {
                Debug.LogError("Please input an app id");
                return false;
            }

            AuthenticationValues authentication = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
            authentication.AddAuthParameter("username", username);
            authentication.AddAuthParameter("token", token);
            PhotonNetwork.AuthValues = authentication;


            Manager.State = ConnectionState.Connecting;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = Manager.AppId;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = Manager.VoiceAppId;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = Manager.Region;
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log($"Connecting - AppId: {PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime} VoiceAppId: {PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice}");
            return true;
        }

        /// <summary>
        /// Disconnects from the Photon servers
        /// </summary>
        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        /// <summary>
        /// Changes Photon servers
        /// </summary>
        /// <param name=Id">The new AppId</param>
        /// <param name="VoiceId">The new VoiceAppId</param>
        public static void ChangeServers(string Id, string VoiceId)
        {
            PhotonNetwork.Disconnect();
            Manager.AppId = Id;
            Manager.VoiceAppId = VoiceId;
            Connect();
        }

        /// <summary>
        /// Changes Photon servers
        /// </summary>
        /// <param name=Id">The new AppId</param>
        /// <param name="VoiceId">The new VoiceAppId</param>
        public static void ChangeServersAuthenticated(string Id, string VoiceId, string username, string token)
        {
            PhotonNetwork.Disconnect();
            Manager.AppId = Id;
            Manager.VoiceAppId = VoiceId;
            ConnectAuthenticated(username, token);
        }

        /// <summary>
        /// Sets the Photon nickname to something
        /// </summary>
        /// <param name="Name">The string you want the Photon nickname to be</param>
        public static void SetUsername(string Name)
        {
            PhotonNetwork.LocalPlayer.NickName = Name;
            PlayerPrefs.SetString("Username", Name);

            if (PhotonNetwork.InRoom)
                if (Manager.LocalPlayer != null)
                    Manager.LocalPlayer.RefreshPlayerValues();
        }

        /// <summary>
        /// Sets the colour
        /// </summary>
        /// <param name="PlayerColour">The colour you want the player to be</param>
        public static void SetColour(Color PlayerColour)
        {
            Manager.Colour = PlayerColour;
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["Colour"] = JsonUtility.ToJson(PlayerColour);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            PlayerPrefs.SetString("Colour", JsonUtility.ToJson(PlayerColour));

            if (PhotonNetwork.InRoom)
                if (Manager.LocalPlayer != null)
                    Manager.LocalPlayer.RefreshPlayerValues();
        }

        /// <summary>
        /// Sets the cosmetics
        /// </summary>
        /// <param name="PlayerCosmetics">The cosmetics you want to set</param>
        public static void SetCosmetics(Dictionary<string, string> PlayerCosmetics)
        {
            Manager.Cosmetics = PlayerCosmetics;
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["Cosmetics"] = Manager.Cosmetics;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            PhotonVRValueSaver.SaveDictionary("Cosmetics", Manager.Cosmetics);

            if (PhotonNetwork.InRoom)
                if (Manager.LocalPlayer != null)
                    Manager.LocalPlayer.RefreshPlayerValues();
        }

        /// <summary>
        /// Sets a specefic cosmetic
        /// </summary>
        /// <param name="Type">The type of cosmetic you want to set</param>
        public static void SetCosmetic(string Type, string CosmeticId)
        {
            Manager.Cosmetics[Type] = CosmeticId;
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["Cosmetics"] = Manager.Cosmetics;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            PhotonVRValueSaver.SaveDictionary("Cosmetics", Manager.Cosmetics);

            if (PhotonNetwork.InRoom)
                if (Manager.LocalPlayer != null)
                    Manager.LocalPlayer.RefreshPlayerValues();
        }

        public override void OnConnectedToMaster()
        {
            State = ConnectionState.Connected;
            Debug.Log("Connected");

            PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("Username");
            PhotonNetwork.LocalPlayer.CustomProperties["Colour"] = JsonUtility.ToJson(Colour);
            PhotonNetwork.LocalPlayer.CustomProperties["Cosmetics"] = Cosmetics;

            if (JoinRoomOnConnect)
                JoinRandomRoom(DefaultQueue, DefaultRoomLimit);
        }

        /// <summary>
        /// Gets the connection state
        /// </summary>
        /// <returns>The current connection state</returns>
        public static ConnectionState GetConnectionState()
        {
            return Manager.State;
        }

        /// <summary>
        /// Switches scenes and joins a new room
        /// </summary>
        /// <param name="SceneIndex"></param>
        /// <param name="MaxPlayers"></param>
        public static void SwitchScenes(int SceneIndex, int MaxPlayers)
        {
            SceneManager.LoadScene(SceneIndex);
            JoinRandomRoom(SceneIndex.ToString(), MaxPlayers);
        }

        /// <summary>
        /// Switches scenes and joins a new room
        /// </summary>
        /// <param name="SceneIndex"></param>
        public static void SwitchScenes(int SceneIndex)
        {
            SceneManager.LoadScene(SceneIndex);
            JoinRandomRoom(SceneIndex.ToString(), Manager.DefaultRoomLimit);
        }

        /// <summary>
        /// Joins a room
        /// </summary>
        public static void JoinRandomRoom(string Queue, int MaxPlayers) => _JoinRandomRoom(Queue, MaxPlayers);

        /// <summary>
        /// Joins a room
        /// </summary>
        public static void JoinRandomRoom(string Queue) => _JoinRandomRoom(Queue, Manager.DefaultRoomLimit);

        private static void _JoinRandomRoom(string Queue, int MaxPlayers)
        {
            Manager.State = ConnectionState.JoiningRoom;
            ExitGames.Client.Photon.Hashtable hastable = new ExitGames.Client.Photon.Hashtable();
            hastable.Add("queue", Queue);
            hastable.Add("version", Application.version);

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = (byte)MaxPlayers;
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            roomOptions.CustomRoomProperties = hastable;
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "queue", "version" };
            Manager.options = roomOptions;

            PhotonNetwork.JoinRandomRoom(hastable, (byte)roomOptions.MaxPlayers, MatchmakingMode.RandomMatching, null, null, null);
            Debug.Log($"Joining random with type {hastable["queue"]}");
        }

        /// <summary>
        /// Joins a private room
        /// </summary>
        /// <param name="RoomId">The room code</param>
        /// <param name="MaxPlayers">The maximum amount of players that can be in the room</param>
        public static void JoinPrivateRoom(string RoomId, int MaxPlayers) => _JoinPrivateRoom(RoomId, MaxPlayers);

        /// <summary>
        /// Joins a private room
        /// </summary>
        /// <param name="RoomId">The room code</param>
        /// <param name="MaxPlayers">The maximum amount of players that can be in the room</param>
        public static void JoinPrivateRoom(string RoomId) => _JoinPrivateRoom(RoomId, Manager.DefaultRoomLimit);

        public static void _JoinPrivateRoom(string RoomId, int MaxPlayers)
        {
            PhotonNetwork.JoinOrCreateRoom(RoomId, new RoomOptions()
            {
                IsVisible = false,
                IsOpen = true,
                MaxPlayers = (byte)MaxPlayers
            }, null, null);
            Debug.Log($"Joining a private room: {RoomId}");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined a room");
            State = ConnectionState.InRoom;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            State = ConnectionState.Disconnected;
            Debug.Log("Disconnected from server");
        }

        public override void OnJoinRandomFailed(short returnCode, string message) => HandleJoinError();

        private void HandleJoinError()
        {
            Debug.Log("Failed to join room - creating a new one");

            string roomCode = CreateRoomCode();
            Debug.Log($"Joining {roomCode}");
            PhotonNetwork.CreateRoom(roomCode, options, null, null);
        }

        /// <summary>
        /// Generates a random room code
        /// </summary>
        /// <returns>A room code</returns>
        public string CreateRoomCode()
        {
            return new System.Random().Next(99999).ToString();
        }
    }

    public enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,
        JoiningRoom,
        InRoom
    }
}