using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Controls;
using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;


namespace EditorUI_DX.Controls
{
    public class ListView : Control, IControl_Container<ListViewItem>, IDragDrop_Container
    {
        public Action<object, ListViewItem> OnListViewItemSelected;
        public ListViewItem SelectedListViewItem
        {
            get
            {
                return _selectedListViewItem;
            }
        }
        private ListViewItem _selectedListViewItem;

        public EditorUI_DX.Utils.Padding Padding;
        public Control_Collection<ListViewItem> Controls{get;set;} = new Control_Collection<ListViewItem>();
        public Vector2_Int CellSize{get;set;} = new Vector2_Int(64, 84);

        public event Action<object, DragEventArgs> OnDragDrop;


        public ListView(Desktop _desktop, string _fontName) : base(_desktop)
        {
            Padding = new EditorUI_DX.Utils.Padding(15);

            BackgroundColor = new Color(57, 60, 64);

            Controls.OnControlsChanged += After_Invalidated;

            this.Size = new Vector2_Int(100,100);

            _desktop.OnDragDrop += Internale_DragDrop;
        }

        ~ListView()
        {
            _desktop.OnDragDrop -= Internale_DragDrop;
        }

        private void Internale_DragDrop(object sender, DragEventArgs e)
        {
            if(SourceRectangle.Contains(this._desktop.Input.MousePosition))
            {
                OnDragDrop?.Invoke(sender, e);
            }
        }

        public ListViewItem Add(string _text, object _tag)
        {
            ListViewItem _item = new ListViewItem(this._desktop, _desktop.DefaultFontName, ListViewCallBack)
            {
                Text = _text,
                Size = CellSize,
                Tag = _tag
            };
            Controls.Add(_item);

            return _item;
        }
        public ListViewItem Add(string _text, Texture2D _texture, object _tag)
        {
            ListViewItem _item = new ListViewItem(this._desktop, _desktop.DefaultFontName, ListViewCallBack)
            {
                Text = _text,
                Size = CellSize,
                Image_Texture = _texture,
                Tag = _tag
            };
            
            Controls.Add(_item);

            return _item;
        }
        protected override void After_Invalidated()
        {
            
            int _colCount = this.Size.X / (CellSize.X + Padding.Left + (Padding.Left / 2));
            int _colIndex = 0;
            int _rowsIndex = 0;

            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                if(_colIndex > _colCount)
                {
                    _rowsIndex += 1;
                    _colIndex = 0;
                }

                Control _current = Controls.Collecton[i];

                int x = _colIndex * CellSize.X + (Padding.Left * _colIndex) + (Padding.Left / 2);
                int y = _rowsIndex * CellSize.Y + (Padding.Top * _rowsIndex) + (Padding.Top / 2);

                _current.Position = new Vector2_Int(x, y) + this.Position;


                _colIndex += 1;
            }
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
        private void ListViewCallBack(ListViewItem _viewItem)
        {
            _selectedListViewItem = _viewItem;
            OnListViewItemSelected?.Invoke(this, _selectedListViewItem);
        }
    }

    public class ListViewItem : Control
    {
        public Texture2D Image_Texture
        {
            get
            {
                return _image.Texture;
            }
            set
            {
                _image.Texture = value;
            }
        }
        private Image _image;
        public string Text
        {
            get
            {
                return _label.Text;
            }
            set
            {
                _label.Text = value;
            }
        }
        private Label _label;
        private Action<ListViewItem> _actionCallBack;


        public Color NormalColor{get;set;} = Color.White;
        public Color HighlightColor{get;set;} = Color.Yellow;


        public ListViewItem(Desktop _desktop, string _textName, Action<ListViewItem> _callBack) : base(_desktop)
        {
            _actionCallBack = _callBack;

            BackgroundColor = Color.Transparent;

            _image = new Image(_desktop, "Color");
            _label = new Label(_desktop)
            {
                Text = "Item"
            };

            this.OnMouseDown += MouseDown;

            this.OnMouseEnter += MouseEnter;
            this.OnMouseExit += MouseExit;
        }
        protected override void After_Invalidated()
        {
            _image.Position = this.Position + new Vector2_Int(0, 20);
            _image.Size = this.Size - new Vector2_Int(0, 20);

            int _xCenter = this.Center.X - Vector2_Int.FromVec2(_label.FontBrush.HalfFontSize).X;

            _label.Position = new Vector2_Int(_xCenter, this.Position.Y);
        }
        protected override void After_Process()
        {
            _label.Process();
            _image.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _image.Render(_spritebatch);
        }
        private void MouseDown(EditorUI_DX.Utils.MouseEventArgs e)
        {
            _actionCallBack?.Invoke(this);
        }

        private void MouseEnter()
        {
            this.Position -= new Vector2_Int(5,5);
           this.Size += new Vector2_Int(10,10);
        }
        private void MouseExit()
        {
            this.Position += new Vector2_Int(5,5);
            this.Size -= new Vector2_Int(10,10);
        }
    }
}


