using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pupple.Objects
{
    public class Button : IComponent
    {
        protected Vector2 position;
        protected Texture2D texture;
        public Rectangle sourceRectangle;
        private bool isHovered;
        private bool isClicked;
        public Action onClickAction;

        public Button(Vector2 position, Texture2D texture, Rectangle sourceRectangle, Action onClickAction)
        {
            this.position = position;
            this.texture = texture;
            this.sourceRectangle = sourceRectangle;
            this.onClickAction = onClickAction;
        }

        public void Reset() { }

        public void Update()
        {
            // Check if the mouse is hovering over the button
            if (IsMouseOver())
            {
                isHovered = true;
            }
            else
            {
                isHovered = false;
            }
            // Detect click and execute onClickAction if the button is clicked
            if (isHovered && Mouse.GetState().LeftButton == ButtonState.Pressed && !isClicked)
            {
                isClicked = true;
                onClickAction?.Invoke();  // Trigger the click action
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                isClicked = false;  // Reset click state when mouse is released
            }
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(texture,
                position,
                sourceRectangle,
                isHovered ? Color.Gray : Color.White
            );
        }

        private bool IsMouseOver()
        {
            var mouseState = Mouse.GetState();
            var buttonBounds = new Rectangle((int)position.X, (int)position.Y, sourceRectangle.Width, sourceRectangle.Height);
            return buttonBounds.Contains(mouseState.X, mouseState.Y);
        }
    }
}
