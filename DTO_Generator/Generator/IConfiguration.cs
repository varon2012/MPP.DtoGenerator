﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public interface IConfiguration
    {
        int MaxTaskCount { get; }
        string Namespace { get; }
    }
}
