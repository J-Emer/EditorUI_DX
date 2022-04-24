using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Controls;


namespace EditorUI_DX.Utils
{
    public class Border
    {
        public int Thickness{get;set;} = 1;
        public Element Parent{get;set;}
        public Color DrawColor{get;set;} = Color.White;
        private Texture2D _texture;


        public Border(GraphicsDevice _graphics, int _thickness, Element _parent)
        {
            this.Parent = _parent;

            this.Thickness = _thickness;

            this._texture = new Texture2D(_graphics, 1,1);
            _texture.SetData(new Color[] {DrawColor});
        }


        public void Render(SpriteBatch _spritebatch)
        {
            if(_texture == null || Thickness == 0){return;}

            _spritebatch.Draw(_texture, new Rectangle(Parent.SourceRectangle.Left, Parent.SourceRectangle.Top, Parent.SourceRectangle.Width, Thickness), DrawColor);//top
            _spritebatch.Draw(_texture, new Rectangle(Parent.SourceRectangle.Right, Parent.SourceRectangle.Top, Thickness, Parent.SourceRectangle.Height), DrawColor);//right
            _spritebatch.Draw(_texture, new Rectangle(Parent.SourceRectangle.Left, Parent.SourceRectangle.Bottom, Parent.SourceRectangle.Width + Thickness, Thickness), DrawColor);//bottom
            _spritebatch.Draw(_texture, new Rectangle(Parent.SourceRectangle.Left, Parent.SourceRectangle.Top, Thickness, Parent.SourceRectangle.Height), DrawColor);//left
        }
    }
}
