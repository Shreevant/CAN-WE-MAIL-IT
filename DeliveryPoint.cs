using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    public Transform dropZone;
    public bool playerInside = false;

    private void Awake() { gameObject.SetActive(false); }

    public bool IsAtPoint(Vector3 playerPos)
    {
        if (dropZone == null) dropZone = transform;
        return Vector3.Distance(playerPos, dropZone.position) < 3f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInside = false;
    }
}
