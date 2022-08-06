using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

public class MenuPanel : Panel
{
    [SerializeField]
    private Button exitBtn;

    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private TMP_Dropdown resolutionDropdown;

    private bool IsOpen => cvs.interactable;

    private int resolutionNum = 0;
    FullScreenMode screenMode = FullScreenMode.Windowed;

    private List<Resolution> resolutionList = new List<Resolution>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        MenuPanel[] panels = FindObjectsOfType<MenuPanel>();

        if(panels.Length > 1)
        {
            for (int i = 1; i < panels.Length; i++)
            {
                Destroy(panels[i].transform.parent.gameObject);
            }
            return;
        }
        

        base.Start();
        InitUI();
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }
    }

    private void OpenMenu()
    {
        if(IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            int width = Screen.resolutions[i].width;
            int height = Screen.resolutions[i].height;
            if (Screen.resolutions[i].refreshRate.Equals(60) && ((float)width / height) >= 1.7f)
            {
                resolutionList.Add(Screen.resolutions[i]);
            }
        }

        resolutionList.Sort((x, y) => y.width.CompareTo(x.width));

        resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutionList)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            string s = "Windowed";

            if(optionNum == 0)
            {
                s = "Full Screen";
            }

            option.text = $"{item.width} x {item.height} ({s})";
            resolutionDropdown.options.Add(option);

            if(item.width.Equals(Screen.width) && item.height.Equals(Screen.height))
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }

        exitBtn.onClick.AddListener(() => Application.Quit());

        sfxSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.Instance.ChangeSfxSound(value);
        });

        bgmSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.Instance.ChangeBgmSound(value);
        });

        resolutionDropdown.onValueChanged.AddListener(ChangeDropDownValue);

        resolutionDropdown.RefreshShownValue();
    }

    public void ChangeDropDownValue(int value)
    {
        resolutionNum = value;
        screenMode = resolutionDropdown.options[resolutionNum].text.Contains("Full Screen") ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;

        int width = resolutionList[resolutionNum].width;
        int height = resolutionList[resolutionNum].height;

        Screen.SetResolution(width, height, screenMode);
    }
}
