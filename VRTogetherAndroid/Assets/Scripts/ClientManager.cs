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

    private Dictionary<int, SlaveFly> flies;

    private int flyID = -1;

    public GameObject flyPrefab;

    public GameObject spectatorCamera;

    public FlyController localController;
    public Text debugText;
    public Text scoreText;
    public Text bigText;

    private int flyScore = 0;

    public List<Transform> networkedOrientationList;
    public List<Transform> flySpawns;

    private void Awake()
    {
        flies = new Dictionary<int, SlaveFly>();

        //TryStartClient();
        //SwapToSpectator();

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

        scoreText.text = flyScore + "/6 grapes stolen";
    }

    public void TryStartClient(string ip)
    {
        if (!isListening)
        {
            Debug.Log("Not listening, creating client");

            client = new NetworkClient();

            client.RegisterHandler(MsgType.Connect, OnConnected);
            client.RegisterHandler(VRMsgType.Orientation, OnOrientation);
            client.RegisterHandler(VRMsgType.FlyAdd, OnFlyJoined);
            client.RegisterHandler(VRMsgType.IDHandshake, OnHandshake);
            client.RegisterHandler(VRMsgType.FlyMove, OnFlyMoved);
            client.RegisterHandler(VRMsgType.FlySwatted, OnFlySwatted);
            client.RegisterHandler(VRMsgType.FlyGrapeInfo, OnFlyGrapeChanged);
            client.RegisterHandler(VRMsgType.FlySwatted, OnFlySwatted);

            client.RegisterHandler(VRMsgType.GameOver, OnGameOver);
            client.RegisterHandler(VRMsgType.GameStart, OnGameStart);

            client.Connect(ip, 4444);//"172.20.10.11"
            isListening = true;

            Log("Started Client");
        }
    }

    public void OnGameStart(NetworkMessage netMsg)
    {
        Transform spawnPoint;
        int index = Random.Range(0, flySpawns.Count);
        spawnPoint = flySpawns[index];

        localController.transform.position = spawnPoint.position;
        //localController.transform.rotation = spawnPoint.rotation;

        SwapToFlyView();
        bigText.enabled = false;
    }

    public void OnGameOver(NetworkMessage netMsg)
    {
        VRGameOverMessage msg = netMsg.ReadMessage<VRGameOverMessage>();

        if(msg.fliesWon)
        {
            bigText.text = "THE FLIES HAVE\nWON".Replace("\\n","\n");
        }
        else
        {
            bigText.text = "THE EXTERMINATOR\nHAS WON".Replace("\\n","\n");
        }

        SwapToSpectator();
    }

    public void OnFlyGrapeChanged(NetworkMessage netMsg)
    {
        VRFlyGrapeMessage msg = netMsg.ReadMessage<VRFlyGrapeMessage>();

        SlaveFly fly;
        if(flies.TryGetValue(msg.id, out fly))
        {
            if(msg.holdingGrape)
            {
                fly.PickupGrape();
            }
            else
            {
                fly.DropGrape();
                flyScore++;
            }
        }
    }

    public void PickupGrapeOverNetwork()
    {
        if (!localController.IsHoldingGrape())
        {
            Log("Picked up grape over network!");
            VRFlyGrapeMessage msg = new VRFlyGrapeMessage();
            msg.id = flyID;
            msg.holdingGrape = true;

            client.Send(VRMsgType.FlyGrapeInfo, msg);

            localController.HoldGrape();
        }
    }

    public void DropGrapeOverNetwork()
    {
        if (localController.IsHoldingGrape())
        {
            VRFlyGrapeMessage msg = new VRFlyGrapeMessage();
            msg.id = flyID;
            msg.holdingGrape = false;

            client.Send(VRMsgType.FlyGrapeInfo, msg);

            localController.DropGrape();
            flyScore++;
        }
    }

    private void SwapToSpectator()
    {
        spectatorCamera.SetActive(true);
        localController.gameObject.SetActive(false);
        bigText.gameObject.SetActive(true);
    }

    private void SwapToFlyView()
    {
        spectatorCamera.SetActive(false);
        localController.gameObject.SetActive(true);
    }

    public void OnFlySwatted(NetworkMessage netMsg)
    {
        VRFlySwattedMessage msg = netMsg.ReadMessage<VRFlySwattedMessage>();

        Log("swatted " + msg.id);

        if(msg.id == flyID)
        {
            //Move us to spectator camera
            bigText.text = "YOU GOT SWATTED";
            SwapToSpectator();
        }
        else
        {
            flies[msg.id].gameObject.SetActive(false);
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
        flies.Add(msg.id, fly.GetComponent<SlaveFly>());
    }

    public void OnFlyMoved(NetworkMessage netMsg)
    {
        VRFlyMoveMessage moveMsg = netMsg.ReadMessage<VRFlyMoveMessage>();
        Log("Another fly moved! (" + moveMsg.id + ")");

        //Update that fly's position and sich
        SlaveFly fly;
        if (flies.TryGetValue(moveMsg.id, out fly))
        {
            fly.transform.position = moveMsg.position;
            fly.transform.rotation = moveMsg.rotation;
            fly.moving = moveMsg.moving;
        }
    }

    private void Log(string log)
    {
        Debug.Log(log);
        debugText.text = log;
    }
}
