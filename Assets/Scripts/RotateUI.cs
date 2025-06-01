using UnityEngine;

public class RotateUI : MonoBehaviour
{
    public float rotationSpeed = 5f; // degrees per second
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.unscaledDeltaTime);
    }
}