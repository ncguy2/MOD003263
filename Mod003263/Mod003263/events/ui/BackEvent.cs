using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 *  Author: Ryan Cowell
 *  Date: 28/11/2016
 *  Contains: BackEvent, BackListener
 */

namespace Mod003263.events.ui
{
    public class BackEvent : AbstractEvent {

        public interface BackListener {
            [Event]
            void OnBack(BackEvent e);
        }

    }
}
