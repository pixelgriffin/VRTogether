using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace VRTogether.Net
{
    public class MacrogameClient : SingletonComponent<MacrogameClient> {

        public string playerName = "Player";

        public LobbyManager lobbyManager = null;

        private bool isListening = false;

        private NetworkClient client;

        private string minigameSceneToLoad = "";

        private void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this.gameObject);
        }

        public string GetMinigameSceneToLoad()
        {
            return minigameSceneToLoad;
        }

        public void ClearMinigameSceneToLoad()
        {
            minigameSceneToLoad = "";
        }

        public void AttemptConnection(string ip)
        {
            if (!isListening)
            {
                client = new NetworkClient();

                client.RegisterHandler(MacroMsgType.MacroServerRequestPlayerName, OnServerRequestsName);
                client.RegisterHandler(MacroMsgType.MacroServerRejectPlayerName, OnServerRejectedName);
                client.RegisterHandler(MacroMsgType.MacroServerPlayerJoined, OnPlayerJoined);
                client.RegisterHandler(MacroMsgType.MacroServerLoadMinigame, OnServerLoadMinigame);
                client.RegisterHandler(MacroMsgType.MacroServerStartMinigame, OnMinigameStarted);
                client.RegisterHandler(MacroMsgType.MacroServerSendPlayerName, OnReceivedPlayerName);

                client.Connect(ip, 4444);
                isListening = true;
            }
        }

        public NetworkClient GetClient()
        {
            return client;
        }

        private void OnServerRequestsName(NetworkMessage msg)
        {
            StringMessage nameMsg = new StringMessage();
            nameMsg.str = playerName;

            client.Send(MacroMsgType.MacroClientSendPlayerName, nameMsg);
        }

        private void OnPlayerJoined(NetworkMessage msg)
        { 

            StringMessage nameMsg = msg.ReadMessage<StringMessage>();

            Debug.Log("Player " + nameMsg.str + " Joined");

            if (nameMsg.str == playerName)
            {
                //We were accepted by the server
                if (lobbyManager)
                {
                    lobbyManager.SwitchToLobby();

                }
            }
            else
            {
                //Another player has joined
            }

            lobbyManager.AddPlayerNameToPlayerList(nameMsg.str);

        }

        private void OnServerRejectedName(NetworkMessage msg)
        {
            //We need to try again with a new name
            client.Shutdown();
            client = null;
            isListening = false;

            lobbyManager.EnableError("Name taken, please pick a new name in Options.");
        }

        private void OnServerLoadMinigame(NetworkMessage msg)
        {
            Debug.Log("Loading minigame scene: " + msg);
            StringMessage sceneMsg = msg.ReadMessage<StringMessage>();

            minigameSceneToLoad = sceneMsg.str;
            SceneManager.LoadScene(sceneMsg.str);
        }

        private void OnMinigameStarted(NetworkMessage msg)
        {
        }

        private void OnReceivedPlayerName(NetworkMessage msg)
        {
            StringMessage nameMsg = msg.ReadMessage<StringMessage>();
            lobbyManager.AddPlayerNameToPlayerList(nameMsg.str);
        }
    }
}