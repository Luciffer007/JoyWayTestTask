using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Singleton;

        #region Serialized Fields
        [SerializeField] 
        private NetworkUI networkUI;
    
        [SerializeField] 
        private PlayerUI playerUI;
    
        [SerializeField] 
        private ModalWindow modalWindow;

        [SerializeField] 
        private ProgressSpinner connectionSpinner;
        #endregion

        public NetworkUI NetworkUI => networkUI;
    
        public PlayerUI PlayerUI => playerUI;
    
        public ModalWindow ModalWindow => modalWindow;
        
        public ProgressSpinner ConnectionSpinner => connectionSpinner;
    
        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            if (Singleton != null && Singleton != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Singleton = this;
            }

            NetworkUI.SetActive(true);
            ConnectionSpinner.SetActive(false);
            PlayerUI.SetActive(false);
            ModalWindow.Hide();
        }
    }
}
