using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemTutorial : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Information")]
    public string itemName;
    [TextArea(3, 5)]
    public string itemDescription;
    public Sprite itemDetailImage; // optional larger image for detail view
    
    [Header("Visual References")]
    public GameObject detailPanel; // the purple background panel that shows details
    public TMPro.TextMeshProUGUI detailNameText;
    public TMPro.TextMeshProUGUI detailDescriptionText;
    public Image detailImage;
    public Image exitImage; // the exit image that acts as a button
    
    
    // component references
    private Collider itemCollider;
    private MysteryTutorial gameManager;
    
    // state
    public bool isInteractable = true;
    
    // events
    public System.Action OnItemClicked;
    public System.Action OnExitClicked;
    public System.Action<ItemTutorial> OnThreadModeClick;
    
    void Start()
    {
        // get component references
        itemCollider = GetComponent<Collider>();
        
        gameManager = FindObjectOfType<MysteryTutorial>();
        
        // setup detail panel
        if (detailPanel != null)
        {
            detailPanel.SetActive(false);
        }
        
        // setup exit image as clickable button
        if (exitImage != null)
        {
            // add button component if it doesn't exist
            Button exitButton = exitImage.GetComponent<Button>();
            if (exitButton == null)
            {
                exitButton = exitImage.gameObject.AddComponent<Button>();
            }
            exitButton.onClick.AddListener(HideDetails);
        }
        
    }
    
    void Update()
    {
        // handle escape key to close detail panel
        if (detailPanel != null && detailPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            HideDetails();
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable) return;
        
        // Get the game manager to check thread mode
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<MysteryTutorial>();
        }
        
        if (gameManager != null)
        {
            // In thread mode: only allow shift+click for connections
            if (gameManager.threadMode)
            {
                // Only handle thread mode connections if shift is held
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    OnThreadModeClick?.Invoke(this);
                }
                // If in thread mode but shift not held, do nothing (no inspection)
            }
            else
            {
                // In inspect mode: normal inspection behavior
                ShowDetails();
                OnItemClicked?.Invoke();
            }
        }
        else
        {
            // Fallback: normal inspection behavior if no game manager found
            ShowDetails();
            OnItemClicked?.Invoke();
        }
    }
    
    public void ShowDetails()
    {
        if (detailPanel == null) return;
        
        // populate detail panel with text on purple background
        if (detailDescriptionText != null) detailDescriptionText.text = itemDescription;
        
        detailPanel.SetActive(true);
    }
    
    public void HideDetails()
    {
        if (detailPanel != null)
        {
            detailPanel.SetActive(false);
        }
        
        // notify tutorial system that exit was clicked
        OnExitClicked?.Invoke();
    }
    
    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;
        if (itemCollider != null) itemCollider.enabled = interactable;
    }
    
}