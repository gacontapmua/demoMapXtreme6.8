using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping;
using MapInfo.Styles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class mapControl : Form
    {
        public mapControl()
        {
            InitializeComponent();
        }
        private Map _map = null;
        private Map map1 = null;
        TableInfoMemTable tableInfo;
        Table pointTable;
        MapInfo.Data.TableInfo ti;//= MapInfo.Data.TableInfoFactory.CreateTemp("temp2");
        MapInfo.Data.Table table;//= MapInfo.Engine.Session.Current.Catalog.CreateTable(ti);
        private string[] arrfilename;
        private Catalog _catalog = Session.Current.Catalog;
        private int add = 0;//them diem

        IResultSetFeatureCollection fc_point = null;
        IResultSetFeatureCollection fc_line = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            mapControl1.Map.Clear();
            CoordSys coordSys = mapControl1.Map.GetDisplayCoordSys();
            _map = this.mapControl1.Map;
            map1 = _map;
            ////////////////////tao bang tam
            mapControl1.Map.Clear();
            // CoordSys coordSys = mapControl1.Map.GetDisplayCoordSys();
            tableInfo = new TableInfoMemTable("temp");
            tableInfo.Temporary = true;

            Column column;
            column = new GeometryColumn(coordSys);
            column.Alias = "MI_Geometry";
            column.DataType = MIDbType.FeatureGeometry;
            tableInfo.Columns.Add(column);

            column = new Column();
            column.Alias = "MI_Style";
            column.DataType = MIDbType.Style;
            tableInfo.Columns.Add(column);

            column = new Column();
            column.Alias = "ID";
            column.DataType = MIDbType.String;
            tableInfo.Columns.Add(column);


            pointTable = Session.Current.Catalog.CreateTable(tableInfo);
            mapControl1.Map.Layers.Add(new MapInfo.Mapping.FeatureLayer(pointTable));
            _map = this.mapControl1.Map;
            ti = MapInfo.Data.TableInfoFactory.CreateTemp("temp2");
            table = MapInfo.Engine.Session.Current.Catalog.CreateTable(ti);

        }
        private void OpenTable()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = "TAB";
            openFileDialog1.Filter =
                "MapInfo Tables (*.tab)|*.tab|" +
                "MapInfo Geoset (*.gst)|*.gst|" +
                "MapInfo WorkSpace (*.mws)|*.mws";
            string strCantOpenList = null;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = Application.ExecutablePath;
            if (openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                arrfilename = openFileDialog1.FileNames;
                foreach (string filename in openFileDialog1.FileNames)
                    try
                    {
                        //--------------if filename(*.gst)
                        if (filename.ToLower().EndsWith(MapLoader.FileExtensionGST))
                        {
                            mapControl1.Map.Load(new MapGeosetLoader(filename)); // add geoset
                        }
                        //--------------if filename(*.mws)
                        else if (filename.ToLower().EndsWith(MapLoader.FileExtensionWOR))
                        {
                            mapControl1.Map.Load(new MapWorkSpaceLoader(filename));  // add workspace

                        }
                        //--------------if filename(*.tab)
                        else
                        {
                            mapControl1.Map.Load(new MapTableLoader(filename));  // add table
                        }
                    }
                    catch (MapException me)
                    {
                        if (strCantOpenList == null) strCantOpenList = me.Arg;
                        else strCantOpenList = strCantOpenList + ", " + me.Arg;
                    }

                AddListPointsToComboBox("tbOffice", "NameOffice", ref comOffice);
                AddListPointsToComboBox("tbBUS", "IdBus", ref comBusNum);
                AddListPointsToComboBox("tbNodeTrafic", "NameNode", ref comTrafic);
                AddListPointsToComboBox("tbLake", "NameLake", ref comLake);
                BuildGraphPoints();

            }
            if (strCantOpenList != null)
                MessageBox.Show("The following failed to open: " + strCantOpenList);

        }
        private int iAllLines = 0;
        private Graph gr = new Graph();
        private int[] ParentPoint;
        private int iAllPoints = 0;
        private void BuildGraphPoints()
        {
            //lay tong so duong
            MapInfo.Data.Table tbl1 = _catalog.GetTable("tbLine");
            SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchAll();
            IResultSetFeatureCollection setfc = _catalog.Search("tbLine", si);
            fc_line = setfc;
            iAllLines = setfc.Count;
            ////xay dung Graph
            MapInfo.Data.Table tbl = _catalog.GetTable("tbPoint");
            DataTable dtPoints = new DataTable();
            if (tbl != null)//exist
            {
                // xay dung do thi cac Points
                gr = new Graph();
                si = MapInfo.Data.SearchInfoFactory.SearchAll();
                setfc = _catalog.Search("tbPoint", si);
                fc_point = setfc;
                if (setfc.Count > 0)
                {
                    ParentPoint = new int[setfc.Count];
                    iAllPoints = setfc.Count;
                    for (int i = 0; i < setfc.Count; i++)
                    {

                        DataRow dr = null;
                        string strQuery = "Select * from tbPoint where IdPoint = '" + (i + 1).ToString() + "'";

                        dtPoints = GetDataTable(tbl.Alias, strQuery);
                        Feature ft = nchan[0];
                        double x = ft.Geometry.Centroid.x;
                        double y = ft.Geometry.Centroid.y;
                        Graph.Node node = new Graph.Node(i + 1, x, y, 0, 0, 0, false);

                        dr = dtPoints.Rows[0];

                        ///////////Nhap diem ke???

                        string sAdjPoints = "";
                        sAdjPoints = dr["NearPoint"].ToString().Trim();
                        if (sAdjPoints.Contains(","))
                        {
                            string[] sArrAdjPoints = sAdjPoints.Split(',');
                            for (int j = 0; j < sArrAdjPoints.Length; j++)
                            {
                                node.ListAdjNode.Add(Convert.ToInt32(sArrAdjPoints[j]));
                            }
                        }
                        else if (sAdjPoints != "")
                        {
                            node.ListAdjNode.Add(Convert.ToInt32(sAdjPoints));
                        }
                        //-------------------------
                        gr.ListNodes.Add(node);
                    }

                }
            }
        }
        private IResultSetFeatureCollection nchan;
        private DataTable GetDataTable(string nTable, string strQuery)
        {
            MIConnection connect = new MIConnection();
            connect.Open();
            MICommand cmd = connect.CreateCommand();
            if (strQuery == "")
                cmd.CommandText = "Select * from " + nTable;
            else
                cmd.CommandText = strQuery;
            cmd.Prepare();
            nchan = cmd.ExecuteFeatureCollection();

            //2.tao doi tuong MIDataReader de doc du lieu
            MapInfo.Data.MIDataReader miDataReader = cmd.ExecuteReader();
            //3.tao doi tuong DataTable de chua du lieu, ->hien thi trong Datagrid
            DataTable dt = new DataTable("table");
            //3.1 tao cot cho DataTable
            for (int i = 0; i < miDataReader.FieldCount; i++)
            {
                dt.Columns.Add(miDataReader.GetName(i));
            }
            //3.2. dich con tro de doc tung record
            while (miDataReader.Read())
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < miDataReader.FieldCount; i++)
                {
                    dr[i] = miDataReader.GetValue(i);
                }
                //add tung record vao DataTable
                dt.Rows.Add(dr);
            }
            //4.add DataTable vao DataGird
            miDataReader.Close();
            connect.Close();
            return dt;
        }
        private void AddListPointsToComboBox(string tab, string fname, ref System.Windows.Forms.ComboBox com)
        {
            try
            {
                MapInfo.Data.Table tblPoint = _catalog.GetTable(tab);
                string strQuery = "Select * from " + tab;
                DataTable dtPoints = new DataTable();
                dtPoints = GetDataTable(tab, strQuery);

                DataRow dtRow = null;
                string ma;
                if (com.Items.Count > 0) com.Items.Clear();
                //if (combo_ma.Items.Count > 0) combo_ma.Items.Clear();
                for (int i = 0; i < dtPoints.Rows.Count; i++)
                {
                    dtRow = dtPoints.Rows[i];
                    //ten = dtRow["ma_diem"].ToString();
                    //combo_ten.Items.Add(ten);
                    ma = dtRow[fname].ToString();
                    com.Items.Add(ma);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Canh_Bao!");
                return;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenTable();
        }
        string Idpoint = "";
        string Nearpoint = "";
        string Listbus = "";
        System.Drawing.Point p_addpoint = new System.Drawing.Point();
        private Selection _selection = Session.Current.Selections.DefaultSelection;
        private void mapControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (add == 1)//them diem
            {
                BuildGraphPoints();

                Idpoint = "";
                Nearpoint = "";
                Listbus = "";
                p_addpoint = new System.Drawing.Point(e.X, e.Y);

                SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, p_addpoint, 15);
                IResultSetFeatureCollection fc = _catalog.Search("tbPoint", si);
                _selection.Clear();
                _selection.Add(fc);
                if (fc.Count > 0)
                {
                    IResultSetFeatureCollection rc = Session.Current.Selections.DefaultSelection[Session.Current.Catalog.GetTable("tbPoint")];
                    if (rc.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy điểm nào");
                    }
                    else
                    {
                        DataTable dtPoints = new DataTable();
                        DataRow dr = null;
                        string strQuery = "Select * from tbPoint where IdPoint = '" + rc[0][1].ToString() + "'";

                        dtPoints = GetDataTable("tbPoint", strQuery);
                        Feature ft = nchan[0];
                        dr = dtPoints.Rows[0];
                        Idpoint = dr["IdPoint"].ToString();
                        Nearpoint = dr["NearPoint"].ToString();
                        Listbus = dr["ListBus"].ToString();
                    }
                }
                if (Idpoint == "")
                {
                    Idpoint = Convert.ToString(iAllPoints + 1);

                }
                CreateForm();


            }
        }
        int p_data = 0;
        public void GetValue(string value1, string value2, string value3, int value4)
        {
            Idpoint = value1;
            Nearpoint = value2;
            Listbus = value3;
            p_data = value4;
            if (p_data == 1)
            {
                AddPoint(p_addpoint.X, p_addpoint.Y, Idpoint, Nearpoint, Listbus);
            }
            if (p_data == 2)
            {
                EditPoint(Idpoint, Nearpoint, Listbus);

            }
            if (p_data == 3)
            {
                DeletePoint(Idpoint);
            }

            // 
            // 

        }
        private void split(string str, ref ArrayList arrstr)
        {

            int t = -1;//vi tri truoc
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ',')
                {
                    arrstr.Add(str.Substring(t + 1, i - t - 1));
                    t = i;

                }

            }
            arrstr.Add(str.Substring(t + 1, str.Length - t - 1));

        }

        private MIConnection connect = new MIConnection();
        private void AddPoint(int x, int y, string idpoint, string nearpoint, string listbus)
        {
            System.Drawing.Point point = new System.Drawing.Point(x, y);

            //kiem tra neu da ton tai mot diem o rat gan thi khong them nua

            SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, point, 15);
            IResultSetFeatureCollection fc = _catalog.Search("tbPoint", si);
            if (fc.Count > 0)
            {
                MessageBox.Show("Đã tồn tại một điểm ở rất gần!");
            }
            else
            {

                ////
                DPoint DP = new DPoint();
                mapControl1.Map.DisplayTransform.FromDisplay(point, out DP);
                connect.Open();
                MICommand cmd = connect.CreateCommand();
                FeatureGeometry geometry = new MapInfo.Geometry.Point(mapControl1.Map.GetDisplayCoordSys(), DP);
                SimpleVectorPointStyle vStyle = fc_point[0].Style as SimpleVectorPointStyle;
                CompositeStyle cStyle = new MapInfo.Styles.CompositeStyle(vStyle);



                //tim xem diem doc lap hay diem tren duong
                si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, point, 15);
                fc = _catalog.Search("tbLine", si);
                _selection.Clear();
                _selection.Add(fc);
                cmd = connect.CreateCommand();

                if (fc.Count > 0)//day khong phai diem doc lap
                {

                    IResultSetFeatureCollection rc = _selection[_catalog.GetTable("tbLine")];
                    //================update lai cac diem ke========================
                    //lay danh sach cac dinh tren Line
                    string s1 = "Select Points from tbLine where IdLine='" + rc[0][1].ToString() + "'";
                    cmd.CommandText = s1;
                    cmd.Prepare();
                    IResultSetFeatureCollection n1 = cmd.ExecuteFeatureCollection();
                    string pointofline = n1[0][0].ToString();
                    ArrayList arr_poline = new ArrayList();
                    split(pointofline, ref arr_poline);
                    int len_arrP = arr_poline.Count;
                    DPoint[] arrP = new DPoint[len_arrP];
                    //lay toa do cua cac diem 
                    for (int i = 0; i < len_arrP; i++)
                    {
                        int j = Convert.ToInt16(arr_poline[i]);
                        arrP[i] = new DPoint(gr.ListNodes[j - 1].X, gr.ListNodes[j - 1].Y);

                    }

                    DPoint DPInsert = new DPoint();
                    mapControl1.Map.DisplayTransform.FromDisplay(point, out DPInsert);
                    //tim diem ke cua diem them vao
                    bool kt = false;
                    int truoc = 0, sau = 0;
                    for (int i = 0; i < len_arrP - 1; i++)
                    {
                        kt = CheckPoint(arrP[i], DPInsert, arrP[i + 1]);
                        if (kt)
                        {
                            truoc = i;
                            sau = i + 1;
                            break;
                        }
                    }
                    string id1 = arr_poline[truoc].ToString();
                    string id2 = arr_poline[sau].ToString();
                    //lay NearPoint cua cac diem id1 va id2
                    cmd.CommandText = "select NearPoint from tbPoint where IdPoint='" + id1 + "'";
                    cmd.Prepare();
                    n1 = cmd.ExecuteFeatureCollection();
                    string near1 = n1[0][0].ToString();
                    string[] arrNear1 = near1.Split(',');
                    int lenNear1 = arrNear1.Length;
                    near1 = "";
                    for (int i = 0; i < lenNear1; i++)
                    {
                        if (arrNear1[i] != id2)
                            near1 = near1 + "," + arrNear1[i];
                        else
                            near1 = near1 + "," + idpoint;

                    }
                    near1 = near1.Remove(0, 1);



                    cmd.CommandText = "select NearPoint from tbPoint where IdPoint='" + id2 + "'";
                    cmd.Prepare();
                    n1 = cmd.ExecuteFeatureCollection();
                    string near2 = n1[0][0].ToString();

                    string[] arrNear2 = near2.Split(',');
                    int lenNear2 = arrNear2.Length;
                    near2 = "";
                    for (int i = 0; i < lenNear2; i++)
                    {
                        if (arrNear2[i] != id1)
                            near2 = near2 + "," + arrNear2[i];
                        else
                            near2 = near2 + "," + idpoint;

                    }
                    near2 = near2.Remove(0, 1);

                    //update truong NearPoint cua cac diem ke
                    cmd.CommandText = "update tbPoint set NearPoint='" + near1 + "' where IdPoint='" + id1 + "'";
                    cmd.Prepare();
                    int u1 = cmd.ExecuteNonQuery();

                    cmd.CommandText = "update tbPoint set NearPoint='" + near2 + "' where IdPoint='" + id2 + "'";
                    cmd.Prepare();
                    int u2 = cmd.ExecuteNonQuery();
                    //tinh ListBus cho diem moi them vao
                    cmd.CommandText = "select ListBus from tbPoint where IdPoint='" + id1 + "'";
                    cmd.Prepare();
                    IResultSetFeatureCollection irs = cmd.ExecuteFeatureCollection();

                    string listBus = irs[0][0].ToString();
                    ArrayList arr_listBus1 = new ArrayList();
                    split(listBus, ref arr_listBus1);
                    int len_arrB1 = arr_listBus1.Count;

                    cmd.CommandText = "select ListBus from tbPoint where IdPoint='" + id2 + "'";
                    cmd.Prepare();
                    irs = cmd.ExecuteFeatureCollection();

                    listBus = irs[0][0].ToString();
                    ArrayList arr_listBus2 = new ArrayList();
                    split(listBus, ref arr_listBus2);
                    int len_arrB2 = arr_listBus2.Count;

                    string NewListBus = "";
                    for (int i = 0; i < len_arrB1; i++)
                    {
                        for (int j = 0; j < len_arrB2; j++)
                        {
                            if (arr_listBus1[i].ToString() == arr_listBus2[j].ToString())
                            {
                                NewListBus = NewListBus + "," + arr_listBus1[i].ToString();
                            }

                        }

                    }
                    NewListBus = NewListBus.Remove(0, 1);
                    cmd.Parameters.Add("geometry", MIDbType.FeatureGeometry);
                    cmd.Parameters.Add("style", MIDbType.Style);
                    cmd.Parameters.Add("IdPoint", MIDbType.String, 100);
                    cmd.Parameters.Add("NearPoint", MIDbType.String, 100);
                    cmd.Parameters.Add("ListBus", MIDbType.String, 100);
                    //them vao coso du lieu
                    cmd.CommandText = "Insert Into tbPoint (MI_Geometry,MI_Style,IdPoint,NearPoint,ListBus) values (geometry,style,IdPoint,NearPoint,ListBus)";
                    cmd.Prepare();
                    cmd.Parameters[0].Value = geometry;
                    cmd.Parameters[1].Value = cStyle;
                    cmd.Parameters[2].Value = idpoint;
                    cmd.Parameters[3].Value = id1 + "," + id2;
                    cmd.Parameters[4].Value = NewListBus;
                    int nchanged = cmd.ExecuteNonQuery();




                    //update bang tbLine
                    DataTable dtPoints = new DataTable();
                    DataRow dr = null;
                    string strQuery = "Select * from tbLine where IdLine = '" + rc[0][1].ToString() + "'";

                    dtPoints = GetDataTable("tbLine", strQuery);
                    Feature ft = nchan[0];
                    dr = dtPoints.Rows[0];
                    IdLine = dr["IdLine"].ToString();
                    //tinh lai cac diem tren Line
                    PointinLine = dr["Points"].ToString();
                    string[] strTem = PointinLine.Split(',');
                    int lenStrTem = strTem.Length;

                    string NewPoints = "";

                    for (int i = 0; i < lenStrTem; i++)
                    {
                        NewPoints = NewPoints + "," + strTem[i];
                        if (strTem[i] == id1)
                            NewPoints = NewPoints + "," + idpoint;
                    }
                    NewPoints = NewPoints.Remove(0, 1);
                    //update tbLine

                    cmd.CommandText = "update tbLine set Points='" + NewPoints + "' where IdLine='" + IdLine + "'";
                    cmd.Prepare();
                    nchanged = cmd.ExecuteNonQuery();

                }
                else  //neu la diem doc lap thi insert lun vao
                {
                    //insert
                    cmd.Parameters.Add("geometry", MIDbType.FeatureGeometry);
                    cmd.Parameters.Add("style", MIDbType.Style);
                    cmd.Parameters.Add("IdPoint", MIDbType.String, 100);
                    cmd.Parameters.Add("NearPoint", MIDbType.String, 100);
                    cmd.Parameters.Add("ListBus", MIDbType.String, 100);
                    //them vao coso du lieu
                    cmd.CommandText = "Insert Into tbPoint (MI_Geometry,MI_Style,IdPoint,NearPoint,ListBus) values (geometry,style,IdPoint,NearPoint,ListBus)";
                    cmd.Prepare();
                    cmd.Parameters[0].Value = geometry;
                    cmd.Parameters[1].Value = cStyle;
                    cmd.Parameters[2].Value = idpoint;
                    cmd.Parameters[3].Value = nearpoint;
                    cmd.Parameters[4].Value = listbus;
                    int nchanged = cmd.ExecuteNonQuery();


                }
                connect.Close();
                cmd.Dispose();
            }

            mapControl1.Update();

            //foreach (string filename in arrfilename)
            //{
            //    //--------------if filename(*.gst)
            //    if (filename.ToLower().EndsWith(MapLoader.FileExtensionGST))
            //    {
            //        mapControl1.Map.Load(new MapGeosetLoader(filename)); // add geoset
            //    }
            //    //--------------if filename(*.mws)
            //    else if (filename.ToLower().EndsWith(MapLoader.FileExtensionWOR))
            //    {
            //        mapControl1.Map.Load(new MapWorkSpaceLoader(filename));  // add workspace

            //    }
            //    //--------------if filename(*.tab)
            //    else
            //    {
            //        mapControl1.Map.Load(new MapTableLoader(filename));  // add table
            //    }
            //}
            mapControl1.Refresh();

        }
        private bool CheckPoint(DPoint p1, DPoint p2, DPoint p3)//Kt p2 c nam giua (p1,p3)
        {

            double d12 = Distance1(p1, p2);
            double d23 = Distance1(p2, p3);
            double d13 = Distance1(p1, p3);

            double delta = Math.Abs(d13 - d12 - d23);
            if (delta < 0.005)
                return true;
            else
                return false;

        }
        public delegate void PassData(string value1, string value2, string value3, string value4, string value5);
        static public PassData passData;
        private void CreateForm()
        {
            Form2 fr = new Form2();
            fr.passData = new Form2.PassData(GetValue);
            fr.Show();

            if (passData != null)
            {
                passData(p_addpoint.X.ToString(), p_addpoint.Y.ToString(), Idpoint, Nearpoint, Listbus);
            }

            //this.Hide();

        }
        private void thêmĐiểmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add = 1;
        }
    }
}
