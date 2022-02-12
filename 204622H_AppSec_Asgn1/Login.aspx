<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="_204622H_AppSec_Asgn1.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render="></script>
    <script type="text/javascript">
        
        function validateEmail() {
            var email = document.getElementById('<%=tb_email.ClientID%>').value

            if (email == '') {
                document.getElementById("lbl_email").innerHTML = " Field is required";
                document.getElementById("lbl_email").style.color = "Red";
            }
            else if (email.search(/[a-zA-Z0-9]+@[a-zA-Z]/) == -1) {
                document.getElementById("lbl_email").innerHTML = " Invalid Email Format";
                document.getElementById("lbl_email").style.color = "Red";
            }
            else {
                document.getElementById("lbl_email").innerHTML = " ";
            }

        }
    
        function validatePassword() {
            var str = document.getElementById('<%=tb_password.ClientID %>').value;
            if (str == '') {
                document.getElementById("pwd_checker").innerHTML = " Field is required";
                document.getElementById("pwd_checker").style.color = "Red";
            }

            
                document.getElementById("pwd_checker").innerHTML = " ";         
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h1 style="text-align:center;">Login</h1>
        <asp:Label ID="lbl_error" runat="server" Text="" ></asp:Label>
          <table align="center">
                <tr colspan ="2">
                    <td>Email: </td>
                    <td><asp:TextBox ID="tb_email" runat="server" Height="35px" Width="260px" onkeyup="javascript:validateEmail()" autocomplete="off" TextMode="Email"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_email" runat="server" Text="" ></asp:Label></td>
                </tr>
                <tr>
                    <td>Password: </td>
                    <td><asp:TextBox ID="tb_password" runat="server" Height="35px" Width="260px" onkeyup="javascript:validatePassword()" autocomplete="off" TextMode="Password"></asp:TextBox></td>
                     <td><asp:Label ID="pwd_checker" runat="server" Text="" ></asp:Label></td>
                </tr>

              <tr>
                   <td><input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" /></td>
              </tr>
              <tr>
                <td><asp:Button ID="Submit" runat="server" Text="Login" Width="213px" OnClick="btn_Login" ></asp:Button></td>
                 
                 
                </tr>
              
              </table>
        <asp:Label ID="lbl_gScore" runat="server" Text="Json message: " EnableViewState="False" ></asp:Label>
               
    </form>

    <script>
        grecaptcha.ready(function () {

            grecaptcha.execute('', { action: 'Login' }).then(function (token) {
                document.getElementById('g-recaptcha-response').value = token;
            });
        });
    </script>
     
  
</body>
</html>
