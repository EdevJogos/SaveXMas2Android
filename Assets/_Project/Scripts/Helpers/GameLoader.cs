using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public TMPro.TextMeshProUGUI loadingLabel;

    private void Start()
    {
        StartCoroutine(RoutineLoadGame());
    }

    private IEnumerator RoutineLoadGame()
    {
        var sceneLoader = SceneManager.LoadSceneAsync(1);

        while (sceneLoader.progress <= 1)
        {
            loadingLabel.text = (100 * sceneLoader.progress) + "%";

            yield return null;
        }
    }
}
