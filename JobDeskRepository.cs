﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace asmTimex
{
    public class JobDeskRepository
    {
        public String JobDate { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public String JobDeskName { get; set; }
        public String TotalTime { get; set; }
        public String Timespan { get; set; }
    }
}
