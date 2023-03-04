using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NetworkUI : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] 
        private TMP_InputField ipAddressInputField;
    
        [SerializeField] 
        private Button hostButton;
    
        [SerializeField] 
        private Button joinButton;
        #endregion

        public bool interactable
        {
            get
            {
                return _interactable;
            }
            set
            {
                _interactable = value;
                ipAddressInputField.interactable = value;
                hostButton.interactable = value;

                if (value)
                {
                    joinButton.interactable = IPHelper.ValidateIpAddress(ipAddressInputField.text);
                    return;
                }
                
                joinButton.interactable = false;
            }
        }

        private bool _interactable = false;

        void Awake()
        {
            hostButton.onClick.AddListener(OnClickHostButtonHandler);
            joinButton.onClick.AddListener(OnClickJoinButtonHandler);
            ipAddressInputField.onValueChanged.AddListener(ValidateIpAddress);
        
            joinButton.interactable = false;
        }

        private void ValidateIpAddress(string ipAddress)
        {
            joinButton.interactable = IPHelper.ValidateIpAddress(ipAddress);
        }

        private void OnClickJoinButtonHandler()
        {
            CustomNetworkManager.Singleton.StartClient(ipAddressInputField.text);
            interactable = false;
        }
    
        private void OnClickHostButtonHandler()
        {
            CustomNetworkManager.Singleton.StartHost();
        }

        public void ClearIpAddressInputField()
        {
            ipAddressInputField.text = "";
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
