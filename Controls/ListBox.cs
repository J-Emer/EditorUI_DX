using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Controls;
using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;
using System.IO;

namespace EditorUI_DX.Controls
{
    public class ListBox : Control, IControl_Container<ListBoxItem>, IDragDrop_Container
    {
        public event Action<object, ListBoxItem> OnListBoxItemSelected;
        public ListBoxItem SelectedItem{get; private set;}
        public int SelectedIndex{get; private set;}


        public Control_Collection<ListBoxItem> Controls{get;set;} = new Control_Collection<ListBoxItem>();
        private Layout _layout;
        private Scroll_Rect _scrollRect;
        private EditorUI_DX.Utils.Padding _padding;


        public Color ItemNormalColor{get;set;} = new Color(57, 60, 64);
        public Color ItemHighlightColor{get;set;} = new Color(39, 126, 242);


        public event Action<object, DragEventArgs> OnDragDrop;


        public ListBox(Desktop _desktop) : base(_desktop)
        {
            this.BackgroundColor = new Color(57, 60, 64);

            _padding = new EditorUI_DX.Utils.Padding(5,5,5,0);

            _scrollRect = new Scroll_Rect();

            _layout = new Vertical_Stretch_Layout();

            Controls.OnControlsChanged += After_Invalidated;

            this.OnScrollWheel += Scroll;

            _desktop.OnDragDrop += DragDrop;
        }

        ~ListBox()
        {
            _desktop.OnDragDrop -= DragDrop;
        }
        private void DragDrop(object sender, DragEventArgs e)
        {
            if(SourceRectangle.Contains(Input.Instance.MousePosition))
            {
                /*OnScreenLog.Instance.Log("---listbox: DragDrop");

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    //do something with the files -> file is the full path to the file that was dropped
                    Add(Path.GetFileName(file));
                }*/

                OnDragDrop?.Invoke(sender, e);
            }
        }






        /// <summary>
        /// Adds a ListBoxItem to this ListBox
        /// </summary>
        /// <param name="_text">The text to display on ListBoxItem</param>
        public void Add(string _text)
        {
            ListBoxItem _item = new ListBoxItem(this._desktop, ListItemClick)
            {
                Name = "ListBoxItem",
                Text = _text,
                Size = new Vector2_Int(150,25),
                BackgroundColor = ItemNormalColor,
                NormalColor = ItemNormalColor,
                HighlightColor = ItemHighlightColor,
                FontColor = Color.White
            };
            
            Controls.Add(_item);
        }
        /// <summary>
        /// Adds a ListBoxItem to this ListBox
        /// </summary>
        /// <param name="_text">The text to display on ListBoxItem</param>
        /// <param name="_tag"></param>
        public void Add(string _text, object _tag)
        {
            ListBoxItem _item = new ListBoxItem(this._desktop, ListItemClick)
            {
                Name = "ListBoxItem",
                Tag = _tag,
                Text = _text,
                Size = new Vector2_Int(150, 20),
                BackgroundColor = ItemNormalColor,
                NormalColor = ItemNormalColor,
                HighlightColor = ItemHighlightColor,
                FontColor = Color.White
            };
            
            Controls.Add(_item);
        }
        /// <summary>
        /// Removes a ListBoxItem from this ListBox
        /// </summary>
        /// <param name="_item"></param>
        public void Remove(ListBoxItem _item)
        {
            Controls.Remove(_item);
        }

        protected override void After_Invalidated()
        {
            //_scrollRect._parentSource = this.SourceRectangle;
            //_scrollRect._rect = this.SourceRectangle;
            _scrollRect.Set(this.SourceRectangle);

            Do_Layout();
        }
        protected override void After_Process()
        {
            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].IsActive = SourceRectangle.Contains(Controls.Collecton[i].SourceRectangle);
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
        private void Scroll(object sender, float dir)
        {
            _scrollRect.Scroll(dir, Controls.Elements, _padding);
            Do_Layout();
        }
        private void Do_Layout()
        {
            _layout.Handle_Layout(Controls.Controls, _scrollRect.Rectangle, _padding);
        }
        private void ListItemClick(ListBoxItem _item)
        {
            SelectedItem = _item;

            int index = 0;

            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                if(Controls.Collecton[i] == _item)
                {
                    index = i;
                    continue;
                }
            }

            OnListBoxItemSelected?.Invoke(this, SelectedItem);
        }
    }

    public class ListBoxItem : Control
    {
        private Label _label;
        public Color FontColor
        {
            get
            {
                return _label.FontBrush.DrawColor;
            }
            set
            {
                _label.FontBrush.DrawColor = value;
            }
        }
        public string Text
        {
            get
            {
                return _label.FontBrush.Text;
            }
            set
            {
                _label.FontBrush.Text = value;
            }
        }
        private Action<ListBoxItem> ClickCallBack;


        public Color NormalColor{get;set;} = new Color(57, 60, 64);
        public Color HighlightColor{get;set;} = new Color(39, 126, 242);


        public ListBoxItem(Desktop _desktop, Action<ListBoxItem> _clickCallBack) : base(_desktop)
        {
            this.ClickCallBack = _clickCallBack;

            _label = new Label(_desktop);
            Text = "ListBoxItem";
            FontColor = Color.Black;

            this.OnMouseDown += MouseDown;
            this.OnMouseEnter += MouseEnter;
            this.OnMouseExit += MouseExit;
        }
        protected override void After_Invalidated()
        {
            Vector2_Int _centered =  this.Center - Vector2_Int.FromVec2(_label.FontBrush.HalfFontSize);
            _label.Position = new Vector2_Int((this.Position.X + 10), _centered.Y);
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
        }
        private void MouseDown(EditorUI_DX.Utils.EventArgs e)
        {
            ClickCallBack?.Invoke(this);
        }
        private void MouseEnter()
        {
            BackgroundColor = HighlightColor;
        }
        private void MouseExit()
        {
            BackgroundColor = NormalColor;
        }

    }
}


