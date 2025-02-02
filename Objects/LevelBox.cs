using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pupple.Objects;
public class LevelBox : Box
{
    public LevelBox(int width, int height, Vector2 originPos, string header, Color bgColor, Color fontColor) : base(width, height, originPos, header, bgColor, fontColor)
    {
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

        var textSize = Globals.Font.MeasureString($"Level {Globals.GameState.Level}");

        // draw in the middle
        Globals.SpriteBatch.DrawString(Globals.Font, $"Level {Globals.GameState.Level}", OriginPos + new Vector2(Width / 2 - textSize.X / 2, Height / 2 - textSize.Y / 2), FontColor);
    }

}