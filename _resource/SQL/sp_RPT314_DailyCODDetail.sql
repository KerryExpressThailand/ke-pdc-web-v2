USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[sp_RPT314_DailyCODDetail]    Script Date: 21/3/2560 14:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_RPT314_DailyCODDetail]
		@DateFrom VARCHAR(8),
		@DateTo VARCHAR(8),
		@UserID VARCHAR(20),
		@BranchID VARCHAR(20), -- ASK
		@Skip int = 0,
		@Take int = 10
AS
BEGIN
		BEGIN TRANSACTION T1
		--//***** Begin Syntax *****//--

		--============================================================
			SELECT	 s.BranchId AS Branch
					,s.Consignment_No AS con
					,s.cod_account_id AS accNo
					,s.cod_amount AS cod
					,DATENAME(MONTH, s.pickup_date) AS 'Month'
					,CONVERT(VARCHAR(8),s.pickup_date,112) AS pickup

			FROM	tb_CutOffDetail AS c WITH (NOLOCK)
					INNER JOIN tb_Shipment AS s WITH (NOLOCK)
						ON c.Consignment_No = s.Consignment_No AND s.cod_amount > 0 

			WHERE	s.BranchID = @branchID
					AND CONVERT(VARCHAR(8),c.ScannedTime,112) >= @DateFrom
					AND CONVERT(VARCHAR(8),c.ScannedTime,112) <= @DateTo
					

			ORDER BY CONVERT(VARCHAR(8),pickup_date,112)
			OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
		--============================================================


		--//***** Finally Process *****//--
		IF (@@ERROR <> 0)
		BEGIN
				rollback transaction T1
		END ELSE
		BEGIN
				commit transaction T1
		END;
END

