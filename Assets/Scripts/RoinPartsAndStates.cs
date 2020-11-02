using UnityEngine;

public class RoinPartsAndStates : MonoBehaviour
{
    [Header("Части РОИНа")]
    public Transform _body;
    public Transform _arm0,
                     _arm1,
                     _arm2,
                     _arm3,
                     _arm4,
                     _arm5,
                     _arm6,
                     _link1,
                     _link2,
                     _link3,
                     _link4,
                     _link5,
                     _link6,
                     _cylinder1,
                     _cylinder2,
                     _cylinder3,
                     _cylinder5,
                     _cylinder7,
                     _piston1,
                     _piston2,
                     _piston3,
                     _piston5,
                     _piston7,
                     _fixed3,
                     _fixed5,
                     _fixed7,
                     _clutch5,
                     _clutch7,
                     _tong,

                     _footFR,
                     _cylinderFR,
                     _pistonFR,
                     _footFL,
                     _cylinderFL,
                     _pistonFL,
                     _footBR,
                     _cylinderBR,
                     _pistonBR,
                     _footBL,
                     _cylinderBL,
                     _pistonBL,

                     _pitcher,
                     _cylinderRoller,
                     _roller,
                     _yawer,
                     _grip,
                     _pistonRoller;
    protected Transform body;
    protected Transform arm0,
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

    [Header("Настройки скорости")]
    public float _speed = 2.5f;
    public float _rotationSpeed = 0.5f,
                 _elongationSpeed = 0.5f;
    protected float currentSpeed, defaultSpeed,
                  currentSpeedRotation, defaultSpeedRotation,
                  currentSpeedElongation, defaultSpeedElongation;

    protected const float PI = Mathf.PI;

    protected bool allowRotation;
    protected bool allowMove;

    protected float hitFootMin, //для лап 
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
    protected float alpha0,
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

    private RoinStatesKit defaultState;
    [Header("Наборы состояний")]
    public RoinStatesKit specialState;

    protected void MatchProperties()
    {
        defaultSpeed = _speed * Time.fixedDeltaTime;
        defaultSpeedRotation = _rotationSpeed * Time.fixedDeltaTime;
        defaultSpeedElongation = _elongationSpeed * Time.fixedDeltaTime;

        currentSpeedElongation = defaultSpeedElongation; // Не изменяется

        allowRotation = false;
        allowMove = true;

        body = _body;
        arm0 = _arm0;
        arm1 = _arm1;
        arm2 = _arm2;
        arm3 = _arm3;
        arm4 = _arm4;
        arm5 = _arm5;
        arm6 = _arm6;
        link1 = _link1;
        link2 = _link2;
        link3 = _link3;
        link4 = _link4;
        link5 = _link5;
        link6 = _link6;
        cylinder1 = _cylinder1;
        cylinder2 = _cylinder2;
        cylinder3 = _cylinder3;
        cylinder5 = _cylinder5;
        cylinder7 = _cylinder7;
        piston1 = _piston1;
        piston2 = _piston2;
        piston3 = _piston3;
        piston5 = _piston5;
        piston7 = _piston7;
        fixed3 = _fixed3;
        fixed5 = _fixed5;
        fixed7 = _fixed7;
        clutch5 = _clutch5;
        clutch7 = _clutch7;
        tong = _tong;

        footFR = _footFR;
        cylinderFR = _cylinderFR;
        pistonFR = _pistonFR;
        footFL = _footFL;
        cylinderFL = _cylinderFL;
        pistonFL = _pistonFL;
        footBR = _footBR;
        cylinderBR = _cylinderBR;
        pistonBR = _pistonBR;
        footBL = _footBL;
        cylinderBL = _cylinderBL;
        pistonBL = _pistonBL;

        pitcher = _pitcher;
        cylinderRoller = _cylinderRoller;
        roller = _roller;
        yawer = _yawer;
        grip = _grip;
        pistonRoller = _pistonRoller;
    }

