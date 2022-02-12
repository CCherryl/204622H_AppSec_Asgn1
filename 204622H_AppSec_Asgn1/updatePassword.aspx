<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="updatePassword.aspx.cs" Inherits="_204622H_AppSec_Asgn1.updatePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

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
        if (str.length < 12) {
            document.getElementById("pwd_checker").innerHTML = "Password length must be at least 12 Charaters";
            document.getElementById("pwd_checker").style.color = "Red";
            return ("too_short");
        }
        if (str.search(/[0-9]/) == -1) {
            document.getElementById("pwd_checker").innerHTML = "Password require at least 1 number";
            document.getElementById("pwd_checker").style.color = "Red";
            return ("no_number");
        }
        if (str.search(/[A-Z]/) == -1) {
            document.getElementById("pwd_checker").innerHTML = "Password require at least 1 uppercase";
            document.getElementById("pwd_checker").style.color = "Red";
            return ("no_uppercase");
        }
        if (str.search(/[a-z]/) == -1) {
            document.getElementById("pwd_checker").innerHTML = "Password require at least 1 lowercase";
            document.getElementById("pwd_checker").style.color = "Red";
            return ("no_lowercase");
        }
        if (str.search(/[^a-zA-Z0-9]/) == -1) {
            document.getElementById("pwd_checker").innerHTML = "Password require at least 1 special characters";
            document.getElementById("pwd_checker").style.color = "Red";
            return ("no_specialcharacters");
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


        document.getElementById("lbl_new").innerHTML = " ";
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <h1 style="text-align:center;">Password Update</h1>
        <asp:Label ID="lbl_error" runat="server" Text="" ></asp:Label>
            <asp:Label ID="lbl_Response" runat="server" Text="" ></asp:Label>
          <table align="center">
                <tr colspan ="2">
                    <td>Old Password: </td>
                    <td><asp:TextBox  ID="tb_oldpwd" runat="server" Height="35px" Width="260px" onkeyup="javascript:validateOldPassword()" autocomplete="off" TextMode="Password"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_old" runat="server" Text="" ></asp:Label></td>
                </tr>
                <tr>
                    <td>New Password: </td>
                    <td><asp:TextBox  ID="tb_newpwd" runat="server" Height="35px" Width="260px" onkeyup="javascript:validateNewPassword()" autocomplete="off" TextMode="Password"></asp:TextBox></td>
                     <td><asp:Label ID="pwd_checker" runat="server" Text="" ></asp:Label></td>
                </tr>
               <tr>
                    <td>Confirm New Password: </td>
                    <td><asp:TextBox  ID="tb_Cnewpwd" runat="server" Height="35px" Width="260px" onkeyup="javascript:validateCNewPassword()" autocomplete="off" TextMode="Password"></asp:TextBox></td>
                     <td><asp:Label ID="lbl_new" runat="server" Text="" ></asp:Label></td>
                </tr>
              <tr>
                  <td><asp:Button ID="Submit" runat="server" Text="Update Password" Width="213px" OnClick="btn_Change"/></td>
              </tr>

              
              </table>
        </div>
    </form>
</body>
</html>
