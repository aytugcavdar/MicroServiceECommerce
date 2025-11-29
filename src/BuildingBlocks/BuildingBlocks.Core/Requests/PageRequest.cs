using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Core.Requests;

public class PageRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
