using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CCTVClient
{
    public class ContextPanel:Panel
    {
        public MainPage parent;
        public String type;
        public ContextPanel()
        {  
        }

        public virtual void updateData()
        {
            //Nothing, this should be overridden in child classes.
        }
    }
}
