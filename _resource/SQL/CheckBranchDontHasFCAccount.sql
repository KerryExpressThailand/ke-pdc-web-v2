USE [POS]
GO

SELECT s.[userid]
      ,b.BranchID
      ,s.[PSA_Sync_Date]
FROM [tb_Branch] AS b
	LEFT JOIN [tb_master_user_shop] AS s ON b.BranchID = s.shop_id AND s.userid LIKE 'fc00%'
WHERE b.branch_type = 'FC-SHOP'
	AND b.BranchID IN ('BYAI', 'RSIT', 'KKAW', 'MTNG', 'MAHA', 'PKED', 'SCON', 'BKAE', 'BBUA', 'HPPY', 'TNON', 'NMIN', 'TPLU', 'PINK', 'RMA2', 'SMAI', 'BBON', 'TSIT', 'SUKS', 'DONM', 'NAWA', 'ONUT', 'LKAB', 'SMUT', 'TEPA', 'BPEE', 'TNPT', 'NAIN', 'TUPM', 'BANA', 'TTAI', 'NKAM', 'PANT', 'KBAN', 'EKKA', 'SAMK', 'TAIT', 'NJOK', 'BSTO', 'PTNK', 'MPIN', 'CHC4', 'SLYA', 'TKRU', 'BKEN', 'MINB', 'TYA3', 'BROM', 'NLCH', 'KVIL', 'LPDU', 'PS43', 'TAC4', 'BSAE', 'LAMB', 'SNOI', 'CWNA', 'SNBN', 'SAP2', 'PBSK', 'LUK2', 'SNDA', 'PYSL', 'LKAE', 'SUAS', 'ROMK', 'BANB', 'MCCS', 'BAPU', 'KSWA', 'AMTA', 'TYA6', 'POWK', 'NKCS', 'PTYA', 'MNKP', 'BWIN', 'ONTC', 'PWET', 'PNAM', 'PYSC', 'BTEC', 'FPAK', 'SAI4', 'BAKY', 'SKMT', 'TTLY', 'LAK4', 'SATU', 'SEC2', 'PT90', 'SPPA', 'BCPS', 'TPMR', 'CHLP', 'LUAP', 'PH44', 'NBKK', 'KNTH', 'KLUA', 'PTMH', 'POKN', 'DHLO', 'SKIT', 'EKA2', 'NRCM', 'TMDM', 'SIMM', 'TMHA', 'PORM', 'SUK2', 'NWA2', 'LTMP', 'BPLA', 'TAPO', 'SCBY', 'BTKH', 'TLIP', 'MVLD', 'PSJR', 'WDSR', 'BRO2', 'KAOL', 'TLCH', 'PASA', 'RKH2', 'YESP', 'IDPS', 'SVNP', 'SAI5', 'HPRP', 'MCRI', 'KLPP', 'TLPS', 'NMIT', 'RATN', 'SHPC', 'TLNN', 'MIST', 'CHBP', 'MTCP', 'PPDA', 'PTPR', 'CSKV', 'PTCS', 'WPLN', 'TLSN', 'PTSN', 'WCBK', 'LKL2', 'TPA2', 'SINS', 'SNCT', 'KTB2', 'TINT', 'PYL2', 'PPNM', 'RKRD', 'RPTN', 'RM34', 'NZMI', 'KTK7', 'SKB1', 'NMON', 'TONK', 'TNR5', 'TAI2', 'WWAN', 'TWAN', 'RBRN', 'LP16', 'NVPZ', 'MTEC', 'TNN2', 'WP16', 'SM65', 'SEC3', 'LLK3', 'PLUS', 'LTWH', 'KRCA', 'TSRD', 'PHAK', 'CH42', 'TSI2', 'SLY2', 'HPPS', 'HMRK', 'PMPL', 'HTAI', 'BCSD', 'LASW', 'TNWM', 'ZEER', 'CLPK', 'SNCH', 'NKBP', 'LKL3', 'ARUN', 'WWL4', 'PTRR', 'SDET', 'PSSN', 'SNB2', 'UDOM', 'BR56', 'HKLP', 'LASL', 'WMBS', 'HPSK', 'DKR2', 'BN15', 'BMOD', 'CRKB', 'PICK', 'WDIS', 'MKND', 'PCSP', 'KMBK', 'PTK7', 'PSSR', 'CNPA', 'PSLR', 'PUT5', 'SPA2', 'BABO', 'ACSM', 'CTWG', 'MKMR', 'YTK2', 'NMDA', 'SK52', 'LPUT', 'SCSR', 'BBB3', 'EK89', 'LTBK', 'PSNR', 'MBPA', 'PNLB', 'WATK')
	AND s.userid IS NULL
GO


