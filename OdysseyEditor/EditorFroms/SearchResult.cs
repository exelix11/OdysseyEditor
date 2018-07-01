using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyEditor.EditorFroms
{
    public partial class SearchResult : Form
    {
        public Tuple<ObjList, LevelObj>[] SearchResultArr;
        EditorForm owner;
        public SearchResult(Tuple<ObjList,LevelObj>[] _sr, string title, EditorForm _owner)
        {
            InitializeComponent();
            title = "Search result: " + title;
            SearchResultArr = _sr;
            owner = _owner;
        }

        private void SearchResult_Load(object sender, EventArgs e)
        {
            foreach (var res in SearchResultArr)
            {
                listBox1.Items.Add(res.Item2.ToString() + " in " + res.Item1.name);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                owner.SelectObject(SearchResultArr[listBox1.SelectedIndex].Item1, SearchResultArr[listBox1.SelectedIndex].Item2);
            } 
        }
    }
}
