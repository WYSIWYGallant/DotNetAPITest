<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Page2.aspx.cs" Inherits="TelerikWebApp1.Page2" Async="true" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server"></telerik:RadScriptManager>
        <div>
            <telerik:RadGrid ID="RadGrid1" runat="server" PageSize="10" PagerStyle-PageButtonCount="5"
                OnNeedDataSource="RadGrid1_NeedDataSource" RenderMode="Auto">
                <MasterTableView AutoGenerateColumns="False">
                    <Columns>
                        <telerik:GridBoundColumn DataField="PersonName" 
                            UniqueName="PersonName">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="PersonAge"
                            UniqueName="PersonAge">
                            <HeaderStyle Width="150px" />
                        </telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="PersonType" 
                            UniqueName="PersonType">
                            <HeaderStyle Width="150px" />
                        </telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
