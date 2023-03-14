using Unity.Netcode;
using UnityEngine;

public class ElevatorController : NetworkBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private float speed = 3f;
    #endregion
    
    private int _currentDirection = -1;

    void FixedUpdate()
    {
        if (!IsHost)
        {
            return;
        }
        
        if (transform.position.y < 9 && _currentDirection == 1)
        {
            transform.position = transform.position + Vector3.up * Time.deltaTime * speed;
        }
        
        if (transform.position.y > 1 && _currentDirection == -1)
        {
            transform.position = transform.position + Vector3.down * Time.deltaTime * speed;
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            collider.transform.GetComponent<NetworkObject>().TrySetParent(transform);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            collider.transform.GetComponent<NetworkObject>().TryRemoveParent();
        }
    }

    public void Move()
    {
        FireServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void FireServerRpc()
    {
        _currentDirection = -_currentDirection;
    }
}
