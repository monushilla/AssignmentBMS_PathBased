using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileHover : MonoBehaviour
{
    public Camera mainCamera;
    public TextMeshProUGUI infoText; // Reference to the UI Text element

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
                if (tileInfo != null)
                {
                    DisplayTileInfo(tileInfo);
                }
            }
        }
    }

    void DisplayTileInfo(TileInfo tileInfo)
    {
        infoText.text = "Tile Position: " + tileInfo.gridPosition.ToString();
    }
}
