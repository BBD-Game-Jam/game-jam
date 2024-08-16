using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera cam;
    public Transform character;
    public Vector3 offsetFromCharacter;
    public float cameraHeightLimit = 1f;
    public float padding = 10f;  // Padding around the terrain and character
    public float smoothTime = 0.5f;

    Vector3 yVelocity = Vector3.zero;

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void Update()
    {
        if (character != null)
        {
            // Increase the height of the camera based on where the character is
            float targetSize = character.position.y > cameraHeightLimit ?
                (character.position.y / 2) + padding :
                (cameraHeightLimit / 2) + padding;

            // Smoothly adjust the orthographic size
            cam.orthographicSize = targetSize;

            // Follow the character
            float targetY = cam.orthographicSize;
            cam.transform.position = new Vector3(character.position.x + offsetFromCharacter.x, targetY, character.position.z + offsetFromCharacter.z);
        }
    }
}