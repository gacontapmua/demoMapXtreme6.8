Imports System.Reflection
Imports System.IO
Imports MapInfo.Data
Imports MapInfo.Mapping
Imports MapInfo.Engine
Imports MapInfo.Windows.Dialogs

Imports System.Windows.Forms
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports MapInfo.Persistence
Imports System.Data

Imports MapInfo.Styles
Imports MapInfo.Windows.Controls
Imports MapInfo.Geometry

Public Class Form1
    Inherits System.Windows.Forms.Form
    Private _table As Table
    Private _layer As FeatureLayer
    Private _lineStyleDlg As LineStyleDlg = Nothing
    Private path As String = System.IO.Path.GetDirectoryName(
      System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)
    Private ud = New UserDrawHighlighter("Highlighted Cities", "userdraw")
    Private connect As New MIConnection()
    ' Private path As String = "D:\\Mayao\\Maps\\BanDoNguoc\\Maps\\"
    ''' <summary>
    ''' Mở Geoset
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'load geoset
        path = path.Substring(6)
        Dim gl As New MapGeosetLoader(path + "\Maps\BanDo.gst")
        MapControl1.Map.Load(gl)
        LayerControl1.Map = MapControl1.Map
        ' đặt mức zoom khi load bản đồ lên là 25000m

        Me.MapControl1.Map.Zoom = New MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
        ' now create and add our user draw layer to the map

        MapControl1.Map.Layers.Insert(0, ud)


        g = Me.MapControl1.CreateGraphics()
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
    End Sub
    ''' <summary>
    ''' Mở bảng .TAB
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim path As String = Environment.CurrentDirectory + "\Map"
        Session.Current.TableSearchPath.Path = path
        _table = Session.Current.Catalog.OpenTable("SongSuoiA.TAB")
        _layer = New FeatureLayer(_table)
        MapControl1.Map.Layers.Add(_layer)
    End Sub
    ''' <summary>
    ''' Lấy Tỷ lệ bản đồ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim dblScale As Double = System.Convert.ToDouble(String.Format("{0:E2}", MapControl1.Map.Scale.ToString()))
        MessageBox.Show("Scale=  1:" + dblScale.ToString())
    End Sub
    ''' <summary>
    ''' lưu geoset. với mapxtreme 6.8 có thể sử dụng được geoset của bản mapx 5.0 nhưng không có chiều ngược lại
    ''' Bản mapxtreme 6.8 dữ liệu worckspace lưu định dạng .wor
    ''' In MapX, you used the Map.SaveMapAsGeoset 
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim wsp As New WorkSpacePersistence()
        wsp.Save(path + "MapSave.mws")
        MessageBox.Show("Lưu thành công MapSave.mws")
    End Sub
    ''' <summary>
    ''' Remove Layer "SongSuoiA"
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim LayerThuyHe As FeatureLayer = MapControl1.Map.Layers("SongSuoiA")
        If Not LayerThuyHe Is Nothing Then
            MapControl1.Map.Layers.Remove(LayerThuyHe)
        End If
    End Sub
    ''' <summary>
    ''' Giá trị zoom map
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        ' Display the zoom level
        Dim dblZoom As Double = System.Convert.ToDouble(String.Format("{0:E2}", MapControl1.Map.Zoom.Value))
        MessageBox.Show("Zoom: " + dblZoom.ToString() + " " + MapControl1.Map.Zoom.Unit.ToString())
    End Sub
    ''' <summary>
    ''' Đổi style cho lớp đối tượng
    ''' cái này lưu ý phải add references là MapInfo.Windows.Framework (trong thư mục commonfile/mapinfo) 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim _lyr As FeatureLayer = Me.MapControl1.Map.Layers("DuongBoBien")
        'If _lineStyleDlg Is Nothing Then
        _lineStyleDlg = New LineStyleDlg
        'End If
        If _lineStyleDlg.ShowDialog() = DialogResult.OK Then
            Dim fsm As FeatureOverrideStyleModifier = New FeatureOverrideStyleModifier(Nothing, New MapInfo.Styles.CompositeStyle(_lineStyleDlg.LineStyle))
            _lyr.Modifiers.Append(fsm)
            Me.MapControl1.Map.Zoom = New MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
        End If
        ' tương tự cho các đối tượng vùng, điểm, nhãn
        'Private _areaStyleDlg As AreaStyleDlg = Nothing
        'Private _textStyleDlg As TextStyleDlg = Nothing
        'Private _symbolStyleDlg As SymbolStyleDlg = Nothing

        'Get the layer we want
        'Dim _lyr As FeatureLayer = Me.MapControl1.Map.Layers("world")
        ''Create and show the style dialog
        'If _areaStyleDlg Is Nothing Then
        '    _areaStyleDlg = New AreaStyleDlg
        'End If
        '' After getting style from dialog, create and apply the featureoverridestylemodifier object to layer
        'If _areaStyleDlg.ShowDialog() = DialogResult.OK Then
        '    Dim fsm As FeatureOverrideStyleModifier = New FeatureOverrideStyleModifier(Nothing, New MapInfo.Styles.CompositeStyle(_areaStyleDlg.AreaStyle))
        '    _lyr.Modifiers.Append(fsm)
        '    Me.MapControl1.Map.Zoom = New MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
        'End If
        '-------------
        'Dim _lyr As MapInfo.Mapping.LabelLayer = Me.MapControl1.Map.Layers("worldlabels")
        'If _textStyleDlg Is Nothing Then
        '    _textStyleDlg = New TextStyleDlg
        'End If
        'If _textStyleDlg.ShowDialog() = DialogResult.OK Then
        '    _lyr.Sources("world").DefaultLabelProperties.Style = New TextStyle(_textStyleDlg.FontStyle, _lyr.Sources("world").DefaultLabelProperties.Style.CalloutLine)
        '    _lyr.Sources("worldcap").DefaultLabelProperties.Style = New TextStyle(_textStyleDlg.FontStyle, _lyr.Sources("worldcap").DefaultLabelProperties.Style.CalloutLine)
        '    _lyr.Sources("wldcty25").DefaultLabelProperties.Style = New TextStyle(_textStyleDlg.FontStyle, _lyr.Sources("wldcty25").DefaultLabelProperties.Style.CalloutLine)
        '    Me.MapControl1.Map.Zoom = New MapInfo.Geometry.Distance(6250, MapInfo.Geometry.DistanceUnit.Mile)
        'End If
        '.......................
        'Dim _lyr As FeatureLayer = Me.MapControl1.Map.Layers("worldcap")
        'If _symbolStyleDlg Is Nothing Then
        '    _symbolStyleDlg = New SymbolStyleDlg
        'End If
        'If _symbolStyleDlg.ShowDialog() = DialogResult.OK Then
        '    Dim fsm As FeatureOverrideStyleModifier = New FeatureOverrideStyleModifier(Nothing, New MapInfo.Styles.CompositeStyle(_symbolStyleDlg.SymbolStyle))
        '    _lyr.Modifiers.Append(fsm)
        '    Me.MapControl1.Map.Zoom = New MapInfo.Geometry.Distance(6250, MapInfo.Geometry.DistanceUnit.Mile)
        'End If
        '------------------------------
        ' Get the layer we want
        'Dim _lyr As FeatureLayer = Me.MapControl1.Map.Layers("worldcap")
        ''Create a sparse point style
        'Dim vs As MapInfo.Styles.SimpleVectorPointStyle = New SimpleVectorPointStyle
        ''Just change the color and code and attributes flag to indicate that
        'vs.Code = 55
        'vs.PointSize = 25
        'vs.Color = System.Drawing.Color.Red
        '' vs.Attributes = StyleAttributes.PointAttributes.Color | StyleAttributes.PointAttributes.VectorCode;
        '' And apply to the layer
        'Dim fsm As FeatureOverrideStyleModifier = New FeatureOverrideStyleModifier(Nothing, New MapInfo.Styles.CompositeStyle(vs))
        '_lyr.Modifiers.Append(fsm)
        'Me.MapControl1.Map.Zoom = New MapInfo.Geometry.Distance(6250, MapInfo.Geometry.DistanceUnit.Mile)
    End Sub
    ''' <summary>
    ''' Hiển thị nhãn 'name' lớp 'FIR'
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim table As Table = Session.Current.Catalog.OpenTable("D:\Mayao\Maps\BanDoNguoc\Maps\Map\FIR.tab")
        Dim featureLayer As New FeatureLayer(table)
        MapControl1.Map.Layers.Add(featureLayer)
        ' label layer before the feature layer
        Dim labelLayer As New MapInfo.Mapping.LabelLayer()
        MapControl1.Map.Layers.Insert(0, labelLayer)
        Dim source As New MapInfo.Mapping.LabelSource(table)
        source.DefaultLabelProperties.Caption = "name"
        ' Append the label source to the label layer so that it shows on the map
        labelLayer.Sources.Append(source)
    End Sub
    ''' <summary>
    ''' ẩn hiện lớp dữ liệu Biên giới quốc gia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If (CheckBox1.Checked = True) Then

            MapControl1.Map.Layers.Item("BienGioiQuocGia").Enabled = True
        Else
            MapControl1.Map.Layers.Item("BienGioiQuocGia").Enabled = False
        End If

    End Sub

    Private Sub MapControl1_MouseDown(sender As Object, e As MouseEventArgs) Handles MapControl1.MouseDown
        'Cho phép vẽ đường thẳng bằng chuột
        'Me.TopMenuStrip.Show(MapControl1, New Point(e.X, e.Y))
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Dim table As Table = Session.Current.Catalog.OpenTable("D:\Mayao\Maps\BanDoNguoc\Maps\Map\FIR.tab")


        Dim featureLayer As New FeatureLayer(table)
        Dim cor As String
        cor = featureLayer.CoordSys.Datum.ToString()
        MessageBox.Show(cor.ToString())
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim table As Table = Session.Current.Catalog.OpenTable(path + "\\FIR.tab")
        Dim featureLayer As New FeatureLayer(table)
        bando.MapInfo_Mapping_Adornments(Me.MapControl1.Map, featureLayer)
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim table As Table = Session.Current.Catalog.OpenTable(path + "\\FIR.tab")
        Dim featureLayer As New FeatureLayer(table)
        bando.MapInfo_Mapping_Adornments_WithMapControl(Me.MapControl1, featureLayer)
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        'bando.MapInfo_Mapping_Thematics_BarTheme(Me.MapControl1.Map)
        bando.MapInfo_Mapping_HowDoICreateLabelLayerWithProperties(Me.MapControl1)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        connect.Open()
        bando.MapInfo_Mapping_HowDoICreateFeatureAddToMap(Me.MapControl1, connect, 0, 0)
        connect.Close()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        bando.MapInfo_Tools_HowDoICreateAddPolygonWithCustomProperties(Me.MapControl1)
    End Sub

    Private Sub VẽĐườngThẳngToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VẽĐườngThẳngToolStripMenuItem.Click
        'Dim dPoints As MapInfo.Geometry.DPoint()
        'dPoints(0) = New MapInfo.Geometry.DPoint(-103, 32)
        'dPoints(1) = New MapInfo.Geometry.DPoint(-97, 26)
        'dPoints(2) = New MapInfo.Geometry.DPoint(-88, 29)
        'dPoints(3) = New MapInfo.Geometry.DPoint(-94, 36)
        'dPoints(4) = New MapInfo.Geometry.DPoint(-103, 32)
        ''Create Object of FeatureGeometry Class
        'Dim g As MapInfo.Geometry.FeatureGeometry = New MapInfo.Geometry.MultiPolygon(MapControl1.Map.GetDisplayCoordSys(), MapInfo.Geometry.CurveSegmentType.Linear, dPoints)
        'Dim sis As MapInfo.Styles.SimpleInterior = New MapInfo.Styles.SimpleInterior(9, System.Drawing.Color.Purple)
        'Dim lw As MapInfo.Styles.LineWidth = New MapInfo.Styles.LineWidth(3, MapInfo.Styles.LineWidthUnit.Point)
        ''Apply Style over area
        'Dim sl As MapInfo.Styles.SimpleLineStyle = New MapInfo.Styles.SimpleLineStyle(lw, 3)
        'Dim ar As MapInfo.Styles.AreaStyle = New MapInfo.Styles.AreaStyle(sl, sis)
        ''Fill with CompositeStyle 
        'Dim cs As MapInfo.Styles.CompositeStyle = New MapInfo.Styles.CompositeStyle(ar, Nothing, Nothing, Nothing)

        'Dim f As Feature = New Feature(g, cs)
        'Dim ti As MapInfo.Data.TableInfo = MapInfo.Data.TableInfoFactory.CreateTemp("Temp")
        'Dim table As MapInfo.Data.Table = MapInfo.Engine.Session.Current.Catalog.CreateTable(ti)

        'Dim k As MapInfo.Data.Key = table.InsertFeature(f)
        Dim ti As MapInfo.Data.TableInfo = MapInfo.Data.TableInfoFactory.CreateTemp("Temp")
        Dim table As MapInfo.Data.Table = MapInfo.Engine.Session.Current.Catalog.CreateTable(ti)

        Dim startPoint As New MapInfo.Geometry.DPoint(-100, 40)
        Dim endPoint As New MapInfo.Geometry.DPoint(-70, 20)

        Dim g As MapInfo.Geometry.FeatureGeometry = MapInfo.Geometry.MultiCurve.CreateLine(Me.MapControl1.Map.GetDisplayCoordSys(), startPoint, endPoint)

        'set line style
        Dim bl As New MapInfo.Styles.SimpleLineStyle(New MapInfo.Styles.LineWidth(3, MapInfo.Styles.LineWidthUnit.Pixel), 2)
        Dim cs As New MapInfo.Styles.CompositeStyle(Nothing, bl, Nothing, Nothing)

        Dim f As New MapInfo.Data.Feature(g, cs)
        table.InsertFeature(f)

        Dim fl As New FeatureLayer(table)
        Me.MapControl1.Map.Layers.Add(fl)
    End Sub
    Public Shared Sub DrawLine(ByVal mapControl1 As MapControl, ByVal map As MapInfo.Mapping.Map, ByVal tableName As String, ByVal begDpoint As MapInfo.Geometry.DPoint,
        ByVal endDpoint As MapInfo.Geometry.DPoint, ByVal Pattoner As Integer, ByVal color As System.Drawing.Color)

        Dim myMap As MapInfo.Mapping.Map = MapInfo.Engine.Session.Current.MapFactory(mapControl1.Map.Alias)
        Dim workLayer As FeatureLayer = myMap.Layers(tableName + "layer")
        Dim Table As MapInfo.Data.Table = workLayer.Table

        Dim mc As MapInfo.Geometry.MultiCurve = MapInfo.Geometry.MultiCurve.CreateLine(myMap.GetDisplayCoordSys(), begDpoint, endDpoint)
        Dim bl As MapInfo.Styles.SimpleLineStyle = New MapInfo.Styles.SimpleLineStyle(New MapInfo.Styles.LineWidth(2, MapInfo.Styles.LineWidthUnit.Pixel), Pattoner, color)
        Dim cs As MapInfo.Styles.CompositeStyle = New MapInfo.Styles.CompositeStyle(Nothing, bl, Nothing, Nothing)
        Dim f As MapInfo.Data.Feature = New Feature(Table.TableInfo.Columns)
        f.Geometry = mc
        f.Style = cs
        Table.InsertFeature(f)

    End Sub
    Private m_PointsClick As New ArrayList()
    Private m_drawPoint As New ArrayList()
    Private g As Graphics
    Dim pointsPen As Pen = New Pen(Color.Red)
    Private Sub MapControl1_MouseClick(sender As Object, e As MouseEventArgs) Handles MapControl1.MouseClick
        If e.Button = MouseButtons.Right Then
            Dim m_Point As System.Drawing.Point = New System.Drawing.Point(e.X, e.Y)
            m_drawPoint.Add(m_Point)
            'Đầu tiên cần xác định điểm Click trên màn hình và chuyển đổi sang tọa độ bản đồ.
            Dim dPoint As New DPoint()
            MapControl1.Map.DisplayTransform.FromDisplay(m_Point, dPoint)
            m_PointsClick.Add(dPoint)

            g.DrawRectangle(pointsPen, m_Point.X - 4, m_Point.Y - 4, 4 * 2, 4 * 2)
            If m_PointsClick.Count > 1 Then

                ud.diemdau = m_drawPoint(m_drawPoint.Count - 2)
                ud.diemcuoi = m_drawPoint(m_drawPoint.Count - 1)

                g.DrawLine(pointsPen, m_drawPoint(m_drawPoint.Count - 2), m_drawPoint(m_drawPoint.Count - 1))
            End If

        End If




    End Sub
End Class
