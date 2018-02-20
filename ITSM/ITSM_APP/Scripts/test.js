function Export(){
    //1创建
    var XLObj = new ActiveXObject("Excel.Application" );
    var xlBook = XLObj.Workbooks.Add; //新增工作簿
    var ExcelSheet = xlBook.Worksheets(1); //创建工作表

    //2.保存表格
    ExcelSheet.SaveAs("C:\\TEST.XLS" );

    //3.使 Excel 通过 Application 对象可见
    ExcelSheet.Application.Visible = true;

    //4.打印
    xlBook.PrintOut;
    //或者:
    ExcelSheet.PrintOut;

    //5.关闭
    xlBook.Close(savechanges=false);
    //或者:
    ExcelSheet.Close(savechanges=false);

    //6.结束进程
    ExcelSheet.Application.Quit();
    //或者:
    XLObj.Quit();
    XLObj=null;

    //7.页面设置
    ExcelSheet.ActiveSheet.PageSetup.LeftMargin= 2/0.035;
    //页边距 左2厘米
    ExcelSheet.ActiveSheet.PageSetup.RightMargin = 3/0.035;
    //页边距右3厘米
    ExcelSheet.ActiveSheet.PageSetup.TopMargin = 4/0.035;
    //页边距上4厘米
    ExcelSheet.ActiveSheet.PageSetup.BottomMargin = 5/0.035;
    //页边距下5厘米
    ExcelSheet.ActiveSheet.PageSetup.HeaderMargin = 1/0.035;
    //页边距页眉1厘米
    ExcelSheet.ActiveSheet.PageSetup.FooterMargin = 2/0.035;
    //页边距页脚2厘米
    ExcelSheet.ActiveSheet.PageSetup.CenterHeader = "页眉中部内容";
    ExcelSheet.ActiveSheet.PageSetup.LeftHeader = "页眉左部内容";
    ExcelSheet.ActiveSheet.PageSetup.RightHeader = "页眉右部内容";
    ExcelSheet.ActiveSheet.PageSetup.CenterFooter = "页脚中部内容";
    ExcelSheet.ActiveSheet.PageSetup.LeftFooter = "页脚左部内容";
    ExcelSheet.ActiveSheet.PageSetup.RightFooter = "页脚右部内容";

    //8.对单元格操作，带*部分对于行，列，区域都有相应属性
    ExcelSheet.ActiveSheet.Cells(row,col).Value = "内容";
    //设置单元格内容
    ExcelSheet.ActiveSheet.Cells(row,col).Borders.Weight = 1;
    //设置单元格边框*()
    ExcelSheet.ActiveSheet.Cells(row,col).Interior.ColorIndex = 1;
    //设置单元格底色*(1-黑色，2-白色，3-红色，4-绿色，5-蓝色，6-黄色，7-粉红色，8-天蓝色，9-酱土色..可以多做尝试)
    ExcelSheet.ActiveSheet.Cells(row,col).Interior.Pattern = 1;
    //设置单元格背景样式*(1-无，2-细网格，3-粗网格，4-斑点，5-横线，6-竖线..可以多做尝试)
    ExcelSheet.ActiveSheet.Cells(row,col).Font.ColorIndex = 1;
    //设置字体颜色*(与上相同)
    ExcelSheet.ActiveSheet.Cells(row,col).Font.Size = 10;
    //设置为10号字*
    ExcelSheet.ActiveSheet.Cells(row,col).Font.Name = "黑体";
    //设置为黑体*
    ExcelSheet.ActiveSheet.Cells(row,col).Font.Italic = true;
    //设置为斜体*
    ExcelSheet.ActiveSheet.Cells(row,col).Font.Bold = true;
    //设置为粗体*
    ExcelSheet.ActiveSheet.Cells(row,col).ClearContents;
    //清除内容*
    ExcelSheet.ActiveSheet.Cells(row,col).WrapText=true;
    //设置为自动换行*
    ExcelSheet.ActiveSheet.Cells(row,col).HorizontalAlignment = 3;
    //水平对齐方式枚举* (1-常规，2-靠左，3-居中，4-靠右，5-填充 6-两端对齐，7-跨列居中，8-分散对齐)
    ExcelSheet.ActiveSheet.Cells(row,col).VerticalAlignment = 2;
    //垂直对齐方式枚举*(1-靠上，2-居中，3-靠下，4-两端对齐，5-分散对齐)

    //行，列有相应操作:
    ExcelSheet.ActiveSheet.Rows(row).
    ExcelSheet.ActiveSheet.Columns(col).
    ExcelSheet.ActiveSheet.Rows(startrow+":"+endrow).
    //如Rows("1:5" )即1到5行
    ExcelSheet.ActiveSheet.Columns(startcol+":"+endcol).
    //如Columns("1:5" )即1到5列

    //区域有相应操作:
    XLObj.Range(startcell+":"+endcell).Select;
    //如Range("A2:H8" )即A列第2格至H列第8格的整个区域
    XLObj.Selection.

    //合并单元格
    XLObj.Range(startcell+":"+endcell).MergeCells = true;
    //如Range("A2:H8" )即将A列第2格至H列第8格的整个区域合并为一个单元格
    XLObj.Range("A2",XLObj.Cells(8, 8)).MergeCells = true;

    //9.设置行高与列宽
    ExcelSheet.ActiveSheet.Columns(startcol+":"+endcol).ColumnWidth = 22;
    //设置从firstcol到stopcol列的宽度为22
    ExcelSheet.ActiveSheet.Rows(startrow+":"+endrow).RowHeight = 22;
    //设置从firstrow到stoprow行的宽度为22
}