using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace VRNetworkingMessages
{
    public class VRMsgType
    {
        public static short Orientation = MsgType.Highest + 1;
        public static short FlyRotation = MsgType.Highest + 2;
        public static short FlyMove = MsgType.Highest + 3;
    }

    public class VROrientationMessage : MessageBase
    {
        public string objectName;
        public Vector3 position;
        public Quaternion rotation;
    }

    public class VRFlyRotationMessage : MessageBase
    {
        public Quaternion rotation;
    }

    public class VRFlyMoveMessage : MessageBase
    {
        public bool moving;
    }
}