using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace EditorUI_DX.Brushes
{
    public class Solid_Brush
    {
        public Texture2D Texture { get; private set; }
        public Color DrawColor { get; set; } = Color.White;

        public Solid_Brush(Desktop _desktop)
        {
            this.Texture = _desktop.DefaultTexture;
        }
    }
}
