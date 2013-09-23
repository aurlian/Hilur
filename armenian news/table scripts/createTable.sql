-- =========================================
-- Create table script
-- =========================================
USE Hilur
GO

IF OBJECT_ID('dbo.Entry', 'U') IS NOT NULL
  DROP TABLE dbo.Entry


GO

CREATE TABLE [dbo].[Entry](
	[Id] [int] IDENTITY(1,1) NOT NULL,	
	[Title] [varchar](200) NOT NULL,
	[PubDate] DateTime NOT NULL,
	[Link] [varchar](300) NOT NULL,
	[Post] [Text] NOT NULL,
	[FetchDate] DateTime NOT NULL,
	[Votes] int NOT NULL DEFAULT 0
 CONSTRAINT [PK_entries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

IF OBJECT_ID('dbo.SiteUser', 'U') IS NOT NULL
  DROP TABLE dbo.SiteUser


GO

CREATE TABLE [dbo].[SiteUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,	
	[Username] [varchar](200) NOT NULL,
	[JoinDate] DateTime NOT NULL,
	[PostCount] int NOT NULL,
	[SignupIp] varchar(100) NOT NULL,
	[AvatarUrl] varchar(200) NOT NULL
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

IF OBJECT_ID('dbo.Comment', 'U') IS NOT NULL
  DROP TABLE dbo.Comment

GO

CREATE TABLE [dbo].[Comment](
	[Id] [int] IDENTITY(1,1) NOT NULL,	
	[ArticleId] int NOT NULL,
	[Description] text NOT NULL,
	[Title] varchar(50) NOT NULL,
	[Date] DateTime NOT NULL,
	[UserId] int NOT NULL
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO


ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_comment_user] FOREIGN KEY([UserId])
REFERENCES [dbo].[SiteUser] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_comment_user]
GO

ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_comment_article] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Entry] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_comment_article]
GO




IF OBJECT_ID('dbo.Vote', 'U') IS NOT NULL
  DROP TABLE dbo.Vote

GO

CREATE TABLE [dbo].[Vote](
	[Id] [int] IDENTITY(1,1) NOT NULL,	
	[ArticleId] int NOT NULL,
	[Vote] bit Not NULL,
	[Date] DateTime NOT NULL,
	[UserId] int NOT NULL
 CONSTRAINT [PK_Vote] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

ALTER TABLE [dbo].[Vote]  WITH CHECK ADD  CONSTRAINT [FK_vote_user] FOREIGN KEY([UserId])
REFERENCES [dbo].[SiteUser] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Vote] CHECK CONSTRAINT [FK_vote_user]
GO

ALTER TABLE [dbo].[Vote]  WITH CHECK ADD  CONSTRAINT [FK_vote_article] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Entry] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Vote] CHECK CONSTRAINT [FK_vote_article]
GO