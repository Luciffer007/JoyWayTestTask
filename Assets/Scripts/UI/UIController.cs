using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public static UIController instance;

        [SerializeField] 
        private NetworkUI networkUI;
    
        [SerializeField] 
        private PlayerUI playerUI;
    
        [SerializeField] 
        private ModalWindow modalWindow;

        [SerializeField] 
        private ProgressSpinner connectionSpinner;

        public NetworkUI NetworkUI => networkUI;
    
        public PlayerUI PlayerUI => playerUI;
    
        public ModalWindow ModalWindow => modalWindow;
        
        public ProgressSpinner ConnectionSpinner => connectionSpinner;
    
        private void Awake()
        {
            instance = this;
        }
    }
}
