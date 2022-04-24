using System;
using System.Collections.Generic;
using System.Linq;

using EditorUI_DX.Controls;


namespace EditorUI_DX.Utils
{
    public class Control_Collection<T> where T : Control
    {
              
        private List<T> _controls = new List<T>();

        /// <summary>
        /// Returns the List<T> Controls
        /// </summary>
        public IList<T> Collecton => _controls.AsReadOnly();

        /// <summary>
        /// Returns the List<T> as List<Control>
        /// </summary>
        public IList<Control> Controls => _controls.ToList<Control>().AsReadOnly();

        /// <summary>
        /// Returns the List<T> as List<Element>
        /// </summary>
        public IList<Element> Elements => _controls.ToList<Element>().AsReadOnly();

        /// <summary>
        /// Returns the Controls ordered by ZOrder Ascending
        /// </summary>
        public IOrderedEnumerable<Control> GetByZOrder_Asc => _controls.OrderBy(x => x.ZOrder);
        
        /// <summary>
        /// Retruns Controls ordered by ZOrder Descending
        /// </summary>
        public IOrderedEnumerable<Control> GetByZOrder_Des => _controls.OrderByDescending(x => x.ZOrder);




        /// <summary>
        /// Fires when a controls is Added/Removed
        /// </summary>
        public event Action OnControlsChanged;



        /// <summary>
        /// Adds a Control to the Collection
        /// </summary>
        /// <param name="_control"></param>
        public void Add(T _control)
        {
            _controls.Add(_control);
            OnControlsChanged?.Invoke();
        }

        /// <summary>
        /// Adds a range of Controls to the Collection
        /// </summary>
        /// <param name="_childrenControls">a List<T> Controls: where T is a Control </param>
        public void AddRange(List<T> _childrenControls)
        {
            _controls.AddRange(_childrenControls);
            OnControlsChanged?.Invoke();
        }
        /// <summary>
        /// Removes a Control from the Collection
        /// </summary>
        /// <param name="_control"></param>
        public void Remove(T _control)
        {
            _controls.Remove(_control);
            OnControlsChanged?.Invoke();
        }

        /// <summary>
        /// Returns a Control based on its Name property
        /// </summary>
        /// <param name="_name">the Name of the Control</param>
        public T Get(string _name)
        {
            return _controls.FirstOrDefault(x => x.Name == _name);
        }

        /// <summary>
        /// Clears all Controls from this Collection
        /// </summary>
        public void Clear()
        {
            _controls.Clear();
        }

    }


}
