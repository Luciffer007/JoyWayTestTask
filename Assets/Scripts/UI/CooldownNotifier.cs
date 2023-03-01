using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CooldownNotifier : MonoBehaviour
    {
        [SerializeField] 
        private Image filler;
    
        public void UpdateNotifier(float fillAmount)
        {
            filler.fillAmount = fillAmount;
        }
    }
}
