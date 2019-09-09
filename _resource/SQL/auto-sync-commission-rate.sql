DECLARE @Month int = 2
DECLARE @Year int = 2019
DECLARE @as_of_month_previous int = 0
DECLARE @as_of_month int = 0

WHILE @Month < 12
BEGIN

	SET @as_of_month_previous = (@Year*100 + @Month)

	IF @Month < 1
	BEGIN  
		SET @as_of_month_previous = ((@Year-1)*100 + 12)
	END

	SET @Month = @Month+1
	SET @as_of_month = (@Year*100 + @Month)

	INSERT tb_Commission_Rate (branch_id, as_of_month, vat, commission, dropoff, dhl, cod, insure, bsd, rtsp, psp)
	SELECT	 b.BranchID AS branch_id
			,@as_of_month AS as_of_month
			,cr.vat
			,cr.commission
			,cr.dropoff
			,cr.dhl
			,cr.cod
			,cr.insure
			,cr.bsd
			,cr.rtsp
			,cr.psp
	FROM tb_Branch AS b
	LEFT JOIN tb_Commission_Rate AS cr
		ON b.BranchID = cr.branch_id
		AND as_of_month = @as_of_month_previous
	WHERE BranchID IN ('BYAI', 'RSIT', 'KKAW', 'MTNG', 'MAHA', 'PKED', 'SCON', 'BKAE', 'BBUA', 'HPPY', 'TNON', 'NMIN', 'TPLU', 'PINK', 'RMA2', 'SMAI', 'BBON', 'TSIT', 'SUKS', 'DONM', 'NAWA', 'ONUT', 'LKAB', 'SMUT', 'TEPA', 'BPEE', 'TNPT', 'NAIN', 'TUPM', 'BANA', 'TTAI', 'NKAM', 'PANT', 'KBAN', 'EKKA', 'SAMK', 'TAIT', 'NJOK', 'BSTO', 'PTNK', 'MPTN', 'CHC4', 'SLYA', 'TKRU', 'BKEN', 'MINB', 'TYA3', 'BROM', 'NLCH', 'KVIL', 'LPDU', 'PS43', 'TAC4', 'BSAE', 'LAMB', 'SNOI', 'CWNA', 'SNBN', 'SAP2', 'PBSK', 'LUK2', 'SNDA', 'PYSL', 'LKAE', 'SUAS', 'ROMK', 'BANB', 'MCCS', 'BAPU', 'KSWA', 'AMTA', 'TYA6', 'POKW', 'NKCS', 'PTYA', 'MNKP', 'BWIN', 'ONTC', 'PWET', 'PNAM', 'PYSC', 'BTEC', 'FPAK', 'SAI4', 'BAKY', 'SKMT', 'TTLY', 'LAK4', 'SATU', 'SEC2', 'PT90', 'SPPA', 'BCPS', 'TPMR', 'CHLP', 'LUAP', 'PH44', 'NBKK', 'KNTH', 'KLUA', 'PTMH', 'EKA2', 'POKN', 'SKIT', 'DHLO', 'SUK2', 'NRCM', 'TMDM', 'SIMM', 'BPLA', 'PORM', 'TMHA', 'PSJR', 'NWA2', 'LTMP', 'MVLD', 'TAPO', 'TLIP', 'SCBY', 'BTKH', 'WDSR', 'BRO2', 'KAOL', 'TLCH', 'PASA', 'RKH2', 'YESP', 'IDPS', 'SVNP', 'SAI5', 'HPRP', 'MCRI', 'PTPR', 'KLPP', 'TLPS', 'NMIT', 'RATN', 'SHPC', 'TLNN', 'MIST', 'CHBP', 'MTCP', 'PPDA', 'CSKV', 'PTCS', 'WPLN', 'TLSN', 'PTSN', 'WCBK', 'LKL2', 'TPA2', 'SINS', 'SNCT', 'KTB2', 'TINT', 'PYL2', 'PPNM', 'RKRD', 'RPTN', 'RM34', 'NZMI', 'KTK7', 'SKB1', 'NMON', 'TONK', 'TNR5', 'TAI2', 'WWAN', 'TWAN', 'RBRN', 'LP16', 'NVPZ', 'MTEC', 'TNN2', 'WP16', 'SM65', 'SEC3', 'LLK3', 'PLUS', 'LTWH', 'PHAK', 'CH42', 'TSI2', 'SLY2', 'HPPS', 'HMRK', 'PMPL', 'HTAI', 'BCSD', 'LASW', 'TNWM', 'ZEER', 'CLPK', 'SNCH', 'NKBP', 'LKL3', 'ARUN', 'WWL4', 'PTRR', 'SDET', 'PSSN', 'SNB2', 'UDOM', 'BR56', 'HKLP', 'LASL', 'WMBS', 'HPSK', 'DKR2', 'BN15', 'BMOD', 'CRKB', 'PICK', 'WDIS', 'MKND', 'PCSP', 'PTK7', 'PSSR', 'CNPA', 'PSLR', 'PUT5', 'SPA2', 'BABO', 'ACSM', 'CTWG', 'MKMR', 'YTK2', 'NMDA', 'SK52', 'LPUT', 'SCSR', 'BBB3', 'EK89', 'LTBK', 'PSNR', 'MBPA', 'PNLB', 'WATK', 'THEX', 'PTBT', 'RH17', 'BNKP', 'PSR2', 'BKTN', 'BCPD', 'BBN5', 'TCRP', 'BW82', 'SPAW', 'SSCT', 'KMIN', 'RTA7', 'PSRP', 'BAAO', 'RPTI', 'HAT8', 'JTCR', 'TUR3', 'FMCP', 'PSKD', 'MEGA', 'PAN2', 'MVKB', 'YKBP', 'PTSK', 'BCWT', 'PTPN', 'KUBO', 'TNSL', 'PTP8', 'TYA1', 'PTRT', 'PTK4', 'TWCR', 'PTBP', 'GSRJ', 'BLT5', 'PTP3', 'TNN3')
END