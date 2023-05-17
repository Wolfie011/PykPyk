using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 startPos;
    private float deltaX, deltaY;

    void Start()
    {
        startPos = Input.mousePosition;
        startPos = Camera.main.ScreenToWorldPoint(startPos);

        deltaX = startPos.x - transform.position.x;
        deltaY = startPos.y - transform.position.y;
    }


    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY);

        Vector3Int cellPos = GridBuildingSystem.current.gridLayout.WorldToCell(pos);
        transform.position = GridBuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPos);

        PanZoom.current.FollowObject(gameObject.transform);
    }
    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            gameObject.GetComponent<PlacableObject>().CanBePlaced();
            PanZoom.current.UnFollowObject();
            Destroy(this);
        }
    }
}