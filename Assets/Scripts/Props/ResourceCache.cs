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
        [SerializeField]
        private AnimationManager animationManager;
        [SerializeField]
        private AnimationName grassIdle;
        [SerializeField]
        private AnimationName grassExplode;
        [SerializeField]
        private AnimationName dirtIdle;
        [SerializeField]
        private AnimationName dirtExplode;

        private bool _isMarkedForDeath;

        public void Setup(Sprite sprite, ResourceType resourceType)
        {
            spriteRenderer.sprite = sprite;
            myResourceType = resourceType;

            if (myResourceType == ResourceType.Dirt)
            {
                animationManager.PlayAnimation(dirtIdle, 0);
            }
            else if (myResourceType == ResourceType.Grass)
            {
                animationManager.PlayAnimation(grassIdle, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isMarkedForDeath)
            {
                return;
            }

           
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                StartCoroutine(PlayExplode());
            }
        }

        private IEnumerator PlayExplode()
        {
            _isMarkedForDeath = true;

            if(myResourceType == ResourceType.Dirt)
            {
                animationManager.PlayAnimation(grassExplode, 0);
            }else if(myResourceType == ResourceType.Grass)
            {
                animationManager.PlayAnimation(dirtExplode, 0);
            }

            yield return new WaitUntil(() => animationManager.IsCurrentAnimLoopFinished());

            int randomSpawn = Random.Range(minSpawn, maxSpawn);
            for (int i = 0; i < randomSpawn; i++)
            {
                Resource resource = Instantiate(resourcePrefab, transform.position, transform.rotation);
                resource.Setup(GameManager.UIManager.ResourceSpriteDictionary[myResourceType], myResourceType, GameManager.CurrencyManager.ResourceValue);
            }
            Destroy(gameObject);
        }
    }
}