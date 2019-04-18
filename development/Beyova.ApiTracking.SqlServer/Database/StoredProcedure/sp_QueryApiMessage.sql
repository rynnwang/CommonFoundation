CREATE PROCEDURE [dbo].[sp_QueryApiMessage](
    @Key [UNIQUEIDENTIFIER],
    @Count [INT],
    @ServiceIdentifier [NVARCHAR](64),
    @ServerIdentifier [NVARCHAR](128),
    @Category [NVARCHAR](256),
    @Message [NVARCHAR](MAX),
    @FromStamp [DATETIME],
    @ToStamp [DATETIME]
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = N'';

    IF @Count IS NULL OR @Count < 1 OR @Count > 500
    BEGIN
        SET @Count = 200;
    END

    SET @SqlStatement = N'SELECT TOP ' + CONVERT(NVARCHAR(MAX),@Count) + N' [Key]
      ,[Category]
      ,[Message]
      ,[ServiceIdentifier]
      ,[ServerIdentifier]
      ,[CreatedStamp]
    FROM [dbo].[ApiMessage]';

    IF @Key IS NOT NULL
    BEGIN
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'Key', N'=', CONVERT(NVARCHAR(MAX), @Key), 1);
    END
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'ServiceIdentifier', N'=', @ServiceIdentifier, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'ServerIdentifier', N'=', @ServerIdentifier, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'Category', N'=', @Category, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'Message', N'LIKE', @Message, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'CreatedStamp', N'>=', CONVERT(NVARCHAR(MAX), @FromStamp, 121), 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'CreatedStamp', N'<', CONVERT(NVARCHAR(MAX), @ToStamp, 121), 1);
    END
    IF(@WhereStatement <> N'')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + N' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + N' ORDER BY [CreatedStamp] DESC';
    END
    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement; 
END
GO
