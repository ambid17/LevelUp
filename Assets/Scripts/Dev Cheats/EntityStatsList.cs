using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatsList : MonoBehaviour
{
    [SerializeField] private EntityStatsRow entityStatsRowPrefab;
    [SerializeField] private Transform entityStatsRowContainer;
    [SerializeField] private GameObject visuals;
    [SerializeField] private DevCheatsUI devCheatsUI;


    private List<EntityStatsRow> statsRows;

    public void Show()
    {
        visuals.SetActive(true);
        SetupEntityList();
    }

    private void SetupEntityList()
    {
        statsRows = new List<EntityStatsRow>();

        foreach (var entity in FightDataLoader.SerializableEntities)
        {
            var entityStatsRow = Instantiate(entityStatsRowPrefab, entityStatsRowContainer);
            string entityFileName = FightDataLoader.FileNameRemappings[entity.statsFileName];
            entityStatsRow.Setup(entity.name, entityFileName, () => OnRowSelected(entity, entityStatsRow));

            statsRows.Add(entityStatsRow);
        }
    }

    private void OnRowSelected(Entity entity, EntityStatsRow row)
    {
        devCheatsUI.selectedEntity = entity;

        foreach (var statsRow in statsRows)
        {
            statsRow.Deselect();
        }

        row.Select();
    }


    public void Hide()
    {
        visuals.SetActive(false);
        ClearEntityStats();
    }

    private void ClearEntityStats()
    {
        // Ignore the header (first) row
        for (int i = 1; i < entityStatsRowContainer.childCount; i++)
        {
            Destroy(entityStatsRowContainer.GetChild(i).gameObject);
        }
    }
}
