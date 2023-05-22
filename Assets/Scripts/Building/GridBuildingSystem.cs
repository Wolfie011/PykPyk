using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    public Canvas UI;
    public Behaviour panelEdit;
    public Behaviour buildingPanel;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    public static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    public Building temp;
    public Vector3 prevPos;
    public BoundsInt prevArea;

    #region Unity Methods

    private void Awake()
    {
        current = this;
        panelEdit.enabled = false;
        buildingPanel.enabled = false;
    }
    private void Start()
    {
        string tilePath = @"Tiles/";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "WhiteTile96x48"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "Green96x48"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "Red96x48"));
    }

    private void Update()
    {
        if (!temp)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                if (!temp.Placed)
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
                    buildingPanel.enabled = false;
                    if (prevPos != cellPos)
                    {
                        temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
                        prevPos = cellPos;
                        FollowBuilding();
                        
                    }
                }
            }
        }
    }
    public void PlacePref()
    {
        if (temp.CanBePlaced())
        {
            temp.Place();
            panelEdit.enabled = false;
            temp = null;
        }
    }
    public void DestroyPref()
    {
        ClearArea();
        Destroy(temp.gameObject);
        panelEdit.enabled = false;
        temp = null;
    }

    public void HideBuildingPanel()
    {
        buildingPanel.enabled = false;
        temp = null;
    }

    public void SetPlacedFalse()
    {
        temp.Placed = false;
        ClearMoved(temp.area);
        buildingPanel.enabled = false;
        panelEdit.enabled = true;
    }

    public static string GenerateUUID()
    {
        Guid guid = Guid.NewGuid();
        string uuid = guid.ToString();
        uuid = uuid.Replace("-", "").ToLower();
        return uuid;
    }
    #endregion

    #region Tilemap Managment

    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    #endregion

    #region Building Placement

    public void InitializeWithBuilding(GameObject building, Vector3 pos)
    {
        pos.z = 0;
        pos.y -= building.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        Vector3 position = gridLayout.CellToLocalInterpolated(cellPos);

        temp = Instantiate(building, position, Quaternion.identity).GetComponent<Building>();
        temp.GetComponent<Building>().CanBePlaced();
        temp.name = GenerateUUID();
        temp.gameObject.AddComponent<ObjectDrag>();

        FollowBuilding();
        panelEdit.enabled = true;
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }
    public void ClearMoved(BoundsInt area)
    {
        TileBase[] toClear = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(toClear, TileType.White);
        MainTilemap.SetTilesBlock(area, toClear);
    }
    public void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for(int i = 0; i < baseArray.Length; i++)
        {
            if(baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }
        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach(var b in baseArray)
        {
            if(b != tileBases[TileType.White])
            {
                Debug.Log("Cannot place here");
                return false;
            }
        }
        return true;
    }
    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
        temp = null;
    }
    #endregion
}
public enum TileType
{
    Empty,
    White,
    Green,
    Red
}
