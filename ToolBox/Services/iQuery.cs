using System.Collections.Generic;

namespace ToolBox.Services
{
    public class iQuery
    {
        public IList<FieldValues> tablesColumns { get; set; }
        public IList<whereFieldValues> whereValues { get; set; }
    }
}
