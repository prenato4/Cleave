using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer; // Referência ao LineRenderer que representa a linha
    public float speed = 5f; // Velocidade da plataforma
    private int currentWaypointIndex = 0;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Define o Rigidbody como kinematic para movimento controlado por script
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Interpolação para movimento suave
    }

    void Update()
    {
        if (currentWaypointIndex < lineRenderer.positionCount)
        {
            Vector2 targetPosition = lineRenderer.GetPosition(currentWaypointIndex);
            rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));

            if ((Vector2)transform.position == targetPosition)
            {
                currentWaypointIndex++;
            }
        }
    }
}