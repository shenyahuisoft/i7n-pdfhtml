/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2017 iText Group NV
    Authors: iText Software.

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License version 3
    as published by the Free Software Foundation with the addition of the
    following permission added to Section 15 as permitted in Section 7(a):
    FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
    ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
    OF THIRD PARTY RIGHTS

    This program is distributed in the hope that it will be useful, but
    WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
    or FITNESS FOR A PARTICULAR PURPOSE.
    See the GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License
    along with this program; if not, see http://www.gnu.org/licenses or write to
    the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
    Boston, MA, 02110-1301 USA, or download the license from the following URL:
    http://itextpdf.com/terms-of-use/

    The interactive user interfaces in modified source and object code versions
    of this program must display Appropriate Legal Notices, as required under
    Section 5 of the GNU Affero General Public License.

    In accordance with Section 7(b) of the GNU Affero General Public License,
    a covered work must retain the producer line in every PDF that is created
    or manipulated using iText.

    You can be released from the requirements of the license by purchasing
    a commercial license. Buying such a license is mandatory as soon as you
    develop commercial activities involving the iText software without
    disclosing the source code of your own applications.
    These activities include: offering paid services to customers as an ASP,
    serving PDFs on the fly in a web application, shipping iText with a closed
    source product.

    For more information, please contact iText Software Corp. at this
    address: sales@itextpdf.com */
using System;
using System.Collections.Generic;
using iText.Forms.Fields;
using iText.Html2pdf.Attach.Impl.Layout.Form.Element;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Html2pdf.Attach.Impl.Layout.Form.Renderer {
    public abstract class AbstractTextFieldRenderer : AbstractFormFieldRenderer {
        protected internal PdfFont font;

        protected internal AbstractTextFieldRenderer(IFormField modelElement)
            : base(modelElement) {
        }

        //Class that represents form field based on text content.
        protected internal virtual IRenderer CreateParagraphRenderer(String defaultValue) {
            if (String.IsNullOrEmpty(defaultValue.Trim())) {
                // TODO: change to 'defaultValue = "\u00A0"' after trimming of non-breakable spaces is fixed;
                defaultValue = "\u00B7";
            }
            return new Paragraph(defaultValue).SetMultipliedLeading(1).SetMargin(0).CreateRendererSubTree();
        }

        protected internal virtual void ApplyDefaultFieldProperties(PdfFormField inputField) {
            inputField.GetWidgets()[0].SetHighlightMode(PdfName.N);
            inputField.SetBorderWidth(0);
            TransparentColor color = GetPropertyAsTransparentColor(Property.FONT_COLOR);
            if (color != null) {
                inputField.SetColor(color.GetColor());
            }
            inputField.SetDefaultValue(new PdfString(GetDefaultValue()));
        }

        protected internal virtual void UpdatePdfFont(ParagraphRenderer renderer) {
            Object retrievedFont;
            if (renderer != null) {
                IList<LineRenderer> lines = renderer.GetLines();
                if (lines != null) {
                    foreach (LineRenderer line in lines) {
                        foreach (IRenderer child in line.GetChildRenderers()) {
                            retrievedFont = child.GetProperty<PdfFont>(Property.FONT);
                            if (retrievedFont is PdfFont) {
                                font = (PdfFont)retrievedFont;
                                return;
                            }
                        }
                    }
                }
            }
            retrievedFont = renderer.GetProperty<PdfFont>(Property.FONT);
            if (retrievedFont is PdfFont) {
                font = (PdfFont)retrievedFont;
            }
        }
    }
}