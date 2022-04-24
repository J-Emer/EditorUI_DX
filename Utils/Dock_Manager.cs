using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using EditorUI_DX;
using EditorUI_DX.Brushes;
using EditorUI_DX.Controls;
using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;


namespace EditorUI_DX.Utils
{
    public class Dock_Manager
    {
        public void Do_Dock(Rectangle _parentRect, Element _element)
        {
            int xP = 0;
            int yP = 0;
            int sX = 0;
            int sY = 0;

            if(_element.DockStyle == DockStyle.Left)
            {
                xP = _parentRect.X;
                yP = _parentRect.Y;
                sX = _element.Size.X;
                sY = _parentRect.Height;
            }
            if(_element.DockStyle == DockStyle.Top)
            {
                xP = _parentRect.X;
                yP = _parentRect.Y;
                sX = _parentRect.Width;
                sY = _element.Size.Y;
            }
            if(_element.DockStyle == DockStyle.Right)
            {
                xP = _parentRect.Right - _element.Size.X;
                yP = _parentRect.Y;
                sX = _element.Size.X;
                sY = _parentRect.Height;
            }
            if(_element.DockStyle == DockStyle.Bottom)
            {
                xP = _parentRect.X;
                yP = _parentRect.Bottom - _element.Size.Y;
                sX = _parentRect.Width;
                sY = _element.Size.Y;;
            }
            if(_element.DockStyle == DockStyle.Center)
            {
                xP = _parentRect.X;
                yP = _parentRect.Y;
                sX = _parentRect.Width;
                sY = _parentRect.Height;
            }

            _element.Position = new Vector2_Int(xP, yP);
            _element.Size = new Vector2_Int(sX, sY);
        }

        //returns the Parents avaliable dock area....as a rectangle
        public Rectangle SubtractRect(DockStyle _dockStyle, Rectangle _parent, Rectangle _child)
        {
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;

            if(_dockStyle == DockStyle.Left)
            {
                x = _child.X + _child.Width;
                y = _parent.Y;
                w = _parent.Width - _child.Width;
                h = _parent.Height;
            }

            if(_dockStyle == DockStyle.Right)
            {
                x = _parent.X;
                y = _parent.Y;
                w = _parent.Width - _child.Width;
                h = _parent.Height;
            }

            if(_dockStyle == DockStyle.Top)
            {
                x = _parent.X;
                y = _parent.Y + _child.Height;
                w = _parent.Width;
                h = _parent.Height - _child.Height;
            }

            if(_dockStyle == DockStyle.Bottom)
            {
                x = _parent.X;
                y = _parent.Y;
                w = _parent.Width;
                h = _parent.Height - _child.Height;
            }

            return new Rectangle(x,y,w,h);
        }
    }
}
