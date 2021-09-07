using UnityEngine;

public class EGrow : MonoBehaviour
{
    public float speed;
    public Vector2 from, to;

    private bool _grow;

    private void Update()
    {
        if (_grow)
        {
            transform.localScale = Vector2.MoveTowards(transform.localScale, to, speed * Time.deltaTime);

            if (transform.localScale.x >= to.x) _grow = false;
        }
        else
        {
            transform.localScale = Vector2.MoveTowards(transform.localScale, from, speed * Time.deltaTime);

            if (transform.localScale.x <= from.x) _grow = true;
        }
    }
}
