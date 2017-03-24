USE [Boilerplate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](16) NOT NULL,
	[Role] [varchar](8) NOT NULL,
	[Password] [varchar](64) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Users] ON 
GO

INSERT [dbo].[Users] ([UserID], [Username], [Role], [Password]) VALUES (1, N'admin', N'Admin', N'$2b$10$vrtHbt7mw1jz8HuVKa5UWepKN84c8x.4a3/RLYS.UgQpbItUHsdGy')
GO
INSERT [dbo].[Users] ([UserID], [Username], [Role], [Password]) VALUES (2, N'user', N'User', N'$2b$10$MdjR59HqP.UVxzMHzuRtWumfp9IHEGhJ9Lv7BL51IN4pDLksww6I6')
GO

SET IDENTITY_INSERT [dbo].[Users] OFF
GO
