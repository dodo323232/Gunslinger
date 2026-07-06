using Unity.Mathematics;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float newY;
    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        newY = -3.29f + math.sin(Time.time * 8f) * 0.3f;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        if(transform.position.x < -13f)
        {
            Destroy(gameObject);
        }
    }
}