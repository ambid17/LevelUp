using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }
}
