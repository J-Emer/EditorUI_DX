using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Utils;

namespace EditorUI_DX.Controls
{
    public class Button : Control
    {
        public Label Label { get; private set; }
        public Color FontColor
        {
            get
            {
                return Label.FontBrush.DrawColor;
            }
            set
            {
                Label.FontBrush.DrawColor = value;
            }
        }
        public string Text
        {
            get
            {
                return Label.Text;
            }
            set
            {
                Label.Text = value;
            }
        }

        public Color NormalColor { get; set; } = new Color(41, 97, 171);
        public Color HighlightColor { get; set; } = new Color(39, 126, 242);

        public Button(Desktop _desktop) : base(_desktop)
        {
            Label = new Label(_desktop);
            Label.Text = "Button";

            this.BackgroundColor = NormalColor;

            this.Size = new Vector2_Int(100, 30);

            this.OnMouseEnter += MouseEnter;
            this.OnMouseExit += MouseExit;
        }

        protected override void After_Invalidated()
        {
            Label.Position = this.Center - Vector2_Int.FromVec2(Label.FontBrush.HalfFontSize);

            Vector2_Int _fontSize = Vector2_Int.FromVec2(Label.FontBrush.FontSize);

            Label.IsActive = _fontSize.X < this.Size.X && _fontSize.Y < this.Size.Y;//---toggles Label.IsActive if the Lable.FontSize is greater than this.SourceRectangle
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            Label.Render(_spritebatch);
        }


        private void MouseEnter()
        {
            BackgroundColor = HighlightColor;
        }
        private void MouseExit()
        {
            BackgroundColor = NormalColor;
        }
    }
}
