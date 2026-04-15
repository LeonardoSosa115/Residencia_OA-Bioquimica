using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDebug : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK detectado en: " + gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("HOVER detectado en: " + gameObject.name);
    }
}