using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static string playerName;
    public TMP_InputField inputField;

    public void StartGame()
    {
        playerName =  inputField.text;
        SceneManager.LoadScene("main");
    }

}
