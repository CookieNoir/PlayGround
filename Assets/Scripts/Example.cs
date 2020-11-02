using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
public class Example : RoinPartsAndStates
{
    private bool touchSurface;

    [Header("Камера")]
    public CameraController cameraController;
    [Header("Самопересечения")]
    public BoxMainObject boxIntersection;
    public GameObject defaultBox;

    [HideInInspector] public Rigidbody rb;

    [Header("Навесное оборудование")]
    public Transform join;
    public Camera joinCamera;
    public float cameraTurnOnDistance; // Дистанция, при которой включается камера для отслеживания соединения
    public float distanceToConnect; // Дистанция, при которой соединение возможно
    public MaskableGraphic joinCameraUI; // должна иметь дочерний объект Text
    public Text joinCameraText;

    public Color colorNotConnect;
    public Color colorConnect;

    [HideInInspector] public GeneralCenterOfMass generalCenterOfMass;
    private List<Accessory> accessories;
    private Accessory accessory;
    private bool selected = false;
    private bool equipped = false;
    private float distance;

    [Header("Перегруз")]
    public float defaultMass;
    public float maxOverload;
    private float maxOverloadInverse;
    private float overloadTimer = 0f;

    [Header("Система уведомлений")]
    public NotificationSystem notificationSystem;

    public void SetTouchSurface(bool value)
    {
        touchSurface = value;
    }

    public bool GetSelected()
    {
        return selected;
    }

    public bool GetEquipped()
    {
        return equipped;
    }

    public void ChangeAntiIntersectionBox(GameObject box)
    {
        boxIntersection.SetLast(box);
    }

