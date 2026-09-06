using UnityEngine;
using System.Collections;

public class SlotReel : MonoBehaviour
{
    // The 3 symbols inside this reel.
    [SerializeField] private RectTransform[] symbols;

    // How long this reel spins.
    [SerializeField] private float spinDuration = 1.5f;

    // Speed of the reel movement.
    [SerializeField] private float spinSpeed = 500f;

    // True = top to bottom, False = bottom to top.
    [SerializeField] private bool spinDown = true;

    // Prevents multiple spins at the same time.
    private bool isSpinning = false;

    // Original positions of the symbols.
    private Vector2[] originalPositions;

    // Distance between two neighbouring symbols.
    private float symbolSpacing;

    // Total distance covered by one complete symbol cycle.
    private float loopDistance;

    // Allows other scripts to check the reel state.
    public bool IsSpinning => isSpinning;


    private void Awake()
    {
        // Store the original position of every symbol.
        originalPositions = new Vector2[symbols.Length];

        for (int i = 0; i < symbols.Length; i++)
        {
            originalPositions[i] = symbols[i].anchoredPosition;
        }

        // Calculate the average spacing between the symbols.
        float spacing1 = Mathf.Abs(
            originalPositions[0].y - originalPositions[1].y
        );

        float spacing2 = Mathf.Abs(
            originalPositions[1].y - originalPositions[2].y
        );

        symbolSpacing = (spacing1 + spacing2) / 2f;

        // One complete cycle contains all 3 symbol spaces.
        loopDistance = symbolSpacing * symbols.Length;
    }


    // Starts the reel spin.
    public void Spin()
    {
        if (isSpinning)
            return;

        StartCoroutine(SpinReel());
    }


    // Handles the reel movement.
    private IEnumerator SpinReel()
    {
        isSpinning = true;

        float timer = 0f;

        // Find the highest and lowest original symbol positions.
        float highestY = originalPositions[0].y;
        float lowestY = originalPositions[0].y;

        for (int i = 1; i < originalPositions.Length; i++)
        {
            if (originalPositions[i].y > highestY)
                highestY = originalPositions[i].y;

            if (originalPositions[i].y < lowestY)
                lowestY = originalPositions[i].y;
        }

        // Keep the complete symbol inside the reel area.
        float symbolHalfHeight = symbols[0].rect.height * symbols[0].localScale.y / 2f;

        float topLimit = highestY + symbolSpacing / 2f;
        float bottomLimit = lowestY - symbolSpacing / 2f;

        while (timer < spinDuration)
        {
            // Calculate movement for this frame.
            float movement = spinSpeed * Time.deltaTime;

            foreach (RectTransform symbol in symbols)
            {
                if (spinDown)
                {
                    // Move the symbol downward.
                    symbol.anchoredPosition += Vector2.down * movement;

                    // Recycle the symbol before it moves too far outside.
                    if (symbol.anchoredPosition.y < bottomLimit)
                    {
                        symbol.anchoredPosition += Vector2.up * loopDistance;
                    }
                }
                else
                {
                    // Move the symbol upward.
                    symbol.anchoredPosition += Vector2.up * movement;

                    // Recycle the symbol before it moves too far outside.
                    if (symbol.anchoredPosition.y > topLimit)
                    {
                        symbol.anchoredPosition -= Vector2.up * loopDistance;
                    }
                }
            }

            timer += Time.deltaTime;

            yield return null;
        }

        // Restore the exact manually aligned positions.
        for (int i = 0; i < symbols.Length; i++)
        {
            symbols[i].anchoredPosition = originalPositions[i];
        }

        isSpinning = false;
    }
}