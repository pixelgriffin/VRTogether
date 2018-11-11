using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VRNetworkingMessages;

public class ClientManager : MonoBehaviour
{
    private static bool isListening = false;

    private NetworkClient client;

    private Dictionary<int, Transform> flies;

    private int flyID = -1;

    public GameObject flyPrefab;

    public FlyController localController;
    public Text debugText;

    public List<Transform> networkedOrientationList;

    private void Awake()
    {
        flies = new Dictionary<int, Transform>();

        TryStartClient();
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        client.Shutdown();
    }

    private void Update()
    {
        if (flyID != -1)
        {
            VRFlyMoveMessage moveMsg = new VRFlyMoveMessage();
            moveMsg.id = flyID;
            moveMsg.moving = localController.IsMoving();
            moveMsg.position = localController.transform.position;
            moveMsg.rotation = localController.transform.rotation;

            client.Send(VRMsgType.FlyMove, moveMsg);
        }
    }

    private void TryStartClient()
    {
        if (!isListening)
        {
            client = new NetworkClient();

            client.RegisterHandler(MsgType.Connect, OnConnected);
            client.RegisterHandler(VRMsgType.Orientation, OnOrientation);
            client.RegisterHandler(VRMsgType.FlyAdd, OnFlyJoined);
            client.RegisterHandler(VRMsgType.IDHandshake, OnHandshake);
            client.RegisterHandler(VRMsgType.FlyMove, OnFlyMoved);

            client.Connect("172.20.10.11", 4444);
            isListening = true;

            Log("Started Client");
        }
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Log("Connected to server");

        //We successfully connected to the server
        //So we need to tell the server to inform us of other flies
        //And tell the other flies we joined
        VRFlyAddMessage meMsg = new VRFlyAddMessage();
        meMsg.id = 0;//The server doesn't use our ID yet, it needs to generate one first

        client.Send(VRMsgType.FlyAdd, meMsg);
    }

    public void OnHandshake(NetworkMessage netMsg)
    {
        VRIDHandshake idMsg = netMsg.ReadMessage<VRIDHandshake>();
        flyID = idMsg.id;

        Log("Handshake " + idMsg.id);
    }

    public void OnOrientation(NetworkMessage netMsg)
    {
        Debug.Log("Received orientation message");

        VROrientationMessage msg = netMsg.ReadMessage<VROrientationMessage>();

        foreach (Transform t in networkedOrientationList)
        {
            if (t.name == msg.objectName)
            {
                OrientationInterpolator interp = t.GetComponent<OrientationInterpolator>();

                interp.desiredPos = msg.position;
                interp.desiredRot = msg.rotation;

                break;
            }
        }
    }

    public void OnFlyJoined(NetworkMessage netMsg)
    {
        VRFlyAddMessage msg = netMsg.ReadMessage<VRFlyAddMessage>();

        Log("A new fly joined! (" + msg.id + ")");

        //We were told by the server there's a new fly in town!
        //Create new fly prefab
        GameObject fly = Instantiate(flyPrefab, Vector3.zero, Quaternion.identity);
        flies.Add(msg.id, fly.transform);
    }

    public void OnFlyMoved(NetworkMessage netMsg)
    {
        VRFlyMoveMessage moveMsg = netMsg.ReadMessage<VRFlyMoveMessage>();
        Log("Another fly moved! (" + moveMsg.id + ")");

        //Update that fly's position and sich
        Transform fly;
        if (flies.TryGetValue(moveMsg.id, out fly))
        {
            fly.transform.position = moveMsg.position;
            fly.transform.rotation = moveMsg.rotation;
            fly.GetComponent<SlaveFly>().moving = moveMsg.moving;
        }
    }

    private void Log(string log)
    {
        Debug.Log(log);
        debugText.text = log;
    }
}
