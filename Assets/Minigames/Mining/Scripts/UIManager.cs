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
        ObjectType _objectType = ObjectType.None;
        [SerializeField] private TextMeshPro _interactText;
        [SerializeField] GameObject _fuelShopPanel, _oreMarketPanel, _upgradeShopPanel, _repairStationPanel;
        // Start is called before the first frame update
        void Awake()
        {
            _eventService = Services.Instance.EventService;
            _eventService.Add<OnCanInteractEvent>(OnCanInteract);
            _eventService.Add<OnCantInteractEvent>(OnCantInteract);
            _interactText = GameManager.PlayerController.GetComponentInChildren<TextMeshPro>(true);
        }


        void OnCanInteract(OnCanInteractEvent _event)
        {
            _interactText.gameObject.SetActive(true);
            _objectType = _event.ObjectType;
        }

        void OnCantInteract(OnCantInteractEvent _event)
        {
            _interactText.gameObject.SetActive(false);
            _objectType = ObjectType.None;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E) && _objectType != ObjectType.None)
            {
                TogglePanel();
            }
        }

        void TogglePanel()
        {
            switch (_objectType)
            {
                case ObjectType.FuelShop:
                    _fuelShopPanel.SetActive(true);
                    break;
                case ObjectType.OreMarket:
                    _oreMarketPanel.SetActive(true);
                    break;
                case ObjectType.UpgradeShop:
                    _upgradeShopPanel.SetActive(true);
                    break;
                case ObjectType.RepairStation:
                    _repairStationPanel.SetActive(true);
                    break;
            }
        }

    }

}
