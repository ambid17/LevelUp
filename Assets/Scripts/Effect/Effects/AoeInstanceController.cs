using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AoeInstanceController : MonoBehaviour
{
    Entity myEntity;
    AoeEffect myEffect;


    public float timer;

    private Dictionary<Entity, float> entitiesInAoe;

    void Awake()
    {
        entitiesInAoe = new Dictionary<Entity, float>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= myEffect.duration)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(Entity source, AoeEffect aoeEffect)
    {
        myEntity = source;
        myEffect = aoeEffect;
        transform.localScale = new Vector2(aoeEffect.size, aoeEffect.size);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.gameObject.GetComponent<Entity>();

        // Make sure not to apply AOE effects to the source
        if (target == myEntity || target == null)
        {
            return;
        }

        entitiesInAoe.Add(target, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach(var entity in entitiesInAoe.Keys)
        {
            entitiesInAoe[entity] += Time.deltaTime;

            if (entitiesInAoe[entity] > myEffect.tickRate)
            {
                myEffect.positive.Execute(myEntity, entity);
                entitiesInAoe[entity] = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var target = collision.gameObject.GetComponent<Entity>();

        // Make sure not to apply AOE effects to the source
        if (target == myEntity || target == null)
        {
            return;
        }

        entitiesInAoe.Remove(target);

        myEffect.statusEffect.Execute(myEntity, target);
    }
}
