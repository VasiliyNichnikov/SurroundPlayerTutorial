#nullable enable
using Core;
using Core.Enemy;
using Core.Player;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; } = null!;
    public IPlayerController PlayerController => _player;
    public SurroundingManager SurroundingManager => _surroundingManager;
    
    [SerializeField] private PlayerController _player = null!;
    [SerializeField] private EnemyManager _enemyManager = null!;
    [SerializeField] private SurroundingManager _surroundingManager = null!;
    
    private void Awake()
    {
        Instance = this;

        _enemyManager.Init();
    }

    private void Start()
    {
        _surroundingManager.Init(_player);
    }
}