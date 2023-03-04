using TMPro;
using UnityEngine;

namespace UI
{
    public class Notifier : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField]
        private TextMeshProUGUI counter;
        #endregion

        public void UpdateCounter(int count)
        {
            if (count > 0)
            {
                counter.text = count.ToString();
                gameObject.SetActive(true);
                return;
            }
        
            gameObject.SetActive(false);
        }
    }
}
