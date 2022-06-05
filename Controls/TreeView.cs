using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;

namespace EditorUI_DX.Controls
{
    public class TreeView : Control, IControl_Container<TreeNode>
    {
        public event Action<object, TreeNode> OnNodeSelected;
        public TreeNode SelectedNode { get; private set; }

        public Control_Collection<TreeNode> Controls { get; set; } = new Control_Collection<TreeNode>();
        public Color NodeNormalColor { get; set; } = Color.Transparent;
        public Color NodeHighlightColor { get; set; } = new Color(39, 126, 242);
        public Color NodeFontColor { get; set; } = Color.White;
        public Color ToggleButtonColor { get; set; } = Color.Transparent;
        public Color ToggleButtonHighlightColor { get; set; } = new Color(39, 126, 242);



        private TreeView_ScrollRect _scrollRect;
        private Padding _padding;
        private string _fontName;

        public TreeView(Desktop _desktop, string _fontName) : base(_desktop)
        {
            this.BackgroundColor = new Color(57, 60, 64);
            this._fontName = _fontName;

            _padding = new Padding(5);
            _scrollRect = new TreeView_ScrollRect();


            this.Controls.OnControlsChanged += After_Invalidated;

            this.OnScrollWheel += Scroll;
        }
        /// <summary>
        /// Adds a Parent Tree Node that other TreeNodes can be the children of
        /// </summary>
        /// <param name="_text">The text the TreeNode will display</param>
        /// <returns>TreeNode</returns>
        public TreeNode AddParent(string _text, object _tag = null)
        {
            TreeNode _node = new TreeNode(this._desktop, _fontName, After_Invalidated, NodeCallBack, this)
            {
                Name = $"Node_{_text}",
                Text = _text,
                Size = new Vector2_Int(150, 25),
                Tag = _tag,
                BackgroundColor = NodeNormalColor,
                NodeNormalColor = NodeNormalColor,
                NodeHighlightColor = NodeHighlightColor,
                FontColor = NodeFontColor,
                ToggleButtonBackgroundColor = ToggleButtonColor,
                ToggleButtonHighlightColor = ToggleButtonHighlightColor,
                ToggleButtonNormal = ToggleButtonColor,
            };

            Controls.Add(_node);

            return _node;
        }

        /// <summary>
        /// Adds a Child TreeNode to a Parent TreeNode
        /// </summary>
        /// <param name="_parent">The Parent TreeNode</param>
        /// <param name="_text">The text the ChildTreeNode will dispaly</param>
        public void AddChild(TreeNode _parent, string _text, object _tag = null)
        {
            TreeNode _node = new TreeNode(this._desktop, _fontName, After_Invalidated, NodeCallBack, this, _parent)
            {
                Name = $"Node_{_text}",
                Text = _text,
                Size = new Vector2_Int(150, 25),
                Tag = _tag,
                BackgroundColor = NodeNormalColor,
                NodeNormalColor = NodeNormalColor,
                NodeHighlightColor = NodeHighlightColor,
                FontColor = NodeFontColor,
                ToggleButtonBackgroundColor = ToggleButtonColor,
                ToggleButtonHighlightColor = ToggleButtonHighlightColor,
                ToggleButtonNormal = ToggleButtonColor,
            };

            _parent.Controls.Add(_node);
        }
        private void Scroll(object sender, float dir)
        {
            _scrollRect.Scroll(dir, Controls.Collecton, _padding);
            Do_Layout();
        }
        private void Do_Layout()
        {
            int yPos = _scrollRect.Rectangle.Y + _padding.Top;

            foreach (var Parent in Controls.Collecton)
            {
                Parent.Position = new Vector2_Int(this.Position.X, yPos);
                Parent.Size = new Vector2_Int(this.Size.X, Parent.Size.Y);

                yPos += Parent.Size.Y + _padding.Top;

                if (Parent.ShowChildren)
                {
                    foreach (var Child in Parent.Controls.Collecton)
                    {
                        Child.Position = new Vector2_Int(this.Position.X + 30, yPos);
                        Parent.Size = new Vector2_Int(this.Size.X - 30, Child.Size.Y);

                        yPos += Child.Size.Y + _padding.Top;
                    }
                }
            }
        }
        protected override void After_Invalidated()
        {
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
        private void NodeCallBack(TreeNode _node)
        {
            SelectedNode = _node;
            OnNodeSelected?.Invoke(this, SelectedNode);
        }
    }

