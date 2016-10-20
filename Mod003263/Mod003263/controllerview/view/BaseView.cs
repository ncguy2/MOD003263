using Mod003263.controllerview.controller;

namespace Mod003263.controllerview.view {
    public abstract class BaseView
    {

        protected BaseController controller;

        public BaseView Pair(BaseController controller) {
            if (controller == null) return this;
            if (controller.Equals(this.controller)) return this;
            this.controller = controller;
            this.controller.Pair(this);
            return this;
        }

    }
}