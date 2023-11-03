using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            Destroy(collision.gameObject);
        else
        {
            Transform transform = collision.gameObject.transform;
            Vector2 spawnPosition;

            if (transform.position.x <= 0)
            {
                spawnPosition = new Vector2(transform.position.x + 10, transform.position.y + 7);
            }
            else
            {
                spawnPosition = new Vector2(transform.position.x - 3, transform.position.y + 7);
            }
            transform.position = spawnPosition;
        }
    }
}
