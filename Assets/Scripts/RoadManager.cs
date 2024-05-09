using UnityEngine;

public class RoadManager : Singleton<RoadManager>
{
    public RoadPoints[] roadPoints;
    protected override void Awake()
    {
        base.Awake();
        GetRoadPoints();
    }

    private void GetRoadPoints()
    {
        roadPoints = new RoadPoints[transform.childCount];
        var childObjects = transform.GetComponentsInChildren<Road>();
        for (int i = 0; i < transform.childCount; i++)
        {
            roadPoints[i] = new RoadPoints(childObjects[i].startPoint.position, childObjects[i].endPoint.position);
        }
    }
    
    public struct RoadPoints
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;

        public RoadPoints(Vector3 startPoint, Vector3 endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
    }
}
