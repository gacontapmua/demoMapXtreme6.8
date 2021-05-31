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

Public Class Form1
    Inherits System.Windows.Forms.Form
    Private _table As Table
    Private _layer As FeatureLayer
    Private _lineStyleDlg As LineStyleDlg = Nothing
    Private path As String = "I:\May ao\Source Code\KCD-S-Git\KCD-S\bin\x86\Maps\Maps_55"
    ' Private path As String = "D:\\Mayao\\Maps\\BanDoNguoc\\Maps\\"
    ''' <summary>
    ''' Mở Geoset
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'load geoset
        Dim gl As New MapGeosetLoader(path + "\BanDo.gst")
        MapControl1.Map.Load(gl)
        LayerControl1.Map = MapControl1.Map
        ' đặt mức zoom khi load bản đồ lên là 25000m

        Me.MapControl1.Map.Zoom = New MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)

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

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Dim table As Table = Session.Current.Catalog.OpenTable("D:\Mayao\Maps\BanDoNguoc\Maps\Map\FIR.tab")


        Dim featureLayer As New FeatureLayer(table)
        Dim cor As String
        cor = featureLayer.CoordSys.Datum.ToString()
        MessageBox.Show(cor.ToString())
    End Sub
End Class
