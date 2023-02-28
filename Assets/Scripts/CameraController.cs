using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
            virtualCamera.Follow = transform;
        }
    }
}
