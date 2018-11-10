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

    public Text debugText;

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

            client.Connect("172.20.10.11", 4444);
            isListening = true;

            Log("Started Client");
        }
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Log("Connected to server");
    }

    public void OnOrientation(NetworkMessage netMsg)
    {
        Log("Received orientation message");

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

    private void Log(string log)
    {
        Debug.Log(log);
        debugText.text = log;
    }
}
