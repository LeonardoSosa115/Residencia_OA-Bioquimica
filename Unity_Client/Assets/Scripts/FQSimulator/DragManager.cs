using UnityEngine;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance { get; private set; }
    public bool IsDragging { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool TryStartDrag()
    {
        if (IsDragging) return false;
        IsDragging = true;
        return true;
    }

    public void EndDrag()
    {
        IsDragging = false;
    }
}