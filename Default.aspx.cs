using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using nsTblPrcs;
using nsCmnPrcs;

public partial class _Default : System.Web.UI.Page
{
    tblPrcs tp = new tblPrcs();
    cmnPrcs cp = new cmnPrcs();
     string D_status;
   

    protected void Page_Load(object sender, EventArgs e)
    {
        sapReportPrms sapReportPrms = new sapReportPrms();

        string[] ALL = sapReportPrms.SQL();
        D_status = ALL[4];

        //True or False
        if (D_status == "False")
        {
            //目前程式停用中，請連絡資訊組
            Response.Redirect("sapReport_false.aspx");
        }
        else {
            if (!IsPostBack) clrBtn();
        }     
    }
   
    protected void btnClr_Click(object sender, EventArgs e)
    {
        clrBtn();
    }
    protected void clrBtn()
    {
        txtBpmBeginS.Text="";
        txtBpmBeginE.Text = "";
        txtBpmEndE.Text = "";
        txtBpmEndS.Text = "";
        txtBpmNo.Text="";
        txtBpmPo.Text="";
        txtMatnr.Text="";
        txtVndrNm.Text = "";
        ddlQA.SelectedIndex = 0;
        rdblQA.SelectedIndex = 0;
        ddlMvt.SelectedIndex = 0;
        Session.Abandon();

    }
    protected void btnSubmt_Click(object sender, EventArgs e)
    {
        
        if (txtBpmBeginS.Text != "") Session["sqlFltr"] += " AND BEGIN_TIME>='" + cp.convertToDatef(txtBpmBeginS.Text.Trim()) + " 00:00'";
        if (txtBpmBeginE.Text != "")
        { Session["sqlFltr"] += " AND BEGIN_TIME<='" + cp.convertToDatef(txtBpmBeginE.Text.Trim()) + " 23:59'"; }
        else txtBpmBeginE.Text = txtBpmBeginS.Text;
        if (txtBpmEndS.Text != "") Session["sqlFltr"] += " AND END_TIME>='" + cp.convertToDatef(txtBpmEndS.Text.Trim()) + " 00:00'";
        if (txtBpmEndE.Text != "")
        { Session["sqlFltr"] += " AND END_TIME<='" + cp.convertToDatef(txtBpmEndE.Text.Trim()) + " 23:59'"; }
        else txtBpmEndE.Text = txtBpmEndS.Text;
        if (txtBpmNo.Text != "") Session["sqlFltr"] += " AND DOC_NBR LIKE '%" + txtBpmNo.Text.Trim() + "%'";
        if (txtBpmPo.Text != "") Session["sqlFltr"] += " AND PO LIKE '%" + txtBpmPo.Text.Trim() + "%'";
        if (txtMtrlDocNbr.Text != "") Session["sqlFltr"] += " AND MTRLDOC LIKE '%" + txtMtrlDocNbr.Text.Trim() + "%'";
        if (txtOrdMtrl.Text != "") Session["sqlFltr"] += " AND ORD_MATERIAL LIKE '%" +  txtMatnr.Text.Trim() + "%'";
        if (txtMatnr.Text!="") Session["sqlFltr"] += " AND MATERIAL like '%"+ txtMatnr.Text.Trim()+"%'";
        if (txtVndrNm.Text != "") Session["sqlFltr"] += " AND VENDOR_NAME like '%" + txtVndrNm.Text.Trim() + "%'";
        if (ddlMvt.SelectedIndex != 0) Session["sqlFltr"] += " AND MOVE_TYPE='"+ddlMvt.SelectedValue+"'";
        if (rdblQA.SelectedIndex != 0) Session["sqlFltr"] += " AND QAresult ='" + rdblQA.SelectedValue + "'";
        if (rbSample.SelectedValue == "Y") Session["sqlFltr"] += " AND RDpersen <>''";
        if (ddlQA.SelectedIndex != 0) Session["sqlFltr"] += " AND " + ddlQA.SelectedValue + "";


        /*
        switch (ddlQA.SelectedIndex)
        { 
            case 0:
                break;
            case 1:
                Session["sqlFltr"] += " AND task_status=1";
                break;
            case 2:
                Session["sqlFltr"] += " AND task_status=2";
                break;
            case 3:
                Session["sqlFltr"] += " AND excpMit='特採中'";
                break;
        } */

        Response.Redirect("result.aspx");

    }

    protected void ddlStorelocation_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void txtBpmBeginE_TextChanged(object sender, EventArgs e)
    {
        
    }
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        
    }
    protected void rdblQA_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}