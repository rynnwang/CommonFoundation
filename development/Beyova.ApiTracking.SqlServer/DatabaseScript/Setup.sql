IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApiEvent]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[ApiEvent];
END
GO

CREATE TABLE [dbo].[ApiEvent](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [ServiceIdentifier] [NVARCHAR](128) NULL,
    [ServerIdentifier] [NVARCHAR](128) NULL,
    [ServerHost] [NVARCHAR](128) NULL,
    [RawUrl] [NVARCHAR](512) NULL,
    [EntryStamp] [DATETIME] NULL,
    [ExitStamp] [DATETIME] NULL,
    [ContentLength] [BIGINT] NULL,
    [Duration] [float] NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [State] [INT] NOT NULL DEFAULT 0,
CONSTRAINT [PK_ApiEvent_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_ApiEvent] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApiMessage]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[ApiMessage];
END
GO

CREATE TABLE [dbo].[ApiMessage](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [Category] [NVARCHAR](256) NULL,
    [Message] [NVARCHAR](MAX) NOT NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
CONSTRAINT [PK_ApiMessage_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_ApiMessage] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExceptionInfo]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[ExceptionInfo];
END
GO

CREATE TABLE [dbo].[ExceptionInfo](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [MajorCode] INT NULL,
    [MinorCode] NVARCHAR(64) NULL,
    [ServiceIdentifier] [NVARCHAR](64) NULL,
    [ServerIdentifier] [NVARCHAR](128) NULL,
    [ServerHost] [NVARCHAR](128) NULL,
    [RawUrl] [NVARCHAR](512) NULL,
    [Message] [NVARCHAR](512) NULL,
    [TargetSite] [NVARCHAR](MAX) NULL,
    [StackTrace] [NVARCHAR](MAX) NULL,
    [ExceptionType] [NVARCHAR](128) NULL,
    [Level] [INT] NULL,
    [Source] [NVARCHAR](512) NULL,
    [EventKey] [UNIQUEIDENTIFIER] NULL,
    [OperatorCredential] [NVARCHAR](MAX) NULL,
    [InnerException] [NVARCHAR](MAX) NULL,
    [Data] [NVARCHAR](MAX)  NULL,
    [Scene] [NVARCHAR](MAX) NULL,
    [Hint] [NVARCHAR](MAX) NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
CONSTRAINT [PK_ExceptionInfo_Key] PRIMARY KEY NONCLUSTERED
(
[Key] ASC
),
CONSTRAINT [CIX_ExceptionInfo] UNIQUE CLUSTERED
(
[RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON));
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_LogApiEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_LogApiEvent]
GO

CREATE PROCEDURE [dbo].[sp_LogApiEvent](
    @Key [UNIQUEIDENTIFIER],
    @PartnerKey [UNIQUEIDENTIFIER],
    @UserKey [UNIQUEIDENTIFIER],
    @AuthorizationToken [NVARCHAR](512),
    @IsUsed BIT,
    @StartIndex INT,
    @Count INT
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX);

   SET @WhereStatement = '[dbo].[fn_ObjectIsVisible]([State]) = 1 AND ';

    IF @Count IS NULL OR @Count < 1
        SET @Count = 100;

    IF @StartIndex IS NULL OR @StartIndex < 0
        SET @StartIndex = 0;

    SET @SqlStatement = 'SELECT TOP ' +  CONVERT(NVARCHAR(MAX), @Count) + ' [Key]
      ,[PartnerKey]
      ,[ClientRequestId]
      ,[AuthorizationToken]
      ,[UserKey]
      ,[ExpiredStamp]
      ,[UsedStamp]
      ,[CallbackUrl]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
    FROM [dbo].[view_SSOAuthorization]';

    IF @Key IS NOT NULL
        SET @WhereStatement = '[Key] = ''' + CONVERT(NVARCHAR(MAX), @Key) + ''' AND ';
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('PartnerKey','=',CONVERT(NVARCHAR(MAX), @PartnerKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('UserKey','=',CONVERT(NVARCHAR(MAX), @UserKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('AuthorizationToken','=',@AuthorizationToken,1);

        IF @IsUsed IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + '[UsedStamp] IS ' + (CASE WHEN @IsUsed = 1 THEN 'NOT ' ELSE '' END) + 'NULL AND ';
        END
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
    END

    SET  @SqlStatement = @SqlStatement +' ORDER BY [CreatedStamp] DESC OFFSET (' + CONVERT(NVARCHAR(MAX), @StartIndex) + ') ROW FETCH NEXT ' + CONVERT(NVARCHAR(MAX), @Count) + ' ROWS ONLY;';

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
   
END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_LogApiMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_LogApiMessage]
GO

CREATE PROCEDURE [dbo].[sp_LogApiMessage](
    @Category [NVARCHAR](256),
    @Message [NVARCHAR](MAX)
)
AS
DECLARE @Key AS UNIQUEIDENTIFIER;
BEGIN
    IF @Message IS NOT NULL AND @Message <> ''
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[ApiMessage]
                ([Key]
                ,[Category]
                ,[Message]
                ,[CreatedStamp])
            VALUES
                (@Key
                ,@Category
                ,@Message
                ,GETUTCDATE());

        SELECT @Key;
    END
   
END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_LogException]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_LogException]
GO

CREATE PROCEDURE [dbo].[sp_LogException](
    @MajorCode INT,
    @MinorCode NVARCHAR(64),
    @ServiceIdentifier [NVARCHAR](64),
    @ServerIdentifier [NVARCHAR](128),
    @ServerHost [NVARCHAR](128),
    @RawUrl [NVARCHAR](512),
    @Message [NVARCHAR](512),
    @TargetSite [NVARCHAR](MAX),
    @StackTrace [NVARCHAR](MAX),
    @ExceptionType [NVARCHAR](128),
    @Level [INT],
    @Source [NVARCHAR](512),
    @EventKey [UNIQUEIDENTIFIER],
    @OperatorCredential [NVARCHAR](MAX),
    @InnerException [NVARCHAR](MAX) ,
    @Data [NVARCHAR](MAX),
    @Scene [NVARCHAR](MAX),
    @Hint [NVARCHAR](MAX)
)
AS
DECLARE @Key AS UNIQUEIDENTIFIER = NEWID();
BEGIN
    INSERT INTO [dbo].[ExceptionInfo]
           ([Key]
           ,[MajorCode]
           ,[MinorCode]
           ,[ServiceIdentifier]
           ,[ServerIdentifier]
           ,[ServerHost]
           ,[RawUrl]
           ,[Message]
           ,[TargetSite]
           ,[StackTrace]
           ,[ExceptionType]
           ,[Level]
           ,[Source]
           ,[EventKey]
           ,[OperatorCredential]
           ,[InnerException]
           ,[Data]
           ,[Scene]
           ,[Hint])
     VALUES
           (@Key
           ,@MajorCode
           ,@MinorCode
           ,@ServiceIdentifier
           ,@ServerIdentifier
           ,@ServerHost
           ,@RawUrl
           ,@Message
           ,@TargetSite
           ,@StackTrace
           ,@ExceptionType
           ,@Level
           ,@Source
           ,@EventKey
           ,@OperatorCredential
           ,@InnerException
           ,@Data
           ,@Scene
           ,@Hint);

    SELECT @Key;
END
GO




