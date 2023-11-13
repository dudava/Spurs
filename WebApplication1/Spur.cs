using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Controllers;

namespace CalculationsOfSpurs
{
    public class Spur
    {
        double[] columnsize = new double[2];

        double[] variables = new double[18]; //variables that gets while procceding calculation

        string ibeamtype, concretetype;

        bool[] changerequirements = new bool[2];

        string savepath = @$"{Environment.CurrentDirectory}\\calculation_note.doc";

        public Spur(Request request)
        {
            columnsize = request.columnsize;
            variables = request.variables;
            ibeamtype = request.ibeamtype;
            concretetype = request.concretetype;
            changerequirements = request.changerequirements;

            Calculation calculation = new Calculation(concretetype, ibeamtype, variables, changerequirements);
            ibeamtype = calculation.Getibeamtype();
            variables = calculation.GetDataVariables();
            OutputWordDocument outputWordDocument = new OutputWordDocument(savepath, ibeamtype, columnsize, variables);
        }

        public byte[] GetDocument()
        {
            return System.IO.File.ReadAllBytes(savepath);
        }

    }
}
