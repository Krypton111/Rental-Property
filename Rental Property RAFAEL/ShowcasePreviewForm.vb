Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms

Public Class ShowcasePreviewForm
    Inherits Form

    Private ReadOnly CoffeeDark As Color = Color.FromArgb(62, 39, 35)
    Private ReadOnly Coffee As Color = Color.FromArgb(111, 78, 55)
    Private ReadOnly Caramel As Color = Color.FromArgb(166, 123, 91)
    Private ReadOnly Cream As Color = Color.FromArgb(245, 237, 227)
    Private ReadOnly CardColor As Color = Color.FromArgb(255, 249, 243)
    Private ReadOnly TextDark As Color = Color.FromArgb(59, 48, 42)
    Private ReadOnly MutedText As Color = Color.FromArgb(130, 112, 98)

    Public Sub New()
        BuildShowcase()
    End Sub

    Private Sub BuildShowcase()
        Me.Text = "Casa Brew - Showcase Preview"
        Me.BackColor = Cream
        Me.Font = New Font("Segoe UI", 9.5F)
        Me.FormBorderStyle = FormBorderStyle.None
        Me.TopLevel = False
        Me.Dock = DockStyle.Fill

        Dim header As New Panel With {
            .Location = New Point(25, 12),
            .Size = New Size(970, 112),
            .BackColor = Cream,
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        }

        Dim eyebrow As New Label With {
            .Text = "PROPERTY INSPIRATION",
            .Location = New Point(2, 0),
            .Size = New Size(350, 22),
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
            .ForeColor = Caramel
        }

        Dim heading As New Label With {
            .Text = "Explore the possibilities",
            .Location = New Point(0, 22),
            .Size = New Size(720, 42),
            .Font = New Font("Georgia", 21.0F, FontStyle.Bold),
            .ForeColor = TextDark
        }

        Dim intro As New Label With {
            .Text = "A visual collection of the property types managed in Casa Brew.",
            .Location = New Point(2, 70),
            .Size = New Size(800, 28),
            .Font = New Font("Segoe UI", 9.5F),
            .ForeColor = Coffee
        }

        header.Controls.AddRange(New Control() {eyebrow, heading, intro})
        Me.Controls.Add(header)

        Dim note As New Label With {
            .Text = "REFERENCE GALLERY  •  Images are illustrative examples, not active listings.",
            .Location = New Point(28, 122),
            .Size = New Size(930, 25),
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
            .ForeColor = MutedText,
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        }
        Me.Controls.Add(note)

        Dim gallery As New FlowLayoutPanel With {
            .Location = New Point(25, 152),
            .Size = New Size(970, 460),
            .BackColor = Cream,
            .AutoScroll = True,
            .WrapContents = True,
            .FlowDirection = FlowDirection.LeftToRight,
            .Padding = New Padding(0, 0, 8, 12),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        }

        AddPropertyCard(
            gallery,
            "APARTMENT",
            "Modern apartment living",
            "Multi-unit residential buildings with practical layouts, shared facilities, and convenient city access.",
            "https://www.collierscanada.com/-/media/images/colliers/canada/images/services/cre-beyond-2022/hero-the-shift-in-multifamily-investment-mindset--from-cap-rates-to-rental-growth.ashx?bid=1918fc1b881e4c9d97a0a98235b5bf45&hash=88D8BB1438628C46DE07CF0C793F985E&sc_lang=en-ca",
            "https://www.collierscanada.com/en-ca/news/multifamily-investments",
            "Image source: Colliers"
        )

        AddPropertyCard(
            gallery,
            "HOUSE",
            "A place to call home",
            "Standalone homes with private entrances, outdoor space, and room for everyday family living.",
            "https://cdn.myportfolio.com/25f176fd-8640-4f3a-880f-4c405379eb1f/affa0fd4-5a82-4253-8a53-6d82515c1706_rw_1920.jpg?h=72f4d42b27d511bcf63dc51132b4f5dd",
            "https://proabode.co.uk/exteriors",
            "Image source: Pro Abode"
        )

        AddPropertyCard(
            gallery,
            "CONDOMINIUM",
            "Elevated condo interiors",
            "Self-contained units in managed residential towers, often featuring compact, polished interiors and city views.",
            "https://img.lazudi.com/public/articles/545/17530900686798.jpg?mark=null%3Fmark%3Dnull",
            "https://lazudi.com/th-en/blog/buy-to-rent-in-thailand-a-smart-investors-guide",
            "Image source: Lazudi"
        )

        AddPropertyCard(
            gallery,
            "ROOM",
            "Comfortable private rooms",
            "A furnished or unfurnished private room designed for convenient, budget-conscious accommodation.",
            "https://atinn.jp/cache/images/atinn-api_69b0e0ca61bc62_38364879_260311122602_900x600.jpeg",
            "https://atinn.jp/plan/34406",
            "Image source: At Inn"
        )

        AddPropertyCard(
            gallery,
            "COMMERCIAL SPACE",
            "Space for business",
            "Flexible storefronts and commercial interiors suited to retail, services, and other business activities.",
            "https://revivalconstruction.ca/assets/commercial-retail-NCzBQKOU.jpg",
            "https://revivalconstruction.ca/commercial-services/retail",
            "Image source: Revival Construction"
        )

        Me.Controls.Add(gallery)

        Dim footer As New Label With {
            .Text = "Casa Brew  |  Property Showcase",
            .Location = New Point(28, 620),
            .Size = New Size(930, 22),
            .Font = New Font("Segoe UI", 8.0F),
            .ForeColor = MutedText,
            .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        }
        Me.Controls.Add(footer)
    End Sub

    Private Sub AddPropertyCard(
        gallery As FlowLayoutPanel,
        category As String,
        title As String,
        description As String,
        imageUrl As String,
        sourceUrl As String,
        sourceText As String
    )
        Dim card As New Panel With {
            .Size = New Size(300, 390),
            .BackColor = CardColor,
            .Margin = New Padding(0, 0, 18, 18),
            .Padding = New Padding(0)
        }

        Dim photoFrame As New Panel With {
            .Location = New Point(0, 0),
            .Size = New Size(300, 172),
            .BackColor = Color.FromArgb(232, 222, 210)
        }

        Dim photo As New PictureBox With {
            .Location = New Point(0, 0),
            .Size = New Size(300, 172),
            .SizeMode = PictureBoxSizeMode.Zoom,
            .BackColor = Color.FromArgb(232, 222, 210)
        }

        Dim loading As New Label With {
            .Text = "Loading property photo…",
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Italic),
            .ForeColor = MutedText
        }

        photo.Controls.Add(loading)
        photoFrame.Controls.Add(photo)

        AddHandler photo.LoadCompleted,
            Sub(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
                If photo.Controls.Contains(loading) Then
                    photo.Controls.Remove(loading)
                    loading.Dispose()
                End If
                If e.Error IsNot Nothing OrElse e.Cancelled Then
                    photo.Image = Nothing
                    photo.BackColor = Color.FromArgb(224, 213, 199)
                    Dim fallback As New Label With {
                        .Text = "PHOTO UNAVAILABLE" & Environment.NewLine & "Check your internet connection",
                        .Dock = DockStyle.Fill,
                        .TextAlign = ContentAlignment.MiddleCenter,
                        .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
                        .ForeColor = MutedText
                    }
                    photo.Controls.Add(fallback)
                End If
            End Sub

        Try
            photo.LoadAsync(imageUrl)
        Catch
            loading.Text = "Photo could not be loaded"
        End Try

        Dim tag As New Label With {
            .Text = category,
            .Location = New Point(17, 185),
            .Size = New Size(265, 20),
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .ForeColor = Caramel
        }

        Dim name As New Label With {
            .Text = title,
            .Location = New Point(17, 207),
            .Size = New Size(265, 31),
            .Font = New Font("Georgia", 12.0F, FontStyle.Bold),
            .ForeColor = TextDark
        }

        Dim details As New Label With {
            .Text = description,
            .Location = New Point(17, 244),
            .Size = New Size(265, 76),
            .Font = New Font("Segoe UI", 8.5F),
            .ForeColor = Coffee
        }

        Dim divider As New Panel With {
            .Location = New Point(17, 327),
            .Size = New Size(265, 1),
            .BackColor = Color.FromArgb(225, 214, 202)
        }

        Dim source As New LinkLabel With {
            .Text = sourceText & "  ↗",
            .Location = New Point(17, 340),
            .Size = New Size(265, 24),
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .LinkColor = Caramel,
            .ActiveLinkColor = CoffeeDark,
            .VisitedLinkColor = Coffee
        }

        AddHandler source.LinkClicked,
            Sub(sender As Object, e As LinkLabelLinkClickedEventArgs)
                Try
                    Process.Start(New ProcessStartInfo(sourceUrl) With {.UseShellExecute = True})
                Catch ex As Exception
                    MessageBox.Show(
                        "Unable to open the image source." & Environment.NewLine & ex.Message,
                        "Casa Brew",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    )
                End Try
            End Sub

        card.Controls.AddRange(New Control() {
            photoFrame, tag, name, details, divider, source
        })

        gallery.Controls.Add(card)
    End Sub
End Class
