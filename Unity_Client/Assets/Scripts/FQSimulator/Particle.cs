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

    [Header("Referencias")]
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private bool isDragging = false;
    private bool isInContainer = true;

    [Header("Drag")]
    private Vector3 offset;
    private Camera mainCamera;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Evitar errores si no hay ratón conectado
        if (Mouse.current == null) return;

        // 1. Detectar si acabamos de HACER CLIC este frame
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            // Disparar un rayo para ver si tocamos el collider de esta partícula
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                rb.gravityScale = 0;
                rb.linearVelocity = Vector2.zero;
                
                mousePos.z = 0;
                offset = transform.position - mousePos;
            }
        }

        // 2. Detectar si SOLTAMOS el clic este frame
        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;

            // Si sigue dentro del contenedor, restaura gravedad
            if (isInContainer)
            {
                rb.gravityScale = 1f;
            }
            else
            {
                // Fuera del contenedor, flota
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
            }
        }

        // 3. Mover la partícula si la estamos arrastrando
        if (isDragging)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Container"))
        {
            isInContainer = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Container"))
        {
            isInContainer = true;
            
            // Solo aplicamos gravedad si no lo estamos agarrando con el ratón en este momento
            if (!isDragging) 
            {
                rb.gravityScale = 1f;
            }
        }
    }

    public void SetInContainer(bool value)
    {
        isInContainer = value;
    }
}