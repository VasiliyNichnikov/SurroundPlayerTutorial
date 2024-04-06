using System.Collections.Generic;

namespace RadiusModule
{
    /// <summary>
    /// Тестовый код для проверки и обновления точек
    /// </summary>
    internal class DebugCheckerForPointUpdates : ICheckerForPointUpdates
    {
        public void Check(Dictionary<int, Dictionary<int, RadiusPointData>> pointData)
        {
            foreach (var kvp in pointData)
            {
                foreach (var data in kvp.Value)
                {
                    // Пока лучшее решение, просто брать и сбрасывать все занятые точки
                    data.Value.SetOccupied();
                    
                    if ((kvp.Key == 0 && data.Key == 4) || (kvp.Key == 1 && data.Key == 12))
                    {
                        // Точка заблокирована стенной
                        data.Value.SetFree();
                    }
                    
#if UNITY_EDITOR
                    data.Value.Transform.name += $"_{data.Value.State}";
#endif
                }
            }
        }
    }
}