#nullable enable
using System.Collections.Generic;
using Core.Player;
using UnityEngine;

namespace Core.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        private struct EnemyAndDistanceToPlayerData
        {
            public readonly EnemyController Enemy;
            public readonly float DistanceToPlayer;

            public EnemyAndDistanceToPlayerData(EnemyController enemy, float distanceToPlayer)
            {
                Enemy = enemy;
                DistanceToPlayer = distanceToPlayer;
            }
        }
        
        #region ParametersCircle
        private const float Radius = 2.0f;
        #endregion
        
        private IPlayerController _player = null!;
        
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private EnemyController[] _enemies = null!;

        public void Init()
        {
            _player = Main.Instance.PlayerController;
            _player.OnPlayerMovement += UpdateEnemyPoints;
        }

        private void Start()
        {
            UpdateEnemyPoints();
        }

        private void UpdateEnemyPoints()
        {
            var pointsAroundPlayer =
                CircleSurroundings.CalculatePointsInCircleWithWalls(_player.Transform.position, Radius, _layerMask);

            var nearbyEnemies = GetNearbyEnemiesToPlayer(pointsAroundPlayer.Length);
            for (var i = 0; i < pointsAroundPlayer.Length; i++)
            {
                nearbyEnemies[i].SetPointForMovement(pointsAroundPlayer[i]);
            }
        }
        
        private EnemyController[] GetNearbyEnemiesToPlayer(int numberEnemies)
        {
            numberEnemies = _enemies.Length < numberEnemies ? _enemies.Length : numberEnemies;
            
            var result = new List<EnemyController>();
            var data = new List<EnemyAndDistanceToPlayerData>();

            foreach (var enemy in _enemies)
            {
                var distanceToPlayer = Vector3.Distance(_player.Transform.position, enemy.transform.position);
                data.Add(new EnemyAndDistanceToPlayerData(enemy, distanceToPlayer));
            }

            data.Sort(SortByProximityToPlayer);

            for (var i = 0; i < numberEnemies; i++)
            {
                result.Add(data[i].Enemy);
            }

            return result.ToArray();
        }

        private static int SortByProximityToPlayer(EnemyAndDistanceToPlayerData a, EnemyAndDistanceToPlayerData b)
        {
            return (int)(a.DistanceToPlayer - b.DistanceToPlayer);
        }

        private void OnDestroy()
        {
            _player.OnPlayerMovement -= UpdateEnemyPoints;
        }
    }
}