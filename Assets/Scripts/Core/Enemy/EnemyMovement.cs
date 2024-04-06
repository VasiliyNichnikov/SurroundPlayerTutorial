#nullable enable
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemy
{
    public class EnemyMovement
    {
        public float GetDistanceToPoint()
        {
            if (!_pointForMovement.HasValue)
            {
                Debug.LogError("EnemyMovement.PointForMovement is null. State == Movement");
                return float.MaxValue;
            }

            return Vector3.Distance(_pointForMovement.Value, _enemyTransform.position);
        }
        

        public Vector3 PointForMovement => _pointForMovement ?? Vector3.zero;

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

            // if (_path.status == NavMeshPathStatus.PathComplete)
            // {
            //     return;
            // }
            
            _agent.SetPath(_path);
        }

        public void SetPoint(Vector3? point)
        {
            if (point == null)
            {
                return;
            }

            if (point.Value == _pointForMovement)
            {
                return;
            }
            
            var condition = _agent.CalculatePath(point.Value, _path);
            _pointForMovement = condition ? point : null;
        }
    }
}