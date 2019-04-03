using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace VRTogether.Net
{
    public class IntegerEvent : UnityEvent<int>
    {
    }

    public class MinigameServer : SingletonComponent<MinigameServer>
    {
        public NetworkedPrefabList networkedPrefabs;

        private List<int> readyPlayers = new List<int>();

        private Dictionary<string, NetworkVariable> vars = new Dictionary<string, NetworkVariable>();

        private bool playersReady = false;
        private bool gameStarted = false;

        [HideInInspector]
        public IntegerEvent OnClientJoined = new IntegerEvent();
        [HideInInspector]
        public UnityEvent OnAllClientsReady = new UnityEvent();

        private void Start()
        {
            NetworkServer.RegisterHandler(MacroMsgType.MacroClientMinigameReady, OnClientReady);
            NetworkServer.RegisterHandler(MiniMsgType.MiniInstantiateObject, OnSlaveInstantiateRequested);
            NetworkServer.RegisterHandler(MiniMsgType.MiniDestroyObject, OnSlaveDestroyRequested);
            NetworkServer.RegisterHandler(MiniMsgType.MiniSyncOrientation, OnSlaveOrient);
            NetworkServer.RegisterHandler(MiniMsgType.MiniRequestDestroySlave, OnNetDestroyRequested);

            NetworkServer.RegisterHandler(MiniMsgType.MiniBoolVar, OnBooleanVariableReceived);
            NetworkServer.RegisterHandler(MiniMsgType.MiniIntVar, OnIntegerVariableReceived);
            NetworkServer.RegisterHandler(MiniMsgType.MiniFloatVar, OnFloatVariableReceived);

        }

        private void Update()
        {
            //Wait until all players have loaded the minigame
            if (!playersReady)
            {
                if (MacrogameServer.Instance.GetMacroPlayers().Count > 0)
                {
                    bool allPlayersReady = true;
                    foreach (MacrogamePlayer p in MacrogameServer.Instance.GetMacroPlayers())
                    {
                        if (!readyPlayers.Contains(p.connectionID))
                        {
                            allPlayersReady = false;
                            break;
                        }
                    }

                    if (allPlayersReady)
                    {
                        playersReady = true;
                        NetworkServer.SendToAll(MiniMsgType.MiniOtherPlayersReady, new EmptyMessage());
                        OnAllClientsReady.Invoke();
                    }
                }
            }
            else
            {
                foreach(NetworkID id in networkedPrefabs.GetNetworkedObjects())
                {
                    Transform t = networkedPrefabs.GetNetworkedObject(id.netID).transform;
                    SlaveOrientMessage orient = new SlaveOrientMessage();
                    orient.networkID = id.netID;
                    orient.loc = t.position;
                    orient.rot = t.rotation;
                    orient.scale = t.localScale;

                    NetworkServer.SendToAll(MiniMsgType.MiniSyncOrientation, orient);
                }
            }
        }

        public void RegisterVariable(string id, NetworkVariable var)
        {
            var.objID = id;
            vars.Add(var.objID + "-" + var.name, var);
        }

        public void UnregisterVariable(NetworkVariable var)
        {
            vars.Remove(var.objID + "-" + var.name);
        }

        public GameObject NetworkInstantiate(GameObject prefab)
        {
            if(networkedPrefabs.prefabs.Contains(prefab))
            {
                GameObject obj = networkedPrefabs.NetworkInstantiate(prefab);

                SlaveInstantiateMessage newSlaveMsg = new SlaveInstantiateMessage();
                newSlaveMsg.networkID = obj.GetComponent<NetworkID>().netID;
                newSlaveMsg.objectName = prefab.name;

                NetworkServer.SendToAll(MiniMsgType.MiniInstantiateObject, newSlaveMsg);

                return obj;
            }

            return null;
        }

        public void NetworkRequestInstantiate(GameObject prefab, Vector3 pos, Quaternion rot, int connectionID)
        {
            if(networkedPrefabs.prefabs.Contains(prefab))
            {
                ObjectInstantiateMessage newObjectMsg = new ObjectInstantiateMessage();
                newObjectMsg.objectName = prefab.name;
                newObjectMsg.pos = pos;
                newObjectMsg.rot = rot;

                NetworkServer.SendToClient(connectionID, MiniMsgType.MiniRequestInstantiateObject, newObjectMsg);
            }
        }

        public bool NetworkDestroy(GameObject netObj)
        {
            if (netObj.GetComponent<NetworkID>() != null)
            {
                if (!networkedPrefabs.IsSlave(netObj.GetComponent<NetworkID>().netID))
                {
                    networkedPrefabs.NetworkDestroy(netObj);

                    StringMessage destroySlaveMsg = new StringMessage();
                    destroySlaveMsg.str = netObj.GetComponent<NetworkID>().netID;

                    NetworkServer.SendToAll(MiniMsgType.MiniDestroyObject, destroySlaveMsg);

                    return true;
                }
                else
                {
                    Debug.Log("Requesting owner to destroy slave");
                    StringMessage idMsg = new StringMessage();
                    idMsg.str = netObj.GetComponent<NetworkID>().netID;

                    NetworkServer.SendToAll(MiniMsgType.MiniRequestDestroySlave, idMsg);
                }
            }

            return false;
        }

        public bool AllPlayersReady()
        {
            return playersReady;
        }

        public bool MinigameStarted()
        {
            return gameStarted;
        }

        public void StartMinigame()
        {
            if (AllPlayersReady())
            {
                NetworkServer.SendToAll(MacroMsgType.MacroServerStartMinigame, new EmptyMessage());
                gameStarted = true;
            }
        }

        public void EndGame(string returnScene, bool vrWon, int score)
        {
            if(AllPlayersReady())
            {
                StringMessage msg = new StringMessage();
                msg.str = returnScene;

                networkedPrefabs.Shutdown();
                NetworkServer.SendToAll(MiniMsgType.MiniEndGame, msg);

                MacrogameServer.Instance.AddScore(vrWon, score);

                SceneManager.LoadScene(returnScene);
            }

        }

        public void SendBooleanToAll(NetworkBool netBool)
        {
            BooleanVarMessage boolMsg = new BooleanVarMessage();
            boolMsg.networkID = netBool.objID;
            boolMsg.varName = netBool.name;
            boolMsg.value = netBool.value;

            NetworkServer.SendToAll(MiniMsgType.MiniBoolVar, boolMsg);
        }

        public void SendIntegerToAll(NetworkInt netInt)
        {
            IntegerVarMessage intMsg = new IntegerVarMessage();
            intMsg.networkID = netInt.objID;
            intMsg.varName = netInt.name;
            intMsg.value = netInt.value;

            NetworkServer.SendToAll(MiniMsgType.MiniIntVar, intMsg);
        }

        public void SendFloatToAll(NetworkFloat netFloat)
        {
            FloatVarMessage floatMsg = new FloatVarMessage();
            floatMsg.networkID = netFloat.objID;
            floatMsg.varName = netFloat.name;
            floatMsg.value = netFloat.value;

            NetworkServer.SendToAll(MiniMsgType.MiniFloatVar, floatMsg);
        }

        private void OnClientReady(NetworkMessage netMsg)
        {
            if (!readyPlayers.Contains(netMsg.conn.connectionId))
            {
                readyPlayers.Add(netMsg.conn.connectionId);
                OnClientJoined.Invoke(netMsg.conn.connectionId);
            }
        }

        private void OnSlaveInstantiateRequested(NetworkMessage msg)
        {
            SlaveInstantiateMessage newSlaveMsg = msg.ReadMessage<SlaveInstantiateMessage>();
            networkedPrefabs.NetworkSlave(newSlaveMsg.objectName, newSlaveMsg.networkID);

            //Forward the update message to all other clients
            SendToAllBut(msg.conn.connectionId, MiniMsgType.MiniInstantiateObject, newSlaveMsg);

            Debug.Log("instantiated slave: " + newSlaveMsg.objectName);
        }

        private void OnSlaveDestroyRequested(NetworkMessage msg)
        {
            StringMessage destroySlaveMsg = msg.ReadMessage<StringMessage>();
            networkedPrefabs.NetworkUnslave(destroySlaveMsg.str);

            //Forward the update message to all other clients
            SendToAllBut(msg.conn.connectionId, MiniMsgType.MiniDestroyObject, destroySlaveMsg);
        }

        private void OnNetDestroyRequested(NetworkMessage msg)
        {
            StringMessage idMsg = msg.ReadMessage<StringMessage>();

            GameObject obj = networkedPrefabs.GetNetworkedObject(idMsg.str);
            if(obj != null)
            {
                //The request was for the server to destroy its auth-obj, do it
                NetworkDestroy(obj);
            }
            else
            {
                //Someone else controls the object
                SendToAllBut(msg.conn.connectionId, MiniMsgType.MiniRequestDestroySlave, idMsg);
            }
        }

        private void OnSlaveOrient(NetworkMessage msg)
        {
            SlaveOrientMessage orient = msg.ReadMessage<SlaveOrientMessage>();

            GameObject obj = networkedPrefabs.GetSlaveObject(orient.networkID);
            if (obj != null)
            {
                Transform t = obj.transform;
                t.position = orient.loc;
                t.rotation = orient.rot;
                t.localScale = orient.scale;
            }

            //Forward the update message to all other clients
            SendToAllBut(msg.conn.connectionId, MiniMsgType.MiniSyncOrientation, orient);
        }

        private void OnBooleanVariableReceived(NetworkMessage msg)
        {
            Debug.Log("Boolean variable received");

            BooleanVarMessage boolMsg = msg.ReadMessage<BooleanVarMessage>();

            //Process the information for us
            NetworkVariable netBool;
            if(vars.TryGetValue(boolMsg.networkID + "-" + boolMsg.varName, out netBool))
            {
                ((NetworkBool)netBool).value = boolMsg.value;
            }

            //Forward the information to the other clients
            SendToAllBut(msg.conn.connectionId, MiniMsgType.MiniBoolVar, boolMsg);
        }

        private void OnIntegerVariableReceived(NetworkMessage msg)
        {
            IntegerVarMessage intMsg = msg.ReadMessage<IntegerVarMessage>();

            //Process the information for us
            NetworkVariable netInt;
            if (vars.TryGetValue(intMsg.networkID + "-" + intMsg.varName, out netInt))
            {
                ((NetworkInt)netInt).value = intMsg.value;
            }

            //Forward the information to the other clients
            SendToAllBut(msg.conn.connectionId, MiniMsgType.MiniIntVar, intMsg);
        }

        private void OnFloatVariableReceived(NetworkMessage msg)
        {
            FloatVarMessage floatMsg = msg.ReadMessage<FloatVarMessage>();

            //Process the information for us
            NetworkVariable netFloat;
            if (vars.TryGetValue(floatMsg.networkID + "-" + floatMsg.varName, out netFloat))
            {
                ((NetworkFloat)netFloat).value = floatMsg.value;
            }

            //Forward the information to the other clients
            SendToAllBut(msg.conn.connectionId, MiniMsgType.MiniFloatVar, floatMsg);
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
    }
}