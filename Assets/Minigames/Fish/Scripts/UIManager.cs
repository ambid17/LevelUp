using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
    
        void Start()
        {
        
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _pausePanel.SetActive(!_pausePanel.activeInHierarchy);
            }
        }
    }
}

