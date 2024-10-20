using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Models;

public class UndoRequest
{
    public ProductRequest RequestedProduct { get; set; } = null!;

    public int Action { get; set; }
}
