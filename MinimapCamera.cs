using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        player = transform.parent;
    }

    void LateUpdate()
    {
        // Follow player's position
        transform.position = new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        );

        // Keep minimap camera rotation fixed
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}