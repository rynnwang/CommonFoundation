CREATE PROCEDURE [dbo].[sp_CreateBinaryStorageMetaData](
    @Container NVARCHAR(128),
    @Identifier UNIQUEIDENTIFIER,
    @Name NVARCHAR(512),
    @Mime VARCHAR(64),
    @Height INT,
    @Width INT,
    @Duration INT,
    @KVMeta NVARCHAR(MAX),
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @ExistedState AS INT;
DECLARE @ExistedOwnerKey AS UNIQUEIDENTIFIER;
BEGIN
    IF @Container IS NOT NULL AND @OperatorKey IS NOT NULL
    BEGIN
        IF @Identifier IS NOT NULL AND EXISTS (SELECT TOP 1 * FROM [dbo].[BinaryStorageMetaData] WHERE [Identifier] = @Identifier)
        BEGIN
            EXEC [dbo].[sp_ThrowException]
                @Name = N'sp_CreateBinaryStorageMetaData',
                @Code = 409,
                @Reason = N'',
                @Message = N'Binary in same identifier is already existed.';
            RETURN;
        END
        ELSE
        BEGIN
            SET @Identifier = NEWID();
        END

        INSERT INTO [dbo].[BinaryStorageMetaData]
               ([Identifier]
               ,[Container]
               ,[Name]
               ,[Mime]
               ,[Hash]
               ,[Length]
               ,[Height]
               ,[Width]
               ,[Duration]
               ,[KVMeta]
               ,[CreatedStamp]
               ,[CreatedBy]
               ,[LastUpdatedStamp]
               ,[LastUpdatedBy]
               ,[State])
         VALUES
               (@Identifier
               ,@Container
               ,ISNULL(@Name, CONVERT(VARCHAR(512),@Identifier))
               ,ISNULL(@Mime, N'application/octet-stream')
               ,NULL
               ,NULL
               ,@Height
               ,@Width
               ,@Duration
               ,@KVMeta
               ,@NowTime
               ,@OperatorKey
               ,@NowTime
               ,@OperatorKey
               ,1 -- Commit pending: 1
        );

        SELECT TOP 1 [Identifier]
          ,[Container]
          ,[Name]
          ,[Mime]
          ,[Hash]
          ,[Length]
          ,[Height]
          ,[Width]
          ,[Duration]
          ,[KVMeta]
          ,[CreatedBy] AS [OwnerKey]
          ,[CreatedStamp]
          ,[CreatedBy]
          ,[LastUpdatedStamp]
          ,[LastUpdatedBy]
          ,[State]
        FROM [dbo].[BinaryStorageMetaData]
        WHERE [Identifier] = @Identifier;
    END
END
GO


