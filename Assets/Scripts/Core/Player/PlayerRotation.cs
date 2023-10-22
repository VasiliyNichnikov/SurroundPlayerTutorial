#nullable enable
using Core.Utils;
using UnityEngine;

namespace Core.Player
{
    public class PlayerRotation
    {
        private readonly Camera _camera;
        private readonly Transform _playerTransform;
        
        private Vector3 _vectorToHit;
        
        public PlayerRotation(Transform playerTransform, Camera camera)
        {
            _camera = camera;
            _playerTransform = playerTransform;
        }

        public Vector3 GetVectorToHitNormalized()
        {
            CalculateMousePosition();
            return _vectorToHit.normalized;
        }

        public void TryRotate()
        {
            CalculateMousePosition();
            
            var directionToHit = _vectorToHit.normalized;
            var rotation = Quaternion.LookRotation(directionToHit, _playerTransform.up);

            if (rotation == _playerTransform.rotation)
            {
                return;
            }

            _playerTransform.rotation = rotation;
        }

        private void CalculateMousePosition()
        {
            var ray = MousePointer.GetWorldRay(_camera);
            
            var plane = new Plane(Vector3.up, _playerTransform.position);
            if (plane.Raycast(ray, out float enter))
            {
                var hitPoint = ray.GetPoint(enter);
                var hitPointXZ = new Vector3(hitPoint.x, _playerTransform.position.y, hitPoint.z);
                _vectorToHit = (hitPointXZ - _playerTransform.position);
            }
        }
    }
}