Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class MainForm
    Inherits Form

    Private pnlNavigation As Panel
    Private pnlContent As Panel

    Private lblPageTitle As Label
    Private lblUser As Label

    Private btnDashboard As Button
    Private btnProperties As Button
    Private btnTenants As Button
    Private btnPayments As Button
    Private btnReports As Button
    Private btnUsers As Button
    Private btnSettings As Button
    Private btnLogout As Button

    '==========================================================
    ' COLORS - COZY COFFEE CAFE
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
    ' FORM CONSTRUCTOR
    '==========================================================

    Public Sub New()

        BuildMainForm()

        ShowDashboard()

    End Sub


    '==========================================================
    ' BUILD MAIN FORM
    '==========================================================

    Private Sub BuildMainForm()

        Me.Text = "Casa Brew - Rental Property Management"

        Me.StartPosition =
        FormStartPosition.CenterScreen

        Me.Size =
        New Size(1250, 760)

        Me.MinimumSize =
        New Size(1100, 680)

        Me.BackColor =
        Cream

        Me.Font =
        New Font("Segoe UI", 10.0F)


        '======================================================
        ' LEFT NAVIGATION
        '======================================================

        pnlNavigation = New Panel()

        pnlNavigation.Dock =
        DockStyle.Left

        'Keep the sidebar wide enough for its existing buttons
        pnlNavigation.Width =
        235

        pnlNavigation.BackColor =
        CoffeeDark

        '======================================================
        ' MAIN CONTENT
        '======================================================

        pnlContent = New Panel()

        pnlContent.Dock =
        DockStyle.Fill

        pnlContent.BackColor =
        Cream

        'Space between sidebar and page controls
        pnlContent.Padding =
        New Padding(
            28,
            22,
            28,
            28
        )


        '======================================================
        ' ADD MAIN PANELS
        '======================================================

        'Add content first.
        'The navigation panel is added afterward so it remains
        'a separate left-side area instead of covering content.

        Me.Controls.Add(pnlContent)

        Me.Controls.Add(pnlNavigation)

        'Do not call BringToFront here. With DockStyle.Left and
        'DockStyle.Fill, changing z-order can make the sidebar
        'paint over the fill panel instead of reserving its width.
        'WinForms docking will size pnlContent to the remaining area.


        '======================================================
        ' LOGO
        '======================================================

        Dim lblLogo As New Label()

        lblLogo.Text =
        "CASA BREW"

        lblLogo.Location =
        New Point(22, 22)

        lblLogo.Size =
        New Size(190, 40)

        lblLogo.Font =
        New Font(
            "Georgia",
            18,
            FontStyle.Bold
        )

        lblLogo.ForeColor =
        Color.White

        pnlNavigation.Controls.Add(lblLogo)


        '======================================================
        ' SUBTITLE
        '======================================================

        Dim lblSubtitle As New Label()

        lblSubtitle.Text =
        "Rental Property Management"

        lblSubtitle.Location =
        New Point(24, 62)

        lblSubtitle.Size =
        New Size(190, 40)

        lblSubtitle.Font =
        New Font(
            "Segoe UI",
            8.5F
        )

        lblSubtitle.ForeColor =
        Color.FromArgb(
            220,
            205,
            190
        )

        pnlNavigation.Controls.Add(lblSubtitle)


        '======================================================
        ' NAVIGATION BUTTONS
        '======================================================

        btnDashboard =
        CreateNavigationButton(
            "Dashboard"
        )

        btnProperties =
        CreateNavigationButton(
            "Properties"
        )

        btnTenants =
        CreateNavigationButton(
            "Tenants"
        )

        btnPayments =
        CreateNavigationButton(
            "Payments"
        )

        btnReports =
        CreateNavigationButton(
            "Reports"
        )

        btnUsers =
        CreateNavigationButton(
            "User Management"
        )

        btnSettings =
        CreateNavigationButton(
            "Settings"
        )

        btnLogout =
        CreateNavigationButton(
            "Logout"
        )


        '======================================================
        ' BUTTON LOCATIONS
        '======================================================

        btnDashboard.Location =
        New Point(12, 125)

        btnProperties.Location =
        New Point(12, 177)

        btnTenants.Location =
        New Point(12, 229)

        btnPayments.Location =
        New Point(12, 281)

        btnReports.Location =
        New Point(12, 333)

        btnUsers.Location =
        New Point(12, 385)

        btnSettings.Location =
        New Point(12, 437)

        'Anchor logout to the bottom of the sidebar
        btnLogout.Anchor =
        AnchorStyles.Left Or
        AnchorStyles.Bottom

        btnLogout.Location =
        New Point(
            12,
            pnlNavigation.ClientSize.Height -
            btnLogout.Height -
            20
        )


        '======================================================
        ' ADD BUTTONS
        '======================================================

        pnlNavigation.Controls.Add(btnDashboard)

        pnlNavigation.Controls.Add(btnProperties)

        pnlNavigation.Controls.Add(btnTenants)

        pnlNavigation.Controls.Add(btnPayments)

        pnlNavigation.Controls.Add(btnReports)

        pnlNavigation.Controls.Add(btnUsers)

        pnlNavigation.Controls.Add(btnSettings)

        pnlNavigation.Controls.Add(btnLogout)


        '======================================================
        ' PAGE TITLE
        '======================================================

        lblPageTitle = New Label()

        lblPageTitle.Location =
        New Point(28, 22)

        lblPageTitle.Size =
        New Size(700, 45)

        lblPageTitle.Font =
        New Font(
            "Georgia",
            22,
            FontStyle.Bold
        )

        lblPageTitle.ForeColor =
        TextDark

        pnlContent.Controls.Add(lblPageTitle)


        '======================================================
        ' CURRENT USER
        '======================================================

        lblUser = New Label()

        lblUser.Location =
        New Point(700, 27)

        lblUser.Size =
        New Size(450, 35)

        lblUser.TextAlign =
        ContentAlignment.MiddleRight

        lblUser.Font =
        New Font(
            "Segoe UI",
            9.5F
        )

        lblUser.ForeColor =
        Coffee

        pnlContent.Controls.Add(lblUser)


        '======================================================
        ' EVENTS
        '======================================================

        AddHandler btnDashboard.Click,
        AddressOf DashboardClicked

        AddHandler btnProperties.Click,
        AddressOf PropertiesClicked

        AddHandler btnTenants.Click,
        AddressOf TenantsClicked

        AddHandler btnPayments.Click,
        AddressOf PaymentsClicked

        AddHandler btnReports.Click,
        AddressOf ReportsClicked

        AddHandler btnUsers.Click,
        AddressOf UsersClicked

        AddHandler btnSettings.Click,
        AddressOf SettingsClicked

        AddHandler btnLogout.Click,
        AddressOf LogoutClicked


        '======================================================
        ' ROLE PERMISSIONS
        '======================================================

        ApplyRolePermissions()

    End Sub


    '==========================================================
    ' CONTENT PANEL RESIZE
    '==========================================================

    Private Sub ContentPanelResized(
        sender As Object,
        e As EventArgs
    )

        If lblUser Is Nothing Then
            Return
        End If

        lblUser.Location =
            New Point(
                Math.Max(
                    500,
                    pnlContent.ClientSize.Width - 430
                ),
                27
            )

    End Sub


    '==========================================================
    ' NAVIGATION BUTTON CREATOR
    '==========================================================

    Private Function CreateNavigationButton(
        buttonText As String
    ) As Button

        Dim button As New Button()

        button.Text =
            buttonText

        '------------------------------------------------------
        ' REDUCED BUTTON WIDTH
        '------------------------------------------------------

        button.Size =
            New Size(180, 45)

        button.FlatStyle =
            FlatStyle.Flat

        button.BackColor =
            CoffeeDark

        button.ForeColor =
            Color.FromArgb(
                244,
                232,
                218
            )

        button.TextAlign =
            ContentAlignment.MiddleLeft

        button.Padding =
            New Padding(12, 0, 0, 0)

        button.Font =
            New Font(
                "Segoe UI",
                9.0F,
                FontStyle.Bold
            )

        button.Cursor =
            Cursors.Hand

        button.UseVisualStyleBackColor =
            False

        button.FlatAppearance.BorderSize =
            0

        button.Anchor =
            AnchorStyles.Left

        Return button

    End Function


    '==========================================================
    ' ROLE PERMISSIONS
    '==========================================================

    Private Sub ApplyRolePermissions()

        If String.IsNullOrEmpty(
            LoginForm.LoggedInRole
        ) Then

            Return

        End If


        Dim role As String =
            LoginForm.LoggedInRole


        'Admin only

        If role = "Admin" Then

            btnUsers.Visible = True

        Else

            btnUsers.Visible = False

        End If


        'Reports

        If role = "Admin" OrElse
           role = "Property Manager" OrElse
           role = "Manager" Then

            btnReports.Visible = True

        Else

            btnReports.Visible = False

        End If


        'Payments

        If role = "Staff" Then

            btnPayments.Visible = False

        Else

            btnPayments.Visible = True

        End If

    End Sub


    '==========================================================
    ' DASHBOARD BUTTON
    '==========================================================

    Private Sub DashboardClicked(
        sender As Object,
        e As EventArgs
    )

        ShowDashboard()

    End Sub


    '==========================================================
    ' PROPERTY BUTTON
    '==========================================================

    Private Sub PropertiesClicked(
        sender As Object,
        e As EventArgs
    )

        Dim form As New PropertiesForm()

        OpenPage(
            form,
            "Properties"
        )

    End Sub


    '==========================================================
    ' TENANTS BUTTON
    '==========================================================

    Private Sub TenantsClicked(
        sender As Object,
        e As EventArgs
    )

        Dim form As New TenantsForm()

        OpenPage(
            form,
            "Tenants"
        )

    End Sub


    '==========================================================
    ' PAYMENTS BUTTON
    '==========================================================

    Private Sub PaymentsClicked(
        sender As Object,
        e As EventArgs
    )

        Dim form As New PaymentsForm()

        OpenPage(
            form,
            "Payments"
        )

    End Sub


    '==========================================================
    ' REPORTS BUTTON
    '==========================================================

    Private Sub ReportsClicked(
        sender As Object,
        e As EventArgs
    )

        Dim form As New ReportsForm()

        OpenPage(
            form,
            "Reports"
        )

    End Sub


    '==========================================================
    ' USER MANAGEMENT BUTTON
    '==========================================================

    Private Sub UsersClicked(
        sender As Object,
        e As EventArgs
    )

        Dim form As New UserManagementForm()

        OpenPage(
            form,
            "User Management"
        )

    End Sub


    '==========================================================
    ' SETTINGS BUTTON
    '==========================================================

    Private Sub SettingsClicked(
        sender As Object,
        e As EventArgs
    )

        Dim form As New SettingsForm()

        OpenPage(
            form,
            "Settings"
        )

    End Sub


    '==========================================================
    ' OPEN PAGE
    '==========================================================

    Private Sub OpenPage(
        page As Form,
        pageTitle As String
    )

        pnlContent.Controls.Clear()

        pnlContent.Controls.Add(
            lblPageTitle
        )

        pnlContent.Controls.Add(
            lblUser
        )


        lblPageTitle.Text =
            pageTitle


        lblUser.Text =
            LoginForm.LoggedInFullName &
            "  •  " &
            LoginForm.LoggedInRole


        page.TopLevel = False

        page.FormBorderStyle =
            FormBorderStyle.None

        page.Dock =
            DockStyle.Fill

        page.BackColor =
            Cream


        pnlContent.Controls.Add(page)

        page.Show()

        page.BringToFront()

    End Sub


    '==========================================================
    ' DASHBOARD
    '==========================================================

    Private Sub ShowDashboard()

        pnlContent.Controls.Clear()

        pnlContent.Controls.Add(
            lblPageTitle
        )

        pnlContent.Controls.Add(
            lblUser
        )


        lblPageTitle.Text =
            "Welcome to Casa Brew"


        lblUser.Text =
            LoginForm.LoggedInFullName &
            "  •  " &
            LoginForm.LoggedInRole


        '------------------------------------------------------
        ' INTRO
        '------------------------------------------------------

        Dim lblIntro As New Label()

        lblIntro.Text =
            "Your cozy workspace for managing rental properties."

        lblIntro.Location =
            New Point(30, 75)

        lblIntro.AutoSize = True

        lblIntro.Font =
            New Font(
                "Segoe UI",
                10.5F
            )

        lblIntro.ForeColor =
            Coffee

        pnlContent.Controls.Add(lblIntro)


        '------------------------------------------------------
        ' TOTAL PROPERTY CARD
        '------------------------------------------------------

        Dim cardTotal As Panel =
            CreateStatisticCard(
                "TOTAL PROPERTIES",
                GetPropertyCount(),
                Coffee,
                New Point(30, 120)
            )

        pnlContent.Controls.Add(cardTotal)


        '------------------------------------------------------
        ' OCCUPIED CARD
        '------------------------------------------------------

        Dim cardOccupied As Panel =
            CreateStatisticCard(
                "OCCUPIED",
                GetPropertyStatusCount(
                    "Occupied"
                ),
                Sage,
                New Point(225, 120)
            )

        pnlContent.Controls.Add(cardOccupied)


        '------------------------------------------------------
        ' AVAILABLE CARD
        '------------------------------------------------------

        Dim cardAvailable As Panel =
            CreateStatisticCard(
                "AVAILABLE",
                GetPropertyStatusCount(
                    "Available"
                ),
                Caramel,
                New Point(420, 120)
            )

        pnlContent.Controls.Add(cardAvailable)


        '------------------------------------------------------
        ' RESERVED CARD
        '------------------------------------------------------

        Dim cardReserved As Panel =
            CreateStatisticCard(
                "RESERVED",
                GetPropertyStatusCount(
                    "Reserved"
                ),
                Latte,
                New Point(615, 120)
            )

        pnlContent.Controls.Add(cardReserved)


        '------------------------------------------------------
        ' WELCOME MESSAGE
        '------------------------------------------------------

        Dim welcomePanel As New Panel()

        welcomePanel.Location =
            New Point(30, 255)

        welcomePanel.Size =
            New Size(850, 250)

        welcomePanel.BackColor =
            CardColor

        pnlContent.Controls.Add(
            welcomePanel
        )


        Dim lblWelcomeTitle As New Label()

        lblWelcomeTitle.Text =
            "Today's Workspace"

        lblWelcomeTitle.Location =
            New Point(25, 25)

        lblWelcomeTitle.AutoSize = True

        lblWelcomeTitle.Font =
            New Font(
                "Georgia",
                18,
                FontStyle.Bold
            )

        lblWelcomeTitle.ForeColor =
            TextDark

        welcomePanel.Controls.Add(
            lblWelcomeTitle
        )


        Dim lblWelcomeText As New Label()

        lblWelcomeText.Text =
            "Use the navigation panel to manage your rental properties, tenants, payments, reports, and system users." &
            Environment.NewLine &
            Environment.NewLine &
            "Take a breath. Grab a coffee. Let's get things organized."

        lblWelcomeText.Location =
            New Point(27, 70)

        lblWelcomeText.Size =
            New Size(780, 120)

        lblWelcomeText.Font =
            New Font(
                "Segoe UI",
                11
            )

        lblWelcomeText.ForeColor =
            TextDark

        welcomePanel.Controls.Add(
            lblWelcomeText
        )


        '------------------------------------------------------
        ' QUICK PROPERTY BUTTON
        '------------------------------------------------------

        Dim btnQuickProperty As Button =
            CreateActionButton(
                "Add Property",
                New Point(900, 280),
                Coffee
            )

        AddHandler btnQuickProperty.Click,
            AddressOf PropertiesClicked

        pnlContent.Controls.Add(
            btnQuickProperty
        )


        '------------------------------------------------------
        ' QUICK TENANT BUTTON
        '------------------------------------------------------

        Dim btnQuickTenant As Button =
            CreateActionButton(
                "Manage Tenants",
                New Point(900, 335),
                Caramel
            )

        AddHandler btnQuickTenant.Click,
            AddressOf TenantsClicked

        pnlContent.Controls.Add(
            btnQuickTenant
        )


        '------------------------------------------------------
        ' QUICK PAYMENT BUTTON
        '------------------------------------------------------

        Dim btnQuickPayment As Button =
            CreateActionButton(
                "View Payments",
                New Point(900, 390),
                Sage
            )

        AddHandler btnQuickPayment.Click,
            AddressOf PaymentsClicked

        pnlContent.Controls.Add(
            btnQuickPayment
        )

    End Sub


    '==========================================================
    ' GET TOTAL PROPERTY COUNT
    '==========================================================

    Private Function GetPropertyCount() As String

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT COUNT(*) " &
                    "FROM Properties " &
                    "WHERE IsActive = 1"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    Return command.ExecuteScalar().ToString()

                End Using

            End Using

        Catch

            Return "0"

        End Try

    End Function


    '==========================================================
    ' GET PROPERTY STATUS COUNT
    '==========================================================

    Private Function GetPropertyStatusCount(
        status As String
    ) As String

        Try

            Using connection =
                DatabaseHelper.GetConnection()

                connection.Open()


                Dim sql As String =
                    "SELECT COUNT(*) " &
                    "FROM Properties " &
                    "WHERE PropertyStatus = @Status " &
                    "AND IsActive = 1"


                Using command As New SqlCommand(
                    sql,
                    connection
                )

                    command.Parameters.AddWithValue(
                        "@Status",
                        status
                    )


                    Return command.ExecuteScalar().ToString()

                End Using

            End Using

        Catch

            Return "0"

        End Try

    End Function


    '==========================================================
    ' STATISTIC CARD
    '==========================================================

    Private Function CreateStatisticCard(
        title As String,
        value As String,
        accent As Color,
        location As Point
    ) As Panel

        Dim panel As New Panel()

        panel.Location =
            location

        panel.Size =
            New Size(175, 105)

        panel.BackColor =
            CardColor


        Dim accentPanel As New Panel()

        accentPanel.Location =
            New Point(0, 0)

        accentPanel.Size =
            New Size(175, 7)

        accentPanel.BackColor =
            accent

        panel.Controls.Add(
            accentPanel
        )


        Dim lblTitle As New Label()

        lblTitle.Text =
            title

        lblTitle.Location =
            New Point(16, 22)

        lblTitle.AutoSize = True

        lblTitle.Font =
            New Font(
                "Segoe UI",
                8.5F,
                FontStyle.Bold
            )

        lblTitle.ForeColor =
            Color.Gray

        panel.Controls.Add(
            lblTitle
        )


        Dim lblValue As New Label()

        lblValue.Text =
            value

        lblValue.Location =
            New Point(16, 47)

        lblValue.AutoSize = True

        lblValue.Font =
            New Font(
                "Georgia",
                23,
                FontStyle.Bold
            )

        lblValue.ForeColor =
            TextDark

        panel.Controls.Add(
            lblValue
        )


        Return panel

    End Function


    '==========================================================
    ' ACTION BUTTON
    '==========================================================

    Private Function CreateActionButton(
        text As String,
        location As Point,
        buttonColor As Color
    ) As Button

        Dim button As New Button()

        button.Text =
            text

        button.Location =
            location

        button.Size =
            New Size(200, 45)

        button.BackColor =
            buttonColor

        button.ForeColor =
            Color.White

        button.FlatStyle =
            FlatStyle.Flat

        button.Font =
            New Font(
                "Segoe UI",
                9,
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
    ' LOGOUT
    '==========================================================

    Private Sub LogoutClicked(
        sender As Object,
        e As EventArgs
    )

        Dim answer As DialogResult =
            MessageBox.Show(
                "Do you want to log out?",
                "Casa Brew",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )


        If answer =
            DialogResult.No Then

            Return

        End If


        LoginForm.LoggedInUserID = 0

        LoginForm.LoggedInUsername = ""

        LoginForm.LoggedInFullName = ""

        LoginForm.LoggedInRole = ""


        Me.Hide()


        Using login As New LoginForm()

            If login.ShowDialog() =
                DialogResult.OK Then

                ApplyRolePermissions()

                Me.Show()

                ShowDashboard()

            Else

                Application.Exit()

            End If

        End Using

    End Sub

End Class