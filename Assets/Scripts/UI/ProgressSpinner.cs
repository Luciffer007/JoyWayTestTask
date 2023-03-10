using UnityEngine;

namespace UI
{
    public class ProgressSpinner : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] 
        private RectTransform fillerRectTransform;

        [SerializeField]
        private float rotateSpeed;
        #endregion

        void Update () 
        {
            fillerRectTransform.Rotate(0f, 0f, -(rotateSpeed * Time.deltaTime));
        }

        public void Show()
        {
            fillerRectTransform.Rotate(Vector3.zero);
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
