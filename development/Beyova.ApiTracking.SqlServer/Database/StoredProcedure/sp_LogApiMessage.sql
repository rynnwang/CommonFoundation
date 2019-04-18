CREATE PROCEDURE [dbo].[sp_LogApiMessage](
    @ServerIdentifier [NVARCHAR](128),
    @ServiceIdentifier [NVARCHAR](64),
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
                ,[ServerIdentifier]
                ,[ServiceIdentifier]
                ,[CreatedStamp])
            VALUES
                (@Key
                ,@Category
                ,@Message
                ,@ServerIdentifier
                ,@ServiceIdentifier
                ,GETUTCDATE());

        SELECT @Key;
    END
END
GO

