using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Timpul total de supravietuire, folosit la calculul scorului.
    private float elapsedTime = 0f;
    // Scorul curent afisat in UI.
    private float score = 0f;
    // Blocheaza input-ul si actualizarea scorului dupa moarte.
    private bool isDead = false;

    public float scoreMultiplier = 10f;
    public float thrustForce = 5f;
    public float maxSpeed = 10f;

    Rigidbody2D rb;

    public UIDocument uiDocument;
    private Label scoreText;

    public GameObject explosionEffect;

    private Button restartButton;

    private void OnEnable()
    {
        // Daca nu este asignat din Inspector, cautam automat UIDocument-ul din scena.
        if (uiDocument == null)
        {
            uiDocument = FindAnyObjectByType<UIDocument>();
        }

        if (uiDocument != null && uiDocument.rootVisualElement != null)
        {
            // Legam elementele UI folosite in gameplay.
            scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
            restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
            if (restartButton != null)
            {
                // Butonul de restart este ascuns pana la moarte.
                restartButton.style.display = DisplayStyle.None;
                restartButton.clicked += ReloadScene;
            }
        }
    }

    private void MovePlayer()
    { 
        if (isDead) return;
        // Cand este apasat click stanga, nava accelereaza spre pozitia mouse-ului.
        if (Mouse.current.leftButton.isPressed)
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            transform.up = direction;
            rb.AddForce(direction * thrustForce);
        }

        // Limitam viteza maxima pentru control mai bun al navei.
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void UpdateScore()
    {
        if (isDead) return;
        // Scorul creste in timp, apoi este amplificat cu un multiplicator.
        elapsedTime += Time.deltaTime;
        score = (int)(elapsedTime * scoreMultiplier);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void Start()
    {
        // Referinta la fizica navei.
        rb = GetComponent<Rigidbody2D>();

        if (scoreText == null && uiDocument != null && uiDocument.rootVisualElement != null)
        {
            scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        }
    }

    void Update()
    {
        UpdateScore();
        MovePlayer();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        isDead = true;

        // Efect vizual la distrugere.
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Ascundem modelul navei si dezactivam coliziunile dupa impact.
        foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        foreach (var c in GetComponentsInChildren<Collider2D>()) c.enabled = false;

        if (rb != null)
        {
            // Oprim complet simularea fizica.
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        // Afisam butonul de restart doar dupa moarte.
        if (restartButton != null)
            restartButton.style.display = DisplayStyle.Flex;
    }

    void ReloadScene()
    {
        // Reincarca scena curenta pentru un nou joc.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   
    }

    private void OnDisable()
    {
        // Curatam listener-ul pentru a evita abonari duplicate.
        if (restartButton != null)
        {
            restartButton.clicked -= ReloadScene;
        }
    }

}