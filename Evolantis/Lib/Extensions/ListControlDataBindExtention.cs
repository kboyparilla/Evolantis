using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.UI.WebControls;

namespace Evolantis.Lib.Extensions
{
    public static class ListControlDataBindExtention
    {
        public static void BindData(this Repeater Control, object DataSource)
        {
            Control.DataSource = DataSource;
            Control.DataBind();
        }

        public static void BindData(this ListControl Control, object DataSource, string DataTextField, string DataValueField)
        {
            Control.DataSource = DataSource;
            Control.DataTextField = DataTextField;
            Control.DataValueField = DataValueField;
            Control.DataBind();
        }
    }
}
