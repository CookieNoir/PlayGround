using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class CreateListLevel : MonoBehaviour
{
    public Task[] Tasks;
    public EducationHandler educationHandler;
    private List<ButtonInfo> _buttons = new List<ButtonInfo>();

    [Header("Для генерации кнопок")]
    public GameObject Button;
    public Transform ButtonParent;

    //isEducationMode
    //true - обычный режим обучения
    //false - режим доп. тренировок
    public void SetTasks(Task[] task, TaskMode mode)
    {
        Tasks = task;
        GenerateLevels(mode);
        Debug.Log(Tasks.Length);
    }

    private void OnEnable()
    {
        //при каждом открытии, обновляем очки уровней
        UpdateLevels();
    }

    public void GenerateLevels(TaskMode mode)
    {
        RemoveList();
        int levelCount = Tasks.Length;
        for (int i = 0; i < levelCount; i++)
        {
            if (Tasks[i])
            {
                ButtonInfo button = Instantiate(Button, ButtonParent).GetComponent<ButtonInfo>();
                _buttons.Add(button);
                Task task = Tasks[i];
                button.SetTask(task, mode, i + 1);
                button.SetEvent(() => educationHandler.DropAndSetTask(task));
            }
        }
    }

    public void RemoveList()
    {
        foreach (var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        _buttons.Clear();
    }

    public void UpdateLevels()
    {
        foreach (var button in _buttons)
        {
            if (Tasks.Contains(button.GetTask()))
            {
                button.UpdateInfo();
            }
        }
    }
}
