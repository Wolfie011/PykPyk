using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Building current;
    public bool Placed; //{ get; private set; }
    public BoundsInt area;

    private void Awake()
    {
        current = this;
    }

    #region Build Methods
    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        return GridBuildingSystem.current.CanTakeArea(areaTemp);
    }
    public virtual void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.current.TakeArea(areaTemp);
    }
    #endregion

    private void OnMouseDown()
    {
        /*if(GridBuildingSystem.current.temp == null)
        {
            Placed = false;
            GridBuildingSystem.current.panelEdit.enabled = true;
            GridBuildingSystem.current.ClearMoved(current.area);
            GridBuildingSystem.current.temp = current;
        }*/
        if(GridBuildingSystem.current.temp == null)
        {
            GridBuildingSystem.current.buildingPanel.enabled = true;
            GridBuildingSystem.current.temp = current;
        }

    }
}
