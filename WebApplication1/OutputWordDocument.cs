using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace CalculationsOfSpurs
{
    internal class OutputWordDocument
    {
        // Common

        double Bcolumn, Hcolumn;

        double Qf, Gammab1, a, b, h0;

        const double Gammab3 = 0.85;

        // ConcreateChippingCheck

        string ibeamtype;

        double Qult, nu, Hox, Hoy, Ab, Rbt;

        // MinimalSpurDepth

        double Fbult, Rb, Abloc, Abmax, Rloc, fib, c, e;

        //

        string savepath;

        // Word variables

        object oMissing = System.Reflection.Missing.Value;
        object oRange;

        public OutputWordDocument(string savepath, string ibeamtype, double[] columnsize, double[] variables)
        {
            this.savepath = savepath;
            
            // Common

            Bcolumn = columnsize[0];
            Hcolumn = columnsize[1];

            Qf       = variables[0];
            Gammab1  = variables[1];
            a        = variables[2];
            b        = variables[3];
            h0       = variables[4];

            // ConcreateChippingCheck

            this.ibeamtype = ibeamtype;

            Qult     = variables[5];
            nu       = variables[6];
            Hox      = variables[7];
            Hoy      = variables[8];
            Ab       = variables[9];
            Rbt      = variables[10];

            // MinimalSpurDepth

            Fbult    = variables[11];
            Rb       = variables[12];
            Abloc    = variables[13];
            Abmax    = variables[14];
            Rloc     = variables[15];
            fib      = variables[16];
            c        = variables[17]; 
            e        = variables[18];

            Console.WriteLine("all fine");
            CreateDocument();
            Console.WriteLine("Document has been created");
        }

        void CreateDocument()
        {
            Console.WriteLine("here is a problem");
            object oEndOfDoc = "\\endofdoc";



        WordDocument doc = new WordDocument();
            WSection section = doc.AddSection() as WSection;
            section.PageSetup.Margins.All = 72;
            section.PageSetup.PageSize = new Syncfusion.Drawing.SizeF(612, 792);

            WParagraphStyle styleHeaderRussian = doc.AddParagraphStyle("HeaderRussian") as WParagraphStyle;
            WParagraphStyle styleHeaderEnglish = doc.AddParagraphStyle("HeaderEnglish") as WParagraphStyle;
            IWParagraph paragraph = section.AddParagraph();

            styleHeaderRussian.CharacterFormat.FontName = "Times New Roman";
            styleHeaderRussian.CharacterFormat.FontSize = 20f;
            styleHeaderRussian.CharacterFormat.Bold = true;
            styleHeaderRussian.ParagraphFormat.AfterSpacing = 12;
            styleHeaderRussian.ParagraphFormat.LeftIndent = 2;
            styleHeaderRussian.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
            paragraph.ApplyStyle("HeaderRussian");
            WTextRange textRange = paragraph.AppendText("Расчёт шпоры") as WTextRange;

            styleHeaderEnglish.CharacterFormat.FontName = "Times New Roman";
            styleHeaderEnglish.CharacterFormat.FontSize = 20f;
            styleHeaderEnglish.CharacterFormat.Bold = true;
            styleHeaderEnglish.ParagraphFormat.AfterSpacing = 20;
            styleHeaderEnglish.ParagraphFormat.LeftIndent = 2;
            styleHeaderEnglish.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
            paragraph = section.AddParagraph();
            paragraph.ApplyStyle("HeaderEnglish");
            textRange = paragraph.AppendText("Calculation of a spur") as WTextRange;


            WParagraphStyle paragraphStyle = doc.AddParagraphStyle("StandartParagraph") as WParagraphStyle;

            paragraphStyle.CharacterFormat.FontName = "Times New Roman";
            paragraphStyle.CharacterFormat.FontSize = 14;
            paragraphStyle.CharacterFormat.Bold = false;
            paragraphStyle.ParagraphFormat.BeforeSpacing = 6;
            paragraphStyle.ParagraphFormat.AfterSpacing = 6;
            paragraphStyle.ParagraphFormat.LeftIndent = 1;
            paragraphStyle.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;

            InsertAtEndBasicText(section.AddParagraph(), $"Ж/б колонна сечением {Bcolumn * 1000}x{Hcolumn * 1000} мм");                                              
            InsertAtEndBasicText(section.AddParagraph(), ("Шпора - двутавр " + ibeamtype));                                                     
            InsertAtEndBasicText(section.AddParagraph(), $"Максимальное сдвигающее усилие на шпору из расчетной схемы - {Qf} т");   

            InsertAtEndBasicText(section.AddParagraph(), "Проверка бетона на выкалывание будет проходить согласно СП63.13330 п.8.1.47. " +
                                      "Расчет элементов без поперечной арматуры при действии сосредоточенной " +
                                      "силы производится из условия:");
            //InsertAtEndMath(doc, "F ⩽ F_bult");
            InsertAtEndBasicText(section.AddParagraph(), "где F – сосредоточенная сила от внешней нагрузки;");
            InsertAtEndBasicText(section.AddParagraph(), "Fb,ult – предельное усилие воспринимаемое бетоном;");
            InsertAtEndBasicText(section.AddParagraph(), "Усилие Fb,ult определяют по формуле:");
            //(doc, "F_bult = R_bt ∙ A_b");
            InsertAtEndBasicText(section.AddParagraph(), "где Ab –площадь расчетного поперечного сечения," +
                                      " расположенного на расстоянии 0,5 h0 от границы " +
                                      "площади приложения сосредоточенной силы F с рабочей " +
                                      "высотой сечения h0.");
            InsertAtEndBasicText(section.AddParagraph(), "Площадь Ab определяют по формуле:");
            //InsertAtEndMath(doc, "A_b = u ∙ h_0");
            InsertAtEndBasicText(section.AddParagraph(), "где u — периметр контура расчетного поперечного сечения;");
            InsertAtEndBasicText(section.AddParagraph(), "h₀ — приведенная рабочая высота сечения ;");
            //InsertAtEndMath(doc, "h_0 = 0,5 ∙ (h_0x + h_0y)");
            InsertAtEndBasicText(section.AddParagraph(), "Исходные данные для расчёта");//, Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter);
            InsertAtEndBasicText(section.AddParagraph(), $"hox = {Hox} м;");
            InsertAtEndBasicText(section.AddParagraph(), $"hoy = {Hoy} м;");
            InsertAtEndBasicText(section.AddParagraph(), $"a = {a} м;");
            InsertAtEndBasicText(section.AddParagraph(), $"b = {b} м;");
            InsertAtEndBasicText(section.AddParagraph(), $"Rbt = {Hoy} т/м²;");
            InsertAtEndBasicText(section.AddParagraph(), $"γb1 = {Gammab1};");
            InsertAtEndBasicText(section.AddParagraph(), $"γb3 = {Gammab3};");
            InsertAtEndBasicText(section.AddParagraph(), "Расчёт:");//, Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter);

            //InsertAtEndMath(doc, $"{h0} = 0,5∙({Hox}+{Hoy});");
            //InsertAtEndMath(doc, $"u = 2∙({b}+{a}+(3∙{h0})/2);");
            //InsertAtEndMath(doc, $"{Ab} = {h0} ∙ {nu};");
            //InsertAtEndMath(doc, $"{Fbult} = {Rbt} ∙ {Ab} ∙ {Gammab1} ∙ {Gammab3};");
            InsertAtEndBasicText(section.AddParagraph(), $"Допустимая горизонтальная нагрузка на оголовок ж/б колонны {Fbult} т.");


            WParagraphStyle styleHeaderSpurDepth = doc.AddParagraphStyle("styleHeaderSpurDepth") as WParagraphStyle;
            styleHeaderSpurDepth.CharacterFormat.FontName = "Times New Roman";
            styleHeaderSpurDepth.CharacterFormat.FontSize = 18f;
            styleHeaderSpurDepth.CharacterFormat.Bold = true;
            styleHeaderSpurDepth.ParagraphFormat.BeforeSpacing = 14;
            styleHeaderSpurDepth.ParagraphFormat.AfterSpacing = 18;
            styleHeaderSpurDepth.ParagraphFormat.LeftIndent = 0;
            styleHeaderSpurDepth.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
            paragraph = section.AddParagraph();
            paragraph.ApplyStyle("styleHeaderSpurDepth");
            textRange = paragraph.AppendText("Расчёт минимальной глубины заделки шпоры") as WTextRange;





            InsertAtEndBasicText(section.AddParagraph(), $"Примем глубину заделки {a * 1000} мм. Монтажную подливку в расчет не принимаем, " +
                                      $"соответственно нагрузка на шпору приходит на расстоянии равным {c * 1000} мм.");
            InsertAtEndBasicText(section.AddParagraph(), "Для расчета минимальной допустимой глубины заделки шпоры воспользуемся формулой (9.16):");
            //InsertAtEndMath(doc, "Q ⩽ (abR_c)/((6e_0)/a + 1),");
            InsertAtEndBasicText(section.AddParagraph(), "где Q - расчётная нагрузка от веса балки и приложенных к ней нагрузок;");
            InsertAtEndBasicText(section.AddParagraph(), "Rc - расчётное сопротивление кладки при смятии;");
            InsertAtEndBasicText(section.AddParagraph(), "a - глубина заделки балки в кладку;");
            InsertAtEndBasicText(section.AddParagraph(), "b - ширина полок балки;");
            InsertAtEndBasicText(section.AddParagraph(), "e₀ - эксцентриситет расчётной силы относительно середины заделки по формуле:");
            //InsertAtEndMath(doc, "e_0 = c + a/2,");
            InsertAtEndBasicText(section.AddParagraph(), "где c - расстояние силы Q от плоскости стены.");
            InsertAtEndBasicText(section.AddParagraph(), "В качестве Rc для нашего случая примем Rb,loc - расчетное сопротивление" +
                                      " бетона сжатию при местном действии сжимающей силы согласно п.8.1.44 СП63.13330.2018:");
            //InsertAtEndMath(doc, "R_bloc = φ_b∙R_b,");
            InsertAtEndBasicText(section.AddParagraph(), "где φb определяется по формуле:");
            //InsertAtEndMath(doc, "φ_b = 0,8∙√(A_bmax/A_bloc);");
            InsertAtEndBasicText(section.AddParagraph(), "a1 = b;");
            InsertAtEndBasicText(section.AddParagraph(), "a2 = a/2;");
            InsertAtEndBasicText(section.AddParagraph(), "согласно эпюрам сжимающих напряжение по рисунку из СП15.13330.2020:");
            //InsertAtEndMath(doc, "A_bmax = (a_2 + a_1 + a_2)∙a_2;");
            //InsertAtEndMath(doc, "A_bloc = a_1∙a_2;");

            InsertAtEndBasicText(section.AddParagraph(), "Исходные данные для расчёта");//, Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter);
            InsertAtEndBasicText(section.AddParagraph(), $"Ж/б колонна сечением {Bcolumn * 1000}x{Hcolumn * 1000} мм");                                                     
            InsertAtEndBasicText(section.AddParagraph(), ("Шпора - двутавр" + ibeamtype));                                                                   
            InsertAtEndBasicText(section.AddParagraph(), $"Максимальное сдвигающее усилие на шпору из расчетной схемы - {Qf} т");                                  

            InsertAtEndBasicText(section.AddParagraph(), "В связи с отсутствием методики минимальной глубины заделки шпоры" +
                                      " в бетон, воспользуемся методикой согласно СП15.13330.2020 «Каменные" +
                                      " и армокаменные конструкции» с применением в расчетах расчетных характеристик бетона.");
            InsertAtEndBasicText(section.AddParagraph(), "Расчёт:");//, Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter);

            InsertAtEndBasicText(section.AddParagraph(), $"a = {a} м;");
            InsertAtEndBasicText(section.AddParagraph(), $"b = {b} м;");
            InsertAtEndBasicText(section.AddParagraph(), $"c = {c} м;");
            InsertAtEndBasicText(section.AddParagraph(), $"Qf = {Qf} тс;");
            InsertAtEndBasicText(section.AddParagraph(), $"γb1 = {Gammab1};");
            InsertAtEndBasicText(section.AddParagraph(), $"γb3 = {Gammab3};");
            InsertAtEndBasicText(section.AddParagraph(), $"Rb = {Rb} т/м²;");

            //InsertAtEndMath(doc, $"{e}={a / 2}+{c};");
            //InsertAtEndMath(doc, $"{Abloc} = {b} ∙ {a / 2};");
            //InsertAtEndMath(doc, $"{Abmax} = 2 ∙ {a / 2}² + {Abloc};");
            //InsertAtEndMath(doc, $"{Rloc} = {Rb} ∙ {fib} ∙ {Gammab1} ∙ {Gammab3};");
            //InsertAtEndMath(doc, $"{fib} = 0,8 ∙ √({Abmax}/{Abloc});");
            //InsertAtEndMath(doc, $"{Qult} = ({Rloc} ∙ {a} ∙ {b})/(6∙{e}/{a}+1);");
            InsertAtEndBasicText(section.AddParagraph(), $"В соответствии с формулой (9.16) получаем максимально-допустимое усилие на шпору = {Qult} т, при ее условии заглубления на {a * 1000} мм.");
            InsertAtEndBasicText(section.AddParagraph(), $"Максимально-допустимое усилие на шпору = {Qult} т больше усилия приходящего на шпору {Qf} т, согласно исходным данным. Глубина заделки шпоры – обеспечена.");


            SaveDocument(doc);
            //app.Quit();
        }


        void InsertAtEndBasicText(IWParagraph paragraph, string str)
        {
            paragraph.ApplyStyle("StandartParagraph");
            WTextRange textRange = paragraph.AppendText(str) as WTextRange;
        }

        //void InsertAtEndMath(Microsoft.Office.Interop.Word.Document doc, string str, Microsoft.Office.Interop.Word.WdParagraphAlignment basicalignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter)
        //{
        //    object oMissing = System.Reflection.Missing.Value;

        //    Microsoft.Office.Interop.Word.Paragraph paragraph;
        //    oRange = doc.Paragraphs[doc.Paragraphs.Count].Range;
        //    paragraph = doc.Content.Paragraphs.Add(ref oRange);
        //    paragraph.Range.Text = str;
        //    paragraph.Range.Font.Name = "Cambria Math";
        //    paragraph.Range.Font.Size = 16;
        //    paragraph.Range.Font.Bold = 0;
        //    paragraph.Format.SpaceBefore = 10;
        //    paragraph.Format.SpaceAfter = 10;
        //    paragraph.Format.LeftIndent = 1;
        //    paragraph.Alignment = basicalignment;
        //    doc.OMaths.Add(paragraph.Range).OMaths.BuildUp();
        //    paragraph.Range.InsertParagraphAfter();
        //}

        void SaveDocument(WordDocument doc)
        {
            FileStream outputStream = new FileStream(savepath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            doc.Save(outputStream, FormatType.Docx);
            doc.Close();
            outputStream.Dispose();
        }
    }
}
