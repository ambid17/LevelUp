using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerVisualController : VisualController
    {
        protected override void Start()
        {
            base.Start();
            EventService.Add<PlayerDiedEvent>(Die);
            EventService.Add<PlayerRevivedEvent>(Revive);
        }

        private void Revive()
        {
            SpriteRenderer.color = Color.white;
        }

        private void Die()
        {
            SpriteRenderer.color = Color.black;
        }
    }
}