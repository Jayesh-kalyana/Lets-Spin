using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotLever : MonoBehaviour
{
    [SerializeField] private GameObject normalLever;
    [SerializeField] private GameObject pulledLever;
    [SerializeField] private SlotMachineController slotMachine;

    private bool canPull = true;
    private Button leverButton;

    private void Awake()
    {
        leverButton = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PullLever();
    }

    public void PullLever()
    {
        if (!canPull)
            return;

        StartCoroutine(Pull());
    }

    // Disable the lever while the reels are spinning.
    private IEnumerator Pull()
    {
        canPull = false;
        leverButton.interactable = false;

        normalLever.GetComponent<Image>().enabled = false;
        pulledLever.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        pulledLever.SetActive(false);
        normalLever.GetComponent<Image>().enabled = true;

        slotMachine.Spin();
    }

    // Enables the lever for the next round.
    public void EnableLever()
    {
        canPull = true;
        leverButton.interactable = true;
    }
}