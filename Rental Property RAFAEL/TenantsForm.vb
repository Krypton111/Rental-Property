Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class TenantsForm
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


    '==========================================================
    ' CONTROLS
    '==========================================================

    Private lblTitle As Label
    Private lblSubtitle As Label

    Private txtSearch As TextBox
    Private btnSearch As Button
    Private btnRefresh As Button

    Private txtTenantID As TextBox
    Private txtTenantName As TextBox
    Private txtContactNumber As TextBox
    Private txtEmail As TextBox
    Private txtAddress As TextBox

    Private cboProperty As ComboBox

    Private dtpLeaseStart As DateTimePicker
    Private dtpLeaseEnd As DateTimePicker

    Private cboPaymentStatus As ComboBox

    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnDelete As Button
    Private btnClear As Button

    Private dgvTenants As DataGridView

    Private selectedTenantID As Integer = 0


    '==========================================================
    ' CONSTRUCTOR
    '==========================================================

    Public Sub New()

        BuildForm()

        LoadPropertiesIntoComboBox()

        LoadTenants()

    End Sub


    '==========================================================
    ' BUILD FORM
    '==========================================================

    Private Sub BuildForm()

        Me.Text =
            "Casa Brew - Tenants"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(1100, 700)

        Me.MinimumSize =
            New Size(1000, 650)

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
            "Tenant Management"

        lblTitle.Location =
            New Point(25, 20)

        lblTitle.Size =
            New Size(500, 40)

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
            "Manage tenant information and active rental agreements."

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
            New Size(340, 32)

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
                New Point(375, 118),
                New Size(100, 36),
                Coffee
            )

        Me.Controls.Add(btnSearch)


        btnRefresh =
            CreateButton(
                "Refresh",
                New Point(485, 118),
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
            New Size(1020, 260)

        formPanel.BackColor =
            CardColor

        Me.Controls.Add(formPanel)


        '======================================================
        ' TENANT ID
        '======================================================

        Dim lblTenantID As Label =
            CreateLabel(
                "Tenant ID",
                New Point(20, 15)
            )

        formPanel.Controls.Add(lblTenantID)


        txtTenantID =
            CreateTextBox()

        txtTenantID.Location =
            New Point(20, 45)

        txtTenantID.Size =
            New Size(145, 30)

        txtTenantID.ReadOnly = True

        txtTenantID.BackColor =
            Color.FromArgb(
                235,
                225,
                215
            )

        formPanel.Controls.Add(txtTenantID)


        '======================================================
        ' TENANT NAME
        '======================================================

        Dim lblTenantName As Label =
            CreateLabel(
                "Tenant Name",
                New Point(185, 15)
            )

        formPanel.Controls.Add(lblTenantName)


        txtTenantName =
            CreateTextBox()

        txtTenantName.Location =
            New Point(185, 45)

        txtTenantName.Size =
            New Size(300, 30)

        formPanel.Controls.Add(txtTenantName)


        '======================================================
        ' CONTACT NUMBER
        '======================================================

        Dim lblContact As Label =
            CreateLabel(
                "Contact Number",
                New Point(505, 15)
            )

        formPanel.Controls.Add(lblContact)


        txtContactNumber =
            CreateTextBox()

        txtContactNumber.Location =
            New Point(505, 45)

        txtContactNumber.Size =
            New Size(200, 30)

        formPanel.Controls.Add(txtContactNumber)


        '======================================================
        ' EMAIL
        '======================================================

        Dim lblEmail As Label =
            CreateLabel(
                "Email Address",
                New Point(725, 15)
            )

        formPanel.Controls.Add(lblEmail)


        txtEmail =
            CreateTextBox()

        txtEmail.Location =
            New Point(725, 45)

        txtEmail.Size =
            New Size(270, 30)

        formPanel.Controls.Add(txtEmail)


        '======================================================
        ' ADDRESS
        '======================================================

        Dim lblAddress As Label =
            CreateLabel(
                "Home Address",
                New Point(20, 85)
            )

        formPanel.Controls.Add(lblAddress)


        txtAddress =
            CreateTextBox()

        txtAddress.Location =
            New Point(20, 115)

        txtAddress.Size =
            New Size(300, 30)

        formPanel.Controls.Add(txtAddress)


        '======================================================
        ' PROPERTY
        '======================================================

        Dim lblProperty As Label =
            CreateLabel(
                "Rental Property",
                New Point(340, 85)
            )

        formPanel.Controls.Add(lblProperty)


        cboProperty =
            CreateComboBox()

        cboProperty.Location =
            New Point(340, 115)

        cboProperty.Size =
            New Size(250, 30)

        formPanel.Controls.Add(cboProperty)


        '======================================================
        ' PAYMENT STATUS
        '======================================================

        Dim lblPaymentStatus As Label =
            CreateLabel(
                "Payment Status",
                New Point(610, 85)
            )

        formPanel.Controls.Add(lblPaymentStatus)


        cboPaymentStatus =
            CreateComboBox()

        cboPaymentStatus.Location =
            New Point(610, 115)

        cboPaymentStatus.Size =
            New Size(175, 30)

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
        ' LEASE START
        '======================================================

        Dim lblLeaseStart As Label =
            CreateLabel(
                "Lease Start Date",
                New Point(805, 85)
            )

        formPanel.Controls.Add(lblLeaseStart)


        dtpLeaseStart =
            CreateDatePicker()

        dtpLeaseStart.Location =
            New Point(805, 115)

        dtpLeaseStart.Size =
            New Size(190, 30)

        formPanel.Controls.Add(dtpLeaseStart)


        '======================================================
        ' LEASE END
        '======================================================

        Dim lblLeaseEnd As Label =
            CreateLabel(
                "Lease End Date",
                New Point(20, 165)
            )

        formPanel.Controls.Add(lblLeaseEnd)


        dtpLeaseEnd =
            CreateDatePicker()

        dtpLeaseEnd.Location =
            New Point(20, 195)

        dtpLeaseEnd.Size =
            New Size(200, 30)

        formPanel.Controls.Add(dtpLeaseEnd)


        '======================================================
        ' BUTTONS
        '======================================================

        btnClear =
            CreateButton(
                "Clear",
                New Point(500, 185),
                New Size(105, 38),
                Sage
            )

        formPanel.Controls.Add(btnClear)


        btnAdd =
            CreateButton(
                "Add Tenant",
                New Point(615, 185),
                New Size(110, 38),
                Coffee
            )

        formPanel.Controls.Add(btnAdd)


        btnUpdate =
            CreateButton(
                "Update",
                New Point(735, 185),
                New Size(110, 38),
                Caramel
            )

        formPanel.Controls.Add(btnUpdate)


        btnDelete =
            CreateButton(
                "Delete",
                New Point(855, 185),
                New Size(110, 38),
                Color.FromArgb(
                    145,
                    75,
                    65
                )
            )

        formPanel.Controls.Add(btnDelete)


        AddHandler btnClear.Click,
            AddressOf ClearClicked

        AddHandler btnAdd.Click,
            AddressOf AddTenantClicked

        AddHandler btnUpdate.Click,
            AddressOf UpdateTenantClicked

        AddHandler btnDelete.Click,
            AddressOf DeleteTenantClicked


        '======================================================
        ' DATA GRID
        '======================================================

        dgvTenants =
            New DataGridView()

        dgvTenants.Location =
            New Point(25, 450)

        dgvTenants.Size =
            New Size(1020, 190)

        dgvTenants.BackgroundColor =
            CardColor

        dgvTenants.BorderStyle =
            BorderStyle.None

        dgvTenants.ReadOnly = True

        dgvTenants.AllowUserToAddRows =
            False

        dgvTenants.AllowUserToDeleteRows =
            False

        dgvTenants.RowHeadersVisible =
            False

        dgvTenants.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvTenants.MultiSelect =
            False

        dgvTenants.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvTenants.EnableHeadersVisualStyles =
            False

        dgvTenants.ColumnHeadersDefaultCellStyle.BackColor =
            CoffeeDark

        dgvTenants.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White

        dgvTenants.ColumnHeadersDefaultCellStyle.Font =
            New Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            )

        dgvTenants.DefaultCellStyle.BackColor =
            CardColor

        dgvTenants.DefaultCellStyle.ForeColor =
            TextDark

        dgvTenants.DefaultCellStyle.SelectionBackColor =
            Latte

        dgvTenants.DefaultCellStyle.SelectionForeColor =
            TextDark

        Me.Controls.Add(dgvTenants)


        AddHandler dgvTenants.CellClick,
            AddressOf TenantRowClicked

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
    ' LOAD PROPERTIES INTO COMBOBOX
    '==========================================================

    Private Sub LoadPropertiesIntoComboBox()

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT PropertyID, PropertyName " &
                    "FROM Properties " &
                    "WHERE IsActive = 1 " &
                    "ORDER BY PropertyName"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using reader As SqlDataReader =
                        command.ExecuteReader()

                        cboProperty.Items.Clear()


                        While reader.Read()

                            Dim item As New PropertyItem()

                            item.ID =
                                Convert.ToInt32(
                                    reader("PropertyID")
                                )

                            item.Name =
                                reader(
                                    "PropertyName"
                                ).ToString()


                            cboProperty.Items.Add(
                                item
                            )

                        End While

                    End Using

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load rental properties." &
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
    ' PROPERTY ITEM
    '==========================================================

    Private Class PropertyItem

        Public Property ID As Integer

        Public Property Name As String


        Public Overrides Function ToString() As String

            Return ID.ToString() &
                   " - " &
                   Name

        End Function

    End Class


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
                    "t.ContactNumber, " &
                    "t.Email, " &
                    "t.Address, " &
                    "t.PropertyID, " &
                    "p.PropertyName, " &
                    "t.LeaseStartDate, " &
                    "t.LeaseEndDate, " &
                    "t.PaymentStatus " &
                    "FROM Tenants t " &
                    "LEFT JOIN Properties p " &
                    "ON t.PropertyID = p.PropertyID " &
                    "WHERE t.IsActive = 1 " &
                    "ORDER BY t.TenantID DESC"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Using adapter As New SqlDataAdapter(
                        command
                    )

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvTenants.DataSource =
                            table

                    End Using

                End Using

            End Using


            FormatGrid()


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
    ' FORMAT GRID
    '==========================================================

    Private Sub FormatGrid()

        If dgvTenants.Columns.Count = 0 Then

            Return

        End If


        If dgvTenants.Columns.Contains(
            "TenantID"
        ) Then

            dgvTenants.Columns(
                "TenantID"
            ).HeaderText =
                "ID"

        End If


        If dgvTenants.Columns.Contains(
            "TenantName"
        ) Then

            dgvTenants.Columns(
                "TenantName"
            ).HeaderText =
                "Tenant Name"

        End If


        If dgvTenants.Columns.Contains(
            "ContactNumber"
        ) Then

            dgvTenants.Columns(
                "ContactNumber"
            ).HeaderText =
                "Contact"

        End If


        If dgvTenants.Columns.Contains(
            "Email"
        ) Then

            dgvTenants.Columns(
                "Email"
            ).HeaderText =
                "Email"

        End If


        If dgvTenants.Columns.Contains(
            "Address"
        ) Then

            dgvTenants.Columns(
                "Address"
            ).HeaderText =
                "Address"

        End If


        If dgvTenants.Columns.Contains(
            "PropertyID"
        ) Then

            dgvTenants.Columns(
                "PropertyID"
            ).HeaderText =
                "Property ID"

        End If


        If dgvTenants.Columns.Contains(
            "PropertyName"
        ) Then

            dgvTenants.Columns(
                "PropertyName"
            ).HeaderText =
                "Property"

        End If


        If dgvTenants.Columns.Contains(
            "LeaseStartDate"
        ) Then

            dgvTenants.Columns(
                "LeaseStartDate"
            ).HeaderText =
                "Lease Start"

        End If


        If dgvTenants.Columns.Contains(
            "LeaseEndDate"
        ) Then

            dgvTenants.Columns(
                "LeaseEndDate"
            ).HeaderText =
                "Lease End"

        End If


        If dgvTenants.Columns.Contains(
            "PaymentStatus"
        ) Then

            dgvTenants.Columns(
                "PaymentStatus"
            ).HeaderText =
                "Payment"

        End If


        dgvTenants.ClearSelection()

    End Sub


    '==========================================================
    ' ADD TENANT
    '==========================================================

    Private Sub AddTenantClicked(
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
                    "INSERT INTO Tenants " &
                    "(" &
                    "TenantName, " &
                    "ContactNumber, " &
                    "Email, " &
                    "Address, " &
                    "PropertyID, " &
                    "LeaseStartDate, " &
                    "LeaseEndDate, " &
                    "PaymentStatus, " &
                    "IsActive" &
                    ") " &
                    "VALUES " &
                    "(" &
                    "@TenantName, " &
                    "@ContactNumber, " &
                    "@Email, " &
                    "@Address, " &
                    "@PropertyID, " &
                    "@LeaseStartDate, " &
                    "@LeaseEndDate, " &
                    "@PaymentStatus, " &
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
                "Tenant added successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadTenants()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to add the tenant." &
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
    ' UPDATE TENANT
    '==========================================================

    Private Sub UpdateTenantClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedTenantID = 0 Then

            MessageBox.Show(
                "Please select a tenant first.",
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
                    "UPDATE Tenants SET " &
                    "TenantName = @TenantName, " &
                    "ContactNumber = @ContactNumber, " &
                    "Email = @Email, " &
                    "Address = @Address, " &
                    "PropertyID = @PropertyID, " &
                    "LeaseStartDate = @LeaseStartDate, " &
                    "LeaseEndDate = @LeaseEndDate, " &
                    "PaymentStatus = @PaymentStatus " &
                    "WHERE TenantID = @TenantID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    AddParameters(command)


                    command.Parameters.Add(
                        "@TenantID",
                        SqlDbType.Int
                    ).Value =
                        selectedTenantID


                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Tenant updated successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadTenants()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to update the tenant." &
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
    ' DELETE TENANT
    '==========================================================

    Private Sub DeleteTenantClicked(
        sender As Object,
        e As EventArgs
    )

        If selectedTenantID = 0 Then

            MessageBox.Show(
                "Please select a tenant first.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            Return

        End If


        Dim answer As DialogResult =
            MessageBox.Show(
                "Are you sure you want to remove this tenant?",
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
                    "UPDATE Tenants " &
                    "SET IsActive = 0 " &
                    "WHERE TenantID = @TenantID"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.Add(
                        "@TenantID",
                        SqlDbType.Int
                    ).Value =
                        selectedTenantID


                    command.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Tenant removed successfully.",
                "Casa Brew",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadTenants()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Unable to delete the tenant." &
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

            LoadTenants()

            Return

        End If


        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT " &
                    "t.TenantID, " &
                    "t.TenantName, " &
                    "t.ContactNumber, " &
                    "t.Email, " &
                    "t.Address, " &
                    "t.PropertyID, " &
                    "p.PropertyName, " &
                    "t.LeaseStartDate, " &
                    "t.LeaseEndDate, " &
                    "t.PaymentStatus " &
                    "FROM Tenants t " &
                    "LEFT JOIN Properties p " &
                    "ON t.PropertyID = p.PropertyID " &
                    "WHERE t.IsActive = 1 " &
                    "AND (" &
                    "CAST(t.TenantID AS NVARCHAR(20)) LIKE @Search " &
                    "OR t.TenantName LIKE @Search " &
                    "OR ISNULL(t.ContactNumber, '') LIKE @Search " &
                    "OR ISNULL(t.Email, '') LIKE @Search " &
                    "OR ISNULL(t.Address, '') LIKE @Search " &
                    "OR ISNULL(p.PropertyName, '') LIKE @Search " &
                    "OR t.PaymentStatus LIKE @Search" &
                    ") " &
                    "ORDER BY t.TenantID DESC"


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

                        dgvTenants.DataSource =
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

        LoadPropertiesIntoComboBox()

        LoadTenants()

        ClearFields()

    End Sub


    '==========================================================
    ' SELECT TENANT
    '==========================================================

    Private Sub TenantRowClicked(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then

            Return

        End If


        Dim row As DataGridViewRow =
            dgvTenants.Rows(
                e.RowIndex
            )


        If row.Cells("TenantID").Value Is Nothing Then

            Return

        End If


        If IsDBNull(
            row.Cells("TenantID").Value
        ) Then

            Return

        End If


        selectedTenantID =
            Convert.ToInt32(
                row.Cells("TenantID").Value
            )


        txtTenantID.Text =
            selectedTenantID.ToString()


        txtTenantName.Text =
            GetCellString(
                row,
                "TenantName"
            )


        txtContactNumber.Text =
            GetCellString(
                row,
                "ContactNumber"
            )


        txtEmail.Text =
            GetCellString(
                row,
                "Email"
            )


        txtAddress.Text =
            GetCellString(
                row,
                "Address"
            )


        cboPaymentStatus.Text =
            GetCellString(
                row,
                "PaymentStatus"
            )


        '------------------------------------------------------
        ' SELECT PROPERTY
        '------------------------------------------------------

        If Not IsDBNull(
            row.Cells("PropertyID").Value
        ) Then

            Dim propertyID As Integer =
                Convert.ToInt32(
                    row.Cells("PropertyID").Value
                )


            For i As Integer =
                0 To cboProperty.Items.Count - 1

                Dim item As PropertyItem =
                    TryCast(
                        cboProperty.Items(i),
                        PropertyItem
                    )


                If item IsNot Nothing AndAlso
                   item.ID = propertyID Then

                    cboProperty.SelectedIndex =
                        i

                    Exit For

                End If

            Next

        Else

            cboProperty.SelectedIndex = -1

        End If


        '------------------------------------------------------
        ' LEASE DATES
        '------------------------------------------------------

        If Not IsDBNull(
            row.Cells("LeaseStartDate").Value
        ) Then

            dtpLeaseStart.Value =
                Convert.ToDateTime(
                    row.Cells("LeaseStartDate").Value
                )

        End If


        If Not IsDBNull(
            row.Cells("LeaseEndDate").Value
        ) Then

            dtpLeaseEnd.Value =
                Convert.ToDateTime(
                    row.Cells("LeaseEndDate").Value
                )

        End If

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

        If txtTenantName.Text.Trim() = "" Then

            MessageBox.Show(
                "Please enter the tenant name.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtTenantName.Focus()

            Return False

        End If


        If txtContactNumber.Text.Trim() = "" Then

            MessageBox.Show(
                "Please enter the tenant contact number.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtContactNumber.Focus()

            Return False

        End If


        If cboProperty.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a rental property.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cboProperty.Focus()

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


        If dtpLeaseEnd.Value <
           dtpLeaseStart.Value Then

            MessageBox.Show(
                "Lease end date cannot be earlier than lease start date.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

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

        command.Parameters.Add(
            "@TenantName",
            SqlDbType.NVarChar,
            150
        ).Value =
            txtTenantName.Text.Trim()


        If txtContactNumber.Text.Trim() = "" Then

            command.Parameters.Add(
                "@ContactNumber",
                SqlDbType.NVarChar,
                30
            ).Value =
                DBNull.Value

        Else

            command.Parameters.Add(
                "@ContactNumber",
                SqlDbType.NVarChar,
                30
            ).Value =
                txtContactNumber.Text.Trim()

        End If


        If txtEmail.Text.Trim() = "" Then

            command.Parameters.Add(
                "@Email",
                SqlDbType.NVarChar,
                150
            ).Value =
                DBNull.Value

        Else

            command.Parameters.Add(
                "@Email",
                SqlDbType.NVarChar,
                150
            ).Value =
                txtEmail.Text.Trim()

        End If


        If txtAddress.Text.Trim() = "" Then

            command.Parameters.Add(
                "@Address",
                SqlDbType.NVarChar,
                300
            ).Value =
                DBNull.Value

        Else

            command.Parameters.Add(
                "@Address",
                SqlDbType.NVarChar,
                300
            ).Value =
                txtAddress.Text.Trim()

        End If


        Dim selectedProperty As PropertyItem =
            TryCast(
                cboProperty.SelectedItem,
                PropertyItem
            )


        If selectedProperty Is Nothing Then

            command.Parameters.Add(
                "@PropertyID",
                SqlDbType.Int
            ).Value =
                DBNull.Value

        Else

            command.Parameters.Add(
                "@PropertyID",
                SqlDbType.Int
            ).Value =
                selectedProperty.ID

        End If


        command.Parameters.Add(
            "@LeaseStartDate",
            SqlDbType.Date
        ).Value =
            dtpLeaseStart.Value.Date


        command.Parameters.Add(
            "@LeaseEndDate",
            SqlDbType.Date
        ).Value =
            dtpLeaseEnd.Value.Date


        command.Parameters.Add(
            "@PaymentStatus",
            SqlDbType.NVarChar,
            50
        ).Value =
            cboPaymentStatus.Text

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

        selectedTenantID = 0

        txtTenantID.Clear()

        txtTenantName.Clear()

        txtContactNumber.Clear()

        txtEmail.Clear()

        txtAddress.Clear()

        cboProperty.SelectedIndex = -1

        cboPaymentStatus.SelectedIndex = -1

        dtpLeaseStart.Value =
            Date.Today

        dtpLeaseEnd.Value =
            Date.Today

        dgvTenants.ClearSelection()

    End Sub

End Class