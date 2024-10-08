﻿
Imports System.Data.SqlClient

Public Class Cars
    Dim Con = New SqlConnection("Data Source=DESKTOP-G2H37OO\SQLEXPRESS;Initial Catalog=CarRentalmanagementVbdb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True")

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Con.Open()
            Dim query = "Insert into CarTbl values('" & RegNumTb.Text & "','" & BrandCb.SelectedItem.ToString() & "','" & ModelTb.Text & "','" & PriceTb.Text & "','" & ColorTb.Text & "','" & AvailableCb.SelectedItem.ToString() & "')"
            Dim cmd As SqlCommand
            cmd = New SqlCommand(query, Con)
            cmd.ExecuteNonQuery()
            MsgBox("Car Successfully Saved")
            Con.Close()
            Clear()
            populate()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Clear()
        RegNumTb.Text = ""
        BrandCb.SelectedIndex = -1
        ModelTb.Text = ""
        PriceTb.Text = ""
        ColorTb.Text = ""
        AvailableCb.SelectedIndex = -1
        Key = 0
    End Sub
    Private Sub populate()
        Con.Open()
        Dim sql = "select * from CarTbl"
        Dim cmd = New SqlCommand(sql, Con)
        Dim adapter As SqlDataAdapter
        adapter = New SqlDataAdapter(cmd)
        Dim builder As SqlCommandBuilder
        builder = New SqlCommandBuilder(adapter)
        Dim ds As DataSet
        ds = New DataSet
        adapter.Fill(ds)
        CarDgv.DataSource = ds.Tables(0)
        Con.Close()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Clear()
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles CarDgv.CellMouseClick
        Dim row As DataGridViewRow = CarDgv.Rows(e.RowIndex)
        RegNumTb.Text = row.Cells(1).Value.ToString
        BrandCb.SelectedItem = row.Cells(2).Value.ToString
        ModelTb.Text = row.Cells(3).Value.ToString
        PriceTb.Text = row.Cells(4).Value.ToString
        ColorTb.Text = row.Cells(5).Value.ToString
        AvailableCb.SelectedItem = row.Cells(6).Value.ToString
        If RegNumTb.Text = "" Then
            Key = 0
        Else
            Key = Convert.ToInt32(row.Cells(0).Value.ToString)
        End If
    End Sub

    Private Sub Cars_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        populate()
    End Sub
    Dim Key = 0
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Key = 0 Then
            MsgBox("Select the Car")
        Else
            Try
                Con.Open()
                Dim query = "delete from CarTbl where CId=" & Key & ""
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                MsgBox("Car Successfully Deleted")
                Con.Close()
                Clear()
                populate()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub CarDgv_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles CarDgv.CellContentClick

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If RegNumTb.Text = "" Or BrandCb.SelectedIndex = -1 Or ModelTb.Text = "" Or PriceTb.Text = "" Or ColorTb.Text = "" Or AvailableCb.SelectedIndex = -1 Then
            MsgBox("Missing Information")
        Else
            Try
                Con.Open()
                Dim query = "update CarTbl set RegNo='" & RegNumTb.Text & " ',Brand='" & BrandCb.SelectedItem.ToString() & "',Model='" & ModelTb.Text & "',Price=" & PriceTb.Text & ",Color='" & ColorTb.Text & "',Available='" & AvailableCb.SelectedItem.ToString() & "' where CId=" & Key & ""
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                MsgBox("Car Successfully Updated")
                Con.Close()
                Clear()
                populate()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
End Class