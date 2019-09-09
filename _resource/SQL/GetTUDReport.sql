SELECT CONVERT(VARCHAR, dr.ReportDate,112) + '-' + dr.BranchID AS id,
				dr.BranchID,
			    dr.ReportDate,
				b.DMSID,
				b.branch_type,
				b.ERP_ID,
				b.BranchName,
				ISNULL(dr.LineTopUpService,0) AS TUC,
				ISNULL(dr.BSDLineTopUp,0) AS TUP,
				ISNULL(dr.TUDForService,0) AS TUD,
				CASE WHEN dr.Captured=1 then 'Yes' else 'No' end AS Captured,
				dr.CapturedBy,				
				dr.CapturedDate,
				dr.TUDVerifyBy,
				dr.TUDVerifyDate,
				dr.RemittanceDate
			FROM tb_daily_revenue AS dr WITH (NOLOCK)
			INNER JOIN tb_Branch AS b WITH (NOLOCK)
			on dr.BranchID = b.BranchID
			WHERE	CONVERT(VARCHAR,dr.ReportDate,112) BETWEEN '20170404' AND '20170419'
					--AND b.BranchID = @BranchID
					--AND ISNULL(dr.TUDVerifyDate,0) = 0
					AND ISNULL(dr.TUDForService,0) <> 0
			ORDER BY b.ERP_ID, dr.ReportDate