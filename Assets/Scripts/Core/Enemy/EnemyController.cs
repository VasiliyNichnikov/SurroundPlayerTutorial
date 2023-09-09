#nullable enable
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _agent = null!;

        private EnemyMovement _movement = null!;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            var player = Main.Instance.PlayerController.Transform;
            _movement = new EnemyMovement(_agent, player);
        }

        private void Update()
        {
            _movement.TryMove();
        }
    }
}