using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Minigames.Mining
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private GridService _gridService;
        [SerializeField]
        private PlayerController _playerController;
        public static PlayerController PlayerController => Instance._playerController;
        public static GridService GridService => Instance._gridService;

        public static UnityEvent<ObjectType> OnCanInteractEvent;
    }
}
