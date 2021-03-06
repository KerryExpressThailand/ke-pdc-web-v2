SELECT	 COUNT(CASE SendToERP WHEN 1 then 1 ELSE NULL END) AS Branch_Sended
		,COUNT(*) AS Branch_Total
FROM [POS].[dbo].[tb_daily_revenue]
WHERE ReportDate between '2018-11-12' and '2018-11-19'
	AND Approved = 1




SELECT	 ISNULL(SUM(CASE WHEN api = 0 THEN 1 END), 0) AS Column_Pending
		,ISNULL(SUM(CASE WHEN api = 1 THEN 1 END), 0) AS Column_Sended
FROM [POS].[dbo].[tb_daily_revenue_erp]
WHERE report_date between '2018-11-12' and '2018-11-19'




SELECT SUM(ar_amount_dis) AS ar_amount_dis
  FROM [POS].[dbo].[tb_daily_revenue_erp] where report_date between '2018-11-12' and '2018-11-19' and api = 1