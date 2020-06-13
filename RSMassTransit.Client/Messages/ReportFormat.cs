/*
    Copyright (C) 2020 Jeffrey Sharp

    Permission to use, copy, modify, and distribute this software for any
    purpose with or without fee is hereby granted, provided that the above
    copyright notice and this permission notice appear in all copies.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
    WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
    MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
    ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
    WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
    ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
    OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

namespace RSMassTransit.Messages
{
    // Rendering extensions reported by a SSRS 2016 installation:
    // https://docs.microsoft.com/en-us/sql/reporting-services/report-builder/export-reports-report-builder-and-ssrs
    // https://docs.microsoft.com/en-us/sql/reporting-services/report-server/rsreportserver-config-configuration-file#bkmk_rendering
    // https://msdn.microsoft.com/en-us/library/ff420717(v=sql.105).aspx
    //
    // Name         Description             'Vis'?  Notes
    // -----------  ------------------      ------  -----
    // WORDOPENXML  Word
    // WORD         Word 97-2003            False
    // EXCELOPENXML Excel
    // EXCEL        Excel 97-2003           False
    // PPTX         PowerPoint
    // PDF          PDF
    // IMAGE        TIFF
    // HTML4.0      HTML 4.0                False
    // HTML5        HTML 5                  False
    // MHTML        Web archive
    // CSV          Comma-separated values
    // XML          Data-only XML
    // ATOM         List of Atom feeds
    // NULL         Null renderer           False   Probably a renderer that doesn't render anything.
    // RGDI         Remote GDI+             False   Used by SSRS when it communicates with viewer controls to offload rendering to the client.
    // RPL          Report Page Layout      False   Used by SSRS when it communicates with viewer controls to offload rendering to the client.

    /// <summary>
    ///   Supported report output formats.
    /// </summary>
    public enum ReportFormat
    {
        /// <summary>
        ///   Word 2007+ (Open XML) format.
        ///   Uses the 'WORDOPENXML' rendering extension.
        /// </summary>
        Word,

        /// <summary>
        ///   Word 97-2003 format.
        ///   Uses the 'WORD' rendering extension.
        /// </summary>
        WordLegacy,

        /// <summary>
        ///   Excel 2007+ (Open XML) format.
        ///   Uses the 'EXCELOPENXML' rendering extension.
        /// </summary>
        Excel,

        /// <summary>
        ///   Excel 97-2003 format.
        ///   Uses the 'EXCEL' rendering extension.
        /// </summary>
        ExcelLegacy,

        /// <summary>
        ///   PowerPoint 2007+ (Open XML) format.
        ///   Uses the 'PPTX' rendering extension.
        /// </summary>
        PowerPoint,

        /// <summary>
        ///   Portable Document Format
        ///   Uses the 'PDF' rendering extension.
        /// </summary>
        Pdf,

        /// <summary>
        ///   TIFF image format.
        ///   Uses the 'IMAGE' rendering extension.
        /// </summary>
        Tiff,

        /// <summary>
        ///   HTML 4.0 format.
        ///   Uses the 'HTML4.0' rendering extension.
        /// </summary>
        Html4,

        /// <summary>
        ///   HTML 5 format.
        ///   Uses the 'HTML5' rendering extension.
        /// </summary>
        Html5,

        /// <summary>
        ///   MHTML (web archive) format.
        ///   Uses the 'MHTML' rendering extension.
        /// </summary>
        Mhtml,

        /// <summary>
        ///   Comma-separated values format.
        ///   Uses the 'CSV' rendering extension.
        /// </summary>
        Csv,

        /// <summary>
        ///   Data-only XML format.
        ///   Uses the 'XML' rendering extension.
        /// </summary>
        Xml,

        // Not supported:
        // * Atom, because of complexity
        // * Null, because it is unlikely to be useful
        // * Rgdi, because it is unlikely to be useful
        // * Rpl,  because it is unlikely to be useful
    }
}
