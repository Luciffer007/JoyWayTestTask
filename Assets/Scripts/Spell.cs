using Interfaces;
using Unity.Netcode;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public class Spell : NetworkBehaviour, IHasCooldown
{
    #region Serialized Fields

    [SerializeField] 
    private int id;

    [SerializeField] 
    private float cooldownDuration; // seconds
    
    [SerializeField]
    private float speed;
    
    [SerializeField]
    private float lifeTime;
    
    [SerializeField] 
    public int instantDamage;
    
    [SerializeField]
    public bool overTimeDamage;
    
    [HideInInspector]
    public float damagePerTick;
    
    [HideInInspector]
    public float tickDuration; // seconds
    
    [HideInInspector]
    public float duration; // seconds
    #endregion

    #region Components
    private Rigidbody _rb;
    #endregion
    
    #region Interface implementation
    public int Id => id;

    public float CooldownDuration => cooldownDuration; // seconds
    #endregion

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner)
        {
            return;
        }
        
        DestroyServerRpc();

        if (collision.transform.CompareTag("Player"))
        {
            HitServerRpc(collision.transform.GetComponent<NetworkObject>().OwnerClientId);
        }
    }

    [ServerRpc]
    private void DestroyServerRpc()
    {
        Destroy(gameObject);
    }

    [ServerRpc]
    private void HitServerRpc(ulong playerId)
    {
        NetworkManager.ConnectedClients[playerId].PlayerObject.gameObject.GetComponent<PlayerController>().TakeDamage(instantDamage, overTimeDamage, damagePerTick, tickDuration, duration);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Spell))]
public class SpellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Spell spell = (Spell) target;

        if (spell.overTimeDamage)
        {
            spell.damagePerTick = EditorGUILayout.FloatField("Damage Per Tick", spell.damagePerTick);
            spell.tickDuration = EditorGUILayout.FloatField("Tick Duration", spell.tickDuration);
            spell.duration = EditorGUILayout.FloatField("Duration", spell.duration);
        }
    }
}
#endif
