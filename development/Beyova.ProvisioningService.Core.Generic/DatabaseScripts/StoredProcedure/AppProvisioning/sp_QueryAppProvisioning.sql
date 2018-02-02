IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryAppProvisioning]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryAppProvisioning]
GO

CREATE PROCEDURE [dbo].[sp_QueryAppProvisioning](
    @Key UNIQUEIDENTIFIER,
    @PlatformKey UNIQUEIDENTIFIER,
    @Name NVARCHAR(256),
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;

BEGIN    
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[SnapshotKey]
      ,[PlatformKey]
      ,[Name]
      ,[Content]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[view_AppProvisioning]';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','=',@Name,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('PlatformKey','=',CONVERT(NVARCHAR(MAX), @PlatformKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('OperatorKey','=',CONVERT(NVARCHAR(MAX), @OperatorKey),1);
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [PlatformKey],[Name] ';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END
GO

