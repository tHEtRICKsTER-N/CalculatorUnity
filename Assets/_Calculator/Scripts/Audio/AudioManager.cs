using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UnityEngine.UI.Button[] _buttons;
    [SerializeField] private AudioSource _aSource;
    [SerializeField] private AudioClip _clip;

    private void Start()
    {
        AddSoundToButtons();
    }

    private void AddSoundToButtons()
    {
        foreach (UnityEngine.UI.Button button in _buttons)
        {
            button.onClick.AddListener(() =>
            {
                if (_aSource.isPlaying)
                    _aSource.Stop();

                _aSource.PlayOneShot(_clip);
            });
        }
    }
}
