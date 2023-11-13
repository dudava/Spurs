using CalculationsOfSpurs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocsController : ControllerBase
    {
        [HttpPost]
        public void Post()
        {
            Request request = new Request();

            request.columnsize[0] = Convert.ToDouble(Request.Form["column_length"].ToString().Replace(".", ","));//.Replace(",", "."));
            request.columnsize[1] = Convert.ToDouble(Request.Form["column_width"].ToString().Replace(".", ","));

            request.ibeamtype = Request.Form["tipe_I_beam"].ToString();
            request.concretetype = Request.Form["concrete_grade"].ToString();

            request.changerequirements[0] = Convert.ToBoolean(Request.Form["change_type_I_beam"].ToString());
            request.changerequirements[0] = Convert.ToBoolean(Request.Form["change_value_sealing_spur"].ToString());

            request.variables[7] = Convert.ToDouble(Request.Form["hox"].ToString().Replace(".", ","));
            request.variables[8] = Convert.ToDouble(Request.Form["hoy"].ToString().Replace(".", ","));
            request.variables[1] = Convert.ToDouble(Request.Form["yb1"].ToString().Replace(".", ","));
            request.variables[17] = Convert.ToDouble(Request.Form["amount_concrete_gravy"].ToString().Replace(".", ","));
            request.variables[0] = Convert.ToDouble(Request.Form["shear_force"].ToString().Replace(".", ","));
            request.variables[2] = Convert.ToDouble(Request.Form["value_sealing_spur"].ToString().Replace(".", ","));

            Spur spur = new Spur(request);
            Response.SendFileAsync((new PhysicalFileProvider(Directory.GetCurrentDirectory())).GetFileInfo("calculation_note.doc"));
            //return spur.GetDocument();


        } 

    }

    public class Request
    {
        public double[] columnsize = new double[2];
        public double[] variables = new double[18];
        public string ibeamtype, concretetype;
        public bool[] changerequirements = new bool[2];
    }
}
