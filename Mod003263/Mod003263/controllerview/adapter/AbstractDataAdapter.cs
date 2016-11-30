
/**
 * Author: Nick Guy
 * Date: 07/11/2016
 * Contains: AbstractDataAdapter
 */

using System.Windows;

namespace Mod003263.controllerview.adapter {

    /// <summary>
    /// Generic implementation for a dataset/UI adapter
    /// </summary>
    /// <typeparam name="T">The data type for adaption</typeparam>
    public abstract class AbstractDataAdapter<T, U> where U:UIElement {

        public abstract T GetFromUI(U element);
        public abstract U PushToUI(T data);

    }
}