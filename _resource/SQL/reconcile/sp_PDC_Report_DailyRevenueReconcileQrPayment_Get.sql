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
CREATE PROCEDURE [dbo].[sp_PDC_Report_DailyRevenueReconcileQrPayment_Get]
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
				 ,CAST(CASE WHEN DailyRevenue = EOD
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
					,ISNULL(eod.QRPay, 0) AS EOD
					-- EOD ==================================================================

					-- DailyRevenue =========================================================
					,ISNULL(dr.QRPay, 0) AS DailyRevenue
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
					ON dr.BranchID = rsm.branch_id AND dr.ReportDate = rsm.report_date AND rsm.[type_id] = 'Q'

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