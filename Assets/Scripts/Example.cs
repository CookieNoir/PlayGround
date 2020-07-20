﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
public class Example : MonoBehaviour
{
    public GameObject player;//сама платформа

    protected GameObject body,
                        cameras,
                        arm0,
                        arm1,
                        arm2,
                        arm3,
                        arm4,
                        arm5,
                        arm6,
                        link1,
                        link2,
                        link3,
                        link4,
                        link5,
                        link6,
                        link7,
                        cylinder1,
                        cylinder2,
                        cylinder3,
                        cylinder5,
                        cylinder7,
                        piston1,
                        piston2,
                        piston3,
                        piston5,
                        piston7,
                        fixed3,
                        fixed5,
                        fixed7,
                        clutch5,
                        clutch7,
                        tong,

                        footFR,
                        cylinderFR,
                        pistonFR,
                        footFL,
                        cylinderFL,
                        pistonFL,
                        footBR,
                        cylinderBR,
                        pistonBR,
                        footBL,
                        cylinderBL,
                        pistonBL,

                        pitcher,
                        cylinderRoller,
                        roller,
                        yawer,
                        grip,
                        pistonRoller;

    private Vector3 body_startPosition,
                        cameras_startPosition,
                        arm0_startPosition,
                        arm1_startPosition,
                        arm2_startPosition,
                        arm3_startPosition,
                        arm4_startPosition,
                        arm5_startPosition,
                        arm6_startPosition,
                        link1_startPosition,
                        link2_startPosition,
                        link3_startPosition,
                        link4_startPosition,
                        link5_startPosition,
                        link6_startPosition,
                        cylinder1_startPosition,
                        cylinder2_startPosition,
                        cylinder3_startPosition,
                        cylinder5_startPosition,
                        cylinder7_startPosition,
                        piston1_startPosition,
                        piston2_startPosition,
                        piston3_startPosition,
                        piston5_startPosition,
                        piston7_startPosition,
                        fixed3_startPosition,
                        fixed5_startPosition,
                        fixed7_startPosition,
                        clutch5_startPosition,
                        clutch7_startPosition,
                        tong_startPosition,

                        footFR_startPosition,
                        cylinderFR_startPosition,
                        pistonFR_startPosition,
                        footFL_startPosition,
                        cylinderFL_startPosition,
                        pistonFL_startPosition,
                        footBR_startPosition,
                        cylinderBR_startPosition,
                        pistonBR_startPosition,
                        footBL_startPosition,
                        cylinderBL_startPosition,
                        pistonBL_startPositionv,

                        pitcher_startPosition,
                        cylinderRoller_startPosition,
                        roller_startPosition,
                        yawer_startPosition,
                        grip_startPosition,
                        pistonRoller_startPosition;

    private Quaternion body_startRotation,
                        cameras_startRotation,
                        arm0_startRotation,
                        arm1_startRotation,
                        arm2_startRotation,
                        arm3_startRotation,
                        arm4_startRotation,
                        arm5_startRotation,
                        arm6_startRotation,
                        link1_startRotation,
                        link2_startRotation,
                        link3_startRotation,
                        link4_startRotation,
                        link5_startRotation,
                        link6_startRotation,
                        cylinder1_startRotation,
                        cylinder2_startRotation,
                        cylinder3_startRotation,
                        cylinder5_startRotation,
                        cylinder7_startRotation,
                        piston1_startRotation,
                        piston2_startRotation,
                        piston3_startRotation,
                        piston5_startRotation,
                        piston7_startRotation,
                        fixed3_startRotation,
                        fixed5_startRotation,
                        fixed7_startRotation,
                        clutch5_startRotation,
                        clutch7_startRotation,
                        tong_startRotation,

                        footFR_startRotation,
                        cylinderFR_startRotation,
                        pistonFR_startRotation,
                        footFL_startRotation,
                        cylinderFL_startRotation,
                        pistonFL_startRotation,
                        footBR_startRotation,
                        cylinderBR_startRotation,
                        pistonBR_startRotation,
                        footBL_startRotation,
                        cylinderBL_startRotation,
                        pistonBL_startRotation,

                        pitcher_startRotation,
                        cylinderRoller_startRotation,
                        roller_startRotation,
                        yawer_startRotation,
                        grip_startRotation,
                        pistonRoller_startRotation;

    public float speed,
                 speedRotation,
                 speedElongation;
    private float startSpeed,
                  startSpeedRotation,
                  startSpeedElongation;

    public float PI = Mathf.PI;

    [HideInInspector] public bool touchSurface;
    public bool allowRotation;
    public bool allowMove;

    public float hitFootMin, //для лап
                 hitFootMax,
                 hit1Min,
                 hit1Max,
                 hit2Min,
                 hit2Max,
                 hitFractureMin,
                 hitFractureMax,
                 hit5Min,
                 hit5Max,
                 hit6Min,
                 hit6Max,
                 hitPitchMin,
                 hitPitchMax,
                 hitRollMin,
                 hitRollMax;
    // Длины сторон и углы для вычисления треугольников
    public float alpha0,
                    alpha1, beta1, gamma1, d1, l1,
                    alpha2, beta2, gamma2, d2, l2,
                    alpha3, beta3, gamma3, d3, l3,
                    alpha5, beta5, gamma5, phi5, psi51, psi52, theta5,
                    OA5, OB5, CD5, BD5, OC5, //Размеры, которые остаются постоянными
                    l6,
                    alphaPitch, beta7, gamma7, phi7, psi71, psi72, theta7,
                    OA7, OB7, CD7, BD7, OC7, //Размеры, которые остаются постоянными
                    alphaFR, betaFR, gammaFR, dFR, lFR,
                    alphaFL, betaFL, gammaFL, dFL, lFL,
                    alphaBR, betaBR, gammaBR, dBR, lBR,
                    alphaBL, betaBL, gammaBL, dBL, lBL,
                    alphaRoll, betaRoll, gammaRoll, lRoll, dRoll,
                    alphaYaw;

