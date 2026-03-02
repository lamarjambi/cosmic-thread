using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Connection
{
    public GameObject[] items;

    public Connection(List<GameObject> selectedItems)
    {
        items = selectedItems.ToArray();
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UILineRenderer uiLineRenderer;
    public Canvas canvas;

    private List<Connection> connections = new List<Connection>();
    private List<GameObject> selectedItems = new List<GameObject>();
    [SerializeField] ModeIndicator modeIndicator;
    public bool isEscape;
    public bool isSelected = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!modeIndicator.isThreadMode && (selectedItems.Count > 0 || connections.Count > 0))
        {
            ClearSelection();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && modeIndicator.isThreadMode)
        {
            ClearSelection();
        }
    }

    public void OnItemClicked(GameObject item)
    {
        if (!modeIndicator.isThreadMode) return;
        if (selectedItems.Contains(item)) return;

        selectedItems.Add(item);
        item.GetComponent<Image>().color = Color.yellow;

        if (Input.GetKey(KeyCode.LeftShift) && selectedItems.Count >= 2)
        {
            Connection connection = new Connection(selectedItems);
            connections.Add(connection);

            List<Vector2> points = new List<Vector2>();
            foreach (GameObject obj in selectedItems)
                points.Add(GetCanvasPosition(obj.GetComponent<RectTransform>()));

            uiLineRenderer.AddConnection(points);

            foreach (GameObject obj in selectedItems)
            {
                Image img = obj.GetComponent<Image>();
                if (img != null) img.color = Color.white;
            }
            selectedItems.Clear();
        }
    }

    void ClearSelection()
    {
        foreach (GameObject item in selectedItems)
        {
            Image img = item.GetComponent<Image>();
            if (img != null) img.color = Color.white;
        }
        selectedItems.Clear();
        connections.Clear();
        uiLineRenderer.ClearAll();
    }

    Vector2 GetCanvasPosition(RectTransform itemRect)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, itemRect.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiLineRenderer.GetComponent<RectTransform>(),
            screenPoint,
            null,
            out Vector2 localPoint
        );

        return localPoint;
    }
}