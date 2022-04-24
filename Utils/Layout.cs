using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Controls;
using EditorUI_DX.Utils;

namespace EditorUI_DX.Utils
{
    public class Layout
    {
        public virtual void Handle_Layout(IList<Control> _controls, Rectangle _parentRect, Padding _padding){}
    }

    /// <summary>
    /// Lays out child controls horizontally, does NOT adjust size of child control
    /// </summary>
    public class Horizontal_Layout : Layout
    {
        public override void Handle_Layout(IList<Control> _controls, Rectangle _parentRect, Padding _padding)
        {
            int xPos = _parentRect.Left + _padding.Left;

            foreach (var item in _controls)
            {
                item.Position = new Vector2_Int(xPos, _parentRect.Y);
                item.Size = new Vector2_Int(item.Size.X, _parentRect.Height);
                xPos += item.Size.X + _padding.Left;
            }
        }
    }
    
    /// <summary>
    /// Lays out child controls vertically, does NOT adjust size of child control
    /// </summary>
    public class Vertical_Layout : Layout
    {
        public override void Handle_Layout(IList<Control> _controls, Rectangle _parentRect, Padding _padding)
        {
            int yPos = _parentRect.Top + _padding.Top;

            foreach (var item in _controls)
            {
                item.Position = new Vector2_Int((_parentRect.X + _padding.Left), yPos);
                yPos += (item.Size.Y + _padding.Top);
            }
        }
    }
    
    /// <summary>
    /// Layout child controls vertically, DOES adjust size of child control
    /// </summary>
    public class Vertical_Stretch_Layout : Layout
    {
        public override void Handle_Layout(IList<Control> _controls, Rectangle _parentRect, Padding _padding)
        {
            int yPos = _parentRect.Top + _padding.Top;

            foreach (var item in _controls)
            {
                item.Position = new Vector2_Int((_parentRect.X + _padding.Left), yPos);
                int _sizeX = _parentRect.Width - (_padding.Left + _padding.Right);
                item.Size = new Vector2_Int(_sizeX, item.Size.Y);
                yPos += (item.Size.Y + _padding.Top);
            }
        }
    }
    
    /// <summary>
    /// Layout child controls horizontally, DOES adjust size of child control
    /// </summary>
    public class Horizontal_Stretch_Layout : Layout
    {
        public override void Handle_Layout(IList<Control> _controls, Rectangle _parentRect, Padding _padding)
        {
            int xPos = _parentRect.X + _padding.Left;
            int yPos = _parentRect.Y + _padding.Top;

            int ySize = _parentRect.Height - (_padding.Top + _padding.Bottom);

            foreach (var item in _controls)
            {
                item.Position = new Vector2_Int(xPos, yPos);

                item.Size = new Vector2_Int(item.Size.X, ySize);

                xPos += item.Size.X + _padding.Right;
            }
        }
    }
    
    /// <summary>
    /// Centers child control in parent, and stretches child control to fit parent's SourceRectangle. Only allows 1 child control
    /// </summary>
    public class Stretch_Layout : Layout
    {
        public override void Handle_Layout(IList<Control> _controls, Rectangle _parentRect, Padding _padding)
        {
            if(_controls.Count > 1)
            {
                throw new Exception("Stetch Layout can only have 1 child Control");
            }

            if(_controls.Count == 0){return;}

            Control _cont = _controls[0];
            _cont.Position = new Vector2_Int(_parentRect.X + _padding.Left, _parentRect.Y + _padding.Top);
            _cont.Size = new Vector2_Int(_parentRect.Width - (_padding.Left + _padding.Right), _parentRect.Height - (_padding.Top + _padding.Bottom));
        }
    }
    
    /// <summary>
    /// Layout child controls into a grid. Specify number of Columbs/Rows and CellSize. Or set AutoSize = true to allow child controls to be autosized
    /// </summary>
    public class Grid_Layout : Layout
    {
        public int Columbs { get; set; } = 1;
        public int Rows { get; set; } = 1;
        public Vector2_Int CellSize { get; set; } = new Vector2_Int(100, 100);
        public bool AutoSize { get; set; } = true;


        public override void Handle_Layout(IList<Control> _controls, Rectangle _parentRect, Padding _padding)
        {
            if (AutoSize)
            {
                int _x = _parentRect.Width / Columbs;
                int _y = _parentRect.Height / Rows;
                CellSize = new Vector2_Int(_x, _y);
            }

            int index = 0;

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columbs; x++)
                {
                    if (index > _controls.Count - 1) { return; }

                    int xPos = (x * CellSize.X) + _parentRect.X + (_padding.Left / 2);
                    int yPos = (y * CellSize.Y) + _parentRect.Y + (_padding.Top / 2);
                    int xSize = CellSize.X - _padding.Left;
                    int ySize = CellSize.Y - _padding.Top;

                    _controls[index].Position = new Vector2_Int(xPos, yPos);
                    _controls[index].Size = new Vector2_Int(xSize, ySize);


                    index += 1;
                }
            }


        }
    }
}
