using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
	public InputField mainInputField;

	// Checks if there is anything entered into the input field.
	void LockInput(InputField input)
	{
		if (input.text.Length > 0)
		{
			Debug.Log("Text has been entered");

			HighScoreManager.instance.currentPlayerName = input.text;
		}
		else if (input.text.Length == 0)
		{
			Debug.Log("Main Input Empty");
		}
	}

	public void Start()
	{
		//Adds a listener that invokes the "LockInput" method when the player finishes editing the main input field.
		//Passes the main input field into the method when "LockInput" is invoked
		mainInputField.onEndEdit.AddListener(delegate { LockInput(mainInputField); });
	}

	public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
		HighScoreManager.instance.SaveHighScores();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit game
#endif
    }
}
