using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.VR.Saving
{
    public class PhotonVRValueSaver : MonoBehaviour
    {
        public static void SaveDictionary(string location, Dictionary<string, string> value)
        {
            PlayerPrefs.SetString(location, string.Join(",", value.Keys));
            foreach (KeyValuePair<string, string> kv in value)
            {
                PlayerPrefs.SetString(location + kv.Key, kv.Value.ToString());
            }
        }

        public static Dictionary<string, string> GetDictionary(string location)
        {
            string[] locations = PlayerPrefs.GetString(location).Split(',');
            Dictionary<string, string> value = new Dictionary<string, string>();
            foreach (string str in locations)
            {
                value[str] = PlayerPrefs.GetString(location + str);
            }
            return value;
        }
    }
}