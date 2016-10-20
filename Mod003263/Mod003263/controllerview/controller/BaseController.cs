using Mod003263.controllerview.view;

namespace Mod003263.controllerview.controller {
    public abstract class BaseController {

        protected BaseView view;

        public BaseController Pair(BaseView view) {
            if (view == null) return this;
            if (view.Equals(this.view)) return this;
            this.view = view;
            this.view.Pair(this);
            return this;
        }

    }
}