using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 10f;

    void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(_bulletSpeed * Mathf.Sign(transform.localScale.x), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            var potentialEnemy = collision.GetComponent<EnemyMovement>();
            if (potentialEnemy != null)
            {
                potentialEnemy.Kill();
            }
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
