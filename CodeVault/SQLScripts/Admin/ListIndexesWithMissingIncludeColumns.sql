-- Find missing include columns.
-- http://bradsruminations.blogspot.com/
-- The Case of the Key Lookup Killer

with xmlnamespaces 
(
  default 'http://schemas.microsoft.com/sqlserver/2004/07/showplan'
)
select TableName=IxDB+'.'+IxSchema+'.'+IxTable
      ,IndexName=IxIndex
      ,TableAliasInQuery=isnull(IxAlias,IxTable)
      ,ColumnsToInclude=ColList
      ,usecounts
      ,size_in_bytes
      ,objtype
      ,BatchCode
      ,QueryPlan=qp.query_plan 
from sys.dm_exec_cached_plans qs 
cross apply 
  --Get the Query Text
  sys.dm_exec_sql_text(qs.plan_handle) qt             
cross apply 
  --Get the Query Plan
  sys.dm_exec_query_plan(qs.plan_handle) qp
cross apply
  --Get the Code for the Batch in Hyperlink Form
  (select BatchCode
            =(select [processing-instruction(q)]=':'+nchar(13)+qt.text+nchar(13)
              for xml path(''),type)
  ) F_Code
cross apply
  --Find the Key Lookups in the Plan
  qp.query_plan.nodes
  (
    '//RelOp[@LogicalOp="Clustered Index Seek"]/IndexScan[@Lookup=1]'
  ) F_Lookup(LookupNode)
cross apply 
  --Get the Database,Schema,Table of the Lookup
  --Also get the Alias (if it exists) in case the table
  --  is used more than once in the query
  (select LookupDB=LookupNode.value('(./Object[1]/@Database)','sysname')
         ,LookupSchema=LookupNode.value('(./Object[1]/@Schema)','sysname')
         ,LookupTable=LookupNode.value('(./Object[1]/@Table)','sysname')
         ,LookupAlias=isnull(LookupNode.value('(./Object[1]/@Alias)','sysname'),'')
         ,ColumnCount=LookupNode.value('count(../OutputList[1]/ColumnReference)','int')
  ) F_LookupInfo
cross apply 
  --Get the Output Columns
  (select stuff(
            (select ','+ColName
             from LookupNode.nodes('(../OutputList[1]/ColumnReference)') F_Col(ColNode)
             cross apply 
               (select ColName=ColNode.value('(./@Column)','sysname')) F_ColInfo
             order by ColName
             for xml path(''),type).value('(./text())[1]','varchar(max)')
            ,1,1,'')
  ) F_ColList(ColList)
outer apply
  --Get the Parent RelOp Node, hoping that it is a Nested Loops operator.
  --Use OUTER APPLY because we may not find it
  LookupNode.nodes
  (
    '(./../../..[@PhysicalOp="Nested Loops"])'
  ) F_ParentLoop(ParentLoopNode)
outer apply
  --Get the GrandParent RelOp Node, hoping that it is a Nested Loops operator.
  --Use OUTER APPLY because we may not find it
  LookupNode.nodes
  (
    '(./../../../../..[@PhysicalOp="Nested Loops"])'
  ) F_GrandParentLoop(GrandParentLoopNode)
cross apply 
  --Get the Nested Loop Node... Could be the Parent or the GrandParent
  (select LoopNode=isnull(ParentLoopNode.query('.')
                         ,GrandParentLoopNode.query('.'))
  ) F_LoopNode
cross apply
  --Now that we (hopefully) have a Nested Loops Node, let's find a descendant
  --of that node that is an Index Seek or Index Scan and acquire its Object Information
  LoopNode.nodes
    (
      '//RelOp[@LogicalOp="Index Scan" or @LogicalOp="Index Seek"]
       /IndexScan[1]/Object[1]'
    ) F_SeekNode(SeekObjNode)
cross apply
  --Get the Database,Schema,Table and Index of the Index Seek/Scan
  --Also get the Alias (if it exists) so we can match it up with 
  --  the Lookup Table
  (select IxDB=SeekObjNode.value('(./@Database)','sysname')
         ,IxSchema=SeekObjNode.value('(./@Schema)','sysname')
         ,IxTable=SeekObjNode.value('(./@Table)','sysname')
         ,IxAlias=isnull(SeekObjNode.value('(./@Alias)','sysname'),'')
         ,IxIndex=SeekObjNode.value('(./@Index)','sysname')
  ) F_SeekInfo
where qs.cacheobjtype='Compiled Plan'
  and usecounts>=5       --Only interested in those plans used at least 5 times
  and LookupDB=IxDB          --( Lookup and IndexSeek/Scan )
  and LookupSchema=IxSchema  --(   Database,Schema,Table,  )
  and LookupTable=IxTable    --(   and [possible] Alias    )
  and LookupAlias=IxAlias    --(   must match              )
  and ColumnCount<=5     --Limit to the #columns we're willing to INCLUDE
order by TableName
        ,IndexName
        ,ColumnsToInclude