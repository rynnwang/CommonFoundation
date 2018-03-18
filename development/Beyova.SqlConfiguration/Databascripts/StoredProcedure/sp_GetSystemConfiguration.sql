CREATE PROCEDURE [dbo].[sp_GetSystemConfiguration](
    @Key NVARCHAR(64)
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Key IS NOT NULL
    BEGIN
        SELECT TOP 1 [Key]
          ,[Type]
          ,[MinComponentVersionRequired]
          ,[MaxComponentVersionLimited]
          ,[Value]
          ,[CreatedStamp]
          ,[LastUpdatedStamp]
        FROM [dbo].[SystemConfiguration] 
        WHERE [Key] = @Key;
    END
    ELSE
    BEGIN
        SELECT [Key]
          ,[Type]
          ,[MinComponentVersionRequired]
          ,[MaxComponentVersionLimited]
          ,[Value]
          ,[CreatedStamp]
          ,[LastUpdatedStamp]
        FROM [dbo].[SystemConfiguration];
    END
END
GO


