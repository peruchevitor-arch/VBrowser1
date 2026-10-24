using Microsoft.Web.WebView2.WinForms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
namespace Teste;

public partial class Form1 : Form
{
    private WebView2 navegador;
    private Panel barraSuperior = new Panel();[DllImport("user32.dll")]
    private static extern bool ReleaseCapture();[DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    // codigo para redimensionar as janelas \\
    protected override void WndProc(ref Message m)
    {
        const int WM_NCHITTEST = 0x84;

        if (m.Msg == WM_NCHITTEST && WindowState == FormWindowState.Normal)
        {
            Point mouse = PointToClient(Cursor.Position);
            int borda = 8;

            bool esquerda = mouse.X < borda;
            bool direita = mouse.X >= ClientSize.Width - borda;
            bool cima = mouse.Y < borda;
            bool baixo = mouse.Y >= ClientSize.Height - borda;

            if (cima && esquerda) m.Result = (IntPtr)13;
            else if (cima && direita) m.Result = (IntPtr)14;
            else if (baixo && esquerda) m.Result = (IntPtr)16;
            else if (baixo && direita) m.Result = (IntPtr)17;
            else if (esquerda) m.Result = (IntPtr)10;
            else if (direita) m.Result = (IntPtr)11;
            else if (cima) m.Result = (IntPtr)12;
            else if (baixo) m.Result = (IntPtr)15;
            else
            {
                base.WndProc(ref m);
                return;
            }

            return;
        }

        base.WndProc(ref m);
    }
    public Form1()
    {
        InitializeComponent();
        MaximizedBounds = Screen.FromControl(this).WorkingArea;
        FormBorderStyle = FormBorderStyle.None;
        BackColor = Color.Black;
        barraSuperior.BackColor = Color.Black;
        barraSuperior.Dock = DockStyle.Top;
        barraSuperior.Height = 45;
        barraSuperior.MouseDown += (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        };

        Controls.Add(barraSuperior);

        
        // ----------------------- Botões ------------ \\
        var btnF = new PictureBox();
        var btnM = new PictureBox();
        var btnMM = new PictureBox();

        btnF.Image = Image.FromFile("Assets/fechar.png");
        btnM.Image = Image.FromFile("Assets/minimizar.png");
        btnMM.Image = Image.FromFile("Assets/maximizar.png");
       

        int posX = 10;

        foreach (PictureBox btn in new[] { btnF, btnM, btnMM })
        {
            btn.Location = new Point(posX, 14);
            posX += 26;

            btn.Text = "";
            btn.Size = new Size (16,16);
            btn.SizeMode = PictureBoxSizeMode.Zoom;
        }
        // funções do botão

        btnF.Click +=(s,e) => Close();
        btnM.Click +=(s,e) => WindowState = FormWindowState.Minimized;
        btnMM.Click +=(s,e) =>
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                return;
            }
            WindowState = FormWindowState.Maximized;
        };

        barraSuperior.Controls.Add(btnF);
        barraSuperior.Controls.Add(btnM);
        barraSuperior.Controls.Add(btnMM);
        // --------------------------------------------------------------------------\\
    }
}
