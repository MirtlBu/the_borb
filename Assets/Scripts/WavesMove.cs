using UnityEngine;

public class WavesMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Update()
    {
       float rotationSpeed = 360f;
       transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime / 8);
        
    }
}
