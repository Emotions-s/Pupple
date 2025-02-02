using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pupple.Objects
{
    public class FreezeTimeBox : Box
    {
        public FreezeTimeBox(int width, int height, Vector2 originPos, string header, Color bgColor, Color fontColor) : base(width, height, originPos, header, bgColor, fontColor)
        {
        }
        public override void Update()
        {
        }

        public override void Draw()
        {
            var text = "Not in use yet";
            var color = BgColor;
            if (Globals.GameState.FreezeTime > 0)
            {
                text = $"{Globals.GameState.FreezeTime:F1} Sec.";
                color = Globals.FreezeColor;
            }

            Globals.SpriteBatch.Draw(Globals.Pixel,
                OriginPos,
                null,
                color,
                0f,
                Vector2.Zero,
                new Vector2(Width, Height),
                SpriteEffects.None,
                0f
            );

            var textSize = Globals.Font.MeasureString(Header);
            Globals.SpriteBatch.DrawString(Globals.Font, Header, OriginPos + new Vector2((Width - textSize.X) / 2, (Globals.GridSize - textSize.Y) / 2), FontColor);

            textSize = Globals.Font.MeasureString(text);
            Globals.SpriteBatch.DrawString(Globals.Font, text, OriginPos + new Vector2((Width - textSize.X) / 2, (Globals.GridSize - textSize.Y) / 2 + Globals.GridSize), FontColor);
        }
    }

}