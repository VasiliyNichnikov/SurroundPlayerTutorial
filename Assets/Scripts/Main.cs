#nullable enable
using Core.Player;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; }
    public IPlayerController PlayerController => _player;
    
    
    [SerializeField] private PlayerController _player = null!;
    
    private void Awake()
    {
        Instance = this;
    }
}