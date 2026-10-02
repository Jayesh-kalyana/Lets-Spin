using UnityEngine;

public class UIAudio : MonoBehaviour
{
    // Audio source used for UI click sounds.
    [SerializeField] private AudioSource clickSound;

    // Plays a normal UI click sound immediately.
    public void PlayClick()
    {
        clickSound.PlayOneShot(clickSound.clip);
    }

    // Plays a click sound when the player types a bet amount.
    public void PlayInputClick(string value)
    {
        // Do not play when the input is empty.
        if (string.IsNullOrEmpty(value))
            return;

        clickSound.PlayOneShot(clickSound.clip);
    }
}