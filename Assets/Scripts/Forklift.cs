using UnityEngine;

public class Forklift : MonoBehaviour
{
    public int capacity = 3; // private machen und über getter
    public int currentLoad = 0;

    public void OnTriggerEnter2D(Collider2D other)
    {
        Package pkg = other.GetComponent<Package>();
        if (pkg != null)
        {
            Destroy(other.gameObject);
            if (currentLoad >= capacity)
            {
                MoveToTarget();
            }
        }
        else if (pkg != null)
        {
            GameManager.Instance.LoseHeart(); // Fehler
            Destroy(other.gameObject);
        }
    }

    void MoveToTarget()
    {
        // Automatische Bewegung zum Ziel (Platzhalter-Logik)
        Debug.Log("Fahre zu los");
        currentLoad = 0;
        GameManager.Instance.AddScore(1); // ein Punkt pro Forklift
    }
}