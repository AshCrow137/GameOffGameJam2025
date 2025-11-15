using UnityEngine;

public class TestTRoate : MonoBehaviour
{
    public float rotationSpeed = 90f; // скорость вращения в градусах в секунду

    void Update()
    {
        // Вращаем объект вокруг оси Y каждый кадр
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