    public class TreeNode : Control, IControl_Container<TreeNode>
    {
        public Control_Collection<TreeNode> Controls { get; set; } = new Control_Collection<TreeNode>();
        public bool ShowChildren
        {
            get
            {
                return _showChildren;
            }
        }
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
                return _label.Text;
            }
            set
            {
                _label.Text = value;
            }
        }
        public Color NodeNormalColor { get; set; } = Color.Transparent;
        public Color NodeHighlightColor { get; set; } = new Color(39, 126, 242);
        public Color NodeFontColor { get; set; } = Color.White;
        public Color ToggleButtonBackgroundColor
        {
            get
            {
                return _button.BackgroundColor;
            }
            set
            {
                _button.BackgroundColor = value;
            }
        }
        public Color ToggleButtonNormal
        {
            get
            {
                return _button.NormalColor;
            }
            set
            {
                _button.NormalColor = value;
            }
        }
        public Color ToggleButtonHighlightColor
        {
            get
            {
                return _button.HighlightColor;
            }
            set
            {
                _button.HighlightColor = value;
            }
        }
        public string Path
        {
            get
            {
                string _path = "";

                if (_parentNode != null)
                {
                    _path += _parentNode.Text + @"\";
                }

                _path += this.Text;
                return _path;
            }
        }

        private bool _showChildren = true;
        private Label _label;
        private Button _button;


        private Action _toggleChildrenCallBack;
        private Action<TreeNode> _nodeClickedCallBack;
        private TreeNode _parentNode;
        private TreeView _parentTree;

        public TreeNode(Desktop _desktop, string _fontName, Action _toggleChildrenCallBack, Action<TreeNode> _nodeClickedCallBack, TreeView _parentTree, TreeNode _parentNode = null) : base(_desktop)
        {
            this._parentTree = _parentTree;
            this._parentNode = _parentNode;
            this._nodeClickedCallBack = _nodeClickedCallBack;
            this._toggleChildrenCallBack = _toggleChildrenCallBack;

            _label = new Label(_desktop);
            _button = new Button(_desktop)
            {
                Text = "v"
            };

            _button.OnMouseDown += Button_MouseDown;

            this.Size = new Vector2_Int(150, 30);
            this.OnMouseEnter += Node_MouseEnter;
            this.OnMouseExit += Node_MouseExit;
            this.OnMouseDown += Node_MouseDown;
        }
        protected override void After_Invalidated()
        {
            _button.IsActive = Controls.Collecton.Count > 0;
            _button.Position = this.Position;
            _button.Size = new Vector2_Int(this.Size.Y, this.Size.Y);

            Vector2_Int _center = this.Center - Vector2_Int.FromVec2(_label.FontBrush.HalfFontSize);
            _label.Position = new Vector2_Int(this.Position.X + 35, _center.Y);
        }
        protected override void After_Process()
        {
            _label.Process();
            _button.Process();

            foreach (var child in Controls.Collecton)
            {
                child.Process();
            }

        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _button.Render(_spritebatch);

            foreach (var child in Controls.Collecton)
            {
                child.Render(_spritebatch);
            }
        }



        private void Button_MouseDown(EditorUI_DX.Utils.EventArgs e)
        {
            _showChildren = !_showChildren;

            if (_showChildren)
            {
                _button.Text = "v";
            }
            else
            {
                _button.Text = ">";
            }

            foreach (var child in Controls.Collecton)
            {
                child.IsActive = _showChildren;
            }

            _toggleChildrenCallBack?.Invoke();
        }

        private void Node_MouseEnter()
        {
            BackgroundColor = NodeHighlightColor;
        }
        private void Node_MouseExit()
        {
            BackgroundColor = NodeNormalColor;
        }
        private void Node_MouseDown(EditorUI_DX.Utils.MouseEventArgs e)
        {
            _nodeClickedCallBack?.Invoke(this);
        }
    }
}
