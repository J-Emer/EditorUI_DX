using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EditorUI_DX.Brushes
{
    public class Font_Brush
    {
        public SpriteFont Font;
        public Color DrawColor { get; set; } = Color.White;
        public Vector2 FontSize
        {
            get
            {
                return Font.MeasureString(Text);
            }
        }
        public Vector2 HalfFontSize
        {
            get
            {
                return FontSize / 2f;
            }
        }
        public string Text = "";


        public Font_Brush(SpriteFont _font)
        {
            this.Font = _font;
        }
    }
}
