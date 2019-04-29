using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTogether.Net
{
    [CreateAssetMenu(fileName = "NewNetworkPrefabList", menuName = "VRTogether/Network Prefab List")]
    public class NetworkedPrefabList : ScriptableObject
    {
        public List<GameObject> prefabs;

        private HashSet<NetworkID> instantiatedNetObjs = new HashSet<NetworkID>();
        private HashSet<NetworkID> slaveNetObjs = new HashSet<NetworkID>();

        public HashSet<NetworkID> GetNetworkedObjects()
        {
            return instantiatedNetObjs;
        }

        public HashSet<NetworkID> GetSlaveObjects()
        {
            return slaveNetObjs;
        }

        public GameObject NetworkInstantiate(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab);

            //Gather or create a network ID for the system and save them for us to be authoritative over
            NetworkID id = obj.GetComponent<NetworkID>();
            id.netID = System.Guid.NewGuid().ToString();
            id.owner = -1;//-1 for server (no connection id)
            if (id != null)
            {
                instantiatedNetObjs.Add(id);
            }
            else
            {
                instantiatedNetObjs.Add(obj.AddComponent<NetworkID>());
            }

            return obj;
        }

        /// <summary>
        /// Create the slave of a another authority
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="id"></param>
        public GameObject NetworkSlave(string objectName, string id, int ownerConnectionID)
        {
            GameObject prefab = null;
            foreach(GameObject p in prefabs)
            {
                if(p.name == objectName)
                {
                    prefab = p;
                    break;
                }
            }

            GameObject obj = Instantiate(prefab);

            NetworkID netID = obj.GetComponent<NetworkID>();
            if(netID == null)
                netID = obj.AddComponent<NetworkID>();

            netID.netID = id;
            netID.owner = ownerConnectionID;

            slaveNetObjs.Add(netID);

            return obj;
        }

        public void NetworkDestroy(GameObject obj)
        {
            NetworkID netID = obj.GetComponent<NetworkID>();
            if(netID != null)
            {
                instantiatedNetObjs.Remove(netID);
                Destroy(obj);
            }
        }

        /// <summary>
        /// Stops accepting network input for the slave object and destroys the local object
        /// </summary>
        /// <param name="id">The network id for this slave</param>
        public void NetworkUnslave(string id)
        {
            NetworkID removeID = null;
            foreach(NetworkID slaveID in slaveNetObjs)
            {
                if(slaveID.netID == id)
                {
                    removeID = slaveID;
                    break;
                }
            }

            if(removeID != null)
            {
                Destroy(removeID.gameObject);
                slaveNetObjs.Remove(removeID);
            }
        }

        public void NetworkUnslave(NetworkID id)
        {
            if (id != null)
            {
                Debug.Log("destroying");
                Destroy(id.gameObject);
                slaveNetObjs.Remove(id);
            }
        }

        public bool IsSlave(string id)
        {
            foreach(NetworkID netid in slaveNetObjs)
            {
                if (netid.netID == id)
                    return true;
            }

            return false;
        }

        public void Shutdown()
        {
            instantiatedNetObjs.Clear();
        }

        /// <summary>
        /// Will return a networked object's local game object instance
        /// </summary>
        /// <param name="id">The GUID string of the object in question</param>
        /// <returns>The GameObject will be null if the local game object instance does not exist</returns>
        public GameObject GetNetworkedObject(string id)
        {
            foreach(NetworkID netID in instantiatedNetObjs)
            {
                if(netID.netID == id)
                {
                    return netID.gameObject;
                }
            }

            foreach (NetworkID netID in slaveNetObjs)
            {
                if (netID.netID == id)
                {
                    return netID.gameObject;
                }
            }

            return null;
        }

        public List<NetworkID> GetNetworkedIDsOwnedBy(int ownerConnectionID)
        {
            List<NetworkID> objs = new List<NetworkID>();

            foreach (NetworkID netID in slaveNetObjs)
            {
                if (netID.owner == ownerConnectionID)
                {
                    objs.Add(netID);
                }
            }

            return objs;
        }

        public GameObject GetSlaveObject(string id)
        {
            foreach (NetworkID netID in slaveNetObjs)
            {
                if (netID.netID == id)
                {
                    return netID.gameObject;
                }
            }

            return null;
        }
    }
}