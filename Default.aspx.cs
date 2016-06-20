using System;
using nsTblPrcs;
using nsCmnPrcs;

public partial class _Default : System.Web.UI.Page
{
    tblPrcs tp = new tblPrcs();
    cmnPrcs cp = new cmnPrcs();
    bool isFormEnabled = true;   

    protected void Page_Load(object sender, EventArgs e)
    {
        //True or False
        if (! isFormEnabled)
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
        string today = DateTime.Today.ToShortDateString();

        if (txtBpmBeginS.Text != "") Session["dateFltr"] = " AND BEGIN_TIME>='" + cp.convertToDatef(txtBpmBeginS.Text.Trim()) + " 00:00'";
        else Session["dateFltr"] += " AND BEGIN_TIME>='" + today + " 00:00'";

        if (txtBpmBeginE.Text != "")  Session["dateFltr"] += " AND BEGIN_TIME<='" + cp.convertToDatef(txtBpmBeginE.Text.Trim()) + " 23:59'";
        else txtBpmBeginE.Text = txtBpmBeginS.Text;

        if (txtBpmEndS.Text != "") Session["dateFltr"] += " AND END_TIME>='" + cp.convertToDatef(txtBpmEndS.Text.Trim()) + " 00:00'";
        else Session["dateFltr"] += " AND END_TIME>='" + today + " 00:00'";

        if (txtBpmEndE.Text != "") Session["dateFltr"] += " AND END_TIME<='" + cp.convertToDatef(txtBpmEndE.Text.Trim()) + " 23:59'"; 
        else txtBpmEndE.Text = txtBpmEndS.Text;

        if (txtBpmNo.Text != "") Session["ftr"] = " AND DOC_NBR LIKE '%" + txtBpmNo.Text.Trim() + "%'";
        if (txtBpmPo.Text != "") Session["ftr"] += " AND PO LIKE '%" + txtBpmPo.Text.Trim() + "%'";
        if (txtMtrlDocNbr.Text != "") Session["ftr"] += " AND MTRLDOC LIKE '%" + txtMtrlDocNbr.Text.Trim() + "%'";
        if (txtOrdMtrl.Text != "") Session["ftr"] += " AND ORD_MATERIAL LIKE '%" + txtOrdMtrl.Text.Trim() + "%'";
        if (txtMatnr.Text != "") Session["ftr"] += " AND MATERIAL like '%" + txtMatnr.Text.Trim() + "%'";
        if (txtVndrNm.Text != "") Session["ftr"] += " AND VENDOR_NAME like '%" + txtVndrNm.Text.Trim() + "%'";
        if (ddlMvt.SelectedIndex != 0) Session["ftr"] += " AND MOVE_TYPE='" + ddlMvt.SelectedValue + "'";
        if (rdblQA.SelectedIndex != 0) Session["ftr"] += " AND QAresult ='" + rdblQA.SelectedValue + "'";
        if (rbSample.SelectedValue == "Y") Session["ftr"] += " AND RDpersen <>''";
        if (ddlQA.SelectedIndex != 0) Session["ftr"] += " AND '" + ddlQA.SelectedValue + "'";

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