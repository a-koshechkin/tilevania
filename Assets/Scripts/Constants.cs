using System.Collections.Generic;

public static class Constants
{
    public enum Layer
    {
        Ground,
        Climbing,
        Bouncing,
        Enemies,
        Hazard
    }
    public enum PlayerState
    {
        Running,
        Climbing,
        Falling,
        Idling,
        Dying,
        Shooting
    }

    public static readonly Dictionary<Layer, string> LayerTags = new()
    {
        { Layer.Ground, "Ground"},
        { Layer.Climbing, "Climbing"},
        { Layer.Bouncing, "Bouncing"},
        { Layer.Enemies, "Enemies"},
        { Layer.Hazard, "Hazard"}
    };

    public static readonly Dictionary<PlayerState, string> PlayerStateTags = new()
    {
        { PlayerState.Running, "IsRunning"},
        { PlayerState.Climbing, "IsClimbing"},
        { PlayerState.Falling, "IsFalling"},
        { PlayerState.Idling, "IsIdling"},
        { PlayerState.Dying, "Dying"},
        { PlayerState.Shooting, "Shooting"}
    };
}
