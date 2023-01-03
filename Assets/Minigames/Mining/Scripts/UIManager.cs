using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

namespace Minigames.Mining
{
    public class UIManager : MonoBehaviour
    {
        private EventService _eventService;
        [SerializeField] private TextMeshPro _interactText;
        // Start is called before the first frame update
        void Awake()
        {
            _eventService = Services.Instance.EventService;
            _eventService.Add<OnCanInteractEvent>(OnCanInteract);
            _interactText = GameManager.PlayerController.GetComponentInChildren<TextMeshPro>(true);
        }


        void OnCanInteract()
        {
            _interactText.gameObject.SetActive(true);
        }
    }

}
