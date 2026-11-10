Imports System.Data
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms

Public Class RecordFilterForm
    Inherits Form

    Private ReadOnly sourceTable As DataTable
    Private ReadOnly criteria As New Dictionary(Of String, Object())()
    Private ReadOnly scrollPanel As FlowLayoutPanel
    Private ReadOnly CoffeeDark As Color = Color.FromArgb(62, 39, 35)
    Private ReadOnly Coffee As Color = Color.FromArgb(111, 78, 55)
    Private ReadOnly Caramel As Color = Color.FromArgb(166, 123, 91)
    Private ReadOnly Cream As Color = Color.FromArgb(245, 237, 227)
    Private ReadOnly CardColor As Color = Color.FromArgb(255, 249, 243)
    Private ReadOnly TextDark As Color = Color.FromArgb(59, 48, 42)

    Public Property FilterExpression As String = String.Empty

    Public Sub New(table As DataTable, windowTitle As String, fieldNames As String())
        sourceTable = table

        Me.Text = windowTitle
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Size = New Size(760, 680)
        Me.MinimumSize = New Size(650, 520)
        Me.BackColor = Cream
        Me.Font = New Font("Segoe UI", 9.5F)
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ShowInTaskbar = False

        Dim header As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 92,
            .BackColor = CoffeeDark,
            .Padding = New Padding(26, 16, 20, 12)
        }
        Me.Controls.Add(header)

        Dim title As New Label With {
            .Text = windowTitle,
            .Font = New Font("Georgia", 19.0F, FontStyle.Bold),
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(25, 13)
        }
        header.Controls.Add(title)

        Dim subtitle As New Label With {
            .Text = "Choose one or more conditions. All selected conditions will be combined.",
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = Color.FromArgb(232, 218, 207),
            .AutoSize = True,
            .Location = New Point(27, 52)
        }
        header.Controls.Add(subtitle)

        scrollPanel = New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.TopDown,
            .WrapContents = False,
            .AutoScroll = True,
            .Padding = New Padding(22, 18, 22, 12),
            .BackColor = Cream
        }
        Me.Controls.Add(scrollPanel)

        Dim footer As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 76,
            .BackColor = CardColor,
            .Padding = New Padding(20, 14, 20, 14)
        }
        Me.Controls.Add(footer)

        Dim btnClear As New Button With {
            .Text = "Clear filters",
            .Size = New Size(115, 38),
            .Location = New Point(20, 18),
            .BackColor = Color.FromArgb(225, 213, 201),
            .ForeColor = TextDark,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI Semibold", 9.0F),
            .Cursor = Cursors.Hand,
            .DialogResult = DialogResult.OK
        }
        btnClear.FlatAppearance.BorderSize = 0
        AddHandler btnClear.Click, AddressOf ClearClicked
        footer.Controls.Add(btnClear)

        Dim btnCancel As New Button With {
            .Text = "Cancel",
            .Size = New Size(100, 38),
            .Location = New Point(footer.ClientSize.Width - 235, 18),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .BackColor = Color.White,
            .ForeColor = CoffeeDark,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI Semibold", 9.0F),
            .Cursor = Cursors.Hand,
            .DialogResult = DialogResult.Cancel
        }
        btnCancel.FlatAppearance.BorderColor = Caramel
        footer.Controls.Add(btnCancel)

        Dim btnApply As New Button With {
            .Text = "Apply filters",
            .Size = New Size(125, 38),
            .Location = New Point(footer.ClientSize.Width - 125, 18),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .BackColor = Coffee,
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI Semibold", 9.0F),
            .Cursor = Cursors.Hand
        }
        btnApply.FlatAppearance.BorderSize = 0
        AddHandler btnApply.Click, AddressOf ApplyClicked
        footer.Controls.Add(btnApply)

        For Each fieldName As String In fieldNames
            If sourceTable.Columns.Contains(fieldName) Then
                AddFieldControl(fieldName, sourceTable.Columns(fieldName).DataType)
            End If
        Next

        If criteria.Count = 0 Then
            Dim emptyLabel As New Label With {
                .Text = "There are no filterable fields in this view.",
                .AutoSize = True,
                .ForeColor = TextDark,
                .Padding = New Padding(4, 12, 4, 4)
            }
            scrollPanel.Controls.Add(emptyLabel)
        End If

        Me.AcceptButton = btnApply
        Me.CancelButton = btnCancel
    End Sub

    Private Sub AddFieldControl(fieldName As String, dataType As Type)
        Dim row As New Panel With {
            .Width = 680,
            .Height = 72,
            .BackColor = CardColor,
            .Margin = New Padding(0, 0, 0, 10),
            .Padding = New Padding(12)
        }

        Dim label As New Label With {
            .Text = FriendlyName(fieldName),
            .Location = New Point(12, 7),
            .AutoSize = True,
            .Font = New Font("Segoe UI Semibold", 9.0F),
            .ForeColor = CoffeeDark
        }
        row.Controls.Add(label)

        If dataType Is GetType(DateTime) Then
            Dim fromPicker As New DateTimePicker With {
                .Format = DateTimePickerFormat.Custom,
                .CustomFormat = "'From' MMM dd, yyyy",
                .ShowCheckBox = True,
                .Checked = False,
                .Width = 245,
                .Location = New Point(12, 32),
                .Font = New Font("Segoe UI", 9.0F)
            }
            Dim toPicker As New DateTimePicker With {
                .Format = DateTimePickerFormat.Custom,
                .CustomFormat = "'To' MMM dd, yyyy",
                .ShowCheckBox = True,
                .Checked = False,
                .Width = 245,
                .Location = New Point(285, 32),
                .Font = New Font("Segoe UI", 9.0F)
            }
            row.Controls.Add(fromPicker)
            row.Controls.Add(toPicker)
            criteria(fieldName) = New Object() {fromPicker, toPicker}
        ElseIf IsNumericType(dataType) Then
            Dim minimum As New TextBox With {
                .Width = 245,
                .Location = New Point(12, 32),
                .Font = New Font("Segoe UI", 9.0F),
                .BorderStyle = BorderStyle.FixedSingle,
                .PlaceholderText = "Minimum value"
            }
            Dim maximum As New TextBox With {
                .Width = 245,
                .Location = New Point(285, 32),
                .Font = New Font("Segoe UI", 9.0F),
                .BorderStyle = BorderStyle.FixedSingle,
                .PlaceholderText = "Maximum value"
            }
            row.Controls.Add(minimum)
            row.Controls.Add(maximum)
            criteria(fieldName) = New Object() {minimum, maximum}
        Else
            Dim valueBox As New TextBox With {
                .Width = 518,
                .Location = New Point(12, 32),
                .Font = New Font("Segoe UI", 9.0F),
                .BorderStyle = BorderStyle.FixedSingle,
                .PlaceholderText = "Type to find matching " & FriendlyName(fieldName).ToLowerInvariant() & "..."
            }
            row.Controls.Add(valueBox)
            criteria(fieldName) = New Object() {valueBox}
        End If

        scrollPanel.Controls.Add(row)
    End Sub

    Private Shared Function IsNumericType(dataType As Type) As Boolean
        Return dataType Is GetType(Byte) OrElse dataType Is GetType(Short) OrElse
               dataType Is GetType(Integer) OrElse dataType Is GetType(Long) OrElse
               dataType Is GetType(Single) OrElse dataType Is GetType(Double) OrElse
               dataType Is GetType(Decimal)
    End Function

    Private Shared Function FriendlyName(value As String) As String
        Dim result As String = value
        result = result.Replace("PropertyType", "Property Type").
                        Replace("PropertyStatus", "Property Status").
                        Replace("PaymentStatus", "Payment Status").
                        Replace("PaymentMethod", "Payment Method").
                        Replace("MonthlyRent", "Monthly Rent").
                        Replace("SecurityDeposit", "Security Deposit").
                        Replace("LeaseStartDate", "Lease Start Date").
                        Replace("LeaseEndDate", "Lease End Date").
                        Replace("PaymentDate", "Payment Date").
                        Replace("AmountDue", "Amount Due").
                        Replace("AmountPaid", "Amount Paid").
                        Replace("TenantName", "Tenant Name").
                        Replace("PropertyName", "Property Name")
        Return result
    End Function

    Private Sub ApplyClicked(sender As Object, e As EventArgs)
        Dim expressions As New List(Of String)()

        For Each entry In criteria
            Dim columnName As String = "[" & entry.Key.Replace("]", "]]") & "]"
            Dim controls As Object() = entry.Value
            Dim columnType As Type = sourceTable.Columns(entry.Key).DataType

            If columnType Is GetType(DateTime) Then
                Dim fromPicker = DirectCast(controls(0), DateTimePicker)
                Dim toPicker = DirectCast(controls(1), DateTimePicker)
                If fromPicker.Checked AndAlso toPicker.Checked AndAlso fromPicker.Value.Date > toPicker.Value.Date Then
                    MessageBox.Show("The start date cannot be later than the end date.", "Check date range", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
                If fromPicker.Checked Then expressions.Add(columnName & " >= #" & fromPicker.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) & "#")
                If toPicker.Checked Then expressions.Add(columnName & " < #" & toPicker.Value.Date.AddDays(1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) & "#")
            ElseIf IsNumericType(columnType) Then
                Dim minimum = DirectCast(controls(0), TextBox).Text.Trim()
                Dim maximum = DirectCast(controls(1), TextBox).Text.Trim()
                Dim minValue As Decimal
                Dim maxValue As Decimal
                If minimum <> "" Then
                    If Not Decimal.TryParse(minimum, NumberStyles.Number, CultureInfo.CurrentCulture, minValue) Then
                        MessageBox.Show("Enter a valid minimum number for " & FriendlyName(entry.Key) & ".", "Invalid number", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    expressions.Add(columnName & " >= " & minValue.ToString(CultureInfo.InvariantCulture))
                End If
                If maximum <> "" Then
                    If Not Decimal.TryParse(maximum, NumberStyles.Number, CultureInfo.CurrentCulture, maxValue) Then
                        MessageBox.Show("Enter a valid maximum number for " & FriendlyName(entry.Key) & ".", "Invalid number", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    expressions.Add(columnName & " <= " & maxValue.ToString(CultureInfo.InvariantCulture))
                End If
                If minimum <> "" AndAlso maximum <> "" AndAlso minValue > maxValue Then
                    MessageBox.Show("The minimum value cannot be greater than the maximum value.", "Check value range", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            Else
                Dim searchText As String = DirectCast(controls(0), TextBox).Text.Trim()
                If searchText <> "" Then
                    searchText = searchText.Replace("'", "''").Replace("[", "[[]").Replace("]", "[]]").Replace("%", "[%]").Replace("*", "[*]")
                    expressions.Add(columnName & " LIKE '%" & searchText & "%'")
                End If
            End If
        Next

        FilterExpression = String.Join(" AND ", expressions)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ClearClicked(sender As Object, e As EventArgs)
        For Each entry In criteria
            For Each control As Object In entry.Value
                If TypeOf control Is TextBox Then
                    DirectCast(control, TextBox).Clear()
                ElseIf TypeOf control Is DateTimePicker Then
                    DirectCast(control, DateTimePicker).Checked = False
                End If
            Next
        Next
        FilterExpression = String.Empty
    End Sub

    Public Shared Function GetSourceTable(source As Object) As DataTable
        If TypeOf source Is DataTable Then Return DirectCast(source, DataTable)
        If TypeOf source Is DataView Then Return DirectCast(source, DataView).Table
        If TypeOf source Is BindingSource Then
            If TypeOf DirectCast(source, BindingSource).DataSource Is DataTable Then
                Return DirectCast(DirectCast(source, BindingSource).DataSource, DataTable)
            End If
            If TypeOf DirectCast(source, BindingSource).DataSource Is DataView Then
                Return DirectCast(DirectCast(source, BindingSource).DataSource, DataView).Table
            End If
        End If
        Return Nothing
    End Function

    Public Shared Sub StyleFilterButton(button As Button)
        button.Text = "Filter"
        button.BackColor = Color.FromArgb(111, 78, 55)
        button.ForeColor = Color.White
        button.FlatStyle = FlatStyle.Flat
        button.FlatAppearance.BorderSize = 0
        button.Font = New Font("Segoe UI Semibold", 9.0F)
        button.Cursor = Cursors.Hand
        button.TextAlign = ContentAlignment.MiddleRight
        button.Padding = New Padding(0, 0, 12, 0)
        AddHandler button.Paint, AddressOf DrawFilterIcon
    End Sub

    Private Shared Sub DrawFilterIcon(sender As Object, e As PaintEventArgs)
        Dim button As Button = DirectCast(sender, Button)
        Dim x As Integer = 14
        Dim y As Integer = (button.Height - 16) \ 2
        Using pen As New Pen(Color.White, 1.8F)
            pen.StartCap = Drawing2D.LineCap.Round
            pen.EndCap = Drawing2D.LineCap.Round
            e.Graphics.DrawLine(pen, x, y, x + 18, y)
            e.Graphics.DrawLine(pen, x + 2, y + 1, x + 7, y + 7)
            e.Graphics.DrawLine(pen, x + 16, y + 1, x + 11, y + 7)
            e.Graphics.DrawLine(pen, x + 7, y + 7, x + 7, y + 12)
            e.Graphics.DrawLine(pen, x + 7, y + 12, x + 11, y + 15)
            e.Graphics.DrawLine(pen, x + 11, y + 15, x + 11, y + 7)
        End Using
    End Sub

End Class
