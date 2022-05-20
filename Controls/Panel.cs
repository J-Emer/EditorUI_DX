using System;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;



namespace EditorUI_DX.Controls
{
    public class Panel : Control, IControl_Container<Control>, IDragDrop_Container
    {
        public Control_Collection<Control> Controls{get;set;} = new Control_Collection<Control>();
        public EditorUI_DX.Utils.Padding Padding = new EditorUI_DX.Utils.Padding(0);
        public Layout Layout;

        public event Action<object, DragEventArgs> OnDragDrop;

        public Panel(Desktop _desktop) : base(_desktop)
        {
            this.BackgroundColor = new Color(57, 60, 64);
            Layout = new Layout();
            Controls.OnControlsChanged += After_Invalidated;

            _desktop.OnDragDrop += Internal_DragDrop;
        }

        ~Panel()
        {
            _desktop.OnDragDrop -= Internal_DragDrop;
        }

        private void Internal_DragDrop(object sender, DragEventArgs e)
        {
            if(SourceRectangle.Contains(this._desktop.Input.MousePosition))
            {
                OnDragDrop?.Invoke(sender, e);

                Console.WriteLine("panel has something dropped on it");
            }
        }

        protected override void After_Invalidated()
        {
            Layout.Handle_Layout(Controls.Collecton, this.SourceRectangle, this.Padding);
        }
        protected override void After_Process()
        {
            foreach (var item in Controls.Collecton)
            {
                item.Process();
            }
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            foreach (var item in Controls.Collecton)
            {
                item.Render(_spritebatch);
            }
        }
    }
}


