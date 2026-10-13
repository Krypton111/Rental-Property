Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class PaymentsForm
    Inherits Form

    '==========================================================
    ' COZY COFFEE COLORS
    '==========================================================

    Private ReadOnly CoffeeDark As Color =
        Color.FromArgb(62, 39, 35)

    Private ReadOnly Coffee As Color =
        Color.FromArgb(111, 78, 55)

    Private ReadOnly Caramel As Color =
        Color.FromArgb(166, 123, 91)

    Private ReadOnly Latte As Color =
        Color.FromArgb(200, 162, 122)

    Private ReadOnly Cream As Color =
        Color.FromArgb(245, 237, 227)

    Private ReadOnly CardColor As Color =
        Color.FromArgb(255, 249, 243)

    Private ReadOnly TextDark As Color =
        Color.FromArgb(59, 48, 42)

    Private ReadOnly Sage As Color =
        Color.FromArgb(122, 139, 114)

    Private ReadOnly RedBrown As Color =
        Color.FromArgb(145, 75, 65)


    '==========================================================
    ' CONTROLS
    '==========================================================

    Private lblTitle As Label
    Private lblSubtitle As Label

    Private txtSearch As TextBox
    Private btnSearch As Button
    Private btnRefresh As Button

    Private txtPaymentID As TextBox
    Private cboTenant As ComboBox
    Private txtProperty As TextBox

    Private dtpPaymentDate As DateTimePicker

    Private txtAmountDue As TextBox
    Private txtAmountPaid As TextBox

    Private cboPaymentMethod As ComboBox
    Private cboPaymentStatus As ComboBox

    Private txtNotes As TextBox

    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnDelete As Button
    Private btnClear As Button

    Private dgvPayments As DataGridView

    Private selectedPaymentID As Integer = 0


    '==========================================================
    ' CONSTRUCTOR
    '==========================================================

    Public Sub New()

        BuildForm()

        LoadTenants()

        LoadPayments()

    End Sub


    '==========================================================
    ' BUILD FORM
    '==========================================================

    Private Sub BuildForm()

        Me.Text =
            "Casa Brew - Payments"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(1120, 720)

        Me.MinimumSize =
            New Size(1050, 680)

        Me.BackColor =
            Cream

        Me.Font =
            New Font(
                "Segoe UI",
                9.5F
            )


        '======================================================
        ' TITLE
        '======================================================

        lblTitle = New Label()

        lblTitle.Text =
            "Payment Management"

        lblTitle.Location =
            New Point(25, 20)

        lblTitle.Size =
            New Size(600, 40)

        lblTitle.Font =
            New Font(
                "Georgia",
                21,
                FontStyle.Bold
            )

        lblTitle.ForeColor =
            TextDark

        Me.Controls.Add(lblTitle)


        '======================================================
        ' SUBTITLE
        '======================================================

        lblSubtitle = New Label()

        lblSubtitle.Text =
            "Record rental payments and keep track of tenant balances."

        lblSubtitle.Location =
            New Point(27, 58)

        lblSubtitle.AutoSize = True

        lblSubtitle.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        lblSubtitle.ForeColor =
            Coffee

        Me.Controls.Add(lblSubtitle)


        '======================================================
        ' SEARCH
        '======================================================

        Dim lblSearch As Label =
            CreateLabel(
                "Search",
                New Point(25, 95)
            )

        Me.Controls.Add(lblSearch)


        txtSearch = New TextBox()

        txtSearch.Location =
            New Point(25, 120)

        txtSearch.Size =
            New Size(350, 32)

        txtSearch.Font =
            New Font(
                "Segoe UI",
                10
            )

        txtSearch.BorderStyle =
            BorderStyle.FixedSingle

        Me.Controls.Add(txtSearch)


        btnSearch =
            CreateButton(
                "Search",
                New Point(385, 118),
                New Size(100, 36),
                Coffee
            )

        Me.Controls.Add(btnSearch)


        btnRefresh =
            CreateButton(
                "Refresh",
                New Point(495, 118),
                New Size(100, 36),
                Caramel
            )

        Me.Controls.Add(btnRefresh)


        AddHandler btnSearch.Click,
            AddressOf SearchClicked

        AddHandler btnRefresh.Click,
            AddressOf RefreshClicked


        '======================================================
        ' FORM PANEL
        '======================================================

        Dim formPanel As New Panel()

        formPanel.Location =
            New Point(25, 170)

        formPanel.Size =
            New Size(1040, 285)

        formPanel.BackColor =
            CardColor

        Me.Controls.Add(formPanel)


        '======================================================
        ' PAYMENT ID
        '======================================================

        Dim lblPaymentID As Label =
            CreateLabel(
                "Payment ID",
                New Point(20, 15)
            )

        formPanel.Controls.Add(lblPaymentID)


        txtPaymentID =
            CreateTextBox()

        txtPaymentID.Location =
            New Point(20, 43)

        txtPaymentID.Size =
            New Size(130, 30)

        txtPaymentID.ReadOnly = True

        txtPaymentID.BackColor =
            Color.FromArgb(
                235,
                225,
                215
            )

        formPanel.Controls.Add(txtPaymentID)


        '======================================================
        ' TENANT
        '======================================================

        Dim lblTenant As Label =
            CreateLabel(
                "Tenant",
                New Point(170, 15)
            )

        formPanel.Controls.Add(lblTenant)


        cboTenant =
            CreateComboBox()

        cboTenant.Location =
            New Point(170, 43)

        cboTenant.Size =
            New Size(280, 30)

        formPanel.Controls.Add(cboTenant)


        AddHandler cboTenant.SelectedIndexChanged,
            AddressOf TenantChanged


        '======================================================
        ' PROPERTY
        '======================================================

        Dim lblProperty As Label =
            CreateLabel(
                "Rental Property",
                New Point(470, 15)
            )

        formPanel.Controls.Add(lblProperty)


        txtProperty =
            CreateTextBox()

        txtProperty.Location =
            New Point(470, 43)

        txtProperty.Size =
            New Size(270, 30)

        txtProperty.ReadOnly = True

        txtProperty.BackColor =
            Color.FromArgb(
                235,
                225,
                215
            )

        formPanel.Controls.Add(txtProperty)


        '======================================================
        ' PAYMENT DATE
        '======================================================

        Dim lblPaymentDate As Label =
            CreateLabel(
                "Payment Date",
                New Point(760, 15)
            )

        formPanel.Controls.Add(lblPaymentDate)


        dtpPaymentDate =
            CreateDatePicker()

        dtpPaymentDate.Location =
            New Point(760, 43)

        dtpPaymentDate.Size =
            New Size(250, 30)

        formPanel.Controls.Add(dtpPaymentDate)


        '======================================================
        ' AMOUNT DUE
        '======================================================

        Dim lblAmountDue As Label =
            CreateLabel(
                "Amount Due",
                New Point(20, 90)
            )

        formPanel.Controls.Add(lblAmountDue)


        txtAmountDue =
            CreateTextBox()

        txtAmountDue.Location =
            New Point(20, 118)

        txtAmountDue.Size =
            New Size(200, 30)

        formPanel.Controls.Add(txtAmountDue)


        '======================================================
        ' AMOUNT PAID
        '======================================================

        Dim lblAmountPaid As Label =
            CreateLabel(
                "Amount Paid",
                New Point(240, 90)
            )

        formPanel.Controls.Add(lblAmountPaid)


        txtAmountPaid =
            CreateTextBox()

        txtAmountPaid.Location =
            New Point(240, 118)

        txtAmountPaid.Size =
            New Size(200, 30)

        formPanel.Controls.Add(txtAmountPaid)


        '======================================================
        ' PAYMENT METHOD
        '======================================================

        Dim lblPaymentMethod As Label =
            CreateLabel(
                "Payment Method",
                New Point(460, 90)
            )

        formPanel.Controls.Add(lblPaymentMethod)


        cboPaymentMethod =
            CreateComboBox()

        cboPaymentMethod.Location =
            New Point(460, 118)

        cboPaymentMethod.Size =
            New Size(200, 30)

        cboPaymentMethod.Items.Add(
            "Cash"
        )

        cboPaymentMethod.Items.Add(
            "Bank Transfer"
        )

        cboPaymentMethod.Items.Add(
            "GCash"
        )

        cboPaymentMethod.Items.Add(
            "Maya"
        )

        cboPaymentMethod.Items.Add(
            "Credit/Debit Card"
        )

        formPanel.Controls.Add(cboPaymentMethod)


        '======================================================
        ' PAYMENT STATUS
        '======================================================

        Dim lblPaymentStatus As Label =
            CreateLabel(
                "Payment Status",
                New Point(680, 90)
            )

        formPanel.Controls.Add(lblPaymentStatus)


        cboPaymentStatus =
            CreateComboBox()

        cboPaymentStatus.Location =
            New Point(680, 118)

        cboPaymentStatus.Size =
            New Size(180, 30)

        cboPaymentStatus.Items.Add(
            "Paid"
        )

        cboPaymentStatus.Items.Add(
            "Unpaid"
        )

        cboPaymentStatus.Items.Add(
            "Partially Paid"
        )

        formPanel.Controls.Add(cboPaymentStatus)


        '======================================================
        ' NOTES
        '======================================================

        Dim lblNotes As Label =
            CreateLabel(
                "Notes",
                New Point(20, 165)
            )

        formPanel.Controls.Add(lblNotes)


        txtNotes =
            CreateTextBox()

        txtNotes.Location =
            New Point(20, 193)

        txtNotes.Size =
            New Size(420, 55)

        txtNotes.Multiline = True

        txtNotes.ScrollBars =
            ScrollBars.Vertical

        formPanel.Controls.Add(txtNotes)


        '======================================================
        ' BUTTONS
        '======================================================

        btnClear =
            CreateButton(
                "Clear",
                New Point(500, 195),
                New Size(105, 38),
                Sage
            )

        formPanel.Controls.Add(btnClear)


        btnAdd =
            CreateButton(
                "Add Payment",
                New Point(615, 195),
                New Size(120, 38),
                Coffee
            )

        formPanel.Controls.Add(btnAdd)


        btnUpdate =
            CreateButton(
                "Update",
                New Point(745, 195),
                New Size(110, 38),
                Caramel
            )

        formPanel.Controls.Add(btnUpdate)


        btnDelete =
            CreateButton(
                "Delete",
                New Point(865, 195),
                New Size(110, 38),
                RedBrown
            )

        formPanel.Controls.Add(btnDelete)


        AddHandler btnClear.Click,
            AddressOf ClearClicked

        AddHandler btnAdd.Click,
            AddressOf AddPaymentClicked

        AddHandler btnUpdate.Click,
            AddressOf UpdatePaymentClicked

        AddHandler btnDelete.Click,
            AddressOf DeletePaymentClicked


        '======================================================
        ' DATA GRID
        '======================================================

        dgvPayments =
            New DataGridView()

        dgvPayments.Location =
            New Point(25, 475)

        dgvPayments.Size =
            New Size(1040, 205)

        dgvPayments.BackgroundColor =
            CardColor

        dgvPayments.BorderStyle =
            BorderStyle.None

        dgvPayments.ReadOnly = True

        dgvPayments.AllowUserToAddRows =
            False

        dgvPayments.AllowUserToDeleteRows =
            False

        dgvPayments.RowHeadersVisible =
            False

        dgvPayments.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvPayments.MultiSelect =
            False

        dgvPayments.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvPayments.EnableHeadersVisualStyles =
            False

        dgvPayments.ColumnHeadersDefaultCellStyle.BackColor =
            CoffeeDark

        dgvPayments.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White

        dgvPayments.ColumnHeadersDefaultCellStyle.Font =
            New Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            )

        dgvPayments.DefaultCellStyle.BackColor =
            CardColor

        dgvPayments.DefaultCellStyle.ForeColor =
            TextDark

        dgvPayments.DefaultCellStyle.SelectionBackColor =
            Latte

        dgvPayments.DefaultCellStyle.SelectionForeColor =
            TextDark

        Me.Controls.Add(dgvPayments)


        AddHandler dgvPayments.CellClick,
            AddressOf PaymentRowClicked

    End Sub


    '==========================================================
    ' CREATE LABEL
    '==========================================================

    Private Function CreateLabel(
        text As String,
        location As Point
    ) As Label

        Dim label As New Label()

        label.Text =
            text

        label.Location =
            location

        label.AutoSize = True

        label.Font =
            New Font(
                "Segoe UI",
                8.5F,
                FontStyle.Bold
            )

        label.ForeColor =
            TextDark

        Return label

    End Function


    '==========================================================
    ' CREATE TEXTBOX
    '==========================================================

    Private Function CreateTextBox() As TextBox

        Dim textbox As New TextBox()

        textbox.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        textbox.BorderStyle =
            BorderStyle.FixedSingle

        textbox.BackColor =
            Color.White

        textbox.ForeColor =
            TextDark

        Return textbox

    End Function


    '==========================================================
    ' CREATE COMBOBOX
    '==========================================================

    Private Function CreateComboBox() As ComboBox

        Dim combo As New ComboBox()

        combo.DropDownStyle =
            ComboBoxStyle.DropDownList

        combo.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        combo.BackColor =
            Color.White

        combo.ForeColor =
            TextDark

        Return combo

    End Function


    '==========================================================
    ' CREATE DATE PICKER
    '==========================================================

    Private Function CreateDatePicker() As DateTimePicker

        Dim picker As New DateTimePicker()

        picker.Format =
            DateTimePickerFormat.Short

        picker.Font =
            New Font(
                "Segoe UI",
                9.5F
            )

        picker.Value =
            Date.Today

        Return picker

    End Function


    '==========================================================
    ' CREATE BUTTON
    '==========================================================

    Private Function CreateButton(
        text As String,
        location As Point,
        size As Size,
        buttonColor As Color
    ) As Button

        Dim button As New Button()

        button.Text =
            text

        button.Location =
            location

        button.Size =
            size

        button.BackColor =
            buttonColor

        button.ForeColor =
            Color.White

        button.FlatStyle =
            FlatStyle.Flat

        button.Font =
            New Font(
                "Segoe UI",
                8.5F,
                FontStyle.Bold
            )

        button.Cursor =
            Cursors.Hand

        button.UseVisualStyleBackColor =
            False

        button.FlatAppearance.BorderSize =
            0

        Return button

    End Function


    '==========================================================
    ' LOAD TENANTS
    '==========================================================

    Private Sub LoadTenants()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "t.TenantID, " &
                    "t.TenantName, " &
                    "t.PropertyID, " &
                    "p.PropertyName " &
                    "FROM Tenants t " &
                    "LEFT JOIN Properties p " &
                    "ON t.PropertyID = p.PropertyID " &
                    "WHERE t.IsActive = 1 " &
                    "ORDER BY t.TenantName"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using reader As SqlDataReader =
                        command.ExecuteReader()

                        cboTenant.Items.Clear()


                        While reader.Read()

                            Dim item As New TenantItem()

                            item.ID =
                                Convert.ToInt32(
                                    reader("TenantID")
                                )

                            item.Name =
                                reader(
                                    "TenantName"
                                ).ToString()


                            If Not IsDBNull(
                                reader("PropertyID")
                            ) Then

                                item.PropertyID =
                                    Convert.ToInt32(
                                        reader("PropertyID")
                                    )

                            Else

                                item.PropertyID = 0

                            End If


                            If Not IsDBNull(
                                reader("PropertyName")
                            ) Then

                                item.PropertyName =
                                    reader(
                                        "PropertyName"
                                    ).ToString()

                            Else

                                item.PropertyName =
                                    "No Property"

                            End If


                            cboTenant.Items.Add(
                                item
                            )

                        End While

                    End Using

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load tenants." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' TENANT ITEM
    '==========================================================

    Private Class TenantItem

        Public Property ID As Integer

        Public Property Name As String

        Public Property PropertyID As Integer

        Public Property PropertyName As String


        Public Overrides Function ToString() As String

            Return ID.ToString() &
                   " - " &
                   Name

        End Function

    End Class


    '==========================================================
    ' TENANT CHANGED
    '==========================================================

    Private Sub TenantChanged(
        sender As Object,
        e As EventArgs
    )

        Dim tenant As TenantItem =
            TryCast(
                cboTenant.SelectedItem,
                TenantItem
            )


        If tenant Is Nothing Then

            txtProperty.Clear()

            Return

        End If


        txtProperty.Text =
            tenant.PropertyName

    End Sub


    '==========================================================
    ' LOAD PAYMENTS
    '==========================================================

    Private Sub LoadPayments()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "pay.PaymentID, " &
                    "pay.TenantID, " &
                    "t.TenantName, " &
                    "p.PropertyName, " &
                    "pay.PaymentDate, " &
                    "pay.AmountDue, " &
                    "pay.AmountPaid, " &
                    "pay.PaymentMethod, " &
                    "pay.PaymentStatus, " &
                    "pay.Notes " &
                    "FROM Payments pay " &
                    "INNER JOIN Tenants t " &
                    "ON pay.TenantID = t.TenantID " &
                    "LEFT JOIN Properties p " &
                    "ON t.PropertyID = p.PropertyID " &
                    "WHERE pay.IsActive = 1 " &
                    "ORDER BY pay.PaymentID DESC"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvPayments.DataSource =
                            table

                    End Using

                End Using

            End Using


            FormatGrid()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load payments." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' FORMAT GRID
    '==========================================================

    Private Sub FormatGrid()

        If dgvPayments.Columns.Count = 0 Then

            Return

        End If


        If dgvPayments.Columns.Contains(
            "PaymentID"
        ) Then

            dgvPayments.Columns(
                "PaymentID"
            ).HeaderText =
                "ID"

        End If


        If dgvPayments.Columns.Contains(
            "TenantID"
        ) Then

            dgvPayments.Columns(
                "TenantID"
            ).Visible = False

        End If


        If dgvPayments.Columns.Contains(
            "TenantName"
        ) Then

            dgvPayments.Columns(
                "TenantName"
            ).HeaderText =
                "Tenant"

        End If


        If dgvPayments.Columns.Contains(
            "PropertyName"
        ) Then

            dgvPayments.Columns(
                "PropertyName"
            ).HeaderText =
                "Property"

        End If


        If dgvPayments.Columns.Contains(
            "PaymentDate"
        ) Then

            dgvPayments.Columns(
                "PaymentDate"
            ).HeaderText =
                "Payment Date"

        End If


        If dgvPayments.Columns.Contains(
            "AmountDue"
        ) Then

            dgvPayments.Columns(
                "AmountDue"
            ).HeaderText =
                "Amount Due"

        End If


        If dgvPayments.Columns.Contains(
            "AmountPaid"
        ) Then

            dgvPayments.Columns(
                "AmountPaid"
            ).HeaderText =
                "Amount Paid"

        End If


        If dgvPayments.Columns.Contains(
            "PaymentMethod"
        ) Then

            dgvPayments.Columns(
                "PaymentMethod"
            ).HeaderText =
                "Method"

        End If


        If dgvPayments.Columns.Contains(
            "PaymentStatus"
        ) Then

            dgvPayments.Columns(
                "PaymentStatus"
            ).HeaderText =
                "Status"

        End If


        If dgvPayments.Columns.Contains(
            "Notes"
        ) Then

            dgvPayments.Columns(
                "Notes"
            ).HeaderText =
                "Notes"

        End If


        dgvPayments.ClearSelection()

    End Sub


    '==========================================================
    ' ADD PAYMENT
    '==========================================================

    Private Sub AddPaymentClicked(
        sender As Object,
        e As EventArgs
    )

        If Not ValidateFields() Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "INSERT INTO Payments " &
                    "(" &
                    "TenantID, " &
                    "PaymentDate, " &
                    "AmountDue, " &
                    "AmountPaid, " &
                    "PaymentMethod, " &
                    "PaymentStatus, " &
                    "Notes, " &
                    "IsActive" &
                    ") " &
                    "VALUES " &
                    "(" &
                    "@TenantID, " &
                    "@PaymentDate, " &
                    "@AmountDue, " &
                    "@AmountPaid, " &
                    "@PaymentMethod, " &
                    "@PaymentStatus, " &
                    "@Notes, " &
                    "1" &
                    ")"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    AddParameters(command)

                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Payment added successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadPayments()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to add the payment." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' UPDATE PAYMENT
    '==========================================================

    Private Sub UpdatePaymentClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedPaymentID = 0 Then

            MessageBox.Show(
                "Please select a payment first.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            Return

        End If


        If Not ValidateFields() Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "UPDATE Payments SET " &
                    "TenantID = @TenantID, " &
                    "PaymentDate = @PaymentDate, " &
                    "AmountDue = @AmountDue, " &
                    "AmountPaid = @AmountPaid, " &
                    "PaymentMethod = @PaymentMethod, " &
                    "PaymentStatus = @PaymentStatus, " &
                    "Notes = @Notes " &
                    "WHERE PaymentID = @PaymentID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    AddParameters(command)


                    command.Parameters.Add(
                        "@PaymentID",
                        SqlDbType.Int
                    ).Value =
                        selectedPaymentID


                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Payment updated successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadPayments()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to update the payment." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' DELETE PAYMENT
    '==========================================================

    Private Sub DeletePaymentClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedPaymentID = 0 Then

            MessageBox.Show(
                "Please select a payment first.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            Return

        End If


        Dim answer As DialogResult =
            MessageBox.Show(
                "Are you sure you want to remove this payment record?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )


        If answer =
            DialogResult.No Then

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "UPDATE Payments " &
                    "SET IsActive = 0 " &
                    "WHERE PaymentID = @PaymentID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.Add(
                        "@PaymentID",
                        SqlDbType.Int
                    ).Value =
                        selectedPaymentID


                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Payment removed successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadPayments()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to delete the payment." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' SEARCH
    '==========================================================

    Private Sub SearchClicked(
        sender As Object,
        e As EventArgs
    )

        Dim searchText As String =
            txtSearch.Text.Trim()


        If searchText = "" Then

            LoadPayments()

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "pay.PaymentID, " &
                    "pay.TenantID, " &
                    "t.TenantName, " &
                    "p.PropertyName, " &
                    "pay.PaymentDate, " &
                    "pay.AmountDue, " &
                    "pay.AmountPaid, " &
                    "pay.PaymentMethod, " &
                    "pay.PaymentStatus, " &
                    "pay.Notes " &
                    "FROM Payments pay " &
                    "INNER JOIN Tenants t " &
                    "ON pay.TenantID = t.TenantID " &
                    "LEFT JOIN Properties p " &
                    "ON t.PropertyID = p.PropertyID " &
                    "WHERE pay.IsActive = 1 " &
                    "AND (" &
                    "CAST(pay.PaymentID AS NVARCHAR(20)) LIKE @Search " &
                    "OR t.TenantName LIKE @Search " &
                    "OR ISNULL(p.PropertyName, '') LIKE @Search " &
                    "OR pay.PaymentMethod LIKE @Search " &
                    "OR pay.PaymentStatus LIKE @Search " &
                    "OR ISNULL(pay.Notes, '') LIKE @Search" &
                    ") " &
                    "ORDER BY pay.PaymentID DESC"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.Add(
                        "@Search",
                        SqlDbType.NVarChar,
                        200
                    ).Value =
                        "%" & searchText & "%"


                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvPayments.DataSource =
                            table

                    End Using

                End Using

            End Using


            FormatGrid()


        Catch ex As Exception

            MessageBox.Show(
                "Search failed." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '==========================================================
    ' REFRESH
    '==========================================================

    Private Sub RefreshClicked(
        sender As Object,
        e As EventArgs
    )

        txtSearch.Clear()

        LoadTenants()

        LoadPayments()

        ClearFields()

    End Sub


    '==========================================================
    ' PAYMENT ROW CLICKED
    '==========================================================

    Private Sub PaymentRowClicked(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then

            Return

        End If


        Dim row As DataGridViewRow =
            dgvPayments.Rows(
                e.RowIndex
            )


        If row.Cells("PaymentID").Value Is Nothing Then

            Return

        End If


        If IsDBNull(
            row.Cells("PaymentID").Value
        ) Then

            Return

        End If


        selectedPaymentID =
            Convert.ToInt32(
                row.Cells("PaymentID").Value
            )


        txtPaymentID.Text =
            selectedPaymentID.ToString()


        '------------------------------------------------------
        ' TENANT
        '------------------------------------------------------

        If Not IsDBNull(
            row.Cells("TenantID").Value
        ) Then

            Dim tenantID As Integer =
                Convert.ToInt32(
                    row.Cells("TenantID").Value
                )


            For i As Integer =
                0 To cboTenant.Items.Count - 1

                Dim tenant As TenantItem =
                    TryCast(
                        cboTenant.Items(i),
                        TenantItem
                    )


                If tenant IsNot Nothing AndAlso
                   tenant.ID = tenantID Then

                    cboTenant.SelectedIndex =
                        i

                    Exit For

                End If

            Next

        End If


        '------------------------------------------------------
        ' PAYMENT DATE
        '------------------------------------------------------

        If Not IsDBNull(
            row.Cells("PaymentDate").Value
        ) Then

            dtpPaymentDate.Value =
                Convert.ToDateTime(
                    row.Cells("PaymentDate").Value
                )

        End If


        '------------------------------------------------------
        ' AMOUNTS
        '------------------------------------------------------

        If Not IsDBNull(
            row.Cells("AmountDue").Value
        ) Then

            txtAmountDue.Text =
                Convert.ToDecimal(
                    row.Cells("AmountDue").Value
                ).ToString("0.00")

        Else

            txtAmountDue.Clear()

        End If


        If Not IsDBNull(
            row.Cells("AmountPaid").Value
        ) Then

            txtAmountPaid.Text =
                Convert.ToDecimal(
                    row.Cells("AmountPaid").Value
                ).ToString("0.00")

        Else

            txtAmountPaid.Clear()

        End If


        '------------------------------------------------------
        ' METHOD
        '------------------------------------------------------

        cboPaymentMethod.Text =
            GetCellString(
                row,
                "PaymentMethod"
            )


        '------------------------------------------------------
        ' STATUS
        '------------------------------------------------------

        cboPaymentStatus.Text =
            GetCellString(
                row,
                "PaymentStatus"
            )


        '------------------------------------------------------
        ' NOTES
        '------------------------------------------------------

        txtNotes.Text =
            GetCellString(
                row,
                "Notes"
            )

    End Sub


    '==========================================================
    ' GET CELL STRING
    '==========================================================

    Private Function GetCellString(
        row As DataGridViewRow,
        columnName As String
    ) As String

        If row.Cells(columnName).Value Is Nothing Then

            Return ""

        End If


        If IsDBNull(
            row.Cells(columnName).Value
        ) Then

            Return ""

        End If


        Return row.Cells(
            columnName
        ).Value.ToString()

    End Function


    '==========================================================
    ' VALIDATE
    '==========================================================

    Private Function ValidateFields() As Boolean

        If cboTenant.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a tenant.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboTenant.Focus()

            Return False

        End If


        Dim amountDue As Decimal

        If Not Decimal.TryParse(
            txtAmountDue.Text.Trim(),
            amountDue
        ) Then

            MessageBox.Show(
                "Please enter a valid amount due.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtAmountDue.Focus()

            Return False

        End If


        If amountDue < 0 Then

            MessageBox.Show(
                "Amount due cannot be negative.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return False

        End If


        Dim amountPaid As Decimal

        If Not Decimal.TryParse(
            txtAmountPaid.Text.Trim(),
            amountPaid
        ) Then

            MessageBox.Show(
                "Please enter a valid amount paid.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtAmountPaid.Focus()

            Return False

        End If


        If amountPaid < 0 Then

            MessageBox.Show(
                "Amount paid cannot be negative.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return False

        End If


        If amountPaid > amountDue Then

            MessageBox.Show(
                "Amount paid cannot be greater than the amount due.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtAmountPaid.Focus()

            Return False

        End If


        If cboPaymentMethod.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a payment method.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboPaymentMethod.Focus()

            Return False

        End If


        If cboPaymentStatus.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a payment status.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboPaymentStatus.Focus()

            Return False

        End If


        Return True

    End Function


    '==========================================================
    ' SQL PARAMETERS
    '==========================================================

    Private Sub AddParameters(
        command As SqlCommand
    )

        Dim tenant As TenantItem =
            TryCast(
                cboTenant.SelectedItem,
                TenantItem
            )


        command.Parameters.Add(
            "@TenantID",
            SqlDbType.Int
        ).Value =
            tenant.ID


        command.Parameters.Add(
            "@PaymentDate",
            SqlDbType.Date
        ).Value =
            dtpPaymentDate.Value.Date


        Dim amountDue As Decimal

        Decimal.TryParse(
            txtAmountDue.Text.Trim(),
            amountDue
        )


        command.Parameters.Add(
            "@AmountDue",
            SqlDbType.Decimal
        ).Precision = 18

        command.Parameters(
            "@AmountDue"
        ).Scale = 2

        command.Parameters(
            "@AmountDue"
        ).Value =
            amountDue


        Dim amountPaid As Decimal

        Decimal.TryParse(
            txtAmountPaid.Text.Trim(),
            amountPaid
        )


        command.Parameters.Add(
            "@AmountPaid",
            SqlDbType.Decimal
        ).Precision = 18

        command.Parameters(
            "@AmountPaid"
        ).Scale = 2

        command.Parameters(
            "@AmountPaid"
        ).Value =
            amountPaid


        command.Parameters.Add(
            "@PaymentMethod",
            SqlDbType.NVarChar,
            50
        ).Value =
            cboPaymentMethod.Text


        command.Parameters.Add(
            "@PaymentStatus",
            SqlDbType.NVarChar,
            50
        ).Value =
            cboPaymentStatus.Text


        If txtNotes.Text.Trim() = "" Then

            command.Parameters.Add(
                "@Notes",
                SqlDbType.NVarChar,
                500
            ).Value =
                DBNull.Value

        Else

            command.Parameters.Add(
                "@Notes",
                SqlDbType.NVarChar,
                500
            ).Value =
                txtNotes.Text.Trim()

        End If

    End Sub


    '==========================================================
    ' CLEAR BUTTON
    '==========================================================

    Private Sub ClearClicked(
        sender As Object,
        e As EventArgs
    )

        ClearFields()

    End Sub


    '==========================================================
    ' CLEAR FIELDS
    '==========================================================

    Private Sub ClearFields()

        selectedPaymentID = 0

        txtPaymentID.Clear()

        cboTenant.SelectedIndex = -1

        txtProperty.Clear()

        dtpPaymentDate.Value =
            Date.Today

        txtAmountDue.Clear()

        txtAmountPaid.Clear()

        cboPaymentMethod.SelectedIndex = -1

        cboPaymentStatus.SelectedIndex = -1

        txtNotes.Clear()

        dgvPayments.ClearSelection()

    End Sub

End Class