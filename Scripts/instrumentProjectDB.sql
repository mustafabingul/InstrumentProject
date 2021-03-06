USE [Instrument_DB]
GO
/****** Object:  Table [dbo].[tbl_alert]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_alert](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](75) NOT NULL,
	[Status] [bit] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[StockId] [bigint] NOT NULL,
 CONSTRAINT [PK_tbl_Alert] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_marketsummary]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_marketsummary](
	[AppId] [bigint] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](100) NULL,
	[ShortName] [varchar](100) NULL,
	[Type] [varchar](100) NULL,
 CONSTRAINT [PK_tbl_marketsummary] PRIMARY KEY CLUSTERED 
(
	[AppId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_stocksummary]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_stocksummary](
	[AppId] [bigint] IDENTITY(1,1) NOT NULL,
	[ShortName] [varchar](50) NULL,
	[LongName] [varchar](100) NULL,
	[Symbol] [varchar](50) NULL,
	[Currency] [varchar](50) NULL,
	[CurrencySymbol] [varchar](50) NULL,
	[ExchangeName] [varchar](50) NULL,
	[Price] [decimal](18, 2) NULL,
	[Summary] [nvarchar](1000) NULL,
	[Recommendation] [varchar](100) NULL,
 CONSTRAINT [PK_tbl_stocksummary] PRIMARY KEY CLUSTERED 
(
	[AppId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_alert]  WITH CHECK ADD  CONSTRAINT [FK__tbl_Alert__Stock__36B12243] FOREIGN KEY([StockId])
REFERENCES [dbo].[tbl_stocksummary] ([AppId])
GO
ALTER TABLE [dbo].[tbl_alert] CHECK CONSTRAINT [FK__tbl_Alert__Stock__36B12243]
GO
/****** Object:  StoredProcedure [dbo].[sp_check_marketsummary]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_check_marketsummary]
	
	@Symbol varchar(100)
	
	

AS

Declare @ErrorMsg int


   IF NOT EXISTS (SELECT * FROM dbo.tbl_marketsummary 
                   WHERE Symbol = @Symbol
                  )
 Begin
        Set @ErrorMsg = -1
        Return(@ErrorMsg)
End
GO
/****** Object:  StoredProcedure [dbo].[sp_check_stocksummary]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[sp_check_stocksummary]
	
	@Symbol varchar(100)
	
	

AS

Declare @ErrorMsg int


   IF NOT EXISTS (SELECT * FROM dbo.tbl_stocksummary 
                   WHERE Symbol = @Symbol
                  )
 Begin
        Set @ErrorMsg = -1
        Return(@ErrorMsg)
End
GO
/****** Object:  StoredProcedure [dbo].[sp_createalert]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[sp_createalert]
@Id bigint NULL OUTPUT,
@Email varchar(75),
@Status bit,
@Price decimal(18, 2),
@StockId bigint 
AS 
Declare @ErrorCode int 
BEGIN INSERT INTO [dbo].[tbl_alert]
([Email]
,[Status]
,[Price]
,[StockId])
VALUES
(@Email
,@Status
,@Price
,@StockId
)
SET @Id = SCOPE_IDENTITY()
RETURN @@Error
END

GO
/****** Object:  StoredProcedure [dbo].[sp_getalertstatus]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[sp_getalertstatus]
	
	@Status bit
	
	


 AS
BEGIN
	
	SELECT * FROM dbo.tbl_Alert
                   WHERE Status = @Status

	Return @@Error
END

GO
/****** Object:  StoredProcedure [dbo].[sp_getall_marketsummary]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_Marketsummary]    Script Date: 17/06/2022 10:41:15 ******/
CREATE PROCEDURE [dbo].[sp_getall_marketsummary]
(
   
	@PageNumber int,
	@PageSize int
)
AS
BEGIN
    
    SELECT * 
    FROM dbo.tbl_marketsummary
  
	ORDER BY [AppId] ASC
	OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;

 

    RETURN @@Error
END

GO
/****** Object:  StoredProcedure [dbo].[sp_getall_stocksummary]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_Marketsummary]    Script Date: 17/06/2022 10:41:15 ******/
Create PROCEDURE [dbo].[sp_getall_stocksummary]
(
   
	@PageNumber int,
	@PageSize int
)
AS
BEGIN
    
    SELECT * 
    FROM dbo.tbl_stocksummary
  
	ORDER BY [AppId] ASC
	OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;

 

    RETURN @@Error
END

GO
/****** Object:  StoredProcedure [dbo].[sp_getstocksummary_id]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[sp_getstocksummary_id]
	
	@Id bigint
	
	

AS

Declare @ErrorMsg int


   IF NOT EXISTS (SELECT * FROM dbo.tbl_stocksummary 
                   WHERE AppId = @Id
                  )
 Begin
        Set @ErrorMsg = -1
        Return(@ErrorMsg)
End
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_marketsummary]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_insert_marketsummary]
	@AppId bigint NULL OUTPUT,
	@Symbol varchar(100),
	@ShortName varchar(100),
	@Type varchar(15)
	

AS

Declare @ErrorCode int  

BEGIN
   IF NOT EXISTS (SELECT * FROM dbo.tbl_marketsummary 
                   WHERE Symbol = @Symbol
                   AND ShortName = @ShortName
                   AND Type = @Type)

BEGIN

	INSERT INTO dbo.tbl_marketsummary
			   (Symbol
			   ,ShortName
			
			   ,Type
			   )
		 VALUES
			   (@Symbol
			   ,@ShortName
			   ,@Type
			   )
     SET @AppId = SCOPE_IDENTITY()
  RETURN @@Error
END
End
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_stocksummary]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_insert_stocksummary]
	@AppId bigint NULL OUTPUT,
	@Symbol varchar(50),
	@ShortName varchar(100),
	@LongName varchar(100),
	@Currency varchar(50),
	@CurrencySymbol varchar(50),
	@ExchangeName varchar(50) ,
	@Price decimal(18, 2),
	@Summary nvarchar(1000),
	@Recommendation varchar(100)
	

AS

Declare @ErrorCode int  



BEGIN

	INSERT INTO dbo.tbl_stocksummary
           (ShortName
           ,LongName
           ,Symbol
           ,Currency
           ,CurrencySymbol
           ,ExchangeName
           ,Price
           ,Summary
           ,Recommendation)
		 VALUES
			   (@ShortName
           ,@LongName
           ,@Symbol
           ,@Currency
           ,@CurrencySymbol
           ,@ExchangeName
           ,@Price
           ,@Summary
           ,@Recommendation
			   )
     SET @AppId = SCOPE_IDENTITY()
  RETURN @@Error
END

GO
/****** Object:  StoredProcedure [dbo].[sp_update_price]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_Marketsummary]    Script Date: 17/06/2022 10:41:15 ******/
CREATE PROCEDURE [dbo].[sp_update_price]
(
   
	@Price int,
	@Id bigint
)
AS
BEGIN
    
    Update tbl_stocksummary
  
	set 
	Price = @Price 
    Where AppId = @Id 

 

    RETURN @@Error
END

GO
/****** Object:  StoredProcedure [dbo].[sp_updatealertby_id]    Script Date: 19/06/2022 15:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_updatealertby_id]
(



@Status int,
@Id bigint
)
AS
BEGIN



Update tbl_Alert



set
Status = @Status
Where Id = @Id







RETURN @@Error
END
GO
