
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
		,CAST(CASE WHEN DailyRevenueCreditBBL = EODCreditBBL
		 THEN 1 ELSE 0 END AS BIT) AS VerifyEODCreditBBL

		 ,CAST(CASE WHEN DailyRevenueCreditSCB = EODCreditSCB
		 THEN 1 ELSE 0 END AS BIT) AS VerifyEODCreditSCB

		 ,CAST(CASE WHEN DailyRevenueRabbit = EODRabbit
		 THEN 1 ELSE 0 END AS BIT) AS VerifyEODRabbit

		,CAST(CASE WHEN DailyRevenue = Reconcile
		 THEN 1 ELSE 0 END AS BIT) AS VerifyReconcile

		,Reconcile - DailyRevenue AS Variance
FROM (
	SELECT	 b.ERP_ID
			,b.BranchID
			,b.branch_type AS BranchType
			,dr.ReportDate

			-- EOD ==================================================================
			,ISNULL(eod.CreditBBL, 0) AS EODCreditBBL
			,ISNULL(eod.CreditSCB, 0) AS EODCreditSCB
			,ISNULL(eod.Rabbit, 0) AS EODRabbit
			
			,ISNULL(eod.CreditBBL, 0)
			 + ISNULL(eod.CreditSCB, 0)
			 + ISNULL(eod.Rabbit, 0)
			AS EOD
			-- EOD ==================================================================

			-- DailyRevenue =========================================================
			,ISNULL(dr.CreditBBL, 0) AS DailyRevenueCreditBBL
			,ISNULL(dr.CreditSCB, 0) AS DailyRevenueCreditSCB
			,ISNULL(dr.Rabbit, 0) AS DailyRevenueRabbit

			,ISNULL(dr.CreditBBL, 0)
			 + ISNULL(dr.CreditSCB, 0)
			 + ISNULL(dr.Rabbit, 0)
			AS DailyRevenue
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
			ON dr.BranchID = rsm.branch_id AND dr.ReportDate = rsm.report_date AND rsm.[type_id] = 'C'

	WHERE	b.BranchID IN (SELECT * FROM OPENJSON (@branches) WITH ( BranchID VARCHAR(10) '$' ))
			AND ReportDate BETWEEN @dateFrom AND @dateTo
) AS x
WHERE Reconcile IS NOT NULL
ORDER BY ERP_ID
-- ================================================================================