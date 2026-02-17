using UnityEngine;

public class FloatMove : MonoBehaviour
{
    public float angle = 5f;      
    public float speed = 0.4f;      
    public float floatHeight = 1f;
    private Vector3 startPos;
    private Quaternion startRot; 
     void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

void Update()
{
    float z = Mathf.Sin(Time.time * speed) * angle;
    transform.rotation = startRot * Quaternion.Euler(0f, 0f, z);

    float yOffset = Mathf.Sin(Time.time * speed) * floatHeight;
    transform.position = startPos + new Vector3(0f, yOffset, 0f);
}
}
