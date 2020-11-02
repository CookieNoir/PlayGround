using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuManager : MonoBehaviour
{
    public PauseMenuManager PauseMenuManager;
    public GameObject MainMenu;
    public AllTasks AllTasks;
    public EducationHandler educationHandler;
    private CreateListLevel _createList;
    private bool _isActive;

    private void Awake()
    {
        if (PauseMenuManager == null)
            return;
        _createList = PauseMenuManager.SelectLevelMenuMenuView.GetComponent<CreateListLevel>();
    }

    private void OnEnable()
    {
        Open();
    }

    public void Open()
    {
        Time.timeScale = 0;
        _isActive = true;
        MainMenu.SetActive(true);
    }

    public void Close()
    {
        Time.timeScale = 1;
        _isActive = false;
        MainMenu.SetActive(false);
    }

    public bool IsActive()
    {
        return _isActive;
    }

    public void ResumeEducation()
    {
        _createList.SetTasks(AllTasks.Tasks, TaskMode.Education);
        //PauseMenuManager.Pause();
        Close();
    }

    public void NewEducation()
    {
        AllTasks.ResetTask();
        educationHandler.DropTask();
        _createList.SetTasks(AllTasks.Tasks, TaskMode.Education);
        //PauseMenuManager.Pause();
        Close();
    }

    public void ResearchesMode()
    {
        Task[] task = { AllTasks.ResearchTask };
        _createList.SetTasks(task, TaskMode.Education);
        Close();
    }

    public void TrainingMode()
    {
        _createList.SetTasks(AllTasks.RandomTasks, TaskMode.Training);
        PauseMenuManager.Pause();
        Close();
    }
}