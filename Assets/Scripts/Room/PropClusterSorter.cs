using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropClusterSorter : MonoBehaviour
{
    public List<SpriteRenderer> Props => _props;

    private List<SpriteRenderer> _props;


    [ContextMenu("SortProps")]
    public void SortProps()
    {
        _props = GetComponentsInChildren<SpriteRenderer>().ToList();

        foreach (SpriteRenderer renderer in _props)
        {
            renderer.sortingOrder = Mathf.FloorToInt(renderer.transform.position.y * -100);
        }
    }

}
