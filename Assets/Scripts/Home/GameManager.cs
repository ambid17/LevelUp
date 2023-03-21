using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private FightDataLoader _fightDataLoader;
    [SerializeField] private UIManager _uiManager;

    [SerializeField] private Texture2D _cursorTexture;
    public UIManager UIManager => Instance._uiManager;
    
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
    
    public override void Initialize()
    {
        _fightDataLoader.Load();
        Vector2 cursorOffset = new Vector2(_cursorTexture.width/2, _cursorTexture.height/2);
        Cursor.SetCursor(_cursorTexture, cursorOffset, CursorMode.Auto);
    }

    void Update()
    {
        
    }
}
