IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsRubricFromCourseNode]'))
DROP FUNCTION [dbo].[fn_IsRubricFromCourseNode]
GO

CREATE FUNCTION [dbo].[fn_IsRubricFromCourseNode](
    @Code VARCHAR(64)
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
	DECLARE @Index AS INT;
    IF @Code IS NOT NULL 
	  BEGIN
	 SET @Index=CHARINDEX('-',@Code);
		IF @Index > 0
			 SET @ReturnValue = 1;
	   ELSE 
			SET @ReturnValue=0;
	  END
    ELSE
        SET @ReturnValue =  0;

    RETURN @ReturnValue;
END
GO