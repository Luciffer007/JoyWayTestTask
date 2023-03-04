using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cameraTransform.position);
    }
}
