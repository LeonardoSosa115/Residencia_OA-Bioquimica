using UnityEngine;
using UnityEngine.InputSystem;

public enum ParticleType
{
    Proton,
    Neutron,
    Electron
}

public class Particle : MonoBehaviour
{
    [Header("Tipo")]
    public ParticleType particleType;

    [Header("Estado")]
    public bool isInNucleus = false;

    [Header("Referencias")]
    public AtomController atomController;
    public ParticleSpawner particleSpawner;
    public ParticleContainer myContainer;


    private Rigidbody2D rb;
    private bool isDragging = false;
    private bool isInContainer = true;
    private Vector3 offset;
    private Camera mainCamera;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            Collider2D[] hits = Physics2D.OverlapCircleAll(mousePos2D, 0.1f);

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject)
                {
                    // Solo empieza si no hay otro drag activo
                    if (DragManager.Instance != null && !DragManager.Instance.TryStartDrag())
                        return;

                    isDragging = true;

                    if (rb != null)
                    {
                        rb.gravityScale = 0;
                        rb.linearVelocity = Vector2.zero;
                    }

                    mousePos.z = 0;
                    offset = transform.position - mousePos;
                    break;
                }
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            DragManager.Instance?.EndDrag();
            VerificarSoltar();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            VerificarSoltar();
        }

        if (isDragging)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
    }

    void VerificarSoltar()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (Collider2D hit in hits)
        {
            DropZone dropZone = hit.GetComponent<DropZone>();
            if (dropZone != null)
            {
                if (isInNucleus)
                    RegresarAlNucleo();
                else
                    dropZone.AbsorberParticula(this);
                return;
            }
        }

        // Se soltó fuera de la DropZone
        if (isInNucleus)
        {
            RevertirDelAtomo();
        }
        else
        {
            // Estaba en el contenedor o flotando → regresa al contenedor con gravedad
            RegresarAlContenedor();
        }
    }

    void RegresarAlContenedor()
    {
        if (myContainer != null)
        {
            // Teletransporta al punto de spawn del contenedor
            Vector3 spawnPos = myContainer.spawnPoint != null
                ? myContainer.spawnPoint.position
                : myContainer.transform.position;

            spawnPos += new Vector3(
                Random.Range(-0.2f, 0.2f),
                Random.Range(-0.2f, 0.2f),
                0
            );

            transform.position = spawnPos;
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void RegresarAlNucleo()
    {
        // Vuelve a la posición del núcleo
        if (atomController != null)
            transform.position = atomController.transform.position;

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void RevertirDelAtomo()
    {
        if (atomController != null)
        {
            switch (particleType)
            {
                case ParticleType.Proton:
                    atomController.RemoveSpecificProton(gameObject);
                    break;
                case ParticleType.Neutron:
                    atomController.RemoveSpecificNeutron(gameObject);
                    break;
                case ParticleType.Electron:
                    // Remueve este electrón específico en lugar del último
                    atomController.RemoveSpecificElectron(gameObject);
                    break;
            }
        }

        if (particleSpawner != null)
        {
            switch (particleType)
            {
                case ParticleType.Proton:
                    particleSpawner.ContainerProtones.AddParticle();
                    break;
                case ParticleType.Neutron:
                    particleSpawner.ContainerNeutrones.AddParticle();
                    break;
                case ParticleType.Electron:
                    particleSpawner.ContainerElectrones.AddParticle();
                    break;
            }
            particleSpawner.ActualizarUI();
        }

        isInNucleus = false;
        Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Container"))
            isInContainer = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Container"))
        {
            isInContainer = true;
            if (!isDragging)
                rb.gravityScale = 1f;
        }
    }

    public void SetInContainer(bool value)
    {
        isInContainer = value;
    }
}