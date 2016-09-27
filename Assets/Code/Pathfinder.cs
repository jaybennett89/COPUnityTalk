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
        return AstarPath.active.GetNearest(targetPos, new NNConstraint() { walkable = true}).clampedPosition;
    }

    public void ScanMap()
    {
        AstarPath.active.Scan ();
    }
}
