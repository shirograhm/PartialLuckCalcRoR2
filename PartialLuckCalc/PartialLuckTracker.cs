using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PartialLuckPlugin
{
    public class PartialLuckTracker : MonoBehaviour
    {
        private float _partialLuck;
        public float PartialLuck
        {
            get { return _partialLuck; }
            set
            {
                _partialLuck = value;
                if (NetworkServer.active)
                {
                    new Sync(
                        gameObject.GetComponent<NetworkIdentity>().netId,
                        value
                    ).Send(NetworkDestination.Clients);
                    
                    CharacterMaster master = this.GetComponentInParent<CharacterMaster>();
                    if (master) SetPartialLuck(master, value);
                }
            }
        }

        private void SetPartialLuck(CharacterMaster master, float partial)
        {
            float luck = 0;
            if (master.inventory)
            {
                luck += master.inventory.GetItemCount(RoR2Content.Items.Clover);
                luck -= master.inventory.GetItemCount(RoR2Content.Items.LunarBadLuck);
            }
            luck += partial;

            master.luck = luck;
        }

        public class Sync : INetMessage
        {
            NetworkInstanceId objId;
            float partialLuck;

            public Sync() { }

            public Sync(NetworkInstanceId netId, float luck)
            {
                this.objId = netId;
                partialLuck = luck;
            }

            public void Deserialize(NetworkReader reader)
            {
                objId = reader.ReadNetworkId();
                partialLuck = reader.ReadSingle();
            }

            public void OnReceived()
            {
                if (NetworkServer.active) return;

                GameObject obj = Util.FindNetworkObject(objId);
                if (obj != null)
                {
                    PartialLuckTracker tracker = obj.GetComponent<PartialLuckTracker>();
                    if (tracker != null)
                    {
                        tracker.PartialLuck = partialLuck;
                    }
                }
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(objId);
                writer.Write(partialLuck);
            }
        }
    }
}
