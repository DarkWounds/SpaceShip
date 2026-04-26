Liceul [Numele Liceului Tău] 

 

 

 

 

 

 

 

Jocul Space Survival

 

Lucrare de atestat profesional la informatică 

 

 

 

 

 

 

Candidat: [Numele Tău]    Îndrumător: [Numele Profesorului] 

 

 

 

 

 

 

 

 

2024 / 2025 

 

Introducere 

Încă din primele zile ale jocurilor video, explorarea spațiului cosmic și testarea reflexelor în fața unor pericole iminente au fost teme captivante pentru jucători. Acest joc continuă tradiția jocurilor de tip "survival arcade", oferind o experiență bazată pe strategie, control precis și reacții rapide. Scopul principal este simplu: ghidează racheta folosind mouse-ul și supraviețuiește cât mai mult timp evitând asteroizii și marginile suprafeței de joc.

Jocul se distinge prin mecanici de fizică captivante, unde asteroizii ricoșează și accelerează la fiecare impact, punând constant la încercare viteza de reacție a jucătorului.

Dincolo de partea de divertisment, acest proiect este unul educativ, care a permis aprofundarea conceptelor de programare Orientată pe Obiect în C#, logica matematică pentru orientarea în spațiu, logica fizicii 2D în motorul Unity, implementarea sistemelor de particule și designul interactiv folosind noul sistem UI Toolkit. Dezvoltarea jocului reprezintă o demonstrație practică a conceptelor asimilate în anii de liceu.

Pregătește-te pentru o aventură intergalactică și încearcă să obții cel mai mare scor!


Capitolul I – Regulile jocului 

1. Obiectivul principal 
Supraviețuirea pentru o perioadă cât mai îndelungată. Scorul crește constant, fiind direct proporțional cu timpul de supraviețuire.

2. Evitarea pericolelor 
Jucătorul trebuie să propulseze racheta evitând obstacolele din scenă (asteroizii) și marginile hărții. Coliziunea cu oricare dintre aceste entități va duce la distrugerea navei și oprirea cronometrării.

3. Controale 
Deplasarea navei se realizează direct prin intermediul mouse-ului. Apăsarea și menținerea **Click Stânga** (LMB) va orienta nava spre cursor și îi va aplica o forță de propulsie continuă în acea direcție, oferind un sentiment de navigație fluent, bazat pe inerție. 

4. Mecanici ale mediului 
Obiectele de tip obstacol au asignate proprietăți fizice speciale în Unity (un material fizic cu Bounciness mai mare de 1), ceea ce înseamnă că la fiecare coliziune a acestora între ele sau cu pereții, viteza lor crește exponențial. Acest detaliu ridică treptat gradul de dificultate al nivelului doar așteptând.


Capitolul II – Prezentarea aplicației 

Aplicația a fost dezvoltată în prealabil în mediul de dezvoltare Unity (Game Engine), fiind logic integrată prin limbajul C# și vizual asezonată prin interfețe flexibile din UI Toolkit.

* **Jucătorul (Nava spațială)**: Controlată de corpul fizic "Rigidbody2D", aceasta răspunde la input-urile de pe mouse calculând unghiul optim spre cursor. La coliziune cu un obstacol, nava își pierde controlul. În acel moment, scriptul instanțiază un efect de explozie din particule; afișajul vizual și coliziunile rachetei sunt dezactivate automat pentru a simula distrugerea totală a ei, iar jocul este pus "pe pauză" pentru a oferi șansa la resetare.
* **Obstacolele (Asteroizii)**: Sunt instanțiate la dimensiuni și viteze aleatorii la declanșarea scenei. Viteza de start ia în calcul o viteză invers proporțională cu dimensiunea generată. Adăugarea unei inerții unghiulare (Torque) randomizate subliniază mișcările impredictibile și haotice din cosmos.
* **Interfața cu utilizatorul (UI)**: Este realizată folosind standardele moderne prin UI Toolkit. Rulează independent dar interconectat cu "PlayerController.cs". Conține un Element Label pentru actualizarea permanentă a scorului și un buton de Restart. Butonul este ascuns (DisplayStyle.None) pe durata jocului și se afișează (DisplayStyle.Flex) doar după moartea jucătorului, reluând jocul la apăsare.


