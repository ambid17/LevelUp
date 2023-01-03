using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class NotificationPanel : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TMP_Text awayText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private Button closeButton;

        private float _closeTimer;
        private float _closeTime = 10;

        void Start()
        {
            closeButton.onClick.AddListener(Close);
        }

        void Update()
        {
            _closeTimer += Time.deltaTime;

            if (_closeTimer > _closeTime)
            {
                Close();
            }
        }

        public void Notify(int minutesAway, float currencyAwarded)
        {
            container.SetActive(true);
            awayText.text = $"You were gone for {minutesAway} minutes, you earned:";
            goldText.text = $"{currencyAwarded.ToCurrencyString()}";
        }

        private void Close()
        {
            container.SetActive(false);
        }
    }
}
