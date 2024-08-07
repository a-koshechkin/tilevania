using System.Collections;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private readonly float _levelLoadTime = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var potentialPlayer = collision.GetComponent<PlayerMovement>();
        if (potentialPlayer != null)
        {
            StartCoroutine(nameof(ExitLevel));
        }
    }

    private IEnumerator ExitLevel()
    {
        yield return new WaitForSeconds(_levelLoadTime);
        FindObjectOfType<ScenePersist>().ResetScenePersists();
        SceneLoader.LoadNextScene();
    }

}
