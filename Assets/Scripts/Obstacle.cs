using UnityEngine;
using UnityEngine.Rendering;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float randomMinSize = 0.5f;
    public float randomMaxSize = 2.0f;

    public float minSpeed = 50f;
    public float maxSpeed = 150f;

    public float maxSpinSpeed = 10f;

    Rigidbody2D rigidbody2;

    void Start()
    {
        float randomSize = Random.Range(randomMinSize, randomMaxSize);
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        Vector2 randomDirection = Random.insideUnitCircle;
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);

        transform.localScale = new Vector3(randomSize, randomSize, 1);

        rigidbody2 = GetComponent<Rigidbody2D>();
        rigidbody2.AddForce(randomDirection * randomSpeed);
        rigidbody2.AddTorque(randomTorque);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
