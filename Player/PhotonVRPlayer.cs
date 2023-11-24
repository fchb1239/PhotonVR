using System;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using TMPro;

namespace Photon.VR.Player
{
    public class PhotonVRPlayer : MonoBehaviourPun
    {
        [Header("Objects")]
        public Transform Head;
        public Transform Body;
        public Transform LeftHand;
        public Transform RightHand;
        [Tooltip("The objects that will get the colour of the player applied to them")]
        public List<MeshRenderer> ColourObjects;

        [Space]
        [Tooltip("Feel free to add as many slots as you feel necessary")]
        public List<CosmeticSlot> CosmeticSlots = new List<CosmeticSlot>();

        [Header("Other")]
        public TextMeshPro NameText;
        public bool HideLocalPlayer = true;
        private bool Offline;

        private void Awake()
        {
            Offline = PhotonVRManager.Manager.Offline;
            if (photonView.IsMine && !Offline)
            {
                PhotonVRManager.Manager.LocalPlayer = this;
                if (HideLocalPlayer)
                {
                    Head.gameObject.SetActive(false);
                    Body.gameObject.SetActive(false);
                    RightHand.gameObject.SetActive(false);
                    LeftHand.gameObject.SetActive(false);
                    NameText.gameObject.SetActive(false);
                }
            }
            else if (Offline){
                photonView.enabled = false;

            }
            
            // It will delete automatically when you leave the room
            DontDestroyOnLoad(gameObject);

            _RefreshPlayerValues();
        }

        private void Update()
        {
            Offline = PhotonVRManager.Manager.Offline;
            if (photonView.IsMine || Offline)
            {
                Head.transform.position = PhotonVRManager.Manager.Head.transform.position;
                Head.transform.rotation = PhotonVRManager.Manager.Head.transform.rotation;

                RightHand.transform.position = PhotonVRManager.Manager.RightHand.transform.position;
                RightHand.transform.rotation = PhotonVRManager.Manager.RightHand.transform.rotation;

                LeftHand.transform.position = PhotonVRManager.Manager.LeftHand.transform.position;
                LeftHand.transform.rotation = PhotonVRManager.Manager.LeftHand.transform.rotation;
            }
        }

        public void RefreshPlayerValues() => photonView.RPC("RPCRefreshPlayerValues", RpcTarget.All);

        [PunRPC]
        private void RPCRefreshPlayerValues()
        {
            _RefreshPlayerValues();
        }

        private void _RefreshPlayerValues()
        {
            // Name
            if (NameText != null && !Offline) {
                NameText.text = photonView.Owner.NickName;
            }
            else if (Offline){
                NameText.text = PlayerPrefs.GetString("Username", null);
            }
            // Colour
            foreach (MeshRenderer renderer in ColourObjects)
            {
                if(renderer != null)
                    if(!Offline)
                    renderer.material.color = JsonUtility.FromJson<Color>((string)photonView.Owner.CustomProperties["Colour"]);
                    else
                    renderer.material.color = PhotonVRManager.Manager.Colour;
            }

            // Cosmetics - it's a little ugly to look at
            Dictionary<string, string> cosmetics;
            if (!Offline){
                cosmetics = (Dictionary<string, string>)photonView.Owner.CustomProperties["Cosmetics"];
            }
            else{
                cosmetics = PhotonVRManager.Manager.Cosmetics;
            }
            foreach (KeyValuePair<string, string> cosmetic in cosmetics)
            {
                Debug.Log(cosmetic.Key);
                foreach (CosmeticSlot slot in CosmeticSlots)
                {
                    if (slot.SlotName == cosmetic.Key)
                    {
                        foreach (Transform cos in slot.Object)
                            if (cos.name != cosmetic.Value)
                                cos.gameObject.SetActive(false);
                            else
                                cos.gameObject.SetActive(true);
                    }
                }
            }
        }

        [Serializable]
        public class CosmeticSlot
        {
            public string SlotName;
            public Transform Object;
        }
    }
}