using UnityEngine;
using UnityEngine.UI;
public class TimerContent : MonoBehaviour
{
    public MaskableGraphic maskableGraphic;
    public Text timerText;
    public Color defaultColor;
    public Color dangerColor;
    public Color endColor;
    public Color terminateColor;

    public void UpdateTime(float seconds)
    {
        if (seconds < 0) seconds = 0;
        if (seconds < 5) maskableGraphic.color = Color.Lerp(dangerColor, defaultColor, OtherMath.SmoothStep(seconds * 0.2f));
        int secondsFloor = Mathf.FloorToInt(seconds);
        timerText.text = (secondsFloor / 60).ToString() + ':' + (secondsFloor % 60).ToString("D2");
    }

    public void SetDefaultColor()
    {
        maskableGraphic.color = defaultColor;
    }

    public void SetEndColor()
    {
        maskableGraphic.color = endColor;
    }

    public void SetTerminateColor()
    {
        maskableGraphic.color = terminateColor;
    }
}
