using UnityEngine;

public class DestroyOffCamera : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
