using UnityEngine;

[CreateAssetMenu(fileName = "New Theme",menuName = "Calculator/New Theme")]
public class ThemeSO : ScriptableObject
{
    public string themeName;

    public Color backgroundColor;
    public Color expressionBGColor;
    public Color primaryTextColor;
    public Color numberButtonsTextColor;
    public Color operationsButtonsTextColor;
    public Color operationsButtonColor;
    public Color numberButtonColor;
    public Color firstRowThreeButtonsColor;

    public Sprite icon;
}
