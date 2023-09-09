#nullable enable
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemy
{
    public enum EnemyState
    {
        Idle,
        Movement
    }
    
    public class EnemyController : MonoBehaviour
    {
        public EnemyState State => _movement.State;

        public float GetDistanceToPoint => _movement.GetDistanceToPoint();
        
        [SerializeField]
        private NavMeshAgent _agent = null!;

        private EnemyMovement _movement = null!;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _movement = new EnemyMovement(transform, _agent);
        }

        private void Update()
        {
            _movement.TryMove();
        }


        public void SetIdle()
        {
            _movement.SetPoint(null);
        }
        
        public void SetPointForMovement(Vector3 point)
        {
            _movement.SetPoint(point);
        }
    }
}