using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Generic.Code.Timer_System
{
    [SelectionBase]
    public class AdvancedTimer : MonoBehaviour
    {
        [SerializeField] private bool isWorking = false;
        public bool isWinable = false;
        public float time = 10;

        [SerializeField] private Gradient colorOverTime;
        public Image _image;
        [SerializeField] private TextMeshProUGUI timerText;
    

        public float currentTime;
        public UnityEvent OnStarted;
        public UnityEvent OnFinished;
        public UnityEvent OnFailed;

        private void Awake()
        {
            currentTime = time;
            UpdateTimertext();
        }

        // private void OnEnable()
        // {
        //     //CloseTimer();
        // }

        void Update()
        {
            if (!isWorking) return;


            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                if (_image == null) return;
                _image.fillAmount = currentTime / time;
                //saat gösterimi ile göster örnek 12:20
                UpdateTimertext();
                _image.color = colorOverTime.Evaluate(_image.fillAmount);
                return;
            }

            timerText.text = "00:00";
            if (isWinable)
            {
                OnFinished?.Invoke();
            }
            else
            {
                OnFailed?.Invoke();
            }

            SetIsWorking(false);
        }

        public void UpdateTimertext()
        {
            timerText.text = $"{Mathf.Floor(currentTime / 60).ToString("00")}:{Mathf.Floor(currentTime % 60).ToString("00")}";
        }

        public void SetIsWorking(bool boolean)
        {
            isWorking = boolean;
        }

        public void Destroy(float delay)
        {
            Destroy(gameObject, delay);
        }


        public void ResetTimer()
        {
            currentTime = time;
            _image.gameObject.SetActive(true);
            SetIsWorking(true);
        }

        public void CloseTimer()
        {
            _image.gameObject.SetActive(false);
            SetIsWorking(false);
        }

        public void CloseTimerForSeccond(float duration)
        {
            _image.gameObject.SetActive(false);
            SetIsWorking(false);
        }

        public void OpenTimer()
        {
            currentTime = time;
            _image.gameObject.SetActive(true);
            SetIsWorking(true);
        }
    }
}