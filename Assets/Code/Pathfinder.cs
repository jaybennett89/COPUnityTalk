using UnityEngine;
using Pathfinding;

public interface IPathfinder
{
    void ScanMap();
    Vector3 GetNearest(Vector3 targetPos);
}

public class Pathfinder : IPathfinder
{
    public Vector3 GetNearest(Vector3 targetPos)
    {
        if(AstarPath.active)
        {
            return AstarPath.active.GetNearest(targetPos, new NNConstraint() { walkable = true}).clampedPosition;
        }

        return targetPos;
    }

    public void ScanMap()
    {
        if(AstarPath.active)
        {
            AstarPath.active.Scan ();
        }
    }
}
