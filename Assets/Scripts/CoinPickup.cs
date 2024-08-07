using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip _coinPickupSound;
    [SerializeField] private float _coinCost = 1f;

    private bool _wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_wasCollected)
        {
            var potentialPlayer = collision.GetComponent<PlayerMovement>();
            if (potentialPlayer != null)
            {
                _wasCollected = true;
                AudioSource.PlayClipAtPoint(_coinPickupSound, Camera.main.transform.position);
                FindObjectOfType<GameSession>().AddScore(_coinCost);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
