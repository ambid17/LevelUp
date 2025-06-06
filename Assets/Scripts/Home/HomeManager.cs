using UnityEngine;
using Utils;

public class HomeManager : Singleton<HomeManager>
{
    [SerializeField] private FightDataLoader _fightDataLoader;
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] private UIManager _uiManager;
        
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
