using UnityEngine;

public class AccessoryChange : Task
{
    [Header("Объекты, связанные с задачей")]
    public Transform accessory;
    public Transform accessoryDefaultPoint;
    public PointOfInterest joinPoint;
    public PointOfInterest separationPoint;
    public GameObject[] otherObjects;

    protected override void EnableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        accessory.gameObject.SetActive(true);
        joinPoint.gameObject.SetActive(true);
        accessory.position = accessoryDefaultPoint.position;
        accessory.rotation = accessoryDefaultPoint.rotation;
    }

    protected override void DisableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        mainGameObject.UnequipAccessory();
        accessory.gameObject.SetActive(false);
        joinPoint.gameObject.SetActive(false);
        separationPoint.gameObject.SetActive(false);
    }

    protected override int Task_0() // Достичь точки присоединения
    {
        if (joinPoint.IsReached())
        {
            joinPoint.gameObject.SetActive(false);
            SetStage(1, Task_1, false);
            return 1;
        }
        else return 0;
    }

    private int Task_1() // Приблизить стрелу для включения камеры
    {
        if (mainGameObject.GetSelected())
        {
            SetStage(2, Task_2, false);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private int Task_2() // Получить зеленый цвет
    {
        if (mainGameObject.IsAccessoryAvailableToJoin())
        {
            SetStage(3, Task_3, true);
            return 1;
        }
        else
        {
            if (mainGameObject.GetSelected())
            {
                return 0;
            }
            else
            {
                SetStage(1, Task_1, false);
                return 1;
            }
        }
    }

    private int Task_3() // Присоединить оборудование
    {
        if (mainGameObject.GetEquipped())
        {
            separationPoint.gameObject.SetActive(true);
            SetStage(4, Task_4, true);
            return 1;
        }
        else
        {
            if (mainGameObject.GetSelected())
            {
                if (mainGameObject.IsAccessoryAvailableToJoin())
                {
                    return 0;
                }
                else
                {
                    SetStage(2, Task_2, false);
                    return 1;
                }
            }
            else
            {
                SetStage(1, Task_1, false);
                return 1;
            }
        }
    }

    private int Task_4() // Сбросить оборудование в точке 2
    {
        if (separationPoint.IsReached() && !mainGameObject.GetEquipped())
        {
            SetStage(5, EndTask, false);
            return 1;
        }
        else
        {
            if (mainGameObject.GetSelected())
            {
                if (mainGameObject.IsAccessoryAvailableToJoin())
                {
                    if (mainGameObject.GetEquipped())
                    {
                        return 0;
                    }
                    else
                    {
                        SetStage(3, Task_3, true);
                        return 1;
                    }
                }
                else
                {
                    SetStage(2, Task_2, false);
                    return 1;
                }
            }
            else
            {
                SetStage(1, Task_1, false);
                return 1;
            }
        }
    }
}
