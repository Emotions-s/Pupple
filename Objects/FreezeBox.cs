using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Managers;
using Pupple.Objects.Scenes;
using Pupple.States;

namespace Pupple.Objects;

public class FreezeBox : Box
{
    Texture2D Texture { get; set; }
    private int _activeIndex;
    public FreezeBox(int width, int height, Vector2 originPos, string header, Color bgColor, Color fontColor, Texture2D texture) : base(width, height, originPos, header, bgColor, fontColor)
    {
        Texture = texture;
        _activeIndex = 0;
    }

    public override void Update()
    {
        _activeIndex = 0;
        if (InputManager.MousePosition.Y >= OriginPos.Y + Globals.GridSize
        && InputManager.MousePosition.Y <= OriginPos.Y + Globals.GridSize * 2)
        {
            int indexX = (int)(InputManager.MousePosition.X - PlayScene.GameWindowOffset - PlayScene.GameWindowWidth - OriginPos.X) / Globals.GridSize;
            if (indexX >= 1 && indexX <= PlayerState.MaxBubbleQueueSize)
            {
                _activeIndex = indexX;
                if (InputManager.Clicked)
                {
                    Globals.Shooter.ChangeBubble(0, new FreezeBubble(Vector2.Zero, Texture));
                    Globals.PlayerState.FreezeNum--;
                }
            }
        }
    }

    public override void Draw()
    {
        Globals.SpriteBatch.Draw(Globals.Pixel,
            OriginPos,
            null,
            BgColor,
            0f,
            Vector2.Zero,
            new Vector2(Width, Height),
            SpriteEffects.None,
            0f
        );

        var textSize = Globals.Font.MeasureString(Header);
        Globals.SpriteBatch.DrawString(Globals.Font, Header, OriginPos + new Vector2((Width - textSize.X) / 2, (Globals.GridSize - textSize.Y) / 2), FontColor);

        for (var i = 1; i <= Globals.PlayerState.FreezeNum; i++)
        {
            var vp = BubbleHelper.SpecialBubbleViewPort[BubbleSpecial.Freeze];
            Globals.SpriteBatch.Draw(Texture, OriginPos + new Vector2(Globals.GridSize * i, Globals.GridSize * 2 - vp.Height), vp, _activeIndex == i ? Color.Gray : Color.White);
        }
    }
}