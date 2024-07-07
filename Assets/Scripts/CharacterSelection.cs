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
		//Debug.Log("1");
		selectedCharacter = (selectedCharacter + 1) % characters.Length;
		characters[selectedCharacter].SetActive(true);
		//Debug.Log("2");
	}

	public void PreviousCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter--;
		if (selectedCharacter < 0)
		{
			selectedCharacter += characters.Length;
		}
		characters[selectedCharacter].SetActive(true);
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		//SceneManager.LoadScene(2, LoadSceneMode.Single);
		SceneManager.LoadScene(newGameSceneName);
	}
}
