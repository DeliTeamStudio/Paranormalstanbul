using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "New Character", order = 1)]

public class NewCharacter : ScriptableObject
{
    public Sprite characterPhoto;
    public string characterName;
    public GameObject characterPrefab;
}
