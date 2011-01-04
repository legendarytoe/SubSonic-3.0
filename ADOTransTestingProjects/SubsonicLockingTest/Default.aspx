<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnRepull" runat="server" Text="Repull from Db" onclick="btnRepull_Click" 
              />
    <asp:Button ID="btnUpdateRec1" runat="server" Text="Update Rec 1" 
            onclick="btnUpdateRec1_Click" />
    <asp:Button ID="btnUpdateRec2" runat="server" Text="Update Rec 2" 
            onclick="btnUpdateRec2_Click" />  
    </div>
    </form>
</body>
</html>
