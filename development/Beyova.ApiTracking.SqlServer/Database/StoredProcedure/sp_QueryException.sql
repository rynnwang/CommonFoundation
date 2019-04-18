IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryException]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryException]
GO

CREATE PROCEDURE [dbo].[sp_QueryException](
    @Key [UNIQUEIDENTIFIER],
    @MajorCode INT,
    @MinorCode NVARCHAR(64),
    @ServiceIdentifier [NVARCHAR](64),
    @ServerIdentifier [NVARCHAR](128),
    @HttpMethod  [NVARCHAR](16),
    @Path  [NVARCHAR](256),
    @RawUrl [NVARCHAR](512),
    @ExceptionType [NVARCHAR](128),
    @EventKey [UNIQUEIDENTIFIER],
    @Keyword [NVARCHAR](MAX),
    @OperatorCredential [NVARCHAR](MAX),
    @FromStamp [DATETIME],
    @ToStamp [DATETIME],
    @Count [INT]
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
      ,[MajorCode]
      ,[MinorCode]
      ,[ServiceIdentifier]
      ,[ServerIdentifier]
      ,[HttpMethod]
      ,[Path]
      ,[RawUrl]
      ,[Message]
      ,[TargetSite]
      ,[StackTrace]
      ,[ExceptionType]
      ,[Source]
      ,[EventKey]
      ,[OperatorCredential]
      ,[InnerException]
      ,[Data]
      ,[Scene]
      ,[CreatedStamp]
    FROM [dbo].[ExceptionInfo]';

    IF @Key IS NOT NULL
    BEGIN
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'Key', N'=', CONVERT(NVARCHAR(MAX), @Key), 1);
    END
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'MajorCode', N'=', CONVERT(NVARCHAR(MAX), @MajorCode), 0);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'MinorCode', N'=', @MinorCode, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'ServiceIdentifier', N'=', @ServiceIdentifier, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'ServerIdentifier', N'=', @ServerIdentifier, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'HttpMethod', N'=', @HttpMethod, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'Path', N'=', @Path, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'RawUrl', N'LIKE', @RawUrl, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'ExceptionType', N'=', @ExceptionType, 1);
        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'EventKey', N'=', CONVERT(NVARCHAR(MAX), @EventKey), 1);

        SET @Keyword = [dbo].[fn_PreventInjection](@Keyword);
        IF @Keyword IS NOT NULL AND @Keyword <> N''
        BEGIN
            SET @WhereStatement = @WhereStatement + N'([Message] LIKE N''%' + @Keyword + '%'' OR [StackTrace] LIKE N''%' + @Keyword + '%'' OR [TargetSite] LIKE N''%' + @Keyword + '%'' OR [Source] LIKE N''%' + @Keyword + '%'' OR [InnerException] LIKE N''%' + @Keyword + '%'' OR [Data] LIKE N''%' + @Keyword + '%'' OR [Scene] LIKE N''%' + @Keyword + '%'')';
        END

        SET @WhereStatement = @WhereStatement + [dbo].[fn_GenerateWherePattern](N'OperatorCredential', N'LIKE', @OperatorCredential, 1);
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
