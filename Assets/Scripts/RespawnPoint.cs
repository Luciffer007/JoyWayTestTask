using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool IsEmpty
    {
        get
        {
            return _playersInPoint <= 0;
        }
    }
    
    private int _playersInPoint = 0;
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            _playersInPoint++;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            _playersInPoint--;
        }
    }
}
