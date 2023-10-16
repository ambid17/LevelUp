using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public enum Direction
    {
        Down,
        Up,
        Left,
        Right,
    }

    public class PlayerAnimationController : AnimationManager
    {
        public Direction currentDirection = Direction.Down;

        [Header("Down")]
        [SerializeField]
        private AnimationName idleDown;
        [SerializeField]
        private AnimationName runDown;
        [SerializeField]
        private AnimationName takeHitDown;
        [SerializeField]
        private AnimationName dieDown;

        [Header("Left")]
        [SerializeField]
        private AnimationName idleLeft;
        [SerializeField]
        private AnimationName runLeft;
        [SerializeField]
        private AnimationName takeHitLeft;
        [SerializeField]
        private AnimationName dieLeft;

        [Header("Right")]
        [SerializeField]
        private AnimationName idleRight;
        [SerializeField]
        private AnimationName runRight;
        [SerializeField]
        private AnimationName takeHitRight;
        [SerializeField]
        private AnimationName dieRight;

        [Header("Up")]

        [SerializeField]
        private AnimationName idleUp;
        [SerializeField]
        private AnimationName runUp;
        [SerializeField]
        private AnimationName takeHitUp;
        [SerializeField]
        private AnimationName dieUp;

        private float storedNormalizedTime;
        private EventService _eventService;

        private void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<PlayerChangedDirectionEvent>(ChangeDirection);
        }

        public void ChangeDirection(PlayerChangedDirectionEvent e)
        {
            currentDirection = e.NewDirection;
            storedNormalizedTime = CurrentAnimationNomralizedTime;
        }

        public void PlayIdleAnimation()
        {
            AnimationName animation = null;
            switch (currentDirection)
            {
                case Direction.Down:
                    animation = idleDown;
                    break;
                case Direction.Left:
                    animation = idleLeft;
                    break;
                case Direction.Right:
                    animation = idleRight;
                    break;
                case Direction.Up:
                    animation = idleUp;
                    break;
            }
            PlayAnimation(animation, 0);
        }
        public void PlayRunAnimation()
        {
            AnimationName animation = null;
            switch (currentDirection)
            {
                case Direction.Down:
                    animation = runDown;
                    break;
                case Direction.Left:
                    animation = runLeft;
                    break;
                case Direction.Right:
                    animation = runRight;
                    break;
                case Direction.Up:
                    animation = runUp;
                    break;
            }
            PlayAnimation(animation, storedNormalizedTime);
            storedNormalizedTime = 0;
        }
        public AnimationName PlayTakeHitAnimation()
        {
            AnimationName animation = null;
            switch (currentDirection)
            {
                case Direction.Down:
                    animation = takeHitDown;
                    break;
                case Direction.Left:
                    animation = takeHitLeft;
                    break;
                case Direction.Right:
                    animation = takeHitRight;
                    break;
                case Direction.Up:
                    animation = takeHitUp;
                    break;
            }
            OverrideAnimation(animation, 0);
            return animation;
        }
        public void PlayDieAnimation()
        {
            AnimationName animation = null;
            switch (currentDirection)
            {
                case Direction.Down:
                    animation = dieDown;
                    break;
                case Direction.Left:
                    animation = dieLeft;
                    break;
                case Direction.Right:
                    animation = dieRight;
                    break;
                case Direction.Up:
                    animation = dieUp;
                    break;
            }
            OverrideAnimation(animation, 0);
        }
    }
}