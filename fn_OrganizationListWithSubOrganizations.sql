CREATE FUNCTION [dbo].[fn_OrganizationListWithSubOrganizations]
		(
		)
RETURNS @OrganizationResult TABLE
(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    OrganizationId INT,
    Name NVARCHAR(500),
    ParentOrganizationId INT,
    Degree INT,
    Parents NVARCHAR(MAX),
    SubOrganizations NVARCHAR(MAX)
)
AS
BEGIN
    WITH OrganizationCTE AS (
        SELECT Id, Name, ParentId, 0 AS Degree, CONVERT(nvarchar(MAX), Id) AS Parents
        FROM Organizations
        WHERE ParentId IS NULL
        UNION ALL
        SELECT t.Id, t.Name, t.ParentId, cte.Degree + 1, cte.Parents + ',' + CONVERT(nvarchar(MAX), t.Id) AS Parents
        FROM Organizations t
        INNER JOIN OrganizationCTE cte ON cte.Id = t.ParentId
    )
    INSERT INTO @OrganizationResult (OrganizationId, Name, ParentOrganizationId, Degree, Parents, SubOrganizations)
    SELECT 
        Id, 
        Name, 
        ParentId, 
        Degree, 
        Parents,
		convert(nvarchar(max),Id)+','+isnull( STUFF((
            SELECT ',' + CONVERT(nvarchar(MAX), o.Id)
            FROM OrganizationCTE o
            WHERE o.Parents LIKE cte.Parents + ',%'
              AND (o.Id <> cte.ParentId or cte.ParentId is null)
            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''),'') AS SubOrganizations
    FROM OrganizationCTE cte
	 OPTION (MAXRECURSION 0);
    RETURN;
END

		