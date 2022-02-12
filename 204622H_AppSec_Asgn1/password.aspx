<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="password.aspx.cs" Inherits="_204622H_AppSec_Asgn1.password" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <title></title>

<script>
        
       
    
        function validateOldPassword() {
            var str = document.getElementById('<%=tb_oldpwd %>').value;
            if (str == '') {
                document.getElementById("lbl_old").innerHTML = " Field is required";
                document.getElementById("lbl_old").style.color = "Red";
            }

            
                document.getElementById("pwd_checker").innerHTML = " ";         
    }
    function validateNewPassword() {
        var str = document.getElementById('<%=tb_newpwd %>').value;
        if (str == '') {
            document.getElementById("pwd_checker").innerHTML = " Field is required";
            document.getElementById("pwd_checker").style.color = "Red";
        }


        document.getElementById("pwd_checker").innerHTML = " ";
    }
    function validateCNewPassword() {
        var str = document.getElementById('<%=tb_Cnewpwd %>').value;
        var str2 = document.getElementById('<%=tb_newpwd %>').value;
        if (str == '') {
            document.getElementById("lbl_new").innerHTML = " Field is required";
            document.getElementById("lbl_new").style.color = "Red";
        }
        if (str1 != str2) {
            document.getElementById("lbl_new").innerHTML = " Password does not match!";
            document.getElementById("lbl_new").style.color = "Red";
        }


        document.getElementById("pwd_checker").innerHTML = " ";
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <h1 style="text-align:center;">Password Update</h1>
        <asp:Label ID="lbl_error" runat="server" Text="" ></asp:Label>
          <table align="center">
                <tr colspan ="2">
                    <td>old Password: </td>
                    <td><asp:TextBox class="tb_password" ID="tb_oldpwd" runat="server" Height="35px" Width="260px" onkeyup="javascript:validatePassword()" autocomplete="off" TextMode="Email"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_old" runat="server" Text="" ></asp:Label></td>
                </tr>
                <tr>
                    <td>New Password: </td>
                    <td><asp:TextBox class="tb_password" ID="tb_newpwd" runat="server" Height="35px" Width="260px" onkeyup="javascript:validatePassword()" autocomplete="off" TextMode="Password"></asp:TextBox></td>
                     <td><asp:Label ID="pwd_checker" runat="server" Text="" ></asp:Label></td>
                </tr>
               <tr>
                    <td>Confirm New Password: </td>
                    <td><asp:TextBox class="tb_password" ID="tb_Cnewpwd" runat="server" Height="35px" Width="260px" onkeyup="javascript:validatePassword()" autocomplete="off" TextMode="Password"></asp:TextBox></td>
                     <td><asp:Label ID="lbl_new" runat="server" Text="" ></asp:Label></td>
                </tr>

              <tr>
                <td><asp:Button ID="Submit" runat="server" Text="Update Password" Width="213px" OnClick="btn_Change" ></asp:Button></td>
                 
                 
                </tr>
              
              </table>
    
               
    </form>
</body>
</html>
