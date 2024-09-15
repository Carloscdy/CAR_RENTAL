Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView
Public Class Customers
    Dim Con = New SqlConnection("Data Source=DESKTOP-G2H37OO\SQLEXPRESS;Initial Catalog=CarRentalmanagementVbdb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True")

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CustPhoneTb.Text = "" Or CustAddressTb.Text = "" Or CustNameTb.Text = "" Then
            MsgBox("Missing Data")
        Else

            Try
                Con.Open()
                Dim query = "insert into CustomerTbl values('" & CustNameTb.Text & "','" & CustAddressTb.Text & "','" & CustPhoneTb.Text & "')"
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                MsgBox("Customer Successfully Saved")
                Con.Close()
                Clear()
                populate()
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub populate()
        Con.Open()
        Dim sql = "select * from CustomerTbl"
        Dim cmd = New SqlCommand(sql, Con)
        Dim adapter As SqlDataAdapter
        adapter = New SqlDataAdapter(cmd)
        Dim builder As SqlCommandBuilder
        builder = New SqlCommandBuilder(adapter)
        Dim ds As DataSet
        ds = New DataSet
        adapter.Fill(ds)
        CustomersDgv.DataSource = ds.Tables(0)
        Con.Close()
    End Sub
    Private Sub Clear()
        CustNameTb.Text = ""
        CustAddressTb.Text = ""
        CustPhoneTb.Text = ""
    End Sub

    Private Sub Customers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        populate()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Clear()
    End Sub
    Dim Key = 0
    Private Sub CustomersDgv_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles CustomersDgv.CellMouseClick
        Dim row As DataGridViewRow = CustomersDgv.Rows(e.RowIndex)
        CustNameTb.Text = row.Cells(1).Value.ToString
        CustAddressTb.Text = row.Cells(2).Value.ToString
        CustPhoneTb.Text = row.Cells(3).Value.ToString
        If CustNameTb.Text = "" Then
            Key = 0
        Else
            Key = Convert.ToInt32(row.Cells(0).Value.ToString)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Key = 0 Then
            MsgBox("Select the Customer")
        Else
            Try
                Con.Open()
                Dim query = "delete from CustomerTbl where Custid=" & Key & ""
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                MsgBox("Customer Successfully Deleted")
                Con.Close()
                Clear()
                populate()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If CustNameTb.Text = "" Or CustAddressTb.Text = "" Or CustPhoneTb.Text = "" Then
            MsgBox("Missing Information")
        Else
            Try
                Con.Open()
                Dim query = "update CustomerTbl set CustName='" & CustNameTb.Text & "',CustAdd='" & CustAddressTb.Text & "',CustPhone='" & CustPhoneTb.Text & "' where Custid=" & Key & ""
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                MsgBox("Customer Successfully Updated")
                Con.Close()
                Clear()
                populate()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
End Class