    public void SetDefaultAntiIntersectionBox()
    {
        boxIntersection.SetLast(defaultBox);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        WriteDefaultState();

        generalCenterOfMass = GetComponent<GeneralCenterOfMass>();
        maxOverloadInverse = 1f / maxOverload;

        accessory = null;
        accessories = new List<Accessory>();
        boxIntersection.SetMatrixType(accessory);

        notificationSystem.ChangeScale(joinCamera.isActiveAndEnabled);
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (!contact.thisCollider.CompareTag("Caterpillar") &&
                !contact.thisCollider.CompareTag("Leg") &&
                !contact.thisCollider.CompareTag("Accessory"))
            {
                notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                break;
            }
        }
    }

    public bool IsAccessoryAvailableToJoin()
    {
        return joinCameraUI.color == colorConnect;
    }

    public void UnequipAccessory()
    {
        if (equipped)
        {
            accessory.Unequip();
            boxIntersection.SetMatrixType(accessory);
            equipped = false;
        }
    }

    public void EquipAccessoryWithForce(in Accessory accessory)
    {
        this.accessory = accessory;
        EquipAccessory();
        this.accessory.SetFixedDistanceAndRotation();
    }

    private void EquipAccessory()
    {
        accessory.Equip(join, this);
        boxIntersection.SetMatrixType(accessory);
        equipped = true;
        notificationSystem.Notify(NotificationSystem.NotificationTypes.message, "Оборудование сменено на " + accessory.name);
    }

    // Для событий, не связанных с физикой напрямую
    private void Update()
    {
        RecalculateSpeed();
        if (!equipped)
            if (!selected)
            {
                if (accessories.Count > 0)
                {
                    foreach (Accessory ac in accessories)
                    {
                        float dist = Vector3.Magnitude(join.position - ac.transform.position);
                        if (dist < cameraTurnOnDistance)
                        {
                            accessory = ac;
                            joinCamera.enabled = true;
                            notificationSystem.ChangeScale(true);
                            joinCameraUI.gameObject.SetActive(true);
                            joinCameraText.text = dist.ToString("F8") + " m";
                            selected = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (joinCamera.enabled == true)
                    {
                        TurnOffJoinCameraAndUI();
                    }
                }
            }
            else
            {
                distance = Vector3.Magnitude(join.position - accessory.transform.position);
                if (distance >= cameraTurnOnDistance)
                {
                    accessory = null;
                    TurnOffJoinCameraAndUI();
                    selected = false;
                }
                else
                {
                    joinCameraText.text = distance.ToString("F8") + " m";
                    if (distance < distanceToConnect)
                    {
                        joinCameraUI.color = colorConnect;
                    }
                    else
                    {
                        joinCameraUI.color = colorNotConnect;
                    }
                }
            }
        else
        {
            if (joinCamera.enabled == true)
            {
                TurnOffJoinCameraAndUI();
            }
        }

        //Присоединение/отсоединение навесного оборудования
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            if (!equipped)
            {
                if (selected)
                {
                    if (distance < distanceToConnect)
                    {
                        EquipAccessory();
                    }
                    else
                    {
                        notificationSystem.Notify(NotificationSystem.NotificationTypes.warning, "Для присоединения сократите дистанцию");
                    }
                }
            }
            else
            {
                UnequipAccessory();
                notificationSystem.Notify(NotificationSystem.NotificationTypes.message, "Оборудование снято");
            }
        }

        if (equipped)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                accessory.FirstAction();
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                accessory.SecondAction();
            }
        }
    }

    private void TurnOffJoinCameraAndUI()
    {
        joinCamera.enabled = false;
        notificationSystem.ChangeScale(false);
        joinCameraUI.gameObject.SetActive(false);
    }

    private void RecalculateSpeed()
    {
        float f = (maxOverload + defaultMass - rb.mass) * maxOverloadInverse;
        if (f <= 0)
        {
            f = 0;
            if (overloadTimer <= 0)
            {
                notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Перегруз");
                overloadTimer = 5f;
            }
            else
            {
                overloadTimer -= Time.deltaTime;
            }
        }
        else if (f > 1) f = 1;
        f = f * f * (3 - 2 * f);
        currentSpeed = defaultSpeed * f;
        currentSpeedRotation = defaultSpeedRotation * f;
    }

    // Обработка нажатия клавиш
    void FixedUpdate()
    {
        //Движение
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (allowMove && touchSurface)
                transform.position += body.transform.forward * currentSpeed;
        }

        //Движение
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (allowMove && touchSurface)
                transform.position -= body.transform.forward * currentSpeed;
        }

        //Поворот
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (allowMove && touchSurface)
            {
                transform.RotateAround(body.transform.position, body.transform.up, -currentSpeedRotation * Mathf.Rad2Deg);
            }
        }

        //Поворот
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (allowMove && touchSurface)
            {
                transform.RotateAround(body.transform.position, body.transform.up, currentSpeedRotation * Mathf.Rad2Deg);
            }
        }

        //Поворот основания
        if (Input.GetKey(KeyCode.R))
        {
            if (allowRotation)
            {
                //перед вращением элементов робота сохраняем
                //значение этих элементов
                Vector3 tmpv_1 = arm0.transform.position;
                Quaternion tmpq_1 = arm0.transform.rotation;
                float tmpf_1 = alpha0;

                //вращаем
                RAM.RotateBase_Yaw(1, ref arm0, ref alpha0, currentSpeedRotation);

                //если после вращения произошло самопересечение, то вовращаем все назад
                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm0.transform.position = tmpv_1;
                    arm0.transform.rotation = tmpq_1;
                    alpha0 = tmpf_1;
                }
            }
        }

        //Поворот основания
        if (Input.GetKey(KeyCode.T))
        {
            if (allowRotation)
            {

                Vector3 tmpv_1 = arm0.transform.position;
                Quaternion tmpq_1 = arm0.transform.rotation;
                float tmpf_1 = alpha0;

                RAM.RotateBase_Yaw(-1, ref arm0, ref alpha0, currentSpeedRotation);
                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm0.transform.position = tmpv_1;
                    arm0.transform.rotation = tmpq_1;
                    alpha0 = tmpf_1;

                }
            }
        }

        //Поворот первой стрелы с неподвижной базой
        if (Input.GetKey(KeyCode.U))
        {
            if (allowRotation)
            {

                Vector3 tmpV_arm1 = arm1.transform.position;
                Quaternion tmpQ_arm1 = arm1.transform.rotation;
                Vector3 tmpV_cylinder1 = cylinder1.transform.position;
                Quaternion tmpQ_cylinder1 = cylinder1.transform.rotation;
                Vector3 tmpV_piston1 = piston1.transform.position;
                Quaternion tmpQ_piston1 = piston1.transform.rotation;
                float tmp_a = alpha1;
                float tmp_b = beta1;
                float tmp_g = gamma1;

                if (alpha1 <= hit1Max - currentSpeedRotation)
                    RAM.RotateArmEasy(1, ref arm1, ref cylinder1, ref piston1,
                        ref alpha1, ref beta1, ref gamma1,
                        l1, d1, currentSpeedRotation);
                else if (alpha1 < hit1Max)
                    RAM.RotateArmEasy((hit1Max - alpha1) / currentSpeedRotation, ref arm1, ref cylinder1, ref piston1,
                        ref alpha1, ref beta1, ref gamma1,
                        l1, d1, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm1.transform.position = tmpV_arm1;
                    arm1.transform.rotation = tmpQ_arm1;
                    cylinder1.transform.position = tmpV_cylinder1;
                    cylinder1.transform.rotation = tmpQ_cylinder1;
                    piston1.transform.position = tmpV_piston1;
                    piston1.transform.rotation = tmpQ_piston1;
                    alpha1 = tmp_a;
                    beta1 = tmp_b;
                    gamma1 = tmp_g;

                }
            }
        }

        //Поворот первой стрелы с неподвижной базой
        if (Input.GetKey(KeyCode.Y))
        {
            if (allowRotation)
            {
                Vector3 tmpV_arm1 = arm1.transform.position;
                Quaternion tmpQ_arm1 = arm1.transform.rotation;
                Vector3 tmpV_cylinder1 = cylinder1.transform.position;
                Quaternion tmpQ_cylinder1 = cylinder1.transform.rotation;
                Vector3 tmpV_piston1 = piston1.transform.position;
                Quaternion tmpQ_piston1 = piston1.transform.rotation;
                float tmp_a = alpha1;
                float tmp_b = beta1;
                float tmp_g = gamma1;

                if (alpha1 >= hit1Min + currentSpeedRotation)
                    RAM.RotateArmEasy(-1, ref arm1, ref cylinder1, ref piston1,
                        ref alpha1, ref beta1, ref gamma1,
                        l1, d1, currentSpeedRotation);
                else if (alpha1 > hit1Min)
                    RAM.RotateArmEasy((hit1Min - alpha1) / currentSpeedRotation, ref arm1, ref cylinder1, ref piston1,
                        ref alpha1, ref beta1, ref gamma1,
                        l1, d1, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm1.transform.position = tmpV_arm1;
                    arm1.transform.rotation = tmpQ_arm1;
                    cylinder1.transform.position = tmpV_cylinder1;
                    cylinder1.transform.rotation = tmpQ_cylinder1;
                    piston1.transform.position = tmpV_piston1;
                    piston1.transform.rotation = tmpQ_piston1;
                    alpha1 = tmp_a;
                    beta1 = tmp_b;
                    gamma1 = tmp_g;

                }
            }
        }

        //Поворот второй стрелы с неподвижной первой
        if (Input.GetKey(KeyCode.O))
        {
            if (allowRotation)
            {
                Vector3 tmpV_arm = arm2.transform.position;
                Quaternion tmpQ_arm = arm2.transform.rotation;
                Vector3 tmpV_cylinder = cylinder2.transform.position;
                Quaternion tmpQ_cylinder = cylinder2.transform.rotation;
                Vector3 tmpV_piston = piston2.transform.position;
                Quaternion tmpQ_piston = piston2.transform.rotation;
                float tmp_a = alpha2;
                float tmp_b = beta2;
                float tmp_g = gamma2;

                if (alpha2 <= hit2Max - currentSpeedRotation)
                    RAM.RotateArmEasy(1, ref arm2, ref cylinder2, ref piston2,
                        ref alpha2, ref beta2, ref gamma2,
                        l2, d2, currentSpeedRotation);
                else if (alpha2 < hit2Max)
                    RAM.RotateArmEasy((hit2Max - alpha2) / currentSpeedRotation, ref arm2, ref cylinder2, ref piston2,
                        ref alpha2, ref beta2, ref gamma2,
                        l2, d2, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm2.transform.position = tmpV_arm;
                    arm2.transform.rotation = tmpQ_arm;
                    cylinder2.transform.position = tmpV_cylinder;
                    cylinder2.transform.rotation = tmpQ_cylinder;
                    piston2.transform.position = tmpV_piston;
                    piston2.transform.rotation = tmpQ_piston;
                    alpha2 = tmp_a;
                    beta2 = tmp_b;
                    gamma2 = tmp_g;

                }
            }
        }

        //Поворот второй стрелы с неподвижной первой
        if (Input.GetKey(KeyCode.I))
        {
            if (allowRotation)
            {
                Vector3 tmpV_arm = arm2.transform.position;
                Quaternion tmpQ_arm = arm2.transform.rotation;
                Vector3 tmpV_cylinder = cylinder2.transform.position;
                Quaternion tmpQ_cylinder = cylinder2.transform.rotation;
                Vector3 tmpV_piston = piston2.transform.position;
                Quaternion tmpQ_piston = piston2.transform.rotation;
                float tmp_a = alpha2;
                float tmp_b = beta2;
                float tmp_g = gamma2;

                if (alpha2 >= hit2Min + currentSpeedRotation)
                    RAM.RotateArmEasy(-1, ref arm2, ref cylinder2, ref piston2,
                       ref alpha2, ref beta2, ref gamma2,
                       l2, d2, currentSpeedRotation);
                else if (alpha2 > hit2Min)
                    RAM.RotateArmEasy((hit2Min - alpha2) / currentSpeedRotation, ref arm2, ref cylinder2, ref piston2,
                       ref alpha2, ref beta2, ref gamma2,
                       l2, d2, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm2.transform.position = tmpV_arm;
                    arm2.transform.rotation = tmpQ_arm;
                    cylinder2.transform.position = tmpV_cylinder;
                    cylinder2.transform.rotation = tmpQ_cylinder;
                    piston2.transform.position = tmpV_piston;
                    piston2.transform.rotation = tmpQ_piston;
                    alpha2 = tmp_a;
                    beta2 = tmp_b;
                    gamma2 = tmp_g;

                }
            }
        }

        //Излом
        if (Input.GetKey(KeyCode.F))
        {
            if (allowRotation)
            {
                Vector3 tmpV_arm = arm3.transform.position;
                Quaternion tmpQ_arm = arm3.transform.rotation;
                Vector3 tmpV_cylinder = cylinder3.transform.position;
                Quaternion tmpQ_cylinder = cylinder3.transform.rotation;
                Vector3 tmpV_piston = piston3.transform.position;
                Quaternion tmpQ_piston = piston3.transform.rotation;
                Vector3 tmpV_fixed = fixed3.transform.position;
                Quaternion tmpQ_fixed = fixed3.transform.rotation;
                float tmp_a = alpha3;
                float tmp_b = beta3;
                float tmp_g = gamma3;

                if (alpha3 >= hitFractureMin + currentSpeedRotation)
                    RAM.Fracture(1, ref arm3, ref arm4, ref cylinder3, ref piston3, ref fixed3,
                        ref alpha3, ref beta3, ref gamma3,
                        l3, d3, currentSpeedRotation);
                else if (alpha3 > hitFractureMin)
                    RAM.Fracture((alpha3 - hitFractureMin) / currentSpeedRotation, ref arm3, ref arm4, ref cylinder3, ref piston3, ref fixed3,
                       ref alpha3, ref beta3, ref gamma3,
                       l3, d3, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm3.transform.position = tmpV_arm;
                    arm3.transform.rotation = tmpQ_arm;
                    cylinder3.transform.position = tmpV_cylinder;
                    cylinder3.transform.rotation = tmpQ_cylinder;
                    piston3.transform.position = tmpV_piston;
                    piston3.transform.rotation = tmpQ_piston;
                    fixed3.transform.position = tmpV_fixed;
                    fixed3.transform.rotation = tmpQ_fixed;
                    alpha3 = tmp_a;
                    beta3 = tmp_b;
                    gamma3 = tmp_g;

                }
            }
        }

        //Излом
        if (Input.GetKey(KeyCode.G))
        {
            if (allowRotation)
            {
                Vector3 tmpV_arm = arm3.transform.position;
                Quaternion tmpQ_arm = arm3.transform.rotation;
                Vector3 tmpV_cylinder = cylinder3.transform.position;
                Quaternion tmpQ_cylinder = cylinder3.transform.rotation;
                Vector3 tmpV_piston = piston3.transform.position;
                Quaternion tmpQ_piston = piston3.transform.rotation;
                Vector3 tmpV_fixed = fixed3.transform.position;
                Quaternion tmpQ_fixed = fixed3.transform.rotation;
                float tmp_a = alpha3;
                float tmp_b = beta3;
                float tmp_g = gamma3;

                if (alpha3 <= hitFractureMax - currentSpeedRotation)
                    RAM.Fracture(-1, ref arm3, ref arm4, ref cylinder3, ref piston3, ref fixed3,
                       ref alpha3, ref beta3, ref gamma3,
                       l3, d3, currentSpeedRotation);
                else if (alpha3 < hitFractureMax)
                    RAM.Fracture((alpha3 - hitFractureMax) / currentSpeedRotation, ref arm3, ref arm4, ref cylinder3, ref piston3, ref fixed3,
                       ref alpha3, ref beta3, ref gamma3,
                       l3, d3, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm3.transform.position = tmpV_arm;
                    arm3.transform.rotation = tmpQ_arm;
                    cylinder3.transform.position = tmpV_cylinder;
                    cylinder3.transform.rotation = tmpQ_cylinder;
                    piston3.transform.position = tmpV_piston;
                    piston3.transform.rotation = tmpQ_piston;
                    fixed3.transform.position = tmpV_fixed;
                    fixed3.transform.rotation = tmpQ_fixed;
                    alpha3 = tmp_a;
                    beta3 = tmp_b;
                    gamma3 = tmp_g;

                }
            }
        }

        //Поворот четвертой стрелы с неподвижным изломом
        if (Input.GetKey(KeyCode.J))
        {
            if (allowRotation)
            {
                Vector3 tmpV_arm = arm5.transform.position;
                Quaternion tmpQ_arm = arm5.transform.rotation;
                Vector3 tmpV_cylinder = cylinder5.transform.position;
                Quaternion tmpQ_cylinder = cylinder5.transform.rotation;
                Vector3 tmpV_fixed = fixed5.transform.position;
                Quaternion tmpQ_fixed = fixed5.transform.rotation;
                Vector3 tmpV_clutch = clutch5.transform.position;
                Quaternion tmpQ_clutch = clutch5.transform.rotation;
                float tmp_a = alpha5;
                float tmp_b = beta5;
                float tmp_t = theta5;
                float tmp_phi = phi5;
                float tmp_psi51 = psi51;
                float tmp_psi52 = psi52;

                if (alpha5 >= hit5Min + currentSpeedRotation)
                    RAM.RotateArmHard_Pitch(1, ref arm5, ref cylinder5, ref piston5, ref clutch5, ref fixed5,
                        ref alpha5, ref beta5, ref gamma5, ref theta5, ref phi5, ref psi51, ref psi52,
                        OA5, OB5, OC5, BD5, CD5, currentSpeedRotation);
                else if (alpha5 > hit5Min)
                    RAM.RotateArmHard_Pitch((alpha5 - hit5Min) / currentSpeedRotation, ref arm5, ref cylinder5, ref piston5, ref clutch5, ref fixed5,
                        ref alpha5, ref beta5, ref gamma5, ref theta5, ref phi5, ref psi51, ref psi52,
                        OA5, OB5, OC5, BD5, CD5, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm5.transform.position = tmpV_arm;
                    arm5.transform.rotation = tmpQ_arm;
                    cylinder5.transform.position = tmpV_cylinder;
                    cylinder5.transform.rotation = tmpQ_cylinder;
                    fixed5.transform.position = tmpV_fixed;
                    fixed5.transform.rotation = tmpQ_fixed;
                    clutch5.transform.position = tmpV_clutch;
                    clutch5.transform.rotation = tmpQ_clutch;
                    alpha5 = tmp_a;
                    beta5 = tmp_b;
                    theta5 = tmp_t;
                    phi5 = tmp_phi;
                    psi51 = tmp_psi51;
                    psi52 = tmp_psi52;

                }
            }
        }

        //Поворот четвертой стрелы с неподвижным изломом
        if (Input.GetKey(KeyCode.H))
        {
            if (allowRotation)
            {
                Vector3 tmpV_arm = arm5.transform.position;
                Quaternion tmpQ_arm = arm5.transform.rotation;
                Vector3 tmpV_cylinder = cylinder5.transform.position;
                Quaternion tmpQ_cylinder = cylinder5.transform.rotation;
                Vector3 tmpV_fixed = fixed5.transform.position;
                Quaternion tmpQ_fixed = fixed5.transform.rotation;
                Vector3 tmpV_clutch = clutch5.transform.position;
                Quaternion tmpQ_clutch = clutch5.transform.rotation;
                float tmp_a = alpha5;
                float tmp_b = beta5;
                float tmp_t = theta5;
                float tmp_phi = phi5;
                float tmp_psi51 = psi51;
                float tmp_psi52 = psi52;

                if (alpha5 <= hit5Max - currentSpeedRotation)
                    RAM.RotateArmHard_Pitch(-1, ref arm5, ref cylinder5, ref piston5, ref clutch5, ref fixed5,
                       ref alpha5, ref beta5, ref gamma5, ref theta5, ref phi5, ref psi51, ref psi52,
                       OA5, OB5, OC5, BD5, CD5, currentSpeedRotation);
                else if (alpha5 < hit5Max)
                    RAM.RotateArmHard_Pitch((alpha5 - hit5Max) / currentSpeedRotation, ref arm5, ref cylinder5, ref piston5, ref clutch5, ref fixed5,
                       ref alpha5, ref beta5, ref gamma5, ref theta5, ref phi5, ref psi51, ref psi52,
                       OA5, OB5, OC5, BD5, CD5, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm5.transform.position = tmpV_arm;
                    arm5.transform.rotation = tmpQ_arm;
                    cylinder5.transform.position = tmpV_cylinder;
                    cylinder5.transform.rotation = tmpQ_cylinder;
                    fixed5.transform.position = tmpV_fixed;
                    fixed5.transform.rotation = tmpQ_fixed;
                    clutch5.transform.position = tmpV_clutch;
                    clutch5.transform.rotation = tmpQ_clutch;

                    alpha5 = tmp_a;
                    beta5 = tmp_b;
                    theta5 = tmp_t;
                    phi5 = tmp_phi;
                    psi51 = tmp_psi51;
                    psi52 = tmp_psi52;

                }
            }
        }

        //Удлинение пятой стрелы
        if (Input.GetKey(KeyCode.L))
        {
            if (allowRotation)
            {
                Vector3 tmpV_arm = arm6.transform.position;
                Quaternion tmpQ_arm = arm6.transform.rotation;
                float tmp_l = l6;

                if (l6 <= hit6Max - currentSpeedElongation)
                    RAM.Elongation(1, ref arm6, ref l6, currentSpeedElongation);
                else if (l6 < hit6Max)
                    RAM.Elongation((hit6Max - l6) / currentSpeedElongation, ref arm6, ref l6, currentSpeedElongation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm6.transform.position = tmpV_arm;
                    arm6.transform.rotation = tmpQ_arm;
                    l6 = tmp_l;
                }
            }
        }

        //Укорачивание пятой стрелы
        if (Input.GetKey(KeyCode.K))
        {
            if (allowRotation)
            {
                Vector3 tmpV_arm = arm6.transform.position;
                Quaternion tmpQ_arm = arm6.transform.rotation;
                float tmp_l = l6;

                if (l6 >= hit6Min + currentSpeedElongation)
                    RAM.Elongation(-1, ref arm6, ref l6, currentSpeedElongation);
                else if (l6 > hit6Min)
                    RAM.Elongation((hit6Min - l6) / currentSpeedElongation, ref arm6, ref l6, currentSpeedElongation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    arm6.transform.position = tmpV_arm;
                    arm6.transform.rotation = tmpQ_arm;
                    l6 = tmp_l;
                }
            }
        }

        //Тангаж НПУ
        if (Input.GetKey(KeyCode.X))
        {
            if (allowRotation)
            {
                Vector3 tmpV_cylinder = cylinder7.transform.position;
                Quaternion tmpQ_cylinder = cylinder7.transform.rotation;
                Vector3 tmpV_piston = piston7.transform.position;
                Quaternion tmpQ_piston = piston7.transform.rotation;
                Vector3 tmpV_fixed = fixed7.transform.position;
                Quaternion tmpQ_fixed = fixed7.transform.rotation;
                Vector3 tmpV_clutch = clutch7.transform.position;
                Quaternion tmpQ_clutch = clutch7.transform.rotation;
                Vector3 tmpV_pitcher = pitcher.transform.position;
                Quaternion tmpQ_pitcher = pitcher.transform.rotation;
                float tmp_b = beta7;
                float tmp_g = gamma7;
                float tmp_t = theta7;
                float tmp_phi = phi7;
                float tmp_psi51 = psi71;
                float tmp_psi52 = psi72;
                float tmp_p = alphaPitch;

                if (alphaPitch >= hitPitchMin + currentSpeedRotation)
                    RAM.RotateArmHard_Pitch(1, ref pitcher, ref cylinder7, ref piston7, ref clutch7, ref fixed7,
                        ref alphaPitch, ref beta7, ref gamma7, ref theta7, ref phi7, ref psi71, ref psi72,
                        OA7, OB7, OC7, BD7, CD7, currentSpeedRotation);
                else if (alphaPitch > hitPitchMin)
                    RAM.RotateArmHard_Pitch((alphaPitch - hitPitchMin) / currentSpeedRotation, ref pitcher, ref cylinder7, ref piston7, ref clutch7, ref fixed7,
                        ref alphaPitch, ref beta7, ref gamma7, ref theta7, ref phi7, ref psi71, ref psi72,
                        OA7, OB7, OC7, BD7, CD7, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    cylinder7.transform.position = tmpV_cylinder;
                    cylinder7.transform.rotation = tmpQ_cylinder;
                    piston7.transform.position = tmpV_piston;
                    piston7.transform.rotation = tmpQ_piston;
                    fixed7.transform.position = tmpV_fixed;
                    fixed7.transform.rotation = tmpQ_fixed;
                    clutch7.transform.position = tmpV_clutch;
                    clutch7.transform.rotation = tmpQ_clutch;
                    pitcher.transform.position = tmpV_pitcher;
                    pitcher.transform.rotation = tmpQ_pitcher;

                    beta5 = tmp_b;
                    gamma5 = tmp_g;
                    theta5 = tmp_t;
                    phi5 = tmp_phi;
                    psi51 = tmp_psi51;
                    psi52 = tmp_psi52;
                    alphaPitch = tmp_p;
                }
            }
        }

        //Тангаж НПУ
        if (Input.GetKey(KeyCode.Z))
        {
            if (allowRotation)
            {
                Vector3 tmpV_cylinder = cylinder7.transform.position;
                Quaternion tmpQ_cylinder = cylinder7.transform.rotation;
                Vector3 tmpV_piston = piston7.transform.position;
                Quaternion tmpQ_piston = piston7.transform.rotation;
                Vector3 tmpV_fixed = fixed7.transform.position;
                Quaternion tmpQ_fixed = fixed7.transform.rotation;
                Vector3 tmpV_clutch = clutch7.transform.position;
                Quaternion tmpQ_clutch = clutch7.transform.rotation;
                Vector3 tmpV_pitcher = pitcher.transform.position;
                Quaternion tmpQ_pitcher = pitcher.transform.rotation;
                float tmp_b = beta7;
                float tmp_g = gamma7;
                float tmp_t = theta7;
                float tmp_phi = phi7;
                float tmp_psi51 = psi71;
                float tmp_psi52 = psi72;
                float tmp_p = alphaPitch;

                if (alphaPitch <= hitPitchMax - currentSpeedRotation)
                    RAM.RotateArmHard_Pitch(-1, ref pitcher, ref cylinder7, ref piston7, ref clutch7, ref fixed7,
                        ref alphaPitch, ref beta7, ref gamma7, ref theta7, ref phi7, ref psi71, ref psi72,
                        OA7, OB7, OC7, BD7, CD7, currentSpeedRotation);
                else if (alphaPitch < hitPitchMax)
                    RAM.RotateArmHard_Pitch((alphaPitch - hitPitchMax) / currentSpeedRotation, ref pitcher, ref cylinder7, ref piston7, ref clutch7, ref fixed7,
                        ref alphaPitch, ref beta7, ref gamma7, ref theta7, ref phi7, ref psi71, ref psi72,
                        OA7, OB7, OC7, BD7, CD7, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    cylinder7.transform.position = tmpV_cylinder;
                    cylinder7.transform.rotation = tmpQ_cylinder;
                    piston7.transform.position = tmpV_piston;
                    piston7.transform.rotation = tmpQ_piston;
                    fixed7.transform.position = tmpV_fixed;
                    fixed7.transform.rotation = tmpQ_fixed;
                    clutch7.transform.position = tmpV_clutch;
                    clutch7.transform.rotation = tmpQ_clutch;
                    pitcher.transform.position = tmpV_pitcher;
                    pitcher.transform.rotation = tmpQ_pitcher;

                    beta5 = tmp_b;
                    gamma5 = tmp_g;
                    theta5 = tmp_t;
                    phi5 = tmp_phi;
                    psi51 = tmp_psi51;
                    psi52 = tmp_psi52;
                    alphaPitch = tmp_p;
                }
            }
        }

        //Крен НПУ
        if (Input.GetKey(KeyCode.V))
        {
            if (allowRotation)
            {
                Vector3 tmpV_roller = roller.transform.position;
                Quaternion tmpQ_roller = roller.transform.rotation;
                Vector3 tmpV_cylinderRoller = cylinderRoller.transform.position;
                Quaternion tmpQ_cylinderRoller = cylinderRoller.transform.rotation;
                Vector3 tmpV_pistonRoller = pistonRoller.transform.position;
                Quaternion tmpQ_pistonRoller = pistonRoller.transform.rotation;
                float tmp_alphaRoll = alphaRoll;
                float tmp_betaRoll = betaRoll;
                float tmp_gammaRoll = gammaRoll;

                if (alphaRoll >= hitRollMin + currentSpeedRotation)
                    RAM.RotateRoll(1, ref roller, ref cylinderRoller, ref pistonRoller,
                       ref alphaRoll, ref betaRoll, ref gammaRoll,
                       lRoll, dRoll, currentSpeedRotation);
                else if (alphaRoll > hitRollMin)
                    RAM.RotateRoll((alphaRoll - hitRollMin) / currentSpeedRotation, ref roller, ref cylinderRoller, ref pistonRoller,
                       ref alphaRoll, ref betaRoll, ref gammaRoll,
                       lRoll, dRoll, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    pitcher.transform.position = tmpV_roller;
                    pitcher.transform.rotation = tmpQ_roller;
                    cylinderRoller.transform.position = tmpV_cylinderRoller;
                    cylinderRoller.transform.rotation = tmpQ_cylinderRoller;
                    pistonRoller.transform.position = tmpV_pistonRoller;
                    pistonRoller.transform.rotation = tmpQ_pistonRoller;

                    alphaRoll = tmp_alphaRoll;
                    betaRoll = tmp_betaRoll;
                    gammaRoll = tmp_gammaRoll;
                }
            }
        }

        //Крен НПУ
        if (Input.GetKey(KeyCode.C))
        {
            if (allowRotation)
            {
                Vector3 tmpV_roller = roller.transform.position;
                Quaternion tmpQ_roller = roller.transform.rotation;
                Vector3 tmpV_cylinderRoller = cylinderRoller.transform.position;
                Quaternion tmpQ_cylinderRoller = cylinderRoller.transform.rotation;
                Vector3 tmpV_pistonRoller = pistonRoller.transform.position;
                Quaternion tmpQ_pistonRoller = pistonRoller.transform.rotation;
                float tmp_alphaRoll = alphaRoll;
                float tmp_betaRoll = betaRoll;
                float tmp_gammaRoll = gammaRoll;

                if (alphaRoll <= hitRollMax - currentSpeedRotation)
                    RAM.RotateRoll(-1, ref roller, ref cylinderRoller, ref pistonRoller,
                      ref alphaRoll, ref betaRoll, ref gammaRoll,
                      lRoll, dRoll, currentSpeedRotation);
                else if (alphaRoll < hitRollMax)
                    RAM.RotateRoll((alphaRoll - hitRollMax) / currentSpeedRotation, ref roller, ref cylinderRoller, ref pistonRoller,
                      ref alphaRoll, ref betaRoll, ref gammaRoll,
                      lRoll, dRoll, currentSpeedRotation);

                if (boxIntersection.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                    pitcher.transform.position = tmpV_roller;
                    pitcher.transform.rotation = tmpQ_roller;
                    cylinderRoller.transform.position = tmpV_cylinderRoller;
                    cylinderRoller.transform.rotation = tmpQ_cylinderRoller;
                    pistonRoller.transform.position = tmpV_pistonRoller;
                    pistonRoller.transform.rotation = tmpQ_pistonRoller;

                    alphaRoll = tmp_alphaRoll;
                    betaRoll = tmp_betaRoll;
                    gammaRoll = tmp_gammaRoll;
                }
            }
        }

        //Рыскание НПУ
        if (Input.GetKey(KeyCode.N))
        {
            Vector3 tmpV_yawer = yawer.transform.position;
            Quaternion tmpQ_yawer = yawer.transform.rotation;
            float tmp_alphaYaw = alphaYaw;

            if (allowRotation)
                //RAM.RotateYaw(1, ref yawer, ref alphaYaw, speedRotation);
                RAM.RotateBase_Yaw(1, ref yawer, ref alphaYaw, currentSpeedRotation);
            if (boxIntersection.DetectAllCollission())
            {
                notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                yawer.transform.position = tmpV_yawer;
                yawer.transform.rotation = tmpQ_yawer;

                alphaYaw = tmp_alphaYaw;
            }
        }

        //Рыскание НПУ
        if (Input.GetKey(KeyCode.B))
        {
            Vector3 tmpV_yawer = yawer.transform.position;
            Quaternion tmpQ_yawer = yawer.transform.rotation;
            float tmp_alphaYaw = alphaYaw;
            if (allowRotation)
                // RAM.RotateYaw(-1, ref yawer, ref alphaYaw, speedRotation);
                RAM.RotateBase_Yaw(-1, ref yawer, ref alphaYaw, currentSpeedRotation);
            if (boxIntersection.DetectAllCollission())
            {
                notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                yawer.transform.position = tmpV_yawer;
                yawer.transform.rotation = tmpQ_yawer;

                alphaYaw = tmp_alphaYaw;
            }
        }

        //Поднятие лап
        if (Input.GetKey(KeyCode.E))
        {
            Vector3 tmpV_footFR = footFR.transform.position;
            Quaternion tmpQ_footFR = footFR.transform.rotation;
            Vector3 tmpV_cylinderFR = cylinderFR.transform.position;
            Quaternion tmpQ_cylinderFR = cylinderFR.transform.rotation;
            Vector3 tmpV_pistonFR = pistonFR.transform.position;
            Quaternion tmpQ_pistonFR = pistonFR.transform.rotation;
            float tmp_alphaFR = alphaFR;
            float tmp_betaFR = betaFR;
            float tmp_gammaFR = gammaFR;

            Vector3 tmpV_footFL = footFL.transform.position;
            Quaternion tmpQ_footFL = footFL.transform.rotation;
            Vector3 tmpV_cylinderFL = cylinderFL.transform.position;
            Quaternion tmpQ_cylinderFL = cylinderFL.transform.rotation;
            Vector3 tmpV_pistonFL = pistonFL.transform.position;
            Quaternion tmpQ_pistonFL = pistonFL.transform.rotation;
            float tmp_alphaFL = alphaFL;
            float tmp_betaFL = betaFL;
            float tmp_gammaFL = gammaFL;

            Vector3 tmpV_footBR = footBR.transform.position;
            Quaternion tmpQ_footBR = footBR.transform.rotation;
            Vector3 tmpV_cylinderBR = cylinderBR.transform.position;
            Quaternion tmpQ_cylinderBR = cylinderBR.transform.rotation;
            Vector3 tmpV_pistonBR = pistonBR.transform.position;
            Quaternion tmpQ_pistonBR = pistonBR.transform.rotation;
            float tmp_alphaBR = alphaBR;
            float tmp_betaBR = betaBR;
            float tmp_gammaBR = gammaBR;

            Vector3 tmpV_footBL = footBL.transform.position;
            Quaternion tmpQ_footBL = footBL.transform.rotation;
            Vector3 tmpV_cylinderBL = cylinderBL.transform.position;
            Quaternion tmpQ_cylinderBL = cylinderBL.transform.rotation;
            Vector3 tmpV_pistonBL = pistonBL.transform.position;
            Quaternion tmpQ_pistonBL = pistonBL.transform.rotation;
            float tmp_alphaBL = alphaBL;
            float tmp_betaBL = betaBL;
            float tmp_gammaBL = gammaBL;


            if (Mathf.Abs(alphaFR) >= hitFootMin + currentSpeedRotation)
            {

                RAM.RaiseFoot(1, ref footFR, ref cylinderFR, ref pistonFR,
                    ref alphaFR, ref betaFR, ref gammaFR, lFR, dFR, currentSpeedRotation);
                RAM.RaiseFoot(1, ref footFL, ref cylinderFL, ref pistonFL,
                    ref alphaFL, ref betaFL, ref gammaFL, lFL, dFL, currentSpeedRotation);
                RAM.RaiseFoot(1, ref footBR, ref cylinderBR, ref pistonBR,
                    ref alphaBR, ref betaBR, ref gammaBR, lBR, dBR, currentSpeedRotation);
                RAM.RaiseFoot(1, ref footBL, ref cylinderBL, ref pistonBL,
                    ref alphaBL, ref betaBL, ref gammaBL, lBL, dBL, currentSpeedRotation);
            }
            else if (Mathf.Abs(alphaFR) > hitFootMin)
            {
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMin) / currentSpeedRotation, ref footFR, ref cylinderFR, ref pistonFR,
                   ref alphaFR, ref betaFR, ref gammaFR, lFR, dFR, currentSpeedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMin) / currentSpeedRotation, ref footFL, ref cylinderFL, ref pistonFL,
                    ref alphaFL, ref betaFL, ref gammaFL, lFL, dFL, currentSpeedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMin) / currentSpeedRotation, ref footBR, ref cylinderBR, ref pistonBR,
                    ref alphaBR, ref betaBR, ref gammaBR, lBR, dBR, currentSpeedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMin) / currentSpeedRotation, ref footBL, ref cylinderBL, ref pistonBL,
                    ref alphaBL, ref betaBL, ref gammaBL, lBL, dBL, currentSpeedRotation);
            }
            if (boxIntersection.DetectAllCollission())
            {
                notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                footFR.transform.position = tmpV_footFR;
                footFR.transform.rotation = tmpQ_footFR;
                cylinderFR.transform.position = tmpV_cylinderFR;
                cylinderFR.transform.rotation = tmpQ_cylinderFR;
                pistonFR.transform.position = tmpV_pistonFR;
                pistonFR.transform.rotation = tmpQ_pistonFR;
                alphaFR = tmp_alphaFR;
                betaFR = tmp_betaFR;
                gammaFR = tmp_gammaFR;

                footFL.transform.position = tmpV_footFL;
                footFL.transform.rotation = tmpQ_footFL;
                cylinderFL.transform.position = tmpV_cylinderFL;
                cylinderFL.transform.rotation = tmpQ_cylinderFL;
                pistonFL.transform.position = tmpV_pistonFL;
                pistonFL.transform.rotation = tmpQ_pistonFL;
                alphaFL = tmp_alphaFL;
                betaFL = tmp_betaFL;
                gammaFL = tmp_gammaFL;

                footBL.transform.position = tmpV_footBL;
                footBL.transform.rotation = tmpQ_footBL;
                cylinderBL.transform.position = tmpV_cylinderBL;
                cylinderBL.transform.rotation = tmpQ_cylinderBL;
                pistonBL.transform.position = tmpV_pistonBL;
                pistonBL.transform.rotation = tmpQ_pistonBL;
                alphaBL = tmp_alphaBL;
                betaBL = tmp_betaBL;
                gammaBL = tmp_gammaBL;

                footBR.transform.position = tmpV_footBR;
                footBR.transform.rotation = tmpQ_footBR;
                cylinderBR.transform.position = tmpV_cylinderBR;
                cylinderBR.transform.rotation = tmpQ_cylinderBR;
                pistonBR.transform.position = tmpV_pistonBR;
                pistonBR.transform.rotation = tmpQ_pistonBR;
                alphaBR = tmp_alphaBR;
                betaBR = tmp_betaBR;
                gammaBR = tmp_gammaBR;


            }
            allowRotation = false;
            allowMove = true;
        }

        //Опускание лап
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 tmpV_footFR = footFR.transform.position;
            Quaternion tmpQ_footFR = footFR.transform.rotation;
            Vector3 tmpV_cylinderFR = cylinderFR.transform.position;
            Quaternion tmpQ_cylinderFR = cylinderFR.transform.rotation;
            Vector3 tmpV_pistonFR = pistonFR.transform.position;
            Quaternion tmpQ_pistonFR = pistonFR.transform.rotation;
            float tmp_alphaFR = alphaFR;
            float tmp_betaFR = betaFR;
            float tmp_gammaFR = gammaFR;

            Vector3 tmpV_footFL = footFL.transform.position;
            Quaternion tmpQ_footFL = footFL.transform.rotation;
            Vector3 tmpV_cylinderFL = cylinderFL.transform.position;
            Quaternion tmpQ_cylinderFL = cylinderFL.transform.rotation;
            Vector3 tmpV_pistonFL = pistonFL.transform.position;
            Quaternion tmpQ_pistonFL = pistonFL.transform.rotation;
            float tmp_alphaFL = alphaFL;
            float tmp_betaFL = betaFL;
            float tmp_gammaFL = gammaFL;

            Vector3 tmpV_footBR = footBR.transform.position;
            Quaternion tmpQ_footBR = footBR.transform.rotation;
            Vector3 tmpV_cylinderBR = cylinderBR.transform.position;
            Quaternion tmpQ_cylinderBR = cylinderBR.transform.rotation;
            Vector3 tmpV_pistonBR = pistonBR.transform.position;
            Quaternion tmpQ_pistonBR = pistonBR.transform.rotation;
            float tmp_alphaBR = alphaBR;
            float tmp_betaBR = betaBR;
            float tmp_gammaBR = gammaBR;

            Vector3 tmpV_footBL = footBL.transform.position;
            Quaternion tmpQ_footBL = footBL.transform.rotation;
            Vector3 tmpV_cylinderBL = cylinderBL.transform.position;
            Quaternion tmpQ_cylinderBL = cylinderBL.transform.rotation;
            Vector3 tmpV_pistonBL = pistonBL.transform.position;
            Quaternion tmpQ_pistonBL = pistonBL.transform.rotation;
            float tmp_alphaBL = alphaBL;
            float tmp_betaBL = betaBL;
            float tmp_gammaBL = gammaBL;


            if (Mathf.Abs(alphaFR) <= hitFootMax - currentSpeedRotation)
            {
                RAM.RaiseFoot(-1, ref footFR, ref cylinderFR, ref pistonFR,
                     ref alphaFR, ref betaFR, ref gammaFR, lFR, dFR, currentSpeedRotation);
                RAM.RaiseFoot(-1, ref footFL, ref cylinderFL, ref pistonFL,
                    ref alphaFL, ref betaFL, ref gammaFL, lFL, dFL, currentSpeedRotation);
                RAM.RaiseFoot(-1, ref footBR, ref cylinderBR, ref pistonBR,
                    ref alphaBR, ref betaBR, ref gammaBR, lBR, dBR, currentSpeedRotation);
                RAM.RaiseFoot(-1, ref footBL, ref cylinderBL, ref pistonBL,
                    ref alphaBL, ref betaBL, ref gammaBL, lBL, dBL, currentSpeedRotation);
            }
            else if (Mathf.Abs(alphaFR) < hitFootMax)
            {
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMax) / currentSpeedRotation, ref footFR, ref cylinderFR, ref pistonFR,
                   ref alphaFR, ref betaFR, ref gammaFR, lFR, dFR, currentSpeedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMax) / currentSpeedRotation, ref footFL, ref cylinderFL, ref pistonFL,
                    ref alphaFL, ref betaFL, ref gammaFL, lFL, dFL, currentSpeedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMax) / currentSpeedRotation, ref footBR, ref cylinderBR, ref pistonBR,
                    ref alphaBR, ref betaBR, ref gammaBR, lBR, dBR, currentSpeedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMax) / currentSpeedRotation, ref footBL, ref cylinderBL, ref pistonBL,
                    ref alphaBL, ref betaBL, ref gammaBL, lBL, dBL, currentSpeedRotation);
            }
            if (boxIntersection.DetectAllCollission())
            {
                notificationSystem.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
                footFR.transform.position = tmpV_footFR;
                footFR.transform.rotation = tmpQ_footFR;
                cylinderFR.transform.position = tmpV_cylinderFR;
                cylinderFR.transform.rotation = tmpQ_cylinderFR;
                pistonFR.transform.position = tmpV_pistonFR;
                pistonFR.transform.rotation = tmpQ_pistonFR;
                alphaFR = tmp_alphaFR;
                betaFR = tmp_betaFR;
                gammaFR = tmp_gammaFR;

                footFL.transform.position = tmpV_footFL;
                footFL.transform.rotation = tmpQ_footFL;
                cylinderFL.transform.position = tmpV_cylinderFL;
                cylinderFL.transform.rotation = tmpQ_cylinderFL;
                pistonFL.transform.position = tmpV_pistonFL;
                pistonFL.transform.rotation = tmpQ_pistonFL;
                alphaFL = tmp_alphaFL;
                betaFL = tmp_betaFL;
                gammaFL = tmp_gammaFL;

                footBL.transform.position = tmpV_footBL;
                footBL.transform.rotation = tmpQ_footBL;
                cylinderBL.transform.position = tmpV_cylinderBL;
                cylinderBL.transform.rotation = tmpQ_cylinderBL;
                pistonBL.transform.position = tmpV_pistonBL;
                pistonBL.transform.rotation = tmpQ_pistonBL;
                alphaBL = tmp_alphaBL;
                betaBL = tmp_betaBL;
                gammaBL = tmp_gammaBL;

                footBR.transform.position = tmpV_footBR;
                footBR.transform.rotation = tmpQ_footBR;
                cylinderBR.transform.position = tmpV_cylinderBR;
                cylinderBR.transform.rotation = tmpQ_cylinderBR;
                pistonBR.transform.position = tmpV_pistonBR;
                pistonBR.transform.rotation = tmpQ_pistonBR;
                alphaBR = tmp_alphaBR;
                betaBR = tmp_betaBR;
                gammaBR = tmp_gammaBR;


            }
            if (Mathf.Abs(alphaFR) < hitFootMax && Mathf.Abs(alphaFR) > hitFootMax - defaultSpeedRotation)
            {
                allowRotation = true;
                allowMove = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Accessory")
        {
            if (!accessories.Contains(other.GetComponent<Accessory>())) accessories.Add(other.GetComponent<Accessory>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Accessory")
        {
            if (accessories.Contains(other.GetComponent<Accessory>())) accessories.Remove(other.GetComponent<Accessory>());
        }
    }

    private void OnDrawGizmos()
    {
        if (join)
        {
            Vector3 joinPos = join.position;
            Gizmos.color = colorNotConnect;
            Gizmos.DrawLine(joinPos, joinPos - cameraTurnOnDistance * Vector3.up);
            Gizmos.color = colorConnect;
            Gizmos.DrawLine(joinPos, joinPos - distanceToConnect * Vector3.up);
        }
    }
}