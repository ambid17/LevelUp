using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class BankUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text countUpText;
        [SerializeField] private TMP_Text currentDnaText;
        [SerializeField] private TMP_Text bankedDnaText;
        [SerializeField] private Button bankButton;
        [SerializeField] private RectTransform bankDnaContainer;
        [SerializeField] private RectTransform currentDnaContainer;


        void Start()
        {
            bankButton.onClick.AddListener(Bank);
        }

        private void OnEnable()
        {
            slider.value = 0.2f;
            countUpText.text = "";
            currentDnaText.text = GameManager.SettingsManager.progressSettings.Dna.ToString();
            bankedDnaText.text = GameManager.SettingsManager.progressSettings.BankedDna.ToString();

            StartCoroutine(RebuildUI());
        }

        [ContextMenu("test")]
        public void Test()
        {
            StartCoroutine(RebuildUI());
        }

        private IEnumerator RebuildUI()
        {
            yield return new WaitForEndOfFrame();
            LayoutRebuilder.ForceRebuildLayoutImmediate(bankDnaContainer);
            LayoutRebuilder.ForceRebuildLayoutImmediate(currentDnaContainer);
        }

        private void Bank()
        {
            StartCoroutine(RunBankAnimation());
        }

        private IEnumerator RunBankAnimation()
        {
            float totalAnimationTime = 5;
            float startingDna = GameManager.SettingsManager.progressSettings.Dna;
            float startingBankedDna = GameManager.SettingsManager.progressSettings.BankedDna;

            slider.DOValue(1, totalAnimationTime);

            int iterations = 10;
            float dnaDelta = startingDna / iterations;
            float interval = totalAnimationTime / iterations;
            for(int i = 0; i < iterations; i++)
            {
                countUpText.transform.DOPunchScale(Vector3.one * 1.5f, interval);
                countUpText.text = $"{dnaDelta * i}";
                currentDnaText.text = $"{startingDna - dnaDelta}";
                bankedDnaText.text = $"{startingDna + dnaDelta}";
                yield return new WaitForSeconds(interval);
            }


            yield return new WaitForSeconds(1);

            GameManager.SettingsManager.progressSettings.Dna = 0;
            GameManager.SettingsManager.progressSettings.BankedDna = startingDna + startingBankedDna;

        }
    }
}
