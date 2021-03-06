SELECT TOP (1000) [BranchID]
    ,[ReportDate]
    ,[CODSurcharge]
    ,[CODAmount]
	,[CODAmount] * 0.03 AS 'CODAmount 3%'
	,[CODSurcharge] - [CODAmount] * 0.03 AS Diff
FROM [POS].[dbo].[tb_daily_revenue]
WHERE BranchID IN ('CHC4', 'BSAE', 'AMTA', 'NKCS', 'PYSL', 'LAK4', 'BAKY', 'RKH2', 'SAI5', 'SEC2')
	AND MONTH(ReportDate) = 2
	AND YEAR(ReportDate) = 2018
	AND [CODSurcharge] - [CODAmount] * 0.03 <> 0