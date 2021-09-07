using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    private static event System.Action onMute;

    public Sprite speakerSprite, muteSprite;
    public Image soundImage;

    private void Awake()
    {
        onMute += UpdateButtonSprite;
    }

    public void MuteAudio()
    {
        AudioManager.Mute = !AudioManager.Mute;
        AudioListener.volume = AudioManager.Mute ? 0f : 1f;

        onMute?.Invoke();
    }

    public void UpdateButtonSprite()
    {
        soundImage.sprite = AudioManager.Mute ? muteSprite : speakerSprite;
    }
}