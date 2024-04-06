#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RadiusModule;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = "SurroundingSystemConfig", menuName = "KidoGames/SurroundingSystemConfig", order = 0)]
    public class SurroundingSystemConfig : ScriptableObject
    {
        public ReadOnlyCollection<RadiusModule.RadiusSettings> Settings
        {
            get
            {
                var result = new List<RadiusModule.RadiusSettings>();
                foreach (var item in _radiusSettings)
                {
                    var settings = RadiusModule.RadiusSettings.Create(item.LengthFromCenter, item.AngleIncrease);
                    result.Add(settings);
                }

                return result.AsReadOnly();
            }
        }

        
        [Serializable]
        private struct RadiusSettings
        {
            public float LengthFromCenter; // Длина радиуса от центра
            public float AngleIncrease; // Угол между точками
        }

        [SerializeField]
        private RadiusSettings[] _radiusSettings;
    }
}