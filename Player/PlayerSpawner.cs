using UnityEngine;

using Photon.Pun;

namespace Photon.VR.Player
{
    public class PlayerSpawner : MonoBehaviourPunCallbacks
    {
        [Tooltip("The location of the player prefab")]
        public string PrefabLocation = "PhotonVR/Player";
        private GameObject playerTemp;

        private void Awake() => DontDestroyOnLoad(gameObject);

        public override void OnJoinedRoom()
        {
            playerTemp = PhotonNetwork.Instantiate(PrefabLocation, Vector3.zero, Quaternion.identity);
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Destroy(playerTemp);
        }
    }

}