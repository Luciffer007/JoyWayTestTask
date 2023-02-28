using System;
using System.Collections;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private CharacterController characterController;
    
    [SerializeField] 
    private Transform cameraTargetTransform;

    [SerializeField] 
    private NetworkObject spellPrefab;
    
    [SerializeField] 
    private GameObject playerInfo;
    
    [SerializeField] 
    private TextMeshProUGUI playerInfoText;

    [SerializeField]
    private float speed;
    
    [SerializeField]
    private float lookSpeed;
    
    [SerializeField]
    private float minCameraAngle;

    [SerializeField]
    private float maxCameraAngle;

    [SerializeField] 
    private float gravityMultiplier;
    
    [SerializeField]
    private float jumpPower;

    [SerializeField] 
    private float interactionDistance;

    [SerializeField] 
    private int maxHealth;
    #endregion

    #region Constants
    private const float Gravity = -9.81f;
    #endregion

    #region Components
    private Camera _camera;

    private IInteractable _interactableObject;
    #endregion
    
    #region Properties
    private bool IsGrounded => characterController.isGrounded;
    
    private float Health
    {
        get => _health.Value;
        set => _health.Value = Math.Max(0, value);
    }
    #endregion

    #region Network fields
    private readonly NetworkVariable<float> _health = new NetworkVariable<float>();
    
    private  readonly NetworkVariable<int> _fireCounter = new NetworkVariable<int>();
    #endregion

    private Vector2 _input;
    
    private Vector3 _direction;
    
    private float _verticalRotation;

    private float _velocity;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            // Cursor lock when player is in game and game has focus
            Cursor.lockState = CursorLockMode.Locked;
            
            // To ignore the owner model by the camera
            LayerHelper.MoveToLayer(transform, LayerMask.NameToLayer("Owner"));
            
            UIController.instance.NetworkUI.SetActive(false);
            UIController.instance.PlayerUI.SetActive(true);
            
            playerInfo.SetActive(false);

            UIController.instance.PlayerUI.healthBar.SetMaxValue(maxHealth);
            UIController.instance.PlayerUI.healthBar.SetValue(maxHealth);

            _health.OnValueChanged += (previousValue, newValue) =>
            {
                UIController.instance.PlayerUI.healthBar.SetValue((int) Math.Ceiling(newValue));
            };

            _fireCounter.OnValueChanged += (previousValue, newValue) =>
            {
                UIController.instance.PlayerUI.fireNotifier.UpdateCounter(newValue);
            };
            
            GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
            _camera = cameraObject.GetComponent<Camera>();
        }
        else
        {
            playerInfo.SetActive(true);
        }
        
        Health = maxHealth;
    }

    void Update()
    {
        if (!IsOwner)
        {
            playerInfoText.text = "Health: " + Health;
            return;
        }
        
        ApplyMovementDirection();
        ApplyGravity();
        ApplyMovement();

        DrawInteractionRay();
    }

    private void ApplyMovementDirection()
    {
        Vector3 moveHorizontal = transform.right * _input.x;
        Vector3 moveVertical = transform.forward * _input.y;
        _direction = (moveHorizontal + moveVertical).normalized;
    }
    
    private void ApplyGravity()
    {
        if (IsGrounded && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += Gravity * gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    private void ApplyMovement()
    {
        characterController.Move(_direction * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }
    
    public void Rotate(InputAction.CallbackContext context)
    {
        if (!IsOwner)
        {
            return;
        }
        
        Vector2 value = context.ReadValue<Vector2>();
        
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, value.x * lookSpeed + transform.rotation.eulerAngles.y, 0.0f));
        
        _verticalRotation += value.y * lookSpeed;
        _verticalRotation = Mathf.Clamp(_verticalRotation, minCameraAngle, maxCameraAngle);
        cameraTargetTransform.localEulerAngles = -new Vector3(_verticalRotation, 0f, 0f);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        
        if (!IsGrounded)
        {
            return;
        }

        _velocity += jumpPower;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (!IsOwner)
        {
            return;
        }
        
        if (!context.started)
        {
            return;
        }
        
        
        
        FireServerRpc(cameraTargetTransform.position + cameraTargetTransform.forward * 1.4f, cameraTargetTransform.rotation);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void FireServerRpc(Vector3 startPosition, Quaternion rotation, ServerRpcParams serverRpcParams = default)
    {
        NetworkObject spell = Instantiate(spellPrefab, startPosition, rotation);
        spell.SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!IsOwner)
        {
            return;
        }

        _interactableObject?.Interact();
    }

    void DrawInteractionRay()
    {
        _interactableObject = null;
        UIController.instance.PlayerUI.interactionText.text = "";

        if (_camera == null)
        {
            return;
        }
        
        Ray ray = _camera.ViewportPointToRay(Vector3.one / 2.0f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            _interactableObject = hit.collider.GetComponent<IInteractable>();

            if (_interactableObject != null)
            {
                UIController.instance.PlayerUI.interactionText.text = _interactableObject.GetDescription();
            }
        }
    }

    public void TakeDamage(float instantDamage, bool overTimeDamage = false, float damagePerTick = 0.0f, float tickDuration = 0.0f, float duration = 0.0f)
    {
        Health -= instantDamage;

        if (overTimeDamage)
        {
            DamageOverTime(damagePerTick, tickDuration, duration);
        }
    }

    private void DamageOverTime(float damagePerTick, float tickDuration, float duration)
    {
        StartCoroutine(DamageOverTickCoroutine(damagePerTick, tickDuration, duration));
    }

    private IEnumerator DamageOverTickCoroutine(float damagePerTick, float tickDuration, float duration)
    {
        float takenDamage = 0.0f;
        float fullDamage = duration / tickDuration * damagePerTick;

        _fireCounter.Value++;
        
        while (takenDamage < fullDamage)
        {
            yield return new WaitForSeconds(tickDuration);
            Health -= damagePerTick;
            takenDamage += damagePerTick;
        }
        
        _fireCounter.Value--;
    }
}
