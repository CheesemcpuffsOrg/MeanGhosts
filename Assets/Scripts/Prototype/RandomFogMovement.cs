using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFogMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform cloud;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float radius = 5f; 
    [SerializeField] private float speed = 2f;   
    [SerializeField] private float changeDirectionTime = 2f;
    [SerializeField] private Vector2 opacityRange;
    

    private Vector2 randomDirection;
    private float changeDirectionTimer;

    void Start()
    {
        SetRandomOpacity();

        // Set initial random direction
        SetRandomDirection();
    }

    void Update()
    {
        // Move the object
        MoveObject();

        // Update the timer to change direction
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0)
        {
            SetRandomDirection();
            changeDirectionTimer = changeDirectionTime;
        }
    }

    void SetRandomDirection()
    {
        // Pick a random point inside the unit circle and normalize to get direction
        randomDirection = Random.insideUnitCircle.normalized;
    }

    void SetRandomOpacity()
    {

        // Generate a random opacity value between 0 and 1
        float randomOpacity = Random.Range(opacityRange.x, opacityRange.y);

        // Get the current color of the sprite
        Color spriteColor = spriteRenderer.color;

        // Set the alpha value (opacity)
        spriteColor.a = randomOpacity;

        // Apply the new color with random opacity to the sprite
        spriteRenderer.color = spriteColor;
    }

    void MoveObject()
    {
        // Move the object in the random direction
        cloud.position += (Vector3)randomDirection * speed * Time.deltaTime;

        // Clamp the position to stay within the specified radius around the target
        Vector2 offset = cloud.position - target.position;
        if (offset.magnitude > radius)
        {
            // Move the object back within the radius
            cloud.position = target.position + (Vector3)offset.normalized * radius;
        }
    }
}
