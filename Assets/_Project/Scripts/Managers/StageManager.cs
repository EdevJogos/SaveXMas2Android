using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static float HireTimer = GameConfig.HIRE_TIMER, HireTime;

    public static float WindDirection;

    public static List<Gift> OnScreenGifts
    {
        get { return _onScreenGifts; }
    }

    public event System.Action onHireTimerUpdated;
    public event System.Action<Gift, Vector2> onScoreGift;
    public event System.Action onGiftLost;
    public event System.Action onGameOver;

    public static Stage CurrentStage;

    public GameObject[] gameplayObjects;
    public GameObject specialGraphics;
    public ParticleSystem snowParticleSystem;
    public GiftsWorker giftsWorker;
    public Star starPrefab;

    public int lostGifts, caughtGifts, streak, bestStreak;

    private bool _spawning, _spawnWaiting;
    private int _spawnQuantity = 0;
    private float _spawnTime, _quantityTime;

    private int _stageIndex;
    private float _windTime;
    private float _starTime;
    private List<Stage> _stages;
    private static List<Gift> _onScreenGifts = new List<Gift>(10);

    public void Initiate()
    {
        Agent.onGiftCaught += Agent_onGiftCaught;
        Gift.onGiftDestroyed += Gift_onGiftDestroyed;
        Player.onSpecialActive += Player_onSpecialActive;

        _stages = new List<Stage>(20)
        {
            new Stage(1.0f, new Vector2(1.0f, 1.0f), new Vector2(1, 3), new Vector2(2.0f, 3.0f), new Vector2(30f, 60f)),
            new Stage(1.0f, new Vector2(0.5f, 1.0f), new Vector2(1, 3), new Vector2(2.0f, 2.0f), new Vector2(15f, 60f)),

            new Stage(1.0f, new Vector2(0.5f, 1.0f), new Vector2(1, 3), new Vector2(1.0f, 2.0f), new Vector2(15f, 60f)),
            new Stage(1.0f, new Vector2(0.5f, 1.0f), new Vector2(1, 4), new Vector2(2.0f, 2.0f), new Vector2(15f, 60f)),

            new Stage(1.0f, new Vector2(0.5f, 1.0f), new Vector2(1, 4), new Vector2(1.0f, 2.0f), new Vector2(15f, 60f)),
            new Stage(1.1f, new Vector2(0.5f, 1.0f), new Vector2(2, 5), new Vector2(2.0f, 2.0f), new Vector2(15f, 60f)),

            new Stage(1.1f, new Vector2(0.5f, 0.8f), new Vector2(2, 5), new Vector2(1.0f, 2.0f), new Vector2(30f, 60f)),
            new Stage(1.1f, new Vector2(0.5f, 0.8f), new Vector2(2, 5), new Vector2(1.0f, 1.5f), new Vector2(30f, 60f)),

            new Stage(1.1f, new Vector2(0.4f, 0.8f), new Vector2(2, 5), new Vector2(1.0f, 1.0f), new Vector2(30f, 60f)),
            new Stage(1.1f, new Vector2(0.4f, 0.8f), new Vector2(2, 6), new Vector2(1.0f, 2.0f), new Vector2(30f, 60f)),

            new Stage(1.2f, new Vector2(0.4f, 0.8f), new Vector2(2, 6), new Vector2(1.0f, 1.0f), new Vector2(30f, 60f)),
            new Stage(1.2f, new Vector2(0.3f, 0.8f), new Vector2(3, 6), new Vector2(0.5f, 1.0f), new Vector2(30f, 60f)),

            new Stage(1.2f, new Vector2(0.3f, 0.8f), new Vector2(3, 6), new Vector2(0.5f, 1.0f), new Vector2(30f, 60f)),
            new Stage(1.2f, new Vector2(0.3f, 0.7f), new Vector2(3, 6), new Vector2(0.5f, 1.0f), new Vector2(30f, 60f)),

            new Stage(1.3f, new Vector2(0.2f, 0.7f), new Vector2(3, 6), new Vector2(0.5f, 1.0f), new Vector2(30f, 60f)),
            new Stage(1.3f, new Vector2(0.2f, 0.6f), new Vector2(3, 6), new Vector2(0.5f, 1.0f), new Vector2(30f, 60f)),

            new Stage(1.4f, new Vector2(0.2f, 0.6f), new Vector2(3, 6), new Vector2(0.5f, 1.0f), new Vector2(30f, 60f)),
            new Stage(1.4f, new Vector2(0.1f, 0.5f), new Vector2(3, 6), new Vector2(0.5f, 1.0f), new Vector2(30f, 60f)),

            new Stage(1.5f, new Vector2(0.1f, 0.5f), new Vector2(3, 6), new Vector2(0.5f, 1.0f), new Vector2(30f, 60f)),
            new Stage(2.0f, new Vector2(0.1f, 0.3f), new Vector2(3, 6), new Vector2(0.5f, 1.0f), new Vector2(30f, 60f)),
        };

        CurrentStage = _stages[0];
    }

    private void Update()
    {
        if (GameCEO.State == GameState.PAUSE || GameCEO.State == GameState.PLAY)
        {
            if (HireTimer > 0 && Time.time > HireTime)
            {
                HireTimer -= 1;
                HireTime = Time.time + 1; //Update each second

                onHireTimerUpdated?.Invoke();
            }
        }

        if (GameCEO.State != GameState.PLAY)
            return;

        if (!_spawning && TimerManager.PlayTime >= _spawnTime) { _spawning = true; _spawnQuantity = CurrentStage.Quantity; }

        if (_spawning)
        {
            if (_spawnWaiting && TimerManager.PlayTime >= _quantityTime) _spawnWaiting = false;

            if (!_spawnWaiting)
            {
                float __x = Random.Range(CameraManager.HorizontalLimit.x, CameraManager.HorizontalLimit.y);

                Gift __gift = giftsWorker.SpawnRandomGift(new Vector2(__x, CameraManager.VerticalLimit.y));

                _onScreenGifts.Add(__gift);

                AudioManager.PlaySFX(SFXOccurrence.GIFT_ENTER, __gift.id, 0.8f);

                _spawnQuantity--;

                _quantityTime = TimerManager.PlayTime + (Player.SpecialActive ? 0.1f : CurrentStage.Delay);
                _spawnWaiting = true;
            }

            if (_spawnQuantity == 0) { _spawnTime = TimerManager.PlayTime + CurrentStage.SpawnDelay; _spawning = false; }
        }

        if (TimerManager.PlayTime >= _starTime)
        {
            Instantiate(starPrefab, new Vector2(0, CameraManager.VerticalLimit.y * 1.1f), Quaternion.identity);

            _starTime = TimerManager.PlayTime + 120f;
        }

        if (!Player.SpecialActive && TimerManager.PlayTime >= _windTime)
        {
            StartCoroutine(StartWind());

            _windTime = TimerManager.PlayTime + CurrentStage.WindDelay;
        }
    }

    public void InitializeStage()
    {
        CurrentStage = _stages[0];

        caughtGifts = 0;
        lostGifts = 0;
        streak = 0;

        TimerManager.ResetTimers();
        TimerManager.AddTimer(new TimerManager.PlayTimer(true, GameConfig.STAGE_DURATION, LoadNextStage));

        _stageIndex = 0;
        _spawnTime = TimerManager.PlayTime + CurrentStage.SpawnDelay;
        _windTime = TimerManager.PlayTime + 30f;
        _starTime = TimerManager.PlayTime + 60f;

        ActiveGamePlayObjects(true);

        ResetHireTimer();
        onGiftLost?.Invoke();
    }

    public void ResetHireTimer()
    {
        HireTimer = GameConfig.HIRE_TIMER;
        HireTime = Time.time + 1;
        
        onHireTimerUpdated?.Invoke();
    }

    public void ActiveGamePlayObjects(bool p_active)
    {
        for (int __i = 0; __i < gameplayObjects.Length; __i++)
        {
            gameplayObjects[__i].SetActive(p_active);
        }
    }

    public void ClearGifts()
    {
        List<Gift> __temp = new List<Gift>(_onScreenGifts.Count);

        __temp.AddRange(_onScreenGifts);

        for (int __i = 0; __i < __temp.Count; __i++)
        {
            __temp[__i].DestroyImmediately();
        }
    }

    private IEnumerator StartWind()
    {
        int __direction = Random.Range(0, 2) == 1 ? 1 : -1;

        WindDirection = 1.2f * __direction;

        ParticleSystem.EmissionModule __emission = snowParticleSystem.emission;
        ParticleSystem.VelocityOverLifetimeModule __velocity = snowParticleSystem.velocityOverLifetime;

        __emission.rateOverTime = 100;
        __velocity.xMultiplier = 10 * __direction;

        yield return new WaitForSeconds(2f);

        __emission.rateOverTime = 25;

        while (WindDirection != 0f)
        {
            __velocity.xMultiplier = Mathf.MoveTowards(__velocity.xMultiplier, -0.15f, 2 * Time.deltaTime);
            WindDirection = Mathf.MoveTowards(WindDirection, 0f, Time.deltaTime);

            yield return null;
        }

        __velocity.xMultiplier = -0.15f;
    }

    private void LoadNextStage()
    {
        if (_stageIndex < _stages.Count - 1) _stageIndex++;

        CurrentStage = _stages[_stageIndex];
    }

    private void Agent_onGiftCaught(Gift p_gift, Vector2 p_position)
    {
        if(!Player.SpecialActive) streak++;

        caughtGifts++;
        bestStreak = streak > bestStreak ? streak : bestStreak;

        AudioManager.PlaySFX(SFXOccurrence.GIFT_CAUGHT, 0, 1);

        onScoreGift?.Invoke(p_gift, p_position);
    }

    private void Gift_onGiftDestroyed(Gift p_gift)
    {
        _onScreenGifts.Remove(p_gift);

        if(p_gift.destroy && !Player.SpecialActive)
        {
            AudioManager.PlaySFX(SFXOccurrence.GIFT_DESTROYED, 0, 0.6f - (p_gift.id * 0.1f));

            if(GameCEO.State == GameState.PLAY)
            {
                streak = 0;
                lostGifts++;

                onGiftLost?.Invoke();

                if (lostGifts == 10)
                {
                    onGameOver?.Invoke();
                }
            }
        }
    }

    private void Player_onSpecialActive(bool p_active)
    {
        AudioManager.PlaySFX(SFXOccurrence.STAR_CATCH, 0);
        AudioManager.PlaySFX(SFXOccurrence.STAR_CATCH, 1);

        _spawning = p_active;
        _spawnQuantity = p_active ? 100 : 0;
        specialGraphics.SetActive(p_active);

        if (!p_active)
        {
            foreach (Gift __gift in _onScreenGifts)
            {
                __gift.caught = true;

                onScoreGift?.Invoke(__gift, __gift.transform.localPosition);
            }
        }
    }
}
