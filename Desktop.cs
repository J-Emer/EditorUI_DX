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
        private Game _game;
        public GraphicsDevice Graphics
        {
            get
            {
                return _game.GraphicsDevice;
            }
        }
        public ContentManager Content
        {
            get
            {
                return _game.Content;
            }
        }
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
    




        public Desktop(Game _game, string _defalutFontName)
        {
            this._game = _game;

            this.DefaultFontName = _defalutFontName;
            this.DefaultFont = this._game.Content.Load<SpriteFont>(_defalutFontName);

            this.DefaultTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            this.DefaultTexture.SetData(new Color[] { Color.White });

            this._dock_Manager = new Dock_Manager();

            _game.Window.AllowUserResizing = true;
            _game.Window.ClientSizeChanged += Resized;

            Form form = Form.FromHandle(_game.Window.Handle) as Form;
            form.AllowDrop = true;
            form.DragEnter += Form_DragEnter;
            form.DragDrop += Form_DragDrop;

            Load();
            Resized(this, null);
        }

        private void Resized(object sender, System.EventArgs e)
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
            OnScreenLog.Instance.Log("desktop drag enter");

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }

            OnDragEnter?.Invoke(sender, e);

        }
        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            OnScreenLog.Instance.Log("desktop drag drop");

            /*string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
               //do something with the files -> file is the full path to the file that was dropped
               //blaqh asdfkjasdkl
            }*/

            OnDragDrop?.Invoke(sender, e);
        }


        public void Process()
        {
            Input.Instance.Update();

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
