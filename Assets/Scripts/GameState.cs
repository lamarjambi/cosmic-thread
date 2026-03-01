using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState _instance;
    public static GameState Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameState");
                _instance = go.AddComponent<GameState>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // thread mode: false = inspect, true = connect
    public bool threadMode = false;
    
    // connections - need to adjust 
    public List<ConnectionPair> connections = new List<ConnectionPair>();
    
    // Last selected item (for connection creation)
    public Item lastSelectedItem = null;
    
    // Registered items by ID
    private Dictionary<int, Item> itemsById = new Dictionary<int, Item>();
    private int nextItemId = 1;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int RegisterItem(Item item)
    {
        int id = nextItemId++;
        itemsById[id] = item;
        item.itemId = id;
        return id;
    }

    public Item GetItemById(int id)
    {
        itemsById.TryGetValue(id, out Item item);
        return item;
    }

    public void ClearConnections()
    {
        connections.Clear();
        lastSelectedItem = null;
    }

    public bool AreConnected(int id1, int id2)
    {
        // Ensure smallest ID is first
        if (id1 > id2)
        {
            int temp = id1;
            id1 = id2;
            id2 = temp;
        }

        foreach (var pair in connections)
        {
            if (pair.id1 == id1 && pair.id2 == id2)
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class ConnectionPair
{
    public int id1;
    public int id2;

    public ConnectionPair(int id1, int id2)
    {
        // Always store smallest ID first
        if (id1 > id2)
        {
            this.id1 = id2;
            this.id2 = id1;
        }
        else
        {
            this.id1 = id1;
            this.id2 = id2;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is ConnectionPair other)
        {
            return id1 == other.id1 && id2 == other.id2;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return id1.GetHashCode() ^ id2.GetHashCode();
    }
}

