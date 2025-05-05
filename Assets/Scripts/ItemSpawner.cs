using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Range(0f, 1f)]
    private float deleteProbability = 0.75f;


    void Awake()
    {
        TryDeleteSelf();
    }
    void Start()      
    {
       
    }


    void TryDeleteSelf()
    {
        float roll = Random.value;              // 0.0–1.0 inclusive
        Debug.Log($"{name}: roll={roll:F2}, p={deleteProbability:F2}");

        if (roll < deleteProbability)           // 40 % chance by default
            Destroy(gameObject);                // goodbye!
    }
}