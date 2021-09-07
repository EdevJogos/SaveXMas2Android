using System.Collections;
using UnityEngine;

public class Player : Agent
{
    private static Player Instance;

    public static event System.Action<bool> onSpecialActive;

    public static bool SpecialActive;
    public static Vector2 MyPosition { get { return Instance.transform.localPosition; } }

    public SpriteRenderer spriteRenderer;
    public Transform runParticle;
    public ParticleSystem runParticleSys;
    public AudioSource feetAudioSource;

    private bool _jumping;
    private int _currentState, _currentDirection;
    private float _speedBoost = 1f;
    private float _jumpDuration = 1f, _jumpAcceleration;
    private Animator _animator;

    public void Initiate()
    {
        Instance = this;

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY)
            return;

        float __acceleration = Application.isMobilePlatform ? Mathf.Clamp(Input.acceleration.x * 3, -1, 1) : Input.GetAxis("Horizontal");

        if(!_jumping) UpdateMoveState(__acceleration);
        UpdateDirection(__acceleration);

        transform.Translate(__acceleration * 12f * _speedBoost * Time.deltaTime, 0f, 0f);

        if (!_jumping && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            AudioManager.PlaySFX(SFXOccurrence.JUMP, 0);
            AudioManager.PlaySFX(SFXOccurrence.JUMP, 1);

            _animator.SetTrigger("Jump");

            _jumpAcceleration = 12;
            _jumpDuration = Time.time + 0.25f;
            _jumping = true;
        }

        if (_jumping)
        {
            if(Time.time >= _jumpDuration)
            {
                _jumpAcceleration = Mathf.MoveTowards(_jumpAcceleration, 12f, 35f * Time.deltaTime);
                transform.Translate(Vector2.down * _jumpAcceleration * Time.deltaTime);
            }
            else
            {
                _jumpAcceleration = Mathf.MoveTowards(_jumpAcceleration, 0f, 70f * Time.deltaTime);
                transform.Translate(Vector2.up * 12f * Time.deltaTime);

                if (_jumpAcceleration <= 5f) runParticleSys.Stop();
            }

            if (transform.localPosition.y <= -3.4f)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, -3.4f);
                
                runParticleSys.Play();
                _jumping = false;
            }
        }

        //CLAMP
        float __x = Mathf.Clamp(transform.localPosition.x, CameraManager.HorizontalLimit.x, CameraManager.HorizontalLimit.y);
        float __y = Mathf.Clamp(transform.localPosition.y, -3.4f, 10f);

        transform.localPosition = new Vector2(__x, __y);
    }

    public void UpdateMoveState(float p_acceleration)
    {
        int __newState = Mathf.Abs(p_acceleration) == 0 ? 0 : Mathf.Abs(p_acceleration) <= 0.5f ? 1 : 2;

        if(__newState != 0 && !_jumping)
        {
            if(!feetAudioSource.isPlaying)
            {
                AudioManager.PlaySFX(SFXOccurrence.FOOTSTEP, feetAudioSource, Random.Range(0, 6), 3f);
            }
        }

        if (_currentState != __newState)
        {
            _currentState = __newState;
            _animator.SetInteger("State", _currentState);
        }
    }

    private void UpdateDirection(float p_acceleration)
    {
        int __newState = Mathf.Abs(p_acceleration) == 0 ? 0 : Mathf.Abs(p_acceleration) <= 0.5f ? 1 : 2;

        int __newDirection = p_acceleration == 0 ? _currentDirection : p_acceleration < 0 ? -1 : 1;

        if (__newDirection != _currentDirection)
        {
            _currentDirection = __newDirection;

            if (__newState != 0) spriteRenderer.flipX = p_acceleration < 0;
            runParticle.eulerAngles = p_acceleration < 0 ? Vector3.forward * -90 : Vector3.forward * 90;
        }
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if(p_other.tag == "Gift")
        {
            CatchGift(p_other.GetComponent<Gift>(), SpecialActive ? transform.localPosition * new Vector2(1, 0.1f) : (Vector2)transform.localPosition);
        }
        else if(p_other.tag == "Star")
        {
            if (SpecialActive)
                return;

            Destroy(p_other.gameObject);

            ParticlesDabatase.InstantiateParticle(Particles.STAR_BURST, transform.localPosition);

            onSpecialActive?.Invoke(true);

            StartCoroutine(RoutineSpecial());
        }
    }

    private IEnumerator RoutineSpecial()
    {
        SpecialActive = true;

        _speedBoost = 1.5f;

        float __growSize = 1.4f;
        Transform __transform = spriteRenderer.transform;

        while(__transform.localScale.x < __growSize)
        {
            __transform.localScale = Vector2.MoveTowards(__transform.localScale, new Vector2(__growSize, __growSize), 2f * Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(8f);

        while (__transform.localScale.x > 0.7f)
        {
            __transform.localScale = Vector2.MoveTowards(__transform.localScale, new Vector2(0.7f, 0.7f), 2f * Time.deltaTime);

            yield return null;
        }

        _speedBoost = 1f;

        onSpecialActive?.Invoke(false);

        SpecialActive = false;
    }
}