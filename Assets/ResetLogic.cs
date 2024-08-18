using UnityEngine;

public class FloatingOrigin : MonoBehaviour
{
    public Transform character;
    public float threshold = 500f;

    void Update()
    {
        if (Mathf.Abs(character.position.x) > threshold)
        {
            Vector3 offset = new(character.position.x, 0, 0);

            character.position -= offset;

            Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            cam.transform.position -= offset;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Environment"))
            {
                obj.transform.position -= offset;
            }
            
        }
    }
}
