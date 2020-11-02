using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TransformQuantity
{
    public Transform transform;
    public int quantity;
}

public class Gathering : Task
{
    [Header("Объекты, связанные с задачей")]
    public Transform grabTransform;
    public Grab grab;
    public RoinStatesKit specialState;
    [Space(15)]
    public Transform crateTransform;
    public DestroyingArea destroyingArea;
    [Space(15)]
    public TransformQuantity[] transformQuantities;
    [Space(15)]
    public SpiralPoints spiralPoints;
    public Vector2 axisYSpawnRange;
    private List<Transform> randomObjects;
    private int objectsCount;

    private void Start()
    {
        randomObjects = new List<Transform>();
    }

    protected override void SetSpecialState()
    {
        mainGameObject.specialState = specialState;
    }

    private int CountObjects()
    {
        int count = 0;
        for (int i = 0; i < transformQuantities.Length; ++i)
        {
            count += transformQuantities[i].quantity;
        }
        return count;
    }

    protected override void EnableTaskGameObjects()
    {
        grabTransform.gameObject.SetActive(true);
        mainGameObject.EquipAccessoryWithForce(grab);       

        crateTransform.gameObject.SetActive(true);

        objectsCount = CountObjects();
        destroyingArea.SetRequiredObjectsAmount(objectsCount);
        destroyingArea.SetGrab(grab);
        spiralPoints.CreateSequence(objectsCount);
        objectsCount = 0;
        for (int i = 0; i < transformQuantities.Length; ++i)
        {
            for (int j = 0; j < transformQuantities[i].quantity; ++j)
            {
                randomObjects.Add(Instantiate(transformQuantities[i].transform, 
                                              spiralPoints.GetWorldPositionOfPoint(objectsCount) + Vector3.up*Random.Range(axisYSpawnRange.x, axisYSpawnRange.y), 
                                              Random.rotation));
                objectsCount++;
            }
        }
    }

    protected override void DisableTaskGameObjects()
    {
        mainGameObject.UnequipAccessory();
        grabTransform.gameObject.SetActive(false);

        crateTransform.gameObject.SetActive(false);

        foreach (Transform randomObject in randomObjects)
        {
            if (randomObject) Destroy(randomObject.gameObject);
        }
        randomObjects.Clear();
    }

    protected override int Task_0() // Поместить все объекты в ящик
    {
        if (destroyingArea.IsEnoughObjects())
        {
            SetStage(1, EndTask, false); // Задание пройдено
            return 1;
        }
        else if (!mainGameObject.GetEquipped())
        {
            SetStage(2, TerminateTask, false); // Задание прервано. Навесное оборудование было отсоединено
            return 1;
        }
        else if (mainGameObject.GetAllowMove())
        {
            SetStage(3, TerminateTask, false); // Задание прервано. Робот должен оставаться неподвижным
            return 1;
        }
        else return 0;
    }
}
