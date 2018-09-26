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


