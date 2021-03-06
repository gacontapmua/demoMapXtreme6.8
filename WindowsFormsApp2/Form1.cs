using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;




using MapInfo.Data;
using MapInfo.Mapping;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Styles;
using MapInfo.Tools;
using MapInfo.Windows.Controls;
using MapInfo.Windows.Dialogs;
using Point = System.Drawing.Point;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }
        #region KHAI BAO
        static TimeSpan waitTime = new TimeSpan(0, 0, 1);
          
        
      //bien thong bao chon them diem hay them duong
        int add = 0;//them diem
        //bien dung de nhan biet
        int action = 0;
        private Map _map = null;
        private Map map1 = null;
        private MIConnection connect = new MIConnection();
      
        private Selection _selection = Session.Current.Selections.DefaultSelection;
        private Catalog _catalog = Session.Current.Catalog;
        private Table _tblTemp = null;
       // string chon = "";
        int count = 0;
        
        //bien danh cho viec them duong
       // DPoint DP1, DP2, DP3;
        DPoint    DP1 = new DPoint();
        DPoint    DP2 = new DPoint();
       // bool d1 = false;
        //bool d2 = false;
        int countline = 0;
       
        //bien de tim kiem
       bool bus = false;
        bool startPoint = true;
        private int[] ParentPoint;
        private int iAllPoints = 0;
        private int iAllLines = 0;
        private Graph gr = new Graph();
        TableInfoMemTable tableInfo;
        Table pointTable;
        IResultSetFeatureCollection fc_point = null;
        IResultSetFeatureCollection fc_line = null;


        MapInfo.Data.TableInfo ti ;//= MapInfo.Data.TableInfoFactory.CreateTemp("temp2");
        MapInfo.Data.Table table ;//= MapInfo.Engine.Session.Current.Catalog.CreateTable(ti);
        IResultSetFeatureCollection nchan;
        System.Drawing.Point p_addpoint = new System.Drawing.Point();
        string Idpoint="";
        string Nearpoint="";
        string Listbus = "";
        int p_data = 0;
        double distance = 0;
        //cho bang Line
        string IdLine = "";
        string NameLine = "";
        string PointinLine = "";
        System.Drawing.Point pt1 = new System.Drawing.Point();
        System.Drawing.Point pt2 = new System.Drawing.Point();

        //su dung cho form dong
        //public delegate void PassData(string value1, string value2, string value3, string value4, string value5);

       // public PassData passData;
        System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //doi tuong chuyen dong
        double x_m = 0;
        double y_m = 0;
        BitmapPointStyle  m_BitmapSymbol = new BitmapPointStyle();

        DPoint[] arr_p;
        #endregion

        #region CHUONG TRINH CON LOAD BAN DO VAO FORM


        string[] arrfilename;
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
        #endregion

        #region BUILD GRAPH

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
                        string strQuery = "Select * from tbPoint where IdPoint = '" + (i+1).ToString() + "'";
                        
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
        private void AddListPointsToComboBox(string tab,string fname,ref System.Windows.Forms.ComboBox com)
        {
            try
            {
                MapInfo.Data.Table tblPoint = _catalog.GetTable(tab);
                string strQuery = "Select * from "+tab;
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

        #endregion

        #region CONTROL FORM

        private void mnuOpen_Click(object sender, System.EventArgs e)
        {
            OpenTable();
        }

        private void mnuClear_Click(object sender, System.EventArgs e)
        {
            mapControl1.Map.Clear();
        }

        private void mnuExit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, System.EventArgs e)
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

        private void mapControl1_MouseDown(object sender, MouseEventArgs e)
        {

            #region Tim duong giua hai diem
           
            if (action==1)
            {
                Point_startend(e.X,e.Y);
            }

            #endregion

            #region Them diem


            if (add==1)//them diem
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
                if (Idpoint=="")
                {
                    Idpoint = Convert.ToString(iAllPoints+1);
                    
                }
                 CreateForm();


             }
             if (add == 3)//edit diem
             {
                 p_addpoint = new System.Drawing.Point(e.X, e.Y);
                 System.Drawing.Point pt4 = p_addpoint;//new System.Drawing.Point ();
                 
                 SearchInfo si4 = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, pt4, 10);
                 IResultSetFeatureCollection fc4 = _catalog.Search("tbPoint", si4);

                 _selection.Clear();
                 _selection.Add(fc4);

                 if (fc4.Count > 0)
                 {

                     IResultSetFeatureCollection rc4 = Session.Current.Selections.DefaultSelection[Session.Current.Catalog.GetTable("tbPoint")];

                     Idpoint = rc4[0][1].ToString();
                 }
                 CreateForm();

             }
            #endregion

            #region Them duong
             
           
             if (add == 2)
            {
                bool ktp1 = false;//bien kiem tra xem p1 co trong csdl chua
                bool ktp2 = false;
                string point1 = "";
                string point2 = "";
                p_addpoint = new System.Drawing.Point(e.X, e.Y);
                if (countline == 0)
                {
                    pt1 = p_addpoint;
                    countline++;

                   
                }
                else
                {
                    pt2 = p_addpoint;
                    countline=0;
                   
                    #region tim xem DP1 va DP2 co trong CSDL chua
                    SearchInfo si1 = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, pt1, 40);
                    IResultSetFeatureCollection fc1 = _catalog.Search("tbPoint", si1);

                    _selection.Clear();
                    _selection.Add(fc1);

                    if (fc1.Count > 0)
                    {

                        IResultSetFeatureCollection rc1 = Session.Current.Selections.DefaultSelection[Session.Current.Catalog.GetTable("tbPoint")];
                        if (rc1.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy điểm nào");
                        }
                        else
                        {
                            point1 = rc1[0][1].ToString();
                            ktp1 = true;
                            txtStart.Text = point1;
                        }

                    }

                    SearchInfo si2 = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, pt2, 40);
                    IResultSetFeatureCollection fc2 = _catalog.Search("tbPoint", si2);

                    _selection.Clear();
                    _selection.Add(fc2);

                    if (fc2.Count > 0)
                    {

                        IResultSetFeatureCollection rc2 = Session.Current.Selections.DefaultSelection[Session.Current.Catalog.GetTable("tbPoint")];

                        if (rc2.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy điểm nào");
                        }
                        else
                        {
                            point2 = rc2[0][1].ToString();
                            ktp2 = true;
                            txtEnd.Text = point2;
                        }

                    }
                    if ((ktp1)&&(ktp2))
                    {
                         CreateFormLine();
                    }

                    #endregion

                }

            }
            if (add==4)//edit duong
            {
                p_addpoint = new System.Drawing.Point(e.X, e.Y);
                System.Drawing.Point pt3=p_addpoint;//new System.Drawing.Point ();
               
                SearchInfo si3 = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, pt3, 10);
                    IResultSetFeatureCollection fc3 = _catalog.Search("tbLine", si3);

                    _selection.Clear();
                    _selection.Add(fc3);

                    if (fc3.Count > 0)
                    {

                        IResultSetFeatureCollection rc3 = Session.Current.Selections.DefaultSelection[Session.Current.Catalog.GetTable("tbLine")];

                        IdLine = rc3[0][1].ToString();
                    }
                    CreateFormLine();
                
            }


            #endregion

            #region
            Point mousePT = new Point(e.X, e.Y);
            Double X = 0;
            Double Y = 0;
            DPoint outPoint = new DPoint(0, 9);
            //Lấy tọa độ hiển thị lên
            _map.DisplayTransform.ToDisplay(new DPoint(X, Y), out mousePT);
            // _map.DisplayTransform.FromDisplay( (mousePT, outPoint);
            MapInfo.Mapping.LayerType[] normalLyr = new MapInfo.Mapping.LayerType[1];
            normalLyr[0] = MapInfo.Mapping.LayerType.Normal;
            IMapLayerFilter filter = MapInfo.Mapping.MapLayerFilterFactory.FilterByLayerType(normalLyr);
            MapInfo.Mapping.FeatureLayer ftrLayer;
            foreach (FeatureLayer item in _map.Layers.GetMapLayerEnumerator(filter))
            {

            }
          
            #endregion
            //Tìm kiếm
          
        }

        private void displayForm(System.Windows.Forms.Form frm,int x,int y)
        {
            System.Drawing.Point clickPoint = new System.Drawing.Point(x,y);
            frm.Location = clickPoint;
            frm.ShowDialog();

        }
      



        private void cobBusNumber_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            action=2;//tim theo xe bus

        }
        private void tsmAddPoint_Click(object sender, EventArgs e)
        {
            add = 1;//them diem
            action = 0;
        }
        private void tsmEditPoint_Click(object sender, EventArgs e)
        {
            add = 3;
            action = 0;

        }

        private void tsmAddline_Click(object sender, EventArgs e)
        {
            add = 2;//them duong
            action = 0;
        }

        private void tsmEditLine_Click(object sender, EventArgs e)
        {
            add = 4;
            action = 0;

        }
        #region Nut cho tim kiem xe bus hoac tim duong

       
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            DPoint dau = new DPoint();
            DPoint cuoi = new DPoint();

            BuildGraphPoints();

            arr_p = new DPoint[1];
            DPoint[] arr_points = new DPoint[1];
            if (action==2)//tim theo xe bus
            {
               
                //goi chuong trinh con tim xe bus
                ArrayList l = new ArrayList();//danh sach cac diem ma xe bus di qua
                ArrayList d = new ArrayList();
                search_bus(comBusNum.Text, ref l);
                dau = new DPoint(gr.ListNodes[Convert.ToInt16(l[0].ToString()) - 1].X, gr.ListNodes[Convert.ToInt16(l[0].ToString()) - 1].Y);
                cuoi = new DPoint(gr.ListNodes[Convert.ToInt16(l[l.Count-1].ToString()) - 1].X, gr.ListNodes[Convert.ToInt16(l[l.Count-1].ToString()) - 1].Y);
                arr_p = new DPoint[l.Count];
                //gan vao mang arr_p de cho doi tuong chuyen dong
                for (int i = 0; i < arr_p.Length; i++)
                {
                    arr_p[i] = new DPoint(gr.ListNodes[Convert.ToInt16(l[i].ToString()) - 1].X, gr.ListNodes[Convert.ToInt16(l[i].ToString()) - 1].Y);
                    
                }
                Line(l, ref d);
                string du = "";
                for (int i = 0; i < d.Count; i++)
                {
                    du += "->" + d[i].ToString();

                }
                du = du.Remove(0, 2);
                ricPaths.Text = du;
                string diemqua = "";
                for (int i = 0; i < l.Count - 1; i++)
                {
                    
                    diemqua += "-" + l[i].ToString();
                    //hien thi duong di
                    DPoint d1 = new DPoint();
                    DPoint d2 = new DPoint();
                    d1.x = gr.ListNodes[Convert.ToInt16(l[i].ToString()) - 1].X;
                    d1.y = gr.ListNodes[Convert.ToInt16(l[i].ToString()) - 1].Y;
                    d2.x = gr.ListNodes[Convert.ToInt16(l[i + 1].ToString()) - 1].X;
                    d2.y = gr.ListNodes[Convert.ToInt16(l[i + 1].ToString()) - 1].Y;
                    DrawLine(d1, d2);
                }
                diemqua += "-" + l[l.Count-1].ToString();
                diemqua = diemqua.Remove(0, 1);
                MessageBox.Show("Di qua cac diem: " + diemqua);
            }
            if (action == 1)//tim duong khi biet diem dau va diem cuoi
            {
                
                ArrayList sPath = new ArrayList();
                A_sao(txtStart.Text, txtEnd.Text, ref sPath);
                Graph.Node tem =( Graph.Node) sPath[0];
                
                dau = new DPoint(tem.X, tem.Y);
                tem = ( Graph.Node)sPath[sPath.Count - 1];
                cuoi = new DPoint(tem.X,tem.Y); 
               
                arr_points = new DPoint[sPath.Count];
                ArrayList lb = new ArrayList();
                ArrayList lp = new ArrayList();
                
                for (int i = 0; i < sPath.Count; i++)
                {
                    Graph.Node d = (Graph.Node)sPath[i];
                  
                    lp.Add(Convert.ToString(d.ID));
                    if (i < sPath.Count - 1)
                    {
                        Graph.Node d_i1 = (Graph.Node)sPath[i + 1];
                        DPoint d1 = new DPoint();
                        DPoint d2 = new DPoint();
                        d1.x = d.X;
                        d1.y = d.Y;
                        d2.x = d_i1.X;
                        d2.y = d_i1.Y;
                        arr_points[i] = new DPoint(d1.x,d1.y);
                        DrawLine(d1, d2);
                        distance += Distance((Graph.Node)sPath[i], (Graph.Node)sPath[i + 1]);

                    }
                    else
                    {
                        Graph.Node d_i1 = (Graph.Node)sPath[i];
                        DPoint d1 = new DPoint();
                       
                        d1.x = d_i1.X;
                        d1.y = d_i1.Y;
                        arr_points[i] = new DPoint(d1.x, d1.y);
                    }

                  
                }
                distance += Distance((Graph.Node)sPath[sPath.Count-2], (Graph.Node)sPath[sPath.Count-1]);
        
                ListBus(lp, ref lb);
                string buses = "";
                if (lb.Count > 0)
                {
                    for (int i = 0; i < lb.Count; i++)
                    {
                        buses += " , " + "("+lb[i].ToString()+")";

                    }
                    buses = buses.Remove(0, 3);
                    txtBus.Text = buses;
                }
                else
                {
                    txtBus.Text = "Không có xe bus nào đi qua hai điểm này";
                }
                txtDistance.Text = Convert.ToString(distance);
                //gan vao mang moi cho doi tuong chuyen dong
                arr_p=new DPoint [arr_points.Length+2];
                arr_p[0] = DP1;
                arr_p[arr_p.Length - 1] = DP2;
                for (int i = 0; i < arr_points.Length; i++)
                {
                    arr_p[i + 1] = arr_points[i];
                    
                }
                
            }
            
            
            MoveObject(arr_p);
            System.Drawing.Point dau1=new System.Drawing.Point ();
             System.Drawing.Point cuoi1=new System.Drawing.Point ();
            mapControl1.Map.DisplayTransform.ToDisplay(dau,out dau1);
            mapControl1.Map.DisplayTransform.ToDisplay(cuoi, out cuoi1);
            _map.SetView(dau1,cuoi1,ZoomType.ZoomIn);
           
           
        }
        #endregion

        private void toolStripMenuItem4_Click(object sender, System.EventArgs e)
        {
            add = 0;
            (pointTable as IFeatureCollection).Clear();
            (table as IFeatureCollection).Clear();
            //chon tim duong khi biet diem dau, diem cuoi
            bus = false;
            iAllPoints = 0;
            startPoint = true;
            action = 1;
            count = 0;
            txtStart.Text = "";
            txtEnd.Text = "";
            connect.Close();
            distance = 0;
          // BuildGraphPoints();
           
        }

        private void toolStripMenuItem6_Click(object sender, System.EventArgs e)
        {
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            add = 0;
            (pointTable as IFeatureCollection).Clear();
            (table as IFeatureCollection).Clear();
            action = 2;
          
        }



        private void tsmPoint_Click(object sender, EventArgs e)
        {
            add = 0;//them diem
            action = 0;
        }
        private void tsmLine_Click(object sender, EventArgs e)
        {
            add = 0;//them duong
            action = 0;
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            add = 0;
            action = 0;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            add = 0;
            action = 0;
        }

        private void comBusNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            (pointTable as IFeatureCollection).Clear();
            (table as IFeatureCollection).Clear();
        }
        #endregion

        #region LAY DIEM DAU VA DIEM CUOI TREN BAN DO
        private void Point_startend(int x,int y)
        {
            DPoint DP_e = new DPoint();
               
            if (startPoint)//lay diem dau
            {
                (pointTable as IFeatureCollection).Clear();
                (table as IFeatureCollection).Clear();
                 System.Drawing.Point point = new System.Drawing.Point(x, y);
                // DPoint DP = new DPoint();
                
                 mapControl1.Map.DisplayTransform.FromDisplay(point, out DP1);
                
                SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, point,1000);
                IResultSetFeatureCollection fc = _catalog.Search("tbPoint", si);
                DrawPoint(DP1,fc[0]);
                _selection.Clear();
                _selection.Add(fc);
               
                if (fc.Count != 0)
                {
                    IResultSetFeatureCollection rc = Session.Current.Selections.DefaultSelection[Session.Current.Catalog.GetTable("tbPoint")];
                    if (rc.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy điểm nào");
                    }
                    else
                    {
                       txtStart.Text = rc[0][1].ToString();
                       DP_e.x= gr.ListNodes[Convert.ToInt16(txtStart.Text)-1].X;
                       DP_e.y= gr.ListNodes[Convert.ToInt16(txtStart.Text)-1].Y;
                       DrawLine(DP1,DP_e);
                       Graph.Node temp_start = new Graph.Node(0, DP1.x, DP1.y, 0, 0, 0, false);
                       distance += Distance(temp_start, gr.ListNodes[Convert.ToInt16(txtStart.Text) - 1]);
                       startPoint = false;
                       
                    }
                }
            }
            else  //lay diem cuoi
            {
                System.Drawing.Point pt = new System.Drawing.Point(x, y);
                mapControl1.Map.DisplayTransform.FromDisplay(pt, out DP2);
               
                SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, pt,1000);
                IResultSetFeatureCollection fc = _catalog.Search("tbPoint", si);
                 DrawPoint(DP2,fc[0]);
                _selection.Clear();
                _selection.Add(fc);
                if (fc.Count != 0)
                {

                    IResultSetFeatureCollection rc = Session.Current.Selections.DefaultSelection[Session.Current.Catalog.GetTable("tbPoint")];
                    if (rc.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy điểm nào");
                    }
                    else
                    {
                        txtEnd.Text = rc[0][1].ToString();
                        DP_e.x = gr.ListNodes[Convert.ToInt16(txtEnd.Text) - 1].X;
                        DP_e.y = gr.ListNodes[Convert.ToInt16(txtEnd.Text) - 1].Y;
                        DrawLine(DP2, DP_e);
                        Graph.Node temp_start = new Graph.Node(0, DP2.x, DP2.y, 0, 0, 0, false);
                        distance += Distance(temp_start, gr.ListNodes[Convert.ToInt16(txtEnd.Text) - 1]);
                        startPoint = false;
                        
                    }

                }
            }
          
                
        }
        #endregion

        #region HIEN THI DIEM
        private void DrawPoint(DPoint p, Feature f)
        {
            
            connect.Open();
            FeatureGeometry geometry = new MapInfo.Geometry.Point(mapControl1.Map.GetDisplayCoordSys(), p);
            SimpleVectorPointStyle vStyle = f.Style as SimpleVectorPointStyle;//new SimpleVectorPointStyle(34, System.Drawing.Color.Red, 18);
            CompositeStyle cStyle = new MapInfo.Styles.CompositeStyle(vStyle);
            
            ////insert vao bang tam de hien thi tren CSDL
            MICommand cmd = connect.CreateCommand();
            cmd.Parameters.Add("geometry", MIDbType.FeatureGeometry);
            cmd.Parameters.Add("style", MIDbType.Style);
            cmd.Parameters.Add("tid", MIDbType.String);
            cmd.CommandText = "Insert Into temp (MI_Geometry,MI_Style,ID) values (geometry,style,tid)";
            cmd.Prepare();
            cmd.Parameters[0].Value = geometry;
            cmd.Parameters[1].Value = cStyle;
            cmd.Parameters[2].Value = "1";
            int nchanged = cmd.ExecuteNonQuery();
            connect.Close();
            cmd.Dispose();
            
        }
        //Hiện đối tượng hình học 
        private void showFeatureGeometry(FeatureGeometry g)
        {
            try
            {
                TableInfo tbi = TableInfoFactory.CreateTemp("temp1");
                if (_tblTemp != null)
                    _tblTemp.Close();
                _tblTemp = _catalog.CreateTable(tbi);
                _map.Layers.Insert(_map.Layers.Count,new FeatureLayer(_tblTemp));
                //=====================lay icon cua diem de chuyen dong==============
                SimpleVectorPointStyle vStyle = fc_point[0].Style as SimpleVectorPointStyle;
                CompositeStyle cStyle = new MapInfo.Styles.CompositeStyle(vStyle);
                vStyle.Color = System.Drawing.Color.Black;
                vStyle.PointSize = 40;
           
                Style s = null;
                if (g is IGenericSurface)
                {
                    s = new AreaStyle(new SimpleLineStyle(new LineWidth(2, LineWidthUnit.Pixel), 2, Color.Black, false), new SimpleInterior(0));
                }
                else if (g is MapInfo.Geometry.Point)
                {
                    s = new SimpleVectorPointStyle(100, Color.Black, 180);
                }
                Feature f = new Feature(g, cStyle);
                _tblTemp.InsertFeature(f);
               
                
            }
            catch
            {
                MessageBox.Show("Có lỗi gì ấy");
            }
        }
        #endregion

        #region VE DUONG THANG NOI HAI DIEM TREN BAN DO
        private void DrawLine(DPoint DP_start, DPoint DP_end)
        {
            
            MapInfo.Geometry.FeatureGeometry g = MapInfo.Geometry.MultiCurve.CreateLine(this.mapControl1.Map.GetDisplayCoordSys(), DP_start, DP_end);
            //set line style
            MapInfo.Styles.SimpleLineStyle bl = fc_line[0].Style as SimpleLineStyle;
           
           if (action!=0)
            {
                bl = new MapInfo.Styles.SimpleLineStyle(new MapInfo.Styles.LineWidth(6, MapInfo.Styles.LineWidthUnit.Pixel),2);
            }
            MapInfo.Styles.CompositeStyle cs = new MapInfo.Styles.CompositeStyle(null, bl, null, null);

            MapInfo.Data.Feature f = new MapInfo.Data.Feature(g, cs);
            table.InsertFeature(f);

            FeatureLayer fl = new FeatureLayer(table);
            this.mapControl1.Map.Layers.Add(fl);
        }
        #endregion

        #region TIM DUONG CHO MOT XE BUS
        private void search_bus(string bus, ref ArrayList lotrinh)
        {
            string sP, eP;
            // lay diem bat dau va diem ket thuc
            MIConnection connect = new MIConnection();
            connect.Open();
            MICommand cmd = connect.CreateCommand();
            //luu tong so dinh cua do thi
            cmd.CommandText = "select * from tbPoint";
            cmd.Prepare();
            IResultSetFeatureCollection nchanged = cmd.ExecuteFeatureCollection();
            bool [] daxet=new bool [nchanged.Count+1];
            /////
           
            cmd.CommandText = "select * from tbBUS where IdBus='"+bus+"'";
            cmd.Prepare();
            nchanged = cmd.ExecuteFeatureCollection();
            sP=nchanged[0][2].ToString();
            eP=nchanged[0][3].ToString();
            lotrinh.Add(sP);
            string nP = sP;
            ArrayList arrNear = new ArrayList();
             ArrayList arrBus=new  ArrayList();
            string path = "";
            string lBus = "";
           
            while (nP!=eP)
            {
                bool ok = false;
                cmd.CommandText = "select * from tbPoint where IdPoint='"+nP+"'";
                cmd.Prepare();
                nchanged = cmd.ExecuteFeatureCollection();
                split(nchanged[0][2].ToString(),ref arrNear);
               //Xet het cac diem ke
                for (int i = 0; i < arrNear.Count; i++)//xet het cac dinh ke
                {

                    if (!daxet[System.Convert.ToInt16(arrNear[i].ToString())])
                    {
                       
                        cmd = connect.CreateCommand();
                        cmd.CommandText = "select * from tbPoint where IdPoint='" + arrNear[i] + "'";
                        cmd.Prepare();
                        nchanged = cmd.ExecuteFeatureCollection();
                        lBus = nchanged[0][3].ToString();
                        split(lBus, ref arrBus);
                        //voi moi diem ke ta xet cac xeBus di qua diem do, neu co xe can tim thi nap vao
                        for (int j = 0; j < arrBus.Count; j++)//xet het cac xe trong dinh ke
                        {

                            if (arrBus[j].ToString() == bus)
                            {
                                ok = true;
                                break;
                            }
                        }
                        arrBus = new ArrayList();
                        //neu da tim thay xe Bus trong tap diem ke thi thoat khoi vong lap
                        if (ok)
                        {
                          
                           daxet[System.Convert.ToInt16(nP)] = true;
                           // daxet.Add(nP);
                            lotrinh.Add(arrNear[i].ToString());
                            nP = arrNear[i].ToString();
                            arrNear = new ArrayList();
                            arrBus = new ArrayList();

                            break;

                        }
                    }

                }


                
            }
           
        }

        //hien thi duong cho danh sach diem da duoc chon
        private void Line(ArrayList lotrinh, ref ArrayList duong)
        {
            string n1 = "";
            string n2 = "";
            MIConnection connect = new MIConnection();
            connect.Open();
            MICommand cmd = connect.CreateCommand();
            cmd.CommandText = "select * from tbLine";// where Id='" + bus + "'";
            cmd.Prepare();
            IResultSetFeatureCollection nchanged = cmd.ExecuteFeatureCollection();
            for (int i = 0; i < lotrinh.Count-1; i++)
            {
                n1 = lotrinh[i].ToString();
                n2=lotrinh[i + 1].ToString();
               
               
                for (int j = 0; j < nchanged.Count; j++)
                {
                    ArrayList arr = new ArrayList();
                    split(nchanged[j][3].ToString(),ref arr);
                    if (Timxau(n1,n2,arr))
                    {
                        if (duong.Count>0)
                        {
                            if (duong[duong.Count-1].ToString()!=nchanged[j][2].ToString())
                            {
                                duong.Add(nchanged[j][2].ToString());
                                n1 = "";
                                n2 = "";
                            }
                            
                        }
                        else
                        {
                            duong.Add(nchanged[j][2].ToString());
                            n1 = "";
                            n2 = "";
                        }
                        break;
                        
                    }
                    
                }
          
                
            }
            
        }
        private bool Timxau(string s,string e, ArrayList arr)
        {
            bool ok = false;
            for (int i = 0; i < arr.Count-1; i++)
            {
                if (((arr[i].ToString()==s)&&(arr[i+1].ToString()==e))||((arr[i+1].ToString()==s)&&(arr[i].ToString()==e)))
                {
                    ok = true;
                    return ok;
                }
            }
            return ok;
        }
        //hai ham nay chua viet
         private void split(string str, ref ArrayList arrstr)
        {
           
            int t = -1;//vi tri truoc
             for (int i = 0; i < str.Length; i++)
            {
                if (str[i]==',')
                {
                    arrstr.Add( str.Substring(t+1, i-t-1));
                    t = i;
                  
                }
                
            }
            arrstr.Add(str.Substring(t + 1,str.Length-t-1));
           
        }
        
        
        #endregion

        #region TIM DUONG KHI CHO DIEM DAU VA DIEM CUOI
        private void A_sao(string sP, string eP,ref ArrayList shortPath)
        {
           
            int d =Convert.ToInt16(sP);
            int c = Convert.ToInt16(eP);
            d--;
            c--;

            
            //luu danh sach dinh tim
            ArrayList L = new ArrayList();
            gr.ListNodes[d].G = 0;
            gr.ListNodes[d].H = Distance(gr.ListNodes[d], gr.ListNodes[c]);
            gr.ListNodes[d].F = gr.ListNodes[d].H + gr.ListNodes[d].G;
            gr.ListNodes[d].daxet = true;
            L.Add(gr.ListNodes[d]);
            if (L.Count==0)
            {
                MessageBox.Show("Không tìm được đường đi");
                
            }
            Graph.Node u =(Graph.Node) L[0];
            shortPath.Add(u);
            //bat dau vong lap
            while ((u.ID.ToString() != eP) && (L.Count != 0))
            {

                L = new ArrayList();

                //lay tap ke cua u
                string xxx = "";
                for (int i = 0; i < u.ListAdjNode.Count; i++)
                {

                    string v = u.ListAdjNode[i].ToString();
                    xxx = v;
                    int d_v = Convert.ToInt16(v);
                    d_v--;

                    if (!gr.ListNodes[d_v].daxet)
                    {

                        gr.ListNodes[d_v].G = u.G + Distance(gr.ListNodes[d_v], u);
                        gr.ListNodes[d_v].H = Distance(gr.ListNodes[d_v], gr.ListNodes[c]);
                        gr.ListNodes[d_v].F = gr.ListNodes[d_v].G + gr.ListNodes[d_v].H;
                        L.Add(gr.ListNodes[d_v]);
                        gr.ListNodes[d_v].daxet = true;

                    }
                }
                //sap xep sao cho so co f nho nhat dung dau danh sach
                Graph.Node n_temp = (Graph.Node)L[0];
                double tem = n_temp.F;
                int count = L.Count;

                for (int i = 1; i < count; i++)
                {
                   
                    Graph.Node n_temp2 = (Graph.Node)L[i];

                    if (tem > n_temp2.F)
                    {
                        Graph.Node t = (Graph.Node)L[0];
                        L.Insert(0, (Graph.Node)L[i]);
                        L.RemoveAt(1);
                        L.Insert(i, t);
                        L.RemoveAt(i + 1);
                    }
                }
                u = (Graph.Node)L[0];
                shortPath.Add(u);
            }
                //tuc la da tim thay duong
                if (u.ID.ToString() == eP)//tim duoc duong
                {
                    MessageBox.Show("Tim duoc duong di");


                }
                else
                {
                    MessageBox.Show("Khong tim duoc duong");
                }


            

        }

        //hien thi danh sach xe Bus di tren duong do
        private void ListBus(ArrayList lPoint, ref ArrayList part_ListBus)
        {
            ArrayList lBus_temp3 = new ArrayList();
            ArrayList lBus = new ArrayList();
            string partPath = "";
            MIConnection connect = new MIConnection();
            connect.Open();
            MICommand cmd = connect.CreateCommand();
           
      
            for (int i = 0; i < lPoint.Count; i++)
            {
                if (i >= 1)
                {
                    lBus_temp3 = new ArrayList();
                    cmd.CommandText = "select ListBus from tbPoint where IdPoint='" + lPoint[i-1].ToString() + "'";
                    cmd.Prepare();
                    IResultSetFeatureCollection nchanged3 = cmd.ExecuteFeatureCollection();
                    string Buses3 = nchanged3[0][0].ToString();
                    split(Buses3, ref lBus_temp3);
 
                }
                cmd.CommandText = "select ListBus from tbPoint where IdPoint='" + lPoint[i].ToString() + "'";
                cmd.Prepare();
                IResultSetFeatureCollection nchanged = cmd.ExecuteFeatureCollection();
                string Buses = nchanged[0][0].ToString();
                ArrayList lBus_temp = new ArrayList();
                split(Buses, ref lBus_temp);
                if (lBus.Count == 0)
                {
                    split(Buses, ref lBus);
                }
                else
                {
                    ArrayList lBus_temp2 = new ArrayList();
                    for (int j = 0; j < lBus_temp.Count; j++)
                    {
                        bool coxeBus = false;
                        for (int k = 0; k < lBus.Count; k++)
                        {
                            if (lBus_temp[j].ToString() == lBus[k].ToString())
                            {
                                coxeBus = true;
                                break;

                            }

                        }
                        if (coxeBus)
                        {
                            lBus_temp2.Add(lBus_temp[j].ToString());

                        }

                    }
                    //cap nhat lai danh sach xe Bus
                    lBus = new ArrayList();
                    if (lBus_temp2.Count>0)
                    {
                        partPath = "";
                    }
                    for (int j = 0; j < lBus_temp2.Count; j++)
                    {
                        lBus.Add(lBus_temp2[j].ToString());
                        partPath += "," + lBus_temp2[j].ToString();
                        
                    }
                   
                    if ((lBus.Count==0) || (i==lPoint.Count-1))//phai di nhieu chang
                    {
                        partPath = partPath.Remove(0, 1);
                        part_ListBus.Add(partPath);
                        if (i==lPoint.Count-1)
                        {
                            break;
                            
                        }
                        lBus = lBus_temp3;
                        i--;
                        
                        partPath = "";
                    }
                    
                }

                
            }
        }
      
      
        private double Distance(Graph.Node  n1, Graph.Node  n2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((n1.X - n2.X), 2) + Math.Pow((n1.Y - n2.Y), 2));
            return d;
        }
        private double Distance1(DPoint n1, DPoint n2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((n1.x - n2.x), 2) + Math.Pow((n1.y - n2.y), 2));
            return d;
        }
       
        #endregion

        #region THAO TAC TREN BANG tbPoint

        #region SUA DIEM
        
       
        private void EditPoint(string ipoint,string npoint,string lstbus)
        {
            connect.Open();
            MICommand cmd = connect.CreateCommand();
            //update
           
            cmd.CommandText = "update tbPoint set NearPoint='"+npoint+"',ListBus='"+lstbus+"' where IdPoint='"+ipoint+"'";
            cmd.Prepare();
            int nchanged = cmd.ExecuteNonQuery();

            connect.Close();
            cmd.Dispose();
            mapControl1.Update();
            mapControl1.Refresh();

        }
        #endregion


        #region XOA DIEM


        private void DeletePoint(string ipoint)
        {
            connect.Open();
            MICommand cmd = connect.CreateCommand();
            //select cac diem ke
            cmd.CommandText = "select NearPoint from tbPoint where IdPoint='" + ipoint + "'";

            cmd.Prepare();
            IResultSetFeatureCollection nchanged = cmd.ExecuteFeatureCollection();
            string nP = nchanged[0][0].ToString();
            //delete
            cmd.CommandText = "delete from tbPoint where IdPoint='" + ipoint + "'";
            cmd.Prepare();
            int k = cmd.ExecuteNonQuery();
           
           // IResultSetFeatureCollection nchanged = cmd.ExecuteFeatureCollection();
             //edit cac Point khac co diem ke voi Point da xoa
          
            ArrayList arr_nP = new ArrayList();
            split(nP, ref arr_nP);
            for (int i = 0; i < arr_nP.Count; i++)
            {

                cmd.CommandText = "select NearPoint from tbPoint where IdPoint='" + arr_nP[i].ToString() + "'";
                cmd.Prepare();
                nchanged = cmd.ExecuteFeatureCollection();
                string near_temp = nchanged[0][0].ToString();
                ArrayList arrnear_temp = new ArrayList();
                split(near_temp, ref arrnear_temp);
                near_temp = "";
                for (int j = 0; j < arrnear_temp.Count; j++)
                {
                    if (arrnear_temp[j].ToString() != ipoint)
                    {
                        near_temp += "," + arrnear_temp[j].ToString();

                    }


                }
                near_temp = near_temp.Remove(0, 1);
                //update tro lai csdl
                cmd.CommandText = "update tbPoint set NearPoint='" + near_temp + "' where IdPoint='" + arr_nP[i].ToString() + "'";
                cmd.Prepare();
                int nch = cmd.ExecuteNonQuery();
            }
            connect.Close();
            cmd.Dispose();

            //update tbLine


            connect.Open();
            cmd = connect.CreateCommand();
            cmd.CommandText = "select * from tbLine";
            cmd.Prepare();
            string idl = "";

            nchanged = cmd.ExecuteFeatureCollection();
            string pointsline="";
            ArrayList a = new ArrayList();
            for (int j = 0; j < nchanged.Count; j++)
            {
                ArrayList arr = new ArrayList();
                pointsline = nchanged[j][3].ToString();
                split(pointsline, ref arr);
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].ToString() == ipoint)
                    {

                        idl = nchanged[j][1].ToString();

                        break;
                    }

                }
                if (idl != "")
                {
                    break;

                }


            }
            split(pointsline, ref a);
            pointsline = "";
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].ToString() != ipoint)
                {
                    pointsline += "," + a[i].ToString();

                }

            }
            pointsline = pointsline.Remove(0, 1);
            cmd.CommandText = "update tbLine set Points='" + pointsline + "' where IdLine='" + idl + "'";// where Id='" + bus + "'";
            cmd.Prepare();
            int cmd_run = cmd.ExecuteNonQuery();
        }

        #endregion


        #region THEM DIEM

        private bool CheckPoint(DPoint p1, DPoint p2,DPoint p3)//Kt p2 c nam giua (p1,p3)
        {
           
            double d12 = Distance1(p1, p2);
            double d23 = Distance1(p2, p3);
            double d13 = Distance1(p1,p3);
            
            double delta=Math.Abs(d13-d12-d23);
            if (delta < 0.005)
                return true;
            else
                return false;

        }

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
                SimpleVectorPointStyle vStyle =fc_point[0].Style as SimpleVectorPointStyle;
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
                        near1="";
                        for (int i = 0; i < lenNear1; i++)
                        {
                            if (arrNear1[i] != id2)
                                near1 = near1 + "," + arrNear1[i];
                            else
                                near1 = near1 + "," + idpoint;

                        }
                        near1 = near1.Remove(0, 1);



                        cmd.CommandText = "select NearPoint from tbPoint where IdPoint='" + id2+ "'";
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
                        cmd.Parameters[3].Value = id1+","+id2;
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
                      NewPoints = NewPoints.Remove(0,1);
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
        #endregion


    
        #endregion
        //=====================================
        #region THAO TAC TREN BANG tbLine


        #region THEM DUONG


        private void AddLine(string point1, string point2)
        {
            connect.Open();
            bool kt_ke = true;
            DPoint p1 = new DPoint();
            DPoint p2 = new DPoint();

            #region Ve duong
            DataTable dtPoints = new DataTable();
            string strQuery = "Select * from tbPoint where IdPoint = '" + point1 + "'";
            dtPoints = GetDataTable("tbPoint", strQuery);
            Feature ft = nchan[0];
            double x = ft.Geometry.Centroid.x;
            double y = ft.Geometry.Centroid.y;
            p1 = new DPoint(x, y);
            //deim 2
            strQuery = "Select * from tbPoint where IdPoint = '" + point2 + "'";
            dtPoints = GetDataTable("tbPoint", strQuery);
            ft = nchan[0];
            x = ft.Geometry.Centroid.x;
            y = ft.Geometry.Centroid.y;
            p2 = new DPoint(x, y);
           
            MapInfo.Geometry.FeatureGeometry geometry = MapInfo.Geometry.MultiCurve.CreateLine(this.mapControl1.Map.GetDisplayCoordSys(), p1, p2);
            MapInfo.Styles.SimpleLineStyle vStyle = fc_line[0].Style as SimpleLineStyle;
            MapInfo.Styles.CompositeStyle cStyle = new MapInfo.Styles.CompositeStyle(null, vStyle, null, null);
            MapInfo.Data.Feature f = new MapInfo.Data.Feature(geometry, cStyle);
             
            #endregion

         
              MICommand cmd = connect.CreateCommand();

           #region kiem tra xem duong nay da ton tai chua
		 
	
                cmd.CommandText = "select NearPoint from tbPoint where IdPoint='" + point1 + "'";
                IResultSetFeatureCollection n_nearpoint = cmd.ExecuteFeatureCollection();
                string ke = n_nearpoint[0][0].ToString();
                if (ke != "")
                {
                    ArrayList arr_ke = new ArrayList();
                    split(ke, ref arr_ke);
                    for (int i = 0; i < arr_ke.Count; i++)
                    {
                        if (arr_ke[i].ToString() == point2)
                        {
                            kt_ke = false;
                            break;

                        }

                    }
                }
             #endregion
            if (kt_ke)//duong nay chua ton tai
            {
                #region Kiem tra point1 hay point2 là doc lap lu vao bien point_depent
                string point_depent = "";//diem trognn tp lien thong
                string point_indepent = "";
                //cmd.CommandText = "select * from tbPoint where IdPoint='"+point1+"'";
                //cmd.Prepare();
                //IResultSetFeatureCollection inde_point = cmd.ExecuteFeatureCollection();
                if (ke=="")//point 2 la diem trong thanh phan lien thong
                {
                    point_depent = point2;
                    point_indepent = point1;
                }
                else
                {
                    point_depent = point1;
                    point_indepent=point2;
                }

                #endregion

                #region kiem tra xem diem point co la diem dau hay diem cuoi cua xe bus nao ko
                string bus_se = "";
                cmd.CommandText = "select IdBus from tbBUS where StartPoint='"+point_depent+"'";
                cmd.Prepare();
                IResultSetFeatureCollection  inde_point = cmd.ExecuteFeatureCollection();
                int u1 = 0;
                ArrayList dsBus1 = new ArrayList();//luu nhung xe bus de update vao truong ListBus cua diem doc lap
                if (inde_point.Count > 0)//point_depent la diem start cua xe bus nay
                {
                    for (int i = 0; i < inde_point.Count; i++)
                    {
                        bus_se = inde_point[i][0].ToString();
                        dsBus1.Add(bus_se);
                        //update diem dau cua xe bus
                        cmd.CommandText = "update tbBUS set StartPoint='" + point_indepent + "' where IdBus='" + bus_se + "'";
                        cmd.Prepare();
                        u1 = cmd.ExecuteNonQuery();
                    }
                }
                //kiem tra diem cuoi
                 cmd.CommandText = "select IdBus from tbBUS where EndPoint='" + point_depent + "'";
                 cmd.Prepare();
                 inde_point = cmd.ExecuteFeatureCollection();
                 if (inde_point.Count > 0)//point_depent la diem cuoi cua xe bus nay
                    {
                        for (int i = 0; i < inde_point.Count; i++)
                        {
                            bus_se = inde_point[i][0].ToString();
                            dsBus1.Add(bus_se);
                            //update diem cuoi cua xe bus
                            cmd.CommandText = "update tbBUS set EndPoint='" + point_indepent + "' where IdBus='"+bus_se+"'";
                            cmd.Prepare();
                            u1 = cmd.ExecuteNonQuery();
                        }
                    }

                    if (dsBus1.Count == 0)//diem nay ko phai dau hay cuoi cua xe bus nao
                    {
                        MessageBox.Show("Đường này không được phép thêm", "Cảnh báo");
                        mapControl1.Refresh();
                    }
                    else
                    {
                        #region insert Line vao tbLine
                        //insert
                        IdLine = Convert.ToString(iAllLines + 1);
                        PointinLine = point1 + "," + point2;
                        cmd.Parameters.Add("geometry", MIDbType.FeatureGeometry);
                        cmd.Parameters.Add("style", MIDbType.Style);
                        cmd.Parameters.Add("IDLine", MIDbType.String, 100);
                        cmd.Parameters.Add("NameLine", MIDbType.String, 100);
                        cmd.Parameters.Add("Points", MIDbType.String, 100);
                        cmd.CommandText = "Insert Into tbLine (MI_Geometry,MI_Style,IDLine,NameLine,Points) values (geometry,style,IDLine,NameLine,Points)";
                        cmd.Prepare();
                        cmd.Parameters[0].Value = geometry;
                        cmd.Parameters[1].Value = cStyle;
                        cmd.Parameters[2].Value = IdLine;
                        cmd.Parameters[3].Value = NameLine;
                        cmd.Parameters[4].Value = PointinLine;
                        int nchanged = cmd.ExecuteNonQuery();
                        connect.Close();
                        cmd.Dispose();

                        #endregion

                        #region update lai cac diem ke cua hai diem dau va cuoi cua Line


                        //update lai cac diem ke cua p1 va p2
                        connect.Open();
                        cmd = connect.CreateCommand();
                        //diem point1
                        cmd.CommandText = "select NearPoint from tbPoint where IdPoint='" + point_depent + "'";
                        cmd.Prepare();
                        IResultSetFeatureCollection nchanged1 = cmd.ExecuteFeatureCollection();
                        string nP1 = nchanged1[0][0].ToString();
                        //lay danh sach xe bus qua hai diem
                       // string bus2diem = "";
                        bus_se = "";
                        for (int i = 0; i < dsBus1.Count; i++)
                        {
                            bus_se = bus_se + "," + dsBus1[i].ToString();
                            
                        }
                        //bo dau ,
                        bus_se = bus_se.Remove(0, 1);
                       
                            cmd.CommandText = "update tbPoint set NearPoint='" + nP1 + "," + point_indepent + "' where IdPoint='" + point_depent + "'";
                            cmd.Prepare();
                            int nch = cmd.ExecuteNonQuery();
                            cmd.CommandText = "update tbPoint set NearPoint='"  + point_depent + "',ListBus='"+bus_se+"' where IdPoint='" + point_indepent + "'";
                            cmd.Prepare();
                            nch = cmd.ExecuteNonQuery();
                           
                        connect.Close();
                        cmd.Dispose();
                        #endregion

                    }
                #endregion
                
      
            }
            else
            {
                MessageBox.Show("Đường này đã tồn tại");
                mapControl1.Refresh();
            }
            connect.Close();
            connect.Dispose();
            mapControl1.Update();
            mapControl1.Refresh();
        }
        #endregion

        #region SUA DUONG
        private void EditLine()
        {
            //chi thay doi ten duong ma ko can thay doi Points vi AddPoints da thay doi Points
            connect.Open();
            MICommand cmd = connect.CreateCommand();
            //update
            cmd.CommandText = "update tbLine set NameLine='" + NameLine +"' where IdLine='" + IdLine + "'";
            cmd.Prepare();
            int nchanged = cmd.ExecuteNonQuery();
            connect.Close();
            cmd.Dispose();
            mapControl1.Update();
            mapControl1.Refresh();
            
        }
        #endregion

        #region XOA DUONG
        private void DeleteLine()
        {
            //chi thay doi ten duong ma ko can thay doi Points vi AddPoints da thay doi Points
            connect.Open();
            MICommand cmd = connect.CreateCommand();
            //update
            cmd.CommandText = "delete from tbLine where IdLine='" + IdLine + "'";
            cmd.Prepare();
            int nchanged = cmd.ExecuteNonQuery();

            //xoa cac diem nam tren duong
            cmd.CommandText = "select Points from tbLine where IdLine='" + IdLine + "'";
            cmd.Prepare();
            IResultSetFeatureCollection nchanged1 = cmd.ExecuteFeatureCollection();
            ArrayList a = new ArrayList();
            split(nchanged1[0][0].ToString(),ref a);
            for (int i = 0; i < a.Count; i++)
            {
                DeletePoint(a[i].ToString());
                
            }
            connect.Close();
            cmd.Dispose();

        }
        #endregion

        #endregion

        #region DYNAMICFORM
        public delegate void PassData(string value1, string value2,string value3,string value4,string value5);
        static public PassData passData;
        private void CreateForm()
        {
            Form2 fr = new Form2();
            fr.passData = new Form2.PassData(GetValue);
            fr.Show();

            if (passData != null)
            {
                passData(p_addpoint.X.ToString(), p_addpoint.Y.ToString(),Idpoint,Nearpoint,Listbus);
            }

            //this.Hide();

        }
        public void GetValue(string value1, string value2, string value3,int value4)
        {
            Idpoint = value1;
            Nearpoint = value2;
            Listbus = value3;
            p_data = value4;
            if (p_data==1)
            {
                AddPoint(p_addpoint.X, p_addpoint.Y, Idpoint, Nearpoint, Listbus);  
            }
            if (p_data==2)
            {
                EditPoint(Idpoint, Nearpoint, Listbus);
                
            }
            if (p_data==3)
            {
                DeletePoint(Idpoint); 
            }
          
           // 
           // 

        }
        //gui du lieu sang form Line
      // PassData(string value1, string value2, string value3);
       // public delegate void PassData(string value1, string value2, string value3);
        static public PassData passDatal;
        private void CreateFormLine()
        {
            Form3 fr = new Form3();
            fr.passDatal = new Form3.PassData(GetValuel);
            fr.Show();

            if (passDatal != null)
            {
                passDatal(IdLine,NameLine,PointinLine,"","");
            }
        }
        public void GetValuel(string value1, string value2, string value3, int value4)
        {
            IdLine = value1;
            NameLine = value2;
            PointinLine = value3;
            p_data = value4;
            if (p_data == 1)
            {
                AddLine(txtStart.Text,txtEnd.Text);
            }
            if (p_data == 2)
            {
                EditLine();
              
            }
            

        }
       
        #endregion

        #region SET VIEW 
        
       
        private void comTrafic_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            DataTable dtPoints = new DataTable();
            string nameNode = comTrafic.SelectedItem.ToString();
            string strQuery = "Select * from tbNodeTrafic where NameNode = '" + nameNode + "'";

            dtPoints = GetDataTable("tbNodeTrafic", strQuery);
            Feature ft = nchan[0];
            SimpleVectorPointStyle vStyle = fc_point[0].Style as SimpleVectorPointStyle;
            vStyle.Color = System.Drawing.Color.Red;
            vStyle.PointSize = 60;
            CompositeStyle cStyle = new MapInfo.Styles.CompositeStyle(vStyle);
            //===========tao bang====================
            TableInfoMemTable tableInfo;
            Table pointTable;
            MapInfo.Data.Table tblTemp = Session.Current.Catalog.GetTable("temp3");
            if (tblTemp != null)
                tblTemp.Close();
            CoordSys coordSys = mapControl1.Map.GetDisplayCoordSys();
            tableInfo = new TableInfoMemTable("temp3");
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

            //=====================insert==============
            FeatureGeometry geometry = ft.Geometry;
            MICommand cmd = connect.CreateCommand();
            cmd.Connection.Open();
            ////insert vao bang tam de hien thi tren CSDL

            cmd.Parameters.Add("geometry", MIDbType.FeatureGeometry);
            cmd.Parameters.Add("style", MIDbType.Style);
            cmd.Parameters.Add("tid", MIDbType.String);
            cmd.CommandText = "Insert Into temp3 (MI_Geometry,MI_Style,ID) values (geometry,style,tid)";
            cmd.Prepare();
            cmd.Parameters[0].Value = geometry;
            cmd.Parameters[1].Value = cStyle;
            cmd.Parameters[2].Value = "1";
            int nchanged = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            //===========================
            _map.SetView(ft);
            mapControl1.Update();
            _selection.Clear();
            //_selection.Add(nchan);
        }

        private void comLake_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtPoints = new DataTable();
            string nameLake = comLake.SelectedItem.ToString();
            string strQuery = "Select * from tbLake where NameLake = '" + nameLake + "'";

            dtPoints = GetDataTable("tbLake", strQuery);
            Feature ft = nchan[0];
            showFeatureGeometry(ft.Geometry);
            _map.SetView(ft);
            mapControl1.Update();
            _selection.Clear();
            _selection.Add(nchan);
        }
        private void comOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtPoints = new DataTable();
            string nameOff = comOffice.SelectedItem.ToString();
            string strQuery = "Select * from tbOffice where NameOffice = '" + nameOff + "'";

            dtPoints = GetDataTable("tbOffice", strQuery);
            Feature ft = nchan[0];
            SimpleVectorPointStyle vStyle = fc_point[0].Style as SimpleVectorPointStyle;
            vStyle.Color = System.Drawing.Color.Red;
            vStyle.PointSize = 60;
            CompositeStyle cStyle = new MapInfo.Styles.CompositeStyle(vStyle);
            //===========tao bang====================
            TableInfoMemTable tableInfo;
            Table pointTable;
            MapInfo.Data.Table tblTemp = Session.Current.Catalog.GetTable("temp3");
            if (tblTemp != null)
                tblTemp.Close();
            CoordSys coordSys = mapControl1.Map.GetDisplayCoordSys();
            tableInfo = new TableInfoMemTable("temp3");
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

            //=====================insert==============

            FeatureGeometry geometry = ft.Geometry;
            MICommand cmd = connect.CreateCommand();
            cmd.Connection.Open();
            //===========insert vao bang tam de hien thi tren CSDL=====================

            cmd.Parameters.Add("geometry", MIDbType.FeatureGeometry);
            cmd.Parameters.Add("style", MIDbType.Style);
            cmd.Parameters.Add("tid", MIDbType.String);
            cmd.CommandText = "Insert Into temp3 (MI_Geometry,MI_Style,ID) values (geometry,style,tid)";
            cmd.Prepare();
            cmd.Parameters[0].Value = geometry;
            cmd.Parameters[1].Value = cStyle;
            cmd.Parameters[2].Value = "1";
            int nchanged = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            _map.SetView(ft);
            mapControl1.Update();
            _selection.Clear();
            _selection.Add(nchan);
        }
        #endregion

        #region DOI TUONG CHUYEN DONG
       private void MoveObject(DPoint[] p_colection)
        {
            double delta_d = 0.01;
            double sin = 0;
            double cos = 0;
            double delta_x = 0;
            double delta_y = 0;
            
            int bx = 0;
            int by = 0;
            
           //=====================lay icon cua diem de chuyen dong==============
            SimpleVectorPointStyle vStyle = fc_point[0].Style as SimpleVectorPointStyle;
            CompositeStyle cStyle = new MapInfo.Styles.CompositeStyle(vStyle);
            vStyle.Color = System.Drawing.Color.GreenYellow;
            vStyle.PointSize = 30;
           
           //======================
            //====tao bang====
            TableInfoMemTable tableInfo;
            Table pointTable;
            MapInfo.Data.Table tblTemp = Session.Current.Catalog.GetTable("temp3");
            if (tblTemp != null)
                tblTemp.Close();
            CoordSys coordSys = mapControl1.Map.GetDisplayCoordSys();
            tableInfo = new TableInfoMemTable("temp3");
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

            connect.Open();

            //====================================
            for (int i = 0; i < p_colection.Length-1; i++)
            {      
                cos = (Math.Abs(p_colection[i + 1].x - p_colection[i].x)) / d(p_colection[i], p_colection[i + 1]);
                sin = (Math.Abs(p_colection[i + 1].y - p_colection[i].y)) / d(p_colection[i], p_colection[i + 1]);
                if (p_colection[i+1].x>p_colection[i].x)
                {
                    bx = 1;       
                }
                else
                {
                    bx = -1;
                }

                if (p_colection[i + 1].y > p_colection[i].y)
                {
                    by = 1;

                }
                else
                {
                    by = -1;
                }
                delta_x=delta_d * cos * bx;
                delta_y=delta_d * sin * by;

               
                x_m = p_colection[i].x;
                y_m = p_colection[i].y;
                double te = d(new DPoint(x_m,y_m),p_colection[i+1]);
                
                while (te >=delta_d)//(((Math.Abs(te-(delta_d))>=0.05)))
                {
                    (pointTable as IFeatureCollection).Clear();
                    x_m += delta_x;
                    y_m += delta_y;
                    te = d(new DPoint(x_m, y_m), p_colection[i + 1]);
                    //==================hien thi doi tuong=======================
                    FeatureGeometry geometry = new MapInfo.Geometry.Point(mapControl1.Map.GetDisplayCoordSys(), new DPoint (x_m,y_m));
                    
                    MICommand cmd = connect.CreateCommand();
                    ////insert vao bang tam de hien thi tren CSDL
                   
                    cmd.Parameters.Add("geometry", MIDbType.FeatureGeometry);
                    cmd.Parameters.Add("style", MIDbType.Style);
                    cmd.Parameters.Add("tid", MIDbType.String);
                    cmd.CommandText = "Insert Into temp3 (MI_Geometry,MI_Style,ID) values (geometry,style,tid)";
                    cmd.Prepare();
                    cmd.Parameters[0].Value = geometry;
                    cmd.Parameters[1].Value = cStyle;
                    cmd.Parameters[2].Value = "1";
                    int nchanged = cmd.ExecuteNonQuery();
                    Thread.Sleep(50);
                    _map.SetView(geometry);
                    mapControl1.Refresh();
                 
                }
                (pointTable as IFeatureCollection).Clear();
               // timer1.Start();
                x_m = p_colection[i+1].x;
                y_m = p_colection[i+1].y;
              
            }
            connect.Close();
           // cmd.Dispose();
        }
        private double d(DPoint p1, DPoint p2)
        {
            return Math.Sqrt(((p1.x - p2.x) * (p1.x - p2.x)) + ((p1.y - p2.y) * (p1.y - p2.y)));
        }
          #endregion

        private void btnRun_Click(object sender, EventArgs e)
        {
            MoveObject(arr_p);
        }

       

       

       

    }
}