using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class ButtonsManager : MonoBehaviour
{
    
    OptionsData optionData;

    public Slider Volume;
    public Toggle fullScreen, enableMusic;

    public TMPro.TMP_Text volumeText;

    public GameObject optionMenu, mainMenu;

    void OnEnable()
    {
        optionData = new OptionsData();
        volumeText.text = ((int) Volume.value * 100).ToString() + "%";
        
        Volume.onValueChanged.AddListener(delegate{OnChangeVolume();});
        fullScreen.onValueChanged.AddListener(delegate{OnFullScreenChange();});
        enableMusic.onValueChanged.AddListener(delegate{OnEnableMusicChange();});
        fullScreen.isOn = Screen.fullScreen;

        LoadSettings();
    }

    
    void LoadSettings()
    {
        optionData = JsonUtility.FromJson<OptionsData>(File.ReadAllText(Application.dataPath + "/GameSettings.json"));
        fullScreen.isOn = optionData.fullScreen;
        enableMusic.isOn = optionData.enableMusic;
        Volume.value = optionData.volume;
    }

    public void StartGame(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(optionData, true);
        File.WriteAllText(Application.dataPath+ "/GameSettings.json", json);
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Discard()
    {
        fullScreen.isOn = optionData.fullScreen;
        enableMusic.isOn = optionData.enableMusic;
        Volume.value = optionData.volume;
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);

    }

    void OnChangeVolume()
    {
        AudioListener.volume = optionData.volume = Volume.value;
        volumeText.text = ((int) (Volume.value * 100)).ToString() + "%";
    }

    void OnFullScreenChange()
    {
        Screen.fullScreen = optionData.fullScreen = fullScreen.isOn;
    }

    void OnEnableMusicChange()
    {
        optionData.enableMusic = enableMusic.isOn;
        if(optionData.enableMusic)
            AudioListener.volume = Volume.value;
        else
        {
            AudioListener.volume = 0;
        }

    }

    
}
