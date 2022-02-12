<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="_204622H_AppSec_Asgn1.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type ="text/javascript ">
        function validateFname() {
            var Name1 = document.getElementById('<%=tb_Fname.ClientID%>').value
          
            if (Name1.length == 0) {
                document.getElementById("lbl_Fname").innerHTML = " Field is required";
                document.getElementById("lbl_Fname").style.color = "Red";
                return ("empty");
            }
            else {
                document.getElementById("lbl_Fname").innerHTML = " ";
            }         
        }
        function validateLname() {
       
           var Name2 = document.getElementById('<%=tb_Lname.ClientID%>').value

           if (Name2.length == 0) {
               document.getElementById("lbl_Lname").innerHTML = " Field is required";
               document.getElementById("lbl_Lname").style.color = "Red";
               return ("empty");
           }
           else {
               document.getElementById("lbl_Lname").innerHTML = " ";
           }
        }

        function validateDob() {
            var dob = document.getElementById('<%=tb_dob.ClientID%>').value.str

            if (dob.str == 'dd/mm/yyyy') {
                 document.getElementById("lbl_Dob").innerHTML = " Field is required";
                 document.getElementById("lbl_Dob").style.color = "Red";
             }
             else {
                 document.getElementById("lbl_Dob").innerHTML = " ";
             }
        }

        function validateEmail() {
            var email = document.getElementById('<%=tb_email.ClientID%>').value

            if (email == '') {
                document.getElementById("lbl_Email").innerHTML = " Field is required";
                document.getElementById("lbl_Email").style.color = "Red";
            }
            else if (email.search(/[a-zA-Z0-9]+@[a-zA-Z]/) == -1) {
                document.getElementById("lbl_Email").innerHTML = " Invalid Email Format";
                document.getElementById("lbl_Email").style.color = "Red";
            }
            else {
                document.getElementById("lbl_Email").innerHTML = " ";
            }
              
            }
        

        function validateCard() {
            var Credit = document.getElementById('<%=tb_card.ClientID%>').value

            if (Credit == '') {
                document.getElementById("lbl_Card").innerHTML = " Field is required";
                document.getElementById("lbl_Card").style.color = "Red";
            }
            if (Credit.length < 16 || Credit.length > 16) {
                document.getElementById("lbl_Card").innerHTML = " Invalid Card Length!";
                document.getElementById("lbl_Card").style.color = "Red";
            }
            else {
                document.getElementById("lbl_Card").innerHTML = " ";
            }
        }
       
        function validatePassword() {
            var str = document.getElementById('<%=tb_Password.ClientID %>').value;
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Registration
            <br />
            <br />
            </div>
            <table>
                <tr colspan ="2">
                    <td>First Name: </td>
                    <td><asp:TextBox ID="tb_Fname" runat="server" Height="35px" Width="260px" onkeyup="javascript:validateFname()" autocomplete="off"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_Fname" runat="server" Text="" ></asp:Label></td>
                </tr>
                <tr>
                    <td>Last Name: </td>
                    <td><asp:TextBox ID="tb_Lname" runat="server" Height="35px" Width="260px" onkeyup="javascript:validateLname()" autocomplete="off"></asp:TextBox></td>
                     <td><asp:Label ID="lbl_Lname" runat="server" Text="" ></asp:Label></td>
                </tr>
                <tr>
                    <td>Date of Birth: </td>
                    <%--<asp:RequiredFieldValidator runat="server" >--%>
                    <td><asp:TextBox ID="tb_dob" runat="server" Height="35px" Width="260px" TextMode="Date" onkeyup="javascript:validateDob()" autocomplete="off"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_Dob" runat="server" Text="" ></asp:Label></td>
                    <%--</asp:RequiredFieldValidator>--%>
                </tr>
                <tr>
                    <td>Email Address</td>
                    <td><asp:TextBox ID="tb_email" runat="server" Height="35px" Width="260px" TextMode="Email" onkeyup="javascript:validateEmail()" autocomplete="off"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_Email" runat="server" Text="" ></asp:Label></td>
                </tr>
                <tr>
                    <td>Credit Card Info: </td>
                    <td><asp:TextBox ID="tb_card" runat="server" Height="35px" Width="260px" onkeyup="javascript:validateCard()" TextMode="Number" autocomplete="off"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_Card" runat="server" Text="" ></asp:Label></td>
                </tr>
                <tr>
                    <td>Photo: </td>
                    <td><asp:FileUpload ID="tb_photo" runat="server" Height="35px" /></td>
                    <td><asp:Label ID="lbl_File" runat="server" Text="" ></asp:Label></td>
                </tr>
                <tr>
                    <td>Password: </td>
                    <td> <asp:TextBox ID="tb_Password" runat="server" Height="35px" Width="260px" onkeyup="javascript:validatePassword()" autocomplete="off" TextMode="Password"></asp:TextBox></td>
                     <td><asp:Label ID="pwd_checker" runat="server" Text=" " ></asp:Label></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Label ID="pwd_checker2" runat="server" Text=" " ></asp:Label></td>
                </tr>
               
                <tr>
                <td><asp:Button ID="Submit" runat="server" Text="Register" Width="213px" OnClick="btn_Validate" ></asp:Button></td>
                </tr>
                </table>
        <asp:Label ID="lbl_test" runat="server" Text="Json message: " EnableViewState="False" ></asp:Label>
        </form>
    </body>
    </html>