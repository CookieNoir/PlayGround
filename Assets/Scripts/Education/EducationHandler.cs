using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EducationHandler : MonoBehaviour
{
    public Text taskDescription;
    public Text instruction;
    public GameObject descriptionObject;
    public RectTransform instructionObject;

    public Camera joinCamera; // Если камера присоединения включена, то окно с подсказками (instructionObject) следует уменьшить

    [Header("UI Эффекты")]
    public MaskableGraphic descriptionMask;
    public MaskableGraphic instructionMask;
    public float effectDuration = 1f;

    [Header("Таймер")]
    public TimerContent timerContent;
    public string timeoutText;

    private Task task = null;
    private IEnumerator maskFlicker;
    private float alpha = 1f;
    private float step;

    private float timeLeft = 0f;
    private bool timerIsUsed = false;

    private IEnumerator MaskFlicker(string newTaskDescription = null, bool instructionsEnabled = false, string newInstruction = null)
    {
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * step;
            SetMaskAlpha(alpha * alpha);
            yield return null;
        }
        alpha = 0;

        SetTextValues(newTaskDescription, instructionsEnabled, newInstruction);
        if (task.isWaitingForCompletion)
        {
            if (task.isSuccesfullyEnded)
                timerContent.SetEndColor();
            else
                timerContent.SetTerminateColor();
            timerIsUsed = false;
        }

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * step;
            SetMaskAlpha(alpha * alpha);
            yield return null;
        }
        alpha = 1f;
        SetMaskAlpha(alpha);
    }

    private void SetMaskAlpha(float alpha)
    {
        Color result = Color.white * alpha;
        descriptionMask.color = result;
        instructionMask.color = result;
    }

    private void ResetEffect()
    {
        StopCoroutine(maskFlicker);
        alpha = 1f;
        SetMaskAlpha(alpha);
    }

    private void Start()
    {
        ChangeWindowsActivity(false);
        maskFlicker = MaskFlicker(null, false, null);
        SetMaskAlpha(1f);
        step = 2f / effectDuration;
    }

    public void SetTextValues(string newTaskDescription, bool instructionsEnabled, string newInstruction)
    {
        taskDescription.text = newTaskDescription;
        if (instructionsEnabled)
        {
            instructionObject.gameObject.SetActive(true);
            instruction.text = newInstruction;
        }
        else
        {
            instructionObject.gameObject.SetActive(false);
        }
    }

    private void SetTimer(float value)
    {
        if (value <= 0)
        {
            timerIsUsed = false;
        }
        else
        {
            timerIsUsed = true;
            timeLeft = value;
            timerContent.UpdateTime(timeLeft);
            timerContent.SetDefaultColor();
        }
        timerContent.gameObject.SetActive(timerIsUsed);
    }

    public void SetTask(Task newTask)
    {
        task = newTask;
        task.Take();
        ChangeWindowsActivity(true);
        SetTextValues(task.GetCurrentDescription(), task.instructionsEnabled, task.GetCurrentInstruction());
        SetTimer(task.timeLimit);
        ResetEffect();
    }

    public void EndTask(bool result)
    {
        if (task)
        {
            task.TurnIn(result);
            task = null;
        }
        ChangeWindowsActivity(false);
    }

    private void ChangeWindowsActivity(bool value)
    {
        descriptionObject.SetActive(value);
        instructionObject.gameObject.SetActive(value);
    }

    private void Update()
    {
        if (task)
        {
            UpdateTime();
            int result = task.CheckStageTask();
            switch (result)
            {
                case 1: // если 1, то промежуточный этап задачи успешно выполнен
                    {
                        StopCoroutine(maskFlicker);
                        maskFlicker = MaskFlicker(task.GetCurrentDescription(), task.instructionsEnabled, task.GetCurrentInstruction());
                        StartCoroutine(maskFlicker);
                        break;
                    }
                case 2: // если 2, то задача успешно выполнена
                    {
                        // установить число очков за задание, равное value в task

                        EndTask(true);
                        break;
                    }
                case -1: // если -1, то достигнут этап, при котором завершить задачу невозможно, требуется перезапуск задания
                    {
                        // предложить пользователю перезапустить задание или вернуться в меню
                        EndTask(false);
                        break;
                    }
                default: break; // если 0, то выполняется промежуточный этап задачи
            }
        }
    }

    private void UpdateTime()
    {
        if (timerIsUsed)
        {
            timeLeft -= Time.deltaTime;
            timerContent.UpdateTime(timeLeft);
            if (timeLeft <= 0 && !task.isWaitingForCompletion)
            {
                task.TerminateTask();
                StopCoroutine(maskFlicker);
                maskFlicker = MaskFlicker(timeoutText, false, null);
                StartCoroutine(maskFlicker);
                timerIsUsed = false;
            }
        }
    }

    public void ResetTask()
    {
        Task currentTask = task;
        EndTask(false);
        SetTask(currentTask);
    }

    public void DropAndSetTask(Task task)
    {
        DropTask();
        if (task) SetTask(task);
    }

    public void DropTask()
    {
        EndTask(false);
    }
}
