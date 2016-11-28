using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod003263.events.ui
{
    public class BackEvent : AbstractEvent {

        public interface BackListener {
            [Event]
            void OnBack(BackEvent e);
        }

    }
}
