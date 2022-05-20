using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EditorUI_DX.Controls;
using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;


namespace EditorUI_DX
{
    public class Desktop
    {

        public GraphicsDevice Graphics{get; private set;}
        public ContentManager Content{get; private set;}
        public GameWindow Window{get; private set;}
        public Texture2D DefaultTexture { get; private set; }
        public SpriteFont DefaultFont{get; private set;}
        public string DefaultFontName{get; private set;}
        public Control_Collection<EditorUI_DX.Controls.Control> Controls { get; set; } = new Control_Collection<EditorUI_DX.Controls.Control>();
        public Dock_Manager Dock_Manager
        {
            get
            {
                return _dock_Manager;
            }
        }
        private Dock_Manager _dock_Manager;


        public event Action<object, DragEventArgs> OnDragEnter;
        public event Action<object, DragEventArgs> OnDragDrop;
    
        public Input Input{get; private set;}




        public Desktop(GraphicsDevice _graphics, ContentManager _content, GameWindow _window, string _defalutFontName)
        {
            this.Graphics = _graphics;
            this.Content = _content;
            this.Window = _window;

            this.Input = new Input(_window);

            this.DefaultFontName = _defalutFontName;
            this.DefaultFont = this.Content.Load<SpriteFont>(_defalutFontName);

            this.DefaultTexture = new Texture2D(this.Graphics, 1, 1);
            this.DefaultTexture.SetData(new Color[] { Color.White });

            this._dock_Manager = new Dock_Manager();

            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += Internal_Resized;

            Form form = Form.FromHandle(this.Window.Handle) as Form;
            form.AllowDrop = true;
            form.DragEnter += Form_DragEnter;
            form.DragDrop += Form_DragDrop;

            Load();
            Internal_Resized(this, null);
        }

        public void Resize(object sender, System.EventArgs e)
        {
            Internal_Resized(null, null);
        }

        private void Internal_Resized(object sender, System.EventArgs e)
        {
            Rectangle _rect = this.Graphics.Viewport.Bounds;

            foreach (var item in Controls.Collecton)
            {
                if (item.DockStyle == EditorUI_DX.Utils.DockStyle.None) { continue; }

                Dock_Manager.Do_Dock(_rect, item);
                _rect = Dock_Manager.SubtractRect(item.DockStyle, _rect, item.SourceRectangle);
            }
        }

        public virtual void Load() { }
        public virtual void Unload() { }

        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }

            OnDragEnter?.Invoke(sender, e);
        }
        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            OnDragDrop?.Invoke(sender, e);
        }


        public void Process()
        {
            Input.Update();

            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].Process();
            }
        }
        public void Render(SpriteBatch _spritebatch)
        {
            _spritebatch.Begin();

            foreach (var Control in Controls.GetByZOrder_Des)
            {
                Control.Render(_spritebatch);
            }

            /*foreach (var item in Controls.Collecton)
            {
                item.Render(_spritebatch);
            }*/

            _spritebatch.End();
        }

    }
}
