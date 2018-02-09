CREATE PROCEDURE [dbo].[sp_GetUserInfoByKeys](
    @Keys NVARCHAR(MAX)
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Keys IS NOT NULL
    BEGIN
        SELECT U.[Key]
      ,U.[UserId]
      ,U.[DisplayName]
      ,U.[Gender]
      ,U.[EnglishFirstName]
      ,U.[EnglishMiddleName]
      ,U.[EnglishLastName]
      ,U.[ChineseFirstName]
      ,U.[ChineseLastName]
      ,U.[Email]
      ,U.[AvatarKey]
      ,U.[AvatarUrl]
      ,U.[Container]
      ,U.[Identifier]
      ,U.[FunctionalRole]
      ,U.[Language]
      ,U.[TimeZone]
      ,U.[GroupKey]
      ,U.[MarketRegion]
      ,U.[CurrentBookKey]
      ,U.[CreatedStamp]
      ,U.[LastUpdatedStamp]
      ,U.[CreatedBy]
      ,U.[LastUpdatedBy]
      ,U.[State]
        FROM [dbo].[view_UserInfo] AS U
        JOIN [dbo].[fn_JsonListToGuidTable](@Keys) AS IDTABLE
            ON U.[Key] = IDTABLE.[Value]
        WHERE [dbo].[fn_ObjectIsWorkable](U.[State]) = 1;

    END
END
GO


