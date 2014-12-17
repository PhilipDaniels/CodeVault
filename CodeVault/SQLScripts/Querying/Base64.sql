
-- Encode the string "TestData" in Base64 to get "VGVzdERhdGE="
SELECT
    CAST(N'' AS XML).value('xs:base64Binary(xs:hexBinary(sql:column("bin")))', 'NVARCHAR(MAX)') Base64Encoding
FROM (
    SELECT CAST(N'TestData' AS VARBINARY(MAX)) AS bin
) AS bin_sql_server_temp;

-- Decode the Base64-encoded string "VGVzdERhdGE=" to get back "TestData"
SELECT 
    CAST(CAST(N'' AS XML).value('xs:base64Binary("VABlAHMAdABEAGEAdABhAA==")', 'VARBINARY(MAX)') AS NVARCHAR(MAX)) ASCIIEncoding;


-- Encodes the column 'report_text' as Base 64.
UPDATE migration.qguardport.qg_report_data_ss SET
	raw_form_base64 = cast(N'' as xml).value('xs:base64Binary(xs:hexBinary(sql:column("report_text")))', 'varchar(max)')
