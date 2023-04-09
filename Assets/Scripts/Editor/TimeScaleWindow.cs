using UnityEngine;
using UnityEditor;

public class TimeScaleWindow : EditorWindow
{
    private float timeScaleValue = 1.0f;

    [MenuItem("Window/Time Scale")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TimeScaleWindow));
    }

    private void OnGUI()
    {
        timeScaleValue = GUILayout.HorizontalSlider(timeScaleValue, 0.0f, 5.0f);
        GUILayout.Label("Time Scale: " + timeScaleValue.ToString("F2"));
        if (GUILayout.Button("Set Time Scale"))
        {
            Time.timeScale = timeScaleValue;
        }
    }
}