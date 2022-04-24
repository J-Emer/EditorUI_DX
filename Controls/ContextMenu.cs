using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;


namespace EditorUI_DX.Controls
{
    public class ContextMenu : Control, IControl_Container<Button>
    {
        public Control_Collection<Button> Controls{get;set;} = new Control_Collection<Button>();
        private Padding Padding = new Padding(0);
        private Layout Layout;
        private string _fontName;

        public ContextMenu(Desktop _desktop, string _fontName) : base(_desktop)
        {
            this.ZOrder = 0;
            this._fontName = _fontName;
            this.Layout = new Vertical_Stretch_Layout();
            this.Padding = new Padding(5);
            this.Controls.OnControlsChanged += After_Invalidated;

            this.OnMouseExit += MouseExit;

            IsActive = false;
        }

        /// <summary>
        /// Adds a Button to the ContextMenu
        /// </summary>
        /// <param name="_text">The text the Button will display</param>
        /// <returns>Button</returns>
        public Button Add(string _text)
        {
            Button _b = new Button(this._desktop)
            {
                Text = _text
            };

            Controls.Add(_b);
            return _b;
        }

        protected override void After_Invalidated()
        {
            Layout.Handle_Layout(Controls.Controls, this.SourceRectangle, this.Padding);
        }
        protected override void After_Process()
        {
            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].Process();
            }
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].Render(_spritebatch);
            }
        }

        private void MouseExit()
        {
            if(IsActive)
            {
                IsActive = false;
            }
        }
    }
}


