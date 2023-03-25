using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Vector2 newPos => new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));
}
