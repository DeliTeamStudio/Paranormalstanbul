using Esper.ESave.SavableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Esper.ESave.Example
{
    public class SaveLoad : MonoBehaviour
    {
        private const string objectPositionDataKey = "ObjectPosition";

        [SerializeField]
        private Transform playerData;
        private SaveFileSetup saveFileSetup;
        private SaveFile saveFile;

        private void Start()
        {
            // Get save file
            saveFileSetup = GetComponent<SaveFileSetup>();
            saveFile = saveFileSetup.GetSaveFile();

            Debug.Log("Start game.");

            SaveGame();

        }

        public void Update()
        {

            if (Input.GetKey(KeyCode.B))
            {
                LoadGame();
            }
        }


        public void LoadGame()
        {


            // Get Vector3 from a special method because Vector3 is not savable data
            var savableTransform = saveFile.GetTransform(objectPositionDataKey);
            playerData.transform.CopyTransformValues(savableTransform);


            Debug.Log("Loaded game.");
        }


        public void SaveGame()
        {

            saveFile.AddOrUpdateData(objectPositionDataKey, playerData);
            saveFile.Save();

            Debug.Log("Saved game.");
        }
    }
}