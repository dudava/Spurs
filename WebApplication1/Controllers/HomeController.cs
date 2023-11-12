using CalculationsOfSpurs;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        public byte[] Post()
        {
            Request request = new Request();

            request.columnsize[0] = Convert.ToInt32(Request.Form["column_length"]);
            request.columnsize[1] = Convert.ToInt32(Request.Form["column_width"]);

            request.ibeamtype = Request.Form["tipe_I_beam"];
            request.concretetype = Request.Form["concrete_grade"];

            request.changerequirements[0] = Convert.ToBoolean(Request.Form["change_type_I_beam"]);
            request.changerequirements[0] = Convert.ToBoolean(Request.Form["change_value_sealing_spur"]);

            request.variables[0] = Convert.ToInt32(Request.Form["hox"]);
            request.variables[0] = Convert.ToInt32(Request.Form["hoy"]);
            request.variables[0] = Convert.ToInt32(Request.Form["yb"]);
            request.variables[0] = Convert.ToInt32(Request.Form["amount_concrete_gravy"]);
            request.variables[0] = Convert.ToInt32(Request.Form["shear_force"]);
            request.variables[0] = Convert.ToInt32(Request.Form["value_sealing_spur"]);

            Spur spur = new Spur(request);

            return spur.GetDocument();


        } 

    }

    public class Request
    {
        public int[] columnsize = new int[2];
        public double[] variables = new double[18];
        public string ibeamtype, concretetype;
        public bool[] changerequirements = new bool[2];
    }
}
