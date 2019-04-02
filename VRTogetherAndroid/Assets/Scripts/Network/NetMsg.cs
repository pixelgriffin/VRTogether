using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace VRTogether.Net
{
    public class MacroMsgType
    {
        public static short MacroServerPlayerJoined = MsgType.Highest + 1;
        public static short MacroServerPlayerLeft = MsgType.Highest + 2;
        public static short MacroServerPlayerScoreChanged = MsgType.Highest + 3;
        public static short MacroServerGameOver = MsgType.Highest + 4;
        public static short MacroServerLoadMinigame = MsgType.Highest + 5;
        public static short MacroServerStartMinigame = MsgType.Highest + 6;
        public static short MacroServerRequestPlayerName = MsgType.Highest + 7;
        public static short MacroServerRejectPlayerName = MsgType.Highest + 8;

        public static short MacroClientSendPlayerName = MsgType.Highest + 9;
        public static short MacroClientMinigameReady = MsgType.Highest + 10;

        public static short ObjectOrientationChange = MsgType.Highest + 11;
        public static short ObjectInstantiate = MsgType.Highest + 12;
        public static short ObjectDestroy = MsgType.Highest + 13;

        public const short Highest = MsgType.Highest + 18;

        public static short MacroServerSendPlayerName = MsgType.Highest + 15;
        public static short MacroClientRequestNameList = MsgType.Highest + 16;
        public static short MacroClientRequestScore = MsgType.Highest + 17;
        public static short MacroServerSendScore = MsgType.Highest + 18;
    }

    public class MiniMsgType
    {
        public static short MiniInstantiateObject = MacroMsgType.Highest + 1;
        public static short MiniRequestInstantiateObject = MacroMsgType.Highest + 10;
        public static short MiniDestroyObject = MacroMsgType.Highest + 2;
        public static short MiniSyncOrientation = MacroMsgType.Highest + 3;
        public static short MiniOtherPlayersReady = MacroMsgType.Highest + 4;
        public static short MiniRequestDestroySlave = MacroMsgType.Highest + 5;

        public static short MiniBoolVar = MacroMsgType.Highest + 6;
        public static short MiniIntVar = MacroMsgType.Highest + 7;
        public static short MiniFloatVar = MacroMsgType.Highest + 8;

        public static short MiniEndGame = MacroMsgType.Highest + 9;

    }

    public class EmptyMessage : MessageBase
    {
    }

    public class StringMessage : MessageBase
    {
        public string str;
    }

    public class SlaveInstantiateMessage : MessageBase
    {
        public string objectName;
        public string networkID;
    }

    public class ObjectInstantiateMessage : MessageBase
    {
        public string objectName;
        public Vector3 pos;
        public Quaternion rot;
    }

    public class SlaveOrientMessage : MessageBase
    {
        public string networkID;
        public Vector3 loc;
        public Quaternion rot;
        public Vector3 scale;
    }

    public class BooleanVarMessage : MessageBase
    {
        public string networkID;
        public string varName;
        public bool value;
    }

    public class IntegerVarMessage : MessageBase
    {
        public string networkID;
        public string varName;
        public int value;
    }

    public class FloatVarMessage : MessageBase
    {
        public string networkID;
        public string varName;
        public float value;
    }

    public class StringVarMessage : MessageBase
    {
        public string networkID;
        public string varName;
        public string value;
    }

    public class ScoreMessage : MessageBase
    {
        public string name;
        public bool isVrScore;
        public int score;
    }
}
