IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryHeartbeat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryHeartbeat];

GO
CREATE PROCEDURE [dbo].[sp_QueryHeartbeat](
    @ProductKey [UNIQUEIDENTIFIER],
    @ClientKey [UNIQUEIDENTIFIER],
    @FromStamp [DATETIME],
    @ToStamp [DATETIME]
)
AS
BEGIN
    SET @ToStamp = ISNULL(@ToStamp, GETUTCDATE());

    -- Keep duration is no longer than 14 days.
    IF(@FromStamp IS NULL OR DATEDIFF(mi,@FromStamp,@ToStamp) > 20160)
        SET @FromStamp = DATEADD(mi,-20160,@ToStamp);

    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[HostName]
      ,[IpAddress]
      ,[ProductKey]
      ,[ClientKey]
      ,[ConfigurationName]
      ,[CpuUsage]
      ,[MemoryUsage]
      ,[GCMemory]
      ,[CreatedStamp]
    FROM [dbo].[view_Heartbeat]';

    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ProductKey','=',CONVERT(NVARCHAR(MAX), @ProductKey),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ClientKey','=',CONVERT(NVARCHAR(MAX), @ClientKey),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('CreatedStamp','>=',CONVERT(NVARCHAR(MAX), @FromStamp, 121),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('CreatedStamp','<',CONVERT(NVARCHAR(MAX), @ToStamp, 121),1);

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [CreatedStamp];';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END

GO