IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryCentralConfigurationSnapshot]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryCentralConfigurationSnapshot];

GO
CREATE PROCEDURE [dbo].[sp_QueryCentralConfigurationSnapshot](
    @Key [UNIQUEIDENTIFIER],
    @SnapshotKey [UNIQUEIDENTIFIER]
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[SnapshotKey]
      ,[ProductKey]
      ,[Name]
      ,[Configuration]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[view_CentralConfigurationSnapshot]';

    IF @SnapshotKey IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('SnapshotKey','=',CONVERT(NVARCHAR(MAX), @SnapshotKey),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [CreatedStamp] DESC;';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END

GO