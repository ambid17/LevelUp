using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;
using Utils;

public class TooltipShowEvent : IEvent
{
    public string Header;
    public string Content;

    public TooltipShowEvent(string header, string content)
    {
        Header = header;
        Content = content;
    }
}

public class TooltipHideEvent { }
