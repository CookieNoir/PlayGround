using UnityEngine;

public class MovementAlongFigure : Task
{
    [Header("Объекты, связанные с задачей")]
    public RoinStatesKit specialState;
    public ReachablePointNoColliderCheck reachablePoint;
    public LineRenderer lineRenderer;
    public CircularPath circularPath;
    public SmoothQuestLine smoothQuestLine;
    [Min(3)] public int expectedSegmentsCount;
    [Range(1, 9)] public int pointsInSegment;
    private int positionCount;
    private int currentPosition;
    private int positionToFollow;
    private void Start()
    {
        smoothQuestLine.SetMaterial(lineRenderer.material);
    }

    protected override void SetSpecialState()
    {
        mainGameObject.specialState = specialState;
    }

    protected override void EnableTaskGameObjects()
    {
        reachablePoint.gameObject.SetActive(true);
        lineRenderer.gameObject.SetActive(true);

        lineRenderer.positionCount = circularPath.SetPositions(expectedSegmentsCount, pointsInSegment);
        lineRenderer.SetPositions(circularPath.mainPositions);
        positionCount = circularPath.GetCount();
        currentPosition = 0;
        positionToFollow = pointsInSegment;
        smoothQuestLine.SetStartValues(circularPath.GetPathLength() / lineRenderer.startWidth, 0f, 0.001f);
        UpdatePosition();
    }

    protected override void DisableTaskGameObjects()
    {
        reachablePoint.gameObject.SetActive(false);
        lineRenderer.gameObject.SetActive(false);
    }

    private bool UpdatePosition()
    {
        if (currentPosition < positionCount)
        {
            if (positionToFollow >= positionCount)
            {
                smoothQuestLine.SetValues(circularPath.GetProgressByIndex(currentPosition), 1f);
            }
            else
            {
                smoothQuestLine.SetValues(circularPath.GetProgressByIndex(currentPosition), circularPath.GetProgressByIndex(positionToFollow));
                positionToFollow++;
            }
            currentPosition++;
            reachablePoint.transform.position = circularPath.GetPositionByIndex(currentPosition);
            reachablePoint.ResetReached();
            return false;
        }
        else
        {
            smoothQuestLine.SetValues(1f, 1.001f);
            return true;
        }
    }

    protected override int Task_0() // Поместить все объекты в ящик
    {
        if (mainGameObject.GetAllowMove())
        {
            SetStage(2, TerminateTask, false); // Задание прервано. Робот должен оставаться неподвижным
            return 1;
        }
        else
        {
            if (reachablePoint.IsReached())
            {
                if (UpdatePosition())
                {
                    SetStage(1, EndTask, false); // Задание пройдено
                    return 1;
                }
                else return 0;
            }
            else return 0;
        }
    }
}
