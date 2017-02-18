using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serendipity
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var w = new Window())
                w.Run();
        }
    }
}
