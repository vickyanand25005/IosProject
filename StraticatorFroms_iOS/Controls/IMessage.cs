using System;
using System.Collections.Generic;
using System.Text;

namespace StraticatorFroms_iOS.Controls
{
    public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
