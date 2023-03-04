using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField]
        private Slider slider;
        #endregion

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
