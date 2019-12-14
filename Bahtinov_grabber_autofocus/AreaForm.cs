using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bahtinov_grabber_autofocus
{
  public class AreaForm : Form
  {
    public Point ClickPoint = new Point();
    public Point CurrentTopLeft = new Point();
    public Point CurrentBottomRight = new Point();
    public bool LeftButtonDown;
    public bool activated;
    public Bitmap areaPicture;

    private Pen MyPen = new Pen(Color.Blue, 1f);
    private Pen EraserPen = new Pen(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192), 1f);
    private Graphics g;
    private PictureBox pictureBox;
    private Button browseButton;

    public AreaForm()
    {
      this.InitializeComponent();
      this.MouseDown += new MouseEventHandler(this.Mouse_Click);
      this.MouseUp += new MouseEventHandler(this.Mouse_Up);
      this.MouseMove += new MouseEventHandler(this.Mouse_Move);
      this.pictureBox.MouseDown += new MouseEventHandler(this.PictureBox_Mouse_Down);
      this.pictureBox.MouseUp += new MouseEventHandler(this.PictureBox_Mouse_Up);
      this.pictureBox.MouseMove += new MouseEventHandler(this.PictureBox_Mouse_Move);
      this.MyPen.DashStyle = DashStyle.Dot;
      this.g = this.CreateGraphics();
    }

    private void InitializeComponent()
    {
     this.SuspendLayout();
      this.browseButton = new Button();
      this.browseButton.Location = new Point(5, 5);
      this.browseButton.Name = "BrowseButton";
      this.browseButton.Size = new Size(50, 25);
      this.browseButton.TabIndex = 0;
      this.browseButton.Text = "Browse...";
      this.browseButton.UseVisualStyleBackColor = true;
      this.browseButton.Click += new EventHandler(this.BrowseButton_Click);
      this.Controls.Add((Control)this.browseButton);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(320, 200);
      this.Cursor = Cursors.Arrow;
      this.FormBorderStyle = FormBorderStyle.Sizable;
      this.Name = "AreaForm";
      this.Text = "Star selection";
      this.pictureBox = new PictureBox();
      ((ISupportInitialize)this.pictureBox).BeginInit();
      this.pictureBox.Location = new Point(5, 40);
      this.pictureBox.Name = "pictureBox";
      this.pictureBox.Size = new Size(300, 250);
      this.pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox.BorderStyle = BorderStyle.FixedSingle;
      this.Controls.Add((Control)this.pictureBox);
      ((ISupportInitialize)this.pictureBox).EndInit();
      this.ResumeLayout(false);
    }

    private void BrowseButton_Click(object sender, EventArgs e)
    {
      this.pictureBox.Image = null;
      this.pictureBox.Size = new Size(300, 250);
      this.ClientSize = new Size(320, 200);
      if (this.areaPicture != null)
      {
        this.areaPicture.Dispose();
        this.areaPicture = null;
      }

      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      DialogResult result = openFileDialog1.ShowDialog();
      if (result == DialogResult.OK)
      {
        string file = openFileDialog1.FileName;
        Bitmap original_areaPicture = new Bitmap(file);
        Rectangle screenBounds = Screen.GetBounds(Screen.GetBounds(Point.Empty));
        float scale = Math.Min(1.0f, Math.Min((float)(screenBounds.Width - 50) / (float)original_areaPicture.Width, (float)(screenBounds.Height - 50) / (float)original_areaPicture.Height));
        this.areaPicture = new Bitmap(original_areaPicture, new Size((int)(original_areaPicture.Width * scale), (int)(original_areaPicture.Height * scale)));

        if (this.areaPicture != null)
        {
          this.ClientSize = new Size(Math.Min(screenBounds.Width, this.areaPicture.Width), Math.Min(screenBounds.Height, this.areaPicture.Height));
          if (this.areaPicture != null)
          {
            this.pictureBox.Size = new Size(Math.Min(screenBounds.Width, this.areaPicture.Width), Math.Min(screenBounds.Height, this.areaPicture.Height));
            this.pictureBox.Image = (Image)this.areaPicture;
          }
        }
      }
    }

    private void PictureBox_Mouse_Down(object sender, MouseEventArgs e)
    {
      MouseEventArgs eTrans = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
      this.OnMouseDown(eTrans);
    }

    private void PictureBox_Mouse_Up(object sender, MouseEventArgs e)
    {
      MouseEventArgs eTrans = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
      this.OnMouseUp(eTrans);
    }

    private void PictureBox_Mouse_Move(object sender, MouseEventArgs e)
    {
      MouseEventArgs eTrans = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
      this.OnMouseMove(eTrans);
    }

    private void Mouse_Click(object sender, MouseEventArgs e)
    {
      // this.g.Clear(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192));
      this.LeftButtonDown = true;
      this.ClickPoint = new Point(e.X, e.Y);
      this.activated = false;
    }

    private void Mouse_Up(object sender, MouseEventArgs e)
    {
      if (this.LeftButtonDown)
      {
        this.LeftButtonDown = false;
        this.activated = true;
        this.Hide();
      }
    }

    private void Mouse_Move(object sender, MouseEventArgs e)
    {
      int num = 1;
      if (this.LeftButtonDown)
      {
        // this.g.DrawRectangle(this.EraserPen, this.CurrentTopLeft.X, this.CurrentTopLeft.Y, this.CurrentBottomRight.X - this.CurrentTopLeft.X, this.CurrentBottomRight.Y - this.CurrentTopLeft.Y);
        num = Math.Max(Math.Abs(e.X - this.ClickPoint.X), Math.Abs(e.Y - this.ClickPoint.Y));
        this.CurrentTopLeft.X = this.ClickPoint.X - num;
        this.CurrentTopLeft.Y = this.ClickPoint.Y - num;
        this.CurrentBottomRight.X = this.ClickPoint.X + num;
        this.CurrentBottomRight.Y = this.ClickPoint.Y + num;
        this.g.DrawRectangle(this.MyPen, this.CurrentTopLeft.X, this.CurrentTopLeft.Y, this.CurrentBottomRight.X - this.CurrentTopLeft.X, this.CurrentBottomRight.Y - this.CurrentTopLeft.Y);
      }
    }
  }
}
