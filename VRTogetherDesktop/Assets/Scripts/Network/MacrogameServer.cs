﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace VRTogether.Net
{
    public class MacrogamePlayer
    {
        public int connectionID = -1;
        public string name = "";
        public int score = 0;
    }

    public class MacrogameServer : SingletonComponent<MacrogameServer>
    {
        private static bool isListening = false;

        private List<MacrogamePlayer> macroPlayerList = new List<MacrogamePlayer>();

        private string minigameToLoad = "";

        void Awake()
        {
            base.Awake();

            TryStartServer();
            DontDestroyOnLoad(this.gameObject);
        }

        private void OnDestroy()
        {
            base.OnDestroy();

            NetworkServer.Shutdown();
        }

        private void OnLevelWasLoaded(int level)
        {
            Debug.Log("Level loaded: " + SceneManager.GetSceneByBuildIndex(level).name + " == " + minigameToLoad);
            if(SceneManager.GetSceneByBuildIndex(level).name == minigameToLoad)
            {
                Debug.Log("Minigame loaded");
                RequestClientsLoadMinigame(minigameToLoad);
                minigameToLoad = "";
            }
        }

        private bool TryStartServer()
        {
            if (!isListening)
            {
                isListening = NetworkServer.Listen(4444);

                NetworkServer.RegisterHandler(MsgType.Connect, OnPlayerConnected);
                NetworkServer.RegisterHandler(MsgType.Disconnect, OnPlayerDisconnected);
                NetworkServer.RegisterHandler(MacroMsgType.MacroClientSendPlayerName, OnReceivedPlayerName);

                if (isListening)
                    Debug.Log("Started server");
            }

            return isListening;
        }

        #region Public-facing Network Methods

        public void LoadMinigame(string sceneName)
        {
            Debug.Log("Requesting load " + sceneName);
            minigameToLoad = sceneName;
            SceneManager.LoadScene(sceneName);
        }

        private bool RequestClientsLoadMinigame(string sceneName)
        {
            Debug.Log("Attempted client load minigame");
            if(macroPlayerList.Count > 0)
            {
                StringMessage sceneMsg = new StringMessage();
                sceneMsg.str = sceneName;
                NetworkServer.SendToAll(MacroMsgType.MacroServerLoadMinigame, sceneMsg);

                Debug.Log("sent minigame load request: " + sceneMsg.str);

                return true;
            }

            return false;
        }

        public List<MacrogamePlayer> GetMacroPlayers()
        {
            return macroPlayerList;
        }

        #endregion

        #region Network Handlers

        private void OnPlayerConnected(NetworkMessage netMsg)
        {
            NetworkServer.SendToClient(netMsg.conn.connectionId, MacroMsgType.MacroServerRequestPlayerName, new EmptyMessage());
        }

        private void OnPlayerDisconnected(NetworkMessage netMsg)
        {
            MacrogamePlayer left = null;
            foreach(MacrogamePlayer player in macroPlayerList)
            {
                if(player.connectionID == netMsg.conn.connectionId)
                {
                    left = player;
                    break;
                }
            }

            //If left is not null that means the player successfully joined previously
            if (left != null)
            {
                macroPlayerList.Remove(left);

                StringMessage strMsg = new StringMessage();
                strMsg.str = left.name;
                NetworkServer.SendToAll(MacroMsgType.MacroServerPlayerLeft, strMsg);
            }
        }

        private void OnReceivedPlayerName(NetworkMessage netMsg)
        {
            StringMessage strMsg = netMsg.ReadMessage<StringMessage>();

            foreach(MacrogamePlayer macroPlayer in macroPlayerList)
            {
                if(macroPlayer.name == strMsg.str)
                {
                    NetworkServer.SendToClient(netMsg.conn.connectionId, MacroMsgType.MacroServerRejectPlayerName, new EmptyMessage());
                    return;
                }
            }

            //If we get this far the name is unique and we can confirm their join and broadcast the name to all involved
            MacrogamePlayer player = new MacrogamePlayer();
            player.connectionID = netMsg.conn.connectionId;
            player.name = strMsg.str;
            player.score = 0;

            macroPlayerList.Add(player);//Keep track of the player's statistics on the server

            StringMessage acceptedMsg = new StringMessage();
            acceptedMsg.str = strMsg.str;
            NetworkServer.SendToAll(MacroMsgType.MacroServerPlayerJoined, acceptedMsg);
        }

        private void SendToAllBut(int connectionID, short msgType, MessageBase msg)
        {
            foreach (NetworkConnection conn in NetworkServer.connections)
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

        #endregion
    }
}