    [Header("Самопересечения")]
    public BoxMainObject test;
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

    public enum EnumAccessory { empty, hydrohammer, grab };
    public static EnumAccessory IsEquip = EnumAccessory.empty;

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
        test.SetLast(box);
    }

    public void SetDefaultAntiIntersectionBox()
    {
        test.SetLast(defaultBox);
    }

    private void WriteStartValues()
    {
        startSpeed = speed;
        startSpeedRotation = speedRotation;
        startSpeedElongation = speedElongation;

        body_startPosition = body.transform.localPosition;
        cameras_startPosition = cameras.transform.localPosition;
        arm0_startPosition = arm0.transform.localPosition;
        arm1_startPosition = arm1.transform.localPosition;
        arm2_startPosition = arm2.transform.localPosition;
        arm3_startPosition = arm3.transform.localPosition;
        arm4_startPosition = arm4.transform.localPosition;
        arm5_startPosition = arm5.transform.localPosition;
        arm6_startPosition = arm6.transform.localPosition;
        link1_startPosition = link1.transform.localPosition;
        link2_startPosition = link2.transform.localPosition;
        link3_startPosition = link3.transform.localPosition;
        link4_startPosition = link4.transform.localPosition;
        link5_startPosition = link5.transform.localPosition;
        link6_startPosition = link6.transform.localPosition;
        cylinder1_startPosition = cylinder1.transform.localPosition;
        cylinder2_startPosition = cylinder2.transform.localPosition;
        cylinder3_startPosition = cylinder3.transform.localPosition;
        cylinder5_startPosition = cylinder5.transform.localPosition;
        cylinder7_startPosition = cylinder7.transform.localPosition;
        piston1_startPosition = piston1.transform.localPosition;
        piston2_startPosition = piston2.transform.localPosition;
        piston3_startPosition = piston3.transform.localPosition;
        piston5_startPosition = piston5.transform.localPosition;
        piston7_startPosition = piston7.transform.localPosition;
        fixed3_startPosition = fixed3.transform.localPosition;
        fixed5_startPosition = fixed5.transform.localPosition;
        fixed7_startPosition = fixed7.transform.localPosition;
        clutch5_startPosition = clutch5.transform.localPosition;
        clutch7_startPosition = clutch7.transform.localPosition;
        tong_startPosition = tong.transform.localPosition;

        footFR_startPosition = footFR.transform.localPosition;
        cylinderFR_startPosition = cylinderFR.transform.localPosition;
        pistonFR_startPosition = pistonFR.transform.localPosition;
        footFL_startPosition = footFL.transform.localPosition;
        cylinderFL_startPosition = cylinderFL.transform.localPosition;
        pistonFL_startPosition = pistonFL.transform.localPosition;
        footBR_startPosition = footBR.transform.localPosition;
        cylinderBR_startPosition = cylinderBR.transform.localPosition;
        pistonBR_startPosition = pistonBR.transform.localPosition;
        footBL_startPosition = footBL.transform.localPosition;
        cylinderBL_startPosition = cylinderBL.transform.localPosition;
        pistonBL_startPositionv = pistonBL.transform.localPosition;

        pitcher_startPosition = pitcher.transform.localPosition;
        cylinderRoller_startPosition = cylinderRoller.transform.localPosition;
        roller_startPosition = roller.transform.localPosition;
        yawer_startPosition = yawer.transform.localPosition;
        grip_startPosition = grip.transform.localPosition;
        pistonRoller_startPosition = pistonRoller.transform.localPosition;

        // Rotation
        body_startRotation = body.transform.localRotation;
        cameras_startRotation = cameras.transform.localRotation;
        arm0_startRotation = arm0.transform.localRotation;
        arm1_startRotation = arm1.transform.localRotation;
        arm2_startRotation = arm2.transform.localRotation;
        arm3_startRotation = arm3.transform.localRotation;
        arm4_startRotation = arm4.transform.localRotation;
        arm5_startRotation = arm5.transform.localRotation;
        arm6_startRotation = arm6.transform.localRotation;
        link1_startRotation = link1.transform.localRotation;
        link2_startRotation = link2.transform.localRotation;
        link3_startRotation = link3.transform.localRotation;
        link4_startRotation = link4.transform.localRotation;
        link5_startRotation = link5.transform.localRotation;
        link6_startRotation = link6.transform.localRotation;
        cylinder1_startRotation = cylinder1.transform.localRotation;
        cylinder2_startRotation = cylinder2.transform.localRotation;
        cylinder3_startRotation = cylinder3.transform.localRotation;
        cylinder5_startRotation = cylinder5.transform.localRotation;
        cylinder7_startRotation = cylinder7.transform.localRotation;
        piston1_startRotation = piston1.transform.localRotation;
        piston2_startRotation = piston2.transform.localRotation;
        piston3_startRotation = piston3.transform.localRotation;
        piston5_startRotation = piston5.transform.localRotation;
        piston7_startRotation = piston7.transform.localRotation;
        fixed3_startRotation = fixed3.transform.localRotation;
        fixed5_startRotation = fixed5.transform.localRotation;
        fixed7_startRotation = fixed7.transform.localRotation;
        clutch5_startRotation = clutch5.transform.localRotation;
        clutch7_startRotation = clutch7.transform.localRotation;
        tong_startRotation = tong.transform.localRotation;

        footFR_startRotation = footFR.transform.localRotation;
        cylinderFR_startRotation = cylinderFR.transform.localRotation;
        pistonFR_startRotation = pistonFR.transform.localRotation;
        footFL_startRotation = footFL.transform.localRotation;
        cylinderFL_startRotation = cylinderFL.transform.localRotation;
        pistonFL_startRotation = pistonFL.transform.localRotation;
        footBR_startRotation = footBR.transform.localRotation;
        cylinderBR_startRotation = cylinderBR.transform.localRotation;
        pistonBR_startRotation = pistonBR.transform.localRotation;
        footBL_startRotation = footBL.transform.localRotation;
        cylinderBL_startRotation = cylinderBL.transform.localRotation;
        pistonBL_startRotation = pistonBL.transform.localRotation;

        pitcher_startRotation = pitcher.transform.localRotation;
        cylinderRoller_startRotation = cylinderRoller.transform.localRotation;
        roller_startRotation = roller.transform.localRotation;
        yawer_startRotation = yawer.transform.localRotation;
        grip_startRotation = grip.transform.localRotation;
        pistonRoller_startRotation = pistonRoller.transform.localRotation;
    }

    public void SetStartValues()
    {
        speed = startSpeed;
        speedRotation = startSpeedRotation;
        speedElongation = startSpeedElongation;

        body.transform.localPosition = body_startPosition;
        cameras.transform.localPosition = cameras_startPosition;
        arm0.transform.localPosition = arm0_startPosition;
        arm1.transform.localPosition = arm1_startPosition;
        arm2.transform.localPosition = arm2_startPosition;
        arm3.transform.localPosition = arm3_startPosition;
        arm4.transform.localPosition = arm4_startPosition;
        arm5.transform.localPosition = arm5_startPosition;
        arm6.transform.localPosition = arm6_startPosition;
        link1.transform.localPosition = link1_startPosition;
        link2.transform.localPosition = link2_startPosition;
        link3.transform.localPosition = link3_startPosition;
        link4.transform.localPosition = link4_startPosition;
        link5.transform.localPosition = link5_startPosition;
        link6.transform.localPosition = link6_startPosition;
        cylinder1.transform.localPosition = cylinder1_startPosition;
        cylinder2.transform.localPosition = cylinder2_startPosition;
        cylinder3.transform.localPosition = cylinder3_startPosition;
        cylinder5.transform.localPosition = cylinder5_startPosition;
        cylinder7.transform.localPosition = cylinder7_startPosition;
        piston1.transform.localPosition = piston1_startPosition;
        piston2.transform.localPosition = piston2_startPosition;
        piston3.transform.localPosition = piston3_startPosition;
        piston5.transform.localPosition = piston5_startPosition;
        piston7.transform.localPosition = piston7_startPosition;
        fixed3.transform.localPosition = fixed3_startPosition;
        fixed5.transform.localPosition = fixed5_startPosition;
        fixed7.transform.localPosition = fixed7_startPosition;
        clutch5.transform.localPosition = clutch5_startPosition;
        clutch7.transform.localPosition = clutch7_startPosition;
        tong.transform.localPosition = tong_startPosition;

        footFR.transform.localPosition = footFR_startPosition;
        cylinderFR.transform.localPosition = cylinderFR_startPosition;
        pistonFR.transform.localPosition = pistonFR_startPosition;
        footFL.transform.localPosition = footFL_startPosition;
        cylinderFL.transform.localPosition = cylinderFL_startPosition;
        pistonFL.transform.localPosition = pistonFL_startPosition;
        footBR.transform.localPosition = footBR_startPosition;
        cylinderBR.transform.localPosition = cylinderBR_startPosition;
        pistonBR.transform.localPosition = pistonBR_startPosition;
        footBL.transform.localPosition = footBL_startPosition;
        cylinderBL.transform.localPosition = cylinderBL_startPosition;
        pistonBL.transform.localPosition = pistonBL_startPositionv;

        pitcher.transform.localPosition = pitcher_startPosition;
        cylinderRoller.transform.localPosition = cylinderRoller_startPosition;
        roller.transform.localPosition = roller_startPosition;
        yawer.transform.localPosition = yawer_startPosition;
        grip.transform.localPosition = grip_startPosition;
        pistonRoller.transform.localPosition = pistonRoller_startPosition;

        // Rotation
        body.transform.localRotation = body_startRotation;
        cameras.transform.localRotation = cameras_startRotation;
        arm0.transform.localRotation = arm0_startRotation;
        arm1.transform.localRotation = arm1_startRotation;
        arm2.transform.localRotation = arm2_startRotation;
        arm3.transform.localRotation = arm3_startRotation;
        arm4.transform.localRotation = arm4_startRotation;
        arm5.transform.localRotation = arm5_startRotation;
        arm6.transform.localRotation = arm6_startRotation;
        link1.transform.localRotation = link1_startRotation;
        link2.transform.localRotation = link2_startRotation;
        link3.transform.localRotation = link3_startRotation;
        link4.transform.localRotation = link4_startRotation;
        link5.transform.localRotation = link5_startRotation;
        link6.transform.localRotation = link6_startRotation;
        cylinder1.transform.localRotation = cylinder1_startRotation;
        cylinder2.transform.localRotation = cylinder2_startRotation;
        cylinder3.transform.localRotation = cylinder3_startRotation;
        cylinder5.transform.localRotation = cylinder5_startRotation;
        cylinder7.transform.localRotation = cylinder7_startRotation;
        piston1.transform.localRotation = piston1_startRotation;
        piston2.transform.localRotation = piston2_startRotation;
        piston3.transform.localRotation = piston3_startRotation;
        piston5.transform.localRotation = piston5_startRotation;
        piston7.transform.localRotation = piston7_startRotation;
        fixed3.transform.localRotation = fixed3_startRotation;
        fixed5.transform.localRotation = fixed5_startRotation;
        fixed7.transform.localRotation = fixed7_startRotation;
        clutch5.transform.localRotation = clutch5_startRotation;
        clutch7.transform.localRotation = clutch7_startRotation;
        tong.transform.localRotation = tong_startRotation;

        footFR.transform.localRotation = footFR_startRotation;
        cylinderFR.transform.localRotation = cylinderFR_startRotation;
        pistonFR.transform.localRotation = pistonFR_startRotation;
        footFL.transform.localRotation = footFL_startRotation;
        cylinderFL.transform.localRotation = cylinderFL_startRotation;
        pistonFL.transform.localRotation = pistonFL_startRotation;
        footBR.transform.localRotation = footBR_startRotation;
        cylinderBR.transform.localRotation = cylinderBR_startRotation;
        pistonBR.transform.localRotation = pistonBR_startRotation;
        footBL.transform.localRotation = footBL_startRotation;
        cylinderBL.transform.localRotation = cylinderBL_startRotation;
        pistonBL.transform.localRotation = pistonBL_startRotation;

        pitcher.transform.localRotation = pitcher_startRotation;
        cylinderRoller.transform.localRotation = cylinderRoller_startRotation;
        roller.transform.localRotation = roller_startRotation;
        yawer.transform.localRotation = yawer_startRotation;
        grip.transform.localRotation = grip_startRotation;
        pistonRoller.transform.localRotation = pistonRoller_startRotation;

        SetSize();
        SetHits();

        allowMove = true;
        allowRotation = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        TieObjiects();

        generalCenterOfMass = GetComponent<GeneralCenterOfMass>();

        speedRotation = 0.5f * Time.deltaTime;
        speed = 2.5f * Time.deltaTime;
        speedElongation = 0.5f * Time.deltaTime;

        // расчет сторон и углов треугольников
        SetSize();
        SetHits();

        WriteStartValues();

        maxOverloadInverse = 1f / maxOverload;

        //touchSurface = true;
        allowRotation = false;
        allowMove = true;

        accessory = null;
        accessories = new List<Accessory>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (!contact.thisCollider.CompareTag("Caterpillar") &&
                !contact.thisCollider.CompareTag("Leg") &&
                !contact.thisCollider.CompareTag("Accessory"))
            {
                notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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
        if (accessory)
        {
            accessory.Unequip();
            IsEquip = EnumAccessory.empty;
            equipped = false;
        }
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
                            joinCameraUI.gameObject.SetActive(true);
                            joinCameraText.text = dist.ToString("F8");
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
                    joinCameraText.text = distance.ToString("F8");
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
                        accessory.Equip(join, this);

                        if (accessory.gameObject.name == "Гидромолот")
                            IsEquip = EnumAccessory.hydrohammer;
                        else
                            IsEquip = EnumAccessory.grab;

                        equipped = true;
                        notificationSystem.Notify(NotificationSystem.notifyTypes.message, "Оборудование сменено на " + accessory.name);
                    }
                    else
                    {
                        notificationSystem.Notify(NotificationSystem.notifyTypes.warning, "Для присоединения сократите дистанцию");
                    }
                }
            }
            else
            {
                UnequipAccessory();
                notificationSystem.Notify(NotificationSystem.notifyTypes.message, "Оборудование снято");
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
                notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Перегруз");
                overloadTimer = 5f;
            }
            else
            {
                overloadTimer -= Time.deltaTime;
            }
        }
        else if (f > 1) f = 1;
        f = f * f * (3 - 2 * f);
        speed = startSpeed * f;
        speedRotation = startSpeedRotation * f;
    }

    // Обработка нажатия клавиш
    void FixedUpdate()
    {
        //Движение
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (allowMove && touchSurface)
                player.transform.position += body.transform.forward * speed;
        }

        //Движение
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (allowMove && touchSurface)
                player.transform.position -= body.transform.forward * speed;
        }

        //Поворот
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (allowMove && touchSurface)
            {
                player.transform.RotateAround(body.transform.position, body.transform.up, -speedRotation * Mathf.Rad2Deg);
            }
        }

        //Поворот
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (allowMove && touchSurface)
            {
                player.transform.RotateAround(body.transform.position, body.transform.up, speedRotation * Mathf.Rad2Deg);
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
                RAM.RotateBase_Yaw(1, ref arm0, ref alpha0, speedRotation);

                //если после вращения произошло самопересечение, то вовращаем все назад
                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                RAM.RotateBase_Yaw(-1, ref arm0, ref alpha0, speedRotation);
                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alpha1 <= hit1Max - speedRotation)
                    RAM.RotateArmEasy(1, ref arm1, ref cylinder1, ref piston1,
                        ref alpha1, ref beta1, ref gamma1,
                        l1, d1, speedRotation);
                else if (alpha1 < hit1Max)
                    RAM.RotateArmEasy((hit1Max - alpha1) / speedRotation, ref arm1, ref cylinder1, ref piston1,
                        ref alpha1, ref beta1, ref gamma1,
                        l1, d1, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alpha1 >= hit1Min + speedRotation)
                    RAM.RotateArmEasy(-1, ref arm1, ref cylinder1, ref piston1,
                        ref alpha1, ref beta1, ref gamma1,
                        l1, d1, speedRotation);
                else if (alpha1 > hit1Min)
                    RAM.RotateArmEasy((hit1Min - alpha1) / speedRotation, ref arm1, ref cylinder1, ref piston1,
                        ref alpha1, ref beta1, ref gamma1,
                        l1, d1, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alpha2 <= hit2Max - speedRotation)
                    RAM.RotateArmEasy(1, ref arm2, ref cylinder2, ref piston2,
                        ref alpha2, ref beta2, ref gamma2,
                        l2, d2, speedRotation);
                else if (alpha2 < hit2Max)
                    RAM.RotateArmEasy((hit2Max - alpha2) / speedRotation, ref arm2, ref cylinder2, ref piston2,
                        ref alpha2, ref beta2, ref gamma2,
                        l2, d2, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alpha2 >= hit2Min + speedRotation)
                    RAM.RotateArmEasy(-1, ref arm2, ref cylinder2, ref piston2,
                       ref alpha2, ref beta2, ref gamma2,
                       l2, d2, speedRotation);
                else if (alpha2 > hit2Min)
                    RAM.RotateArmEasy((hit2Min - alpha2) / speedRotation, ref arm2, ref cylinder2, ref piston2,
                       ref alpha2, ref beta2, ref gamma2,
                       l2, d2, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alpha3 >= hitFractureMin + speedRotation)
                    RAM.Fracture(1, ref arm3, ref arm4, ref cylinder3, ref piston3, ref fixed3,
                        ref alpha3, ref beta3, ref gamma3,
                        l3, d3, speedRotation);
                else if (alpha3 > hitFractureMin)
                    RAM.Fracture((alpha3 - hitFractureMin) / speedRotation, ref arm3, ref arm4, ref cylinder3, ref piston3, ref fixed3,
                       ref alpha3, ref beta3, ref gamma3,
                       l3, d3, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alpha3 <= hitFractureMax - speedRotation)
                    RAM.Fracture(-1, ref arm3, ref arm4, ref cylinder3, ref piston3, ref fixed3,
                       ref alpha3, ref beta3, ref gamma3,
                       l3, d3, speedRotation);
                else if (alpha3 < hitFractureMax)
                    RAM.Fracture((alpha3 - hitFractureMax) / speedRotation, ref arm3, ref arm4, ref cylinder3, ref piston3, ref fixed3,
                       ref alpha3, ref beta3, ref gamma3,
                       l3, d3, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alpha5 >= hit5Min + speedRotation)
                    RAM.RotateArmHard_Pitch(1, ref arm5, ref cylinder5, ref piston5, ref clutch5, ref fixed5,
                        ref alpha5, ref beta5, ref gamma5, ref theta5, ref phi5, ref psi51, ref psi52,
                        OA5, OB5, OC5, BD5, CD5, speedRotation);
                else if (alpha5 > hit5Min)
                    RAM.RotateArmHard_Pitch((alpha5 - hit5Min) / speedRotation, ref arm5, ref cylinder5, ref piston5, ref clutch5, ref fixed5,
                        ref alpha5, ref beta5, ref gamma5, ref theta5, ref phi5, ref psi51, ref psi52,
                        OA5, OB5, OC5, BD5, CD5, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alpha5 <= hit5Max - speedRotation)
                    RAM.RotateArmHard_Pitch(-1, ref arm5, ref cylinder5, ref piston5, ref clutch5, ref fixed5,
                       ref alpha5, ref beta5, ref gamma5, ref theta5, ref phi5, ref psi51, ref psi52,
                       OA5, OB5, OC5, BD5, CD5, speedRotation);
                else if (alpha5 < hit5Max)
                    RAM.RotateArmHard_Pitch((alpha5 - hit5Max) / speedRotation, ref arm5, ref cylinder5, ref piston5, ref clutch5, ref fixed5,
                       ref alpha5, ref beta5, ref gamma5, ref theta5, ref phi5, ref psi51, ref psi52,
                       OA5, OB5, OC5, BD5, CD5, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (l6 <= hit6Max - speedElongation)
                    RAM.Elongation(1, ref arm6, ref l6, speedElongation);
                else if (l6 < hit6Max)
                    RAM.Elongation((hit6Max - l6) / speedElongation, ref arm6, ref l6, speedElongation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (l6 >= hit6Min + speedElongation)
                    RAM.Elongation(-1, ref arm6, ref l6, speedElongation);
                else if (l6 > hit6Min)
                    RAM.Elongation((hit6Min - l6) / speedElongation, ref arm6, ref l6, speedElongation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alphaPitch >= hitPitchMin + speedRotation)
                    RAM.RotateArmHard_Pitch(1, ref pitcher, ref cylinder7, ref piston7, ref clutch7, ref fixed7,
                        ref alphaPitch, ref beta7, ref gamma7, ref theta7, ref phi7, ref psi71, ref psi72,
                        OA7, OB7, OC7, BD7, CD7, speedRotation);
                else if (alphaPitch > hitPitchMin)
                    RAM.RotateArmHard_Pitch((alphaPitch - hitPitchMin) / speedRotation, ref pitcher, ref cylinder7, ref piston7, ref clutch7, ref fixed7,
                        ref alphaPitch, ref beta7, ref gamma7, ref theta7, ref phi7, ref psi71, ref psi72,
                        OA7, OB7, OC7, BD7, CD7, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alphaPitch <= hitPitchMax - speedRotation)
                    RAM.RotateArmHard_Pitch(-1, ref pitcher, ref cylinder7, ref piston7, ref clutch7, ref fixed7,
                        ref alphaPitch, ref beta7, ref gamma7, ref theta7, ref phi7, ref psi71, ref psi72,
                        OA7, OB7, OC7, BD7, CD7, speedRotation);
                else if (alphaPitch < hitPitchMax)
                    RAM.RotateArmHard_Pitch((alphaPitch - hitPitchMax) / speedRotation, ref pitcher, ref cylinder7, ref piston7, ref clutch7, ref fixed7,
                        ref alphaPitch, ref beta7, ref gamma7, ref theta7, ref phi7, ref psi71, ref psi72,
                        OA7, OB7, OC7, BD7, CD7, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alphaRoll >= hitRollMin + speedRotation)
                    RAM.RotateRoll(1, ref roller, ref cylinderRoller, ref pistonRoller,
                       ref alphaRoll, ref betaRoll, ref gammaRoll,
                       lRoll, dRoll, speedRotation);
                else if (alphaRoll > hitRollMin)
                    RAM.RotateRoll((alphaRoll - hitRollMin) / speedRotation, ref roller, ref cylinderRoller, ref pistonRoller,
                       ref alphaRoll, ref betaRoll, ref gammaRoll,
                       lRoll, dRoll, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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

                if (alphaRoll <= hitRollMax - speedRotation)
                    RAM.RotateRoll(-1, ref roller, ref cylinderRoller, ref pistonRoller,
                      ref alphaRoll, ref betaRoll, ref gammaRoll,
                      lRoll, dRoll, speedRotation);
                else if (alphaRoll < hitRollMax)
                    RAM.RotateRoll((alphaRoll - hitRollMax) / speedRotation, ref roller, ref cylinderRoller, ref pistonRoller,
                      ref alphaRoll, ref betaRoll, ref gammaRoll,
                      lRoll, dRoll, speedRotation);

                if (test.DetectAllCollission())
                {
                    notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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
                RAM.RotateBase_Yaw(1, ref yawer, ref alphaYaw, speedRotation);
            if (test.DetectAllCollission())
            {
                notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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
                RAM.RotateBase_Yaw(-1, ref yawer, ref alphaYaw, speedRotation);
            if (test.DetectAllCollission())
            {
                notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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


            if (Mathf.Abs(alphaFR) >= hitFootMin + speedRotation)
            {

                RAM.RaiseFoot(1, ref footFR, ref cylinderFR, ref pistonFR,
                    ref alphaFR, ref betaFR, ref gammaFR, lFR, dFR, speedRotation);
                RAM.RaiseFoot(1, ref footFL, ref cylinderFL, ref pistonFL,
                    ref alphaFL, ref betaFL, ref gammaFL, lFL, dFL, speedRotation);
                RAM.RaiseFoot(1, ref footBR, ref cylinderBR, ref pistonBR,
                    ref alphaBR, ref betaBR, ref gammaBR, lBR, dBR, speedRotation);
                RAM.RaiseFoot(1, ref footBL, ref cylinderBL, ref pistonBL,
                    ref alphaBL, ref betaBL, ref gammaBL, lBL, dBL, speedRotation);
            }
            else if (Mathf.Abs(alphaFR) > hitFootMin)
            {
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMin) / speedRotation, ref footFR, ref cylinderFR, ref pistonFR,
                   ref alphaFR, ref betaFR, ref gammaFR, lFR, dFR, speedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMin) / speedRotation, ref footFL, ref cylinderFL, ref pistonFL,
                    ref alphaFL, ref betaFL, ref gammaFL, lFL, dFL, speedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMin) / speedRotation, ref footBR, ref cylinderBR, ref pistonBR,
                    ref alphaBR, ref betaBR, ref gammaBR, lBR, dBR, speedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMin) / speedRotation, ref footBL, ref cylinderBL, ref pistonBL,
                    ref alphaBL, ref betaBL, ref gammaBL, lBL, dBL, speedRotation);
            }
            if (test.DetectAllCollission())
            {
                notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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


            if (Mathf.Abs(alphaFR) <= hitFootMax - speedRotation)
            {
                RAM.RaiseFoot(-1, ref footFR, ref cylinderFR, ref pistonFR,
                     ref alphaFR, ref betaFR, ref gammaFR, lFR, dFR, speedRotation);
                RAM.RaiseFoot(-1, ref footFL, ref cylinderFL, ref pistonFL,
                    ref alphaFL, ref betaFL, ref gammaFL, lFL, dFL, speedRotation);
                RAM.RaiseFoot(-1, ref footBR, ref cylinderBR, ref pistonBR,
                    ref alphaBR, ref betaBR, ref gammaBR, lBR, dBR, speedRotation);
                RAM.RaiseFoot(-1, ref footBL, ref cylinderBL, ref pistonBL,
                    ref alphaBL, ref betaBL, ref gammaBL, lBL, dBL, speedRotation);
            }
            else if (Mathf.Abs(alphaFR) < hitFootMax)
            {
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMax) / speedRotation, ref footFR, ref cylinderFR, ref pistonFR,
                   ref alphaFR, ref betaFR, ref gammaFR, lFR, dFR, speedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMax) / speedRotation, ref footFL, ref cylinderFL, ref pistonFL,
                    ref alphaFL, ref betaFL, ref gammaFL, lFL, dFL, speedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMax) / speedRotation, ref footBR, ref cylinderBR, ref pistonBR,
                    ref alphaBR, ref betaBR, ref gammaBR, lBR, dBR, speedRotation);
                RAM.RaiseFoot((Mathf.Abs(alphaFR) - hitFootMax) / speedRotation, ref footBL, ref cylinderBL, ref pistonBL,
                    ref alphaBL, ref betaBL, ref gammaBL, lBL, dBL, speedRotation);
            }
            if (test.DetectAllCollission())
            {
                notificationSystem.Notify(NotificationSystem.notifyTypes.alert, "Удар");
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
            if (Mathf.Abs(alphaFR) < hitFootMax && Mathf.Abs(alphaFR) > hitFootMax - startSpeedRotation)
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



    // Привязка деталей
    void TieObjiects()
    {
        player = GameObject.Find("MainGameObject");
        body = GameObject.Find("body");
        cameras = GameObject.Find("cameras");

        // Связывание стрел
        {
            arm0 = GameObject.Find("arm0");
            arm1 = GameObject.Find("arm1");
            arm2 = GameObject.Find("arm2");
            arm3 = GameObject.Find("arm3");
            arm4 = GameObject.Find("arm4");
            arm5 = GameObject.Find("arm5");
            arm6 = GameObject.Find("arm6");

            link1 = GameObject.Find("link1");
            link2 = GameObject.Find("link2");
            link3 = GameObject.Find("link3");
            link4 = GameObject.Find("link4");
            link5 = GameObject.Find("link5");
            link6 = GameObject.Find("link6");

            cylinder1 = GameObject.Find("cylinder1");
            cylinder2 = GameObject.Find("cylinder2");
            cylinder3 = GameObject.Find("cylinder3");
            cylinder5 = GameObject.Find("cylinder5");
            cylinder7 = GameObject.Find("cylinder7");

            piston1 = GameObject.Find("piston1");
            piston2 = GameObject.Find("piston2");
            piston3 = GameObject.Find("piston3");
            piston5 = GameObject.Find("piston5");
            piston7 = GameObject.Find("piston7");


            fixed3 = GameObject.Find("fixed3");
            fixed5 = GameObject.Find("fixed5");
            fixed7 = GameObject.Find("fixed7");
            clutch5 = GameObject.Find("clutch5");
            clutch7 = GameObject.Find("clutch7");
            tong = GameObject.Find("tong");
        }

        //Связывание лап
        {
            footFR = GameObject.Find("footFR");
            cylinderFR = GameObject.Find("cylinderFR");
            pistonFR = GameObject.Find("pistonFR");

            footBL = GameObject.Find("footBL");
            cylinderBL = GameObject.Find("cylinderBL");
            pistonBL = GameObject.Find("pistonBL");

            footBR = GameObject.Find("footBR");
            cylinderBR = GameObject.Find("cylinderBR");
            pistonBR = GameObject.Find("pistonBR");

            footFL = GameObject.Find("footFL");
            cylinderFL = GameObject.Find("cylinderFL");
            pistonFL = GameObject.Find("pistonFL");
        }

        //Связывание НПУ
        {
            pitcher = GameObject.Find("pitcher");
            cylinderRoller = GameObject.Find("cylinderRoller");
            roller = GameObject.Find("roller");
            yawer = GameObject.Find("yawer");
            grip = GameObject.Find("grip");
            pistonRoller = GameObject.Find("pistonRoller");
        }
    }

    //Задание размеров
    void SetSize()
    {
        float h; //Переменная для вычисления изменяющихся сторон треугольников

        //Угол поворота базы
        {
            alpha0 = 0;
        }

        //Размеры для первой стрелы
        {
            h = OtherMath.DistanceYZ(cylinder1, piston1);
            l1 = OtherMath.DistanceYZ(cylinder1, link1);
            d1 = OtherMath.DistanceYZ(piston1, link1);
            alpha1 = Mathf.Acos((l1 * l1 + d1 * d1 - h * h) / (2 * l1 * d1));
            beta1 = OtherMath.CalcBeta(l1, d1, h, alpha1);
            gamma1 = PI - alpha1 - beta1;
        }

        //Размеры для второй стрелы
        {
            h = OtherMath.DistanceYZ(cylinder2, piston2);
            l2 = OtherMath.DistanceYZ(cylinder2, link2);
            d2 = OtherMath.DistanceYZ(piston2, link2);
            alpha2 = Mathf.Acos((l2 * l2 + d2 * d2 - h * h) / (2 * l2 * d2));
            beta2 = OtherMath.CalcBeta(l2, d2, h, alpha2);
            gamma2 = PI - alpha2 - beta2;
        }

        //Размеры для излома
        {
            cylinder3.transform.SetParent(arm3.transform);
            GameObject emptyArm = new GameObject();
            emptyArm.transform.localPosition = new Vector3(0, 0, 0);
            h = OtherMath.DistanceLocalYZ(cylinder3, piston3);
            l3 = OtherMath.DistanceLocalYZ(cylinder3, emptyArm);
            d3 = OtherMath.DistanceLocalYZ(piston3, emptyArm);
            alpha3 = Mathf.Acos((l3 * l3 + d3 * d3 - h * h) / (2 * l3 * d3));
            beta3 = OtherMath.CalcBeta(l3, d3, h, alpha3);
            gamma3 = PI - alpha3 - beta3;
            cylinder3.transform.SetParent(arm2.transform);
            Destroy(emptyArm);
        }

        //Размеры для четвертой стрелы
        {
            OA5 = OtherMath.DistanceYZ(fixed5, cylinder5);
            OB5 = OtherMath.DistanceYZ(fixed5, piston5);
            h = OtherMath.DistanceYZ(piston5, cylinder5);
            alpha5 = Mathf.Acos((OA5 * OA5 + OB5 * OB5 - h * h) / (2 * OB5 * OA5));
            beta5 = OtherMath.CalcBeta(OA5, OB5, h, alpha5);
            gamma5 = PI - alpha5 - beta5;
            OC5 = OtherMath.DistanceYZ(fixed5, link5);
            BD5 = OtherMath.DistanceYZ(piston5, clutch5);
            CD5 = OtherMath.DistanceYZ(link5, clutch5);
            h = OtherMath.DistanceYZ(piston5, link5);
            theta5 = Mathf.Acos((BD5 * BD5 + CD5 * CD5 - h * h) / (2 * BD5 * CD5));
            psi51 = Mathf.Acos((OC5 * OC5 + h * h - OB5 * OB5) / (2 * OC5 * h));
            psi52 = OtherMath.CalcBeta(CD5, BD5, h, theta5);
            phi5 = OtherMath.CalcBeta(OC5, h, OB5, psi51);
        }

        //Величина удлинения шестой стрелы
        {
            l6 = OtherMath.DistanceYZ(link5, link6);
        }

        //Размеры вычисления тангажа НПУ
        {
            OA7 = OtherMath.DistanceYZ(fixed7, cylinder7);
            OB7 = OtherMath.DistanceYZ(fixed7, piston7);
            h = OtherMath.DistanceYZ(piston7, cylinder7);
            alphaPitch = Mathf.Acos((OA7 * OA7 + OB7 * OB7 - h * h) / (2 * OB7 * OA7));
            beta7 = OtherMath.CalcBeta(OA7, OB7, h, alphaPitch);
            gamma7 = PI - alphaPitch - beta7;
            OC7 = OtherMath.DistanceYZ(fixed7, tong);
            BD7 = OtherMath.DistanceYZ(piston7, clutch7);
            CD7 = OtherMath.DistanceYZ(tong, clutch7);
            h = OtherMath.DistanceYZ(piston7, tong);
            theta7 = Mathf.Acos((BD7 * BD7 + CD7 * CD7 - h * h) / (2 * BD7 * CD7));
            psi71 = Mathf.Acos((OC7 * OC7 + h * h - OB7 * OB7) / (2 * OC7 * h));
            psi72 = OtherMath.CalcBeta(CD7, BD7, h, theta7);
            phi7 = OtherMath.CalcBeta(OC7, h, OB7, psi71);
        }

        //Размеры лап
        {
            h = OtherMath.DistanceXYZ(cylinderFR, pistonFR);
            lFR = OtherMath.DistanceXYZ(cylinderFR, footFR);
            dFR = OtherMath.DistanceXYZ(pistonFR, footFR);
            alphaFR = Mathf.Acos((lFR * lFR + dFR * dFR - h * h) / (2 * lFR * dFR));
            betaFR = OtherMath.CalcBeta(lFR, dFR, h, alphaFR);
            gammaFR = PI - alphaFR - betaFR;

            h = OtherMath.DistanceXYZ(cylinderFL, pistonFL);
            lFL = OtherMath.DistanceXYZ(cylinderFL, footFL);
            dFL = OtherMath.DistanceXYZ(pistonFL, footFL);
            alphaFL = Mathf.Acos((lFL * lFL + dFL * dFL - h * h) / (2 * lFL * dFL));
            betaFL = OtherMath.CalcBeta(lFL, dFL, h, alphaFL);
            gammaFL = PI - alphaFL - betaFL;

            h = OtherMath.DistanceXYZ(cylinderBR, pistonBR);
            lBR = OtherMath.DistanceXYZ(cylinderBR, footBR);
            dBR = OtherMath.DistanceXYZ(pistonBR, footBR);
            alphaBR = Mathf.Acos((lBR * lBR + dBR * dBR - h * h) / (2 * lBR * dBR));
            betaBR = OtherMath.CalcBeta(lBR, dBR, h, alphaBR);
            gammaBR = PI - alphaBR - betaBR;

            h = OtherMath.DistanceXYZ(cylinderBL, pistonBL);
            lBL = OtherMath.DistanceXYZ(cylinderBL, footBL);
            dBL = OtherMath.DistanceXYZ(pistonBL, footBL);
            alphaBL = Mathf.Acos((lBL * lBL + dBL * dBL - h * h) / (2 * lBL * dBL));
            betaBL = OtherMath.CalcBeta(lBL, dBL, h, alphaBL);
            gammaBL = PI - alphaBL - betaBL;
        }

        //Размеры для вычисления крена НПУ
        {
            h = OtherMath.DistanceXY(cylinderRoller, pistonRoller);
            lRoll = OtherMath.DistanceXY(roller, cylinderRoller);
            dRoll = OtherMath.DistanceXY(pistonRoller, roller);
            alphaRoll = Mathf.Acos((lRoll * lRoll + dRoll * dRoll - h * h) / (2 * lRoll * dRoll));
            betaRoll = OtherMath.CalcBeta(lRoll, dRoll, h, alphaRoll);
            gammaRoll = PI - alphaRoll - betaRoll;
        }

        //Угол рыскания НПУ
        {
            alphaYaw = 0;
        }
    }

    //Задание ограничений на повороты и перемещения
    void SetHits()
    {
        hitFootMin = (alphaBL + alphaBR + alphaFR + alphaFL) / 4;
        hitFootMax = 93 * Mathf.Deg2Rad;
        hit1Max = 135 * Mathf.Deg2Rad;
        hit1Min = 31 * Mathf.Deg2Rad;
        hit2Max = 121 * Mathf.Deg2Rad;
        hit2Min = 41 * Mathf.Deg2Rad;
        hitFractureMin = 29 * Mathf.Deg2Rad;
        hitFractureMax = 130 * Mathf.Deg2Rad;
        hit5Max = 118 * Mathf.Deg2Rad;
        hit5Min = 19 * Mathf.Deg2Rad;
        hit6Min = l6;
        hit6Max = 1.88f;
        hitPitchMin = 26 * Mathf.Deg2Rad;
        hitPitchMax = 153 * Mathf.Deg2Rad;
        hitRollMin = 25 * Mathf.Deg2Rad;
        hitRollMax = 118 * Mathf.Deg2Rad;
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