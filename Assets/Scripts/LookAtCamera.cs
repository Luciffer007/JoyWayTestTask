using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
