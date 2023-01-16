using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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

        void Awake()
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

        public void AwardCurrency(int minutesAway, float award)
        {
            if(award == 0) return;
            
            container.SetActive(true);
            awayText.text = $"You were gone for {minutesAway} minutes, you earned:";
            goldText.text = $"{award.ToCurrencyString()}";
        }

        private void Close()
        {
            container.SetActive(false);
        }
    }
}
