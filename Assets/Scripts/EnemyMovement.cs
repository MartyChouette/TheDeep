using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    public GameObject playerObj;
    public LayerMask playerLayer;
    public float detectionRadius = 20f;
    public float moveSpeed = 10f;

    public Rigidbody rb;

    public string currentSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;

        rb = gameObject.GetComponent<Rigidbody>();
        playerObj = GameObject.Find("Player");
        Debug.Log("playerObj: " + playerObj);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Vector3.zero;

        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, detectionRadius, playerLayer);

        if (hits.Length > 0)
        {
            Debug.Log("Found player");
            transform.position = Vector3.MoveTowards(transform.position, playerObj.transform.position, moveSpeed * Time.deltaTime);
        }

        // get start positions
        // Vector3 startPos = rb.position;
        // Vector3 targetPos = playerObj.transform.position;
        // // Direction we want to go
        // Vector3 direction = targetPos - startPos;
        // // Normalize it!
        // direction = direction.normalized;
        // newPos += moveSpeed * Time.deltaTime * direction;


        // if (newPos.x > 8) {
        //     newPos.x = -8;
        // }

        // rb.MovePosition(newPos);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene(currentSceneName);
        }
        // else if (collision.gameObject.CompareTag("Wall"))
        // {
        //     Debug.Log("hit wall");
        // }
    }
}
