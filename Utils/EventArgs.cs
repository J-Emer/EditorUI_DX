using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace EditorUI_DX.Utils
{
    public abstract class EventArgs
    {
        public bool Cancel{get;set;} = false;
    }
    public class MouseEventArgs: EventArgs
    {
        public MouseButtons MouseButton{get;set;}
        public MouseButtonState MouseButtonState{get;set;}
        public Vector2 MousePosition{get;set;}


        public override string ToString()
        {
            return $"MouseButton: {MouseButton} | MouseButtonState: {MouseButtonState} | MousePosition: {MousePosition}";
        }
    }
}


