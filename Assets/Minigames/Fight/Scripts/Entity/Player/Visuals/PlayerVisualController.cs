using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerVisualController : VisualController
    {
        void Start()
        {
            eventService.Add<PlayerDiedEvent>(Die);
            eventService.Add<PlayerRevivedEvent>(Revive);
            eventService.Add<PlayerDamagedEvent>(StartPlayerDamageFx);
        }

        void Update()
        {
        }

        private void Revive()
        {
            spriteRenderer.color = Color.white;
        }

        private void Die()
        {
            spriteRenderer.color = Color.black;
        }

        private void StartPlayerDamageFx(PlayerDamagedEvent e)
        {
            base.StartDamageFx(0);
        }
    }
}