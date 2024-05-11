using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public Road[] roadPoints;
    private void Awake()
    {
        
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
