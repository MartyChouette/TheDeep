using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    float chanceToDelete = .4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomDeletePercentage = Random.Range(0, 1);

        if (randomDeletePercentage > chanceToDelete)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
