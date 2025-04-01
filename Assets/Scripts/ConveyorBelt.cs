using UnityEditor.Rendering;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject packagePrefabTruck;
    public GameObject packagePrefabPlane;
    public GameObject packagePrefabShip;
    public float spawnRate = 4f; // Paket alle x Sekunden
    public float speed = 2f; // Bewegungsgeschwindigkeit
    private float timer = 0f;

    // Grenzen für die Bewegung
    private float bottomY = -4f; // Y-Position unten
    private float topY = 4f;     // Y-Position oben
    private float rightX = -6f;   // X-Position rechts

    void Start()
    {
        SpawnPackage();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnPackage();
            timer = 0f;
        }

        // Bewege alle Pakete
        foreach (Transform child in transform)
        {
            Package package = child.GetComponent<Package>();
            if (package == null) continue;

            Vector3 direction = Vector3.zero;

            // Überspringe die Bewegung, wenn das Paket gezogen wird
            if (package.IsDragging) continue;

            // Phase 0: Nach oben
            if (package.movementPhase == 0)
            {
                direction = Vector3.up;
                if (child.position.y >= topY)
                {
                    package.movementPhase = 1; // Wechsle zu Phase 1 (nach rechts)
                    child.position = new Vector3(child.position.x, topY, child.position.z);
                }
            }

            // Phase 1: Nach rechts
            else if (package.movementPhase == 1)
            {
                direction = Vector3.right;
                if (child.position.x >= rightX)
                {
                    package.movementPhase = 2; // Wechsle zu Phase 2 (nach unten)
                    child.position = new Vector3(rightX, child.position.y, child.position.z);
                }
            }

            // Phase 2: Nach unten
            else if (package.movementPhase == 2)
            {
                direction = Vector3.down;
                if (child.position.y <= bottomY)
                {
                    Debug.Log($"Destroying package {child.name} at position {child.position}");
                    // Paket hat das Ende erreicht
                    GameManager.Instance.LoseHeart();
                    Destroy(child.gameObject);
                }
            }

            // Bewege das Paket in die aktuelle Richtung!
            child.position += direction * speed * Time.deltaTime;

            //// wenn Pakete colliden, dann LoseHeart()
            // GameManager.Instance.LoseHeart(); // Game Over-Logik später
            // Destroy(child.gameObject);
        }
    }

    void SpawnPackage()
    {
        // Wähle zufällig ein Prefab aus
        int randomIndex = Random.Range(0, 3); // 0, 1 oder 2

        GameObject selectedPrefab;
        Package.PackageType selectedType;

        switch (randomIndex)
        {
            case 0:
                selectedPrefab = packagePrefabTruck;
                selectedType = Package.PackageType.Truck;
                break;
            case 1:
                selectedPrefab = packagePrefabPlane;
                selectedType = Package.PackageType.Plane;
                break;
            case 2:
                selectedPrefab = packagePrefabShip;
                selectedType = Package.PackageType.Ship;
                break;
            default:
                Debug.Log("What happened?");

                selectedPrefab = null;
                selectedType = Package.PackageType.Truck;
                break;
        }

        // Spawne das ausgewählte Prefab
        GameObject pkg = Instantiate(selectedPrefab, new Vector3(-8f, -5, -1), Quaternion.identity, transform);
        Package package = pkg.GetComponent<Package>();

        if (package == null)
        {
            Debug.LogError($"Package component not found on {pkg.name} after instantiation!");
            Destroy(pkg); // Zerstöre das Paket, um weitere Fehler zu vermeiden
            return;
        }

        // Debugge die Eigenschaften des instanziierten Pakets
        SpriteRenderer sr = pkg.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError($"SpriteRenderer not found on {pkg.name} after instantiation!");
            Destroy(pkg);
            return;
        }

        // Setze den Typ des Pakets entsprechend dem Prefab
        package.type = selectedType;
    }
}