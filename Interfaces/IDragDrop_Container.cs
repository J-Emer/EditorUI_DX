using System;
using System.Windows.Forms;


namespace EditorUI_DX.Interfaces
{
    public interface IDragDrop_Container
    {
        public event Action<object, DragEventArgs> OnDragDrop;
    }
}


