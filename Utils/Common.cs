using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pupple.Objects.Scenes;

namespace Pupple;

public static class Common
{
    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[Globals.Instance.random.Next(list.Count)];
    }

    public static bool IsInGameWindow(Vector2 position)
    {
        return position.X >= PlayScene.GameWindowOffset && position.X <= PlayScene.GameWindowWidth + PlayScene.GameWindowOffset &&
               position.Y >= 0 && position.Y <= Globals.ScreenH;
    }
}

