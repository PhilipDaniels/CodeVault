<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Identify.aspx.cs" Inherits="IdentityTester.Identify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Identity Tester</title>
    <style type="text/css">
        .style1
        {
            border-style: solid;
            border-width: 1px;
        }
        .style2
        {
            width: 303px;
            font-weight: 700;
        }
        .style3
        {
            width: 336px;
            font-weight: 700;
        }
        .style4
        {
            width: 336px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3>n.b.: To run under IIS, you need to install this web site as an ASP.Net application, not a virtual directory.
        Then turn off Anonymous Authentication and turn on Windows Authentication.</h3>
    
        <hr />
        <h3>Web.config settings are currently:</h3>
        &lt;authentication mode="<asp:Literal ID="litAuthenticationMode" runat="server" />" /&gt;
        <br />
        &lt;identity impersonate="<asp:Literal ID="litImpersonate" runat="server" />" /&gt;
            <asp:Button ID="btnChangeImpersonation" runat="server" Text="Change" OnClick="btnChangeImpersonation_Click" />
        <hr />
        <p />
        
        <table border="1" class="style1">
            <tr bgcolor="Silver">
                <td class="style3">Identity Description</td>
                <td class="style2">Value</td>
            </tr>
            <tr>
                <td>
                    Page.User.Identity (Same thing as HttpContext.User)<br /><br />
                    <i>This will be the end user, if they have IE. It will be blank
                    if anonymous access is on.</i>
                </td>
                <td><asp:Literal ID="litPageUserIdentity" runat="server" /></td>
            </tr>
            <tr>
                <td>
                    WindowsIdentity.GetCurrent()<br /><br />
                    <i>This will be the account that ASP.NET is running under if impersonation
                    is off. If impersonation is on, it will be the end user.</i><br /><br />
                    <i>If you are running this page under Visual Studio, the ASP.Net
                    web server is started by, and running as, you.</i>
                </td>
                <td><asp:Literal ID="litWindowsCurrentIdentity" runat="server" /></td>
            </tr>
            <tr>
                <td>
                    Thread.Current.Identity<br /><br />
                    <i>This appears to be the same as Page.User.Identity.</i>
                </td>
                <td><asp:Literal ID="litThreadCurrentIdentity" runat="server" /></td>
            </tr>
            <tr>
                <td colspan="2" bgcolor="Silver"><b>You are connected using: </b><asp:Literal ID="litConnStr" runat="server" /></td>
            </tr>
            <tr>
                <td>In the impersonation block, the database thinks you are</td>
                <td><asp:Literal ID="litDatabaseInBlock" runat="server" /></td>
            </tr>
            <tr>
                <td>Outside the impersonation block, but still using the connection that was opened inside it, the database thinks you are</td>
                <td><asp:Literal ID="litDatabaseAfterBlock" runat="server" /></td>
            </tr>            
            <tr>
                <td>
                    Using a connection opened after the impersonation block, the database thinks you are<br /><br />
                    <i>You might well see something like "Exception: Login failed for 
                    user 'NT AUTHORITY\NETWORK SERVICE'." You can fix this simply by
                    creating a server-level login for the specified account.
                    </i>
                </td>
                <td><asp:Literal ID="litDatabaseOutsideBlock" runat="server" /></td>
            </tr>
        </table>
    </div>
    
    <br />
    <hr />
    <br />
    <h3>What accounts does ASP.Net run under?</h3>
    <u>This page is hosted by: <asp:Literal ID="litHostSoftware" runat="server" /></u>
    <p />
    Under IIS5, the ASPNET account.
    <p />    
    Under IIS6 and IIS7, the NT AUTHORITY\NETWORK SERVICE account - in fact, for 6 and 7, the application 
    runs under an <i>application pool</i>, and it is the  application pool that specifies the identity.
    <p />
    These accounts are created when ASP.Net is installed, so it is typical to only find one of them.
    Of course, ASP.Net can be set to run under a specified account, such as Local System or a
    domain account.
    <p />
    IIS1 to IIS4 are available on Windows NT.<br />
    IIS5 is available on Windows 2000, XP Pro, and Windows MCE.<br />
    IIS6 is available on Windows Server 2003 and XP Pro x64.<br />
    IIS7 is available on Windows Server 2008 and Vista.<br />
    <p /><p />
    <img src="AppPool.png" />
    <p /><p />
    
    <br />
    <hr />
    <br />
    <h3>Further reference</h3>
    <a href="http://msdn.microsoft.com/en-us/library/ms998351.aspx">Microsoft article on using impersonation</a>
    <br />
    <a href="http://en.wikipedia.org/wiki/Internet_Information_Services">Wikipedia on IIS</a>
    <br />
    <a href="http://www.eggheadcafe.com/articles/20050703.asp">ASP.NET Request Identity by Jon Wojtowicz (inspiration for this app)</a>
    </form>
</body>
</html>
