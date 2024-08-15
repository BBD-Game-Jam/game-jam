using UnityEngine;
using UnityEngine.U2D;

public class CameraScript : MonoBehaviour
{
    public Camera cam;
    public Transform character;
    public SpriteShapeController terrain;
    public Vector3 offsetFromCharacter;
    public float padding = 10f;  // Padding around the terrain and character

    private float terrainHeight;
    private float cameraHeight;

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void Update()
    {
        if (character != null && terrain != null)
        {
            // Follow the character
            float cameraX = character.position.x + offsetFromCharacter.x;
            float cameraY = character.position.y + offsetFromCharacter.y;
            float cameraZ = offsetFromCharacter.z;

            // Set the camera's new position
            Vector3 newCameraPosition = new(cameraX, cameraY, cameraZ);
            cam.transform.position = newCameraPosition;
        }
    }
}
