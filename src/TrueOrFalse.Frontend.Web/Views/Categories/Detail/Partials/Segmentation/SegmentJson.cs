﻿using System;
using System.Collections.Generic;
using System.Linq;

public class CustomSegmentJson { 
    public List<SegmentJson> Segments { get; set; }
}

public class SegmentJson
{
    public string Title { get; set; }
    public int[] CategoryIds { get; set; }
}






