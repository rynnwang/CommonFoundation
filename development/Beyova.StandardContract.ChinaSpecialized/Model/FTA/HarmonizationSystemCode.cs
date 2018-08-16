namespace Beyova.ChinaSpecialized
{
    /// <summary>
    /// Class HarmonizationSystemCode. DB model.
    /// </summary>
    /// <seealso cref="Beyova.IGlobalObjectName" />
    /// <seealso cref="Beyova.IIdentifier" />
    public class HarmonizationSystemCode : HarmonizationSystemCodeEssential, IIdentifier, IGlobalObjectName
    {
        #region Duty Rates

        /// <summary>
        /// Gets or sets the MFN duty rate. 最惠国关税%
        /// </summary>
        /// <value>
        /// The MFN duty rate.
        /// </value>
        public double? MFNDutyRate { get; set; }

        /// <summary>
        /// Gets or sets the general duty rate. 一般关税%
        /// </summary>
        /// <value>
        /// The general duty rate.
        /// </value>
        public double? GeneralDutyRate { get; set; }

        /// <summary>
        /// Gets or sets the provisional duty rate. 暂定关税%
        /// </summary>
        /// <value>
        /// The provisional duty rate.
        /// </value>
        public double? ProvisionalDutyRate { get; set; }

        /// <summary>
        /// Gets or sets the export rebate rate. 出口退税%
        /// </summary>
        /// <value>
        /// The export rebate rate.
        /// </value>
        public double? ExportRebateRate { get; set; }

        /// <summary>
        /// Gets or sets the export duty rate. 出口税率%
        /// </summary>
        /// <value>
        /// The export duty rate.
        /// </value>
        public double? ExportDutyRate { get; set; }

        /// <summary>
        /// Gets or sets the export provisional duty rate. 出口暂定税率
        /// </summary>
        /// <value>
        /// The export provisional duty rate.
        /// </value>
        public double? ExportProvisionalDutyRate { get; set; }

        /// <summary>
        /// Gets or sets the VAT rate. 增值税
        /// </summary>
        /// <value>
        /// The vat rate.
        /// </value>
        public double? VATRate { get; set; }

        /// <summary>
        /// Gets or sets the GST rate. 消费税
        /// </summary>
        /// <value>
        /// The GST rate.
        /// </value>
        public double? GSTRate { get; set; }

        /// <summary>
        /// Gets or sets the countervailing duty rate. 反补贴税
        /// </summary>
        /// <value>
        /// The countervailing duty rate.
        /// </value>
        public double? CountervailingDutyRate { get; set; }

        /// <summary>
        /// Gets or sets the anti dumping duty rate. 反倾销税
        /// </summary>
        /// <value>
        /// The anti dumping duty rate.
        /// </value>
        public double? AntiDumpingDutyRate { get; set; }

        #endregion Duty Rates

        /// <summary>
        /// Gets or sets the measurement unit. 计量单位
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the supervision. 监管要求
        /// </summary>
        /// <value>
        /// The supervision.
        /// </value>
        public string Supervision { get; set; }

        /// <summary>
        /// Gets or sets the CIQ. Inspection &amp; Quarantine: 检验检疫
        /// </summary>
        /// <value>
        /// The ciq.
        /// </value>
        public string CIQ { get; set; }

        /// <summary>
        /// Gets or sets the search full term.
        /// </summary>
        /// <value>
        /// The search full term.
        /// </value>
        public string SearchFullTerm { get; set; }

        /// <summary>
        /// Gets or sets the search short term.
        /// </summary>
        /// <value>
        /// The search short term.
        /// </value>
        public string SearchShortTerm { get; set; }

        #region Duty + Tax description

        /// <summary>
        /// Gets or sets the MFN duty description.
        /// </summary>
        /// <value>
        /// The MFN duty description.
        /// </value>
        public string MFNDutyDescription { get; set; }

        /// <summary>
        /// Gets or sets the general duty rate. 一般关税%
        /// </summary>
        /// <value>
        /// The general duty rate.
        /// </value>
        public string GeneralDutyDescription { get; set; }

        /// <summary>
        /// Gets or sets the provisional duty description. 暂定关税
        /// </summary>
        /// <value>
        /// The provisional duty description.
        /// </value>
        public string ProvisionalDutyDescription { get; set; }

        /// <summary>
        /// Gets or sets the export rebate description. 出口补贴
        /// </summary>
        /// <value>
        /// The export rebate description.
        /// </value>
        public string ExportRebateDescription { get; set; }

        /// <summary>
        /// Gets or sets the export duty rate description. 出口税率
        /// </summary>
        /// <value>
        /// The export duty rate description.
        /// </value>
        public string ExportDutyDescription { get; set; }

        /// <summary>
        /// Gets or sets the export provisional duty rate description. 出口暂定税率
        /// </summary>
        /// <value>
        /// The export provisional duty rate description.
        /// </value>
        public string ExportProvisionalDutyDescription { get; set; }

        /// <summary>
        /// Gets or sets the GST description. 消费税
        /// </summary>
        /// <value>
        /// The GST description.
        /// </value>
        public string GSTDescription { get; set; }

        /// <summary>
        /// Gets or sets the vat description. 增值税
        /// </summary>
        /// <value>
        /// The vat description.
        /// </value>
        public string VATDescription { get; set; }

        /// <summary>
        /// Gets or sets the countervailing duty description. 反补贴
        /// </summary>
        /// <value>
        /// The countervailing duty description.
        /// </value>
        public string CountervailingDutyDescription { get; set; }

        /// <summary>
        /// Gets or sets the anti dumping duty description. 反倾销税
        /// </summary>
        /// <value>
        /// The anti dumping duty description.
        /// </value>
        public string AntiDumpingDutyDescription { get; set; }


        #endregion Duty + Tax description
    }
}