using UnityEngine;

/// <summary>
/// Container for all rules that can be edited by the user
/// </summary>
public class CSettingsContainer : MonoBehaviour
{
    private Vector2Int m_dim;
    private int m_dieLowerLimit;
    private int m_dieUpperLimit;
    private int m_resurrect;
    private float m_playSpeed;

    /// <summary>
    /// The Playground dimensions
    /// </summary>
    public Vector2Int Dimensions
    {
        get { return m_dim; }
        set
        {
            m_dim = new Vector2Int(Mathf.Max(Mathf.Min(value.x, 16), 3), Mathf.Max(Mathf.Min(value.y, 8), 3));  // Clamped to fit on screen
        }
    }

    /// <summary>
    /// Intermediate property
    /// </summary>
    public int DimX
    {
        get { return Dimensions.x; }
        set { Dimensions = new Vector2Int(value, DimY); }
    }

    /// <summary>
    /// Intermediate property
    /// </summary>
    public int DimY
    {
        get { return Dimensions.y; }
        set { Dimensions = new Vector2Int(DimX, value); }
    }

    /// <summary>
    /// Minimum neighbors to live
    /// </summary>
    public int DieLowerLimit
    {
        get { return m_dieLowerLimit; }
        set
        {
            m_dieLowerLimit = Mathf.Max(Mathf.Min(value, 6), 1);    // Clamped to reasonable values
        }
    }

    /// <summary>
    /// Maxiumum neighbors to live
    /// </summary>
    public int DieUpperLimit
    {
        get { return m_dieUpperLimit; }
        set
        {
            m_dieUpperLimit = Mathf.Max(Mathf.Min(value, 7), 3);    // Clamped to reasonable values
        }
    }

    /// <summary>
    /// Exact ammount of neighbors to become alive from dead
    /// </summary>
    public int Resurrect
    {
        get { return m_resurrect; }
        set
        {
            m_resurrect = Mathf.Max(Mathf.Min(value, 8), 1);    // Clamped to reasonable values
        }
    }

    /// <summary>
    /// Chance to start alive
    /// </summary>
    public int AliveChance { get; set; }

    /// <summary>
    /// Speed of each round
    /// </summary>
    public float PlaySpeed
    {
        get { return m_playSpeed; }
        set
        {
            m_playSpeed = Mathf.Max(Mathf.Min(value, 10.0f), 0.1f);     // Clamped to reasonable values
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        CSettingsContainer[] arr_settings = FindObjectsOfType<CSettingsContainer>();

        if (arr_settings.Length > 1)    // Prevents duplicates
        {
            Destroy(arr_settings[1].gameObject);
        }

        ResetValues();      // Set to standard values
    }

    /// <summary>
    /// Resets the rule values to standard
    /// </summary>
    public void ResetValues()
    {
        Dimensions = new Vector2Int(7, 5);
        DieLowerLimit = 2;
        DieUpperLimit = 3;
        Resurrect = 3;
        PlaySpeed = 1;
        AliveChance = 50;
    }
}
