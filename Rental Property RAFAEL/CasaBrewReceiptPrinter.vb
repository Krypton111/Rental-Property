Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Public NotInheritable Class CasaBrewReceiptPrinter

    Private Sub New()
    End Sub

    Public Shared Sub Preview(paymentID As Integer)
        Dim receipt As New DataTable()

        Using connection = DatabaseHelper.GetConnection()
            connection.Open()

            Dim sql As String =
                "SELECT pay.PaymentID, pay.PaymentDate, pay.AmountDue, pay.AmountPaid, " &
                "pay.PaymentMethod, pay.PaymentStatus, pay.Notes, " &
                "t.TenantName, t.ContactNumber, t.Email, t.Address, " &
                "p.PropertyName, p.PropertyAddress " &
                "FROM Payments pay " &
                "INNER JOIN Tenants t ON pay.TenantID = t.TenantID " &
                "LEFT JOIN Properties p ON t.PropertyID = p.PropertyID " &
                "WHERE pay.PaymentID = @PaymentID"

            Using command As New SqlCommand(sql, connection)
                command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = paymentID
                Using adapter As New SqlDataAdapter(command)
                    adapter.Fill(receipt)
                End Using
            End Using
        End Using

        If receipt.Rows.Count = 0 Then
            MessageBox.Show("The selected payment could not be found.", "Casa Brew Receipt",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim row As DataRow = receipt.Rows(0)
        Dim amountDue As Decimal = Convert.ToDecimal(row("AmountDue"))
        Dim amountPaid As Decimal = Convert.ToDecimal(row("AmountPaid"))
        Dim balance As Decimal = Math.Max(0D, amountDue - amountPaid)
        Dim document As New PrintDocument()
        document.DocumentName = "Casa Brew - Receipt #" & paymentID.ToString()
        document.DefaultPageSettings.Margins = New Margins(55, 55, 45, 45)

        AddHandler document.PrintPage,
            Sub(sender As Object, e As PrintPageEventArgs)
                Dim g As Graphics = e.Graphics
                Dim left As Single = e.MarginBounds.Left
                Dim right As Single = e.MarginBounds.Right
                Dim width As Single = e.MarginBounds.Width
                Dim y As Single = e.MarginBounds.Top
                Dim brown As Color = Color.FromArgb(62, 39, 35)
                Dim coffee As Color = Color.FromArgb(111, 78, 55)
                Dim caramel As Color = Color.FromArgb(166, 123, 91)
                Dim cream As Color = Color.FromArgb(245, 237, 227)
                Using titleFont As New Font("Georgia", 25, FontStyle.Bold),
                      subFont As New Font("Georgia", 10, FontStyle.Italic),
                      headingFont As New Font("Segoe UI", 9, FontStyle.Bold),
                      bodyFont As New Font("Segoe UI", 9),
                      smallFont As New Font("Segoe UI", 8),
                      amountFont As New Font("Georgia", 17, FontStyle.Bold),
                      brownBrush As New SolidBrush(brown),
                      coffeeBrush As New SolidBrush(coffee),
                      grayBrush As New SolidBrush(Color.FromArgb(105, 92, 82)),
                      linePen As New Pen(caramel, 1.2F),
                      softPen As New Pen(Color.FromArgb(220, 205, 188), 1)
                    g.DrawString("CASA BREW", titleFont, brownBrush, left, y)
                    y += 36
                    g.DrawString("RENTAL PROPERTY MANAGEMENT", smallFont, coffeeBrush, left + 2, y)
                    g.DrawString("PAYMENT RECEIPT", headingFont, brownBrush,
                                 New RectangleF(left, y - 2, width, 20),
                                 New StringFormat With {.Alignment = StringAlignment.Far})
                    y += 24
                    g.DrawLine(linePen, left, y, right, y)
                    y += 16

                    g.DrawString("RECEIPT NO.", smallFont, grayBrush, left, y)
                    g.DrawString("#CB-" & paymentID.ToString("D6"), headingFont, brownBrush, left, y + 14)
                    g.DrawString("PAYMENT DATE", smallFont, grayBrush, left + width / 2, y)
                    g.DrawString(Convert.ToDateTime(row("PaymentDate")).ToString("MMMM dd, yyyy"),
                                 headingFont, brownBrush, left + width / 2, y + 14)
                    y += 48

                    g.DrawString("RECEIVED FROM", headingFont, coffeeBrush, left, y)
                    y += 19
                    g.DrawString(Safe(row("TenantName")), bodyFont, brownBrush, left, y)
                    y += 17
                    DrawOptional(g, "Contact: " & Safe(row("ContactNumber")), smallFont, grayBrush, left, y, width) : y += 14
                    DrawOptional(g, "Email: " & Safe(row("Email")), smallFont, grayBrush, left, y, width) : y += 14
                    DrawOptional(g, "Address: " & Safe(row("Address")), smallFont, grayBrush, left, y, width) : y += 22

                    g.DrawString("RENTAL DETAILS", headingFont, coffeeBrush, left, y)
                    y += 18
                    g.DrawString("Property", bodyFont, grayBrush, left, y)
                    g.DrawString(Safe(row("PropertyName")), bodyFont, brownBrush, left + 115, y)
                    y += 17
                    DrawOptional(g, "Property address: " & Safe(row("PropertyAddress")), smallFont, grayBrush, left, y, width)
                    y += 25

                    g.DrawLine(softPen, left, y, right, y)
                    y += 12
                    g.DrawString("PAYMENT SUMMARY", headingFont, coffeeBrush, left, y)
                    y += 22
                    DrawAmount(g, "Rental amount due", amountDue, bodyFont, brownBrush, left, right, y) : y += 23
                    DrawAmount(g, "Amount received", amountPaid, bodyFont, brownBrush, left, right, y) : y += 23
                    g.DrawLine(linePen, left, y, right, y)
                    y += 9
                    g.DrawString("REMAINING BALANCE", headingFont, coffeeBrush, left, y)
                    g.DrawString("₱" & balance.ToString("#,##0.00"), amountFont, brownBrush,
                                 New RectangleF(left, y - 3, width, 30),
                                 New StringFormat With {.Alignment = StringAlignment.Far})
                    y += 39

                    g.DrawString("Payment method: " & Safe(row("PaymentMethod")), bodyFont, brownBrush, left, y) : y += 18
                    g.DrawString("Payment status: " & Safe(row("PaymentStatus")), bodyFont, brownBrush, left, y) : y += 20
                    If Safe(row("Notes")) <> "" Then
                        g.DrawString("Notes", headingFont, coffeeBrush, left, y) : y += 16
                        g.DrawString(Safe(row("Notes")), bodyFont, brownBrush, New RectangleF(left, y, width, 48))
                        y += 52
                    End If

                    y += 10
                    g.DrawLine(linePen, left, y, right, y)
                    y += 14
                    g.DrawString("Thank you for choosing Casa Brew.", subFont, coffeeBrush,
                                 New RectangleF(left, y, width, 22),
                                 New StringFormat With {.Alignment = StringAlignment.Center})
                    y += 30
                    g.DrawString("This receipt acknowledges the payment recorded above.", smallFont, grayBrush,
                                 New RectangleF(left, y, width, 18),
                                 New StringFormat With {.Alignment = StringAlignment.Center})
                    y += 44
                    g.DrawLine(softPen, left + 25, y, left + 220, y)
                    g.DrawLine(softPen, right - 220, y, right - 25, y)
                    g.DrawString("Authorized signature", smallFont, grayBrush, left + 65, y + 5)
                    g.DrawString("Tenant acknowledgement", smallFont, grayBrush, right - 205, y + 5)
                End Using
                e.HasMorePages = False
            End Sub

        Using preview As New PrintPreviewDialog()
            preview.Document = document
            preview.Width = 950
            preview.Height = 750
            preview.StartPosition = FormStartPosition.CenterParent
            preview.ShowDialog()
        End Using
        document.Dispose()
    End Sub

    Private Shared Sub DrawAmount(g As Graphics, label As String, amount As Decimal,
                                  font As Font, brush As Brush, left As Single, right As Single, y As Single)
        g.DrawString(label, font, brush, left, y)
        g.DrawString("₱" & amount.ToString("#,##0.00"), font, brush,
                     New RectangleF(left, y, right - left, 18),
                     New StringFormat With {.Alignment = StringAlignment.Far})
    End Sub

    Private Shared Sub DrawOptional(g As Graphics, value As String, font As Font, brush As Brush,
                                    x As Single, y As Single, width As Single)
        If value.Trim() <> "" AndAlso Not value.EndsWith(": ") Then
            g.DrawString(value, font, brush, New RectangleF(x, y, width, 24))
        End If
    End Sub

    Private Shared Function Safe(value As Object) As String
        If value Is Nothing OrElse value Is DBNull.Value Then Return ""
        Return Convert.ToString(value).Trim()
    End Function

End Class
