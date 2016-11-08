using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_FontTransformAttributes : GH_Attributes<Mandrill_FontTransform>
    {
        public const int ItemHeight = 22;

        [System.Runtime.CompilerServices.CompilerGenerated]
        private System.Drawing.RectangleF _ItemBounds;

        [System.Runtime.CompilerServices.CompilerGenerated]
        private System.Drawing.RectangleF _NameBounds;

        private const int ArrowRadius = 6;

        public override bool AllowMessageBalloon
        {
            get { return false; }
        }

        public override bool HasInputGrip
        {
            get { return false; }
        }

        public override bool HasOutputGrip
        {
            get { return true; }
        }

        private System.Drawing.RectangleF ItemBounds
        {
            get { return this._ItemBounds; }
            set { this._ItemBounds = value; }
        }

        private System.Drawing.RectangleF NameBounds
        {
            get { return this._NameBounds; }
            set { this._NameBounds = value; }
        }

        public Mandrill_FontTransformAttributes(Mandrill_FontTransform owner) : base(owner) { }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                GH_ValueListItem item2 = this.Owner.FirstSelectedItem;
                if (item2 != null)
                {
                    if (item2.BoxRight.Contains(e.CanvasLocation))
                    {
                        System.Windows.Forms.ToolStripDropDownMenu menu = new System.Windows.Forms.ToolStripDropDownMenu();
                        GH_ValueListItem activeItem = this.Owner.FirstSelectedItem;
                        try
                        {
                            System.Collections.Generic.List<GH_ValueListItem>.Enumerator enumerator = this.Owner.ListItems.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                GH_ValueListItem existingItem = enumerator.Current;
                                System.Windows.Forms.ToolStripMenuItem menuItem = new System.Windows.Forms.ToolStripMenuItem(existingItem.Name);
                                menuItem.Click += new System.EventHandler(this.ValueMenuItem_Click);
                                if (existingItem == activeItem)
                                {
                                    menuItem.Checked = true;
                                }
                                menuItem.Tag = existingItem;
                                menu.Items.Add(menuItem);
                            }
                        }
                        finally
                        {
                            System.Collections.Generic.List<GH_ValueListItem>.Enumerator enumerator = new List<GH_ValueListItem>.Enumerator();
                            ((System.IDisposable)enumerator).Dispose();
                        }
                        menu.Show(sender, e.ControlLocation);
                        return GH_ObjectResponse.Handled;
                    }
                }
            }
            return base.RespondToMouseDown(sender, e);
        }

        private void ValueMenuItem_Click(object sender, System.EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem menuItem = (System.Windows.Forms.ToolStripMenuItem)sender;
            if (menuItem.Checked)
            {
                return;
            }
            GH_ValueListItem item = menuItem.Tag as GH_ValueListItem;
            if (item == null)
            {
                return;
            }
            this.Owner.SelectItem(this.Owner.ListItems.IndexOf(item));
        }

        protected override void Layout()
        {
            // only layout dropdown
            this.LayoutDropDown();

            this.ItemBounds = this.Bounds;
            System.Drawing.RectangleF bounds = this.Bounds;
            System.Drawing.RectangleF bounds2 = new System.Drawing.RectangleF(bounds.X, this.Bounds.Y, 0f, this.Bounds.Height);
            this.NameBounds = bounds2;
            if (this.Owner.DisplayName != null)
            {
                int nameWidth = GH_FontServer.StringWidth(this.Owner.DisplayName, GH_FontServer.Standard) + 10;
                bounds2 = this.Bounds;
                bounds = new System.Drawing.RectangleF(bounds2.X - (float)nameWidth, this.Bounds.Y, (float)nameWidth, this.Bounds.Height);
                this.NameBounds = bounds;
                this.Bounds = System.Drawing.RectangleF.Union(this.NameBounds, this.ItemBounds);
            }
        }

        private void LayoutDropDown()
        {
            int width = this.ItemMaximumWidth() + 22;
            int height = 22;
            this.Pivot = GH_Convert.ToPoint(this.Pivot);
            System.Drawing.RectangleF bounds = new System.Drawing.RectangleF(this.Pivot.X, this.Pivot.Y, (float)width, (float)height);
            this.Bounds = bounds;
            GH_ValueListItem activeItem = this.Owner.FirstSelectedItem;
            try
            {
                System.Collections.Generic.List<GH_ValueListItem>.Enumerator enumerator = this.Owner.ListItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    GH_ValueListItem item = enumerator.Current;
                    if (item == activeItem)
                    {
                        item.SetDropdownBounds(this.Bounds);
                    }
                    else
                    {
                        item.SetEmptyBounds(this.Bounds);
                    }
                }
            }
            finally
            {
                System.Collections.Generic.List<GH_ValueListItem>.Enumerator enumerator = new List<GH_ValueListItem>.Enumerator();
                ((System.IDisposable)enumerator).Dispose();
            }
        }

        private int ItemMaximumWidth()
        {
            int max = 20;
            try
            {
                System.Collections.Generic.List<GH_ValueListItem>.Enumerator enumerator = this.Owner.ListItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    GH_ValueListItem item = enumerator.Current;
                    int width = GH_FontServer.StringWidth(item.Name, GH_FontServer.Standard);
                    max = System.Math.Max(max, width);
                }
            }
            finally
            {
                System.Collections.Generic.List<GH_ValueListItem>.Enumerator enumerator = new List<GH_ValueListItem>.Enumerator();
                ((System.IDisposable)enumerator).Dispose();
            }
            return max + 10;
        }

        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule capsule = GH_Capsule.CreateCapsule(this.Bounds, GH_Palette.Normal);
                capsule.AddOutputGrip(this.OutputGrip.Y);
                capsule.Render(canvas.Graphics, this.Selected, this.Owner.Locked, this.Owner.Hidden);
                capsule.Dispose();
                int alpha = GH_Canvas.ZoomFadeLow;
                if (alpha > 0)
                {
                    canvas.SetSmartTextRenderingHint();
                    GH_PaletteStyle style = GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.Normal, this);
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(alpha, style.Text);
                    if (this.NameBounds.Width > 0f)
                    {
                        System.Drawing.SolidBrush nameFill = new System.Drawing.SolidBrush(color);
                        graphics.DrawString(this.Owner.NickName, GH_FontServer.Standard, nameFill, this.NameBounds, GH_TextRenderingConstants.CenterCenter);
                        nameFill.Dispose();
                        int x = System.Convert.ToInt32(this.NameBounds.Right);
                        int y0 = System.Convert.ToInt32(this.NameBounds.Top);
                        int y = System.Convert.ToInt32(this.NameBounds.Bottom);
                        GH_GraphicsUtil.EtchFadingVertical(graphics, y0, y, x, System.Convert.ToInt32(0.8 * (double)alpha), System.Convert.ToInt32(0.3 * (double)alpha));
                    }

                    // render dropdown only
                    this.RenderDropDown(canvas, graphics, color);
                }
            }
        }

        private void RenderDropDown(GH_Canvas canvas, System.Drawing.Graphics graphics, System.Drawing.Color color)
        {
            GH_ValueListItem item = this.Owner.FirstSelectedItem;
            if (item == null)
            {
                return;
            }
            graphics.DrawString(item.Name, GH_FontServer.Standard, System.Drawing.Brushes.Black, item.BoxName, GH_TextRenderingConstants.CenterCenter);
            Mandrill_FontTransformAttributes.RenderDownArrow(canvas, graphics, item.BoxRight, color);
        }

        private static void RenderDownArrow(GH_Canvas canvas, System.Drawing.Graphics graphics, System.Drawing.RectangleF bounds, System.Drawing.Color color)
        {
            int x = System.Convert.ToInt32(bounds.X + 0.5f * bounds.Width);
            int y = System.Convert.ToInt32(bounds.Y + 0.5f * bounds.Height);
            System.Drawing.PointF[] array = new System.Drawing.PointF[3];
            System.Drawing.PointF[] arg_54_0_cp_0 = array;
            int arg_54_0_cp_1 = 0;
            System.Drawing.PointF pointF = new System.Drawing.PointF((float)x, (float)(y + 6));
            arg_54_0_cp_0[arg_54_0_cp_1] = pointF;
            System.Drawing.PointF[] arg_72_0_cp_0 = array;
            int arg_72_0_cp_1 = 1;
            System.Drawing.PointF pointF2 = new System.Drawing.PointF((float)(x + 6), (float)(y - 6));
            arg_72_0_cp_0[arg_72_0_cp_1] = pointF2;
            System.Drawing.PointF[] arg_90_0_cp_0 = array;
            int arg_90_0_cp_1 = 2;
            System.Drawing.PointF pointF3 = new System.Drawing.PointF((float)(x - 6), (float)(y - 6));
            arg_90_0_cp_0[arg_90_0_cp_1] = pointF3;
            System.Drawing.PointF[] corners = array;
            Mandrill_FontTransformAttributes.RenderShape(canvas, graphics, corners, color);
        }

        private static void RenderShape(GH_Canvas canvas, System.Drawing.Graphics graphics, System.Drawing.PointF[] points, System.Drawing.Color color)
        {
            int alpha = GH_Canvas.ZoomFadeMedium;
            float x0 = points[0].X;
            float x = x0;
            float y0 = points[0].Y;
            float y = y0;
            int arg_32_0 = 1;
            int num = points.Length - 1;
            for (int i = arg_32_0; i <= num; i++)
            {
                x0 = System.Math.Min(x0, points[i].X);
                x = System.Math.Max(x, points[i].X);
                y0 = System.Math.Min(y0, points[i].Y);
                y = System.Math.Max(y, points[i].Y);
            }
            System.Drawing.RectangleF bounds = System.Drawing.RectangleF.FromLTRB(x0, y0, x, y);
            bounds.Inflate(1f, 1f);
            System.Drawing.Drawing2D.LinearGradientBrush fill = new System.Drawing.Drawing2D.LinearGradientBrush(bounds, color, GH_GraphicsUtil.OffsetColour(color, 50), System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            fill.WrapMode = System.Drawing.Drawing2D.WrapMode.TileFlipXY;
            graphics.FillPolygon(fill, points);
            fill.Dispose();
            if (alpha > 0)
            {
                System.Drawing.Color col0 = System.Drawing.Color.FromArgb(System.Convert.ToInt32(0.5 * (double)alpha), System.Drawing.Color.White);
                System.Drawing.Color col = System.Drawing.Color.FromArgb(0, System.Drawing.Color.White);
                System.Drawing.Drawing2D.LinearGradientBrush highlightFill = new System.Drawing.Drawing2D.LinearGradientBrush(bounds, col0, col, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                highlightFill.WrapMode = System.Drawing.Drawing2D.WrapMode.TileFlipXY;
                System.Drawing.Pen highlightEdge = new System.Drawing.Pen(highlightFill, 3f);
                highlightEdge.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                highlightEdge.CompoundArray = new float[]
                {
                    0f,
                    0.5f
                };
                graphics.DrawPolygon(highlightEdge, points);
                highlightFill.Dispose();
                highlightEdge.Dispose();
            }
            graphics.DrawPolygon(new System.Drawing.Pen(color, 1f)
            {
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            }, points);
        }
    }
}
