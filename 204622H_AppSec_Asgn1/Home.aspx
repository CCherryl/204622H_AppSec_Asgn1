<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="_204622H_AppSec_Asgn1.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            End me session pls :)
            <a href="updatePassword">Change your password here</a>
                <asp:Button ID="Submit" runat="server" Text="Log out" Width="213px" OnClick="btn_LogOut" ></asp:Button>


        </div>
    </form>
</body>
</html>
