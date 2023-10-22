using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class TreeBackgroundButton : MonoBehaviour
    {
        private Button _button;

        void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(DisableInspector);
        }

        void DisableInspector()
        {
            Platform.EventService.Dispatch(new EffectItemSelectedEvent(null));
        }
    }
}