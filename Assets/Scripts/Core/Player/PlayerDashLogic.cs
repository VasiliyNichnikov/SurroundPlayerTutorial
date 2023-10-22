#nullable enable
using System.Collections;
using UnityEngine;

namespace Core.Player
{
    public class PlayerDashLogic
    {
        private const float MaximumDashLength = 5f;
        private const float DashTime = 0.25f;

        private IEnumerator? _dashAnimation;
        private readonly AnimationCurve _dashCurve;
        private readonly LayerMask _layerMaskWalls;
        private readonly PlayerController _controller;
        
        public PlayerDashLogic(PlayerController controller, LayerMask layerMaskWalls, AnimationCurve dashCurve)
        {
            _dashCurve = dashCurve;
            _controller = controller;
            _layerMaskWalls = layerMaskWalls;
        }
        
        private IEnumerator DashAnimation()
        {
            _controller.ControlBlocker.Lock();
            var startingPosition = _controller.transform.position;
            var directionOfDash = new Vector3(PlayerMovement.GetHorizontalInput(), 0, PlayerMovement.GetVerticalInput());
            var dashLength = 0.0f;
            
            if (Physics.Raycast(startingPosition, directionOfDash, out var hit, Mathf.Infinity, _layerMaskWalls))
            {
                var hitPoint = hit.point;
                hitPoint.y = startingPosition.y;
                var distanceToWall = Vector3.Distance(startingPosition, hitPoint);
                dashLength = distanceToWall < MaximumDashLength ? distanceToWall : MaximumDashLength;
            }
            
            var endingOfPosition = startingPosition + directionOfDash * dashLength;

            var time = DashTime;
            while (time > 0)
            {
                var progress = 1.0f - Mathf.Clamp01(time / DashTime);
                _controller.transform.position = Vector3.Lerp(startingPosition, endingOfPosition, _dashCurve.Evaluate(progress));
                yield return null;
                time -= Time.deltaTime;
            }
            _controller.transform.position = endingOfPosition;

            _dashAnimation = null;
            _controller.ControlBlocker.Unlock();
        }

        public void Dash()
        {
            if (_dashAnimation != null)
            {
                return;
            }
            
            _dashAnimation = DashAnimation();
            _controller.StartCoroutine(_dashAnimation);
        }
    }
}