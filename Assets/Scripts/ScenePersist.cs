using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<ScenePersist>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersists()
    {
        Destroy(gameObject);
    }
}
