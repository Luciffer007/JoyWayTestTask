using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ModalWindow : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] 
        private Button confirmButton;
    
        [SerializeField] 
        private Button cancelButton;
        
        [SerializeField] 
        private TextMeshProUGUI titleText;
        
        [SerializeField] 
        private TextMeshProUGUI messageText;
        #endregion

        private Action _confirmHandler;
        private Action _cancelHandler;

        private void Awake()
        {
            confirmButton.onClick.AddListener(OnClickConfirmButtonHandler);
            cancelButton.onClick.AddListener(OnClickCancelButtonHandler);
            
            cancelButton.gameObject.SetActive(false);
        }

        public void Show(string title, string message, string confirmButtonLabel, Action confirmHandler, string cancelButtonLabel = null, Action cancelHandler = null)
        {
            titleText.text = title;
            messageText.text = message;

            _confirmHandler = confirmHandler;
            _cancelHandler = cancelHandler;

            if (cancelButtonLabel != null)
            {
                cancelButton.gameObject.SetActive(true);
            }
            
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            
            cancelButton.gameObject.SetActive(false);
            
            _confirmHandler = null;
            _cancelHandler = null;
        }
        
        private void OnClickConfirmButtonHandler()
        {
            _confirmHandler?.Invoke();
            
            Hide();
        }
    
        private void OnClickCancelButtonHandler()
        {
            _cancelHandler?.Invoke();
            
            Hide();
        }
    }
}
