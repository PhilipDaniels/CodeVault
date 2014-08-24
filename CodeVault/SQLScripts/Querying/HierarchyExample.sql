select top 10 * from ASR.ASR.SpecialSale
sp_help 'asr.specialsale'

select C.HID.ToString(), C.* from ASR.ASR.Cluster C

insert into ASR.Cluster(HID, Name) select HierarchyId::GetRoot(), 'Global'

declare @Parent hierarchyId = (select HID from ASR.Cluster where Name = 'Global')
select @Parent 
select HierarchyId::GetRoot().GetDescendant(@Parent, null) from ASR.Cluster where HID = HierarchyId::GetRoot()

insert into ASR.Cluster(HID, Name) 
values
	(
	(select HierarchyId::GetRoot().GetDescendant(@Parent, null) from ASR.Cluster where HID = hierarchyId::GetRoot())
	,'North America'
	)


-- =========================
select E.org_chart_path.ToString() as PathAsString, E.* from Employees E
select C.HID.ToString(), C.* from ASR.ASR.Cluster C order by HID
--delete ASR.Cluster where Id = 21
declare @root HIERARCHYID = (SELECT hierarchyid::GetRoot());
declare @hid hierarchyid, @hid1 hierarchyid, @hid2 hierarchyid, @parent hierarchyid, @lastChild hierarchyid;
--select @root

--INSERT INTO ASR.Cluster(Name, HID) VALUES ('Europe', @root.GetDescendant(null, null))
select @parent = C.HID from ASR.Cluster C where C.Name = 'Europe'
select @lastChild = max(C.HID) from ASR.Cluster C where HID.GetAncestor(1) = @Parent
--select @lastChild 

INSERT INTO ASR.Cluster(Name, HID) VALUES ('Cyprus', @parent.GetDescendant(@lastChild, null))	-- insert after. lastChild can be null.
select C.HID.ToString(), C.* from ASR.ASR.Cluster C
select C.HID.ToString(), C.* from ASR.ASR.Cluster C order by 1



--INSERT INTO ASR.Cluster(Name, HID) VALUES ('Asia', @root.GetDescendant(null, @hid))	-- insert before ("to the left")
--INSERT INTO ASR.Cluster(Name, HID) VALUES ('Asia', @root.GetDescendant(@hid, null))	-- insert after ("to the right")
--INSERT INTO ASR.Cluster(Name, HID) VALUES ('Asia Minor', @root.GetDescendant(@hid1, @hid2))	-- insert between 





