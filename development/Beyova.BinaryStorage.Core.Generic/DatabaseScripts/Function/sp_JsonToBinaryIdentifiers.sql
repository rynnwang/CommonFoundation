/* -----------------------------------
@Identifiers SAMPLE:
[{
    "Container": ¡°xxx",
    "Identifier": "xxx"
}]
----------------------------------- */
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
                Container NVARCHAR(MAX) '$.Container',
                Identifier UNIQUEIDENTIFIER '$.Identifier' 
            ) AS J;

    END
    RETURN;
END
GO


