IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ObjectCanUpdateOrDelete]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_ObjectCanUpdateOrDelete];
GO
CREATE FUNCTION [dbo].[fn_ObjectCanUpdateOrDelete](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    IF @State IS NOT NULL AND NOT ((@State & 0x1 = 0x1) OR (@State & 0x4 = 0x4))
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;
    RETURN @ReturnValue;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ObjectIsApproved]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_ObjectIsApproved];
GO
CREATE FUNCTION [dbo].[fn_ObjectIsApproved](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    IF @State IS NOT NULL AND @State & 0x1F0 = 0x110
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;
    RETURN @ReturnValue;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ObjectIsVisible]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_ObjectIsVisible];
GO
CREATE FUNCTION [dbo].[fn_ObjectIsVisible](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    IF @State IS NOT NULL AND NOT ((@State & 0x1  = 0x1) OR (@State & 0x2 = 0x2))
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;
    RETURN @ReturnValue;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ObjectIsWorkable]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_ObjectIsWorkable];
GO
CREATE FUNCTION [dbo].[fn_ObjectIsWorkable](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    IF ((@State & 0x1 = 0x1) OR (@State & 0x8 = 0x8))
        SET @ReturnValue = 0;
    ELSE
        SET @ReturnValue = 1;
    RETURN @ReturnValue;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_SetObjectDeleted]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_SetObjectDeleted];
GO
CREATE FUNCTION [dbo].[fn_SetObjectDeleted](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    IF @State IS NOT NULL
        SET @ReturnValue = (@State | 0x1);
    ELSE
        SET @ReturnValue = 0x1;
        RETURN @ReturnValue;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GenerateSqlExpression]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_GenerateSqlExpression];
GO
CREATE FUNCTION [dbo].[fn_GenerateSqlExpression](
    @ColumnName NVARCHAR(MAX),
    @Operator NVARCHAR(MAX),
    @Value NVARCHAR(MAX),
    @IsStringType BIT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Result AS NVARCHAR(MAX);
    SET @Result = N'';
    IF ISNULL(@ColumnName,N'') <> N'' AND ISNULL(@Operator, N'') <> N''
    BEGIN
        IF(@Value IS NULL)
            SET @Result = N'['+ @ColumnName + N'] IS NULL';
        ELSE
            SET @Result = N'['+ @ColumnName + N'] ' + @Operator + N' '
                + (CASE WHEN @IsStringType = 1 THEN N'N''' ELSE N'' END)
                + @Value
                + (CASE WHEN @IsStringType = 1 THEN N'''' ELSE N'' END);
    END
    RETURN @Result;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_PreventInjection]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_PreventInjection];
GO
CREATE FUNCTION [dbo].[fn_PreventInjection](
    @Value NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    IF @Value IS NOT NULL
    BEGIN
        SET @Value = REPLACE(@Value, N'''', N'''''');
        SET @Value = REPLACE(@Value, N'--', N'');
        SET @Value = REPLACE(@Value, N'/*', N'');
        SET @Value = REPLACE(@Value, N'*/', N'');
    END
    RETURN @Value;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GenerateWherePattern]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_GenerateWherePattern];
GO
CREATE FUNCTION [dbo].[fn_GenerateWherePattern](
    @ColumnName NVARCHAR(MAX),
    @Operator NVARCHAR(MAX),
    @Value NVARCHAR(MAX),
    @IsStringType BIT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Result AS NVARCHAR(MAX) = N'';
    IF @Value IS NOT NULL AND @ColumnName IS NOT NULL AND @Operator IS NOT NULL
    BEGIN
        SET @Value = [dbo].[fn_PreventInjection](@Value);
        IF LOWER(@Operator) = N'like'
        BEGIN
            SET @Value = N'%' + @Value + N'%';
            SET @IsStringType = 1;
        END
        SET @Result =  dbo.fn_GenerateSqlExpression(@ColumnName,@Operator,@Value, @IsStringType) + N' AND ';
    END
    RETURN @Result;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_JsonListToGuidTable]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_JsonListToGuidTable];
GO
CREATE FUNCTION [dbo].[fn_JsonListToGuidTable](
    @json NVARCHAR(MAX)
)
RETURNS TABLE
RETURN SELECT * FROM
OPENJSON(@json)
WITH
(
[Value] UNIQUEIDENTIFIER '$'
) AS J;
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_CombineWhereExpression]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_CombineWhereExpression];
GO
CREATE FUNCTION [dbo].[fn_CombineWhereExpression](
    @Container NVARCHAR(MAX),
    @Expression NVARCHAR(MAX),    
    @Operator NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Result AS NVARCHAR(MAX) = ISNULL(@Container, N'');
    IF @Expression IS NOT NULL AND @Expression <> N''
    BEGIN
        IF LEN(@Result) > 0
            SET @Result = @Result + N' ' + (ISNULL(@Operator, N'AND')) + N' ';
        SET @Result = @Result + N'(' + @Expression + N')';
    END
    RETURN @Result;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_JsonListToIntTable]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_JsonListToIntTable];
GO
CREATE FUNCTION [dbo].[fn_JsonListToIntTable](
    @json NVARCHAR(MAX)
)
RETURNS TABLE
RETURN SELECT * FROM
OPENJSON(@json)
WITH
(
[Value] INT '$'
) AS J;
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_JsonListToStringTable]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_JsonListToStringTable];
GO
CREATE FUNCTION [dbo].[fn_JsonListToStringTable](
    @json NVARCHAR(MAX)
)
RETURNS TABLE
RETURN SELECT * FROM
OPENJSON(@json)
WITH
(
[Value] NVARCHAR(MAX) '$'
) AS J;
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsNullOrEmptyString]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_IsNullOrEmptyString];
GO
CREATE FUNCTION [dbo].[fn_IsNullOrEmptyString](
    @String NVARCHAR(MAX)
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT = 0;
    IF @String IS NULL
        OR LTRIM(RTRIM(@String)) = N''
        SET @ReturnValue = 1;
    RETURN @ReturnValue;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GenerateJsonArrayContainsExpression]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_GenerateJsonArrayContainsExpression];
GO
CREATE FUNCTION [dbo].[fn_GenerateJsonArrayContainsExpression](
    @ColumnName NVARCHAR(MAX),
    @JsonPath NVARCHAR(MAX),
    @Value NVARCHAR(MAX),
    @IsStringType BIT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    SET @Value = [dbo].[fn_PreventInjection](@Value);
    IF ISNULL(@ColumnName,N'') <> N'' AND @Value IS NOT NULL
    BEGIN
        IF @JsonPath IS NULL OR @JsonPath = N''
            SET @JsonPath = N'$';
        RETURN 
            ((CASE WHEN @IsStringType = 1 THEN N'N''' ELSE N'' END)
            + @Value
            + (CASE WHEN @IsStringType = 1 THEN N'''' ELSE N'' END))
            + N' IN (SELECT VALUE FROM OPENJSON([' + @ColumnName + N'],N''' + @JsonPath + N''')) AND ';
    END
    RETURN N'';
END
GO


