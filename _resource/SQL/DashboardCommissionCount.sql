use pos

select sum(CASE when a.TotalBranch = a.Captured then 1 else 0 end) as [Verify],
count(*) as Total
from
(
	select day(dr.ReportDate) as [Day],
	count(*) as TotalBranch,
	sum(case when isnull(dr.Captured,0) = 1 then 1 else 0 end) as Captured
	from tb_daily_revenue as dr (nolock)
	inner join tb_Branch as b with (nolock)
	on dr.BranchID = b.BranchID
	where year(dr.ReportDate) = 2017
	and month(dr.ReportDate) = 4
	and b.branch_type = 'FC-SHOP'
	and isnull(dr.Cash,0) + isnull(dr.BSDCash,0) + isnull(dr.Con,0) > 0
	group by day(dr.ReportDate)
) as a