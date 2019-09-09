
-- ================================================================================
DECLARE @dateFrom DATE = '2019-02-01'
DECLARE @dateTo DATE = '2019-02-05'

DECLARE @branches VARCHAR(MAX)
SET @branches = '[  
	"BYAI",
	"AMTA",
	"ONUT",
	"NLCH"
]'
-- ================================================================================




-- ================================================================================
SELECT	 *
		 ,CAST(CASE WHEN DailyRevenue = EOD
		 THEN 1 ELSE 0 END AS BIT) AS VerifyEOD

		,CAST(CASE WHEN DailyRevenue = Reconcile
		 THEN 1 ELSE 0 END AS BIT) AS VerifyReconcile

		,Reconcile - DailyRevenue AS Variance
FROM (
	SELECT	 b.ERP_ID
			,b.BranchID
			,b.branch_type AS BranchType
			,dr.ReportDate

			-- EOD ==================================================================
			,ISNULL(eod.QRPay, 0) AS EOD
			-- EOD ==================================================================

			-- DailyRevenue =========================================================
			,ISNULL(dr.QRPay, 0) AS DailyRevenue
			-- DailyRevenue =========================================================

			-- Reconcile ============================================================
			,rsm.amount AS Reconcile
			-- Reconcile ============================================================

	FROM tb_daily_revenue AS dr
	
		INNER JOIN tb_Branch AS b
			ON b.BranchID = dr.BranchID
	
		LEFT JOIN tb_EOD AS eod
			ON dr.BranchID = eod.BranchID AND dr.ReportDate = eod.Report_Date
	
		LEFT JOIN tb_reconcile_summary_master AS rsm
			ON dr.BranchID = rsm.branch_id AND dr.ReportDate = rsm.report_date AND rsm.[type_id] = 'Q'

	WHERE	b.BranchID IN (SELECT * FROM OPENJSON (@branches) WITH ( BranchID VARCHAR(10) '$' ))
			AND ReportDate BETWEEN @dateFrom AND @dateTo
) AS x
WHERE Reconcile IS NOT NULL
ORDER BY ERP_ID
-- ================================================================================