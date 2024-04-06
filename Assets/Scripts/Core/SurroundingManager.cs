#nullable enable
using System.Collections.ObjectModel;
using System.Linq;
using Core.Configs;
using Core.Enemy;
using Core.Player;
using Core.Surroundings;
using RadiusModule;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Нужен для слияния системы и игрока
    /// </summary>
    public class SurroundingManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _wellLayerMask;
        [SerializeField] private SurroundingSystemConfig _config = null!;
        
        private PlayerController _player = null!;
        private SurroundingSystem _surroundingSystem = null!;
        
        public void Init(PlayerController player)
        {
            _player = player;


            var factory = new TransformPointFactoryDefault(_player.transform);
            var radiusSettings = _config.Settings;
            var context = SurroundingSystemContext.CreateContext(factory, _player, radiusSettings, _wellLayerMask);
            _surroundingSystem = new SurroundingSystem(context);
        }

        public bool CanJoinRadius(EnemyController enemy)
        {
            return _surroundingSystem.CanJoinRadius(enemy);
        }

        public ReadOnlyCollection<Vector3> GetPathToPointInRadius(EnemyController enemy)
        {
            return _surroundingSystem.GetPathToPointInRadius(enemy).ToList().AsReadOnly();
        }
    }
}