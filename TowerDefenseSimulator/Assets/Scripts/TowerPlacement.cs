using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerPlacement : MonoBehaviour
{
    public LayerMask tileLayerMask;
    public float towerPlaceYOffset = 0.1f;
    public PreviewTower previewTower;

    private TowerData towerToPlaceDown;
    private bool placingTower;
    private TowerTile curSelectedTile;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    public void SelectTowerToPlace(TowerData tower)
    {
        towerToPlaceDown = tower;
        placingTower = true;

        previewTower.gameObject.SetActive(true);
        previewTower.SetTower(tower);
    }

    void Update()
    {
        if (placingTower)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // shoot a raycast from our mouse cursor
            if (Physics.Raycast(ray, out hit, 99, tileLayerMask))
            {
                curSelectedTile = hit.collider.GetComponent<TowerTile>();
                previewTower.transform.position = curSelectedTile.transform.position + new Vector3(0, towerPlaceYOffset, 0);
            }
            else
            {
                curSelectedTile = null;
                previewTower.transform.position = new Vector3(0, 999, 0);
            }

            // placing down the tower
            if (Mouse.current.leftButton.isPressed && curSelectedTile != null && curSelectedTile.tower == null)
            {
                PlaceTower();
            }

            // cancelling the tower placement
            if (Mouse.current.rightButton.isPressed)
            {
                CancelPlacement();
            }

        }
    }

    void PlaceTower()
    {

    }

    void CancelPlacement()
    {
        towerToPlaceDown = null;
        placingTower = false;
        curSelectedTile = null;
        previewTower.gameObject.SetActive(false);
    }
}