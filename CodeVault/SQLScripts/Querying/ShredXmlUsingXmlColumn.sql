/*
This example assumes a table called CSO.RFCTransactionLog that contains a column called "cResultXML" which
contains XML (as nvarchar(max), not XML data type) of the following pattern.

<Function name="Y_ACI2_LIST_ORDERS">
  <ImportParameters MESSAGE_NUMBER="000" MESSAGE_TEXT="" />
  <ExportParameters CUSTOMER="0000196636" DIST_CHANNEL="03" DIVISION="02" SALES_ORG="0487" DETAIL_FLAG="X" START_DATE="20121019" END_DATE="20131022" RELEVANT_DAYS="30" UOM="KL" CARD_IND="X" CALLING_SYSTEM="ACI" />
  <Tables>
    <Table name="ORDER_HEADER">
      <Row OrderNumber="1029782791" PoNumber="" CSOOrderNumber="" OrderReason="" Planned="N" PlannedTime="000000" Currency="JPY" Truck="" ShipCondition="62" Complete="Y" CreditBlock="A" Shift="" CardId="" TripNr="00000" VehicleId="" DriverID="" NetAmount="22750000" GrossAmount="23887500" RequiredDeliveryDate="2012-10-19 00:00:00" PlannedDate="0001-01-01 00:00:00" />
      <Row OrderNumber="1029627848" PoNumber="" CSOOrderNumber="" OrderReason="" Planned="N" PlannedTime="000000" Currency="JPY" Truck="" ShipCondition="62" Complete="Y" CreditBlock="A" Shift="" CardId="" TripNr="00000" VehicleId="" DriverID="" NetAmount="60500000" GrossAmount="63525000" RequiredDeliveryDate="2012-10-22 00:00:00" PlannedDate="0001-01-01 00:00:00" />
      <Row OrderNumber="1029718654" PoNumber="" CSOOrderNumber="" OrderReason="" Planned="N" PlannedTime="000000" Currency="JPY" Truck="" ShipCondition="62" Complete="Y" CreditBlock="A" Shift="" CardId="" TripNr="00000" VehicleId="" DriverID="" NetAmount="29250000" GrossAmount="30712500" RequiredDeliveryDate="2012-10-23 00:00:00" PlannedDate="0001-01-01 00:00:00" />
    </Table>
    <Table name="ORDER_ITEMS">
      <Row OrderNumber="1029782791" Item="000010" Material="000000000000100005" Quantity="350.000" UOM="KL" NetValue="22750000" TaxAmount="1137500" Currency="JPY" NetPrice="65000" PriceUnit="1" CondUnit="M3" TankNumber="" Plant="0XQ0" GrossAmount="23887500" PaymentTerm="" />
      <Row OrderNumber="1029627848" Item="000010" Material="000000000000100003" Quantity="500.000" UOM="KL" NetValue="60500000" TaxAmount="3025000" Currency="JPY" NetPrice="121000" PriceUnit="1" CondUnit="M3" TankNumber="" Plant="0X0F" GrossAmount="63525000" PaymentTerm="" />
      <Row OrderNumber="1029718654" Item="000010" Material="000000000000100005" Quantity="450.000" UOM="KL" NetValue="29250000" TaxAmount="1462500" Currency="JPY" NetPrice="65000" PriceUnit="1" CondUnit="M3" TankNumber="" Plant="0XQ0" GrossAmount="30712500" PaymentTerm="" />
    </Table>
  </Tables>
</Function>

*/

-------------------------------------

select 
	--X.x,
	R.dtRFCSTart,
	X.x.value('(/Function/ExportParameters/@CUSTOMER)[1]', 'char(10)') as KUNNR,     -- <<<< Pull out the CUSTOMER attribute of the ExportParameters node.
	X.x.value('(/Function/ExportParameters/@DIST_CHANNEL)[1]', 'char(2)') as VTWEG,
	X.x.value('(/Function/ExportParameters/@DIVISION)[1]', 'char(2)') as SPART,
	X.x.value('(/Function/ExportParameters/@DIVISIONsasasas)[1]', 'char(2)') as DoesNotExist,

	X.x.value('(/Function/ExportParameters/@SALES_ORG)[1]', 'char(4)') as VKORG,
	X.x.value('(/Function/Tables/Table[@name="ORDER_ITEMS"]/Row/@OrderNumber)[1]', 'char(10)') as OrderNumber,   -- <<<< Find Table nodes where name="ORDER_ITEMS", then get their "Row" subnodes, then get the OrderNumber attribute of those nodes.
	X.x.value('(/Function/Tables/Table[@name="ORDER_ITEMS"]/Row/@Plant)[1]', 'char(10)') as WERKS
from
	CSO.RFCTransactionLog R
	cross apply (select cast(R.cResultXML as xml) x) X    -- <<<< Turn it into "real" XML.
where
	R.cRFCID = 'LO'
	and R.dtRFCSTart >= '2012-10-01'
	and X.x.value('(/Function/Tables/Table[@name="ORDER_ITEMS"]/Row/@Plant)[1]', 'char(10)') is not null

