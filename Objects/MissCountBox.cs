using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pupple.States;

namespace Pupple.Objects;

public class MissCountBox : Box
{

    Texture2D Texture { get; set; }
    Rectangle ActiveViewport { get; set; }
    Rectangle NonActiveViewport { get; set; }

    public MissCountBox(int width, int height, Vector2 originPos, string header, Color bgColor, Color fontColor, Texture2D texture, Rectangle activeVP, Rectangle nonActiveVP) : base(width, height, originPos, header, bgColor, fontColor)
    {
        Texture = texture;
        ActiveViewport = activeVP;
        NonActiveViewport = nonActiveVP;
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

        for (int i = 0; i < Globals.GameState.MaxMissCount; i++)
        {
            var vp = i < Globals.GameState.MissCount ?  ActiveViewport : NonActiveViewport;
            Globals.SpriteBatch.Draw(Texture, OriginPos + new Vector2(Globals.GridSize * (i + 1), Globals.GridSize * 2 - vp.Height), vp, Color.White);
        }
    }
}