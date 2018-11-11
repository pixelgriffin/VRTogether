using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using VRNetworkingMessages;

public class ServerManager : MonoBehaviour {

    private static bool isListening = false;

    public List<Transform> networkedOrientationList;
    public GameObject flyPrefab;

    private Dictionary<int, Transform> flies;

    private int flyIDCounter = 0;

    private void Awake()
    {
        flies = new Dictionary<int, Transform>();

        TryStartServer();
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        NetworkServer.Shutdown();
    }

    void Update () {
        if (isListening)
        {
            foreach (Transform t in networkedOrientationList)
            {
                VROrientationMessage orientationMessage = new VROrientationMessage();
                orientationMessage.position = t.position;
                orientationMessage.rotation = t.rotation;
                orientationMessage.objectName = t.name;

                NetworkServer.SendToAll(VRMsgType.Orientation, orientationMessage);
            }
        }
	}

    private bool TryStartServer()
    {
        if (!isListening)
        {
            isListening = NetworkServer.Listen(4444);
            NetworkServer.RegisterHandler(MsgType.Connect, OnOtherConnected);
            NetworkServer.RegisterHandler(VRMsgType.FlyAdd, OnFlyJoined);
            NetworkServer.RegisterHandler(VRMsgType.FlyMove, OnFlyMoved);

            if(isListening)
                Debug.Log("Started server");
        }

        return isListening;
    }

    public void OnOtherConnected(NetworkMessage msg)
    {
        Debug.Log("???????????????/");
    }

    public void OnFlyJoined(NetworkMessage netMsg)
    {
        //Inform all the other flies that a new fly has joined
        VRFlyAddMessage addMsg = new VRFlyAddMessage();
        addMsg.id = flyIDCounter;

        Debug.Log("Fly (" + addMsg.id + ") has joined!");

        SendToAllBut(netMsg.conn.connectionId, VRMsgType.FlyAdd, addMsg);

        //Infrom this new fly of its ID
        VRIDHandshake handshakeMsg = new VRIDHandshake();
        handshakeMsg.id = flyIDCounter;

        NetworkServer.SendToClient(netMsg.conn.connectionId, VRMsgType.IDHandshake, handshakeMsg);

        //Inform this fly of all other existing flies
        foreach(int id in flies.Keys)
        {
            VRFlyAddMessage addOtherMsg = new VRFlyAddMessage();
            addOtherMsg.id = id;

            netMsg.conn.Send(VRMsgType.FlyAdd, addOtherMsg);
        }

        //Add new fly to the list of our existing flies
        GameObject fly = Instantiate(flyPrefab, Vector3.zero, Quaternion.identity);
        flies.Add(flyIDCounter, fly.transform);

        flyIDCounter++;
    }

    public void OnFlyMoved(NetworkMessage netMsg)
    {
        VRFlyMoveMessage moveMsg = netMsg.ReadMessage<VRFlyMoveMessage>();

        Debug.Log("ID: " + moveMsg.id);

        //Update this fly's position and sich
        Transform fly;
        if(flies.TryGetValue(moveMsg.id, out fly))
        {
            fly.transform.position = moveMsg.position;
            fly.transform.rotation = moveMsg.rotation;
            fly.GetComponent<SlaveFly>().moving = moveMsg.moving;
        }

        //Inform all the other flies!
        SendToAllBut(netMsg.conn.connectionId, VRMsgType.FlyMove, moveMsg);
    }

    public void SendToAllBut(int connectionID, short msgType, MessageBase msg)
    {
        foreach(NetworkConnection conn in NetworkServer.connections)
        {
            if (conn != null)
            {
                if (conn.connectionId != connectionID)
                {
                    conn.Send(msgType, msg);
                }
            }
        }
    }
}
