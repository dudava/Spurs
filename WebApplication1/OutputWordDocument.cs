using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace CalculationsOfSpurs
{
    internal class OutputWordDocument
    {
        // Common

        int Bcolumn, Hcolumn;

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

        public OutputWordDocument(string savepath, string ibeamtype, int[] columnsize, double[] variables)
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

            CreateDocument();
            Console.WriteLine("Document has been created");
        }

        void CreateDocument()
        {
            object oEndOfDoc = "\\endofdoc";

            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            app.Visible = false;
            Microsoft.Office.Interop.Word.Document doc = app.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            Microsoft.Office.Interop.Word.Paragraph HeaderRussian;
            HeaderRussian = doc.Content.Paragraphs.Add(ref oMissing);
            HeaderRussian.Range.Text = "Расчёт шпоры";
            HeaderRussian.Range.Font.Name = "Times New Roman";
            HeaderRussian.Range.Font.Size = 20;
            HeaderRussian.Range.Font.Bold = 3;
            HeaderRussian.Format.SpaceAfter = 12;
            HeaderRussian.Format.LeftIndent = 2;
            HeaderRussian.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            HeaderRussian.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Paragraph HeaderEnglish;
            oRange = doc.Paragraphs[doc.Paragraphs.Count].Range;
            HeaderEnglish = doc.Content.Paragraphs.Add(ref oRange);
            HeaderEnglish.Range.Text = "Calculation of a spur";
            HeaderEnglish.Range.Font.Name = "Times New Roman";
            HeaderEnglish.Range.Font.Size = 20;
            HeaderEnglish.Range.Font.Bold = 3;
            HeaderEnglish.Format.SpaceAfter = 20;
            HeaderEnglish.Format.LeftIndent = 2;
            HeaderEnglish.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            HeaderEnglish.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Paragraph ConcreteChipping;
            oRange = doc.Paragraphs[doc.Paragraphs.Count].Range;
            ConcreteChipping = doc.Content.Paragraphs.Add(ref oRange);
            ConcreteChipping.Range.Text = "Расчёт шпоры на выкалывание";
            ConcreteChipping.Range.Font.Name = "Times New Roman";
            ConcreteChipping.Range.Font.Size = 18;
            ConcreteChipping.Range.Font.Bold = 3;
            ConcreteChipping.Format.SpaceBefore = 14;
            ConcreteChipping.Format.SpaceAfter = 18;
            ConcreteChipping.Format.LeftIndent = 0;
            ConcreteChipping.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            ConcreteChipping.Range.InsertParagraphAfter();


            InsertAtEndBasicText(doc, $"Ж/б колонна сечением {Bcolumn*1000}x{Hcolumn*1000} мм");                                                               // Дописать
            InsertAtEndBasicText(doc, ("Шпора - двутавр " + ibeamtype));                                                                             // Дописать
            InsertAtEndBasicText(doc, $"Максимальное сдвигающее усилие на шпору из расчетной схемы - {Qf} т");                                      // Дописать

            InsertAtEndBasicText(doc, "Проверка бетона на выкалывание будет проходить согласно СП63.13330 п.8.1.47. " +
                                      "Расчет элементов без поперечной арматуры при действии сосредоточенной " +
                                      "силы производится из условия:");
            InsertAtEndMath(doc, "F ⩽ F_bult");
            InsertAtEndBasicText(doc, "где F – сосредоточенная сила от внешней нагрузки;");
            InsertAtEndBasicText(doc, "Fb,ult – предельное усилие воспринимаемое бетоном;");
            InsertAtEndBasicText(doc, "Усилие Fb,ult определяют по формуле:");
            InsertAtEndMath(doc, "F_bult = R_bt ∙ A_b");
            InsertAtEndBasicText(doc, "где Ab –площадь расчетного поперечного сечения," +
                                      " расположенного на расстоянии 0,5 h0 от границы " +
                                      "площади приложения сосредоточенной силы F с рабочей " +
                                      "высотой сечения h0.");
            InsertAtEndBasicText(doc, "Площадь Ab определяют по формуле:");
            InsertAtEndMath(doc, "A_b = u ∙ h_0");
            InsertAtEndBasicText(doc, "где u — периметр контура расчетного поперечного сечения;");
            InsertAtEndBasicText(doc, "h₀ — приведенная рабочая высота сечения ;");
            InsertAtEndMath(doc, "h_0 = 0,5 ∙ (h_0x + h_0y)");
            InsertAtEndBasicText(doc, "Исходные данные для расчёта", Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter);
            InsertAtEndBasicText(doc, $"hox = {Hox} м;");
            InsertAtEndBasicText(doc, $"hoy = {Hoy} м;");
            InsertAtEndBasicText(doc, $"a = {a} м;");
            InsertAtEndBasicText(doc, $"b = {b} м;");
            InsertAtEndBasicText(doc, $"Rbt = {Hoy} т/м²;");
            InsertAtEndBasicText(doc, $"γb1 = {Gammab1};");
            InsertAtEndBasicText(doc, $"γb3 = {Gammab3};");
            InsertAtEndBasicText(doc, "Расчёт:", Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter);

            InsertAtEndMath(doc, $"{h0} = 0,5∙({Hox}+{Hoy});");
            InsertAtEndMath(doc, $"u = 2∙({b}+{a}+(3∙{h0})/2);");
            InsertAtEndMath(doc, $"{Ab} = {h0} ∙ {nu};");
            InsertAtEndMath(doc, $"{Fbult} = {Rbt} ∙ {Ab} ∙ {Gammab1} ∙ {Gammab3};");
            InsertAtEndBasicText(doc, $"Допустимая горизонтальная нагрузка на оголовок ж/б колонны {Fbult} т.");

            Microsoft.Office.Interop.Word.Paragraph SpurDepth;
            oRange = doc.Paragraphs[doc.Paragraphs.Count].Range;
            SpurDepth = doc.Content.Paragraphs.Add(ref oRange);
            SpurDepth.Range.Text = "Расчёт минимальной глубины заделки шпоры";
            SpurDepth.Range.Font.Name = "Times New Roman";
            SpurDepth.Range.Font.Size = 18;
            SpurDepth.Range.Font.Bold = 3;
            SpurDepth.Format.SpaceBefore = 14;
            SpurDepth.Format.SpaceAfter = 18;
            SpurDepth.Format.LeftIndent = 0;
            SpurDepth.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            SpurDepth.Range.InsertParagraphAfter();



            InsertAtEndBasicText(doc, $"Примем глубину заделки {a * 1000} мм. Монтажную подливку в расчет не принимаем, " +
                                      $"соответственно нагрузка на шпору приходит на расстоянии равным {c * 1000} мм.");
            InsertAtEndBasicText(doc, "Для расчета минимальной допустимой глубины заделки шпоры воспользуемся формулой (9.16):");
            InsertAtEndMath(doc, "Q ⩽ (abR_c)/((6e_0)/a + 1),");
            InsertAtEndBasicText(doc, "где Q - расчётная нагрузка от веса балки и приложенных к ней нагрузок;");
            InsertAtEndBasicText(doc, "Rc - расчётное сопротивление кладки при смятии;");
            InsertAtEndBasicText(doc, "a - глубина заделки балки в кладку;");
            InsertAtEndBasicText(doc, "b - ширина полок балки;");
            InsertAtEndBasicText(doc, "e₀ - эксцентриситет расчётной силы относительно середины заделки по формуле:");
            InsertAtEndMath(doc, "e_0 = c + a/2,");
            InsertAtEndBasicText(doc, "где c - расстояние силы Q от плоскости стены.");
            InsertAtEndBasicText(doc, "В качестве Rc для нашего случая примем Rb,loc - расчетное сопротивление" +
                                      " бетона сжатию при местном действии сжимающей силы согласно п.8.1.44 СП63.13330.2018:");
            InsertAtEndMath(doc, "R_bloc = φ_b∙R_b,");
            InsertAtEndBasicText(doc, "где φb определяется по формуле:");
            InsertAtEndMath(doc, "φ_b = 0,8∙√(A_bmax/A_bloc);");
            InsertAtEndBasicText(doc, "a1 = b;");
            InsertAtEndBasicText(doc, "a2 = a/2;");
            InsertAtEndBasicText(doc, "согласно эпюрам сжимающих напряжение по рисунку из СП15.13330.2020:");
            InsertAtEndMath(doc, "A_bmax = (a_2 + a_1 + a_2)∙a_2;");
            InsertAtEndMath(doc, "A_bloc = a_1∙a_2;");

            InsertAtEndBasicText(doc, "Исходные данные для расчёта", Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter);
            InsertAtEndBasicText(doc, $"Ж/б колонна сечением {Bcolumn*1000}x{Hcolumn*1000} мм");                                                               // Дописать
            InsertAtEndBasicText(doc, ("Шпора - двутавр" + ibeamtype));                                                                             // Дописать
            InsertAtEndBasicText(doc, $"Максимальное сдвигающее усилие на шпору из расчетной схемы - {Qf} т");                                      // Дописать

            InsertAtEndBasicText(doc, "В связи с отсутствием методики минимальной глубины заделки шпоры" +
                                      " в бетон, воспользуемся методикой согласно СП15.13330.2020 «Каменные" +
                                      " и армокаменные конструкции» с применением в расчетах расчетных характеристик бетона.");
            InsertAtEndBasicText(doc, "Расчёт:", Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter);

            InsertAtEndBasicText(doc, $"a = {a} м;");
            InsertAtEndBasicText(doc, $"b = {b} м;");
            InsertAtEndBasicText(doc, $"c = {c} м;");
            InsertAtEndBasicText(doc, $"Qf = {Qf} тс;");
            InsertAtEndBasicText(doc, $"γb1 = {Gammab1};");
            InsertAtEndBasicText(doc, $"γb3 = {Gammab3};");
            InsertAtEndBasicText(doc, $"Rb = {Rb} т/м²;");

            InsertAtEndMath(doc, $"{e}={a / 2}+{c};");
            InsertAtEndMath(doc, $"{Abloc} = {b} ∙ {a / 2};");
            InsertAtEndMath(doc, $"{Abmax} = 2 ∙ {a / 2}² + {Abloc};");
            InsertAtEndMath(doc, $"{Rloc} = {Rb} ∙ {fib} ∙ {Gammab1} ∙ {Gammab3};");
            InsertAtEndMath(doc, $"{fib} = 0,8 ∙ √({Abmax}/{Abloc});");
            InsertAtEndMath(doc, $"{Qult} = ({Rloc} ∙ {a} ∙ {b})/(6∙{e}/{a}+1);");
            InsertAtEndBasicText(doc, $"В соответствии с формулой (9.16) получаем максимально-допустимое усилие на шпору = {Qult} т, при ее условии заглубления на {a * 1000} мм.");
            InsertAtEndBasicText(doc, $"Максимально-допустимое усилие на шпору = {Qult} т больше усилия приходящего на шпору {Qf} т, согласно исходным данным. Глубина заделки шпоры – обеспечена.");


            SaveDocument(doc);
            app.Quit();
        }


        void InsertAtEndBasicText(Microsoft.Office.Interop.Word.Document doc, string str, Microsoft.Office.Interop.Word.WdParagraphAlignment basicalignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft)
        {
            object oMissing = System.Reflection.Missing.Value;

            Microsoft.Office.Interop.Word.Paragraph paragraph;
            oRange = doc.Paragraphs[doc.Paragraphs.Count].Range;
            paragraph = doc.Content.Paragraphs.Add(ref oRange);
            paragraph.Range.Text = str;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 14;
            paragraph.Range.Font.Bold = 0;
            paragraph.Format.SpaceBefore = 6;
            paragraph.Format.SpaceAfter = 6;
            paragraph.Format.LeftIndent = 1;
            paragraph.Alignment = basicalignment;
            paragraph.Range.InsertParagraphAfter();
        }
        void InsertAtEndMath(Microsoft.Office.Interop.Word.Document doc, string str, Microsoft.Office.Interop.Word.WdParagraphAlignment basicalignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter)
        {
            object oMissing = System.Reflection.Missing.Value;

            Microsoft.Office.Interop.Word.Paragraph paragraph;
            oRange = doc.Paragraphs[doc.Paragraphs.Count].Range;
            paragraph = doc.Content.Paragraphs.Add(ref oRange);
            paragraph.Range.Text = str;
            paragraph.Range.Font.Name = "Cambria Math";
            paragraph.Range.Font.Size = 16;
            paragraph.Range.Font.Bold = 0;
            paragraph.Format.SpaceBefore = 10;
            paragraph.Format.SpaceAfter = 10;
            paragraph.Format.LeftIndent = 1;
            paragraph.Alignment = basicalignment;
            doc.OMaths.Add(paragraph.Range).OMaths.BuildUp();
            paragraph.Range.InsertParagraphAfter();
        }

        void SaveDocument(Microsoft.Office.Interop.Word.Document doc)
        {
            doc.SaveAs2(savepath, oMissing, oMissing, oMissing, oMissing, oMissing,
                        oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                        oMissing, oMissing, oMissing, oMissing, oMissing);
        }
    }
}
