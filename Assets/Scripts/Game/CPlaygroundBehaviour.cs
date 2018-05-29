using UnityEngine;

/// <summary>
/// Populates the playground, assigns cell neighbors and applies game rounds
/// </summary>
public class CPlaygroundBehaviour : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab for a single cell")]
    private GameObject m_cellPrefab;
    [SerializeField, Tooltip("Time between rounds at PlaySpeed x1")]
    private float m_baseWaitTime = 2;   // Base time between rounds in seconds

    private CCellBehaviour[,] m_arr_Playground;     // Holds all cells
    private float m_timer;                          // Timer for game rounds
    private bool m_arrFilled = false;               // Wether the playgound has been popoulated

    /// <summary>
    /// Holds userdefined rules for this game
    /// </summary>
    public CSettingsContainer Settings { get; private set; }

	void Start ()
    {
        Settings = FindObjectOfType<CSettingsContainer>();
        m_arr_Playground = new CCellBehaviour[Settings.Dimensions.x, Settings.Dimensions.y];

        // Fill Playground
        for (int x = 0; x < m_arr_Playground.GetLength(0); x++)
        {
            for (int y = 0; y < m_arr_Playground.GetLength(1); y++)
            {
                GameObject go = Instantiate(m_cellPrefab, gameObject.transform);    // Position cells
                go.transform.localPosition = new Vector3(x, -y);
                CCellBehaviour cellBehaviour = go.GetComponent<CCellBehaviour>();
                cellBehaviour.m_ArrPos = new Vector2Int(x, y);                      // Inform cell about its position
                m_arr_Playground[x, y] = cellBehaviour;       
            }
        }

        // Add neighbors to the cells
        foreach (CCellBehaviour cell in m_arr_Playground)
        {
            AddNeighbors(cell.m_ArrPos.x, cell.m_ArrPos.y, cell);
        }

        m_arrFilled = true;     // The playgound has been populated
        Render();               // Startrender of playground
	}

    /// <summary>
    /// Add neighbors to the cell in the playgound array with full bounds checks
    /// </summary>
    /// <param name="_x"> Cells x-position on playground </param>
    /// <param name="_y"> Cells y-position on playground </param>
    /// <param name="_cell"> Cell which will get neighbors added </param>
    private void AddNeighbors(int _x, int _y, CCellBehaviour _cell)
    {
        try
        {
            // Left
            if (!(_x - 1 < 0))
            {
                _cell.AddNeighbor(m_arr_Playground[_x - 1, _y]);
            }
            else
            {
                _cell.AddNeighbor(m_arr_Playground[m_arr_Playground.GetLength(0) - 1, _y]);
            }

            // Right
            if (!(_x + 1 >= m_arr_Playground.GetLength(0)))
            {
                _cell.AddNeighbor(m_arr_Playground[_x + 1, _y]);
            }
            else
            {
                _cell.AddNeighbor(m_arr_Playground[0, _y]);
            }

            // Top
            if (!(_y - 1 < 0))
            {
                _cell.AddNeighbor(m_arr_Playground[_x, _y - 1]);
            }
            else
            {
                _cell.AddNeighbor(m_arr_Playground[_x, m_arr_Playground.GetLength(1) - 1]);
            }

            // Bottom
            if (!(_y + 1 >= m_arr_Playground.GetLength(1)))
            {
                _cell.AddNeighbor(m_arr_Playground[_x, _y + 1]);
            }
            else
            {
                _cell.AddNeighbor(m_arr_Playground[_x, 0]);
            }

            // Top Left
            if (!(_x - 1 < 0 || _y - 1 < 0))
            {
                _cell.AddNeighbor(m_arr_Playground[_x - 1, _y - 1]);
            }
            else if (!(_x - 1 < 0))
            {
                _cell.AddNeighbor(m_arr_Playground[_x - 1, m_arr_Playground.GetLength(1) - 1]);
            }
            else if(!(_y - 1 < 0))
            {
                _cell.AddNeighbor(m_arr_Playground[m_arr_Playground.GetLength(0) - 1, _y - 1]);
            }
            else
            {
                _cell.AddNeighbor(m_arr_Playground[m_arr_Playground.GetLength(0) - 1, m_arr_Playground.GetLength(1) - 1]);
            }

            // Top Right
            if (!(_x + 1 >= m_arr_Playground.GetLength(0) || _y - 1 < 0))
            {
                _cell.AddNeighbor(m_arr_Playground[_x + 1, _y - 1]);
            }
            else if (!(_x + 1 >= m_arr_Playground.GetLength(0)))
            {
                _cell.AddNeighbor(m_arr_Playground[_x + 1, m_arr_Playground.GetLength(1) - 1]);
            }
            else if (!(_y - 1 < 0))
            {
                _cell.AddNeighbor(m_arr_Playground[0, _y - 1]);
            }
            else
            {
                _cell.AddNeighbor(m_arr_Playground[0, m_arr_Playground.GetLength(1) - 1]);
            }

            // Bottom Left
            if (!(_x - 1 < 0 || _y + 1 >= m_arr_Playground.GetLength(1)))
            {
                _cell.AddNeighbor(m_arr_Playground[_x - 1, _y + 1]);
            }
            else if (!(_x - 1 < 0))
            {
                _cell.AddNeighbor(m_arr_Playground[_x - 1, 0]);
            }
            else if (!(_y + 1 >= m_arr_Playground.GetLength(1)))
            {
                _cell.AddNeighbor(m_arr_Playground[m_arr_Playground.GetLength(0) - 1, _y + 1]);
            }
            else
            {
                _cell.AddNeighbor(m_arr_Playground[m_arr_Playground.GetLength(0) - 1, 0]);
            }

            // Bottom Right
            if (!(_x + 1 >= m_arr_Playground.GetLength(0) || _y + 1 >= m_arr_Playground.GetLength(1)))
            {
                _cell.AddNeighbor(m_arr_Playground[_x + 1, _y + 1]);
            }
            else if (!(_x + 1 >= m_arr_Playground.GetLength(0)))
            {
                _cell.AddNeighbor(m_arr_Playground[_x + 1, 0]);
            }
            else if (!(_y + 1 >= m_arr_Playground.GetLength(1)))
            {
                _cell.AddNeighbor(m_arr_Playground[0, _y + 1]);
            }
            else
            {
                _cell.AddNeighbor(m_arr_Playground[0, 0]);
            }
        }
        catch (System.Exception e)  // Catch errors of assignement (esp. null references)
        {
            Debug.Log(_cell.m_ArrPos.x + "," + _cell.m_ArrPos.y);
        }
    }
	
	private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))       // Return to menu
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        // Check time and populated playground
        m_timer += Time.deltaTime;
        if(m_arrFilled && m_timer < m_baseWaitTime / Settings.PlaySpeed)
        {
            return;
        }
        m_timer = 0;

        // Check all cells and apply rules
        foreach(CCellBehaviour cell in m_arr_Playground)
        {
            cell.CheckNeighbors();
        }

        Render();   // Render the current round
	}

    /// <summary>
    /// Visualizses the current states of all cells
    /// </summary>
    private void Render()
    {
        foreach (CCellBehaviour cell in m_arr_Playground)
        {
            if (cell.State == ECellState.DEAD)
            {
                cell.m_Sprite.enabled = false;
            }
            else
            {
                cell.m_Sprite.enabled = true;
            }
        }
    }
}
