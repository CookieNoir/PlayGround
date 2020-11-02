using UnityEngine;
using UnityEngine.UI;

public class HydraulicHammer : Accessory
{
    public GameObject hitPoints;
    [Header("Объекты для иконки")]
    public Image icon;
    public Sprite imageTurnedOff;
    public Sprite imageTurnedOn;

    protected override void Awake()
    {
        rigidbodyHandler.centerOfMass = centerOfMass;
        rigidbodyHandler.mass = mass;

        fixedDistance = new Vector3(0.0005f, 0, 0);
        fixedRotation = Quaternion.Euler(0, -90, 0);
        setFixedDistance = SetFixedDistance();

        ChangeState(false);
    }

    public override void FirstAction()
    {
        ChangeState(true);
    }

    public override void SecondAction()
    {
        ChangeState(false);
    }

    public override void Equip(Transform parent, Example ex)
    {
        transform.parent = parent;
        example = ex;
        LeaveHandler(example);

        icon.gameObject.SetActive(true);

        StopCoroutine(setFixedDistance);
        setFixedDistance = SetFixedDistance();
        StartCoroutine(setFixedDistance);
    }

    public override void Unequip()
    {
        ReturnToHandler(example);

        icon.gameObject.SetActive(false);
        ChangeState(false);

        example = null;
        equipped = false;
    }

    private void ChangeState(bool state)
    {
        hitPoints.SetActive(state);
        if (state) icon.sprite = imageTurnedOn;
        else icon.sprite = imageTurnedOff;
    }
}
