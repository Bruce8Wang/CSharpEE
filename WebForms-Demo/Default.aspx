﻿<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>ListView Demo</title>
</head>
<body>
    <form runat="server">
        <label for="txtInput">ProductName : </label>
        <asp:TextBox ID="txtInput" runat="server" AutoPostBack="true"></asp:TextBox>
        <br />
        <br />
        <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="ProductID" InsertItemPosition="LastItem" DataMember="DefaultView">
            <AlternatingItemTemplate>
                <tr style="background-color: #FFFFFF; color: #284775;">
                    <td>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="删除" />
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="编辑" />
                    </td>
                    <td>
                        <asp:Label ID="ProductIDLabel" runat="server" Text='<%# Eval("ProductID") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ProductNameLabel" runat="server" Text='<%# Eval("ProductName") %>' />
                    </td>
                    <td>
                        <asp:Label ID="UnitPriceLabel" runat="server" Text='<%# Eval("UnitPrice") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="DiscontinuedCheckBox" runat="server" Checked='<%# Eval("Discontinued") %>' Enabled="false" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <EditItemTemplate>
                <tr style="background-color: #999999;">
                    <td>
                        <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="更新" />
                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="取消" />
                    </td>
                    <td>
                        <asp:Label ID="ProductIDLabel1" runat="server" Text='<%# Eval("ProductID") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="ProductNameTextBox" runat="server" Text='<%# Bind("ProductName") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="UnitPriceTextBox" runat="server" Text='<%# Bind("UnitPrice") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="DiscontinuedCheckBox" runat="server" Checked='<%# Bind("Discontinued") %>' />
                    </td>
                </tr>
            </EditItemTemplate>
            <EmptyDataTemplate>
                <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                    <tr>
                        <td>未返回数据。</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <InsertItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="插入" />
                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="清除" />
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="ProductNameTextBox" runat="server" Text='<%# Bind("ProductName") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="UnitPriceTextBox" runat="server" Text='<%# Bind("UnitPrice") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="DiscontinuedCheckBox" runat="server" Checked='<%# Bind("Discontinued") %>' />
                    </td>
                </tr>
            </InsertItemTemplate>
            <ItemTemplate>
                <tr style="background-color: #E0FFFF; color: #333333;">
                    <td>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="删除" />
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="编辑" />
                    </td>
                    <td>
                        <asp:Label ID="ProductIDLabel" runat="server" Text='<%# Eval("ProductID") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ProductNameLabel" runat="server" Text='<%# Eval("ProductName") %>' />
                    </td>
                    <td>
                        <asp:Label ID="UnitPriceLabel" runat="server" Text='<%# Eval("UnitPrice") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="DiscontinuedCheckBox" runat="server" Checked='<%# Eval("Discontinued") %>' Enabled="false" />
                    </td>
                </tr>
            </ItemTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table id="itemPlaceholderContainer" runat="server" border="1" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;">
                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                    <th runat="server"></th>
                                    <th>
                                        <asp:LinkButton CommandName="Sort" CommandArgument="ProductID" runat="server">ProductID</asp:LinkButton>
                                    </th>
                                    <th>
                                        <asp:LinkButton CommandName="Sort" CommandArgument="ProductName" runat="server">ProductName</asp:LinkButton>
                                    </th>
                                    <th>
                                        <asp:LinkButton CommandName="Sort" CommandArgument="UnitPrice" runat="server">UnitPrice</asp:LinkButton>
                                    </th>
                                    <th>
                                        <asp:LinkButton CommandName="Sort" CommandArgument="Discontinued" runat="server">Discontinued</asp:LinkButton>
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                    <th runat="server"></th>
                                    <th runat="server">ProductID</th>
                                    <th runat="server">ProductName</th>
                                    <th runat="server">UnitPrice</th>
                                    <th runat="server">Discontinued</th>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                            <asp:DataPager ID="DataPager1" runat="server">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                                    <asp:NumericPagerField />
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <SelectedItemTemplate>
                <tr style="background-color: #E2DED6; font-weight: bold; color: #333333;">
                    <td>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="删除" />
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="编辑" />
                    </td>
                    <td>
                        <asp:Label ID="ProductIDLabel" runat="server" Text='<%# Eval("ProductID") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ProductNameLabel" runat="server" Text='<%# Eval("ProductName") %>' />
                    </td>
                    <td>
                        <asp:Label ID="UnitPriceLabel" runat="server" Text='<%# Eval("UnitPrice") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="DiscontinuedCheckBox" runat="server" Checked='<%# Eval("Discontinued") %>' Enabled="false" />
                    </td>
                </tr>
            </SelectedItemTemplate>
        </asp:ListView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="server=10.0.0.143;database=northwind;user id=sa;password=1qaz@WSX"
            ProviderName="System.Data.SqlClient"
            DeleteCommand="DELETE FROM Products WHERE ProductID = @ProductID"
            InsertCommand="INSERT INTO Products (ProductName, UnitPrice, Discontinued) VALUES (@ProductName, @UnitPrice, @Discontinued)"
            SelectCommand="SELECT ProductID, ProductName, UnitPrice, Discontinued FROM Products where ProductName=IIF(@ProductName='',ProductName,@ProductName)"
            UpdateCommand="UPDATE Products SET ProductName = @ProductName, UnitPrice = @UnitPrice, Discontinued = @Discontinued WHERE ProductID = @ProductID">
            <DeleteParameters>
                <asp:Parameter Name="ProductID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="ProductName" Type="String" />
                <asp:Parameter Name="UnitPrice" Type="Decimal" />
                <asp:Parameter Name="Discontinued" Type="Boolean" />
                <asp:Parameter Name="ProductID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="ProductName" Type="String" />
                <asp:Parameter Name="UnitPrice" Type="Decimal" />
                <asp:Parameter Name="Discontinued" Type="Boolean" />
            </InsertParameters>
            <SelectParameters>
                <asp:ControlParameter Name="ProductName" Type="String" ControlID="txtInput" DefaultValue=" " />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
