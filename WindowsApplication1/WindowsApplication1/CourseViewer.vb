Imports System.Data.Objects
Imports System.Data.Objects.DataClasses

Public Class CourseViewer

    Private schoolContext As SchoolEntities


    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        schoolContext.Dispose()
        Me.Close()
    End Sub

    Private Sub CourseViewer_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Initialize the ObjectContext.
        schoolContext = New SchoolEntities()



        ' Define a query that returns all Department objects and related
        ' Course objects, ordered by name.
        Dim departmentQuery As ObjectQuery(Of Department) = _
            schoolContext.Departments.OrderBy("it.Name")

        Try
            ' Bind the ComboBox control to the query, which is 
            ' executed during data binding.
            Me.cboDeptList.DisplayMember = "Name"
            Me.cboDeptList.DataSource = departmentQuery
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub cboDeptList_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboDeptList.SelectedIndexChanged
        Try
            ' Get the object for the selected department.
            Dim department As Department = _
                CType(Me.cboDeptList.SelectedItem, Department)

            ' Bind the grid view to the collection of Course objects 
            ' that are related to the selected Department object.
            dgvCourse.DataSource = department.Courses

            ' Hide the columns that are bound to the navigation properties on Course.
            dgvCourse.Columns("Department").Visible = False
            dgvCourse.Columns("CourseGrades").Visible = False
            dgvCourse.Columns("OnlineCourse").Visible = False
            dgvCourse.Columns("OnsiteCourse").Visible = False
            dgvCourse.Columns("People").Visible = False

            dgvCourse.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub



    Private Sub dgvCourse_SelectionChanged(sender As System.Object, e As System.EventArgs) Handles dgvCourse.SelectionChanged
        Dim c As Course = dgvCourse.CurrentRow.DataBoundItem
        TextBox1.Text = ""
        If c IsNot Nothing Then
            For Each cg As CourseGrade In c.CourseGrades
                TextBox1.Text &= cg.Person.FirstName & cg.Grade & vbNewLine
            Next

        End If

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            ' Save object changes to the database, display a message, 
            ' and refresh the form.
            schoolContext.SaveChanges()
            MessageBox.Show("Changes saved to the database.")
            Me.Refresh()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub



    Private Sub dgvCourse_UserDeletingRow(sender As System.Object, e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles dgvCourse.UserDeletingRow
        schoolContext.DeleteObject(dgvCourse.CurrentRow.DataBoundItem)
        e.Cancel = True
    End Sub
End Class
