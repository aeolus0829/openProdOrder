<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" trace="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 70%;
        }
        .style3
        {
            width: 116px;
        }
        .style5
        {
            width: 122px;
        }
        .auto-style1 {
            width: 219px;
        }
        .auto-style2 {
            width: 122px;
            height: 23px;
        }
        .auto-style3 {
            width: 219px;
            height: 23px;
        }
        .auto-style4 {
            width: 116px;
            height: 23px;
        }
        .auto-style5 {
            height: 23px;
            width: 290px;
        }
        .auto-style6 {
            width: 290px;
        }
        .auto-style7 {
            width: 122px;
            height: 78px;
        }
        .auto-style8 {
            width: 219px;
            height: 78px;
        }
        .auto-style9 {
            width: 116px;
            height: 78px;
        }
        .auto-style10 {
            width: 290px;
            height: 78px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSubmt" visible="True">
    <div>
    
        <h1>
            BPM進料狀況表</h1>
        <table class="style1">
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    BPM起單日-起</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtBpmBeginS" runat="server" TabIndex="1"></asp:TextBox>
                </td>
                <td bgcolor="#99CCFF" class="style3">                    
                    BPM起單日-訖</td>
                <td class="auto-style6">
                    <asp:TextBox ID="txtBpmBeginE" runat="server" 
                        ontextchanged="txtBpmBeginE_TextChanged" TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td bgcolor="#3399FF" class="style5">
                    BPM單號</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtBpmNo" runat="server" TabIndex="5"></asp:TextBox>
                </td>
                <td bgcolor="White" class="style3">
                    &nbsp;</td>
                <td class="auto-style6">                    
                </td>
            </tr>
            <tr>
                <td bgcolor="#3399FF" class="style5">
                    採購單號 %</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtBpmPo" runat="server" TabIndex="6"></asp:TextBox>
                </td>
                <td bgcolor="#3399FF" class="style3">
                    物料文件號碼 %</td>
                <td class="auto-style6">
                    <asp:TextBox ID="txtMtrlDocNbr" runat="server" TabIndex="7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td bgcolor="#3399FF" class="style5">
                    供應商名稱 %</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtVndrNm" runat="server" TabIndex="8"></asp:TextBox>
                </td>
                <td bgcolor="White" class="style3">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td bgcolor="#3399FF" class="style5">
                    物料號碼 %</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtMatnr" runat="server" TabIndex="9"></asp:TextBox>
                </td>
                <td bgcolor="White" class="style3">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td bgcolor="#3399FF" class="style5">
                    工單料號 %</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtOrdMtrl" runat="server" TabIndex="10"></asp:TextBox>
                </td>
                <td bgcolor="White" class="style3">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    單據狀態</td>
                <td class="auto-style1">
                    <asp:DropDownList ID="ddlQA" runat="server" TabIndex="11">
                        <asp:ListItem Selected="True">全部</asp:ListItem>
                        <asp:ListItem Value="task_status='1'">未結案</asp:ListItem>
                        <asp:ListItem Value="excpMit='特採中'">特採中</asp:ListItem>
                        <asp:ListItem Value="task_status='2'">結案</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td bgcolor="#99CCFF" class="style3">
                    樣品</td>
                <td class="auto-style6">
                    <asp:RadioButtonList ID="rbSample" runat="server" RepeatDirection="Horizontal" TabIndex="12">
                        <asp:ListItem Value="Y">是</asp:ListItem>
                        <asp:ListItem Selected="True" Value="N">否</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="auto-style2">
                    異動類型</td>
                <td class="auto-style3">
                    <asp:DropDownList ID="ddlMvt" runat="server" TabIndex="13">
                        <asp:ListItem Selected="True">全部</asp:ListItem>
                        <asp:ListItem>103</asp:ListItem>
                        <asp:ListItem>105</asp:ListItem>
                        <asp:ListItem>104</asp:ListItem>
                        <asp:ListItem>106</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td bgcolor="White" class="auto-style4">
                    </td>
                <td class="auto-style5">
                    &nbsp;</td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="auto-style7">
                    檢驗結果</td>
                <td class="auto-style8">
                    <asp:RadioButtonList ID="rdblQA" runat="server" TabIndex="14" OnSelectedIndexChanged="rdblQA_SelectedIndexChanged">
                        <asp:ListItem Selected="True">全部</asp:ListItem>
                        <asp:ListItem>合格</asp:ListItem>
                        <asp:ListItem>不合格</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td bgcolor="White" class="auto-style9">
                    輸出樣式</td>
                <td class="auto-style10">
                    <asp:RadioButtonList ID="rblStyle" runat="server">
                        <asp:ListItem Selected="True" Value="0">無樣式</asp:ListItem>
                        <asp:ListItem Value="1">印表用</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="style5">
                    &nbsp;</td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="style3">
                    &nbsp;</td>
                <td class="auto-style6">
                    <asp:CheckBox ID="cbCacheMode" runat="server" Text="加速模式" Checked="True" />
                </td>
            </tr>
            <tr>
                <td class="style5">
                    &nbsp;</td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="style3">
                    <asp:Button ID="btnSubmt" runat="server" onclick="btnSubmt_Click" Text="查詢" 
                        style="height: 26px; width: 42px;" TabIndex="50" />
                </td>
                <td class="auto-style6">
                    <asp:Button ID="btnClr" runat="server" onclick="btnClr_Click" Text="清除" 
                        TabIndex="51" />
                </td>
            </tr>
        </table>
    
    </div>
        <ul>
            <li style="color: red; font-weight: bold">某些狀況在加速模式可能會查不到，例：特採、檢驗時間長的物料</li>
            <li>欄位標示為 % 表示輸入部份資料即可</li>
            <li>未輸入任何條件，只會顯示最近三個月的資料</li>
            <li>日期若起訖為同一天，輸入起始日期就好，格式 20131231</li>
            <li>BPM文件日期 2014/7/11後才有檢驗結果及特採狀態可查詢</li>
        </ul>

        <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="/bpm/">回BPM報表畫面</asp:HyperLink>
        <br />
        <asp:Button ID="btnToExcel" runat="server" OnClick="btnToExcel_Click" Text="to Excel" Visible="False" />
        <br />
        <asp:GridView ID="gvResult" runat="server" CellPadding="3" GridLines="Vertical" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px">
            <AlternatingRowStyle BackColor="#DCDCDC" BorderStyle="None" />
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />
        </asp:GridView>
    </form>
</body>
<script src="jquery-3.1.1.js"></script>
</html>
