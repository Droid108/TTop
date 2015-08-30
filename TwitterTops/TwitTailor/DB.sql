USE [TRDB2015]
GO

/****** Object:  Table [dbo].[tblCatTravel]    Script Date: 8/14/2015 5:02:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatTravel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatTravel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO



/****** Object:  Table [dbo].[tblCatSports]    Script Date: 8/14/2015 5:01:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatSports](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatSports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO



/****** Object:  Table [dbo].[tblCatNews]    Script Date: 8/14/2015 5:01:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatNews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatNews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[tblCatMusic]    Script Date: 8/14/2015 5:00:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatMusic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatMusic] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO



/****** Object:  Table [dbo].[tblCatMovies]    Script Date: 8/14/2015 4:59:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatMovies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatMovies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO



/****** Object:  Table [dbo].[tblCatJokes]    Script Date: 8/14/2015 4:59:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatJokes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatJokes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[tblCatHF]    Script Date: 8/14/2015 4:58:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatHF](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatHF] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatGadgets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatGadgets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[tblCatFacts]    Script Date: 8/14/2015 4:56:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatFacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatFacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[tblCatBusiness]    Script Date: 8/14/2015 4:55:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatBusiness](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatBusiness] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[tblCatAuto]    Script Date: 8/14/2015 4:54:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCatAuto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_tblCatAuto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO



CREATE TABLE [dbo].[tblCatTech](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
	[MediaUrl] [nvarchar](300) NULL,
	[MediaType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblCatTech] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO



CREATE TABLE [dbo].[tblCatScience](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TwitID] [numeric](18, 0) NOT NULL,
	[text] [nvarchar](500) NULL,
	[userid] [numeric](18, 0) NULL,
	[name] [nvarchar](50) NOT NULL,
	[screenname] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NULL,
	[IsVerified] [bit] NOT NULL,
	[ProfileUrl] [nvarchar](150) NULL,
	[RT_Count] [numeric](18, 0) NOT NULL,
	[Fav_Count] [numeric](18, 0) NOT NULL,
	[created_at] [datetime] NULL,
	[MediaUrl] [nvarchar](300) NULL,
	[MediaType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblCatScience] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO


select count( *) from tblcatlove where url is not null and Len(Url) > 0

select count( *) from tblcatAuto where url is not null and Len(Url) > 0

select count( *) from tblcatBusiness where url is not null and Len(Url) > 0

select count( *) from tblcatFacts where url is not null and Len(Url) > 0

select count( *) from tblcatScience where url is not null and Len(Url) > 0

select count( *) from tblcatJokes where url is not null and Len(Url) > 0

select count( *) from tblcatTech where url is not null and Len(Url) > 0

select count( *) from tblcatSports where url is not null and Len(Url) > 0

select count( *) from tblcatNews where url is not null and Len(Url) > 0

--sp_helptext 'USP_FetchCATLoveTweets'

select top 1000 * from tblcatlove where screenname = 'LovesQuote0'

elete from tblcatlove where screenname = 'LovesQuote0'





CREATE Procedure [dbo].[USP_FetchCATLoveTweets]  

 @fType int,  

 @fromId Int  

  

As  

Begin  

if (@fType = 0)  

BEGIN  

 select top 50 Id, TwitID, text, userid, name, screenname, location,IsVerified, ProfileUrl, RT_Count, Fav_Count, created_at, MediaUrl, MediaType from tblCatLove Where Id > @fromId order by id desc   

END  

ELSE   

BEGIN  

 select top 50 Id, TwitID, text, userid, name, screenname, location,IsVerified, ProfileUrl, RT_Count, Fav_Count, created_at, MediaUrl, MediaType from tblCatLove Where Id < @fromId order by id desc   

END   

   

End  

  
