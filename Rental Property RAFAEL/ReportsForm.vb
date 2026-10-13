Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Text
Imports Microsoft.Data.SqlClient
Imports System.Windows.Forms

Public Class ReportsForm
    Inherits Form

    '===========================================================
    ' DATABASE
    '===========================================================

    Private ReadOnly ConnectionString As String =
        "Data Source=LAPTOP-A7AVPJM9\SENSERVER;" &
        "Initial Catalog=RentalPropertyDB;" &
        "User ID=sa;" &
        "Password=B1Admin;" &
        "TrustServerCertificate=True;"

    '===========================================================
    ' COLORS - COZY COFFEE CAFE
    '===========================================================

    Private ReadOnly CoffeeDark As Color = Color.FromArgb(62, 39, 35)
    Private ReadOnly CoffeeBrown As Color = Color.FromArgb(93, 64, 55)
    Private ReadOnly CoffeeMedium As Color = Color.FromArgb(121, 85, 72)
    Private ReadOnly CoffeeLight As Color = Color.FromArgb(188, 170, 164)
    Private ReadOnly Cream As Color = Color.FromArgb(255, 248, 240)
    Private ReadOnly CreamDark As Color = Color.FromArgb(239, 229, 218)
    Private ReadOnly Accent As Color = Color.FromArgb(198, 145, 93)
    Private ReadOnly TextDark As Color = Color.FromArgb(55, 45, 40)

    '===========================================================
    ' FORM CONTROLS
    '===========================================================

    Private headerPanel As Panel
    Private contentPanel As Panel
    Private titleLabel As Label
    Private subtitleLabel As Label

    Private summaryPanel As Panel

    Private totalPropertiesLabel As Label
    Private occupiedPropertiesLabel As Label
    Private availablePropertiesLabel As Label
    Private totalTenantsLabel As Label
    Private totalPaymentsLabel As Label
    Private outstandingLabel As Label

    Private reportTypeLabel As Label
    Private reportTypeComboBox As ComboBox

    Private refreshButton As Button
    Private exportButton As Button
    Private closeButton As Button

    Private reportGrid As DataGridView

    Private statusLabel As Label

    '===========================================================
    ' CONSTRUCTOR
    '===========================================================

    Public Sub New()

        Me.Text = "Reports | Rental Property RAFAEL"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Size = New Size(1250, 750)
        Me.MinimumSize = New Size(1050, 650)
        Me.BackColor = Cream
        Me.Font = New Font("Segoe UI", 10.0F)

        BuildInterface()

        AddHandler Me.Resize, AddressOf ReportsForm_Resize

        LoadSummary()
        LoadReport()

    End Sub

    '===========================================================
    ' BUILD INTERFACE
    '===========================================================

    Private Sub BuildInterface()

        '-------------------------------------------------------
        ' HEADER
        '-------------------------------------------------------

        headerPanel = New Panel()

        headerPanel.Dock = DockStyle.Top
        headerPanel.Height = 100
        headerPanel.BackColor = CoffeeDark

        Me.Controls.Add(headerPanel)

        titleLabel = New Label()

        titleLabel.Text = "REPORTS & ANALYTICS"
        titleLabel.ForeColor = Cream
        titleLabel.Font = New Font("Segoe UI Semibold", 22.0F)
        titleLabel.AutoSize = True
        titleLabel.Location = New Point(35, 18)

        headerPanel.Controls.Add(titleLabel)

        subtitleLabel = New Label()

        subtitleLabel.Text = "Rental property overview • occupancy • tenants • payments"
        subtitleLabel.ForeColor = Color.FromArgb(220, 205, 195)
        subtitleLabel.Font = New Font("Segoe UI", 10.0F)
        subtitleLabel.AutoSize = True
        subtitleLabel.Location = New Point(38, 60)

        headerPanel.Controls.Add(subtitleLabel)

        '-------------------------------------------------------
        ' CONTENT PANEL
        '-------------------------------------------------------

        contentPanel = New Panel()

        contentPanel.Dock = DockStyle.Fill
        contentPanel.BackColor = Cream
        contentPanel.Padding = New Padding(25)

        Me.Controls.Add(contentPanel)

        '-------------------------------------------------------
        ' SUMMARY PANEL
        '-------------------------------------------------------

        summaryPanel = New Panel()

        summaryPanel.Dock = DockStyle.Top
        summaryPanel.Height = 145
        summaryPanel.BackColor = Cream

        contentPanel.Controls.Add(summaryPanel)

        CreateSummaryCard(
            summaryPanel,
            "TOTAL PROPERTIES",
            "0",
            CoffeeBrown,
            0,
            AddressOf SetTotalPropertiesLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "OCCUPIED",
            "0",
            CoffeeMedium,
            195,
            AddressOf SetOccupiedPropertiesLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "AVAILABLE",
            "0",
            Color.FromArgb(112, 93, 72),
            390,
            AddressOf SetAvailablePropertiesLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "TENANTS",
            "0",
            Color.FromArgb(136, 98, 67),
            585,
            AddressOf SetTotalTenantsLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "PAYMENTS",
            "₱0.00",
            Color.FromArgb(151, 111, 77),
            780,
            AddressOf SetTotalPaymentsLabel
        )

        CreateSummaryCard(
            summaryPanel,
            "OUTSTANDING",
            "₱0.00",
            Color.FromArgb(103, 74, 61),
            975,
            AddressOf SetOutstandingLabel
        )

        '-------------------------------------------------------
        ' REPORT TOOLBAR
        '-------------------------------------------------------

        Dim toolbarPanel As New Panel()

        toolbarPanel.Dock = DockStyle.Top
        toolbarPanel.Height = 65
        toolbarPanel.BackColor = Cream

        contentPanel.Controls.Add(toolbarPanel)

        reportTypeLabel = New Label()

        reportTypeLabel.Text = "REPORT:"
        reportTypeLabel.Font = New Font("Segoe UI Semibold", 10.0F)
        reportTypeLabel.ForeColor = TextDark
        reportTypeLabel.AutoSize = True
        reportTypeLabel.Location = New Point(5, 22)

        toolbarPanel.Controls.Add(reportTypeLabel)

        reportTypeComboBox = New ComboBox()

        reportTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        reportTypeComboBox.Font = New Font("Segoe UI", 10.0F)
        reportTypeComboBox.Width = 240
        reportTypeComboBox.Location = New Point(75, 17)

        reportTypeComboBox.Items.Add("Property Report")
        reportTypeComboBox.Items.Add("Tenant Report")
        reportTypeComboBox.Items.Add("Payment Report")
        reportTypeComboBox.Items.Add("Occupancy Report")

        reportTypeComboBox.SelectedIndex = 0

        AddHandler reportTypeComboBox.SelectedIndexChanged,
            AddressOf ReportTypeComboBox_SelectedIndexChanged

        toolbarPanel.Controls.Add(reportTypeComboBox)

        refreshButton = CreateButton(
            "↻  Refresh",
            New Point(335, 15),
            New Size(120, 38),
            CoffeeBrown
        )

        AddHandler refreshButton.Click,
            AddressOf RefreshButton_Click

        toolbarPanel.Controls.Add(refreshButton)

        exportButton = CreateButton(
            "▣  Export CSV",
            New Point(465, 15),
            New Size(135, 38),
            CoffeeMedium
        )

        AddHandler exportButton.Click,
            AddressOf ExportButton_Click

        toolbarPanel.Controls.Add(exportButton)

        closeButton = CreateButton(
            "Close",
            New Point(610, 15),
            New Size(100, 38),
            Color.FromArgb(145, 120, 105)
        )

        AddHandler closeButton.Click,
            AddressOf CloseButton_Click

        toolbarPanel.Controls.Add(closeButton)

        '-------------------------------------------------------
        ' DATA GRID
        '-------------------------------------------------------

        reportGrid = New DataGridView()

        reportGrid.Dock = DockStyle.Fill
        reportGrid.BackgroundColor = Color.White
        reportGrid.BorderStyle = BorderStyle.None
        reportGrid.AllowUserToAddRows = False
        reportGrid.AllowUserToDeleteRows = False
        reportGrid.ReadOnly = True
        reportGrid.MultiSelect = False
        reportGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        reportGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        reportGrid.RowHeadersVisible = False

        reportGrid.EnableHeadersVisualStyles = False

        reportGrid.ColumnHeadersDefaultCellStyle.BackColor = CoffeeBrown
        reportGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        reportGrid.ColumnHeadersDefaultCellStyle.Font =
            New Font("Segoe UI Semibold", 10.0F)

        reportGrid.ColumnHeadersHeight = 42

        reportGrid.DefaultCellStyle.BackColor = Color.White
        reportGrid.DefaultCellStyle.ForeColor = TextDark
        reportGrid.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(225, 210, 198)
        reportGrid.DefaultCellStyle.SelectionForeColor = TextDark

        reportGrid.AlternatingRowsDefaultCellStyle.BackColor =
            Color.FromArgb(249, 244, 239)

        reportGrid.RowTemplate.Height = 35

        contentPanel.Controls.Add(reportGrid)

        '-------------------------------------------------------
        ' STATUS
        '-------------------------------------------------------

        statusLabel = New Label()

        statusLabel.Dock = DockStyle.Bottom
        statusLabel.Height = 30
        statusLabel.Text = "Ready"
        statusLabel.TextAlign = ContentAlignment.MiddleLeft
        statusLabel.ForeColor = CoffeeMedium
        statusLabel.Font = New Font("Segoe UI", 9.0F)

        contentPanel.Controls.Add(statusLabel)

    End Sub

    '===========================================================
    ' SUMMARY CARD
    '===========================================================

    Private Sub CreateSummaryCard(
        parent As Control,
        title As String,
        value As String,
        cardColor As Color,
        x As Integer,
        valueSetter As Action(Of Label)
    )

        Dim card As New Panel()

        card.Size = New Size(175, 115)
        card.Location = New Point(x, 10)
        card.BackColor = cardColor

        parent.Controls.Add(card)

        Dim titleLabel As New Label()

        titleLabel.Text = title
        titleLabel.ForeColor = Color.FromArgb(235, 225, 218)
        titleLabel.Font = New Font("Segoe UI Semibold", 8.5F)
        titleLabel.AutoSize = False
        titleLabel.TextAlign = ContentAlignment.MiddleLeft
        titleLabel.Location = New Point(15, 12)
        titleLabel.Size = New Size(145, 25)

        card.Controls.Add(titleLabel)

        Dim valueLabel As New Label()

        valueLabel.Text = value
        valueLabel.ForeColor = Color.White
        valueLabel.Font = New Font("Segoe UI Semibold", 18.0F)
        valueLabel.AutoSize = False
        valueLabel.TextAlign = ContentAlignment.MiddleLeft
        valueLabel.Location = New Point(15, 42)
        valueLabel.Size = New Size(145, 50)

        card.Controls.Add(valueLabel)

        valueSetter(valueLabel)

    End Sub

    Private Sub SetTotalPropertiesLabel(label As Label)
        totalPropertiesLabel = label
    End Sub

    Private Sub SetOccupiedPropertiesLabel(label As Label)
        occupiedPropertiesLabel = label
    End Sub

    Private Sub SetAvailablePropertiesLabel(label As Label)
        availablePropertiesLabel = label
    End Sub

    Private Sub SetTotalTenantsLabel(label As Label)
        totalTenantsLabel = label
    End Sub

    Private Sub SetTotalPaymentsLabel(label As Label)
        totalPaymentsLabel = label
    End Sub

    Private Sub SetOutstandingLabel(label As Label)
        outstandingLabel = label
    End Sub

    '===========================================================
    ' BUTTON CREATION
    '===========================================================

    Private Function CreateButton(
        buttonText As String,
        location As Point,
        buttonSize As Size,
        buttonColor As Color
    ) As Button

        Dim btn As New Button()

        btn.Text = buttonText
        btn.Location = location
        btn.Size = buttonSize
        btn.BackColor = buttonColor
        btn.ForeColor = Color.White
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.Font = New Font("Segoe UI Semibold", 9.0F)
        btn.Cursor = Cursors.Hand

        Return btn

    End Function

    '===========================================================
    ' LOAD SUMMARY
    '===========================================================

    Private Sub LoadSummary()

        Try

            Using connection = DatabaseHelper.GetConnection()

                connection.Open()

                '------------------------------------------------
                ' TOTAL PROPERTIES
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT COUNT(*) FROM Properties WHERE IsActive = 1",
                    connection)

                    totalPropertiesLabel.Text =
                        Convert.ToInt32(command.ExecuteScalar()).ToString()

                End Using

                '------------------------------------------------
                ' OCCUPIED PROPERTIES
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT COUNT(*) FROM Properties " &
                    "WHERE IsActive = 1 AND PropertyStatus = 'Occupied'",
                    connection)

                    occupiedPropertiesLabel.Text =
                        Convert.ToInt32(command.ExecuteScalar()).ToString()

                End Using

                '------------------------------------------------
                ' AVAILABLE PROPERTIES
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT COUNT(*) FROM Properties " &
                    "WHERE IsActive = 1 AND PropertyStatus = 'Available'",
                    connection)

                    availablePropertiesLabel.Text =
                        Convert.ToInt32(command.ExecuteScalar()).ToString()

                End Using

                '------------------------------------------------
                ' TOTAL TENANTS
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT COUNT(*) FROM Tenants WHERE IsActive = 1",
                    connection)

                    totalTenantsLabel.Text =
                        Convert.ToInt32(command.ExecuteScalar()).ToString()

                End Using

                '------------------------------------------------
                ' TOTAL AMOUNT PAID
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT ISNULL(SUM(AmountPaid), 0) " &
                    "FROM Payments " &
                    "WHERE IsActive = 1",
                    connection)

                    Dim totalPaid As Decimal =
                        Convert.ToDecimal(command.ExecuteScalar())

                    totalPaymentsLabel.Text =
                        "₱" & totalPaid.ToString("N2")

                End Using

                '------------------------------------------------
                ' OUTSTANDING BALANCE
                '------------------------------------------------

                Using command As New SqlCommand(
                    "SELECT ISNULL(SUM(AmountDue - AmountPaid), 0) " &
                    "FROM Payments " &
                    "WHERE IsActive = 1",
                    connection)

                    Dim outstanding As Decimal =
                        Convert.ToDecimal(command.ExecuteScalar())

                    outstandingLabel.Text =
                        "₱" & outstanding.ToString("N2")

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Unable to load report summary." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Reports Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    '===========================================================
    ' LOAD REPORT
    '===========================================================

    Private Sub LoadReport()

        If reportTypeComboBox Is Nothing Then
            Return
        End If

        Select Case reportTypeComboBox.SelectedIndex

            Case 0
                LoadPropertyReport()

            Case 1
                LoadTenantReport()

            Case 2
                LoadPaymentReport()

            Case 3
                LoadOccupancyReport()

        End Select

    End Sub

    '===========================================================
    ' PROPERTY REPORT
    '===========================================================

    Private Sub LoadPropertyReport()

        Try

            Dim query As String =
                "SELECT " &
                "PropertyID AS [ID], " &
                "PropertyName AS [Property Name], " &
                "PropertyAddress AS [Address], " &
                "PropertyType AS [Type], " &
                "MonthlyRent AS [Monthly Rent], " &
                "SecurityDeposit AS [Security Deposit], " &
                "PropertyStatus AS [Status], " &
                "PaymentStatus AS [Payment] " &
                "FROM Properties " &
                "WHERE IsActive = 1 " &
                "ORDER BY PropertyID"

            LoadGrid(query)

            statusLabel.Text =
                "Showing all active rental properties."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' TENANT REPORT
    '===========================================================

    Private Sub LoadTenantReport()

        Try

            Dim query As String =
                "SELECT " &
                "t.TenantID AS [Tenant ID], " &
                "t.TenantName AS [Tenant Name], " &
                "t.ContactNumber AS [Contact Number], " &
                "t.Email AS [Email], " &
                "p.PropertyName AS [Property], " &
                "p.PropertyType AS [Type], " &
                "t.LeaseStartDate AS [Lease Start], " &
                "t.LeaseEndDate AS [Lease End], " &
                "t.PaymentStatus AS [Payment Status] " &
                "FROM Tenants t " &
                "LEFT JOIN Properties p " &
                "ON t.PropertyID = p.PropertyID " &
                "WHERE t.IsActive = 1 " &
                "ORDER BY t.TenantID"

            LoadGrid(query)

            statusLabel.Text =
                "Showing all active tenants and their rental information."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' PAYMENT REPORT
    '===========================================================

    Private Sub LoadPaymentReport()

        Try

            Dim query As String =
                "SELECT " &
                "pay.PaymentID AS [Payment ID], " &
                "t.TenantName AS [Tenant Name], " &
                "p.PropertyName AS [Property], " &
                "pay.PaymentDate AS [Payment Date], " &
                "pay.AmountDue AS [Amount Due], " &
                "pay.AmountPaid AS [Amount Paid], " &
                "(pay.AmountDue - pay.AmountPaid) AS [Balance], " &
                "pay.PaymentMethod AS [Method], " &
                "pay.PaymentStatus AS [Status], " &
                "pay.Notes AS [Notes] " &
                "FROM Payments pay " &
                "INNER JOIN Tenants t " &
                "ON pay.TenantID = t.TenantID " &
                "LEFT JOIN Properties p " &
                "ON t.PropertyID = p.PropertyID " &
                "WHERE pay.IsActive = 1 " &
                "ORDER BY pay.PaymentDate DESC"

            LoadGrid(query)

            statusLabel.Text =
                "Showing rental payment records and outstanding balances."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' OCCUPANCY REPORT
    '===========================================================

    Private Sub LoadOccupancyReport()

        Try

            Dim query As String =
                "SELECT " &
                "PropertyStatus AS [Property Status], " &
                "COUNT(*) AS [Number of Properties] " &
                "FROM Properties " &
                "WHERE IsActive = 1 " &
                "GROUP BY PropertyStatus " &
                "ORDER BY PropertyStatus"

            LoadGrid(query)

            statusLabel.Text =
                "Showing property occupancy by current status."

        Catch ex As Exception

            ShowDatabaseError(ex)

        End Try

    End Sub

    '===========================================================
    ' LOAD GRID
    '===========================================================

    Private Sub LoadGrid(query As String)

        Dim table As New DataTable()

        Using connection As New SqlConnection(ConnectionString)

            Using command As New SqlCommand(query, connection)

                Using adapter As New SqlDataAdapter(command)

                    adapter.Fill(table)

                End Using

            End Using

        End Using

        reportGrid.DataSource = table

        FormatGrid()

    End Sub

    '===========================================================
    ' GRID FORMATTING
    '===========================================================

    Private Sub FormatGrid()

        If reportGrid.Columns.Count = 0 Then
            Return
        End If

        For Each column As DataGridViewColumn In reportGrid.Columns

            column.SortMode =
                DataGridViewColumnSortMode.Automatic

        Next

        ' Currency formatting

        If reportGrid.Columns.Contains("Monthly Rent") Then
            reportGrid.Columns("Monthly Rent").DefaultCellStyle.Format =
                "₱#,##0.00"
        End If

        If reportGrid.Columns.Contains("Security Deposit") Then
            reportGrid.Columns("Security Deposit").DefaultCellStyle.Format =
                "₱#,##0.00"
        End If

        If reportGrid.Columns.Contains("Amount Due") Then
            reportGrid.Columns("Amount Due").DefaultCellStyle.Format =
                "₱#,##0.00"
        End If

        If reportGrid.Columns.Contains("Amount Paid") Then
            reportGrid.Columns("Amount Paid").DefaultCellStyle.Format =
                "₱#,##0.00"
        End If

        If reportGrid.Columns.Contains("Balance") Then
            reportGrid.Columns("Balance").DefaultCellStyle.Format =
                "₱#,##0.00"
        End If

        ' Date formatting

        If reportGrid.Columns.Contains("Payment Date") Then
            reportGrid.Columns("Payment Date").DefaultCellStyle.Format =
                "MMM dd, yyyy"
        End If

        If reportGrid.Columns.Contains("Lease Start") Then
            reportGrid.Columns("Lease Start").DefaultCellStyle.Format =
                "MMM dd, yyyy"
        End If

        If reportGrid.Columns.Contains("Lease End") Then
            reportGrid.Columns("Lease End").DefaultCellStyle.Format =
                "MMM dd, yyyy"
        End If

    End Sub

    '===========================================================
    ' REFRESH
    '===========================================================

    Private Sub RefreshButton_Click(
        sender As Object,
        e As EventArgs
    )

        LoadSummary()
        LoadReport()

        statusLabel.Text =
            "Reports refreshed successfully."

    End Sub

    '===========================================================
    ' REPORT TYPE CHANGE
    '===========================================================

    Private Sub ReportTypeComboBox_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    )

        LoadReport()

    End Sub

    '===========================================================
    ' EXPORT CSV
    '===========================================================

    Private Sub ExportButton_Click(
        sender As Object,
        e As EventArgs
    )

        If reportGrid.Rows.Count = 0 Then

            MessageBox.Show(
                "There are no records to export.",
                "Export",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            Return

        End If

        Try

            Dim reportName As String =
                reportTypeComboBox.Text.Replace(" ", "_")

            Dim saveDialog As New SaveFileDialog()

            saveDialog.Filter =
                "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"

            saveDialog.FileName =
                reportName & "_" &
                DateTime.Now.ToString("yyyyMMdd_HHmmss") &
                ".csv"

            If saveDialog.ShowDialog() <> DialogResult.OK Then
                Return
            End If

            Dim csv As New StringBuilder()

            '---------------------------------------------------
            ' HEADERS
            '---------------------------------------------------

            For i As Integer = 0 To reportGrid.Columns.Count - 1

                If i > 0 Then
                    csv.Append(",")
                End If

                csv.Append(
                    EscapeCsv(
                        reportGrid.Columns(i).HeaderText
                    )
                )

            Next

            csv.AppendLine()

            '---------------------------------------------------
            ' ROWS
            '---------------------------------------------------

            For Each row As DataGridViewRow In reportGrid.Rows

                If row.IsNewRow Then
                    Continue For
                End If

                For i As Integer = 0 To reportGrid.Columns.Count - 1

                    If i > 0 Then
                        csv.Append(",")
                    End If

                    Dim value As Object =
                        row.Cells(i).Value

                    Dim text As String = ""

                    If value IsNot Nothing AndAlso
                       Not IsDBNull(value) Then

                        text = value.ToString()

                    End If

                    csv.Append(EscapeCsv(text))

                Next

                csv.AppendLine()

            Next

            File.WriteAllText(
                saveDialog.FileName,
                csv.ToString(),
                Encoding.UTF8
            )

            MessageBox.Show(
                "Report exported successfully.",
                "Export Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

        Catch ex As Exception

            MessageBox.Show(
                "Unable to export the report." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Export Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    '===========================================================
    ' CSV ESCAPING
    '===========================================================

    Private Function EscapeCsv(value As String) As String

        If value Is Nothing Then
            Return """"""
        End If

        value = value.Replace("""", """""")

        Return """" & value & """"

    End Function

    '===========================================================
    ' CLOSE
    '===========================================================

    Private Sub CloseButton_Click(
        sender As Object,
        e As EventArgs
    )

        Me.Close()

    End Sub

    '===========================================================
    ' DATABASE ERROR
    '===========================================================

    Private Sub ShowDatabaseError(ex As Exception)

        MessageBox.Show(
            "Unable to load the report." &
            Environment.NewLine &
            Environment.NewLine &
            ex.Message,
            "Database Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

    End Sub

    '===========================================================
    ' FORM RESIZE
    '===========================================================

    Private Sub ReportsForm_Resize(
        sender As Object,
        e As EventArgs
    )

        If summaryPanel Is Nothing Then
            Return
        End If

        Dim availableWidth As Integer =
            summaryPanel.ClientSize.Width

        Dim cardWidth As Integer = 175
        Dim spacing As Integer = 10

        Dim totalWidth As Integer =
            (cardWidth * 6) +
            (spacing * 5)

        Dim startX As Integer =
            Math.Max(10, (availableWidth - totalWidth) \ 2)

        Dim x As Integer = startX

        For Each control As Control In summaryPanel.Controls

            If TypeOf control Is Panel Then

                control.Left = x

                x += cardWidth + spacing

            End If

        Next

    End Sub

End Class