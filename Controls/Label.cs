using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Controls;
using EditorUI_DX.Utils;

namespace EditorUI_DX.Controls
{
    public class Label : Control
    {
        public Font_Brush FontBrush { get; private set; }

        public Color FontColor
        {
            get
            {
                return FontBrush.DrawColor;
            }
            set
            {
                FontBrush.DrawColor = value;
            }
        }
        public string Text
        {
            get
            {
                return FontBrush.Text;
            }
            set
            {
                FontBrush.Text = value;
            }
        }




        public Label(Desktop _desktop) : base(_desktop)
        {
            this.FontBrush = new Font_Brush(_desktop.DefaultFont);
            BackgroundColor = Color.Transparent;
            Text = "Label";
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _spritebatch.DrawString(FontBrush.Font, FontBrush.Text, Vector2_Int.ToVec2(Position), FontBrush.DrawColor);
        }
    }
}
