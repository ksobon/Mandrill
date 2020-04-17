using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_FontStyleAttributes : GH_Attributes<Mandrill_FontStyle>
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

        public Mandrill_FontStyleAttributes(Mandrill_FontStyle owner) : base(owner) { }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var item2 = this.Owner.FirstSelectedItem;
                if (item2 != null)
                {
                    if (item2.BoxRight.Contains(e.CanvasLocation))
                    {
                        var menu = new System.Windows.Forms.ToolStripDropDownMenu();
                        var activeItem = this.Owner.FirstSelectedItem;
                        try
                        {
                            var enumerator = this.Owner.ListItems.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                var existingItem = enumerator.Current;
                                var menuItem = new System.Windows.Forms.ToolStripMenuItem(existingItem.Name);
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
                            var enumerator = new List<GH_ValueListItem>.Enumerator();
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
            var menuItem = (System.Windows.Forms.ToolStripMenuItem)sender;
            if (menuItem.Checked)
            {
                return;
            }
            var item = menuItem.Tag as GH_ValueListItem;
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
            var bounds = this.Bounds;
            var bounds2 = new System.Drawing.RectangleF(bounds.X, this.Bounds.Y, 0f, this.Bounds.Height);
            this.NameBounds = bounds2;
            if (this.Owner.DisplayName != null)
            {
                var nameWidth = GH_FontServer.StringWidth(this.Owner.DisplayName, GH_FontServer.Standard) + 10;
                bounds2 = this.Bounds;
                bounds = new System.Drawing.RectangleF(bounds2.X - (float)nameWidth, this.Bounds.Y, (float)nameWidth, this.Bounds.Height);
                this.NameBounds = bounds;
                this.Bounds = System.Drawing.RectangleF.Union(this.NameBounds, this.ItemBounds);
            }
        }

        private void LayoutDropDown()
        {
            var width = this.ItemMaximumWidth() + 22;
            var height = 22;
            this.Pivot = GH_Convert.ToPoint(this.Pivot);
            var bounds = new System.Drawing.RectangleF(this.Pivot.X, this.Pivot.Y, (float)width, (float)height);
            this.Bounds = bounds;
            var activeItem = this.Owner.FirstSelectedItem;
            try
            {
                var enumerator = this.Owner.ListItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
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
                var enumerator = new List<GH_ValueListItem>.Enumerator();
                ((System.IDisposable)enumerator).Dispose();
            }
        }

        private int ItemMaximumWidth()
        {
            var max = 20;
            try
            {
                var enumerator = this.Owner.ListItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    var width = GH_FontServer.StringWidth(item.Name, GH_FontServer.Standard);
                    max = System.Math.Max(max, width);
                }
            }
            finally
            {
                var enumerator = new List<GH_ValueListItem>.Enumerator();
                ((System.IDisposable)enumerator).Dispose();
            }
            return max + 10;
        }

        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Objects)
            {
                var capsule = GH_Capsule.CreateCapsule(this.Bounds, GH_Palette.Normal);
                capsule.AddOutputGrip(this.OutputGrip.Y);
                capsule.Render(canvas.Graphics, this.Selected, this.Owner.Locked, this.Owner.Hidden);
                capsule.Dispose();
                var alpha = GH_Canvas.ZoomFadeLow;
                if (alpha > 0)
                {
                    canvas.SetSmartTextRenderingHint();
                    var style = GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.Normal, this);
                    var color = System.Drawing.Color.FromArgb(alpha, style.Text);
                    if (this.NameBounds.Width > 0f)
                    {
                        var nameFill = new System.Drawing.SolidBrush(color);
                        graphics.DrawString(this.Owner.NickName, GH_FontServer.Standard, nameFill, this.NameBounds, GH_TextRenderingConstants.CenterCenter);
                        nameFill.Dispose();
                        var x = System.Convert.ToInt32(this.NameBounds.Right);
                        var y0 = System.Convert.ToInt32(this.NameBounds.Top);
                        var y = System.Convert.ToInt32(this.NameBounds.Bottom);
                        GH_GraphicsUtil.EtchFadingVertical(graphics, y0, y, x, System.Convert.ToInt32(0.8 * (double)alpha), System.Convert.ToInt32(0.3 * (double)alpha));
                    }

                    // render dropdown only
                    this.RenderDropDown(canvas, graphics, color);
                }
            }
        }

        private void RenderDropDown(GH_Canvas canvas, System.Drawing.Graphics graphics, System.Drawing.Color color)
        {
            var item = this.Owner.FirstSelectedItem;
            if (item == null)
            {
                return;
            }
            graphics.DrawString(item.Name, GH_FontServer.Standard, System.Drawing.Brushes.Black, item.BoxName, GH_TextRenderingConstants.CenterCenter);
            Mandrill_FontStyleAttributes.RenderDownArrow(canvas, graphics, item.BoxRight, color);
        }

        private static void RenderDownArrow(GH_Canvas canvas, System.Drawing.Graphics graphics, System.Drawing.RectangleF bounds, System.Drawing.Color color)
        {
            var x = System.Convert.ToInt32(bounds.X + 0.5f * bounds.Width);
            var y = System.Convert.ToInt32(bounds.Y + 0.5f * bounds.Height);
            var array = new System.Drawing.PointF[3];
            var arg_54_0_cp_0 = array;
            var arg_54_0_cp_1 = 0;
            var pointF = new System.Drawing.PointF((float)x, (float)(y + 6));
            arg_54_0_cp_0[arg_54_0_cp_1] = pointF;
            var arg_72_0_cp_0 = array;
            var arg_72_0_cp_1 = 1;
            var pointF2 = new System.Drawing.PointF((float)(x + 6), (float)(y - 6));
            arg_72_0_cp_0[arg_72_0_cp_1] = pointF2;
            var arg_90_0_cp_0 = array;
            var arg_90_0_cp_1 = 2;
            var pointF3 = new System.Drawing.PointF((float)(x - 6), (float)(y - 6));
            arg_90_0_cp_0[arg_90_0_cp_1] = pointF3;
            var corners = array;
            Mandrill_FontStyleAttributes.RenderShape(canvas, graphics, corners, color);
        }

        private static void RenderShape(GH_Canvas canvas, System.Drawing.Graphics graphics, System.Drawing.PointF[] points, System.Drawing.Color color)
        {
            var alpha = GH_Canvas.ZoomFadeMedium;
            var x0 = points[0].X;
            var x = x0;
            var y0 = points[0].Y;
            var y = y0;
            var arg_32_0 = 1;
            var num = points.Length - 1;
            for (var i = arg_32_0; i <= num; i++)
            {
                x0 = System.Math.Min(x0, points[i].X);
                x = System.Math.Max(x, points[i].X);
                y0 = System.Math.Min(y0, points[i].Y);
                y = System.Math.Max(y, points[i].Y);
            }
            var bounds = System.Drawing.RectangleF.FromLTRB(x0, y0, x, y);
            bounds.Inflate(1f, 1f);
            var fill = new System.Drawing.Drawing2D.LinearGradientBrush(bounds, color, GH_GraphicsUtil.OffsetColour(color, 50), System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            fill.WrapMode = System.Drawing.Drawing2D.WrapMode.TileFlipXY;
            graphics.FillPolygon(fill, points);
            fill.Dispose();
            if (alpha > 0)
            {
                var col0 = System.Drawing.Color.FromArgb(System.Convert.ToInt32(0.5 * (double)alpha), System.Drawing.Color.White);
                var col = System.Drawing.Color.FromArgb(0, System.Drawing.Color.White);
                var highlightFill = new System.Drawing.Drawing2D.LinearGradientBrush(bounds, col0, col, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                highlightFill.WrapMode = System.Drawing.Drawing2D.WrapMode.TileFlipXY;
                var highlightEdge = new System.Drawing.Pen(highlightFill, 3f);
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
