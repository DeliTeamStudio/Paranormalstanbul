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
		//characters[selectedCharacter].SetActive(false);
		turn.transform.Rotate(0, 90, 0);
		selectedCharacter = (selectedCharacter + 1) % characters.Length;

		//characters[selectedCharacter].SetActive(true);
	}

	public void PreviousCharacter()
	{
		//characters[selectedCharacter].SetActive(false);
		turn.transform.Rotate(0, -90, 0);
		selectedCharacter--;
		//if (selectedCharacter < 0)
		//{
		//selectedCharacter += characters.Length;
		//}
		//characters[selectedCharacter].SetActive(true);
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		//SceneManager.LoadScene(2, LoadSceneMode.Single);
		SceneManager.LoadScene(newGameSceneName);
	}
}
