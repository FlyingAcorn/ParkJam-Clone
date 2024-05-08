using UnityEngine;

public class CarMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
//90 up,180right,270 down,0 left (car Facing angles)
//TODO: if cars angle is in the same direction do the motion.
    public void Movement(Vector3 direction)
    {
        Debug.Log(direction+"Deneme");
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider trigger)
    {
        
    }
}
