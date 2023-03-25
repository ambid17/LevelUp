using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Vector2 newPos => new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
}
