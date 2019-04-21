using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace VRTogether.Net
{
    [System.Serializable]
    public class StringEvent : UnityEvent<string>
    {
    }

    public class MacrogameClient : SingletonComponent<MacrogameClient> {

        public string playerName = "Player";

        [HideInInspector]
        public UnityEvent OnNameRejected = new UnityEvent();
        [HideInInspector]
        public StringEvent OnWeJoinedServer = new StringEvent();
        [HideInInspector]
        public StringEvent OnOtherPlayerJoined = new StringEvent();
        [HideInInspector]
        public StringEvent OnPlayerNameReceived = new StringEvent();
        [HideInInspector]
        public StringEvent OnPlayerLeftServer = new StringEvent();


        private Dictionary<string, int> playerScores = new Dictionary<string, int>();
        private int vrScore = 0;

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

        public bool IsConnected()
        {
            return (client != null && client.isConnected);
        }

        public void AttemptConnection(string ip)
        {
            if (!isListening)
            {
                client = new NetworkClient();

                client.RegisterHandler(MacroMsgType.MacroServerRequestPlayerName, OnServerRequestsName);
                client.RegisterHandler(MacroMsgType.MacroServerRejectPlayerName, OnServerRejectedName);
                client.RegisterHandler(MacroMsgType.MacroServerPlayerJoined, OnPlayerJoined);
                client.RegisterHandler(MacroMsgType.MacroServerPlayerLeft, OnPlayerLeft);
                client.RegisterHandler(MacroMsgType.MacroServerLoadMinigame, OnServerLoadMinigame);
                client.RegisterHandler(MacroMsgType.MacroServerStartMinigame, OnMinigameStarted);
                client.RegisterHandler(MacroMsgType.MacroServerSendPlayerName, OnReceivedPlayerName);
                client.RegisterHandler(MacroMsgType.MacroServerSendScore, OnReceivedScore);

                client.Connect(ip, 4444);
                isListening = true;
            }
        }

        public void Disconnect()
        {
            client.Shutdown();

            isListening = false;
        }

        public void RequestNameList()
        {
            client.Send(MacroMsgType.MacroClientRequestNameList, new EmptyMessage());
        }

        public void RequestScoreUpdate()
        {
            client.Send(MacroMsgType.MacroClientRequestScore, new EmptyMessage());
        }

        public int GetVRScore()
        {
            return vrScore;
        }

        public List<string> GetPlayerNames()
        {
            return new List<string>(playerScores.Keys);
        }
         
        public int GetPlayerScore(string name)
        {
            int val = 0;
            if(playerScores.TryGetValue(name, out val))
            {
                return val;
            }
            else
            {
                return 0;
            }
        }

        public NetworkClient GetClient()
        {
            return client;
        }

        private void OnReceivedScore(NetworkMessage msg)
        {
            ScoreMessage scoreMsg = msg.ReadMessage<ScoreMessage>();

            if (scoreMsg.isVrScore)
            {
                vrScore = scoreMsg.score;
            }
            else
            {
                if (playerScores.ContainsKey(scoreMsg.name))
                {
                    playerScores[scoreMsg.name] = scoreMsg.score;
                }
                else
                {
                    playerScores.Add(scoreMsg.name, scoreMsg.score);
                }
            }
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
                OnWeJoinedServer.Invoke(nameMsg.str);
            }
            else
            {
                //Another player has joined
                OnOtherPlayerJoined.Invoke(nameMsg.str);
            }
        }

        private void OnPlayerLeft(NetworkMessage msg)
        {
            //lobbyManager.ClearPlayerList();
            StringMessage nameMsg = msg.ReadMessage<StringMessage>();

            playerScores.Remove(nameMsg.str);

            OnPlayerLeftServer.Invoke(nameMsg.str);
        }

        private void OnServerRejectedName(NetworkMessage msg)
        {
            //We need to try again with a new name
            client.Shutdown();
            client = null;
            isListening = false;

            OnNameRejected.Invoke();
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

            OnPlayerNameReceived.Invoke(nameMsg.str);
        }
    }
}