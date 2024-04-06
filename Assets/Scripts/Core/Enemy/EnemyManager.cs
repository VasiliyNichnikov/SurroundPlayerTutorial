#nullable enable
using Core.Player;
using UnityEngine;

namespace Core.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        private IPlayerController _player = null!;
        
        [SerializeField] private EnemyController[] _enemies = null!;

        public void Init()
        {
            _player = Main.Instance.PlayerController;
        }

        private void Start()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Init(_player);
            }
            
        }
    }
}