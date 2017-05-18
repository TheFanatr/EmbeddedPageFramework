using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EmbeddedPageFramework
{
    public class EmbeddedPage : Grid
    {
        private static int NextIdentifier = 0;

        public int UniqueIdentifier { get; private set; } = NextIdentifier;

        public int Identifier { get; set; } = NextIdentifier++;

        public int HistoryIdentifier;
    }
}
