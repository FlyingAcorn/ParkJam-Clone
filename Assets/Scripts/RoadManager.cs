using UnityEngine;

public class RoadManager : Singleton<RoadManager>
{
    public Road[] roadPoints;
    protected override void Awake()
    {
        base.Awake();
        GetRoadPoints();
    }

    private void GetRoadPoints()
    {
        roadPoints = new Road[transform.childCount];
        var childObjects = transform.GetComponentsInChildren<Road>();
        for (int i = 0; i < transform.childCount; i++)
        {
            roadPoints[i] = childObjects[i];
        }
    }
}
