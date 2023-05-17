using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private Vector3 prevPos;
    private BoundsInt prevArea;

    public static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    #region Metody

    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        string tilePath = @"Tiles/";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "WhiteTile96x48"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "GreenTile96x48"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "RedTile96x48"));
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
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
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

    #region Building Placment

    public void InitializeWithBuilding(GameObject building, Vector3 pos)
    {
        pos.z = 0;
        pos.y -= building.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        Vector3 position = gridLayout.CellToLocalInterpolated(cellPos);

        GameObject obj = Instantiate(building, position, Quaternion.identity);
        obj.name = GenerateUUID();
        PlacableObject temp = obj.transform.GetComponent<PlacableObject>();
        temp.gameObject.AddComponent<ObjectDrag>();

        //CanTakeArea(building.transform.GetComponent<PlacableObject>().area);
        //FollowBuilding();
    }
    /*public void FollowBuilding()
    {
        //ClearArea();

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
    }*/
    public void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
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
