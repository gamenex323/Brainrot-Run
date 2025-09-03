using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    public RaceManager raceManager;

    [Header("UI Root")]
    public GameObject countdownUI;   // Parent UI for countdown

    [Header("Countdown Objects")]
    public GameObject countdownObjMarks; // "On your marks..."
    public GameObject countdownObjSet;   // "Set"
    public GameObject countdownObjGo;    // "Go"

    [Header("Optional Texts")]
    public TextMeshProUGUI falseStartText0;
    public TextMeshProUGUI falseStartText1;

    public bool isRunning;
    public bool finished;

    public int state;
    public static int MARKS = 0;
    public static int SET = 1;
    public static int GO = 2;

    private void Start()
    {
        HideAllCountdownObjects();
        countdownUI.SetActive(false);
        falseStartText0.gameObject.SetActive(false);
        falseStartText1.gameObject.SetActive(false);
    }

    public void startCountdown()
    {
        isRunning = true;
        finished = false;

        countdownUI.SetActive(true);
        StartCoroutine(countdownSequence());
    }

    public void cancelCountdown()
    {
        StopAllCoroutines();
        countdownUI.SetActive(false);
        HideAllCountdownObjects();
        isRunning = false;
        finished = false;
    }

    private IEnumerator countdownSequence()
    {
        finished = false;

        // MARKS
        state = MARKS;
        countdownObjMarks.SetActive(true);
        // DOTween animation hook for Marks
        yield return new WaitForSeconds(1.5f);
        countdownObjMarks.SetActive(false);

        // Wait for tipsManager if showing (from original script)
        while (raceManager.gc.tipsManager.isShowing)
            yield return null;

        // SET
        state = SET;
        countdownObjSet.SetActive(true);
        // DOTween animation hook for Set
        yield return new WaitForSeconds(2f);
        countdownObjSet.SetActive(false);

        // GO
        state = GO;
        countdownObjGo.SetActive(true);
        // DOTween animation hook for Go
        yield return new WaitForSeconds(1f);
        countdownObjGo.SetActive(false);

        // End of Countdown
        countdownUI.SetActive(false);
        finished = true;
        isRunning = false;
    }

    private void HideAllCountdownObjects()
    {
        countdownObjMarks.SetActive(false);
        countdownObjSet.SetActive(false);
        countdownObjGo.SetActive(false);
    }

    public void showFalseStartText()
    {
        StartCoroutine(falseStartText());
    }

    private IEnumerator falseStartText()
    {
        falseStartText0.gameObject.SetActive(true);
        falseStartText1.gameObject.SetActive(true);

        Color c1 = new Color32(255, 65, 0, 0);
        Color c0 = new Color32(0, 0, 0, 0);

        falseStartText0.color = c0;
        falseStartText1.color = c1;

        while (c1.a < 1f)
        {
            c1.a += 15f * Time.deltaTime / Time.timeScale;
            c0.a += 15f * Time.deltaTime / Time.timeScale;
            falseStartText1.color = c1;
            falseStartText0.color = c0;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        while (c1.a > 0f)
        {
            c1.a -= 1.75f * Time.deltaTime / Time.timeScale;
            c0.a -= 1.75f * Time.deltaTime / Time.timeScale;
            falseStartText1.color = c1;
            falseStartText0.color = c0;
            yield return null;
        }
        c1.a = 0f;
        c0.a = 0f;
        falseStartText1.color = c1;
        falseStartText0.color = c0;
        falseStartText0.gameObject.SetActive(false);
        falseStartText1.gameObject.SetActive(false);
    }

    public void hideFalseStartText()
    {
        falseStartText0.gameObject.SetActive(false);
        falseStartText1.gameObject.SetActive(false);
    }
}
