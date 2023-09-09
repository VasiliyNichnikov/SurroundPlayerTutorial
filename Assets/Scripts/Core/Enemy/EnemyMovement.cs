#nullable enable
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemy
{
    public class EnemyMovement
    {
        private readonly Transform _player;
        private readonly NavMeshAgent _agent;

        public EnemyMovement(NavMeshAgent agent, Transform player)
        {
            _player = player;
            _agent = agent;
        }

        public void TryMove()
        {
            var path = CanMoveToPlayerAndGetPath();
            if (path == null)
            {
                return;
            }
            
            _agent.SetPath(path);
            
            // _agent.Move();
        }

        private NavMeshPath? CanMoveToPlayerAndGetPath()
        {
            var path = new NavMeshPath();
            var condition = _agent.CalculatePath(_player.position, path);
            return condition ? path : null;
        }
        
        private bool CanMoveToPlayer()
        {
            var path = new NavMeshPath();
            var condition = _agent.CalculatePath(_player.position, path);
            return condition;
        }
    }
}