    protected void WriteDefaultState()
    {
        MatchProperties();
        SetSize();
        SetHits();
        defaultState = gameObject.AddComponent(typeof(RoinStatesKit)) as RoinStatesKit;
        WriteValuesToStatesKit(defaultState);
    }

    public void SetState()
    {
        if (specialState) SetValuesFromStatesKit(specialState);
        else SetValuesFromStatesKit(defaultState);
    }

    public bool GetAllowMove()
    {
        return allowMove;
    }

    public bool GetAllowRotation()
    {
        return allowRotation;
    }

    public void WriteValuesToStatesKit(RoinStatesKit kit)
    {
        kit.speed = defaultSpeed;
        kit.speedRotation = defaultSpeedRotation;
        kit.speedElongation = defaultSpeedElongation;

        kit.allowMove = allowMove;
        kit.allowRotation = allowRotation;

        kit.body_position = body.transform.localPosition;
        kit.arm0_position = arm0.transform.localPosition;
        kit.arm1_position = arm1.transform.localPosition;
        kit.arm2_position = arm2.transform.localPosition;
        kit.arm3_position = arm3.transform.localPosition;
        kit.arm4_position = arm4.transform.localPosition;
        kit.arm5_position = arm5.transform.localPosition;
        kit.arm6_position = arm6.transform.localPosition;
        kit.link1_position = link1.transform.localPosition;
        kit.link2_position = link2.transform.localPosition;
        kit.link3_position = link3.transform.localPosition;
        kit.link4_position = link4.transform.localPosition;
        kit.link5_position = link5.transform.localPosition;
        kit.link6_position = link6.transform.localPosition;
        kit.cylinder1_position = cylinder1.transform.localPosition;
        kit.cylinder2_position = cylinder2.transform.localPosition;
        kit.cylinder3_position = cylinder3.transform.localPosition;
        kit.cylinder5_position = cylinder5.transform.localPosition;
        kit.cylinder7_position = cylinder7.transform.localPosition;
        kit.piston1_position = piston1.transform.localPosition;
        kit.piston2_position = piston2.transform.localPosition;
        kit.piston3_position = piston3.transform.localPosition;
        kit.piston5_position = piston5.transform.localPosition;
        kit.piston7_position = piston7.transform.localPosition;
        kit.fixed3_position = fixed3.transform.localPosition;
        kit.fixed5_position = fixed5.transform.localPosition;
        kit.fixed7_position = fixed7.transform.localPosition;
        kit.clutch5_position = clutch5.transform.localPosition;
        kit.clutch7_position = clutch7.transform.localPosition;
        kit.tong_position = tong.transform.localPosition;

        kit.footFR_position = footFR.transform.localPosition;
        kit.cylinderFR_position = cylinderFR.transform.localPosition;
        kit.pistonFR_position = pistonFR.transform.localPosition;
        kit.footFL_position = footFL.transform.localPosition;
        kit.cylinderFL_position = cylinderFL.transform.localPosition;
        kit.pistonFL_position = pistonFL.transform.localPosition;
        kit.footBR_position = footBR.transform.localPosition;
        kit.cylinderBR_position = cylinderBR.transform.localPosition;
        kit.pistonBR_position = pistonBR.transform.localPosition;
        kit.footBL_position = footBL.transform.localPosition;
        kit.cylinderBL_position = cylinderBL.transform.localPosition;
        kit.pistonBL_position = pistonBL.transform.localPosition;

        kit.pitcher_position = pitcher.transform.localPosition;
        kit.cylinderRoller_position = cylinderRoller.transform.localPosition;
        kit.roller_position = roller.transform.localPosition;
        kit.yawer_position = yawer.transform.localPosition;
        kit.grip_position = grip.transform.localPosition;
        kit.pistonRoller_position = pistonRoller.transform.localPosition;

        // Rotation
        kit.body_rotation = body.transform.localRotation;
        kit.arm0_rotation = arm0.transform.localRotation;
        kit.arm1_rotation = arm1.transform.localRotation;
        kit.arm2_rotation = arm2.transform.localRotation;
        kit.arm3_rotation = arm3.transform.localRotation;
        kit.arm4_rotation = arm4.transform.localRotation;
        kit.arm5_rotation = arm5.transform.localRotation;
        kit.arm6_rotation = arm6.transform.localRotation;
        kit.link1_rotation = link1.transform.localRotation;
        kit.link2_rotation = link2.transform.localRotation;
        kit.link3_rotation = link3.transform.localRotation;
        kit.link4_rotation = link4.transform.localRotation;
        kit.link5_rotation = link5.transform.localRotation;
        kit.link6_rotation = link6.transform.localRotation;
        kit.cylinder1_rotation = cylinder1.transform.localRotation;
        kit.cylinder2_rotation = cylinder2.transform.localRotation;
        kit.cylinder3_rotation = cylinder3.transform.localRotation;
        kit.cylinder5_rotation = cylinder5.transform.localRotation;
        kit.cylinder7_rotation = cylinder7.transform.localRotation;
        kit.piston1_rotation = piston1.transform.localRotation;
        kit.piston2_rotation = piston2.transform.localRotation;
        kit.piston3_rotation = piston3.transform.localRotation;
        kit.piston5_rotation = piston5.transform.localRotation;
        kit.piston7_rotation = piston7.transform.localRotation;
        kit.fixed3_rotation = fixed3.transform.localRotation;
        kit.fixed5_rotation = fixed5.transform.localRotation;
        kit.fixed7_rotation = fixed7.transform.localRotation;
        kit.clutch5_rotation = clutch5.transform.localRotation;
        kit.clutch7_rotation = clutch7.transform.localRotation;
        kit.tong_rotation = tong.transform.localRotation;

        kit.footFR_rotation = footFR.transform.localRotation;
        kit.cylinderFR_rotation = cylinderFR.transform.localRotation;
        kit.pistonFR_rotation = pistonFR.transform.localRotation;
        kit.footFL_rotation = footFL.transform.localRotation;
        kit.cylinderFL_rotation = cylinderFL.transform.localRotation;
        kit.pistonFL_rotation = pistonFL.transform.localRotation;
        kit.footBR_rotation = footBR.transform.localRotation;
        kit.cylinderBR_rotation = cylinderBR.transform.localRotation;
        kit.pistonBR_rotation = pistonBR.transform.localRotation;
        kit.footBL_rotation = footBL.transform.localRotation;
        kit.cylinderBL_rotation = cylinderBL.transform.localRotation;
        kit.pistonBL_rotation = pistonBL.transform.localRotation;

        kit.pitcher_rotation = pitcher.transform.localRotation;
        kit.cylinderRoller_rotation = cylinderRoller.transform.localRotation;
        kit.roller_rotation = roller.transform.localRotation;
        kit.yawer_rotation = yawer.transform.localRotation;
        kit.grip_rotation = grip.transform.localRotation;
        kit.pistonRoller_rotation = pistonRoller.transform.localRotation;

        kit.alpha0 = alpha0;
        kit.alpha1 = alpha1;
        kit.beta1 = beta1;
        kit.gamma1 = gamma1;
        kit.d1 = d1;
        kit.l1 = l1;
        kit.alpha2 = alpha2;
        kit.beta2 = beta2;
        kit.gamma2 = gamma2;
        kit.d2 = d2;
        kit.l2 = l2;
        kit.alpha3 = alpha3;
        kit.beta3 = beta3;
        kit.gamma3 = gamma3;
        kit.d3 = d3;
        kit.l3 = l3;
        kit.alpha5 = alpha5;
        kit.beta5 = beta5;
        kit.gamma5 = gamma5;
        kit.phi5 = phi5;
        kit.psi51 = psi51;
        kit.psi52 = psi52;
        kit.theta5 = theta5;
        kit.l6 = l6;
        kit.alphaPitch = alphaPitch;
        kit.beta7 = beta7;
        kit.gamma7 = gamma7;
        kit.phi7 = phi7;
        kit.psi71 = psi71;
        kit.psi72 = psi72;
        kit.theta7 = theta7;
        kit.alphaFR = alphaFR;
        kit.betaFR = betaFR;
        kit.gammaFR = gammaFR;
        kit.dFR = dFR;
        kit.lFR = lFR;
        kit.alphaFL = alphaFL;
        kit.betaFL = betaFL;
        kit.gammaFL = gammaFL;
        kit.dFL = dFL;
        kit.lFL = lFL;
        kit.alphaBR = alphaBR;
        kit.betaBR = betaBR;
        kit.gammaBR = gammaBR;
        kit.dBR = dBR;
        kit.lBR = lBR;
        kit.alphaBL = alphaBL;
        kit.betaBL = betaBL;
        kit.gammaBL = gammaBL;
        kit.dBL = dBL;
        kit.lBL = lBL;
        kit.alphaRoll = alphaRoll;
        kit.betaRoll = betaRoll;
        kit.gammaRoll = gammaRoll;
        kit.lRoll = lRoll;
        kit.dRoll = dRoll;
        kit.alphaYaw = alphaYaw;
    }

