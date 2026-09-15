using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotLever : MonoBehaviour
{
    [SerializeField] private GameObject normalLever;
    [SerializeField] private GameObject pulledLever;
    [SerializeField] private SlotMachineController slotMachine;
    [SerializeField] private BetManager betManager;
    [SerializeField] private BalanceManager balanceManager;

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

            if (!betManager.ValidateBet())
            return;

            if (!balanceManager.Spend(betManager.GetBet()))
            return;

            StartCoroutine(Pull());
        }

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

    public void EnableLever()
    {
        canPull = true;
        leverButton.interactable = true;
    }
}