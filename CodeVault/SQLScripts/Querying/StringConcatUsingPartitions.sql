-- See http://stackoverflow.com/questions/13639262/optimal-way-to-concatenate-aggregate-strings
;WITH Partitioned AS
(
    SELECT 
        ID,
        Name,
        ROW_NUMBER() OVER (PARTITION BY ID ORDER BY Name) AS NameNumber,
        COUNT(*) OVER (PARTITION BY ID) AS NameCount
    FROM dbo.SourceTable
),
Concatenated AS
(
    SELECT ID, CAST(Name AS nvarchar) AS FullName, Name, NameNumber, NameCount FROM Partitioned WHERE NameNumber = 1

    UNION ALL

    SELECT 
        P.ID, CAST(C.FullName + ', ' + P.Name AS nvarchar), P.Name, P.NameNumber, P.NameCount
    FROM Partitioned AS P
        INNER JOIN Concatenated AS C ON P.ID = C.ID AND P.NameNumber = C.NameNumber + 1
)
SELECT 
    ID,
    FullName
FROM Concatenated
WHERE NameNumber = NameCount
