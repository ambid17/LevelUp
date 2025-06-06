using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class ResourceUIController : MonoBehaviour
    {
        [SerializeField]
        private ResourceUI resourceUIPrefab;

        private Dictionary<ResourceType, ResourceUI> _resourceUIDictionary = new();

        private void Start()
        {
            Platform.EventService.Add<PlayerResourceUpdateEvent>(UpdateResource);
        }

        public void UpdateResource(PlayerResourceUpdateEvent e)
        {
            if (_resourceUIDictionary.ContainsKey(e.ResourceType))
            {
                _resourceUIDictionary[e.ResourceType].UpdateValue(e.Number);
                return;
            }
            ResourceUI resourceUI = Instantiate(resourceUIPrefab, transform);
            resourceUI.MyImage.sprite = GameManager.UIManager.ResourceSpriteDictionary[e.ResourceType];
            resourceUI.UpdateValue(e.Number);
            _resourceUIDictionary.Add(e.ResourceType, resourceUI);
        }
    }
}