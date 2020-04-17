using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Collections.Generic;

namespace Mandrill_Grasshopper.Components.PDF
{
    public abstract class Mandrill_BaseDropdown : GH_Param<IGH_Goo>, IGH_PreviewObject, IGH_StateAwareObject
    {
        public GH_ValueListMode m_listMode;
        public System.Collections.Generic.List<GH_ValueListItem> m_userItems;
        public bool m_hidden;

        public Mandrill_BaseDropdown(string name, string nickname, string description, string tab, string panel) :
            base(new GH_InstanceDescription(name, nickname, description, tab, panel))
        {
            this.m_listMode = GH_ValueListMode.DropDown;
        }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.NickName))
                {
                    return null;
                }
                if (this.NickName.Equals("List", System.StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
                return this.NickName;
            }
        }

        public GH_ValueListMode ListMode
        {
            get
            {
                return this.m_listMode;
            }
            set
            {
                this.m_listMode = value;
                if (this.m_attributes != null)
                {
                    this.m_attributes.ExpireLayout();
                }
            }
        }

        public System.Collections.Generic.List<GH_ValueListItem> ListItems
        {
            get { return this.m_userItems; }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.Obsolete("This property has been replaced by ListItems")]
        public System.Collections.Generic.List<GH_ValueListItem> Values
        {
            get
            {
                return this.m_userItems;
            }
        }

        /// <summary>
        ///  Gets the selected value list items. If the ListMode only supports a single selected item 
        ///  then only the first selected item will be included.
        ///  </summary>
        public System.Collections.Generic.List<GH_ValueListItem> SelectedItems
        {
            get
            {
                var items = new System.Collections.Generic.List<GH_ValueListItem>();
                if (this.m_userItems.Count == 0)
                {
                    return items;
                }
                try
                {
                    var enumerator2 = this.m_userItems.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        var item2 = enumerator2.Current;
                        if (item2.Selected)
                        {
                            items.Add(item2);
                            return items;
                        }
                    }
                }
                finally
                {
                    var enumerator2 = new List<GH_ValueListItem>.Enumerator();
                    ((System.IDisposable)enumerator2).Dispose();
                }
                this.m_userItems[0].Selected = true;
                items.Add(this.m_userItems[0]);
                return items;
            }
        }

        public GH_ValueListItem FirstSelectedItem
        {
            get
            {
                if (this.m_userItems.Count == 0)
                {
                    return null;
                }
                try
                {
                    var enumerator = this.m_userItems.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        var item = enumerator.Current;
                        if (item.Selected)
                        {
                            return item;
                        }
                    }
                }
                finally
                {
                    var enumerator = new List<GH_ValueListItem>.Enumerator();
                    ((System.IDisposable)enumerator).Dispose();
                }
                return this.m_userItems[0];
            }
        }

        public bool Hidden
        {
            get { return this.m_hidden; }
            set { this.m_hidden = value; }
        }

        public bool IsPreviewCapable
        {
            get { return true; }
        }

        public BoundingBox ClippingBox
        {
            get { return this.Preview_ComputeClippingBox(); }
        }

        public void SelectItem(int index)
        {
            if (index < 0)
            {
                return;
            }
            if (index >= this.m_userItems.Count)
            {
                return;
            }
            var modify = false;
            var arg_25_0 = 0;
            var num = this.m_userItems.Count - 1;
            for (var i = arg_25_0; i <= num; i++)
            {
                if (i == index)
                {
                    if (!this.m_userItems[i].Selected)
                    {
                        modify = true;
                        break;
                    }
                }
                else if (this.m_userItems[i].Selected)
                {
                    modify = true;
                    break;
                }
            }
            if (!modify)
            {
                return;
            }
            this.RecordUndoEvent("Select: " + this.m_userItems[index].Name);
            var arg_98_0 = 0;
            var num2 = this.m_userItems.Count - 1;
            for (var j = arg_98_0; j <= num2; j++)
            {
                this.m_userItems[j].Selected = (j == index);
            }
            this.ExpireSolution(true);
        }

        protected override IGH_Goo InstantiateT()
        {
            return new GH_ObjectWrapper();
        }

        protected override void CollectVolatileData_Custom()
        {
            this.m_data.Clear();
            try
            {
                var enumerator = this.SelectedItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    this.m_data.Append(item.Value, new GH_Path(0));
                }
            }
            finally
            {
                var enumerator = new List<GH_ValueListItem>.Enumerator();
                ((System.IDisposable)enumerator).Dispose();
            }
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            this.Preview_DrawMeshes(args);
        }

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            this.Preview_DrawWires(args);
        }

        public void LoadState(string state)
        {
            try
            {
                var enumerator = this.m_userItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    item.Selected = false;
                }
            }
            finally
            {
                var enumerator = new List<GH_ValueListItem>.Enumerator();
                ((System.IDisposable)enumerator).Dispose();
            }
            int index;
            if (int.TryParse(state, out index))
            {
                if (index >= 0 && index < this.m_userItems.Count)
                {
                    this.m_userItems[index].Selected = true;
                }
            }
            else
            {
                var arg_81_0 = 0;
                var num = System.Math.Min(state.Length, this.m_userItems.Count) - 1;
                for (var i = arg_81_0; i <= num; i++)
                {
                    this.m_userItems[i].Selected = state[i].Equals('Y');
                }
            }
        }

        public string SaveState()
        {
            var state = new System.Text.StringBuilder(this.m_userItems.Count);
            try
            {
                var enumerator = this.m_userItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    if (item.Selected)
                    {
                        state.Append('Y');
                    }
                    else
                    {
                        state.Append('N');
                    }
                }
            }
            finally
            {
                var enumerator = new List<GH_ValueListItem>.Enumerator();
                ((System.IDisposable)enumerator).Dispose();
            }
            return state.ToString();
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("ListMode", (int)this.ListMode);
            writer.SetInt32("ListCount", this.m_userItems.Count);
            var arg_36_0 = 0;
            var num = this.m_userItems.Count - 1;
            for (var i = arg_36_0; i <= num; i++)
            {
                var chunk = writer.CreateChunk("ListItem", i);
                chunk.SetString("Name", this.m_userItems[i].Name);
                chunk.SetString("Expression", this.m_userItems[i].Expression);
                chunk.SetBoolean("Selected", this.m_userItems[i].Selected);
            }
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            var mode = 1;
            reader.TryGetInt32("UIMode", ref mode);
            reader.TryGetInt32("ListMode", ref mode);
            this.ListMode = (GH_ValueListMode)mode;
            var count = reader.GetInt32("ListCount");
            var cache = 0;
            reader.TryGetInt32("CacheCount", ref cache);
            this.m_userItems.Clear();
            var arg_52_0 = 0;
            var num = count - 1;
            for (var i = arg_52_0; i <= num; i++)
            {
                var chunk = reader.FindChunk("ListItem", i);
                if (chunk == null)
                {
                    reader.AddMessage("Missing chunk for List Value: " + i.ToString(), GH_Message_Type.error);
                }
                else
                {
                    var itemName = chunk.GetString("Name");
                    var itemExpression = chunk.GetString("Expression");
                    var itemSelected = false;
                    chunk.TryGetBoolean("Selected", ref itemSelected);
                    var item = new GH_ValueListItem(itemName, itemExpression);
                    item.Selected = itemSelected;
                    this.m_userItems.Add(item);
                }
            }
            if (reader.ItemExists("ListIndex"))
            {
                var idx = reader.GetInt32("ListIndex");
                if (idx >= 0 && idx < this.m_userItems.Count)
                {
                    this.m_userItems[idx].Selected = true;
                }
            }
            return base.Read(reader);
        }
    }
}