using UnityEngine;

public class ElevatorButton : MonoBehaviour, IInteractable
{
    #region Serialized Fields
    [SerializeField] 
    private GameObject elevator;
    #endregion

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
