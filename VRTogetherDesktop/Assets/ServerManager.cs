using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using VRNetworkingMessages;

public class ServerManager : MonoBehaviour {

    private static bool isListening = false;

    public List<Transform> networkedOrientationList;

    private void Awake()
    {
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

            if(isListening)
                Debug.Log("Started server");
        }

        return isListening;
    }
}
