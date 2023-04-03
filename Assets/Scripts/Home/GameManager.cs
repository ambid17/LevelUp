using UnityEngine;
using Utils;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private FightDataLoader _fightDataLoader;
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] private UIManager _uiManager;
    
    private EventService _eventService;
    public static EventService EventService
    {
        get
        {
            if (Instance._eventService == null)
            {
                Instance._eventService = new EventService();
            }

            return Instance._eventService;
        }
    }

    public static bool IsLoadingScene = false;
    
    protected override void Initialize()
    {
        SetCrosshair();
        _fightDataLoader.Load();
    }

    private void SetCrosshair()
    {
        Vector2 cursorOffset = new Vector2(_cursorTexture.width/2, _cursorTexture.height/2);
        Cursor.SetCursor(_cursorTexture, cursorOffset, CursorMode.Auto);
    }
}
