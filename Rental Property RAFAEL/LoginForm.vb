Imports System.Data
Imports System.Drawing
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public Class LoginForm
    Inherits Form

    ' ============================================================
    ' CONTROLS
    ' ============================================================

    Private lblTitle As Label
    Private lblSubtitle As Label

    Private lblUsername As Label
    Private lblPassword As Label

    Private txtUsername As TextBox
    Private txtPassword As TextBox

    Private btnLogin As Button
    Private btnExit As Button

    Private chkShowPassword As CheckBox

    ' ============================================================
    ' LOGIN INFORMATION
    ' ============================================================

    Public Shared LoggedInUserID As Integer = 0
    Public Shared LoggedInUsername As String = ""
    Public Shared LoggedInFullName As String = ""
    Public Shared LoggedInRole As String = ""

    ' ============================================================
    ' CONSTRUCTOR
    ' ============================================================

    Public Sub New()

        InitializeForm()

    End Sub

    ' ============================================================
    ' INITIALIZE FORM
    ' ============================================================

    Private Sub InitializeForm()

        ' --------------------------------------------------------
        ' FORM SETTINGS
        ' --------------------------------------------------------

        Me.Text = "Rental Property Management System - Login"

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.Size =
            New Size(500, 500)

        Me.FormBorderStyle =
            FormBorderStyle.FixedDialog

        Me.MaximizeBox = False
        Me.MinimizeBox = False

        Me.BackColor =
            Color.White

        Me.Font =
            New Font(
                "Segoe UI",
                10,
                FontStyle.Regular
            )

        ' --------------------------------------------------------
        ' TITLE
        ' --------------------------------------------------------

        lblTitle = New Label With {
            .Text = "RENTAL PROPERTY",
            .Location = New Point(0, 45),
            .Size = New Size(500, 40),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Font =
                New Font(
                    "Segoe UI",
                    22,
                    FontStyle.Bold
                ),
            .ForeColor =
                Color.FromArgb(35, 55, 75)
        }

        Me.Controls.Add(lblTitle)

        ' --------------------------------------------------------
        ' SUBTITLE
        ' --------------------------------------------------------

        lblSubtitle = New Label With {
            .Text = "Management System",
            .Location = New Point(0, 85),
            .Size = New Size(500, 30),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Font =
                New Font(
                    "Segoe UI",
                    12,
                    FontStyle.Regular
                ),
            .ForeColor =
                Color.Gray
        }

        Me.Controls.Add(lblSubtitle)

        ' --------------------------------------------------------
        ' USERNAME LABEL
        ' --------------------------------------------------------

        lblUsername = New Label With {
            .Text = "Username:",
            .Location = New Point(80, 150),
            .AutoSize = True,
            .Font =
                New Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                )
        }

        Me.Controls.Add(lblUsername)

        ' --------------------------------------------------------
        ' USERNAME TEXTBOX
        ' --------------------------------------------------------

        txtUsername = New TextBox With {
            .Location = New Point(80, 180),
            .Size = New Size(340, 32)
        }

        Me.Controls.Add(txtUsername)

        ' --------------------------------------------------------
        ' PASSWORD LABEL
        ' --------------------------------------------------------

        lblPassword = New Label With {
            .Text = "Password:",
            .Location = New Point(80, 230),
            .AutoSize = True,
            .Font =
                New Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                )
        }

        Me.Controls.Add(lblPassword)

        ' --------------------------------------------------------
        ' PASSWORD TEXTBOX
        ' --------------------------------------------------------

        txtPassword = New TextBox With {
            .Location = New Point(80, 260),
            .Size = New Size(340, 32),
            .UseSystemPasswordChar = True
        }

        Me.Controls.Add(txtPassword)

        ' --------------------------------------------------------
        ' SHOW PASSWORD
        ' --------------------------------------------------------

        chkShowPassword = New CheckBox With {
            .Text = "Show Password",
            .Location = New Point(80, 300),
            .AutoSize = True
        }

        AddHandler chkShowPassword.CheckedChanged,
            AddressOf ShowPasswordChanged

        Me.Controls.Add(chkShowPassword)

        ' --------------------------------------------------------
        ' LOGIN BUTTON
        ' --------------------------------------------------------

        btnLogin = New Button With {
            .Text = "LOGIN",
            .Location = New Point(80, 345),
            .Size = New Size(160, 45),
            .BackColor =
                Color.FromArgb(39, 174, 96),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font =
                New Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                ),
            .UseVisualStyleBackColor = False
        }

        btnLogin.FlatAppearance.BorderSize = 0

        AddHandler btnLogin.Click,
            AddressOf LoginButtonClicked

        Me.Controls.Add(btnLogin)

        ' --------------------------------------------------------
        ' EXIT BUTTON
        ' --------------------------------------------------------

        btnExit = New Button With {
            .Text = "EXIT",
            .Location = New Point(260, 345),
            .Size = New Size(160, 45),
            .BackColor =
                Color.FromArgb(127, 140, 141),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font =
                New Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                ),
            .UseVisualStyleBackColor = False
        }

        btnExit.FlatAppearance.BorderSize = 0

        AddHandler btnExit.Click,
            AddressOf ExitButtonClicked

        Me.Controls.Add(btnExit)

        ' --------------------------------------------------------
        ' ENTER KEY
        ' --------------------------------------------------------

        Me.AcceptButton = btnLogin

        ' --------------------------------------------------------
        ' INITIAL FOCUS
        ' --------------------------------------------------------

        Me.ActiveControl = txtUsername

    End Sub

    ' ============================================================
    ' SHOW / HIDE PASSWORD
    ' ============================================================

    Private Sub ShowPasswordChanged(
        sender As Object,
        e As EventArgs
    )

        txtPassword.UseSystemPasswordChar =
            Not chkShowPassword.Checked

    End Sub

    ' ============================================================
    ' LOGIN
    ' ============================================================

    Private Sub LoginButtonClicked(
        sender As Object,
        e As EventArgs
    )

        Dim username As String =
            txtUsername.Text.Trim()

        Dim password As String =
            txtPassword.Text

        ' --------------------------------------------------------
        ' VALIDATION
        ' --------------------------------------------------------

        If String.IsNullOrWhiteSpace(username) Then

            MessageBox.Show(
                "Please enter your username.",
                "Login",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtUsername.Focus()

            Return

        End If

        If String.IsNullOrWhiteSpace(password) Then

            MessageBox.Show(
                "Please enter your password.",
                "Login",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtPassword.Focus()

            Return

        End If

        Try

            Dim passwordHash As String =
                HashPassword(password)

            Dim sql As String =
                "SELECT " &
                "UserID, " &
                "Username, " &
                "FullName, " &
                "Role " &
                "FROM Users " &
                "WHERE Username = @Username " &
                "AND PasswordHash = @PasswordHash " &
                "AND IsActive = 1"

            Using conn As SqlConnection =
                DatabaseHelper.GetConnection()

                Using cmd As New SqlCommand(
                    sql,
                    conn
                )

                    cmd.Parameters.Add(
                        "@Username",
                        SqlDbType.NVarChar,
                        50
                    ).Value = username

                    cmd.Parameters.Add(
                        "@PasswordHash",
                        SqlDbType.NVarChar,
                        64
                    ).Value = passwordHash

                    conn.Open()

                    Using reader As SqlDataReader =
                        cmd.ExecuteReader()

                        If reader.Read() Then

                            LoggedInUserID =
                                Convert.ToInt32(
                                    reader("UserID")
                                )

                            LoggedInUsername =
                                reader(
                                    "Username"
                                ).ToString()

                            LoggedInFullName =
                                reader(
                                    "FullName"
                                ).ToString()

                            LoggedInRole =
                                reader(
                                    "Role"
                                ).ToString()

                            MessageBox.Show(
                                "Welcome, " &
                                LoggedInFullName &
                                "!" &
                                Environment.NewLine &
                                "Role: " &
                                LoggedInRole,
                                "Login Successful",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            )

                            Me.DialogResult =
                                DialogResult.OK

                            Me.Close()

                        Else

                            MessageBox.Show(
                                "Invalid username or password.",
                                "Login Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            )

                            txtPassword.Clear()

                            txtPassword.Focus()

                        End If

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Unable to connect to the database." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    ' ============================================================
    ' SHA-256 PASSWORD HASH
    ' ============================================================

    Private Function HashPassword(
        password As String
    ) As String

        Using sha256 As SHA256 =
            SHA256.Create()

            Dim bytes As Byte() =
                Encoding.UTF8.GetBytes(password)

            Dim hash As Byte() =
                sha256.ComputeHash(bytes)

            Dim builder As New StringBuilder()

            For Each b As Byte In hash

                builder.Append(
                    b.ToString("x2")
                )

            Next

            Return builder.ToString()

        End Using

    End Function

    ' ============================================================
    ' EXIT
    ' ============================================================

    Private Sub ExitButtonClicked(
        sender As Object,
        e As EventArgs
    )

        Application.Exit()

    End Sub

End Class