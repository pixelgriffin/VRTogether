using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using VRNetworkingMessages;

public class ClientManager : MonoBehaviour
{
    private static bool isListening = false;

    private NetworkClient client;

    public List<Transform> networkedOrientationList;

    private void Awake()
    {
        TryStartClient();
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        client.Shutdown();
    }

    private void TryStartClient()
    {
        if (!isListening)
        {
            client = new NetworkClient();

            client.RegisterHandler(MsgType.Connect, OnConnected);
            client.RegisterHandler(VRMsgType.Orientation, OnOrientation);

            client.Connect("127.0.0.1", 4444);
            isListening = true;

            Debug.Log("Started Client");
        }
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }

    public void OnOrientation(NetworkMessage netMsg)
    {
        VROrientationMessage msg = netMsg.ReadMessage<VROrientationMessage>();

        foreach (Transform t in networkedOrientationList)
        {
            if (t.name == msg.objectName)
            {
                t.position = msg.position;
                t.rotation = msg.rotation;
                break;
            }
        }
    }
}
