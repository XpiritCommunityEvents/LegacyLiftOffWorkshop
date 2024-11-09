SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Login]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Login](
        [UId] [int] NOT NULL,
        [Username] [nvarchar](50) NOT NULL,
        [Password] [nvarchar](50) NOT NULL,
        [Role] [nvarchar](50) NOT NULL
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[Login] ADD  CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
    (
        [UId] ASC
    )WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FoodPackage]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[FoodPackage](
        [FId] [int] NOT NULL,
        [FName] [nvarchar](50) NOT NULL,
        [FPackageCost] [float] NOT NULL
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[FoodPackage] ADD  CONSTRAINT [PK_FoodPackage] PRIMARY KEY CLUSTERED 
    (
        [FId] ASC
    )WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServicePackage]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ServicePackage](
        [SId] [int] NOT NULL,
        [SName] [nvarchar](50) NOT NULL,
        [SPackageCost] [float] NOT NULL
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[ServicePackage] ADD  CONSTRAINT [PK_ServicePackage] PRIMARY KEY CLUSTERED 
    (
        [SId] ASC
    )WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Room]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Room](
        [RId] [int] NOT NULL,
        [Category] [nvarchar](50) NOT NULL,
        [IsBooked] [nvarchar](50) NOT NULL,
        [RoomCost] [float] NOT NULL
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[Room] ADD  CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
    (
        [RId] ASC
    )WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Booking]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Booking](
        [BId] [int] NOT NULL,
        [RId] [int] NOT NULL,
        [CheckIn] [date] NOT NULL,
        [CheckOut] [date] NOT NULL,
        [FId] [int] NOT NULL,
        [SId] [int] NOT NULL,
        [CName] [nvarchar](50) NOT NULL,
        [CAdd] [nvarchar](50) NOT NULL,
        [CPhone] [nvarchar](50) NOT NULL,
        [CNID] [nvarchar](50) NOT NULL,
        [Total] [float] NOT NULL,
        [Advance] [float] NOT NULL,
        [Remaining] [float] NOT NULL
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED 
    (
        [BId] ASC
    )WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

    ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_FoodPackage] FOREIGN KEY([FId])
    REFERENCES [dbo].[FoodPackage] ([FId])

    ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_FoodPackage]

    ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Room] FOREIGN KEY([RId])
    REFERENCES [dbo].[Room] ([RId])

    ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Room]

    ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_ServicePackage] FOREIGN KEY([SId])
    REFERENCES [dbo].[ServicePackage] ([SId])

    ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_ServicePackage]
END
GO

