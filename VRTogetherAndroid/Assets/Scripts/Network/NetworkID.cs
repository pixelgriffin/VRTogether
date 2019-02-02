using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTogether.Net
{
    public class NetworkID : MonoBehaviour
    {
        public string netID = null;

        void Start()
        {
            //netID = System.Guid.NewGuid().ToString();
        }
    }
}