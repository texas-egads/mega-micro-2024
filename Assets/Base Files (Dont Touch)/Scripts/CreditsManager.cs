using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private MinigameDefinition[] minigames;
    [SerializeField] private RectTransform panelHolder;
    [SerializeField] private RectTransform bg;
    [SerializeField] private RectTransform button;
    [SerializeField] private GameObject panelPrefab;

    private int count = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (MinigameDefinition m in minigames)
        {
            RectTransform panel = Instantiate(panelPrefab, panelHolder).GetComponent<RectTransform>();
            panel.anchoredPosition = new Vector2(0, -80 - 270 * count++);
            panel.GetComponent<CreditsPanel>().SetMinigame(m);
        }
        button.anchoredPosition = new Vector2(-32.8f, -20 - 270 * count);
        button.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        panelHolder.Translate(Vector2.up * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        float panelY = Mathf.Clamp(panelHolder.anchoredPosition.y, 0, 270 * (count-1));
        panelHolder.anchoredPosition = new Vector2(panelHolder.anchoredPosition.x, panelY);
        bg.anchoredPosition = new Vector2(bg.anchoredPosition.x, (panelY * 150 / (270 * (count - 1)))-300);
    }
}
