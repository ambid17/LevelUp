using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class DevCheatsUI : UIPanel
    {
        // Create an entity stats
        // save it with a file name
        // then import the stats and choose which entity's stats should be overriden
        [SerializeField] private Button editEntityStatsButton;
        [SerializeField] private Button duplicateEntityStatsButton;
        [SerializeField] private Button changeEntityStatsButton;
        [SerializeField] private EntityStatsRow entityStatsRowPrefab;
        [SerializeField] private Transform entityStatsRowContainer;
        [SerializeField] private SerializableDictionary<GameObject, string> entityFilePathMapping;
        [SerializeField] private GameObject entityStatsListContainer;


        [SerializeField] private GameObject entityStatsEditorContainer;
        [SerializeField] private EntityStatsEditor entityStatsEditor;

        [SerializeField] private FileSelector fileSelector;
        [SerializeField] private GameObject fileSelectorContainer;

        private List<EntityStatsRow> statsRows;
        private string currentEntityfileName;
        private EntityStats currentEntityStats;

        private void Start()
        {
            editEntityStatsButton.onClick.AddListener(OnEdit);
            editEntityStatsButton.onClick.AddListener(OnDuplicate);
            editEntityStatsButton.onClick.AddListener(OnChange);

        }

        private void OnEnable()
        {
            SetupEntityList();
        }

        private void SetupEntityList()
        {
            ClearEntityStats();

            statsRows = new List<EntityStatsRow>();

            foreach (var entity in entityFilePathMapping.Keys)
            {
                var entityStatsRow = Instantiate(entityStatsRowPrefab, entityStatsRowContainer);
                string entityFileName = entityFilePathMapping[entity];
                entityStatsRow.Setup(entity.name, entityFileName, () => OnRowSelected(entityFileName));

                statsRows.Add(entityStatsRow);
            }
        }

        private void ClearEntityStats()
        {
            // Ignore the header (first) row
            for (int i = 1; i < entityStatsRowContainer.childCount; i++)
            {
                Destroy(entityStatsRowContainer.GetChild(i).gameObject);
            }
        }

        private void OnRowSelected(string entityFileName)
        {
            currentEntityfileName = entityFileName;
        }

        private void OnEdit()
        {
            entityStatsListContainer.SetActive(false);
            entityStatsEditorContainer.SetActive(true);
            entityStatsEditor.Setup(currentEntityfileName, EntityStatsEditor.EditorMode.Edit);
            // show editor with the currently selected file
        }

        private void OnDuplicate()
        {
            entityStatsListContainer.SetActive(false);
            entityStatsEditorContainer.SetActive(true);
            entityStatsEditor.Setup(currentEntityfileName, EntityStatsEditor.EditorMode.Duplicate);
            // show editor with a copy of the currently selected file
        }

        private void OnChange()
        {
            fileSelectorContainer.SetActive(true);
            // show popup to select a entityStats to use
        }

        /// <summary>
        /// In order for this to work, you have to set the "Asset Label" of each prefab to "Entity"
        /// </summary>
        [ContextMenu("Populate entity mappings")]
        public void PopulateEntityMapping()
        {
            string[] entityGuids = AssetDatabase.FindAssets("l:Entity");
            entityFilePathMapping = new();

            foreach (string guid in entityGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject entity = (GameObject) AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));

                entityFilePathMapping.Add(entity, $"{entity.name}.json");
            }

            EditorUtility.SetDirty(this);
        }

        [ContextMenu("Create EntityStats Files")]
        public void CreateEntityStatsFiles()
        {
            var dataFolder = Application.persistentDataPath;
            var entitiesFolder = Path.Combine(dataFolder, "Entities");

            if (!Directory.Exists(entitiesFolder))
            {
                Directory.CreateDirectory(entitiesFolder);
            }

            foreach (var entity in entityFilePathMapping.Keys)
            {
                var entityFilePath = Path.Combine(entitiesFolder, entityFilePathMapping[entity]);
                var entityStats = entity.GetComponent<Entity>().Stats;
                FileUtils.SaveFile<EntityStats>(entityFilePath, entityStats);
            }
        }
    }
}
