IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_JsonToBinaryIdentifiers]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_JsonToBinaryIdentifiers];
GO
/* -----------------------------------
@Identifiers SAMPLE:
[{
    "container": "xxx",
    "identifier": "xxx"
}]
*/
CREATE FUNCTION [dbo].[fn_JsonToBinaryIdentifiers](
    @Json NVARCHAR(max)
)
RETURNS @DataTable TABLE (
    [Identifier] UNIQUEIDENTIFIER NOT NULL,
    [Container] NVARCHAR(128) NOT NULL
 )
AS
BEGIN
    IF ISJSON(@Json) > 0
    BEGIN
        INSERT INTO @DataTable([Container],[Identifier])
            SELECT * FROM 
            OPENJSON(@json) 
            WITH
            (
                Container NVARCHAR(MAX) '$.container',
                Identifier UNIQUEIDENTIFIER '$.identifier' 
            ) AS J;
    END
    RETURN;
END
GO

