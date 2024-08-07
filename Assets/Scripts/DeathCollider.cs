using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    public void OnColliderHit(GameObject collision)
    {
        var potentialPlayer = collision.GetComponent<PlayerMovement>();
        if (potentialPlayer != null)
        {
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        collision.SetActive(false);
        Destroy(collision);
    }
}
