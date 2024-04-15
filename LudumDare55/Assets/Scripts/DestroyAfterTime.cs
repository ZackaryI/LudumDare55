using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float destroyAfterTime = 1f;

    private void Start()
    {
        Destroy(gameObject, destroyAfterTime);
    }
}
