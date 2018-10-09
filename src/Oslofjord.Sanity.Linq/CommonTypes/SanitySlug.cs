﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Oslofjord.Sanity.Linq.CommonTypes
{
    public class SanitySlug : SanityObject
    {
        public SanitySlug(string current) : base()
        {
            Current = current;
        }

        public string Current { get; set; }
        
    }
}
