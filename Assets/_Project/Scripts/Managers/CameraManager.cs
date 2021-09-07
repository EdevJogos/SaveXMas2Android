using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static Vector2 VerticalLimit;
    public static Vector2 HorizontalLimit;

    private static bool _ScreenShake;
    private static float _Strength;
    private static float _ScreenShakeTime;
    private static Vector3 _OriginalPosition;

    private Vector3 _randomVector;

    public static Camera MainCamera;
    public static Vector2 Center = new Vector2(-10f, 0f);

    public void Initiate()
    {
        MainCamera = Camera.main;

        Vector2 __minLimits = MainCamera.ViewportToWorldPoint(new Vector2(0.05f, 0));
        Vector2 __maxLimits = MainCamera.ViewportToWorldPoint(new Vector2(0.95f, 1.1f));

        VerticalLimit = new Vector2(__minLimits.y, __maxLimits.y);
        HorizontalLimit = new Vector2(__minLimits.x, __maxLimits.x);
    }

    private void Update()
    {
        if (_ScreenShake && MainCamera.enabled)
        {
            _randomVector = Random.insideUnitSphere;

            MainCamera.transform.localPosition = new Vector3(_randomVector.x * _Strength, _randomVector.y * _Strength, -10f) + _OriginalPosition;

            _ScreenShakeTime -= Time.deltaTime;

            if (_ScreenShakeTime <= 0)
            {
                StopShake();
            }
        }
    }

    public static void ShakeScreen(float p_duration, float p_strength)
    {
        _Strength = p_strength;
        _ScreenShakeTime += p_duration;
        _ScreenShakeTime = Mathf.Clamp(_ScreenShakeTime, 0f, p_duration);

        _ScreenShake = true;
    }

    public static void StopShake()
    {
        _ScreenShake = false;
        _ScreenShakeTime = 0f;

        MainCamera.transform.localPosition = _OriginalPosition;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        MainCamera = Camera.main;

        _OriginalPosition = MainCamera.transform.localPosition;
    }
}