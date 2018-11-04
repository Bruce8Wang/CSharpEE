<%@ Page Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        Load += (sender, e1) =>
        {
            string vNum = "X13B";
            int gheight = (int)(vNum.Length * 11.5);
            Bitmap img = new Bitmap(gheight, 22);
            Graphics g = Graphics.FromImage(img);
            g.FillRectangle(new SolidBrush(Color.MintCream), new Rectangle(0, 0, 100, 50));
            g.DrawString(vNum, (new Font("Arial", 9)), (new SolidBrush(Color.Black)), 3, 3);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            Response.ClearContent();
            Response.ContentType = "image/jpeg";
            Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            img.Dispose();
            Response.End();
        };
    }
</script>