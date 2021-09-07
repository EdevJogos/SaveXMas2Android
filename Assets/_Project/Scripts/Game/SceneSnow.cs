using UnityEngine;

public class SceneSnow : MonoBehaviour
{
    public GameObject snowPoint;

    public void Start()
    {
        for (int __i = 0; __i < 100; __i++)
        {
            float __x = Random.Range(CameraManager.HorizontalLimit.x, CameraManager.HorizontalLimit.y);
            float __y = Random.Range(CameraManager.VerticalLimit.x, CameraManager.VerticalLimit.y);

            Vector2 __position = new Vector2(__x, __y);

            Instantiate(snowPoint, __position, Quaternion.identity, transform);
        }
    }
}
