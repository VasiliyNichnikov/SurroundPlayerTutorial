#nullable enable
using Core.Enemy;
using Core.Player;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; } = null!;
    public IPlayerController PlayerController => _player;
    
    
    [SerializeField] private PlayerController _player = null!;
    [SerializeField] private EnemyManager _enemyManager = null!;
    
    private void Awake()
    {
        Instance = this;

        _enemyManager.Init();
    }
}