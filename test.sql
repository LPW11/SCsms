USE [master]
GO
/****** Object:  Database [SCsms]    Script Date: 27/11/2023 下午 11:03:05 ******/
CREATE DATABASE [SCsms]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SCsms', FILENAME = N'E:\SQL2022-SSEI-Expr\Express\R\MSSQL16.MSSQLSERVER\MSSQL\DATA\SCsms.ndf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SCsms_log', FILENAME = N'E:\SQL2022-SSEI-Expr\Express\R\MSSQL16.MSSQLSERVER\MSSQL\DATA\SCsms_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SCsms] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SCsms].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SCsms] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SCsms] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SCsms] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SCsms] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SCsms] SET ARITHABORT OFF 
GO
ALTER DATABASE [SCsms] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SCsms] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SCsms] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SCsms] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SCsms] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SCsms] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SCsms] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SCsms] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SCsms] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SCsms] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SCsms] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SCsms] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SCsms] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SCsms] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SCsms] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SCsms] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SCsms] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SCsms] SET RECOVERY FULL 
GO
ALTER DATABASE [SCsms] SET  MULTI_USER 
GO
ALTER DATABASE [SCsms] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SCsms] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SCsms] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SCsms] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SCsms] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SCsms] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SCsms] SET QUERY_STORE = ON
GO
ALTER DATABASE [SCsms] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SCsms]
GO
/****** Object:  Table [dbo].[Athletes]    Script Date: 27/11/2023 下午 11:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Athletes](
	[AthleteID] [int] NOT NULL,
	[EventID] [int] NULL,
	[UserName] [varchar](255) NOT NULL,
	[Sex] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[AthleteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 27/11/2023 下午 11:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[EventID] [int] NOT NULL,
	[EventName] [varchar](255) NOT NULL,
	[EventDate] [datetime2](7) NOT NULL,
	[MaxParticipants] [int] NOT NULL,
	[EventGroup] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Results]    Script Date: 27/11/2023 下午 11:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Results](
	[ResultID] [int] NOT NULL,
	[EventID] [int] NULL,
	[AthleteID] [int] NULL,
	[Pre_Time] [time](1) NULL,
	[Fin_Time] [time](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 27/11/2023 下午 11:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] NOT NULL,
	[UserName] [varchar](255) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[UserType] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Athletes] ([AthleteID], [EventID], [UserName], [Sex]) VALUES (1, 1, N'王八', N'男')
INSERT [dbo].[Athletes] ([AthleteID], [EventID], [UserName], [Sex]) VALUES (2, 1, N'李二', N'男')
INSERT [dbo].[Athletes] ([AthleteID], [EventID], [UserName], [Sex]) VALUES (3, 2, N'张三', N'女')
INSERT [dbo].[Athletes] ([AthleteID], [EventID], [UserName], [Sex]) VALUES (4, 1, N'王五', N'男')
INSERT [dbo].[Athletes] ([AthleteID], [EventID], [UserName], [Sex]) VALUES (5, 1, N'萧六', N'男')
INSERT [dbo].[Athletes] ([AthleteID], [EventID], [UserName], [Sex]) VALUES (6, 1, N'小明', N'男')
GO
INSERT [dbo].[Events] ([EventID], [EventName], [EventDate], [MaxParticipants], [EventGroup]) VALUES (1, N'100米自由泳', CAST(N'2023-11-08T10:45:00.0000000' AS DateTime2), 30, N'男子组')
INSERT [dbo].[Events] ([EventID], [EventName], [EventDate], [MaxParticipants], [EventGroup]) VALUES (2, N'200米蛙泳', CAST(N'2023-01-12T00:00:00.0000000' AS DateTime2), 40, N'女子组')
INSERT [dbo].[Events] ([EventID], [EventName], [EventDate], [MaxParticipants], [EventGroup]) VALUES (4, N'100米蝶泳', CAST(N'2023-11-07T00:00:00.0000000' AS DateTime2), 40, N'男子组')
INSERT [dbo].[Events] ([EventID], [EventName], [EventDate], [MaxParticipants], [EventGroup]) VALUES (6, N'300米仰泳', CAST(N'2023-11-13T23:43:00.0000000' AS DateTime2), 40, N'男子组')
GO
INSERT [dbo].[Results] ([ResultID], [EventID], [AthleteID], [Pre_Time], [Fin_Time]) VALUES (1, 1, 1, CAST(N'00:55:20' AS Time), CAST(N'00:54:20' AS Time))
INSERT [dbo].[Results] ([ResultID], [EventID], [AthleteID], [Pre_Time], [Fin_Time]) VALUES (2, 1, 2, CAST(N'00:58:10' AS Time), CAST(N'00:57:10' AS Time))
INSERT [dbo].[Results] ([ResultID], [EventID], [AthleteID], [Pre_Time], [Fin_Time]) VALUES (3, 2, 3, CAST(N'02:30:45' AS Time), CAST(N'02:29:45' AS Time))
INSERT [dbo].[Results] ([ResultID], [EventID], [AthleteID], [Pre_Time], [Fin_Time]) VALUES (4, 1, 4, CAST(N'00:57:10' AS Time), CAST(N'00:56:10' AS Time))
INSERT [dbo].[Results] ([ResultID], [EventID], [AthleteID], [Pre_Time], [Fin_Time]) VALUES (5, 1, 5, CAST(N'00:59:10' AS Time), CAST(N'00:58:10' AS Time))
INSERT [dbo].[Results] ([ResultID], [EventID], [AthleteID], [Pre_Time], [Fin_Time]) VALUES (6, 1, 6, CAST(N'01:00:10' AS Time), NULL)
GO
INSERT [dbo].[Users] ([UserID], [UserName], [Password], [UserType]) VALUES (1, N'admin', N'admin123', N'Admin')
INSERT [dbo].[Users] ([UserID], [UserName], [Password], [UserType]) VALUES (2, N'user1', N'user123', N'Normal')
INSERT [dbo].[Users] ([UserID], [UserName], [Password], [UserType]) VALUES (3, N'user2', N'user456', N'Normal')
INSERT [dbo].[Users] ([UserID], [UserName], [Password], [UserType]) VALUES (4, N'user459', N'123', N'Normal')
INSERT [dbo].[Users] ([UserID], [UserName], [Password], [UserType]) VALUES (5, N'user458', N'123456', N'Normal')
GO
ALTER TABLE [dbo].[Athletes]  WITH CHECK ADD FOREIGN KEY([EventID])
REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[Results]  WITH CHECK ADD FOREIGN KEY([AthleteID])
REFERENCES [dbo].[Athletes] ([AthleteID])
GO
ALTER TABLE [dbo].[Results]  WITH CHECK ADD FOREIGN KEY([EventID])
REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([UserType]='Normal' OR [UserType]='Admin'))
GO
USE [master]
GO
ALTER DATABASE [SCsms] SET  READ_WRITE 
GO
