using Mod003263.controllerview.controller;

/**
 *  Author: Nick Guy
 *  Date: 20/10/2016
 *  Classes: BaseView
 */
namespace Mod003263.controllerview.view {
    /// <summary>
    /// An abstract foundation for the UI forms
    /// </summary>
    public abstract class BaseView {

        protected BaseController controller;

        /// <summary>
        /// Pairs the invoking view with the supplied controller
        /// </summary>
        /// <param name="controller">The controller to be paired with</param>
        /// <returns>The invoking view instance, for method chaining</returns>
        public BaseView Pair(BaseController controller) {
            if (controller == null) return this;
            if (controller.Equals(this.controller)) return this;
            this.controller = controller;
            this.controller.Pair(this);
            return this;
        }

    }
}