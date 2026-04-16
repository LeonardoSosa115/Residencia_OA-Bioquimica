using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleButtonHover : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Configuración")]
    public float hoverOffsetY = 10f;
    public float animSpeed    = 8f;

    private Vector2 originalPosition;
    private Vector2 targetPosition;
    private RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        originalPosition = rt.anchoredPosition;
        targetPosition   = originalPosition;
    }

    void Update()
    {
        rt.anchoredPosition = Vector2.Lerp(
            rt.anchoredPosition, 
            targetPosition, 
            Time.deltaTime * animSpeed
        );
    }

    public void OnPointerEnter(PointerEventData e)
    {
        targetPosition = originalPosition + Vector2.up * hoverOffsetY;
    }

    public void OnPointerExit(PointerEventData e)
    {
        targetPosition = originalPosition;
    }

    public void OnPointerDown(PointerEventData e)
    {
        targetPosition = originalPosition + Vector2.down * 4f;
    }

    public void OnPointerUp(PointerEventData e)
    {
        targetPosition = originalPosition + Vector2.up * hoverOffsetY;
    }
}