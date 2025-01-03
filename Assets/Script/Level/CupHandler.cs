using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using Assets.Scripts.Audio;
using UnityEngine.UI;

public class CupHandler : MonoBehaviour
{
    [SerializeField] SlotPlaceHolder slotPlaceHolder;
    public List<Cup> cups = new List<Cup>();
    public Cup[] cupPrefab;
    public Transform cupParent, otherCupsHolder;
    public Transform[] frontCupsHolder;
    public TextMesh CupTextCount;

    [Button]
    public void SortCupsByColor()
    {
        // Ensure there are elements in the list to sort
        if (cups == null || cups.Count == 0) return;

        // Sort the cups based on their ItemColor
        cups.Sort((cup1, cup2) => cup1.itemColor.CompareTo(cup2.itemColor));

        PositionCups();
    }

    public void ShuffleCups()
    {
        if (cups == null || cups.Count == 0) return;

        // Shuffle the cups using the Fisher-Yates algorithm
        for (int i = cups.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (cups[i], cups[randomIndex]) = (cups[randomIndex], cups[i]); // Swap
        }
    }

    public ItemColor GetColorOfCurrentCupCakeOnConvare()
    {
        return cups[0].itemColor;
    }

    public void SortAndShuffleFirstTen()
    {
        // Ensure there are at least 10 elements to process
        int countToProcess = Mathf.Min(15, cups.Count);

        // Get the first 10 elements
        var firstTen = cups.GetRange(0, countToProcess);

        // Sort the first 10 elements by color
        firstTen.Sort((cup1, cup2) => cup1.itemColor.CompareTo(cup2.itemColor));

        // Shuffle the sorted elements
        ShuffleList(firstTen);

        // Replace the first 10 elements in the main list with the sorted/shuffled elements
        for (int i = 0; i < countToProcess; i++)
        {
            cups[i] = firstTen[i];
        }
    }

    private void ShuffleList(List<Cup> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]); // Swap
        }
    }
    public void GenerateCups(Vector3 startPosition, ItemColor cupColor, int numCups)
    {
        for (int i = 0; i < numCups; i++)
        {
            //print((int)cupColor);
            var cup = Instantiate(cupPrefab[((int)cupColor)], startPosition, Quaternion.identity);
            cup.transform.localEulerAngles = new Vector3 (0, -90, 0);
            //cup.SetCupColor(cupColor);
            cup.transform.SetParent(cupParent, true);
            cups.Add(cup);
        }
    }

    [Button]

    public void PositionCups(float offset = 0.5f)
    {
        for (int i = 0; i < cups.Count; i++)
        {
            if (i < frontCupsHolder.Length)
            {
                cups[i].transform.SetParent(frontCupsHolder[i], true);
                cups[i].transform.localPosition = Vector3.zero;
            }
            else
            {
                cups[i].transform.SetParent(otherCupsHolder, true);
                //cups[i].transform.localPosition = new Vector3((i - frontCupsHolder.Length) * offset, 0, 0);
                cups[i].transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        UpdateCountText();
    }

    [Button]
    public void ClearCups()
    {
        foreach (var cup in cups)
        {
            if (cup != null)
            {
                DestroyImmediate(cup.gameObject);
            }
        }
        cups.Clear();
    }

    float moveDuration = 0.1f;
    IEnumerator MoveAvailableCups(GameObject cup, Vector3 startPosition, Vector3 targetPosition)
    {
        
        //Vector3 startPosition = cups[i].transform.position;
        //Vector3 targetPosition = frontCupsHolder[i].position;
        float elapsedTime = 0;

        // Smoothly move the coffee cup to the next position in the queue
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            cup.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            yield return null;
        }

        // Ensure the coffee cup reaches the exact position
        cup.transform.position = targetPosition;
    }

    private bool isFunctionRunning = false;
    public void StartCheckingForBoxes()
    {
        // Ensure only one coroutine runs at a time
        if (!isFunctionRunning)
        {
            StartCoroutine(CheckIfBoxOfColorOffCoffee());
        }
    }

    public IEnumerator CheckIfBoxOfColorOffCoffee()
    {
        isFunctionRunning = true; // Mark the function as running

        // Keep checking until a win or loss condition is met
        while (true)
        {
            // Check if all cups are removed (win condition)
            if (cups.Count == 0)
            {
                print("game end");
                GameManager.instance.GameWin();
                AudioManager.instance.StopBgMusic();
                AudioManager.instance.PlayGameWin();
                break; // Exit the loop
            }

            // If the first cup matches the color
            if (slotPlaceHolder.CheckIfIHaveThatColor(cups[0]))
            {
                // Remove the first cup and update the UI
                cups.RemoveAt(0);
                UpdateCountText();

                // Move remaining cups in sequence
                int totalCups = Mathf.Min(20, cups.Count); // Limit to prevent errors
                for (int i = 0; i < totalCups; i++)
                {
                    if (cups[i] != null)
                    {
                        StartCoroutine(MoveAvailableCups(
                            cups[i].gameObject,
                            cups[i].transform.position,
                            frontCupsHolder[i].position
                        ));
                    }
                }

                // Wait for 1 second before continuing
                yield return new WaitForSeconds(0.4f);
            }
            else
            {
                // Check for a loss condition
                if (slotPlaceHolder.CheckForGameLoss())
                {
                    Debug.Log("Game Loss");
                    GameManager.instance.GameLose(true);
                    AudioManager.instance.StopBgMusic();
                    AudioManager.instance.playGameLoss();
                    break; // Exit the loop
                }

                // If no win or loss, break to avoid infinite loop
                break;
            }
        }

        isFunctionRunning = false; // Reset the flag when the function finishes
    }



    void UpdateCountText()
    {
        CupTextCount.text = cups.Count.ToString();
    }
}
