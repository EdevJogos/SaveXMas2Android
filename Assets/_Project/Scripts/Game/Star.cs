using UnityEngine;

public class Star : MonoBehaviour
{
    private float _changex;
    private float _directionx = 1;

    private void Awake()
    {
        GameCEO.onGameStateChanged += GameCEO_onGameStateChanged;
    }

    private void OnDestroy()
    {
        GameCEO.onGameStateChanged -= GameCEO_onGameStateChanged;
    }

    private void Start()
    {
        _directionx = Random.Range(0, 2) == 1 ? 1 : -1;
        _changex = Time.time + 3f;
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY)
            return;

        if(Time.time >= _changex)
        {
            _directionx = _directionx * -1;

            _changex = Time.time + Random.Range(2f, 10f);
        }

        float __x = _directionx * 3f;
        float __y = -0.5f;

        transform.Translate(new Vector2(_directionx, __y) * Time.deltaTime);

        if(transform.localPosition.y < -7f)
        {
            Destroy(gameObject);
        }
    }

    private void GameCEO_onGameStateChanged()
    {
        if (GameCEO.State == GameState.GAME_OVER || GameCEO.State == GameState.INTRO)
        {
            Destroy(gameObject);
        }
    }
}
