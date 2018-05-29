using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Contains all possible states for a cell
/// </summary>
public enum ECellState
{
    DEAD,
    ALIVE
}

/// <summary>
/// Vital information for cells and their behaviour according to it's neighbors
/// </summary>
public class CCellBehaviour : MonoBehaviour
{
    public SpriteRenderer m_Sprite;     // To control the visual representation of the cell state
    public Vector2Int m_ArrPos;         // This cells position on the playground

    private List<CCellBehaviour> m_list_neighbors = new List<CCellBehaviour>(8);    // Contains all eight neighbors to this cell in no specific order
    private CSettingsContainer m_settings;                                          // grants access to the userdefined settings for this game

    /// <summary>
    /// The current state of this cell
    /// </summary>
    public ECellState State { get; private set; }

    /// <summary>
    /// Adds a new neighbor to each others neighbor list
    /// </summary>
    /// <param name="_neighbor"> The neighbor cell to be added </param>
    public void AddNeighbor(CCellBehaviour _neighbor)
    {
        if (!m_list_neighbors.Contains(_neighbor))      // Prevents duplicates
        {
            m_list_neighbors.Add(_neighbor);            // Add neighbor to this cells list
            _neighbor.AddNeighbor(this);                // Add this cell to the neighbors list
        }
    }

    private void Awake()
    {
        m_settings = GetComponentInParent<CPlaygroundBehaviour>().Settings;
        m_Sprite = GetComponent<SpriteRenderer>();

        if (Random.Range(0, 101) <= m_settings.AliveChance)     // Randomly select start state by chance
        {
            State = ECellState.ALIVE;
        }
        else
        {
            State = ECellState.DEAD;
        }
    }

    /// <summary>
    /// Checks the neighbor states and calculates own state
    /// </summary>
    public void CheckNeighbors()
    {
        #region Debugging
        // Debug for checking correct neighbor assignments
        if (m_list_neighbors.Count != 8)
        {
            Debug.Log(m_ArrPos.x + " " + m_ArrPos.y);
        }
        #endregion

        int neighborsAlive = 0;     // Total number of living neighbors

        // Calculate total number of living neighbors
        foreach(CCellBehaviour neighbor in m_list_neighbors)
        {
            if (neighbor.State == ECellState.ALIVE)
            {
                neighborsAlive += 1;
            }
        }

        if(State == ECellState.DEAD && neighborsAlive == m_settings.Resurrect)      // Revive a if dead and rules are met
        {
            State = ECellState.ALIVE;
        }
        else if(neighborsAlive < m_settings.DieLowerLimit || neighborsAlive > m_settings.DieUpperLimit && State == ECellState.ALIVE)    // Die according to set rules
        {
            State = ECellState.DEAD;
        }
    }
}