    public void SetValuesFromStatesKit(RoinStatesKit kit)
    {
        defaultSpeed = kit.speed;
        defaultSpeedRotation = kit.speedRotation;
        defaultSpeedElongation = kit.speedElongation;

        currentSpeedElongation = defaultSpeedElongation;

        allowMove = kit.allowMove;
        allowRotation = kit.allowRotation;

        body.transform.localPosition = kit.body_position;
        arm0.transform.localPosition = kit.arm0_position;
        arm1.transform.localPosition = kit.arm1_position;
        arm2.transform.localPosition = kit.arm2_position;
        arm3.transform.localPosition = kit.arm3_position;
        arm4.transform.localPosition = kit.arm4_position;
        arm5.transform.localPosition = kit.arm5_position;
        arm6.transform.localPosition = kit.arm6_position;
        link1.transform.localPosition = kit.link1_position;
        link2.transform.localPosition = kit.link2_position;
        link3.transform.localPosition = kit.link3_position;
        link4.transform.localPosition = kit.link4_position;
        link5.transform.localPosition = kit.link5_position;
        link6.transform.localPosition = kit.link6_position;
        cylinder1.transform.localPosition = kit.cylinder1_position;
        cylinder2.transform.localPosition = kit.cylinder2_position;
        cylinder3.transform.localPosition = kit.cylinder3_position;
        cylinder5.transform.localPosition = kit.cylinder5_position;
        cylinder7.transform.localPosition = kit.cylinder7_position;
        piston1.transform.localPosition = kit.piston1_position;
        piston2.transform.localPosition = kit.piston2_position;
        piston3.transform.localPosition = kit.piston3_position;
        piston5.transform.localPosition = kit.piston5_position;
        piston7.transform.localPosition = kit.piston7_position;
        fixed3.transform.localPosition = kit.fixed3_position;
        fixed5.transform.localPosition = kit.fixed5_position;
        fixed7.transform.localPosition = kit.fixed7_position;
        clutch5.transform.localPosition = kit.clutch5_position;
        clutch7.transform.localPosition = kit.clutch7_position;
        tong.transform.localPosition = kit.tong_position;

        footFR.transform.localPosition = kit.footFR_position;
        cylinderFR.transform.localPosition = kit.cylinderFR_position;
        pistonFR.transform.localPosition = kit.pistonFR_position;
        footFL.transform.localPosition = kit.footFL_position;
        cylinderFL.transform.localPosition = kit.cylinderFL_position;
        pistonFL.transform.localPosition = kit.pistonFL_position;
        footBR.transform.localPosition = kit.footBR_position;
        cylinderBR.transform.localPosition = kit.cylinderBR_position;
        pistonBR.transform.localPosition = kit.pistonBR_position;
        footBL.transform.localPosition = kit.footBL_position;
        cylinderBL.transform.localPosition = kit.cylinderBL_position;
        pistonBL.transform.localPosition = kit.pistonBL_position;

        pitcher.transform.localPosition = kit.pitcher_position;
        cylinderRoller.transform.localPosition = kit.cylinderRoller_position;
        roller.transform.localPosition = kit.roller_position;
        yawer.transform.localPosition = kit.yawer_position;
        grip.transform.localPosition = kit.grip_position;
        pistonRoller.transform.localPosition = kit.pistonRoller_position;

        // Rotation
        body.transform.localRotation = kit.body_rotation;
        arm0.transform.localRotation = kit.arm0_rotation;
        arm1.transform.localRotation = kit.arm1_rotation;
        arm2.transform.localRotation = kit.arm2_rotation;
        arm3.transform.localRotation = kit.arm3_rotation;
        arm4.transform.localRotation = kit.arm4_rotation;
        arm5.transform.localRotation = kit.arm5_rotation;
        arm6.transform.localRotation = kit.arm6_rotation;
        link1.transform.localRotation = kit.link1_rotation;
        link2.transform.localRotation = kit.link2_rotation;
        link3.transform.localRotation = kit.link3_rotation;
        link4.transform.localRotation = kit.link4_rotation;
        link5.transform.localRotation = kit.link5_rotation;
        link6.transform.localRotation = kit.link6_rotation;
        cylinder1.transform.localRotation = kit.cylinder1_rotation;
        cylinder2.transform.localRotation = kit.cylinder2_rotation;
        cylinder3.transform.localRotation = kit.cylinder3_rotation;
        cylinder5.transform.localRotation = kit.cylinder5_rotation;
        cylinder7.transform.localRotation = kit.cylinder7_rotation;
        piston1.transform.localRotation = kit.piston1_rotation;
        piston2.transform.localRotation = kit.piston2_rotation;
        piston3.transform.localRotation = kit.piston3_rotation;
        piston5.transform.localRotation = kit.piston5_rotation;
        piston7.transform.localRotation = kit.piston7_rotation;
        fixed3.transform.localRotation = kit.fixed3_rotation;
        fixed5.transform.localRotation = kit.fixed5_rotation;
        fixed7.transform.localRotation = kit.fixed7_rotation;
        clutch5.transform.localRotation = kit.clutch5_rotation;
        clutch7.transform.localRotation = kit.clutch7_rotation;
        tong.transform.localRotation = kit.tong_rotation;

        footFR.transform.localRotation = kit.footFR_rotation;
        cylinderFR.transform.localRotation = kit.cylinderFR_rotation;
        pistonFR.transform.localRotation = kit.pistonFR_rotation;
        footFL.transform.localRotation = kit.footFL_rotation;
        cylinderFL.transform.localRotation = kit.cylinderFL_rotation;
        pistonFL.transform.localRotation = kit.pistonFL_rotation;
        footBR.transform.localRotation = kit.footBR_rotation;
        cylinderBR.transform.localRotation = kit.cylinderBR_rotation;
        pistonBR.transform.localRotation = kit.pistonBR_rotation;
        footBL.transform.localRotation = kit.footBL_rotation;
        cylinderBL.transform.localRotation = kit.cylinderBL_rotation;
        pistonBL.transform.localRotation = kit.pistonBL_rotation;

        pitcher.transform.localRotation = kit.pitcher_rotation;
        cylinderRoller.transform.localRotation = kit.cylinderRoller_rotation;
        roller.transform.localRotation = kit.roller_rotation;
        yawer.transform.localRotation = kit.yawer_rotation;
        grip.transform.localRotation = kit.grip_rotation;
        pistonRoller.transform.localRotation = kit.pistonRoller_rotation;

        alpha0 = kit.alpha0;
        alpha1 = kit.alpha1;
        beta1 = kit.beta1;
        gamma1 = kit.gamma1;
        d1 = kit.d1;
        l1 = kit.l1;
        alpha2 = kit.alpha2;
        beta2 = kit.beta2;
        gamma2 = kit.gamma2;
        d2 = kit.d2;
        l2 = kit.l2;
        alpha3 = kit.alpha3;
        beta3 = kit.beta3;
        gamma3 = kit.gamma3;
        d3 = kit.d3;
        l3 = kit.l3;
        alpha5 = kit.alpha5;
        beta5 = kit.beta5;
        gamma5 = kit.gamma5;
        phi5 = kit.phi5;
        psi51 = kit.psi51;
        psi52 = kit.psi52;
        theta5 = kit.theta5;
        l6 = kit.l6;
        alphaPitch = kit.alphaPitch;
        beta7 = kit.beta7;
        gamma7 = kit.gamma7;
        phi7 = kit.phi7;
        psi71 = kit.psi71;
        psi72 = kit.psi72;
        theta7 = kit.theta7;
        alphaFR = kit.alphaFR;
        betaFR = kit.betaFR;
        gammaFR = kit.gammaFR;
        dFR = kit.dFR;
        lFR = kit.lFR;
        alphaFL = kit.alphaFL;
        betaFL = kit.betaFL;
        gammaFL = kit.gammaFL;
        dFL = kit.dFL;
        lFL = kit.lFL;
        alphaBR = kit.alphaBR;
        betaBR = kit.betaBR;
        gammaBR = kit.gammaBR;
        dBR = kit.dBR;
        lBR = kit.lBR;
        alphaBL = kit.alphaBL;
        betaBL = kit.betaBL;
        gammaBL = kit.gammaBL;
        dBL = kit.dBL;
        lBL = kit.lBL;
        alphaRoll = kit.alphaRoll;
        betaRoll = kit.betaRoll;
        gammaRoll = kit.gammaRoll;
        lRoll = kit.lRoll;
        dRoll = kit.dRoll;
        alphaYaw = kit.alphaYaw;
    }

