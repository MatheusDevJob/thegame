using System.Numerics;

namespace thegame.Entities.Tools;

public static class AnimationTool
{
    public static (int row, int frames) GetAnimationToolById(string toolId)
    {
        return toolId switch
        {
            "AxeTool" => (10, 8),
            "PickaxeTool" => (11, 8),
            "ShovelTool" => (12, 11),
            "IsJobing" => (6, 6),
            _ => (6, 6)
        };
    }
}