#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace RadiusModule
{
    internal class DefaultCheckerForPointUpdates : ICheckerForPointUpdates
    {
        private readonly Dictionary<int, float> _surroundingLength;
        private readonly ICenterComponent _centerComponent;
        private readonly int _layerMask;
        
        public DefaultCheckerForPointUpdates(ICenterComponent centerComponent, Dictionary<int, float> surroundingLength, int layerMask)
        {
            _centerComponent = centerComponent;
            _surroundingLength = surroundingLength;
            _layerMask = layerMask;
        }
        
        public void Check(Dictionary<int, Dictionary<int, RadiusPointData>> pointData)
        {
            var center = _centerComponent.Position;
            foreach (var kvp in pointData)
            {
                foreach (var data in kvp.Value)
                {
                    // Пока лучшее решение, просто брать и сбрасывать все занятые точки
                    data.Value.SetFree();
                    
                    var radiusLength = _surroundingLength[kvp.Key];
                    var direction = data.Value.Transform.position - center;
                    if (Physics.Raycast(center, direction, radiusLength, _layerMask))
                    {
                        // Точка заблокирована стенной
                        data.Value.SetLocked();
                    }
                }
            }
        }
    }
}