using Photon.Pun;
using UnityEngine;

public partial class Damageable : MonoBehaviourPun, IPunObservable
{
    public struct DamageMessage
    {
        public MonoBehaviour damager;
        public int amount;
        public Vector3 direction;
        public Vector3 damageSource;
        public bool throwing;

        public bool stopCamera;
    }
}
