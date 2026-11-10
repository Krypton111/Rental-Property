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

    Private Class PropertyInfo
        Public Category As String
        Public Title As String
        Public Description As String
        Public CoverImage As String
        Public SourceUrl As String
        Public SourceName As String
        Public Gallery As String()()
        Public Captions As String()
    End Class

    Public Sub New()
        BuildShowcase()
    End Sub

    Private Function ImageUrl(photoId As String) As String
        Return "https://images.unsplash.com/" & photoId & "?auto=format&fit=crop&w=1100&q=85"
    End Function

    Private Function BuildProperties() As List(Of PropertyInfo)
        Dim items As New List(Of PropertyInfo)

        items.Add(New PropertyInfo With {
            .Category = "APARTMENT",
            .Title = "Modern apartment living",
            .Description = "Explore multi-unit residential living, from welcoming building exteriors to thoughtfully planned everyday spaces.",
            .CoverImage = ImageUrl("photo-1545324418-cc1a3fa10c00"),
            .SourceUrl = "https://unsplash.com/s/photos/apartment-building",
            .SourceName = "Unsplash · Apartment architecture",
            .Gallery = New String()() {
                New String() {ImageUrl("photo-1545324418-cc1a3fa10c00"), "Building exterior"},
                New String() {ImageUrl("photo-1522708323590-d24dbb6b0267"), "Living and lounge area"},
                New String() {ImageUrl("photo-1600607687939-ce8a6c25118c"), "Open-plan interior"},
                New String() {ImageUrl("photo-1600210492486-724fe5c67fb0"), "Dining and living space"},
                New String() {ImageUrl("photo-1600607687644-c7171b42498f"), "Contemporary kitchen"},
                New String() {ImageUrl("photo-1616486338812-3dadae4b4ace"), "Bedroom and resting area"}
            },
            .Captions = New String() {"Building exterior", "Living and lounge area", "Open-plan interior", "Dining and living space", "Contemporary kitchen", "Bedroom and resting area"}
        })

        items.Add(New PropertyInfo With {
            .Category = "HOUSE",
            .Title = "A place to call home",
            .Description = "A closer look at standalone homes, including their outdoor spaces, family areas, bedrooms, and kitchens.",
            .CoverImage = ImageUrl("photo-1600585154340-be6161a56a0c"),
            .SourceUrl = "https://unsplash.com/s/photos/modern-house",
            .SourceName = "Unsplash · House inspiration",
            .Gallery = New String()() {
                New String() {ImageUrl("photo-1600585154340-be6161a56a0c"), "House exterior"},
                New String() {ImageUrl("photo-1600607687939-ce8a6c25118c"), "Main living room"},
                New String() {ImageUrl("photo-1600566753086-00f18fb6b3ea"), "Kitchen interior"},
                New String() {ImageUrl("photo-1600607687920-4e2a09cf159d"), "Dining area"},
                New String() {ImageUrl("photo-1600566753190-17f0baa2a6c3"), "Bedroom interior"},
                New String() {ImageUrl("photo-1600585154526-990dced4db0d"), "Outdoor patio"}
            },
            .Captions = New String() {"House exterior", "Main living room", "Kitchen interior", "Dining area", "Bedroom interior", "Outdoor patio"}
        })

        items.Add(New PropertyInfo With {
            .Category = "CONDOMINIUM",
            .Title = "Elevated condo interiors",
            .Description = "Discover compact, polished condominium spaces and the shared lifestyle often found in residential towers.",
            .CoverImage = ImageUrl("photo-1600607687920-4e2a09cf159d"),
            .SourceUrl = "https://unsplash.com/s/photos/condominium-interior",
            .SourceName = "Unsplash · Condominium interiors",
            .Gallery = New String()() {
                New String() {ImageUrl("photo-1600607687920-4e2a09cf159d"), "Condo living area"},
                New String() {ImageUrl("photo-1600210492486-724fe5c67fb0"), "Open-plan lounge"},
                New String() {ImageUrl("photo-1600607687644-c7171b42498f"), "Kitchen and dining"},
                New String() {ImageUrl("photo-1616486029423-aaa4789e8c9a"), "Bedroom interior"},
                New String() {ImageUrl("photo-1618221195710-dd6b41faaea6"), "Styled interior details"},
                New String() {ImageUrl("photo-1486406146926-c627a92ad1ab"), "High-rise architecture"}
            },
            .Captions = New String() {"Condo living area", "Open-plan lounge", "Kitchen and dining", "Bedroom interior", "Styled interior details", "High-rise architecture"}
        })

        items.Add(New PropertyInfo With {
            .Category = "ROOM",
            .Title = "Comfortable private rooms",
            .Description = "Browse practical room arrangements, from sleeping and study areas to shared kitchens and compact amenities.",
            .CoverImage = ImageUrl("photo-1616486338812-3dadae4b4ace"),
            .SourceUrl = "https://unsplash.com/s/photos/furnished-bedroom",
            .SourceName = "Unsplash · Room inspiration",
            .Gallery = New String()() {
                New String() {ImageUrl("photo-1616486338812-3dadae4b4ace"), "Furnished bedroom"},
                New String() {ImageUrl("photo-1616486029423-aaa4789e8c9a"), "Bedroom styling"},
                New String() {ImageUrl("photo-1615874694520-474822394e73"), "Compact room layout"},
                New String() {ImageUrl("photo-1618221195710-dd6b41faaea6"), "Interior details"},
                New String() {ImageUrl("photo-1522708323590-d24dbb6b0267"), "Small lounge corner"},
                New String() {ImageUrl("photo-1600607687644-c7171b42498f"), "Kitchenette inspiration"}
            },
            .Captions = New String() {"Furnished bedroom", "Bedroom styling", "Compact room layout", "Interior details", "Small lounge corner", "Kitchenette inspiration"}
        })

        items.Add(New PropertyInfo With {
            .Category = "COMMERCIAL SPACE",
            .Title = "Space for business",
            .Description = "See flexible commercial environments suited to retail, offices, customer-facing services, and business operations.",
            .CoverImage = ImageUrl("photo-1441986300917-64674bd600d8"),
            .SourceUrl = "https://unsplash.com/s/photos/commercial-retail-interior",
            .SourceName = "Unsplash · Commercial spaces",
            .Gallery = New String()() {
                New String() {ImageUrl("photo-1441986300917-64674bd600d8"), "Retail storefront"},
                New String() {ImageUrl("photo-1771033834141-023d630b3965"), "Retail floor and displays"},
                New String() {ImageUrl("photo-1497366754035-f200968a6e72"), "Modern office workspace"},
                New String() {ImageUrl("photo-1497366216548-37526070297c"), "Collaborative work area"},
                New String() {ImageUrl("photo-1497366811353-6870744d04b2"), "Office meeting space"},
                New String() {ImageUrl("photo-1758448500688-3ababa93fd67"), "Business reception area"}
            },
            .Captions = New String() {"Retail storefront", "Retail floor and displays", "Modern office workspace", "Collaborative work area", "Office meeting space", "Business reception area"}
        })

        Return items
    End Function

    Private Sub BuildShowcase()
        Me.Text = "Casa Brew - Showcase Preview"
        Me.BackColor = Cream
        Me.Font = New Font("Segoe UI", 9.5F)
        Me.FormBorderStyle = FormBorderStyle.None
        Me.TopLevel = False
        Me.Dock = DockStyle.Fill
        Me.AutoScroll = False

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
            .Text = "Choose a property type to explore its spaces, layout, and design inspiration.",
            .Location = New Point(2, 70),
            .Size = New Size(850, 28),
            .Font = New Font("Segoe UI", 9.5F),
            .ForeColor = Coffee
        }

        header.Controls.AddRange(New Control() {eyebrow, heading, intro})
        Me.Controls.Add(header)

        Dim note As New Label With {
            .Text = "5 PROPERTY TYPES  •  REFERENCE PHOTOS ONLY  •  SELECT A CARD TO OPEN ITS FULL SHOWCASE",
            .Location = New Point(28, 122),
            .Size = New Size(930, 25),
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .ForeColor = MutedText,
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        }
        Me.Controls.Add(note)

        Dim gallery As New FlowLayoutPanel With {
            .Location = New Point(25, 152),
            .Size = New Size(Math.Max(620, Me.ClientSize.Width - 50), Math.Max(320, Me.ClientSize.Height - 190)),
            .BackColor = Cream,
            .AutoScroll = True,
            .WrapContents = True,
            .FlowDirection = FlowDirection.LeftToRight,
            .Padding = New Padding(4, 4, 12, 18),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        }

        Dim propertyItems As List(Of PropertyInfo) = BuildProperties()
        For index As Integer = 0 To propertyItems.Count - 1
            Dim item As PropertyInfo = propertyItems(index)
            AddPropertyCard(gallery, item)

            ' Force the first three cards onto row one and the remaining two onto row two.
            If index = 2 Then
                gallery.SetFlowBreak(gallery.Controls(gallery.Controls.Count - 1), True)
            End If
        Next

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

    Private Sub AddPropertyCard(gallery As FlowLayoutPanel, item As PropertyInfo)
        Dim card As New Panel With {
            .Size = New Size(280, 350),
            .BackColor = CardColor,
            .Margin = New Padding(0, 0, 12, 16),
            .Padding = New Padding(0),
            .Cursor = Cursors.Hand
        }

        Dim photo As New PictureBox With {
            .Location = New Point(0, 0),
            .Size = New Size(280, 170),
            .SizeMode = PictureBoxSizeMode.Zoom,
            .BackColor = Color.FromArgb(232, 222, 210),
            .Cursor = Cursors.Hand
        }
        AddImageFallback(photo, "PROPERTY PHOTO", "The photo could not be loaded.")
        Try
            photo.LoadAsync(item.CoverImage)
        Catch
        End Try

        Dim tag As New Label With {
            .Text = item.Category,
            .Location = New Point(16, 182),
            .Size = New Size(248, 20),
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .ForeColor = Caramel,
            .Cursor = Cursors.Hand
        }

        Dim name As New Label With {
            .Text = item.Title,
            .Location = New Point(16, 205),
            .Size = New Size(248, 31),
            .Font = New Font("Georgia", 12.0F, FontStyle.Bold),
            .ForeColor = TextDark,
            .Cursor = Cursors.Hand
        }

        Dim details As New Label With {
            .Text = item.Description,
            .Location = New Point(16, 241),
            .Size = New Size(248, 65),
            .Font = New Font("Segoe UI", 8.5F),
            .ForeColor = Coffee,
            .Cursor = Cursors.Hand
        }

        Dim action As New Label With {
            .Text = "VIEW PHOTO SHOWCASE  →",
            .Location = New Point(16, 316),
            .Size = New Size(248, 22),
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .ForeColor = CoffeeDark,
            .Cursor = Cursors.Hand
        }

        AddHandler action.Click, Sub(sender, e) OpenPropertyShowcase(item)
        AddHandler card.Click, Sub(sender, e) OpenPropertyShowcase(item)
        AddHandler photo.Click, Sub(sender, e) OpenPropertyShowcase(item)
        AddHandler tag.Click, Sub(sender, e) OpenPropertyShowcase(item)
        AddHandler name.Click, Sub(sender, e) OpenPropertyShowcase(item)
        AddHandler details.Click, Sub(sender, e) OpenPropertyShowcase(item)

        card.Controls.AddRange(New Control() {photo, tag, name, details, action})
        gallery.Controls.Add(card)
    End Sub

    Private Sub AddImageFallback(photo As PictureBox, heading As String, message As String)
        AddHandler photo.LoadCompleted,
            Sub(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
                If e.Error IsNot Nothing OrElse e.Cancelled Then
                    photo.Image = Nothing
                    photo.BackColor = Color.FromArgb(224, 213, 199)
                    Dim fallback As New Label With {
                        .Text = heading & Environment.NewLine & message,
                        .Dock = DockStyle.Fill,
                        .TextAlign = ContentAlignment.MiddleCenter,
                        .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
                        .ForeColor = MutedText,
                        .BackColor = Color.FromArgb(224, 213, 199)
                    }
                    photo.Controls.Add(fallback)
                End If
            End Sub
    End Sub

    Private Sub OpenPropertyShowcase(item As PropertyInfo)
        Dim detail As New PropertyShowcaseDetailForm(item.Category, item.Title, item.Description, item.SourceUrl, item.SourceName, item.Gallery)
        Dim owner As Form = Me.FindForm()
        If owner IsNot Nothing Then
            detail.ShowDialog(owner)
        Else
            detail.ShowDialog()
        End If
    End Sub
End Class

Public Class PropertyShowcaseDetailForm
    Inherits Form

    Private ReadOnly CoffeeDark As Color = Color.FromArgb(62, 39, 35)
    Private ReadOnly Coffee As Color = Color.FromArgb(111, 78, 55)
    Private ReadOnly Caramel As Color = Color.FromArgb(166, 123, 91)
    Private ReadOnly Cream As Color = Color.FromArgb(245, 237, 227)
    Private ReadOnly CardColor As Color = Color.FromArgb(255, 249, 243)
    Private ReadOnly TextDark As Color = Color.FromArgb(59, 48, 42)
    Private ReadOnly MutedText As Color = Color.FromArgb(130, 112, 98)

    Public Sub New(category As String, title As String, description As String, sourceUrl As String, sourceName As String, photos As String()())
        BuildWindow(category, title, description, sourceUrl, sourceName, photos)
    End Sub

    Private Sub BuildWindow(category As String, title As String, description As String, sourceUrl As String, sourceName As String, photos As String()())
        Me.Text = "Casa Brew | " & category & " Showcase"

        Dim workArea As Rectangle = Screen.FromPoint(Cursor.Position).WorkingArea
        Dim windowWidth As Integer = Math.Min(1120, Math.Max(700, workArea.Width - 40))
        Dim windowHeight As Integer = Math.Min(820, Math.Max(520, workArea.Height - 40))

        Me.StartPosition = FormStartPosition.CenterParent
        Me.Size = New Size(windowWidth, windowHeight)
        Me.MinimumSize = New Size(Math.Min(700, windowWidth), Math.Min(520, windowHeight))
        Me.BackColor = Cream
        Me.Font = New Font("Segoe UI", 9.5F)
        Me.FormBorderStyle = FormBorderStyle.Sizable

        ' A three-row layout reserves dedicated space for the heading and footer.
        ' The photo gallery receives all remaining height and owns its scrollbars.
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 3,
            .Margin = New Padding(0),
            .Padding = New Padding(0),
            .BackColor = Cream
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 150.0F))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 58.0F))

        Dim header As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Cream,
            .Padding = New Padding(28, 14, 28, 8),
            .Margin = New Padding(0)
        }

        Dim eyebrow As New Label With {
            .Text = "CASA BREW  /  PROPERTY SHOWCASE",
            .Location = New Point(0, 0),
            .Size = New Size(500, 22),
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .ForeColor = Caramel
        }

        Dim heading As New Label With {
            .Text = title,
            .Location = New Point(0, 23),
            .Size = New Size(760, 39),
            .Font = New Font("Georgia", 20.0F, FontStyle.Bold),
            .ForeColor = TextDark
        }

        Dim intro As New Label With {
            .Text = description,
            .Location = New Point(0, 65),
            .Size = New Size(900, 38),
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = Coffee
        }

        Dim count As New Label With {
            .Text = photos.Length.ToString() & " PHOTO ANGLES  •  " & category,
            .Location = New Point(0, 108),
            .Size = New Size(700, 22),
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .ForeColor = MutedText
        }

        header.Controls.AddRange(New Control() {eyebrow, heading, intro, count})

        Dim gallery As New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .BackColor = Cream,
            .AutoScroll = True,
            .WrapContents = True,
            .FlowDirection = FlowDirection.LeftToRight,
            .Padding = New Padding(24, 12, 24, 24),
            .Margin = New Padding(0)
        }

        For Each photoInfo As String() In photos
            AddPhotoTile(gallery, photoInfo(0), photoInfo(1))
        Next

        Dim footer As New Panel With {
            .Dock = DockStyle.Fill,
            .Height = 58,
            .BackColor = CardColor,
            .Padding = New Padding(24, 8, 24, 8),
            .Margin = New Padding(0)
        }

        Dim source As New LinkLabel With {
            .Text = sourceName.ToUpperInvariant() & "  ↗",
            .Location = New Point(25, 18),
            .Size = New Size(420, 22),
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .LinkColor = Caramel,
            .ActiveLinkColor = CoffeeDark
        }
        AddHandler source.LinkClicked,
            Sub(sender, e)
                Try
                    Process.Start(New ProcessStartInfo(sourceUrl) With {.UseShellExecute = True})
                Catch ex As Exception
                    MessageBox.Show("Unable to open the photo source." & Environment.NewLine & ex.Message,
                                    "Casa Brew", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End Sub

        Dim close As New Button With {
            .Text = "CLOSE SHOWCASE",
            .Size = New Size(150, 34),
            .Location = New Point(footer.ClientSize.Width - 175, 12),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = CoffeeDark,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .Cursor = Cursors.Hand
        }
        close.FlatAppearance.BorderSize = 0
        AddHandler close.Click, Sub(sender, e) Me.Close()
        AddHandler footer.Resize,
            Sub(sender, e)
                close.Location = New Point(Math.Max(10, footer.ClientSize.Width - close.Width - 24), 12)
            End Sub

        footer.Controls.Add(source)
        footer.Controls.Add(close)

        layout.Controls.Add(header, 0, 0)
        layout.Controls.Add(gallery, 0, 1)
        layout.Controls.Add(footer, 0, 2)
        Me.Controls.Add(layout)
    End Sub

    Private Sub AddPhotoTile(gallery As FlowLayoutPanel, imageUrl As String, caption As String)
        Dim tile As New Panel With {
            .Size = New Size(480, 300),
            .BackColor = CardColor,
            .Margin = New Padding(0, 0, 18, 18)
        }

        Dim photo As New PictureBox With {
            .Location = New Point(0, 0),
            .Size = New Size(480, 252),
            .SizeMode = PictureBoxSizeMode.Zoom,
            .BackColor = Color.FromArgb(232, 222, 210)
        }

        AddHandler photo.LoadCompleted,
            Sub(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
                If e.Error IsNot Nothing OrElse e.Cancelled Then
                    photo.Image = Nothing
                    photo.BackColor = Color.FromArgb(224, 213, 199)
                    photo.Controls.Add(New Label With {
                        .Text = "PHOTO UNAVAILABLE" & Environment.NewLine & "Please check your internet connection.",
                        .Dock = DockStyle.Fill,
                        .TextAlign = ContentAlignment.MiddleCenter,
                        .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                        .ForeColor = MutedText,
                        .BackColor = Color.FromArgb(224, 213, 199)
                    })
                End If
            End Sub

        Try
            photo.LoadAsync(imageUrl)
        Catch
        End Try

        Dim label As New Label With {
            .Text = caption,
            .Location = New Point(18, 261),
            .Size = New Size(444, 28),
            .Font = New Font("Georgia", 10.5F, FontStyle.Bold),
            .ForeColor = TextDark
        }

        tile.Controls.Add(photo)
        tile.Controls.Add(label)
        gallery.Controls.Add(tile)
    End Sub
End Class
