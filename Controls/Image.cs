using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Utils;

namespace EditorUI_DX.Controls
{
    public class Image : Control
    {
        public Texture2D Texture{get;set;}
        public Color DrawColor{get;set;} = Color.White;

        public Image(Desktop _desktop, string _imageName) : base(_desktop)
        {
            Texture = _desktop.Content.Load<Texture2D>(_imageName);
            this.Size = new Vector2_Int(32, 32);
        }

        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _spritebatch.Draw(Texture, this.SourceRectangle, DrawColor);
        }
    }
}


