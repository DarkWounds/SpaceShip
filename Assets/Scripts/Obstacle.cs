using UnityEngine;
using UnityEngine.Rendering;

public class NewMonoBehaviourScript : MonoBehaviour
{

    // Intervalul de scalare aleatorie pentru fiecare obstacol.
    public float randomMinSize = 0.5f;
    public float randomMaxSize = 2.0f;

    // Viteza initiala este ajustata in functie de marime.
    public float minSpeed = 200f;
    public float maxSpeed = 250f;

    // Viteza maxima de rotatie aplicata obstacolului.
    public float maxSpinSpeed = 10f;

    Rigidbody2D rigidbody2;

    void Start()
    {
        // Generam mărimea obstacolului in intervalul configurat.
        float randomSize = Random.Range(randomMinSize, randomMaxSize);
        // Obstacolele mari se misca mai lent, cele mici mai rapid.
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        // Directie aleatorie in plan 2D.
        Vector2 randomDirection = Random.insideUnitCircle;
        // Rotatie aleatorie, in sens orar sau anti-orar.
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);

        transform.localScale = new Vector3(randomSize, randomSize, 1);

        rigidbody2 = GetComponent<Rigidbody2D>();
        // Aplicam impulsul initial si rotatia.
        rigidbody2.AddForce(randomDirection * randomSpeed);
        rigidbody2.AddTorque(randomTorque);
    }

    void Update()
    {
        
    }
}
