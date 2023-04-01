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

public class WeaponSelectedEvent : IEvent
{
    public Weapon Weapon;

    public WeaponSelectedEvent(Weapon weapon)
    {
        Weapon = weapon;
    }
}
