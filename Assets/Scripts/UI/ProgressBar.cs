using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        public void SetMaxValue(int maxValue)
        {
            slider.maxValue = maxValue;
        }
    
        public void SetValue(int newValue)
        {
            slider.value = newValue;
        }
    }
}
