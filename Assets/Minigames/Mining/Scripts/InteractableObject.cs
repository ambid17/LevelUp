using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace Minigames.Mining
{
    public class InteractableObject : MonoBehaviour
    {
        private ObjectType ObjectType;
        private EventService _eventService;

        // Start is called before the first frame update
        void Start()
        {
            _eventService = Services.Instance.EventService;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                _eventService.Dispatch<OnCanInteractEvent>();
            }
        }
    }
    public enum ObjectType
    {
        FuelShop, OreMarket, UpgradeShop, RepairStation
    }


}
