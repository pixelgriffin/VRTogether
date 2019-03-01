using System.Collections;
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

                client.RegisterHandler(MiniMsgType.MiniBoolVar, OnBooleanVariableReceived);
                client.RegisterHandler(MiniMsgType.MiniIntVar, OnIntegerVariableReceived);                 client.RegisterHandler(MiniMsgType.MiniFloatVar, OnFloatVariableReceived);

                if (SceneManager.GetActiveScene().name == MacrogameClient.Instance.GetMinigameSceneToLoad())
                {
                    client.Send(MacroMsgType.MacroClientMinigameReady, new EmptyMessage());
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

        public GameObject NetworkInstantiate(GameObject prefab)
        {
            if (networkedPrefabs.prefabs.Contains(prefab))
            {
                GameObject obj = networkedPrefabs.NetworkInstantiate(prefab);

                SlaveInstantiateMessage newSlaveMsg = new SlaveInstantiateMessage();
                newSlaveMsg.networkID = obj.GetComponent<NetworkID>().netID;
                newSlaveMsg.objectName = prefab.name;

                client.Send(MiniMsgType.MiniInstantiateObject, newSlaveMsg);

                return obj;
            }

            return null;
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

            Debug.Log("Looking for requested slave to kill");

            GameObject obj = networkedPrefabs.GetNetworkedObject(idMsg.str);
            if(obj != null)
            {
                //If we were authoritative over this object we will destroy it
                NetworkDestroy(obj);

                Debug.Log("Killed");
            }
        }

        private void OnSlaveInstantiateRequested(NetworkMessage msg)
        {
            SlaveInstantiateMessage newSlaveMsg = msg.ReadMessage<SlaveInstantiateMessage>();
            networkedPrefabs.NetworkSlave(newSlaveMsg.objectName, newSlaveMsg.networkID);
        }

        private void OnSlaveDestroyRequested(NetworkMessage msg)
        {
            StringMessage destroySlaveMsg = msg.ReadMessage<StringMessage>();
            networkedPrefabs.NetworkUnslave(destroySlaveMsg.str);
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
            IntegerVarMessage intMsg = msg.ReadMessage<IntegerVarMessage>();

            //Process the information for us
            NetworkVariable netInt;
            if (vars.TryGetValue(intMsg.networkID + "-" + intMsg.varName, out netInt))
            {
                ((NetworkInt)netInt).value = intMsg.value;
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

        private void OnOtherPlayersReady(NetworkMessage msg)
        {
            allPlayersReady = true;
        }
    }
}