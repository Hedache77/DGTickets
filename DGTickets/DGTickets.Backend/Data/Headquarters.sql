SET IDENTITY_INSERT [dbo].[Headquarters] ON 
INSERT [dbo].[Headquarters] ([Id], [Name], [Address], [PhoneNumber], [Email], [CityId]) VALUES (1, N'Centro',N'Av. siempre viva', 123456789,  N'centro@centro.com', 1);
SET IDENTITY_INSERT [dbo].[Headquarters] OFF