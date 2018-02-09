
CREATE PROCEDURE [dbo].[sp_GetBinaryStorageMetaDataByIdentifiers](
    @Identifiers NVARCHAR(MAX)
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Identifiers IS NOT NULL
    BEGIN
        SELECT BSMD.[Identifier]
              ,BSMD.[Container]
              ,BSMD.[Name]
              ,BSMD.[Mime]
              ,BSMD.[Hash]
              ,BSMD.[Length]
              ,BSMD.[Height]
              ,BSMD.[Width]
              ,BSMD.[Duration]
              ,NULL AS [OwnerKey]
              ,BSMD.[CreatedStamp]
              ,BSMD.[LastUpdatedStamp]
              ,BSMD.[CreatedBy]
              ,BSMD.[LastUpdatedBy]
              ,BSMD.[State]
            FROM [dbo].[BinaryStorageMetaData] AS BSMD
                JOIN [dbo].[fn_JsonToBinaryIdentifiers](@Identifiers) AS IDTABLE
                    ON BSMD.[Identifier] = IDTABLE.[Identifier] AND (IDTABLE.[Container] IS NULL OR IDTABLE.[Container] = '' OR BSMD.[Container] = IDTABLE.[Container])
                WHERE [dbo].[fn_ObjectIsWorkable](BSMD.[State]) = 1;

    END
END
GO


