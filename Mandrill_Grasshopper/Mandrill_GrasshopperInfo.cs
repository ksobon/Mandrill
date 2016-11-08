using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;

namespace Mandrill_Grasshopper
{
    public class Mandrill_GrasshopperInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Mandrill";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return Resources.mandrillLogo;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "D3.js based data visualization library.";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("6028291a-a446-4338-811b-54e631b2b2c8");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Konrad K Sobon";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "sobon.konrad@gmail.com";
            }
        }
    }
}
