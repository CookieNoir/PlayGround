using UnityEngine;

public class WallBreakingSimplified : WallBreakingPattern
{
    protected override void EnableTaskGameObjects()
    {
        EnableWallBreakingObjects();
    }

    protected override void DisableTaskGameObjects()
    {
        DisableWallBreakingObjects();
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

    private int Task_1() // Уничтожить кусок стены
    {
        if (CountVisitedPoints() == reachablePointsAmount)
        {
            SetStage(2, EndTask, false);
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
