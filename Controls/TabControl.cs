using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;


namespace EditorUI_DX.Controls
{
    public class TabControl : Control, IControl_Container<TabPage>
    {

        public event Action<object, TabPage> OnTabSelectionChanged;
        public int SelectedIndex{get; private set;}
        public TabPage SelectedPage
        {
            get
            {
                return Controls.Collecton[SelectedIndex];
            }
        }



        public Control_Collection<TabPage> Controls{get;set;} = new Control_Collection<TabPage>();


        public Color ButtonFontColor{get;set;} = Color.White;
        public Color ButtonNormalColor{get;set;} = Color.Transparent;
        public Color ButtonHighlightColor{get;set;} = new Color(39, 126, 242);
        public Color ButtonSelectedColor{get;set;} = new Color(148, 181, 235);
        public Color PanelBackgroundColor{get;set;} = Color.Transparent;







        
        public TabControl(Desktop _desktop) : base(_desktop)
        {
            Controls.OnControlsChanged += After_Invalidated;

            this.BackgroundColor = new Color(57, 60, 64);
            this.Size = new Vector2_Int(200,200);
        }

        /// <summary>
        /// Adds a TabPage to the TabControl
        /// </summary>
        /// <param name="_text">The text to display on the TabPage Button</param>
        /// <param name="_layout">The Layout the TabPage will have</param>
        /// <param name="_controls">List of child controls that will be placed on the TabPage. If no controls are needed pass in an empty list of Control></param>
        /// <returns>TabPage</returns>
        public TabPage Add(string _text, Layout _layout, List<Control> _controls)
        {
            TabPage _page = new TabPage(this._desktop, Button_Callback);
            _page.Name = _text;
            
            _page.Button.FontColor = ButtonFontColor;
            _page.Button.BackgroundColor = ButtonNormalColor;
            _page.Button.NormalColor = ButtonNormalColor;
            _page.Button.HighlightColor = ButtonHighlightColor;
            _page.ButtonSelectedColor = ButtonSelectedColor;
            _page.Button.Text = _text;

            _page.Panel.BackgroundColor = PanelBackgroundColor;
            _page.Panel.Layout = _layout;
            _page.Controls.AddRange(_controls);

            Controls.Add(_page);

            return _page;
        }
        protected override void After_Invalidated()
        {
            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].Index = i;
                Controls.Collecton[i].Handle_Pos(this);
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
        private void Button_Callback(int _index)
        {
            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].Panel.IsActive = false;
            }

            Controls.Collecton[_index].Panel.IsActive = true;

            SelectedIndex = _index;
            OnTabSelectionChanged?.Invoke(this, SelectedPage);
        }
    }

    public class TabPage : Control
    {
        
        public Color ButtonFontColor
        {
            get
            {
                return Button.FontColor;
            }
            set
            {
                Button.FontColor = value;
            }
        }
        public Color ButtonSelectedColor;
        public Color ButtonNormalColor
        {
            get
            {
                return Button.NormalColor;
            }
            set
            {
                Button.NormalColor = value;
            }
        }
        public Color ButtonHighlightColor
        {
            get
            {
                return Button.HighlightColor;
            }
            set
            {
                Button.HighlightColor = value;
            }
        }
        public Color PanelBackgroundColor
        {
            get
            {
                return Panel.BackgroundColor;
            }
            set
            {
                Panel.BackgroundColor = value;
            }
        }




        public int Index{get;set;}
        public Button Button{get; private set;}
        public Panel Panel{get; private set;}       
        public Color PageBackgroundColor
        {
            get
            {
                return Panel.BackgroundColor;
            }
            set
            {
                Panel.BackgroundColor = value;
            }
        }
        public Control_Collection<Control> Controls
        {
            get
            {
                return Panel.Controls;
            }
        }
        private Action<int> CallBack;

        public TabPage(Desktop _desktop, Action<int> _callBack) : base(_desktop)
        {
            this.CallBack = _callBack;

            Button = new Button(_desktop)
            {
                Text = "button",
                Size = new Vector2_Int(100,30)
            };

            Button.OnMouseDown += Button_MouseDown;

            Panel = new Panel(_desktop)
            {
                BackgroundColor = Color.Orange
            };
        }
        
        /// <summary>
        /// Called by the Parent TabControl
        /// </summary>
        public void Handle_Pos(TabControl _parent)
        {
            int xOffset = Index * 100;
            Button.Position = new Vector2_Int(xOffset, 0) + _parent.Position;

            int ySize = _parent.Size.Y - 30;
            Panel.Position = _parent.Position + new Vector2_Int(0, 30);
            Panel.Size = new Vector2_Int(_parent.Size.X, ySize);
        }
        protected override void After_Process()
        {
            Button.Process();
            Panel.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            Button.Render(_spritebatch);
            Panel.Render(_spritebatch);
        }
        private void Button_MouseDown(EditorUI_DX.Utils.EventArgs e)
        {
            CallBack?.Invoke(this.Index);
        }
    }
}


