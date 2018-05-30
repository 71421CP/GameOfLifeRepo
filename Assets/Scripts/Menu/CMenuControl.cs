using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI interaction for menus
/// </summary>
public class CMenuControl : MonoBehaviour
{
    private CSettingsContainer m_settings;      // Settings container holds all rules

    private void Start()
    {
        m_settings = FindObjectOfType<CSettingsContainer>();
    }

    /// <summary>
    /// Load a scene by index
    /// </summary>
    /// <param name="_index"> Index of the target scene </param>
    public void LoadScene(int _index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_index);
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR        // Stops playing in editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    /// <summary>
    /// Changes the integer value of the given property in the settings container
    /// </summary>
    /// <param name="_settingsProperty"> Propertyname </param>
    public void SubmitIntValue(string _settingsProperty)
    {
        int o = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>().text); // Conversion
        m_settings.GetType().GetProperty(_settingsProperty).SetValue(m_settings, o, null);  // Setting property value
    }

    /// <summary>
    /// Changes the float value of the given property in the settings container
    /// </summary>
    /// <param name="_settingsProperty"> Propertyname </param>
    public void SubmitFloatValue(string _settingsProperty)
    {
        float o = System.Convert.ToSingle(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>().text);  // Conversion
        m_settings.GetType().GetProperty(_settingsProperty).SetValue(m_settings, o, null);  // Setting property value
    }

    /// <summary>
    /// Calls ResetValue on the settings container
    /// </summary>
    public void ResetValuesClick()
    {
        FindObjectOfType<CSettingsContainer>().ResetValues();
    }
}
