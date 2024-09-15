Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView
Public Class Rent
    Dim Con = New SqlConnection("Data Source=DESKTOP-G2H37OO\SQLEXPRESS;Initial Catalog=CarRentalmanagementVbdb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True")
    Private Sub fillCustomer()
        Con.Open()
        Dim sql = "select * from CustomerTbl"
        Dim cmd As New SqlCommand(sql, Con)
        Dim adapter As New SqlDataAdapter(cmd)
        Dim Tbl As New DataTable()
        adapter.Fill(Tbl)
        CustIdCb.DataSource = Tbl
        CustIdCb.DisplayMember = "CustId"
        CustIdCb.ValueMember = "CustId"
        Con.Close()
    End Sub
    Private Sub fillRegistration()
        Dim Status = "Yes"
        Con.Open()
        Dim sql = "select * from CarTbl where Available ='" & Status & "'"
        Dim cmd As New SqlCommand(sql, Con)
        Dim adapter As New SqlDataAdapter(cmd)
        Dim Tbl As New DataTable()
        adapter.Fill(Tbl)
        RegNumCb.DataSource = Tbl
        RegNumCb.DisplayMember = "RegNo"
        RegNumCb.ValueMember = "RegNo"
        Con.Close()
    End Sub
    Private Sub UpdateCar()
        Dim Status = "No"
        Try
            Con.Open()
            Dim query = "update CarTbl set Available='" & Status & "' where RegNo='" & RegNumCb.SelectedValue.ToString() & "'"
            Dim cmd As SqlCommand
            cmd = New SqlCommand(query, Con)
            cmd.ExecuteNonQuery()
            ' MsgBox("Car Successfully Updated")

            'clear()
            'populate()
            Con.Close()
        Catch ex As Exception
            ' MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub GetCustName()
        Con.Open()
        Dim sql = "select * from CustomerTbl where CustId='" & CustIdCb.SelectedValue.ToString() & "'"
        Dim cmd As New SqlCommand(sql, Con)
        Dim dt As New DataTable
        Dim reader As SqlDataReader
        reader = cmd.ExecuteReader
        While reader.Read
            CustnameTb.Text = reader(1).ToString()
        End While
        Con.Close()
    End Sub
    Dim Cost = 0
    Private Sub GetCarRate()
        Con.Open()
        Dim sql = "select * from CarTbl where RegNo='" & RegNumCb.SelectedValue.ToString() & "'"
        Dim cmd As New SqlCommand(sql, Con)
        Dim dt As New DataTable
        Dim reader As SqlDataReader
        reader = cmd.ExecuteReader
        While reader.Read

            Cost = Convert.ToInt32(reader(4).ToString())
        End While
        Con.Close()
    End Sub
    Private Sub Rent_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillCustomer()
        fillRegistration()
        populate()
    End Sub

    Private Sub CustIdCb_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles CustIdCb.SelectionChangeCommitted
        GetCustName()

    End Sub
    Private Sub clear()
        CustnameTb.Text = ""
        FeesTb.Text = ""
    End Sub
    Private Sub populate()
        Con.Open()
        Dim sql = "select * from RentTbl"
        Dim cmd = New SqlCommand(sql, Con)
        Dim adapter As SqlDataAdapter
        adapter = New SqlDataAdapter(cmd)
        Dim builder As SqlCommandBuilder
        builder = New SqlCommandBuilder(adapter)
        Dim ds As DataSet
        ds = New DataSet
        adapter.Fill(ds)
        RentDgv.DataSource = ds.Tables(0)
        Con.Close()
    End Sub
    Private Sub CalculateFees()
        'Calculate the number of days the car will be Rented
        Dim diff As System.TimeSpan = ReturnDate.Value.Date.Subtract(RentDate.Value.Date)
        'MsgBox(diff.TotalDays)
        Dim Days = diff.TotalDays
        Dim fees = Days * Cost
        FeesTb.Text = fees
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CustnameTb.Text = "" Or RegNumCb.SelectedIndex = -1 Or FeesTb.Text = "" Then
            MsgBox("Missing Data")
        Else
            Try

                Con.Open()
                Dim query = "INSERT INTO RentTbl VALUES (@RegNum, @CustId, @CustName, @RentDate, @ReturnDate, @Fees)"
                Dim cmd As New SqlCommand(query, Con)
                cmd.Parameters.AddWithValue("@RegNum", RegNumCb.SelectedValue.ToString())
                cmd.Parameters.AddWithValue("@CustId", CustIdCb.SelectedValue.ToString())
                cmd.Parameters.AddWithValue("@CustName", CustnameTb.Text)
                cmd.Parameters.AddWithValue("@RentDate", RentDate.Value.Date)
                cmd.Parameters.AddWithValue("@ReturnDate", ReturnDate.Value.Date)
                cmd.Parameters.AddWithValue("@Fees", FeesTb.Text)

                cmd.ExecuteNonQuery()
                MsgBox("Car Successfully Rented.")
                UpdateCar()
                clear()
                populate()
                fillRegistration()
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message)
            Finally
                If Con IsNot Nothing AndAlso Con.State = ConnectionState.Open Then
                    Con.Close()
                End If
            End Try
        End If
    End Sub

    Private Sub ReturnDate_ValueChanged(sender As Object, e As EventArgs) Handles ReturnDate.ValueChanged
        CalculateFees()
    End Sub

    Private Sub RegNumCb_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles RegNumCb.SelectionChangeCommitted
        GetCarRate()
    End Sub
End Class