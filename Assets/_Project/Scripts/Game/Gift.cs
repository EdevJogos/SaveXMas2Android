using UnityEngine;

public class Gift : MonoBehaviour
{
    public static event System.Action<Gift> onGiftDestroyed;

    public bool onIt;
    public bool destroy;
    public bool caught;
    public int id;
    public float weight;

    private bool _grow;
    private float _speed;

    public void Initialize(GiftData p_giftData)
    {
        id = p_giftData.id;
        weight = p_giftData.weight;

        transform.GetChild(id + 1).gameObject.SetActive(true);
        GetComponent<BoxCollider2D>().size = p_giftData.colliderSize;
        GetComponentInChildren<SpriteRenderer>().sprite = p_giftData.GetSprite();

        _speed = p_giftData.speed;
    }

    private void Update()
    {
        bool __update = GameCEO.State == GameState.PLAY ? true : GameCEO.State == GameState.INTRO ? true : false;

        if (!__update)
            return;

        if(caught)
        {
            Transform __transform = transform.GetChild(0);

            __transform.localScale = Vector2.MoveTowards(__transform.localScale, Vector2.zero, 8 * Time.deltaTime);

            if(__transform.localScale.x <= 0)
            {
                DestroyGift();
            }
        }
        else if(destroy)
        {
            Transform __transform = transform.GetChild(0);

            __transform.localScale = Vector2.MoveTowards(__transform.localScale, new Vector2(1.1f, 0.5f), 5 * Time.deltaTime);

            if (__transform.localScale.y <= 0.5f)
            {
                DestroyGift();
            }
        }
        else
        {
            if(_grow)
            {
                transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(1.1f, 1.1f), 0.3f * Time.deltaTime);

                if (transform.localScale.x >= 1.1f) _grow = false;
            }
            else
            {
                transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(1f, 1f), 0.3f * Time.deltaTime);

                if (transform.localScale.x <= 1f) _grow = true;
            }

            transform.Translate(new Vector2(StageManager.WindDirection, -1f) * _speed * StageManager.CurrentStage.speedRatio * Time.deltaTime);

            float __x = Mathf.Clamp(transform.localPosition.x, CameraManager.HorizontalLimit.x, CameraManager.HorizontalLimit.y);
            transform.localPosition = new Vector2(__x, transform.localPosition.y);
        }
    }

    private void DestroyGift()
    {
        onGiftDestroyed?.Invoke(this);

        Destroy(gameObject);
    }

    public void DestroyImmediately()
    {
        ParticlesDabatase.InstantiateParticle(Particles.CONFFETI, transform.localPosition);

        DestroyGift();
    }

    public void RequestDestroy()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        if (GameCEO.State == GameState.PLAY)
        {
            CameraManager.ShakeScreen(0.1f, weight);
        }

        ParticlesDabatase.InstantiateParticle(Particles.CONFFETI, transform.localPosition);

        destroy = true;
    }
}
