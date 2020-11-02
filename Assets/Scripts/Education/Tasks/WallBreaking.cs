using UnityEngine;

public class WallBreaking : WallBreakingPattern
{
    [Header("Объекты, связанные с текущей задачей")]
    public PointOfInterest pointOfInterest;

    // Группа полей, определяющих векторы, используемые при вычислении угла гидромолота
    [Header("Вспомогательные точки")]
    public Transform hammerPoint1;
    public Transform hammerPoint2;
    public Transform axisYtop;
    public Transform axisYbottom;
    public Transform axisXright;
    public Transform axisXleft;

    private Vector3 axisX;
    private Vector3 axisY;
    private const float COS_EPS = 0.15f;

    protected override void EnableTaskGameObjects()
    {
        EnableWallBreakingObjects();

        axisX = axisXright.position - axisXleft.position;
        axisY = axisYtop.position - axisYbottom.position;

        pointOfInterest.gameObject.SetActive(true);
    }

    protected override void DisableTaskGameObjects()
    {
        DisableWallBreakingObjects();
        pointOfInterest.gameObject.SetActive(false);
    }

    protected override int Task_0() // Присоединить оборудование
    {
        if (mainGameObject.GetEquipped())
        {
            SetStage(1, Task_1, false);
            return 1;
        }
        else return 0;
    }

    private bool IsCorrectAngle()
    {
        Vector3 hammerVector = hammerPoint2.position - hammerPoint1.position;
        return (Mathf.Abs(OtherMath.CosOfAngleBetweenTwoVectors(hammerVector, axisX)) < COS_EPS) && (Mathf.Abs(OtherMath.CosOfAngleBetweenTwoVectors(hammerVector, axisY)) < COS_EPS);
    }

    private int Task_1() // Установить молот под углом 90 - epsilon градусов
    {
        if (mainGameObject.GetEquipped())
        {
            if (pointOfInterest.IsReached() && IsCorrectAngle())
            {
                pointOfInterest.gameObject.SetActive(false);
                SetStage(2, Task_2, true);
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            SetStage(0, Task_0, true);
            return 1;
        }
    }

    private int Task_2() // Уничтожить кусок стены
    {
        if (CountVisitedPoints() == reachablePointsAmount)
        {
            SetStage(3, EndTask, false);
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