    //Задание размеров
    private void SetSize()
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
            h = OtherMath.DistanceLocalYZ(cylinder3.localPosition, piston3.localPosition);
            l3 = OtherMath.DistanceLocalYZ(cylinder3.localPosition, Vector3.zero);
            d3 = OtherMath.DistanceLocalYZ(piston3.localPosition, Vector3.zero);
            alpha3 = Mathf.Acos((l3 * l3 + d3 * d3 - h * h) / (2 * l3 * d3));
            beta3 = OtherMath.CalcBeta(l3, d3, h, alpha3);
            gamma3 = PI - alpha3 - beta3;
            cylinder3.transform.SetParent(arm2.transform);
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
    private void SetHits()
    {
        hitFootMin = (alphaBL + alphaBR + alphaFR + alphaFL) / 4f;
        //hitFootMin = 6 * Mathf.Deg2Rad;
        hitFootMax = 93f * Mathf.Deg2Rad;
        hit1Max = 135f * Mathf.Deg2Rad;
        hit1Min = 31f * Mathf.Deg2Rad;
        hit2Max = 121f * Mathf.Deg2Rad;
        hit2Min = 41f * Mathf.Deg2Rad;
        hitFractureMin = 29f * Mathf.Deg2Rad;
        hitFractureMax = 130f * Mathf.Deg2Rad;
        hit5Max = 118f * Mathf.Deg2Rad;
        hit5Min = 19f * Mathf.Deg2Rad;
        hit6Min = l6;
        //hit6Min = 1.419602f;
        hit6Max = 1.88f;
        hitPitchMin = 26f * Mathf.Deg2Rad;
        hitPitchMax = 153f * Mathf.Deg2Rad;
        hitRollMin = 25f * Mathf.Deg2Rad;
        hitRollMax = 118f * Mathf.Deg2Rad;
    }
}
