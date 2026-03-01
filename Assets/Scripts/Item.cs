using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Settings")]
    public int itemId = -1; // default state
    public string itemName;
    public string description;
    
    [Header("Inspect Mode")]
    public GameObject descriptionPanel;
    public TMPro.TextMeshProUGUI descriptionText;
    public Vector2 descriptionOffset = new Vector2(0, 0);
    public float descriptionWidth = 400f;
    
    [Header("Connection Point")]
    public Vector2 connectionPointOffset = new Vector2(30f, 20f); 
    public bool isUIElement = true; 
    
    [Header("Audio")]
    public AudioClip clickSound;
    public AudioClip pinSound; 

    [Header("Description Background")]
    private AudioSource audioSource;
    private Camera mainCamera;
    
    protected virtual void Start()
    {
        if (GameState.Instance != null)
        {
            if (itemId == -1)
            {
                itemId = GameState.Instance.RegisterItem(this);
            }
        }
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
            
        mainCamera = Camera.main;
        if (mainCamera == null)
            mainCamera = FindObjectOfType<Camera>();
        
        // for descriptoin 
        if (descriptionText != null && !string.IsNullOrEmpty(description))
        {
            descriptionText.text = description;
        }
            
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameState gameState = GameState.Instance;
            if (gameState == null) return;

            // thread mode + shift -> connect items
            if (gameState.threadMode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                HandleThreadModeClick();
            }
            // inspect mode -> show description
            else if (!gameState.threadMode)
            {
                HandleInspectModeClick();
            }
        }
    }

    private void HandleThreadModeClick()
    {
        GameState gameState = GameState.Instance;
        
        // Play pin sound
        if (pinSound != null && audioSource != null)
            audioSource.PlayOneShot(pinSound);
        
        // First click: store the first item
        if (gameState.lastSelectedItem == null)
        {
            gameState.lastSelectedItem = this;
        }
        // Second click: connect with previous
        else if (gameState.lastSelectedItem != this)
        {
            // Check if already connected
            if (!gameState.AreConnected(this.itemId, gameState.lastSelectedItem.itemId))
            {
                ConnectionPair newPair = new ConnectionPair(this.itemId, gameState.lastSelectedItem.itemId);
                gameState.connections.Add(newPair);
            }
            gameState.lastSelectedItem = null;
        }
        // Clicking the same item again resets selection
        else
        {
            gameState.lastSelectedItem = null;
        }
    }

    private void HandleInspectModeClick()
    {
        // Play click sound
        if (clickSound != null && audioSource != null)
            audioSource.PlayOneShot(clickSound);
        
        // Show/hide description panel
        if (descriptionPanel != null)
        {
            bool isActive = descriptionPanel.activeSelf;
            descriptionPanel.SetActive(!isActive);
            
            if (!isActive && descriptionText != null)
            {
                // Position description panel near the item
                RectTransform rectTransform = descriptionPanel.GetComponent<RectTransform>();
                if (rectTransform != null && mainCamera != null)
                {
                    // Convert world position to screen position, then to UI position
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
                    Canvas canvas = descriptionPanel.GetComponentInParent<Canvas>();
                    if (canvas != null)
                    {
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            canvas.transform as RectTransform,
                            screenPos,
                            canvas.worldCamera,
                            out Vector2 localPoint
                        );
                        rectTransform.anchoredPosition = localPoint + descriptionOffset;
                    }
                }
            }
        }
    }

    public Vector2 GetConnectionPoint()
    {
        if (isUIElement)
        {
            // For UI elements, use RectTransform
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Canvas canvas = GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    // Get the center position in world space
                    Vector3 center = rectTransform.position;
                    
                    // Add offset in local space, then convert to world
                    Vector3 offsetLocal = new Vector3(connectionPointOffset.x, connectionPointOffset.y, 0);
                    Vector3 offsetWorld = rectTransform.TransformVector(offsetLocal);
                    
                    Vector3 worldPos = center + offsetWorld;
                    
                    // For Screen Space - Overlay, we need to convert to world coordinates via camera
                    if (canvas.renderMode == RenderMode.ScreenSpaceOverlay && mainCamera != null)
                    {
                        // Convert screen position to world position
                        // Get screen position of the UI element
                        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPos);
                        // Convert to world space using camera
                        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
                        if (distance == 0) distance = mainCamera.nearClipPlane + 10f;
                        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, distance));
                        return new Vector2(worldPoint.x, worldPoint.y);
                    }
                    
                    return new Vector2(worldPos.x, worldPos.y);
                }
            }
        }
        
        // For world space items, use transform position
        Vector3 worldPosition = transform.position + (Vector3)connectionPointOffset;
        return new Vector2(worldPosition.x, worldPosition.y);
    }
}

