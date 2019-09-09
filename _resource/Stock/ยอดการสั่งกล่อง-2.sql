DECLARE @BranchIDList varchar(MAX) = 'FCSHOP,BYAI,RSIT,MTNG,KKAW,MAHA,PKED,SCON,HPPY,BKAE,BBUA,TNON,SMAI,SUKS,BBON,DONM,TSIT,ONUT,NAWA,LKAB,PINK,TPLU,NMIN,RMA2,BKEN,NAIN,BANA,SMUT,TEPA,BSTO,TKRU,NKAM,BPEE,TNPT,TUPM,TTAI,KBAN,EKKA,PANT,SAMK,NJOK,SLYA,MPTN,TYA3,TAIT,MINB,CHC4,PTNK,TLCH,BROM,NLCH,KVIL,LPDU,SNBN,CWNA,PS43,TAC4,PBSK,SNDA,BSAE,SNOI,LAMB,BANB,LKAE,SUAS,ROMK,TYA6,BAPU,MCCS,AMTA,NKCS,KSWA,PTYA,BWIN,PYSC,MNKP,POKW,LUK2,SAP2,PYSL,PWET,BTEC,PNAM,ONTC,TTLY,FPAK,SEC2,SATU,SAI4,LAK4,SKMT,BAKY,BCPS,NBKK,PT90,SPPA,TPMR,CHLP,LUAP,PH44,KNTH,KLUA,EKA2,BPLA,POKN,PORM,NWA2,KAOL,TAPO,TMHA,BRO2,PTMH,SKIT,DHLO,NRCM,SUK2,TMDM,SIMM,PSJR,LTMP,MVLD,TLIP,BTKH,SCBY,RKH2,HPRP,WDSR,PASA,YESP,IDPS,SVNP,NMIT,SAI5,KLPP,MCRI,PTPR,TLPS,MTCP,CSKV,RATN,PPDA,TLNN,SHPC,MIST,LKL2,PTCS,CHBP,PTSN,WCBK,WPLN,TLSN,TINT,SNCT,PPNM,SNCH,RM34,WP16,TPA2,SEC3,SINS,KTB2,PSSN,RBRN,PYL2,TAI2,CTWG,PLUS,RPTN,WWL4,LP16,RKRD,NZMI,TONK,NMON,KTK7,MTEC,NVPZ,TWAN,ARUN,WWAN,LTWH,SKB1,SM65,CH42,TNR5,TNN2,LLK3,HPPS,PMPL,SDET,ZEER,CRKB,TSI2,TCRP,HTAI,PHAK,LKL3,SNB2,WDIS,SLY2,HMRK,BCSD,CLPK,LASW,NKBP,TNWM,HKLP,LASL,BR56,LPUT,HPSK,PICK,PTRR,UDOM,WMBS,DKR2,PTK7,PCSP,BN15,MKND,BABO,BMOD,LTBK,PSSR,CNPA,BW82,YTK2,NMDA,PSLR,PSNR,ACSM,SK52,MKMR,MVKB,SPA2,THEX,PUT5,BCPD,PNLB,SCSR,BBB3,BNKP,EK89,PTBT,RH17,JTCR,BBN5,MBPA,WATK,MEGA,SPAW,BKTN,PSR2,SSCT,KMIN,RTA7,TUR3,FMCP,BAAO,PSRP,RPTI,YKBP,KUBO,HAT8,PTSK,PTPN,PAN2,PSKD,TYA1,BCWT'
DECLARE @Month int = 1
DECLARE @Year int = 2019

		--============================================================
		DECLARE	@TbBranch TABLE (b_id varchar(10) primary key);
		DECLARE @BranchID varchar(20),@Pos int;
	
		SET @BranchIDList = LTRIM(RTRIM(@BranchIDList))+ ',';
		SET @Pos = CHARINDEX(',', @BranchIDList, 1);
	
		IF REPLACE(@BranchIDList, ',', '') <> ''
		BEGIN
			WHILE @Pos > 0
			BEGIN
				SET @BranchID = LTRIM(RTRIM(LEFT(@BranchIDList, @Pos - 1)));
				SET @BranchIDList = RIGHT(@BranchIDList, LEN(@BranchIDList) - @Pos);
				SET @Pos = CHARINDEX(',', @BranchIDList, 1);
				INSERT INTO @TbBranch (b_id) VALUES (@BranchID);
			END
		END;
		--============================================================

		SELECT	 b.BranchID
				,SUM(QuantityBoxMini) AS QuantityBoxMini
				--,SUM(AmountBoxMini) AS AmountBoxMini
				,SUM(QuantityBoxS) AS QuantityBoxS
				--,SUM(AmountBoxS) AS AmountBoxS
				,SUM(QuantityBoxSPlus) AS QuantityBoxSPlus
				--,SUM(AmountBoxSPlus) AS AmountBoxSPlus
				,SUM(QuantityBoxM) AS QuantityBoxM
				--,SUM(AmountBoxM) AS AmountBoxM
				,SUM(QuantityBoxMPlus) AS QuantityBoxMPlus
				--,SUM(AmountBoxMPlus) AS AmountBoxMPlus
				,SUM(QuantityBoxL) AS QuantityBoxL
				--,SUM(AmountBoxL) AS AmountBoxL
				
				,SUM(QuantityOrangeKerryExpressTShirt) AS QuantityOrangeKerryExpressTShirt
				--,SUM(AmountOrangeKerryExpressTShirt) AS AmountOrangeKerryExpressTShirt
				
				,SUM(QuantityJacket) AS QuantityJacket
				--,SUM(AmountJacket) AS AmountJacket
				
				,SUM(QuantityOPPTapeCutter) AS QuantityOPPTapeCutter
				--,SUM(AmountOPPTapeCutter) AS AmountOPPTapeCutter
				
				,SUM(QuantityBlackKerryExpressTShirt) AS QuantityBlackKerryExpressTShirt
				--,SUM(AmountBlackKerryExpressTShirt) AS AmountBlackKerryExpressTShirt
				
				,SUM(QuantityGarbageBag) AS QuantityGarbageBag
				--,SUM(AmountGarbageBag) AS AmountGarbageBag
				
				,SUM(QuantityMeasuringTape) AS QuantityMeasuringTape
				--,SUM(AmountMeasuringTape) AS AmountMeasuringTape
				
				,SUM(QuantityIconSticker) AS QuantityIconSticker
				--,SUM(AmountIconSticker) AS AmountIconSticker
				
				,SUM(QuantityProhibitionSticker) AS QuantityProhibitionSticker
				--,SUM(AmountProhibitionSticker) AS AmountProhibitionSticker
				
				,SUM(QuantityOnOffLabel) AS QuantityOnOffLabel
				--,SUM(AmountOnOffLabel) AS AmountOnOffLabel
				
				,SUM(QuantityScotchTape) AS QuantityScotchTape
				--,SUM(AmountScotchTape) AS AmountScotchTape
				
				,SUM(QuantityBin) AS QuantityBin
				--,SUM(AmountBin) AS AmountBin
				
				,SUM(QuantityPenHolder) AS QuantityPenHolder
				--,SUM(AmountPenHolder) AS AmountPenHolder
				
				,SUM(QuantityScotchTapeCutter) AS QuantityScotchTapeCutter
				--,SUM(AmountScotchTapeCutter) AS AmountScotchTapeCutter
				
				,SUM(QuantityAcrylicA5) AS QuantityAcrylicA5
				--,SUM(AmountAcrylicA5) AS AmountAcrylicA5
				
				,SUM(QuantityAcrylicA4) AS QuantityAcrylicA4
				--,SUM(AmountAcrylicA4) AS AmountAcrylicA4
				
				,SUM(QuantityCalculator) AS QuantityCalculator
				--,SUM(AmountCalculator) AS AmountCalculator
				
				,SUM(QuantityAcrylicBrochure) AS QuantityAcrylicBrochure
				--,SUM(AmountAcrylicBrochure) AS AmountAcrylicBrochure
				
				,SUM(QuantityStickerOpenCloseShop) AS QuantityStickerOpenCloseShop
				--,SUM(AmountStickerOpenCloseShop) AS AmountStickerOpenCloseShop
				
				,SUM(QuantityTipBox) AS QuantityTipBox
				--,SUM(AmountTipBox) AS AmountTipBox
				
				,SUM(QuantityClock) AS QuantityClock
				--,SUM(AmountClock) AS AmountClock
				
				,SUM(QuantityCorkOpenCloseShop) AS QuantityCorkOpenCloseShop
				--,SUM(AmountCorkOpenCloseShop) AS AmountCorkOpenCloseShop
				
				,SUM(QuantitySlidingLabel) AS QuantitySlidingLabel
				--,SUM(AmountSlidingLabel) AS AmountSlidingLabel
				
				,SUM(QuantityPushPullingLabel) AS QuantityPushPullingLabel
				--,SUM(AmountSignagePermission) AS AmountSignagePermission
				
				,SUM(QuantitySignagePermission) AS QuantitySignagePermission
				--,SUM(AmountSignagePermission) AS AmountSignagePermission
				
				,SUM(QuantityBrochureShop) AS QuantityBrochureShop
				--,SUM(AmountBrochureShop) AS AmountBrochureShop
				
				,SUM(QuantityBusinessCard) AS QuantityBusinessCard
				--,SUM(AmountBusinessCard) AS AmountBusinessCard
				
				,SUM(QuantityBoxGray) AS QuantityBoxGray
				--,SUM(AmountBoxGray) AS AmountBoxGray
				
				,SUM(QuantityTrafficCone) AS QuantityTrafficCone
				--,SUM(AmountTrafficCone) AS AmountTrafficCone

				,SUM(QuantityPenCradle) AS QuantityPenCradle --PKG162
				--,SUM(AmountPenCradle) AS AmountPenCradle

				,SUM(QuantityTableCleaningTable) AS QuantityTableCleaningTable --PKG163
				--,SUM(AmountTableCleaningTable) AS AmountTableCleaningTable

				,SUM(QuantityStickerPriceStarts30Baht) AS QuantityStickerPriceStarts30Baht --PKG43
				--,SUM(AmountStickerPriceStarts30Baht) AS AmountStickerPriceStarts30Baht

				,SUM(QuantitySloganSticker) AS QuantitySloganSticker --PKG44
				--,SUM(AmountSloganSticker) AS AmountSloganSticker

				,SUM(QuantityStickerTelephone) AS QuantityStickerTelephone --PKG50
				--,SUM(AmountStickerTelephone) AS AmountStickerTelephone
				
				,SUM(QuantityIconStickerOld) AS QuantityIconStickerOld
				--,SUM(AmountIconStickerOld) AS AmountIconStickerOld

				,SUM(QuantityAcrylicA3WallMounted) AS QuantityAcrylicA3WallMounted --PKG68
				--,SUM(AmountAcrylicA3WallMounted) AS AmountAcrylicA3WallMounted

				,SUM(QuantitySmallScissors) AS QuantitySmallScissors --PKG122
				--,SUM(AmountSmallScissors) AS AmountSmallScissors

				,SUM(QuantityBigScissors) AS QuantityBigScissors --PKG123
				--,SUM(AmountBigScissors) AS AmountBigScissors

				,SUM(QuantityStapler) AS QuantityStapler --PKG124
				--,SUM(AmountStapler) AS AmountStapler

				,SUM(QuantityStaples) AS QuantityStaples --PKG125
				--,SUM(AmountStaples) AS AmountStaples

				,SUM(QuantityRuler12) AS QuantityRuler12 --PKG126
				--,SUM(AmountRuler12) AS AmountRuler12

				,SUM(QuantityCuter) AS QuantityCuter --PKG127
				--,SUM(AmountCuter) AS AmountCuter

				,SUM(QuantityClip) AS QuantityClip --PKG128
				--,SUM(AmountClip) AS AmountClip

				,SUM(QuantityBatteryAA) AS QuantityBatteryAA --PKG129
				--,SUM(AmountBatteryAA) AS AmountBatteryAA

				,SUM(QuantityBatteryPack) AS QuantityBatteryPack --PKG130
				--,SUM(AmountBatteryPack) AS AmountBatteryPack

				,SUM(QuantityHomePhone) AS QuantityHomePhone --PKG131
				--,SUM(AmountHomePhone) AS AmountHomePhone

				,SUM(QuantityPowerPlug) AS QuantityPowerPlug --PKG132
				--,SUM(AmountPowerPlug) AS AmountPowerPlug

				,SUM(QuantitySinglePenHolder) AS QuantitySinglePenHolder --PKG133
				--,SUM(AmountSinglePenHolder) AS AmountSinglePenHolder

				,SUM(QuantityAcrylicClareMoney) AS QuantityAcrylicClareMoney --PKG134
				--,SUM(AmountAcrylicClareMoney) AS AmountAcrylicClareMoney

				,SUM(QuantityTwoSidedTape3M) AS QuantityTwoSidedTape3M --PKG171PKG172
				--,SUM(AmountTwoSidedTape3M) AS AmountTwoSidedTape3M

				,SUM(QuantityCobwebBroom) AS QuantityCobwebBroom --PKG108
				--,SUM(AmountCobwebBroom) AS AmountCobwebBroom

				,SUM(QuantityStair) AS QuantityStair --PKG109
				--,SUM(AmountStair) AS AmountStair

				,SUM(Quantity3MFloorCleaner) AS Quantity3MFloorCleaner --PKG110
				--,SUM(Amount3MFloorCleaner) AS Amount3MFloorCleaner

				,SUM(QuantityFloorCleaner) AS QuantityFloorCleaner --PKG111 PKG117 PKG261
				--,SUM(AmountFloorCleaner) AS AmountFloorCleaner

				,SUM(QuantityRonson) AS QuantityRonson --PKG112
				--,SUM(AmountRonson) AS AmountRonson

				,SUM(QuantitySteklean) AS QuantitySteklean --PKG113
				--,SUM(AmountSteklean) AS AmountSteklean

				,SUM(QuantityGlassCleaner) AS QuantityGlassCleaner --PKG114
				--,SUM(AmountGlassCleaner) AS AmountGlassCleaner

				,SUM(QuantitySpaClean) AS QuantitySpaClean --PKG115PKG116
				--,SUM(AmountSpaClean) AS AmountSpaClean

				,SUM(QuantityWipes) AS QuantityWipes --PKG118
				--,SUM(AmountWipes) AS AmountWipes

				,SUM(QuantityCarpet) AS QuantityCarpet --PKG119
				--,SUM(AmountCarpet) AS AmountCarpet

				,SUM(QuantitySprayer) AS QuantitySprayer --PKG120
				--,SUM(AmountSprayer) AS AmountSprayer

				,SUM(QuantityThermometer) AS QuantityThermometer --PKG121
				--,SUM(AmountThermometer) AS AmountThermometer

				,SUM(QuantityBroomSet) AS QuantityBroomSet --PKG158
				--,SUM(AmountBroomSet) AS AmountBroomSet

				,SUM(QuantityWoodenFlooringBuckets) AS QuantityWoodenFlooringBuckets --PKG159
				--,SUM(AmountWoodenFlooringBuckets) AS AmountWoodenFlooringBuckets

				,SUM(QuantityWoodDust) AS QuantityWoodDust --PKG160
				--,SUM(AmountWoodDust) AS AmountWoodDust

				,SUM(QuantityWipesGlass) AS QuantityWipesGlass --PKG161
				--,SUM(AmountWipesGlass) AS AmountWipesGlass

				,SUM(QuantityMeasuringTapeHardLine) AS QuantityMeasuringTapeHardLine --PKG200
				--,SUM(AmountMeasuringTapeHardLine) AS AmountMeasuringTapeHardLine

				,SUM(QuantityVinelJFlag) AS QuantityVinelJFlag --PKG101,PKG102
				--,SUM(AmountVinelJFlag) AS AmountVinelJFlag

				,SUM(QuantityAcrylicDisposableQueue) AS QuantityAcrylicDisposableQueue --PKG206
				--,SUM(AmountAcrylicDisposableQueue) AS AmountAcrylicDisposableQueue

				,SUM(QuantityWipeMirror) AS QuantityWipeMirror --PKG161
				--,SUM(AmountWipeMirror) AS AmountWipeMirror

				,SUM(QuantityJFlagVinel) AS QuantityJFlagVinel --PKG161
				--,SUM(AmountJFlagVinel) AS AmountJFlagVinel
				
				,SUM(QuantityGarbageBagWhite) AS QuantityGarbageBagWhite
				--,SUM(AmountGarbageBagWhite) AS AmountGarbageBagWhite
				
				,SUM(QuantityStampNonCondition) AS QuantityStampNonCondition
				--,SUM(AmountStampNonCondition) AS AmountStampNonCondition
				
				,SUM(QuantityRedInkstand) AS QuantityRedInkstand
				--,SUM(AmountRedInkstand) AS AmountRedInkstand
				
				,SUM(QuantityClearTrash) AS QuantityClearTrash
				--,SUM(AmountClearTrash) AS AmountClearTrash
				
				,SUM(QuantityMousePad) AS QuantityMousePad
				--,SUM(AmountMousePad) AS AmountMousePad
				
				,SUM(QuantityEnvelopeBoxWithoutLid) AS QuantityEnvelopeBoxWithoutLid
				--,SUM(AmountEnvelopeBoxWithoutLid) AS AmountEnvelopeBoxWithoutLid

				
				
				--,SUM(
				--	QuantityBoxMini + QuantityBoxS + QuantityBoxSPlus + QuantityBoxM + QuantityBoxMPlus + QuantityBoxL + QuantityOrangeKerryExpressTShirt + QuantityJacket + QuantityOPPTapeCutter + QuantityBlackKerryExpressTShirt + QuantityGarbageBag + QuantityMeasuringTape + QuantityIconSticker + QuantityProhibitionSticker + QuantityOnOffLabel + QuantityScotchTape + QuantityBin + QuantityPenHolder + QuantityScotchTapeCutter + QuantityAcrylicA5 + QuantityAcrylicA4 + QuantityCalculator + QuantityAcrylicBrochure + QuantityStickerOpenCloseShop + QuantityTipBox + QuantityClock + QuantityCorkOpenCloseShop + QuantitySlidingLabel + QuantityPushPullingLabel + QuantitySignagePermission + QuantityBrochureShop + QuantityBusinessCard + QuantityBoxGray + QuantityTrafficCone + QuantityPenCradle + QuantityTableCleaningTable + QuantityStickerPriceStarts30Baht + QuantitySloganSticker + QuantityStickerTelephone + QuantityIconStickerOld + QuantityAcrylicA3WallMounted + QuantitySmallScissors + QuantityBigScissors + QuantityStapler + QuantityStaples + QuantityRuler12 + QuantityCuter + QuantityClip + QuantityBatteryAA + QuantityBatteryPack + QuantityHomePhone + QuantityPowerPlug + QuantitySinglePenHolder + QuantityAcrylicClareMoney + QuantityTwoSidedTape3M + QuantityCobwebBroom + QuantityStair + Quantity3MFloorCleaner + QuantityFloorCleaner + QuantityRonson + QuantitySteklean + QuantityGlassCleaner + QuantitySpaClean + QuantityWipes + QuantityCarpet + QuantitySprayer + QuantityThermometer + QuantityBroomSet + QuantityWoodenFlooringBuckets + QuantityWoodDust + QuantityWipesGlass
				--) AS TotalQuantity
				--,SUM(
				--	AmountBoxMini + AmountBoxS + AmountBoxSPlus + AmountBoxM + AmountBoxMPlus + AmountBoxL + 
				--	AmountOrangeKerryExpressTShirt + AmountJacket + AmountOPPTapeCutter + 
				--	AmountBlackKerryExpressTShirt + AmountGarbageBag + AmountMeasuringTape + 
				--	AmountIconSticker + AmountProhibitionSticker + AmountOnOffLabel + AmountScotchTape + 
				--	AmountBin + AmountPenHolder + AmountScotchTapeCutter + AmountAcrylicA5 + 
				--	AmountAcrylicA4 + AmountCalculator + AmountAcrylicBrochure + AmountStickerOpenCloseShop + 
				--	AmountTipBox + AmountClock + AmountCorkOpenCloseShop + AmountSlidingLabel + 
				--	AmountPushPullingLabel + AmountSignagePermission + AmountBrochureShop + 
				--	AmountBusinessCard + AmountBoxGray + AmountTrafficCone + AmountPenCradle + 
				--	AmountTableCleaningTable + AmountStickerPriceStarts30Baht + AmountSloganSticker + 
				--	AmountStickerTelephone + AmountIconStickerOld + AmountAcrylicA3WallMounted + 
				--	AmountSmallScissors + AmountBigScissors + AmountStapler + AmountStaples + AmountRuler12 + 
				--	AmountCuter + AmountClip + AmountBatteryAA + AmountBatteryPack + AmountHomePhone + 
				--	AmountPowerPlug + AmountSinglePenHolder + AmountAcrylicClareMoney + AmountTwoSidedTape3M + 
				--	AmountCobwebBroom + AmountStair + Amount3MFloorCleaner + AmountFloorCleaner + 
				--	AmountRonson + AmountSteklean + AmountGlassCleaner + AmountSpaClean + AmountWipes + 
				--	AmountCarpet + AmountSprayer + AmountThermometer + AmountBroomSet + 
				--	AmountWoodenFlooringBuckets + AmountWoodDust + AmountWipesGlass + AmountMeasuringTapeHardLine + 
				--	AmountVinelJFlag + AmountAcrylicDisposableQueue + AmountWipeMirror + AmountJFlagVinel + AmountGarbageBagWhite +
				--	AmountStampNonCondition + AmountRedInkstand + AmountClearTrash + AmountMousePad + AmountEnvelopeBoxWithoutLid
				--) AS TotalAmount
		FROM (
			SELECT	 a.ERP_ID
					,a.BranchID
					,CASE WHEN a.PackageID = 'PKG07' THEN pkg_qty ELSE 0 END AS 'QuantityBoxMini'
					,CASE WHEN a.PackageID = 'PKG07' THEN Amount ELSE 0 END AS 'AmountBoxMini'
					,CASE WHEN a.PackageID = 'PKG03' THEN pkg_qty ELSE 0 END AS 'QuantityBoxS'
					,CASE WHEN a.PackageID = 'PKG03' THEN Amount ELSE 0 END AS 'AmountBoxS'
					,CASE WHEN a.PackageID = 'PKG08' THEN pkg_qty ELSE 0 END AS 'QuantityBoxSPlus'
					,CASE WHEN a.PackageID = 'PKG08' THEN Amount ELSE 0 END AS 'AmountBoxSPlus'
					,CASE WHEN a.PackageID = 'PKG04' THEN pkg_qty ELSE 0 END AS 'QuantityBoxM'
					,CASE WHEN a.PackageID = 'PKG04' THEN Amount ELSE 0 END AS 'AmountBoxM'
					,CASE WHEN a.PackageID = 'PKG18' THEN pkg_qty ELSE 0 END AS 'QuantityBoxMPlus'
					,CASE WHEN a.PackageID = 'PKG18' THEN Amount ELSE 0 END AS 'AmountBoxMPlus'
					,CASE WHEN a.PackageID = 'PKG05' THEN pkg_qty ELSE 0 END AS 'QuantityBoxL'
					,CASE WHEN a.PackageID = 'PKG05' THEN Amount ELSE 0 END AS 'AmountBoxL'

					,CASE WHEN a.PackageID IN ('PKG54', 'PKG55', 'PKG56', 'PKG57') THEN pkg_qty ELSE 0 END AS 'QuantityOrangeKerryExpressTShirt'
					,CASE WHEN a.PackageID IN ('PKG54', 'PKG55', 'PKG56', 'PKG57') THEN Amount ELSE 0 END AS 'AmountOrangeKerryExpressTShirt'

					,CASE WHEN a.PackageID IN ('PKG58', 'PKG59', 'PKG60', 'PKG88', 'PKG241') THEN pkg_qty ELSE 0 END AS 'QuantityJacket'
					,CASE WHEN a.PackageID IN ('PKG58', 'PKG59', 'PKG60', 'PKG88', 'PKG241') THEN Amount ELSE 0 END AS 'AmountJacket'
					
					,CASE WHEN a.PackageID = 'PKG69' THEN pkg_qty ELSE 0 END AS 'QuantityOPPTapeCutter'
					,CASE WHEN a.PackageID = 'PKG69' THEN Amount ELSE 0 END AS 'AmountOPPTapeCutter'

					,CASE WHEN a.PackageID IN ('PKG91', 'PKG92', 'PKG93', 'PKG94') THEN pkg_qty ELSE 0 END AS 'QuantityBlackKerryExpressTShirt'
					,CASE WHEN a.PackageID IN ('PKG91', 'PKG92', 'PKG93', 'PKG94') THEN Amount ELSE 0 END AS 'AmountBlackKerryExpressTShirt'
					
					,CASE WHEN a.PackageID = 'PKG75' THEN pkg_qty ELSE 0 END AS 'QuantityGarbageBag'
					,CASE WHEN a.PackageID = 'PKG75' THEN Amount ELSE 0 END AS 'AmountGarbageBag'
					
					,CASE WHEN a.PackageID = 'PKG74' THEN pkg_qty ELSE 0 END AS 'QuantityMeasuringTape'
					,CASE WHEN a.PackageID = 'PKG74' THEN Amount ELSE 0 END AS 'AmountMeasuringTape'
					
					,CASE WHEN a.PackageID = 'PKG46' THEN pkg_qty ELSE 0 END AS 'QuantityIconSticker'
					,CASE WHEN a.PackageID = 'PKG46' THEN Amount ELSE 0 END AS 'AmountIconSticker'
					
					,CASE WHEN a.PackageID = 'PKG49' THEN pkg_qty ELSE 0 END AS 'QuantityProhibitionSticker'
					,CASE WHEN a.PackageID = 'PKG49' THEN Amount ELSE 0 END AS 'AmountProhibitionSticker'
					
					,CASE WHEN a.PackageID = 'PKG72' THEN pkg_qty ELSE 0 END AS 'QuantityOnOffLabel'
					,CASE WHEN a.PackageID = 'PKG72' THEN Amount ELSE 0 END AS 'AmountOnOffLabel'
					
					,CASE WHEN a.PackageID = 'PKG63' THEN pkg_qty ELSE 0 END AS 'QuantityScotchTape'
					,CASE WHEN a.PackageID = 'PKG63' THEN Amount ELSE 0 END AS 'AmountScotchTape'
					
					,CASE WHEN a.PackageID = 'PKG73' THEN pkg_qty ELSE 0 END AS 'QuantityBin'
					,CASE WHEN a.PackageID = 'PKG73' THEN Amount ELSE 0 END AS 'AmountBin'
					
					,CASE WHEN a.PackageID = 'PKG62' THEN pkg_qty ELSE 0 END AS 'QuantityPenHolder'
					,CASE WHEN a.PackageID = 'PKG62' THEN Amount ELSE 0 END AS 'AmountPenHolder'
					
					,CASE WHEN a.PackageID = 'PKG64' THEN pkg_qty ELSE 0 END AS 'QuantityScotchTapeCutter'
					,CASE WHEN a.PackageID = 'PKG64' THEN Amount ELSE 0 END AS 'AmountScotchTapeCutter'
					
					,CASE WHEN a.PackageID = 'PKG66' THEN pkg_qty ELSE 0 END AS 'QuantityAcrylicA5'
					,CASE WHEN a.PackageID = 'PKG66' THEN Amount ELSE 0 END AS 'AmountAcrylicA5'
					
					,CASE WHEN a.PackageID = 'PKG67' THEN pkg_qty ELSE 0 END AS 'QuantityAcrylicA4'
					,CASE WHEN a.PackageID = 'PKG67' THEN Amount ELSE 0 END AS 'AmountAcrylicA4'
					
					,CASE WHEN a.PackageID = 'PKG65' THEN pkg_qty ELSE 0 END AS 'QuantityCalculator'
					,CASE WHEN a.PackageID = 'PKG65' THEN Amount ELSE 0 END AS 'AmountCalculator'
					
					,CASE WHEN a.PackageID = 'PKG70' THEN pkg_qty ELSE 0 END AS 'QuantityAcrylicBrochure'
					,CASE WHEN a.PackageID = 'PKG70' THEN Amount ELSE 0 END AS 'AmountAcrylicBrochure'
					
					,CASE WHEN a.PackageID = 'PKG52' THEN pkg_qty ELSE 0 END AS 'QuantityStickerOpenCloseShop'
					,CASE WHEN a.PackageID = 'PKG52' THEN Amount ELSE 0 END AS 'AmountStickerOpenCloseShop'
					
					,CASE WHEN a.PackageID = 'PKG61' THEN pkg_qty ELSE 0 END AS 'QuantityTipBox'
					,CASE WHEN a.PackageID = 'PKG61' THEN Amount ELSE 0 END AS 'AmountTipBox'
					
					,CASE WHEN a.PackageID = 'PKG71' THEN pkg_qty ELSE 0 END AS 'QuantityClock'
					,CASE WHEN a.PackageID = 'PKG71' THEN Amount ELSE 0 END AS 'AmountClock'
					
					,CASE WHEN a.PackageID = 'PKG136' THEN pkg_qty ELSE 0 END AS 'QuantityCorkOpenCloseShop'
					,CASE WHEN a.PackageID = 'PKG136' THEN Amount ELSE 0 END AS 'AmountCorkOpenCloseShop'
					
					,CASE WHEN a.PackageID = 'PKG137' THEN pkg_qty ELSE 0 END AS 'QuantitySlidingLabel'
					,CASE WHEN a.PackageID = 'PKG137' THEN Amount ELSE 0 END AS 'AmountSlidingLabel'
					
					,CASE WHEN a.PackageID = 'PKG138' THEN pkg_qty ELSE 0 END AS 'QuantityPushPullingLabel'
					,CASE WHEN a.PackageID = 'PKG138' THEN Amount ELSE 0 END AS 'AmountPushPullingLabel'
					
					,CASE WHEN a.PackageID = 'PKG139' THEN pkg_qty ELSE 0 END AS 'QuantitySignagePermission'
					,CASE WHEN a.PackageID = 'PKG139' THEN Amount ELSE 0 END AS 'AmountSignagePermission'
					
					,CASE WHEN a.PackageID = 'PKG152' THEN pkg_qty ELSE 0 END AS 'QuantityBrochureShop'
					,CASE WHEN a.PackageID = 'PKG152' THEN Amount ELSE 0 END AS 'AmountBrochureShop'
					
					,CASE WHEN a.PackageID = 'PKG153' THEN pkg_qty ELSE 0 END AS 'QuantityBusinessCard'
					,CASE WHEN a.PackageID = 'PKG153' THEN Amount ELSE 0 END AS 'AmountBusinessCard'
					
					,CASE WHEN a.PackageID = 'PKG156' THEN pkg_qty ELSE 0 END AS 'QuantityBoxGray'
					,CASE WHEN a.PackageID = 'PKG156' THEN Amount ELSE 0 END AS 'AmountBoxGray'
					
					,CASE WHEN a.PackageID = 'PKG157' THEN pkg_qty ELSE 0 END AS 'QuantityTrafficCone'
					,CASE WHEN a.PackageID = 'PKG157' THEN Amount ELSE 0 END AS 'AmountTrafficCone'
					
					,CASE WHEN a.PackageID = 'PKG162' THEN pkg_qty ELSE 0 END AS 'QuantityPenCradle'
					,CASE WHEN a.PackageID = 'PKG162' THEN Amount ELSE 0 END AS 'AmountPenCradle'

					,CASE WHEN a.PackageID = 'PKG163' THEN pkg_qty ELSE 0 END AS 'QuantityTableCleaningTable'
					,CASE WHEN a.PackageID = 'PKG163' THEN Amount ELSE 0 END AS 'AmountTableCleaningTable'

					,CASE WHEN a.PackageID = 'PKG43' THEN pkg_qty ELSE 0 END AS 'QuantityStickerPriceStarts30Baht'
					,CASE WHEN a.PackageID = 'PKG43' THEN Amount ELSE 0 END AS 'AmountStickerPriceStarts30Baht'

					,CASE WHEN a.PackageID = 'PKG44' THEN pkg_qty ELSE 0 END AS 'QuantitySloganSticker'
					,CASE WHEN a.PackageID = 'PKG44' THEN Amount ELSE 0 END AS 'AmountSloganSticker'

					,CASE WHEN a.PackageID = 'PKG50' THEN pkg_qty ELSE 0 END AS 'QuantityStickerTelephone'
					,CASE WHEN a.PackageID = 'PKG50' THEN Amount ELSE 0 END AS 'AmountStickerTelephone'

					,CASE WHEN a.PackageID = 'PKG103' THEN pkg_qty ELSE 0 END AS 'QuantityIconStickerOld'
					,CASE WHEN a.PackageID = 'PKG103' THEN Amount ELSE 0 END AS 'AmountIconStickerOld'

					,CASE WHEN a.PackageID = 'PKG68' THEN pkg_qty ELSE 0 END AS 'QuantityAcrylicA3WallMounted'
					,CASE WHEN a.PackageID = 'PKG68' THEN Amount ELSE 0 END AS 'AmountAcrylicA3WallMounted'

					,CASE WHEN a.PackageID = 'PKG122' THEN pkg_qty ELSE 0 END AS 'QuantitySmallScissors'
					,CASE WHEN a.PackageID = 'PKG122' THEN Amount ELSE 0 END AS 'AmountSmallScissors'

					,CASE WHEN a.PackageID = 'PKG123' THEN pkg_qty ELSE 0 END AS 'QuantityBigScissors'
					,CASE WHEN a.PackageID = 'PKG123' THEN Amount ELSE 0 END AS 'AmountBigScissors'

					,CASE WHEN a.PackageID = 'PKG124' THEN pkg_qty ELSE 0 END AS 'QuantityStapler'
					,CASE WHEN a.PackageID = 'PKG124' THEN Amount ELSE 0 END AS 'AmountStapler'

					,CASE WHEN a.PackageID = 'PKG125' THEN pkg_qty ELSE 0 END AS 'QuantityStaples'
					,CASE WHEN a.PackageID = 'PKG125' THEN Amount ELSE 0 END AS 'AmountStaples'

					,CASE WHEN a.PackageID = 'PKG126' THEN pkg_qty ELSE 0 END AS 'QuantityRuler12'
					,CASE WHEN a.PackageID = 'PKG126' THEN Amount ELSE 0 END AS 'AmountRuler12'

					,CASE WHEN a.PackageID = 'PKG127' THEN pkg_qty ELSE 0 END AS 'QuantityCuter'
					,CASE WHEN a.PackageID = 'PKG127' THEN Amount ELSE 0 END AS 'AmountCuter'

					,CASE WHEN a.PackageID = 'PKG128' THEN pkg_qty ELSE 0 END AS 'QuantityClip'
					,CASE WHEN a.PackageID = 'PKG128' THEN Amount ELSE 0 END AS 'AmountClip'

					,CASE WHEN a.PackageID = 'PKG129' THEN pkg_qty ELSE 0 END AS 'QuantityBatteryAA'
					,CASE WHEN a.PackageID = 'PKG129' THEN Amount ELSE 0 END AS 'AmountBatteryAA'

					,CASE WHEN a.PackageID = 'PKG130' THEN pkg_qty ELSE 0 END AS 'QuantityBatteryPack'
					,CASE WHEN a.PackageID = 'PKG130' THEN Amount ELSE 0 END AS 'AmountBatteryPack'

					,CASE WHEN a.PackageID = 'PKG131' THEN pkg_qty ELSE 0 END AS 'QuantityHomePhone'
					,CASE WHEN a.PackageID = 'PKG131' THEN Amount ELSE 0 END AS 'AmountHomePhone'

					,CASE WHEN a.PackageID = 'PKG132' THEN pkg_qty ELSE 0 END AS 'QuantityPowerPlug'
					,CASE WHEN a.PackageID = 'PKG132' THEN Amount ELSE 0 END AS 'AmountPowerPlug'

					,CASE WHEN a.PackageID = 'PKG133' THEN pkg_qty ELSE 0 END AS 'QuantitySinglePenHolder'
					,CASE WHEN a.PackageID = 'PKG133' THEN Amount ELSE 0 END AS 'AmountSinglePenHolder'

					,CASE WHEN a.PackageID = 'PKG134' THEN pkg_qty ELSE 0 END AS 'QuantityAcrylicClareMoney'
					,CASE WHEN a.PackageID = 'PKG134' THEN Amount ELSE 0 END AS 'AmountAcrylicClareMoney'

					,CASE WHEN a.PackageID IN ('PKG171', 'PKG172') THEN pkg_qty ELSE 0 END AS 'QuantityTwoSidedTape3M'
					,CASE WHEN a.PackageID IN ('PKG171', 'PKG172') THEN Amount ELSE 0 END AS 'AmountTwoSidedTape3M'
					
					,CASE WHEN a.PackageID = 'PKG108' THEN pkg_qty ELSE 0 END AS 'QuantityCobwebBroom'
					,CASE WHEN a.PackageID = 'PKG108' THEN Amount ELSE 0 END AS 'AmountCobwebBroom'

					,CASE WHEN a.PackageID = 'PKG109' THEN pkg_qty ELSE 0 END AS 'QuantityStair'
					,CASE WHEN a.PackageID = 'PKG109' THEN Amount ELSE 0 END AS 'AmountStair'

					,CASE WHEN a.PackageID = 'PKG110' THEN pkg_qty ELSE 0 END AS 'Quantity3MFloorCleaner'
					,CASE WHEN a.PackageID = 'PKG110' THEN Amount ELSE 0 END AS 'Amount3MFloorCleaner'

					,CASE WHEN a.PackageID IN ('PKG111', 'PKG117', 'PKG261') THEN pkg_qty ELSE 0 END AS 'QuantityFloorCleaner'
					,CASE WHEN a.PackageID IN ('PKG111', 'PKG117', 'PKG261') THEN Amount ELSE 0 END AS 'AmountFloorCleaner'

					,CASE WHEN a.PackageID = 'PKG112' THEN pkg_qty ELSE 0 END AS 'QuantityRonson'
					,CASE WHEN a.PackageID = 'PKG112' THEN Amount ELSE 0 END AS 'AmountRonson'

					,CASE WHEN a.PackageID = 'PKG113' THEN pkg_qty ELSE 0 END AS 'QuantitySteklean'
					,CASE WHEN a.PackageID = 'PKG113' THEN Amount ELSE 0 END AS 'AmountSteklean'

					,CASE WHEN a.PackageID = 'PKG114' THEN pkg_qty ELSE 0 END AS 'QuantityGlassCleaner'
					,CASE WHEN a.PackageID = 'PKG114' THEN Amount ELSE 0 END AS 'AmountGlassCleaner'

					,CASE WHEN a.PackageID IN ('PKG115', 'PKG116') THEN pkg_qty ELSE 0 END AS 'QuantitySpaClean'
					,CASE WHEN a.PackageID IN ('PKG115', 'PKG116') THEN Amount ELSE 0 END AS 'AmountSpaClean'

					,CASE WHEN a.PackageID = 'PKG118' THEN pkg_qty ELSE 0 END AS 'QuantityWipes'
					,CASE WHEN a.PackageID = 'PKG118' THEN Amount ELSE 0 END AS 'AmountWipes'

					,CASE WHEN a.PackageID = 'PKG119' THEN pkg_qty ELSE 0 END AS 'QuantityCarpet'
					,CASE WHEN a.PackageID = 'PKG119' THEN Amount ELSE 0 END AS 'AmountCarpet'

					,CASE WHEN a.PackageID = 'PKG120' THEN pkg_qty ELSE 0 END AS 'QuantitySprayer'
					,CASE WHEN a.PackageID = 'PKG120' THEN Amount ELSE 0 END AS 'AmountSprayer'

					,CASE WHEN a.PackageID = 'PKG121' THEN pkg_qty ELSE 0 END AS 'QuantityThermometer'
					,CASE WHEN a.PackageID = 'PKG121' THEN Amount ELSE 0 END AS 'AmountThermometer'

					,CASE WHEN a.PackageID = 'PKG158' THEN pkg_qty ELSE 0 END AS 'QuantityBroomSet'
					,CASE WHEN a.PackageID = 'PKG158' THEN Amount ELSE 0 END AS 'AmountBroomSet'

					,CASE WHEN a.PackageID = 'PKG159' THEN pkg_qty ELSE 0 END AS 'QuantityWoodenFlooringBuckets'
					,CASE WHEN a.PackageID = 'PKG159' THEN Amount ELSE 0 END AS 'AmountWoodenFlooringBuckets'

					,CASE WHEN a.PackageID = 'PKG160' THEN pkg_qty ELSE 0 END AS 'QuantityWoodDust'
					,CASE WHEN a.PackageID = 'PKG160' THEN Amount ELSE 0 END AS 'AmountWoodDust'

					,CASE WHEN a.PackageID = 'PKG161' THEN pkg_qty ELSE 0 END AS 'QuantityWipesGlass'
					,CASE WHEN a.PackageID = 'PKG161' THEN Amount ELSE 0 END AS 'AmountWipesGlass'

					,CASE WHEN a.PackageID = 'PKG200' THEN pkg_qty ELSE 0 END AS 'QuantityMeasuringTapeHardLine'
					,CASE WHEN a.PackageID = 'PKG200' THEN Amount ELSE 0 END AS 'AmountMeasuringTapeHardLine'

					,CASE WHEN a.PackageID IN ('PKG101', 'PKG102') THEN pkg_qty ELSE 0 END AS 'QuantityVinelJFlag'
					,CASE WHEN a.PackageID IN ('PKG101', 'PKG102') THEN Amount ELSE 0 END AS 'AmountVinelJFlag'

					,CASE WHEN a.PackageID = 'PKG206' THEN pkg_qty ELSE 0 END AS 'QuantityAcrylicDisposableQueue'
					,CASE WHEN a.PackageID = 'PKG206' THEN Amount ELSE 0 END AS 'AmountAcrylicDisposableQueue'

					,CASE WHEN a.PackageID = 'PKG161' THEN pkg_qty ELSE 0 END AS 'QuantityWipeMirror'
					,CASE WHEN a.PackageID = 'PKG161' THEN Amount ELSE 0 END AS 'AmountWipeMirror'

					,CASE WHEN a.PackageID IN ('PKG36', 'PKG99', 'PKG100') THEN pkg_qty ELSE 0 END AS 'QuantityJFlagVinel'
					,CASE WHEN a.PackageID IN ('PKG36', 'PKG99', 'PKG100') THEN Amount ELSE 0 END AS 'AmountJFlagVinel'
					
					,CASE WHEN a.PackageID IN ('PKG240') THEN pkg_qty ELSE 0 END AS 'QuantityGarbageBagWhite'
					,CASE WHEN a.PackageID IN ('PKG240') THEN Amount ELSE 0 END AS 'AmountGarbageBagWhite'
					
					,CASE WHEN a.PackageID IN ('PKG245') THEN pkg_qty ELSE 0 END AS 'QuantityStampNonCondition'
					,CASE WHEN a.PackageID IN ('PKG245') THEN Amount ELSE 0 END AS 'AmountStampNonCondition'
					
					,CASE WHEN a.PackageID IN ('PKG246') THEN pkg_qty ELSE 0 END AS 'QuantityRedInkstand'
					,CASE WHEN a.PackageID IN ('PKG246') THEN Amount ELSE 0 END AS 'AmountRedInkstand'
					
					,CASE WHEN a.PackageID IN ('PKG251') THEN pkg_qty ELSE 0 END AS 'QuantityClearTrash'
					,CASE WHEN a.PackageID IN ('PKG251') THEN Amount ELSE 0 END AS 'AmountClearTrash'
					
					,CASE WHEN a.PackageID IN ('PKG260') THEN pkg_qty ELSE 0 END AS 'QuantityMousePad'
					,CASE WHEN a.PackageID IN ('PKG260') THEN Amount ELSE 0 END AS 'AmountMousePad'
					
					,CASE WHEN a.PackageID IN ('PKG259') THEN pkg_qty ELSE 0 END AS 'QuantityEnvelopeBoxWithoutLid'
					,CASE WHEN a.PackageID IN ('PKG259') THEN Amount ELSE 0 END AS 'AmountEnvelopeBoxWithoutLid'
			FROM (
				SELECT	 h.order_no AS OrderNo
						,b.ERP_ID
						,h.branch_id AS BranchID
						,h.[status] AS [Status]
						,CONVERT(varchar,h.created_date,5) + ' ' + CONVERT(varchar(5),h.created_date,114) AS OrderDate
						,p.PackageID
						,p.PackageDesc
						,p.unit AS UnitText
						,i.qty
						,i.unit_price
						,ISNULL(i.packing_qty,0) AS pkg_qty
						,ISNULL(i.packing_qty, 0) * ISNULL(i.unit_price, 0) AS Amount
				FROM tb_stock_order_item AS i
					INNER JOIN tb_stock_order_head AS h WITH(NOLOCK) ON i.order_no = h.order_no
					LEFT JOIN tb_Package AS p WITH(NOLOCK) ON i.package_id = p.PackageID
					LEFT JOIN tb_Branch AS b WITH(NOLOCK) ON h.branch_id = b.BranchID
				WHERE (h.branch_id in (SELECT b_id FROM @TbBranch))
					--h.branch_id = 'FPAK'
					--AND h.branch_id NOT IN ('AMTA','BAKY','BANA','BANB','BAPU','BBON','BBUA','BKAE','BKEN','BPEE','BROM','BSAE','BSTO','BTEC','BWIN','BYAI','CHC4','CWNA','DONM','EKKA','FPAK','HPPY','KBAN','KKAW','KSWA','KVIL','LAK4','LAMB','LKAB','LKAE','LPDU','LUK2','MAHA','MCCS','MINB','MNKP','MPTN','MTNG','NAIN','NAWA','NJOK','NKAM','NKCS','NLCH','NMIN','ONTC','ONUT','PANT','PBSK','PINK','PKED','PNAM','POKW','PS43','PTNK','PTYA','PWET','PYSC','PYSL','RMA2','ROMK','RSIT','SAI4','SAMK','SAP2','SATU','SCON','SEC2','SKMT','SLYA','SMAI','SMUT','SNBN','SNDA','SNOI','SUAS','SUKS','TAC4','TAIT','TEPA','TKRU','TNON','TNPT','TPLU','TSIT','TTAI','TTLY','TUPM','TYA3','TYA6')
					--AND (i.package_id in ('PKG07', 'PKG03', 'PKG08', 'PKG04', 'PKG18', 'PKG05'))
					AND (CONVERT(VARCHAR(6),h.delivery_date,112) = (@Year*100 + @Month))
					--AND h.status IN (3, 4)
			) AS a
		) AS b
		GROUP BY b.ERP_ID, b.BranchID
		ORDER BY b.ERP_ID