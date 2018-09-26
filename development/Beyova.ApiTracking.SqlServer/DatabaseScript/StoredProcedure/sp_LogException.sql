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


