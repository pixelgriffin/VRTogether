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
        public static short FlyAdd = MsgType.Highest + 4;
        public static short IDHandshake = MsgType.Highest + 5;
        public static short GameStart = MsgType.Highest + 6;
        public static short FlyGrapeInfo = MsgType.Highest + 7;
        public static short FlySwatted = MsgType.Highest + 8;
        public static short GameOver = MsgType.Highest + 9;
    }

    public class VRGameOverMessage : MessageBase
    {
        public bool fliesWon;
    }

    public class VRGameStartMessage : MessageBase
    {
    }

    public class VRFlyGrapeMessage : MessageBase
    {
        public int id;
        public bool holdingGrape;
    }

    public class VRFlySwattedMessage : MessageBase
    {
        public int id;
    }

    public class VROrientationMessage : MessageBase
    {
        public string objectName;
        public Vector3 position;
        public Quaternion rotation;
    }

    public class VRFlyMoveMessage : MessageBase
    {
        public int id;
        public bool moving;
        public Vector3 position;
        public Quaternion rotation;
    }

    public class VRFlyAddMessage : MessageBase
    {
        public int id;
    }

    public class VRIDHandshake : MessageBase
    {
        public int id;
    }
}