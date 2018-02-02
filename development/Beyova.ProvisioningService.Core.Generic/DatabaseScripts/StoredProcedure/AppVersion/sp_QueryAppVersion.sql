IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryAppVersion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryAppVersion]
GO

CREATE PROCEDURE [dbo].[sp_QueryAppVersion](
    @Key UNIQUEIDENTIFIER,
    @PlatformKey UNIQUEIDENTIFIER,
    @Platform INT,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;

BEGIN    
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[Name]
      ,[PlatformKey]
      ,[LatestBuild]
      ,[LatestVersion]
      ,[MinRequiredBuild]
      ,[Note]
      ,[AppServiceStatus]
      ,[Url]
      ,[BundleId]
      ,[MinOSVersion]
      ,[PlatformType]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[view_AppVersion]';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('PlatformKey','=',CONVERT(NVARCHAR(MAX), @PlatformKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('PlatformType','=',@Platform,0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('OperatorKey','=',CONVERT(NVARCHAR(MAX), @OperatorKey),1);
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [Name],[PlatformType] ';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END
GO

