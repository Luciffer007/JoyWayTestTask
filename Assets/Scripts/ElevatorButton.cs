using UnityEngine;

public class ElevatorButton : MonoBehaviour, IInteractable
{
    [SerializeField] 
    private GameObject elevator;

    private ElevatorController _elevatorController;

    void Start()
    {
        _elevatorController = elevator.GetComponent<ElevatorController>();
    }

    public void Interact()
    {
        _elevatorController.Move();
    }

    public string GetDescription()
    {
        return "Press [RMB]";
    }
}
