using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace asmTimex
{
    public partial class ButtonUX : Component
    {
        public ButtonUX()
        {
            InitializeComponent();
        }

        public ButtonUX(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
