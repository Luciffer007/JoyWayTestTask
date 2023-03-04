using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Singleton;

    [SerializeField]
    private List<RespawnPoint> respawnPoints = new List<RespawnPoint>();

    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
        }
    }

    public Vector3 GetRespawnPosition()
    {
        int randomIndex;
        
        List<RespawnPoint> emptyRespawnPoints = respawnPoints.Where(respawnPoint => respawnPoint.IsEmpty) as List<RespawnPoint>;
        if (emptyRespawnPoints != null && emptyRespawnPoints.Count != 0)
        {
            randomIndex = Random.Range(0, emptyRespawnPoints.Count);
            return emptyRespawnPoints[randomIndex].transform.position;
        }
        
        // If all spawn points are occupied, we take a random one from the occupied ones
        randomIndex = Random.Range(0, respawnPoints.Count);
        return respawnPoints[randomIndex].transform.position;
    }
}
