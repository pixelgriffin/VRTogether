﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace VRTogether.Net
{
    public class MinigameClient : SingletonComponent<MinigameClient>
    {
        public NetworkedPrefabList networkedPrefabs;

        private NetworkClient client;

        private bool allPlayersReady = false;

        private Dictionary<string, NetworkVariable> vars = new Dictionary<string, NetworkVariable>();

        //calculated average RTT (ms)
        private int rtt;

        private void Start()
        {
            client = MacrogameClient.Instance.GetClient();
            if(client != null)
            {
                client.RegisterHandler(MiniMsgType.MiniInstantiateObject, OnSlaveInstantiateRequested);
                client.RegisterHandler(MiniMsgType.MiniDestroyObject, OnSlaveDestroyRequested);
                client.RegisterHandler(MiniMsgType.MiniSyncOrientation, OnSlaveOrient);
                client.RegisterHandler(MiniMsgType.MiniOtherPlayersReady, OnOtherPlayersReady);
                client.RegisterHandler(MiniMsgType.MiniRequestDestroySlave, OnNetDestroyRequested);
                client.RegisterHandler(MiniMsgType.MiniRequestInstantiateObject, OnRequestedInstantiateObject);

                client.RegisterHandler(MiniMsgType.MiniBoolVar, OnBooleanVariableReceived);
                client.RegisterHandler(MiniMsgType.MiniIntVar, OnIntegerVariableReceived);
                client.RegisterHandler(MiniMsgType.MiniFloatVar, OnFloatVariableReceived);

                client.RegisterHandler(MiniMsgType.MiniEndGame, OnMinigameEnded);

                if (SceneManager.GetActiveScene().name == MacrogameClient.Instance.GetMinigameSceneToLoad())
                {
                    //client.Send(MacroMsgType.MacroClientMinigameReady, new EmptyMessage());
                    MacrogameClient.Instance.ClearMinigameSceneToLoad();
                }
            }
        }

        private void Update()
        {
            foreach (NetworkID id in networkedPrefabs.GetNetworkedObjects())
            {
                Transform t = networkedPrefabs.GetNetworkedObject(id.netID).transform;
                SlaveOrientMessage orient = new SlaveOrientMessage();
                orient.networkID = id.netID;
                orient.loc = t.position;
                orient.rot = t.rotation;
                orient.scale = t.localScale;

                client.Send(MiniMsgType.MiniSyncOrientation, orient);
            }
        }

        public void ReadyUp()
        {
            client.Send(MacroMsgType.MacroClientMinigameReady, new EmptyMessage());
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

        public void SendBooleanToAll(NetworkBool netBool)
        {
            BooleanVarMessage boolMsg = new BooleanVarMessage();
            boolMsg.networkID = netBool.objID;
            boolMsg.varName = netBool.name;
            boolMsg.value = netBool.value;

            client.Send(MiniMsgType.MiniBoolVar, boolMsg);
        }

        public void SendIntegerToAll(NetworkInt netInt)
        {
            IntegerVarMessage intMsg = new IntegerVarMessage();
            intMsg.networkID = netInt.objID;
            intMsg.varName = netInt.name;
            intMsg.value = netInt.value;

            client.Send(MiniMsgType.MiniIntVar, intMsg);
        }

        public void SendFloatToAll(NetworkFloat netFloat)
        {
            FloatVarMessage floatMsg = new FloatVarMessage();
            floatMsg.networkID = netFloat.objID;
            floatMsg.varName = netFloat.name;
            floatMsg.value = netFloat.value;

            client.Send(MiniMsgType.MiniFloatVar, floatMsg);
        }

        public bool AllPlayersReady()
        {
            return allPlayersReady;
        }

        public void NetworkRequestServerInstantiate(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            if (networkedPrefabs.prefabs.Contains(prefab))
            {
                ObjectInstantiateMessage newObjectMsg = new ObjectInstantiateMessage();
                newObjectMsg.objectName = prefab.name;
                newObjectMsg.pos = pos;
                newObjectMsg.rot = rot;

                client.Send(MiniMsgType.MiniRequestInstantiateObject, newObjectMsg);
            }
            else
            {
                Debug.Log("networked prefabs does not contain " + prefab.name);
            }
        }

        public GameObject NetworkInstantiate(GameObject prefab)
        {
            if (networkedPrefabs.prefabs.Contains(prefab))
            {
                GameObject obj = networkedPrefabs.NetworkInstantiate(prefab);

                SlaveInstantiateMessage newSlaveMsg = new SlaveInstantiateMessage();
                newSlaveMsg.networkID = obj.GetComponent<NetworkID>().netID;
                newSlaveMsg.objectName = prefab.name;
                newSlaveMsg.owner = MacrogameClient.Instance.GetClient().connection.connectionId;

                client.Send(MiniMsgType.MiniInstantiateObject, newSlaveMsg);
                

                return obj;
            }

            return null;
        }

        public void OnPlayerLeftMinigame(int connectionID)
        {
            foreach (NetworkID obj in networkedPrefabs.GetNetworkedIDsOwnedBy(connectionID))
            {
                networkedPrefabs.NetworkUnslave(obj);
            }
        }

        private void OnRequestedInstantiateObject(NetworkMessage msg)
        {
            ObjectInstantiateMessage newObjMsg = msg.ReadMessage<ObjectInstantiateMessage>();

            GameObject objPrefab = null;
            foreach(GameObject obj in networkedPrefabs.prefabs)
            {
                if(obj.name == newObjMsg.objectName)
                {
                    objPrefab = obj;
                    break;
                }
            }


            if(objPrefab != null)
            {
                GameObject spawn = NetworkInstantiate(objPrefab);
                spawn.transform.position = newObjMsg.pos;
                spawn.transform.rotation = newObjMsg.rot;
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

                    client.Send(MiniMsgType.MiniDestroyObject, destroySlaveMsg);

                    return true;
                }
                else
                {
                    StringMessage idMsg = new StringMessage();
                    idMsg.str = netObj.GetComponent<NetworkID>().netID;

                    client.Send(MiniMsgType.MiniRequestDestroySlave, idMsg);
                }
            }

            return false;
        }

        private void OnNetDestroyRequested(NetworkMessage msg)
        {
            StringMessage idMsg = msg.ReadMessage<StringMessage>();

            Debug.Log("Looking for requested slave to kill: " + idMsg.str);

            GameObject obj = networkedPrefabs.GetNetworkedObject(idMsg.str);
            if(obj != null)
            {
                //If we were authoritative over this object we will destroy it
                NetworkDestroy(obj);

                Debug.Log("Killed");
            }
            else
            {
                Debug.Log("wat? no existy: " + idMsg.str);
            }
        }

        private void OnSlaveInstantiateRequested(NetworkMessage msg)
        {
            SlaveInstantiateMessage newSlaveMsg = msg.ReadMessage<SlaveInstantiateMessage>();
            networkedPrefabs.NetworkSlave(newSlaveMsg.objectName, newSlaveMsg.networkID, newSlaveMsg.owner);
        }

        private void OnSlaveDestroyRequested(NetworkMessage msg)
        {
            StringMessage destroySlaveMsg = msg.ReadMessage<StringMessage>();
            networkedPrefabs.NetworkUnslave(destroySlaveMsg.str);
        }

        private void OnSlaveOrient(NetworkMessage msg)
        {
            SlaveOrientMessage orient = msg.ReadMessage<SlaveOrientMessage>();

            //Update average RTT since this function is called more often than any other function
            UpdateRTT(client.GetRTT());

            GameObject obj = networkedPrefabs.GetSlaveObject(orient.networkID);
            if (obj != null)
            {
                //apply lerp to transform
                Transform t = obj.transform;
                t.position = orient.loc;
                t.rotation = orient.rot;
                t.localScale = orient.scale;
            }
        }

        private void OnBooleanVariableReceived(NetworkMessage msg)
        {
            BooleanVarMessage boolMsg = msg.ReadMessage<BooleanVarMessage>();

            //Process the information for us
            NetworkVariable netBool;
            if (vars.TryGetValue(boolMsg.networkID + "-" + boolMsg.varName, out netBool))
            {
                ((NetworkBool)netBool).value = boolMsg.value;
            }
        }

        private void OnIntegerVariableReceived(NetworkMessage msg)
        {
            Debug.Log("Message received");
            IntegerVarMessage intMsg = msg.ReadMessage<IntegerVarMessage>();

            //Process the information for us
            NetworkVariable netInt;
            if (vars.TryGetValue(intMsg.networkID + "-" + intMsg.varName, out netInt))
            {
                ((NetworkInt)netInt).value = intMsg.value;
                Debug.Log("Network variable set");
            }
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
        }

        private void OnMinigameEnded(NetworkMessage msg)
        {
            networkedPrefabs.Shutdown();
            SceneManager.LoadScene((msg.ReadMessage<StringMessage>()).str);
        }

        private void OnOtherPlayersReady(NetworkMessage msg)
        {
            allPlayersReady = true;
        }

        //updates rtt value from recieved message rtt
        private void UpdateRTT(float newRtt)
        {
            //calculate weighted average
            rtt = (int) (newRtt * 0.7f + rtt * 0.3f);
        }
    }
}