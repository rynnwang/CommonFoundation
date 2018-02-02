IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryCentralConfiguration]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryCentralConfiguration];

GO
CREATE PROCEDURE [dbo].[sp_QueryCentralConfiguration](
    @Key [UNIQUEIDENTIFIER],
    @ProductKey [UNIQUEIDENTIFIER],
    @Name [NVARCHAR](256)
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '[dbo].[fn_ObjectIsWorkable]([State]) = 1 AND ';

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
    FROM [dbo].[view_CentralConfiguration]';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ProductKey','=',CONVERT(NVARCHAR(MAX), @ProductKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','=',@Name,1);
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [ProductKey], [Name];';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END

GO