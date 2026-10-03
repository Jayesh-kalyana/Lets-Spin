using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FullscreenController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMP_Text buttonText;

    public void OnPointerDown(PointerEventData eventData)
    {
        Screen.fullScreen = !Screen.fullScreen;

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        buttonText.text = Screen.fullScreen
            ? "EXIT\nFULL SCREEN"
            : "FULL\nSCREEN";
    }
}