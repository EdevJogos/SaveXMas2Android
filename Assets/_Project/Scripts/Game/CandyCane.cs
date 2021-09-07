using UnityEngine;

public class CandyCane : MonoBehaviour
{
    private void Awake()
    {
        if(!Application.isMobilePlatform)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        float __acceleration = Input.acceleration.x;

        GetComponent<Animator>().SetFloat("Speed", __acceleration > 0 ? 1 : -1f);
    }
}
