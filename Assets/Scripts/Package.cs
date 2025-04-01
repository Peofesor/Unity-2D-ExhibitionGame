using UnityEngine;
using UnityEngine.EventSystems;

public class Package : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int movementPhase = 0; // 0: Nach oben, 1: Nach rechts, 2: Nach unten
    public enum PackageType { Truck, Ship, Plane }
    public PackageType type;
    private Vector3 startPos;
    private bool isDragging = false;
    private Camera mainCamera;

    // Öffentliche Eigenschaft für isDragging
    public bool IsDragging
    {
        get { return isDragging; }
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Bewegung über EventSystem
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(eventData.position);
        mousePos.z = transform.position.z; // Behalte die ursprüngliche Z-Position bei
        transform.position = mousePos;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        isDragging = true;
        startPos = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"EndDrag on {name}");
        isDragging = false;

        // Prüfe, ob das Paket über einem Gabelstapler losgelassen wurde
        Collider2D collider = Physics2D.OverlapPoint(transform.position);

        if (collider != null && collider.CompareTag("Forklift")) // && Forklift is not full yet
        {
            Debug.Log($"Package {name} dropped on Forklift: {collider.gameObject.name}");

            // Hole das Forklift-Skript
            Forklift forklift = collider.GetComponent<Forklift>();

            if (forklift != null)
            {
                // Prüfe, ob der Gabelstapler noch Kapazität hat
                if (forklift.currentLoad < forklift.capacity)
                {
                    forklift.currentLoad++;
                    // Rufe die OnTriggerEnter2D-Methode manuell auf
                    forklift.OnTriggerEnter2D(GetComponent<Collider2D>());
                }
                else
                {
                    // Gabelstapler ist voll, zurück zur Startposition
                    transform.position = startPos;
                }
            }
            else
            {
                // Kein Gabelstapler an dieser Position, zurück zur Startposition
                transform.position = startPos;
            }
        }
        else
        {
            // Kein Gabelstapler an dieser Position, zurück zur Startposition
            transform.position = startPos;
        }
    }
}