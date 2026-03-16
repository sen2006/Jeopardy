using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] GameObject toDestroy;

    public void DestroyTheObject() {
        Destroy(toDestroy);
    }
}
