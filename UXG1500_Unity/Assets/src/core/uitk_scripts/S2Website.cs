using UnityEngine;
using UnityEngine.UIElements;

public class S2Website : MonoBehaviour
{
    private VisualElement root;
    private VisualElement heroImage;
    private Label clockLabel;

    private Color hoverBlue = new Color(0.20f, 0.40f, 1f, 1f);
    private Color normalLinkColor = new Color(0f, 0.2f, 0.55f, 1f);

    VisualElement start, quit;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        heroImage = root.Q<VisualElement>("heroImage");
        clockLabel = root.Q<Label>("clockLabel");

        SetupClock();
        SetupHoverLinks();
        SetupTabs();

        root.style.display = DisplayStyle.None;

        root.RegisterCallback<ClickEvent>(OnQuit);
    }

    void OnQuit(ClickEvent evt)
    {
        GameManager.Instance.PrepareScene(2);
        GameManager.Instance.ArmSceneSwitch();
    }

    // -------------------------------------------------------
    // CLOCK
    // -------------------------------------------------------
    void SetupClock()
    {
        UpdateClock();
        InvokeRepeating(nameof(UpdateClock), 1f, 1f);
    }

    void UpdateClock()
    {
        if (clockLabel != null)
            clockLabel.text = System.DateTime.Now.ToString("HH:mm");
    }

    // -------------------------------------------------------
    // SIDEBAR LINK HOVER
    // -------------------------------------------------------
    void SetupHoverLinks()
    {
        var linkButtons = root.Query<Button>().Class("sidebar-link").ToList();

        foreach (var button in linkButtons)
        {
            button.RegisterCallback<MouseEnterEvent>(evt =>
            {
                button.style.color = hoverBlue;
                button.style.unityFontStyleAndWeight = FontStyle.Bold;
            });

            button.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                button.style.color = normalLinkColor;
                button.style.unityFontStyleAndWeight = FontStyle.Normal;
            });
        }
    }

    // -------------------------------------------------------
    // NAV TAB CLICK LOGIC
    // -------------------------------------------------------
    void SetupTabs()
    {
        var tabs = root.Query<Button>().Class("nav-tab-button").ToList();

        foreach (var tab in tabs)
        {
            tab.RegisterCallback<ClickEvent>(evt =>
            {
                foreach (var t in tabs)
                    t.RemoveFromClassList("nav-tab-selected");

                tab.AddToClassList("nav-tab-selected");
            });
        }
    }

    // -------------------------------------------------------
    // SET HERO IMAGE (MODERN UI TOOLKIT API)
    // -------------------------------------------------------
    public void SetHeroImage(Texture2D tex)
    {
        if (heroImage == null || tex == null)
            return;

        heroImage.style.backgroundImage = new StyleBackground(tex);

        // background-size: cover
        heroImage.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Cover);

        // background-position: center;
        heroImage.style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center);
        heroImage.style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center);

        // background-repeat: no-repeat
        heroImage.style.backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat);
    }


}
