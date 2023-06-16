using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class ResourceCache : MonoBehaviour
    {
        [SerializeField]
        private Resource resourcePrefab;
        [SerializeField]
        private int minSpawn = 10;
        [SerializeField]
        private int maxSpawn = 20;
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private ResourceType myResourceType;

        public void Setup(Sprite sprite, ResourceType resourceType)
        {
            spriteRenderer.sprite = sprite;
            myResourceType = resourceType;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                int randomSpawn = Random.Range(minSpawn, maxSpawn);
                for (int i = 0; i < randomSpawn; i++)
                {
                    Resource resource = Instantiate(resourcePrefab, transform.position, transform.rotation);
                    resource.Setup(GameManager.UIManager.ResourceSpriteDictionary[myResourceType], myResourceType);
                }
                Destroy(gameObject);
            }
        }
    }
}