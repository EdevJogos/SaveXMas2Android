using UnityEngine;

public class Points : MonoBehaviour
{
    public void OnAnimationCompleted()
    {
        Destroy(gameObject);
    }
}
