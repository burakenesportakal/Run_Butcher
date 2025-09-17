using UnityEngine;
using UnityEngine.UI;

public class GuideToggle : MonoBehaviour
{
    [SerializeField] private GameObject guidePanel; // Açılıp kapanacak panel

    private bool isOpen = false;

    private void Start()
    {
        if (guidePanel != null)
            guidePanel.SetActive(false); // oyun başlarken kapalı olsun
    }

    public void ToggleGuide()
    {
        if (guidePanel == null) return;

        isOpen = !isOpen;
        guidePanel.SetActive(isOpen);
    }
}
