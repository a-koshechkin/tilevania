using UnityEngine;
using static Constants;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _deathKick = 10f;
    private Rigidbody2D _rb;
    private bool _isDead = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector2(_runSpeed, _rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            var potentialDeathCollider = collision.GetComponent<DeathCollider>();
            if (potentialDeathCollider != null)
            {
                potentialDeathCollider.OnColliderHit(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isDead) { return; }
        _runSpeed = -_runSpeed;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    public void Kill()
    {
        _isDead = true;
        if (_rb != null)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            _rb.velocity = new Vector2(0, _deathKick);
            GetComponent<Animator>().SetTrigger(PlayerStateTags[PlayerState.Dying]);
        }
    }
}
