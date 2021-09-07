using UnityEngine;

public class EScale : MonoBehaviour
{
    public float speed;
    public float from, to;

    private bool _grow = true;
    private Vector2 _target;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(from, from);

        _target = new Vector2(to, to);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector2.MoveTowards(transform.localScale, _target, speed * Time.deltaTime);

        if(Mathf.Abs(transform.localScale.x - _target.x) <= 0)
        {
            _grow = !_grow;
            _target = _grow ? new Vector2(to, to) : new Vector2(from, from);
        }
    }
}
