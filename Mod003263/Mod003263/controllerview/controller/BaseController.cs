using Mod003263.controllerview.view;

/**
 *  Author: Nick Guy
 *  Date: 20/10/2016
 *  Classes: BaseController
 */
namespace Mod003263.controllerview.controller {
    /// <summary>
    /// An abstract foundation for the view controllers
    /// </summary>
    public abstract class BaseController {

        protected BaseView view;

        /// <summary>
        /// Pairs the invoking controller with the supplied view
        /// </summary>
        /// <param name="view">The view to be paired with</param>
        /// <returns>The invoking controller instance, for method chaining</returns>
        public BaseController Pair(BaseView view) {
            if (view == null) return this;
            if (view.Equals(this.view)) return this;
            this.view = view;
            this.view.Pair(this);
            return this;
        }

    }
}