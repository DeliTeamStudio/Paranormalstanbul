using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	public GameObject turn;
	public int selectedCharacter = 0;

	public string newGameSceneName;

	public void NextCharacter()
	{
		turn.transform.Rotate(0, 90, 0);
		selectedCharacter = (selectedCharacter + 1) % characters.Length;
	}

	public void PreviousCharacter()
	{
		turn.transform.Rotate(0, -90, 0);
		selectedCharacter--;
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		SceneManager.LoadScene(newGameSceneName);
	}
}
