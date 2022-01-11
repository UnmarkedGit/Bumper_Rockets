using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    //and color

    [Header("UI")]
    [SerializeField] public float RedInput = 1.00f;
    [SerializeField] public float GreenInput = 1.00f;
    [SerializeField] public float BlueInput = 1.00f;
    [SerializeField] public GameObject ShipPanel;
    [SerializeField] private TMP_InputField nameInputField = null;
    //[SerializeField] private Button continueButton = null;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";
    private const string PlayerPrefsRedKey = "PlayerColorRed";
    private const string PlayerPrefsGreenKey = "PlayerColorGreen";
    private const string PlayerPrefsBlueKey = "PlayerColorBlue";

    public void Update()
    {
        ShipPanel.GetComponent<Image>().color = new Color(RedInput, GreenInput, BlueInput, 1.0f);
    }
    //private void Start() => SetUpInputField();

    public void RedAdjust(float argColor)
    {
        RedInput = argColor/255;
    }

    public void GreenAdjust(float argColor)
    {
        GreenInput = argColor/255;
    }

    public void BlueAdjust(float argColor)
    {
        BlueInput = argColor/255;
    }

    /*private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }*/

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
        PlayerPrefs.SetFloat(PlayerPrefsRedKey, RedInput);
        PlayerPrefs.SetFloat(PlayerPrefsGreenKey, GreenInput);
        PlayerPrefs.SetFloat(PlayerPrefsBlueKey, BlueInput);
    }
}
