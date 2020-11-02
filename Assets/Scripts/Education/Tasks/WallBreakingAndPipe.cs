using UnityEngine;

public class WallBreakingAndPipe : WallBreakingPattern
{
    [Header("Объекты, связанные с текущей задачей")]
    public Transform grabTransform;
    public Grab grab;
    public Transform grabDefaultPoint;

    public Transform pipe;
    public Transform pipeDefaultPoint;
    public ReachablePoint pipeTracker;

    protected override void EnableTaskGameObjects()
    {
        EnableWallBreakingObjects();

        grabTransform.gameObject.SetActive(true);
        grabTransform.position = grabDefaultPoint.position;
        grabTransform.rotation = grabDefaultPoint.rotation;

        pipe.gameObject.SetActive(true);
        pipe.position = pipeDefaultPoint.position;
        pipe.rotation = pipeDefaultPoint.rotation;

        pipeTracker.gameObject.SetActive(true);
    }

    protected override void DisableTaskGameObjects()
    {
        DisableWallBreakingObjects();

        grabTransform.gameObject.SetActive(false);
        pipe.gameObject.SetActive(false);
        pipeTracker.gameObject.SetActive(false);
    }

    protected override int Task_0() // Уничтожить кусок стены
    {
        if (CountVisitedPoints() == reachablePointsAmount)
        {
            SetStage(1, Task_1, false);
            return 1;
        }
        else return 0;
    }

    private int Task_1() // Установить трубу в отверстие
    {
        if (pipeTracker.IsReached() && !grab.GetAttachedObject())
        {
            SetStage(2, EndTask, false);
            return 1;
        }
        else return 0;
    }
}
