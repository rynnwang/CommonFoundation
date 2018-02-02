IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryAppPlatform]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryAppPlatform]
GO

CREATE PROCEDURE [dbo].[sp_QueryAppPlatform](
    @Key UNIQUEIDENTIFIER,
    @Name NVARCHAR(256),
	@PlatformType INT,
	@BundleId [NVARCHAR](256),
	@Url [NVARCHAR](256)
)
AS
SET NOCOUNT ON;

BEGIN    
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[Name]
      ,[PlatformType]
      ,[BundleId]
      ,[Url]
      ,[MinOSVersion]
      ,[Description]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[AppPlatform]';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','LIKE',@Name,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('PlatformType','=',@PlatformType,0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('BundleId','LIKE',@BundleId,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Url','=',@Url,1);
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