Capitolul III – Prezentarea codului sursă 

Scriptul pentru Asteroizi (Obstacole) - NewMonoBehaviourScript.cs:
Acest script gestionează inițializarea caracteristicilor fizice ale obstacolelor, propulsându-le în direcții și rotații haotice.

```csharp
using UnityEngine;
using UnityEngine.Rendering;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float randomMinSize = 0.5f;
    public float randomMaxSize = 2.0f;

    public float minSpeed = 200f;
    public float maxSpeed = 250f;

    public float maxSpinSpeed = 10f;

    Rigidbody2D rigidbody2;

    void Start()
    {
        float randomSize = Random.Range(randomMinSize, randomMaxSize);
        // Viteza este invers proporțională cu mărimea, simulând fizica reală
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);

        transform.localScale = new Vector3(randomSize, randomSize, 1);

        rigidbody2 = GetComponent<Rigidbody2D>();
        rigidbody2.AddForce(randomDirection * randomSpeed);
        rigidbody2.AddTorque(randomTorque);
    }

    void Update()
    {
        
    }
}
```

Scriptul pentru Jucător - PlayerController.cs:
Fiind nucleul jocului, "PlayerController.cs" este responsabil cu aplicarea forțelor fizice pe nava spațială în funcție de input-ul mouse-ului, cu verificarea stării vitale (isDead), a timer-ului care calculează scorul obținut și instanțiază particulele de explozie, pe lângă reafișarea elementelor UI necesare continuării experienței.

```csharp
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
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
        // Conectarea Automată cu UI Documentul din scenă
        if (uiDocument == null)
        {
            uiDocument = FindAnyObjectByType<UIDocument>();
        }

        if (uiDocument != null && uiDocument.rootVisualElement != null)
        {
            scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
            restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
            if (restartButton != null)
            {
                // Ascundem butonul de restart inițial
                restartButton.style.display = DisplayStyle.None;
                restartButton.clicked += ReloadScene;
            }
        }
    }

    private void MovePlayer()
    { 
        if (isDead) return; // Oprirea inputului vizibil post-moarte
        
        if (Mouse.current.leftButton.isPressed)
        {
            // Corectarea poziției world vs screen din mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            transform.up = direction;
            rb.AddForce(direction * thrustForce); // Adăugarea thrust-ului
        }

        // Limitarea vitezei maxime pentru a evita o accelerare infinită a jucătorului
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void UpdateScore()
    {
        if (isDead) return;
        elapsedTime += Time.deltaTime;
        score = (int)(elapsedTime * scoreMultiplier);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void Start()
    {
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

        if (explosionEffect != null)
        {
            // Declansam efectul vizual pentru explozie
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Ascundem visuals si dezactivam fizica in loc de destroy pentru siguranța scriptului
        foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        foreach (var c in GetComponentsInChildren<Collider2D>()) c.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        // Afișăm butonul de restarteaza
        if (restartButton != null)
            restartButton.style.display = DisplayStyle.Flex;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   
    }

    private void OnDisable()
    {
        // Dezabonarea de la evenimentele de UI pentru a preveni erori la reîncarcărea memoriei
        if (restartButton != null)
        {
            restartButton.clicked -= ReloadScene;
        }
    }

}
```


Bibliografie 

[1] https://docs.unity3d.com/Manual/index.html - Unity Engine Official Manual
[2] https://docs.unity3d.com/Packages/com.unity.inputsystem@1.11/manual/index.html - Unity Input System
[3] https://docs.unity3d.com/Manual/UIElements.html - Unity UI Toolkit Manual
