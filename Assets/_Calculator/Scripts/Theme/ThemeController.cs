using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeController : MonoBehaviour
{
    [Header("Themes")]
    [SerializeField] private ThemeSO[] _themes;

    [Header("Buttons")]
    [SerializeField] private Button _themeButton;
    [SerializeField] private Image[] _numberButtons;
    [SerializeField] private Image[] _operationButtons;
    [SerializeField] private Image[] _firstRowThreeButtons;

    [Header("Images")]
    [SerializeField] private Image _bgImage;
    [SerializeField] private Image _expressionBgImage;
    [SerializeField] private Image _themeButtonBGImage;
    [SerializeField] private Image _themeIconImage;

    [Header("Texts")]
    [SerializeField] private TMP_Text[] _expressionTexts;

    private int _currentThemeIndex = 0;

    private void Start()
    {
        if (_themes.Length == 0)
        {
            Debug.LogError("No themes assigned to ThemeController!");
            return;
        }

        // Inital theme apply
        ApplyTheme(_themes[0]);

        // Adding toggle to theme button
        _themeButton.onClick.AddListener(() =>
        {
            _currentThemeIndex = (_currentThemeIndex + 1) % _themes.Length;
            ThemeSO themeToApply = _themes[_currentThemeIndex];
            ApplyTheme(themeToApply);
        });
    }


    private void ApplyTheme(ThemeSO theme)
    {
        // Number Buttons
        foreach (Image img in _numberButtons)
        {
            img.color = theme.numberButtonColor;
            img.GetComponentInChildren<TMP_Text>().color = theme.numberButtonsTextColor;
        }

        // Operation Buttons
        foreach (Image img in _operationButtons)
        {
            img.color = theme.operationsButtonColor;
            img.GetComponentInChildren<TMP_Text>().color = theme.operationsButtonsTextColor;
        }

        // BG Image
        _bgImage.color = theme.backgroundColor;

        // Expression BG Image
        _expressionBgImage.color = theme.expressionBGColor;

        // Expression Texts
        foreach (TMP_Text text in _expressionTexts)
        {
            text.color = theme.primaryTextColor;
        }

        // First Row Three Buttons (AC, Clear, %)
        foreach (Image img in _firstRowThreeButtons)
        {
            img.color = theme.firstRowThreeButtonsColor;
            img.GetComponentInChildren<TMP_Text>().color = theme.operationsButtonColor;
        }

        // Theme Button
        _themeButtonBGImage.color = theme.backgroundColor;

        // Change theme button icon
        _themeIconImage.sprite = theme.icon;

        print("<color=green>Applied theme: " + theme.themeName + "</color>");
    }
}
