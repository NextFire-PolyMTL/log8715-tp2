using UnityEngine;

public class Velocity : MonoBehaviour
{
    public Vector3 velocity;
    
    void Update()
    {
        transform.localPosition += velocity * Time.deltaTime;
    }
}
