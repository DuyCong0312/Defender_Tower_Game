using UnityEngine;

public class TestUI : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0f;
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    public void Play()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
