using UnityEngine;
using System.Linq;

public class Yeti : Agent
{
    public int id;
    public SpriteRenderer spriteRenderer;
    public AudioSource feetAudioSource;

    private int _direction;
    private float _roamTime;
    private Gift _target;
    private IOrderedEnumerable<Gift> _orderedElements;

    public void Initialize(int p_id)
    {
        id = p_id;
        SetTarget();
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY)
            return;

        if (_target == null)
        {
            Roam();
            SetTarget();
            return;
        }

        if (!feetAudioSource.isPlaying)
        {
            AudioManager.PlaySFX(SFXOccurrence.FOOTSTEP, feetAudioSource, Random.Range(0, 6), 3f);
        }

        int __direction = Mathf.FloorToInt(transform.localPosition.x - _target.transform.localPosition.x);
        Vector2 __targetDropPoint = new Vector2(_target.transform.localPosition.x, transform.localPosition.y);

        if(__direction != 0) spriteRenderer.flipX = __direction > 0;

        transform.localPosition = Vector2.MoveTowards(transform.localPosition, __targetDropPoint, 10f * Time.deltaTime);
    }

    private void SetTarget()
    {
        if (StageManager.OnScreenGifts.Count == 0)
            return;

        _orderedElements = StageManager.OnScreenGifts.OrderBy((__g) => Vector2.Distance(transform.localPosition, __g.transform.localPosition));

        int __total = _orderedElements.Count();

        for (int __i = 0; __i < __total; __i++)
        {
            Gift __gift = _orderedElements.ElementAt(__i);

            float __myDistance = Vector2.Distance(transform.localPosition, __gift.transform.localPosition);
            float __playerDistance = Vector2.Distance(__gift.transform.localPosition, Player.MyPosition);

            if(!__gift.onIt && __myDistance < __playerDistance)
            {
                _target = __gift;
                _target.onIt = true;

                break;
            }
        }
    }

    private void Roam()
    {
        if(Time.time >= _roamTime)
        {
            _direction = Random.Range(0, 2) == 1 ? 1 : -1;
            _roamTime = Time.time + 0.3f;
        }

        spriteRenderer.flipX = _direction < 0;
        transform.Translate(Vector2.right * _direction * 8f * Time.deltaTime);

        float __x = Mathf.Clamp(transform.localPosition.x, CameraManager.HorizontalLimit.x, CameraManager.HorizontalLimit.y);

        transform.localPosition = new Vector2(__x, transform.localPosition.y);
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.tag == "Gift")
        {
            CatchGift(p_other.GetComponent<Gift>(), transform.localPosition * new Vector2(1, 0.75f));
        }
    }
}