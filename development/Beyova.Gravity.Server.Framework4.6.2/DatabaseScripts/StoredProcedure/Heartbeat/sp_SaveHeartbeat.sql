IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SaveHeartbeat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SaveHeartbeat];

GO
CREATE PROCEDURE [dbo].[sp_SaveHeartbeat](
    @ProductKey [UNIQUEIDENTIFIER],
    @HostName [NVARCHAR](128),
    @ServerName [NVARCHAR](128),
    @IpAddress [NVARCHAR](128) ,
    @ConfigurationName [NVARCHAR](256) ,
    @CpuUsage [FLOAT], -- %, @0.00, 1.00]
    @MemoryUsage [DECIMAL](18,0), -- KB 
    @TotalMemory [DECIMAL](18,0) -- KB
)
AS
DECLARE @ClientKey AS UNIQUEIDENTIFIER;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @HeartbeatKey AS UNIQUEIDENTIFIER;
BEGIN
    IF @ProductKey IS NOT NULL
    BEGIN
        SELECT TOP 1 @ClientKey = [Key]
            FROM [dbo].[ProductClient]
            WHERE ISNULL([HostName],'') = ISNULL(@HostName, '')
                AND ISNULL([IpAddress],'') = ISNULL(@IpAddress, '')
                AND [ProductKey] = @ProductKey;

        IF @ClientKey IS NULL
        BEGIN
            SET @ClientKey = NEWID();
            INSERT INTO [dbo].[ProductClient]
                ([Key]
                ,[HostName]
                ,[ServerName]
                ,[IpAddress]
                ,[ConfigurationName]
                ,[ProductKey]
                ,[LastHeartbeatStamp]
                ,[CreatedStamp]
                ,[LastUpdatedStamp]
                ,[State])
            VALUES
                (@ClientKey
                ,@HostName
                ,@ServerName
                ,@IpAddress
                ,@ConfigurationName
                ,@ProductKey
                ,@NowTime
                ,@NowTime
                ,@NowTime
                ,0) ;
        END

        SET @HeartbeatKey = NEWID();
        INSERT INTO [dbo].[Heartbeat]
            ([Key]
            ,[ClientKey]
            ,[CpuUsage]
            ,[MemoryUsage]
            ,[TotalMemory]
            ,[ConfigurationName]
            ,[CreatedStamp])
        VALUES
            (@HeartbeatKey
            ,@ClientKey
            ,@CpuUsage
            ,@MemoryUsage
            ,@TotalMemory
            ,@ConfigurationName
            ,@NowTime);
    END

    SELECT @ClientKey;    
END

GO