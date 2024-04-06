#nullable enable
using System.Collections.ObjectModel;
using Core.Utils.StateMachine;
using UnityEngine;

namespace Core.Enemy.States
{
    public class MovementInCircle : StateBase
    {
        private readonly EnemyController _controller;
        private readonly DefaultStateMachine _stateMachine;
        private readonly SurroundingManager _surroundingManager;
        
        public MovementInCircle(EnemyController controller, DefaultStateMachine stateMachine)
        {
            _surroundingManager = Main.Instance.SurroundingManager;
            _controller = controller;
            _stateMachine = stateMachine;
        }

        private ReadOnlyCollection<Vector3> _points = null!;
        private int _selectedPointIndex;
        
        private float _radius;
        private float _angle;
        
        public override void Enter()
        {
            var startPosition = _controller.transform.position;
            var endPosition = _controller.PlayerController.Transform.position;
            _radius = Vector3.Distance(startPosition, endPosition);

            _points = _surroundingManager.GetPathToPointInRadius(_controller);
            _selectedPointIndex = 0;
            _controller.SetPointForMovement(_points[_selectedPointIndex]);
            _controller.TryMove();

            _controller.PlayerController.OnPlayerMovement += MoveToPlayer;

        }

        public override void LogicUpdate()
        {
            // Попытка движения по заданным точкам // TODO: 0.6 - в настройки
            if (_controller.DistanceToPoint <= 0.6f && _selectedPointIndex + 1 < _points.Count)
            {
                _selectedPointIndex++;
                _controller.SetPointForMovement(_points[_selectedPointIndex]);
                _controller.TryMove();
            }
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            _controller.PlayerController.OnPlayerMovement -= MoveToPlayer;
        }

        private void MoveToPlayer()
        {
            _stateMachine.ChangeState(_controller.IdleState);
        }
    }
}