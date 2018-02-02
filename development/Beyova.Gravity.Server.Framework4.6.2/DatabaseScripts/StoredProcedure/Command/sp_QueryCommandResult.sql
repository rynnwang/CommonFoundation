IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryCommandResult]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryCommandResult];

GO
CREATE PROCEDURE [dbo].[sp_QueryCommandResult](
    @Key [UNIQUEIDENTIFIER],
    @RequestKey [UNIQUEIDENTIFIER],
    @ProductKey [UNIQUEIDENTIFIER],
    @ClientKey [UNIQUEIDENTIFIER]
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[ClientKey]
      ,[RequestKey]
      ,[Content]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
    FROM [dbo].[CommandResult]';

    IF @Key IS NOT NULL
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    END
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('RequestKey','=',CONVERT(NVARCHAR(MAX), @RequestKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ProductKey','=',CONVERT(NVARCHAR(MAX), @ProductKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ClientKey','=',CONVERT(NVARCHAR(MAX), @ClientKey),1);
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