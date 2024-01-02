using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public enum MessageType
    {
        Floor,
        Biome,
    }

    public class DisplayEndOfLevelDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private MessageType messageType;
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (messageType == MessageType.Floor)
            {
                Platform.EventService.Dispatch(new MouseHoverEvent($"Leave this level and proceed to Level: {GameManager.ProgressSettings.CurrentBiome.FloorsCompleted + 1}"));
            }
            if (messageType == MessageType.Biome)
            {
                Platform.EventService.Dispatch(new MouseHoverEvent("Leave this biome and proceed to the next one"));
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Platform.EventService.Dispatch(new MouseHoverEvent(""));
        }
    }
}