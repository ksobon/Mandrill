using Grasshopper.Kernel.Expressions;
using Grasshopper.Kernel.Types;
using Microsoft.VisualBasic.CompilerServices;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class GH_ValueListItem
    {
        private IGH_Goo m_value;

        [System.Runtime.CompilerServices.CompilerGenerated]
        private bool _Selected;

        [System.Runtime.CompilerServices.CompilerGenerated]
        private string _Name;

        [System.Runtime.CompilerServices.CompilerGenerated]
        private string _Expression;

        [System.Runtime.CompilerServices.CompilerGenerated]
        private System.Drawing.RectangleF _BoxName;

        [System.Runtime.CompilerServices.CompilerGenerated]
        private System.Drawing.RectangleF _BoxLeft;

        [System.Runtime.CompilerServices.CompilerGenerated]
        private System.Drawing.RectangleF _BoxRight;

        private const int BoxWidth = 22;

        /// <summary>
        ///  Gets or sets whether this item is selected.
        ///  </summary>
        public bool Selected
        {
            get { return this._Selected; }
            set { this._Selected = value; }
        }

        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        /// <summary>
        ///  Gets or sets the expression of this constant. 
        ///  If you set the expression through this property you must also expire the Value.
        ///  </summary>
        public string Expression
        {
            get { return this._Expression; }
            set { this._Expression = value; }
        }

        public System.Drawing.RectangleF BoxName
        {
            get { return this._BoxName; }
            set { this._BoxName = value; }
        }

        /// <summary>
        ///  Gets or sets the bounds for the U area to the left of this item. 
        ///  Item bounds are used and maintained by the Attributes of the ValueList object.
        ///  </summary>
        public System.Drawing.RectangleF BoxLeft
        {
            get { return this._BoxLeft; }
            set { this._BoxLeft = value; }
        }

        public System.Drawing.RectangleF BoxRight
        {
            get { return this._BoxRight; }
            set { this._BoxRight = value; }
        }

        /// <summary>
        ///  Gets the (cached) result of this constant
        ///  </summary>
        [System.ComponentModel.Browsable(false)]
        public IGH_Goo Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Expression))
                {
                    return null;
                }
                if (this.m_value == null)
                {
                    try
                    {
                        GH_ExpressionParser parser = new GH_ExpressionParser();
                        string exp = GH_ExpressionSyntaxWriter.RewriteAll(this.Expression);
                        GH_Variant val = parser.Evaluate(exp);
                        if (val != null)
                        {
                            this.m_value = val.ToGoo();
                        }
                    }
                    catch (System.Exception expr_42)
                    {
                        ProjectData.SetProjectError(expr_42);
                        ProjectData.ClearProjectError();
                    }
                }
                return this.m_value;
            }
        }

        /// <summary>
        ///  Gets whether this item is visible, i.e. if the NameBox has a height larger than zero.
        ///  </summary>
        public bool IsVisible
        {
            get { return this.BoxName.Height > 0f; }
        }

        public GH_ValueListItem()
        {
            this.Name = string.Empty;
            this.Expression = string.Empty;
        }

        public GH_ValueListItem(string name, string expression)
        {
            this.Name = name;
            this.Expression = expression;
        }

        public void ExpireValue()
        {
            this.m_value = null;
        }

        internal void SetDropdownBounds(System.Drawing.RectangleF bounds)
        {
            System.Drawing.RectangleF rectangleF = new System.Drawing.RectangleF(bounds.X, bounds.Y, 0f, bounds.Height);
            this.BoxLeft = rectangleF;
            rectangleF = new System.Drawing.RectangleF(bounds.X, bounds.Y, bounds.Width - 22f, bounds.Height);
            this.BoxName = rectangleF;
            rectangleF = new System.Drawing.RectangleF(bounds.Right - 22f, bounds.Y, 22f, bounds.Height);
            this.BoxRight = rectangleF;
        }

        internal void SetEmptyBounds(System.Drawing.RectangleF bounds)
        {
            System.Drawing.RectangleF rectangleF = new System.Drawing.RectangleF(bounds.X, bounds.Y, 0f, 0f);
            this.BoxLeft = rectangleF;
            rectangleF = new System.Drawing.RectangleF(bounds.X, bounds.Y, 0f, 0f);
            this.BoxName = rectangleF;
            rectangleF = new System.Drawing.RectangleF(bounds.X, bounds.Y, 0f, 0f);
            this.BoxRight = rectangleF;
        }
    }
}
