CREATE PROCEDURE [dbo].[sp_LogException](
    @MajorCode INT,
    @MinorCode NVARCHAR(64),
    @ServiceIdentifier [NVARCHAR](64),
    @ServerIdentifier [NVARCHAR](128),
    @HttpMethod  [NVARCHAR](16),
    @Path  [NVARCHAR](256),
    @RawUrl [NVARCHAR](512),
    @Message [NVARCHAR](512),
    @TargetSite [NVARCHAR](MAX),
    @StackTrace [NVARCHAR](MAX),
    @ExceptionType [NVARCHAR](128),
    @Source [NVARCHAR](512),
    @EventKey [UNIQUEIDENTIFIER],
    @OperatorCredential [NVARCHAR](MAX),
    @InnerException [NVARCHAR](MAX) ,
    @Data [NVARCHAR](MAX),
    @Scene [NVARCHAR](MAX),
    @CreatedStamp [DATETIME]
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
           ,[HttpMethod]
           ,[Path]
           ,[RawUrl]
           ,[Message]
           ,[TargetSite]
           ,[StackTrace]
           ,[ExceptionType]
           ,[Source]
           ,[EventKey]
           ,[OperatorCredential]
           ,[InnerException]
           ,[Data]
           ,[Scene]
           ,[CreatedStamp])
     VALUES
           (@Key
           ,@MajorCode
           ,@MinorCode
           ,@ServiceIdentifier
           ,@ServerIdentifier
           ,@HttpMethod
           ,@Path
           ,@RawUrl
           ,@Message
           ,@TargetSite
           ,@StackTrace
           ,@ExceptionType
           ,@Source
           ,@EventKey
           ,@OperatorCredential
           ,@InnerException
           ,@Data
           ,@Scene
           ,ISNULL(@CreatedStamp, GETUTCDATE()));

    SELECT @Key;
END
GO


