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
                ,BSMD.[KVMeta]
                ,NULL AS [OwnerKey]
                -- CUSTOMIZED COLUMNS START 
                -- You can put any additonal field as customized columns here.
                -- CUSTOMIZED COLUMNS END 
                ,BSMD.[CreatedStamp]
                ,BSMD.[LastUpdatedStamp]
                ,BSMD.[CreatedBy]
                ,BSMD.[LastUpdatedBy]
                ,BSMD.[State]
            FROM [dbo].[BinaryStorageMetaData] AS BSMD
                JOIN OPENJSON(@Identifiers) WITH(
                    [Identifier] UNIQUEIDENTIFIER '$.identifier',
                    [Container] NVARCHAR(128) '$.container'
                    ) 
                    AS IDTABLE
                    ON BSMD.[Identifier] = IDTABLE.[Identifier] AND (IDTABLE.[Container] IS NULL OR IDTABLE.[Container] = '' OR BSMD.[Container] = IDTABLE.[Container])
                WHERE BSMD.[State] = 2;

    END
END

GO


