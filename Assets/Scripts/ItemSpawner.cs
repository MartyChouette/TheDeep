using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float chanceToDelete = .4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomDeletePercentage = Random.Range(0, .8f);

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
