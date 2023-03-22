using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Vector3 newPos => new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
}
