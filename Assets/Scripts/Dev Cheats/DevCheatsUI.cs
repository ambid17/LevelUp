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
        enum DevCheatsState
        {
            ListStats,
            EditStats,
            ChangeFile,
            DuplicateFile,
        }

        // Create an entity stats
        // save it with a file name
        // then import the stats and choose which entity's stats should be overriden
        [SerializeField] private Button editEntityStatsButton;
        [SerializeField] private Button duplicateEntityStatsButton;
        [SerializeField] private Button changeEntityStatsButton;
        [SerializeField] private GameObject entityStatsListContainer;


        [SerializeField] private EntityStatsList entityStatsList;
        [SerializeField] private EntityStatsEditor entityStatsEditor;
        [SerializeField] private FileSelector fileSelector;
        [SerializeField] private DuplicateStatsPopup duplicateStatsPopup;

        public Entity selectedEntity;
        private DevCheatsState state;


        private void Start()
        {
            editEntityStatsButton.onClick.AddListener(OnEdit);
            duplicateEntityStatsButton.onClick.AddListener(OnDuplicate);
            changeEntityStatsButton.onClick.AddListener(OnChange);

            entityStatsList.Show();
        }

        public void OnFinishOperation()
        {
            entityStatsList.Show();
        }

        /// <summary>
        /// show editor with the currently selected file
        /// </summary>
        private void OnEdit()
        {
            entityStatsList.Hide();
            entityStatsEditor.Setup(selectedEntity);
        }

        /// <summary>
        /// show popup asking to choose file name
        /// </summary>
        private void OnDuplicate()
        {
            entityStatsList.Hide();
            duplicateStatsPopup.Show(selectedEntity);
        }

        /// <summary>
        /// show popup to select a entityStats to use
        /// </summary>
        private void OnChange()
        {
            entityStatsList.Hide();
            fileSelector.SelectStatsForEntity(selectedEntity);
        }
    }
}
