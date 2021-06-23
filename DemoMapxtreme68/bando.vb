Imports MapInfo.Data
Imports MapInfo.Engine
Imports MapInfo.Geometry
Imports MapInfo.Mapping
Imports MapInfo.Mapping.Legends
Imports MapInfo.Mapping.Thematics
Imports MapInfo.Styles
Imports MapInfo.Tools
Imports MapInfo.Windows.Controls

Public Class bando
    ''' <summary>
    ''' Create an adomment (title, scalebar, legend)
    ''' </summary>
    ''' <param name="myMap"></param>
    ''' <param name="fLyr"></param>
    Public Shared Sub MapInfo_Mapping_Adornments(ByVal myMap As Map, ByVal fLyr As FeatureLayer)
        ' Create a scalebar
        Dim sba As New ScaleBarAdornment(myMap)
        ' Position the scalebar at the lower right corner of map
        Dim x As Integer = (myMap.Size.Width - sba.Size.Width)
        Dim y As Integer = (myMap.Size.Height - sba.Size.Height)
        sba.Location = New System.Drawing.Point(x, y)

        ' Add the ScaleBarAdornment to the map
        myMap.Adornments.Append(sba)

        ' Create a TitleAdornment.
        Dim ta As New TitleAdornment(New Size(80, 120), myMap)
        ta.Title = "This is a title adornment."

        ' Position it.
        x = (myMap.Size.Width / 2)
        y = 0
        ta.Location = New System.Drawing.Point(x, y)

        ' Add the TitleAdornment to the map
        myMap.Adornments.Append(ta)

        ' Create a Legend adornment.
        Dim size As New System.Drawing.Size(400, 400)
        Dim legend As MapInfo.Mapping.Legends.Legend = myMap.Legends.CreateLegend("Legend_Name", "Legend_Alias", size)
        ' Create a CartographicLegendFrame.
        Dim frame As CartographicLegendFrame = LegendFrameFactory.CreateCartographicLegendFrame("CartFrameName", "CartFrameAlias", fLyr)

        ' Add CartographicLegendFrame into legend's frames collection
        legend.Frames.Append(frame)

        ' Add the Legend into the map's adornments collection
        myMap.Adornments.Append(legend)
    End Sub

    Public Shared Sub MapInfo_Mapping_Adornments_WithMapControl(ByVal mapControl1 As MapControl, ByVal fLyr As FeatureLayer)
        ' Create a scalebar
        Dim sba As New ScaleBarAdornment(mapControl1.Map)
        ' Position the scalebar at the lower right corner of map
        Dim x As Integer = (mapControl1.Map.Size.Width - sba.Size.Width)
        Dim y As Integer = (mapControl1.Map.Size.Height - sba.Size.Height)
        sba.Location = New System.Drawing.Point(x, y)
        ' Assign pen, brush to this adornment.
        sba.TextBrush = New System.Drawing.SolidBrush(Color.Red)
        sba.BackgroundBrush = New System.Drawing.SolidBrush(Color.Blue)
        sba.Border = True
        sba.BorderPen = New System.Drawing.Pen(Color.RosyBrown, 2.0F)

        ' Add the ScaleBarAdornment to the map
        Dim sbac As New ScaleBarAdornmentControl(sba, mapControl1.Map)
        mapControl1.AddAdornment(sba, sbac)

        ' Create a TitleAdornment.
        Dim ta As New TitleAdornment(New Size(80, 120), mapControl1.Map)
        ta.Title = "This is a title adornment."

        ' Position it.
        x = (mapControl1.Map.Size.Width / 2)
        y = 0
        ta.Location = New System.Drawing.Point(x, y)
        ' Assign Brush and Font to this TitleAdornment.
        ta.TextBrush = New SolidBrush(Color.DarkGray)
        ta.TextFont = New System.Drawing.Font(FontFamily.GenericSansSerif, 16)

        ' Add the TitleAdornment to the map
        Dim tac As New TitleAdornmentControl(ta)
        mapControl1.AddAdornment(ta, tac)

        ' Create a Legend adornment.
        Dim size As New System.Drawing.Size(400, 400)
        Dim legend As MapInfo.Mapping.Legends.Legend = mapControl1.Map.Legends.CreateLegend("Legend_Name", "Legend_Alias", size)
        ' Create a CartographicLegendFrame.
        Dim frame As CartographicLegendFrame = LegendFrameFactory.CreateCartographicLegendFrame("CartFrameName", "CartFrameAlias", fLyr)

        ' Add CartographicLegendFrame into legend's frames collection
        legend.Frames.Append(frame)

        ' Add the Legend into the map's adornments collection
        Dim lac As New LegendAdornmentControl(legend, mapControl1.Map)
        mapControl1.AddAdornment(legend, lac)
    End Sub


#Region "theme"
    Public Shared Sub MapInfo_Mapping_Thematics_BarTheme(ByVal map As Map)
        ' Load a map based on one table.
        map.Load(New MapTableLoader("C:\Program Files (x86)\MapInfo\MapXtreme\6.8.0\Samples\Data\world.tab"))
        Dim lyr As FeatureLayer = CType(map.Layers("world"), FeatureLayer)

        ' Create a new bar theme.
        Dim barTheme As MapInfo.Mapping.Thematics.BarTheme = New MapInfo.Mapping.Thematics.BarTheme(map, lyr.Table, "Pop_Native", "Pop_Asian", "Pop_Other")

        ' Create an object theme layer based on that bar theme.
        Dim thmLayer As ObjectThemeLayer = New ObjectThemeLayer("World Pop", Nothing, barTheme)

        ' Add object theme to the map's layer collection.
        map.Layers.Add(thmLayer)

        ' Stack the bars and graduate by a constant amount.
        barTheme.Stacked = True
        barTheme.GraduateSizeBy = GraduateSizeBy.Constant
        thmLayer.RebuildTheme()
    End Sub
    Public Shared Sub MapInfo_Mapping_Thematics_IndividualValueLabelTheme_RecomputeBins(ByVal theme As MapInfo.Mapping.Thematics.IndividualValueTheme)
        ' disable theme changed event firing before changing a bunch of theme properties.
        theme.SuppressThemeChangedEvents = True

        ' change theme's expression and set some bin values.
        theme.Expression = "Road Type"
        theme.Bins(0).Value = "Interstate"
        theme.Bins(1).Value = "State Highway"
        theme.Bins(2).Value = "County Highway"
        theme.Bins(3).Value = "Private Road"

        ' enable theme changed event firing.
        theme.SuppressThemeChangedEvents = False

        ' recompute the bins.
        theme.RecomputeBins()
    End Sub



#End Region
#Region "label"
    Public Shared Sub MapInfo_Mapping_HowDoICreateLabelLayerWithProperties(ByVal mapControl1 As MapControl)

        Dim labelLayer As New MapInfo.Mapping.LabelLayer("Label Layer", "Label Layer")
        mapControl1.Map.Layers.Add(labelLayer)

        Dim table As Table = MapInfo.Engine.Session.Current.Catalog.OpenTable("C:\Program Files (x86)\MapInfo\MapXtreme\6.8.0\Samples\Data\world.tab")
        Dim layer As New FeatureLayer(table)
        mapControl1.Map.Layers.Add(layer)

        Dim source As New MapInfo.Mapping.LabelSource(table)
        labelLayer.Sources.Append(source)

        source.DefaultLabelProperties.Visibility.Enabled = True
        source.DefaultLabelProperties.Visibility.VisibleRangeEnabled = True
        source.DefaultLabelProperties.Visibility.VisibleRange = New VisibleRange(1000, 2000, MapInfo.Geometry.DistanceUnit.Kilometer)
        source.DefaultLabelProperties.Visibility.AllowDuplicates = False
        source.DefaultLabelProperties.Visibility.AllowOverlap = True
        source.DefaultLabelProperties.Visibility.AllowOutOfView = True
        source.Maximum = 50
        source.DefaultLabelProperties.Layout.UseRelativeOrientation = True
        source.DefaultLabelProperties.Layout.RelativeOrientation = MapInfo.Text.RelativeOrientation.FollowPath
        source.DefaultLabelProperties.Layout.Angle = 33.0
        source.DefaultLabelProperties.Caption = "Capital"
        source.DefaultLabelProperties.Priority.Major = "Pop_1994"
        source.DefaultLabelProperties.Layout.Offset = 7
        source.DefaultLabelProperties.Layout.Alignment = MapInfo.Text.Alignment.BottomRight

    End Sub


#End Region
#Region "Features"
    Public Shared Sub MapInfo_Mapping_HowDoICreateFeatureAddToMap(ByVal mapControl1 As MapControl, ByVal connection As MIConnection, ByVal x As Double, ByVal y As Double)
        Dim map As Map = mapControl1.Map

        'uses wldcty25 as a template
        Dim table As Table = MapInfo.Engine.Session.Current.Catalog.GetTable("FIR")

        ' create a temp table and add a featurelayer for it
        Dim coordSys As CoordSys = map.GetDisplayCoordSys()
        Dim tableInfo As TableInfoMemTable = New TableInfoMemTable("temp")
        tableInfo.Temporary = True

        ' add Geometry column
        Dim column As Column

        ' specify coordsys for object column
        column = New GeometryColumn(coordSys)
        column.Alias = "MI_Geometry"
        column.DataType = MIDbType.FeatureGeometry
        tableInfo.Columns.Add(column)

        ' add style column
        column = New Column
        column.Alias = "MI_Style"
        column.DataType = MIDbType.Style
        tableInfo.Columns.Add(column)

        Dim pointTable As Table = Session.Current.Catalog.CreateTable(tableInfo)

        ' Set the location and display style of the point
        Dim Geometry As FeatureGeometry = New MapInfo.Geometry.Point(coordSys, x, y)
        Dim vStyle As SimpleVectorPointStyle = New SimpleVectorPointStyle(37, Color.Red, 14)
        Dim cStyle As CompositeStyle = New MapInfo.Styles.CompositeStyle(vStyle)

        'Update the table with the location and style of the new feature
        Dim cmd As MICommand = connection.CreateCommand()
        cmd.Parameters.Add("Geometry", MIDbType.FeatureGeometry)
        cmd.Parameters.Add("style", MIDbType.Style)
        cmd.CommandText = "Insert Into temp (MI_Geometry,MI_Style) values (Geometry,style)"
        cmd.Prepare()
        cmd.Parameters(0).Value = Geometry
        cmd.Parameters(1).Value = cStyle
        Dim nchanged As Integer = cmd.ExecuteNonQuery()
        cmd.Dispose()

        'add the table to the map
        map.Layers.Add(New MapInfo.Mapping.FeatureLayer(pointTable))
    End Sub


#End Region
#Region "Tool"
    Public Shared Sub MapInfo_Tools_HowDoICreateAddPolygonWithCustomProperties(ByVal mapControl As MapControl)
        ' Set the map layer filter for the insertion layer.
        Dim insertionLayerFilter As IMapLayerFilter = MapLayerFilterFactory.FilterByLayerType(LayerType.Normal)

        ' Set the default style for the new objects (red fill with blue border).
        Dim style As CompositeStyle = New CompositeStyle(
        New MapInfo.Styles.AreaStyle(
            MapInfo.Styles.StockStyles.BlueLineStyle(),
            MapInfo.Styles.StockStyles.RedFillStyle()),
            MapInfo.Styles.StockStyles.BlackLineStyle(),
            New MapInfo.Styles.TextStyle,
            MapInfo.Styles.StockStyles.DefaultSymbol())

        ' Create custom tool properties.
        Dim addMapToolProperties As AddMapToolProperties = New AddMapToolProperties(
        MapLayerFilterFactory.FilterForTools(mapControl.Map, insertionLayerFilter,
        MapLayerFilterFactory.FilterVisibleLayers(True),
        "CustomPolygonAddMapToolProperties", Nothing), style)

        ' Create an Add polygon tool with non-default properties.
        Dim maptool As MapTool = New AddPolygonMapTool(True, mapControl.Viewer,
        mapControl.Handle.ToInt32(), mapControl.Tools,
        New MouseToolProperties(Cursors.Default, Cursors.Default,
        Cursors.Default), mapControl.Tools.MapToolProperties,
        addMapToolProperties)

        ' Add it to the MapTools collection.
        mapControl.Tools.Add("CustomPolygonAddMapTool", maptool)
    End Sub

#End Region

End Class
