using System;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using TMPro;

using Photon.VR.Cosmetics;

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

        [Header("Cosmetics Parents")]
        public Transform HeadCosmetics;
        public Transform FaceCosmetics;
        public Transform BodyCosmetics;
        public Transform LeftHandCosmetics;
        public Transform RightHandCosmetics;

        [Header("Other")]
        public TextMeshPro NameText;
        public bool HideLocalPlayer = true;

        private void Awake()
        {
            if (photonView.IsMine)
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

            // It will delete automatically when you leave the room
            DontDestroyOnLoad(gameObject);

            _RefreshPlayerValues();
        }

        private void Update()
        {
            if (photonView.IsMine)
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
            if (NameText != null)
                NameText.text = photonView.Owner.NickName;

            // Colour
            foreach (MeshRenderer renderer in ColourObjects)
            {
                renderer.material.color = JsonUtility.FromJson<Color>((string)photonView.Owner.CustomProperties["Colour"]);
            }

            // Cosmetics - it's a little ugly to look at
            PhotonVRCosmeticsData cosmetics = JsonUtility.FromJson<PhotonVRCosmeticsData>((string)photonView.Owner.CustomProperties["Cosmetics"]);
            if(HeadCosmetics != null)
                foreach (Transform cos in HeadCosmetics)
                    if (cos.name != cosmetics.Head)
                        cos.gameObject.SetActive(false);
                    else
                        cos.gameObject.SetActive(true);
            if (BodyCosmetics != null)
                foreach (Transform cos in BodyCosmetics.transform)
                    if (cos.name != cosmetics.Body)
                        cos.gameObject.SetActive(false);
                    else
                        cos.gameObject.SetActive(true);
            if (FaceCosmetics != null)
                foreach (Transform cos in FaceCosmetics.transform)
                    if (cos.name != cosmetics.Face)
                        cos.gameObject.SetActive(false);
                    else
                        cos.gameObject.SetActive(true);
            if (LeftHandCosmetics != null)
                foreach (Transform cos in LeftHandCosmetics.transform)
                    if (cos.name != cosmetics.LeftHand)
                        cos.gameObject.SetActive(false);
                    else
                        cos.gameObject.SetActive(true);
            if (RightHandCosmetics != null)
                foreach (Transform cos in RightHandCosmetics.transform)
                    if (cos.name != cosmetics.RightHand)
                        cos.gameObject.SetActive(false);
                    else
                        cos.gameObject.SetActive(true);
        }
    }
}