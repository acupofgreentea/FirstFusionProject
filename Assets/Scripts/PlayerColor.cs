using UnityEngine;
using Fusion;

public class PlayerColor : NetworkBehaviour
{
    [Networked(OnChanged = nameof(HandlePlayerChange))] public Color Color {get; set;}

    [SerializeField] private Renderer _renderer;

    public Renderer Renderer => _renderer; 

    public override void Spawned()
    {
        Color = Random.ColorHSV();
    }
    private static void HandlePlayerChange(Changed<PlayerColor> changed)
    {
        changed.Behaviour.Renderer.material.color = changed.Behaviour.Color;
    }

}
