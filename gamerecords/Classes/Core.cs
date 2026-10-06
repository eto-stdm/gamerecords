using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gamerecords.Models;

namespace gamerecords.Classes
{
    internal class Core
    {
        public static gamerecordsEntities Context = new gamerecordsEntities();
        public static Players current_player;
    }
}
