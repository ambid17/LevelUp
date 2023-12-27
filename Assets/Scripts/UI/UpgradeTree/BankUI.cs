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
            currentDnaText.text = GameManager.ProgressSettings.Dna.ToString();
            bankedDnaText.text = GameManager.ProgressSettings.BankedDna.ToString();

            // if we have no DNA to bank, disable bank button
            bankButton.interactable = GameManager.ProgressSettings.Dna > 0;

            Platform.EventService.Add<CurrencyUpdatedEvent>(UpdateCurrencyText);

            StartCoroutine(RebuildUI());
        }

        // Forces the horizontal layout groups to regenerate, fixing any overlaps when the text changes
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
            float startingDna = GameManager.ProgressSettings.Dna;
            float startingBankedDna = GameManager.ProgressSettings.BankedDna;

            slider.DOValue(1, totalAnimationTime);

            int iterations = 20;
            float dnaDelta = startingDna / iterations;
            float interval = totalAnimationTime / iterations;
            for(int i = 1; i <= iterations; i++)
            {
                float currentDelta = dnaDelta * i;
                countUpText.transform.DOPunchScale(Vector3.one * 1.2f, interval / 2);
                countUpText.text = $"{currentDelta}";
                currentDnaText.text = $"{startingDna - currentDelta}";
                bankedDnaText.text = $"{startingBankedDna + currentDelta}";
                yield return new WaitForSeconds(interval);
            }


            yield return new WaitForSeconds(1);

            GameManager.ProgressSettings.Dna = 0;
            GameManager.ProgressSettings.BankedDna = startingDna + startingBankedDna;
            Platform.EventService.Dispatch<CurrencyUpdatedEvent>();

            // we have no DNA to bank, disable bank button
            bankButton.interactable = false;
        }

        private void UpdateCurrencyText()
        {
            currentDnaText.text = GameManager.ProgressSettings.Dna.ToString();
            bankedDnaText.text = GameManager.ProgressSettings.BankedDna.ToString();
            StartCoroutine(RebuildUI());
        }
    }
}
