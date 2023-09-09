#nullable enable
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemy
{
    public class EnemyMovement
    {
        public EnemyState State { get; private set; }

        public float GetDistanceToPoint()
        {
            if (State == EnemyState.Idle)
            {
                return float.MaxValue;
            }
            
            if (!_pointForMovement.HasValue)
            {
                Debug.LogError("EnemyMovement.PointForMovement is null. State == Movement");
                return float.MaxValue;
            }

            return Vector3.Distance(_pointForMovement.Value, _enemyTransform.position);
        }

        private readonly Transform _enemyTransform;
        private readonly NavMeshAgent _agent;
        private readonly NavMeshPath _path;
        private Vector3? _pointForMovement;

        public EnemyMovement(Transform enemyTransform, NavMeshAgent agent)
        {
            _path = new NavMeshPath();
            _enemyTransform = enemyTransform;
            _agent = agent;
        }

        public void TryMove()
        {
            if (_pointForMovement == null)
            {
                return;
            }

            if (_path.corners.Length == 0)
            {
                return;
            }
            
            _agent.SetPath(_path);
        }

        public void SetPoint(Vector3? point)
        {
            if (point == null)
            {
                State = EnemyState.Idle;
                return;
            }

            State = EnemyState.Movement;
            var condition = _agent.CalculatePath(point.Value, _path);
            _pointForMovement = condition ? point : null;
        }
    }
}