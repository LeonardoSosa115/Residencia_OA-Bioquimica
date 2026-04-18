using UnityEngine;

public class ContainerWalls : MonoBehaviour
{
    [Header("Tamaño del contenedor")]
    public float width  = 3f;
    public float height = 3f;

    void Start()
    {
        CrearParedes();
    }

    void CrearParedes()
    {
        CrearPared("Fondo",    new Vector2(0, -height / 2), new Vector2(width, 0.1f));
        CrearPared("Techo",    new Vector2(0,  height / 2), new Vector2(width, 0.1f));
        CrearPared("Izquierda",new Vector2(-width / 2, 0),  new Vector2(0.1f, height));
        CrearPared("Derecha",  new Vector2( width / 2, 0),  new Vector2(0.1f, height));
    }

    void CrearPared(string nombre, Vector2 localPos, Vector2 size)
    {
        GameObject pared = new GameObject(nombre);
        pared.transform.SetParent(transform);
        pared.transform.localPosition = localPos;

        BoxCollider2D col = pared.AddComponent<BoxCollider2D>();
        col.size = size;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }   

}