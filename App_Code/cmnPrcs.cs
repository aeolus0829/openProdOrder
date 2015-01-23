using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;


namespace nsCmnPrcs
{
    /// <summary>
    /// Summary description for cmn
    /// </summary>
    public class cmnPrcs
    {
        public string convertToDatef(string str)
        {
            string sY, sM, sD, sDate;
            sY = str.Substring(0, 4);
            sM = str.Substring(4, 2);
            sD = str.Substring(6, 2);
            sDate = sY + '-' + sM + '-' + sD;
            return sDate;
        }

        public DateTime firstDayOfMonth()
    {
                DateTime FirstDay = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
                return FirstDay;
    }

        public DateTime lastDayOfMonth()
        {
            DateTime LastDay = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);
            return LastDay;
        }

        public string GetSortDirection(string column)
        {
            string sortExpression;
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            if (HttpContext.Current.Session["SortExpression"] != null)
            {
                // Retrieve the last column that was sorted.
                sortExpression = HttpContext.Current.Session["SortExpression"].ToString();
            }
            else sortExpression = null;


            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = HttpContext.Current.Session["SortDirection"].ToString();
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            HttpContext.Current.Session["SortDirection"] = sortDirection;
            HttpContext.Current.Session["SortExpression"] = column;

            return sortDirection;
        }


        public void ExportToExcel(GridView gridView)
        {
            //
            // TODO: Add constructor logic here
            //
            // HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
            string FileName = System.DateTime.Now.ToString("yyyy-MM-dd_HHmm");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
            //HttpContext.Current.Response.AddHeader("content-disposition","attachment;filename=PoolExport.xls");
            HttpContext.Current.Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridView.AllowSorting = false;
            gridView.AllowPaging = false;
            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gridView.EnableViewState = false;
            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;
            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gridView);
            page.RenderControl(htw);
            //HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.Write(sw.ToString().Substring(sw.ToString().IndexOf("<table")));
            HttpContext.Current.Response.End();
        }
    }
}