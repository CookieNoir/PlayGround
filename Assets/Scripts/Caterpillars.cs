using UnityEngine;

public class Caterpillars : MonoBehaviour
{
    public Example example;
    private int collisionCount;

    void Start()
    {
        collisionCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Caterpillar")) return;
        collisionCount++;
        example.SetTouchSurface(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Caterpillar")) return;
        collisionCount--;
        if (collisionCount == 0)
            example.SetTouchSurface(false);
    }
}
