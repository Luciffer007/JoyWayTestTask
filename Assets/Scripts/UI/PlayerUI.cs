using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] 
        public TextMeshProUGUI hostIpText;
        
        [SerializeField] 
        public TextMeshProUGUI interactionText;
    
        [SerializeField]
        public ProgressBar healthBar;
    
        [SerializeField]
        public Notifier fireNotifier;

        [SerializeField] 
        public CooldownNotifier fireballCooldownNotifier;
        #endregion

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
