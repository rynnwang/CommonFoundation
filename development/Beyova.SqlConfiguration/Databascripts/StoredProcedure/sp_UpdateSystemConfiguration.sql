CREATE PROCEDURE [dbo].[sp_UpdateSystemConfiguration](
    @Key NVARCHAR(64),
    @Type NVARCHAR(256),
    @MinComponentVersionRequired NVARCHAR(32),
    @MaxComponentVersionLimited NVARCHAR(32),
    @Value NVARCHAR(MAX)
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @Key IS NOT NULL AND @Type IS NOT NULL AND @Value IS NOT NULL
    BEGIN
        IF EXISTS (SELECT TOP 1 1 FROM [dbo].[SystemConfiguration] AS SC
            WHERE SC.[Key] = @Key)
        BEGIN
            UPDATE [dbo].[SystemConfiguration]
                SET [Type] = @Type,
                    [MinComponentVersionRequired] = ISNULL(@MinComponentVersionRequired, [MinComponentVersionRequired]),
                    [MaxComponentVersionLimited] = ISNULL(@MaxComponentVersionLimited, [MaxComponentVersionLimited]),
                    [Value] = @Value,
                    [LastUpdatedStamp] = @NowTime
                WHERE [Key] = @Key;
        END
        ELSE
        BEGIN
            INSERT INTO [dbo].[SystemConfiguration]
           ([Key]
           ,[Type]
           ,[MinComponentVersionRequired]
           ,[MaxComponentVersionLimited]
           ,[Value]
           ,[CreatedStamp]
           ,[LastUpdatedStamp])
             VALUES
                   (@Key
                   ,@Type
                   ,@MinComponentVersionRequired
                   ,@MaxComponentVersionLimited
                   ,@Value
                   ,@NowTime
                   ,@NowTime);
        END
            
    END
END
GO


