using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] 
        private TextMeshProUGUI hostIpText;
        
        [SerializeField] 
        private TextMeshProUGUI interactionText;
    
        [SerializeField]
        private ProgressBar healthBar;
    
        [SerializeField]
        private Notifier fireNotifier;

        [SerializeField] 
        private CooldownNotifier fireballCooldownNotifier;

        [SerializeField] 
        private DeathWindow deathWindow;
        #endregion
        
        public TextMeshProUGUI HostIpText => hostIpText;
        
        public TextMeshProUGUI InteractionText => interactionText;
        
        public ProgressBar HealthBar => healthBar;
        
        public Notifier FireNotifier => fireNotifier;
        
        public CooldownNotifier FireballCooldownNotifier => fireballCooldownNotifier;
        
        public DeathWindow DeathWindow => deathWindow;

        void Awake()
        {
            DeathWindow.SetActive(false);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
