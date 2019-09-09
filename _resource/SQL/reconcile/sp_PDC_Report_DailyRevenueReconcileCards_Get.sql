USE [POS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_PDC_Report_DailyRevenueReconcileCards_Get]
	@dateFrom DATE,				-- '2019-02-01'
	@dateTo DATE,				-- '2019-02-05'
	@branches VARCHAR(MAX),		-- '["ASK", "BANA", "SLOM"]'
	@page INT = 1,				-- 1
	@perPage INT = 15,			-- 15
	@filter VARCHAR(30) = NULL	-- 'matched' or 'unmatched' or'no-transfer'
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T1
	--//***** Begin Syntax *****//--
		SELECT	 *
				,CAST(CASE WHEN DailyRevenueCreditBBL = EODCreditBBL
				 THEN 1 ELSE 0 END AS BIT) AS VerifyEODCreditBBL

				 ,CAST(CASE WHEN DailyRevenueCreditSCB = EODCreditSCB
				 THEN 1 ELSE 0 END AS BIT) AS VerifyEODCreditSCB

				 ,CAST(CASE WHEN DailyRevenueRabbit = EODRabbit
				 THEN 1 ELSE 0 END AS BIT) AS VerifyEODRabbit

				 ,CAST(CASE WHEN DailyRevenueCreditBBL = EODCreditBBL AND DailyRevenueCreditSCB = EODCreditSCB AND DailyRevenueRabbit = EODRabbit
				 THEN 1 ELSE 0 END AS BIT) AS VerifyEOD

				,CAST(CASE WHEN DailyRevenue = Tranfer
				 THEN 1 ELSE 0 END AS BIT) AS VerifyTranfer

				,Tranfer - DailyRevenue AS Variance
		FROM (
			SELECT	 b.ERP_ID AS ERPID
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

					-- Tranfer ============================================================
					,rsm.amount AS Tranfer
					-- Tranfer ============================================================

			FROM tb_daily_revenue AS dr
	
				INNER JOIN tb_Branch AS b
					ON b.BranchID = dr.BranchID
	
				LEFT JOIN tb_EOD AS eod
					ON dr.BranchID = eod.BranchID AND dr.ReportDate = eod.Report_Date
	
				LEFT JOIN tb_reconcile_summary_master AS rsm
					ON dr.BranchID = rsm.branch_id AND dr.ReportDate = rsm.report_date AND rsm.[type_id] = 'C'

			WHERE	b.BranchID IN (SELECT * FROM OPENJSON (@branches) WITH ( BranchID VARCHAR(10) '$' ))
					AND ReportDate BETWEEN @dateFrom AND @dateTo
					AND (
						(@filter = 'matched' AND ISNULL(dr.QRPay, 0) = rsm.amount)
						OR
						(@filter = 'unmatched' AND ISNULL(dr.QRPay, 0) <> rsm.amount)
						OR
						(@filter = 'no-transfer' AND rsm.amount IS NULL)
						OR @filter IS NULL
					)
			
			ORDER BY ERP_ID
			OFFSET @perPage * (@Page - 1) ROWS
			FETCH NEXT @perPage ROWS ONLY
		) AS x
	--//***** Finally Process *****//--
	IF (@@ERROR <> 0)
	BEGIN
			ROLLBACK TRANSACTION T1
	END ELSE
	BEGIN
			COMMIT TRANSACTION T1
	END;
END