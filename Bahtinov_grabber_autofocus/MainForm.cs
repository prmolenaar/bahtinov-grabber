using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Bahtinov_grabber_autofocus
{
  public class MainForm : Form
  {
    private static int num_errorvalues = 150;
    private float[] bahtinov_angles = new float[3] { 0.0f, 0.0f, 0.0f };
    private int updateinterval = 1000;
    private float[] errorvalues = new float[MainForm.num_errorvalues];
    private IContainer components;
    private Button StartButton;
    private PictureBox pictureBox;
    private PictureBox resultBox;
    private ToolTip toolTip1;
    private GroupBox groupBox2;
    private Label label2;
    private NumericUpDown PixelSizeNumericUpDown;
    private Label DiameterLabel;
    private NumericUpDown DiameterNumericUpDown;
    private Label label1;
    private NumericUpDown FocalLengthNumericUpDown;
    private CheckBox SoundCheckBox;
    private Label FocusErrorLabel;
    private Label AverageFocusErrorLabel;
    private Label WithinCriticalFocusLabel;
    private Label AbsFocusErrorLabel;
    private Label MaskTypeLabel;
    private Label MaskAnglesLabel;
    private CheckBox BlueCheckBox;
    private CheckBox RedCheckBox;
    private CheckBox GreenCheckBox;
    private GroupBox RGBgroupBox;
    private CheckBox RotatingFocusserCheckBox;
    private bool logging_enabled = false;
    private bool text_on_bitmap = false;
    private Grabber bahtinov_grabber;
    private Bitmap buffered_picture;
    private float error;
    private bool centered;
    private Timer update_timer;
    private int errorcounter;

    public MainForm()
    {
      this.InitializeComponent();
      this.GetRegistryValues();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      this.StartButton = new Button();
      this.pictureBox = new PictureBox();
      this.resultBox = new PictureBox();
      this.toolTip1 = new ToolTip(this.components);
      this.groupBox2 = new GroupBox();
      this.label2 = new Label();
      this.PixelSizeNumericUpDown = new NumericUpDown();
      this.DiameterLabel = new Label();
      this.DiameterNumericUpDown = new NumericUpDown();
      this.label1 = new Label();
      this.FocalLengthNumericUpDown = new NumericUpDown();
      this.SoundCheckBox = new CheckBox();
      this.FocusErrorLabel = new Label();
      this.AverageFocusErrorLabel = new Label();
      this.WithinCriticalFocusLabel = new Label();
      this.AbsFocusErrorLabel = new Label();
      this.MaskTypeLabel = new Label();
      this.MaskAnglesLabel = new Label();
      this.BlueCheckBox = new CheckBox();
      this.RedCheckBox = new CheckBox();
      this.GreenCheckBox = new CheckBox();
      this.RGBgroupBox = new GroupBox();
      this.RotatingFocusserCheckBox = new CheckBox();
      ((ISupportInitialize) this.pictureBox).BeginInit();
      ((ISupportInitialize)this.resultBox).BeginInit();
      this.groupBox2.SuspendLayout();
      this.PixelSizeNumericUpDown.BeginInit();
      this.DiameterNumericUpDown.BeginInit();
      this.FocalLengthNumericUpDown.BeginInit();
      this.RGBgroupBox.SuspendLayout();
      this.SuspendLayout();
      this.StartButton.Location = new Point(11, 39);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new Size(56, 62);
      this.StartButton.TabIndex = 0;
      this.StartButton.Text = "Set capture area";
      this.toolTip1.SetToolTip((Control) this.StartButton, componentResourceManager.GetString("StartButton.ToolTip"));
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new EventHandler(this.StartButton_Click);
      this.pictureBox.Location = new Point(258, 175);
      this.pictureBox.Name = "pictureBox";
      this.pictureBox.Size = new Size(245, 230);
      this.pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox.TabIndex = 1;
      this.pictureBox.TabStop = false;

      this.resultBox.Location = new Point(258 + 245 + 25, 175);
      this.resultBox.Name = "pictureBox";
      this.resultBox.Size = new Size(245, 230);
      this.resultBox.SizeMode = PictureBoxSizeMode.AutoSize;
      this.resultBox.BorderStyle = BorderStyle.FixedSingle;
      this.toolTip1.AutoPopDelay = 5000;
      this.toolTip1.InitialDelay = 500;
      this.toolTip1.ReshowDelay = 100;
      this.toolTip1.Tag = (object) "";
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Controls.Add((Control) this.PixelSizeNumericUpDown);
      this.groupBox2.Controls.Add((Control) this.DiameterLabel);
      this.groupBox2.Controls.Add((Control) this.DiameterNumericUpDown);
      this.groupBox2.Controls.Add((Control) this.label1);
      this.groupBox2.Controls.Add((Control) this.FocalLengthNumericUpDown);
      this.groupBox2.Location = new Point(328, 4);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(175, (int) sbyte.MaxValue);
      this.groupBox2.TabIndex = 7;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Telescope";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 74);
      this.label2.Name = "label2";
      this.label2.Size = new Size(83, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "pixelsize(micron)";
      this.PixelSizeNumericUpDown.DecimalPlaces = 2;
      this.PixelSizeNumericUpDown.Increment = new Decimal(new int[4]
      {
        1,
        0,
        0,
        131072
      });
      this.PixelSizeNumericUpDown.Location = new Point(99, 72);
      this.PixelSizeNumericUpDown.Name = "PixelSizeNumericUpDown";
      this.PixelSizeNumericUpDown.Size = new Size(60, 20);
      this.PixelSizeNumericUpDown.TabIndex = 4;
      this.PixelSizeNumericUpDown.Value = new Decimal(new int[4]
      {
        56,
        0,
        0,
        65536
      });
      this.PixelSizeNumericUpDown.ValueChanged += new EventHandler(this.PixelSizeNumericUpDown_ValueChanged);
      this.DiameterLabel.AutoSize = true;
      this.DiameterLabel.Location = new Point(64, 48);
      this.DiameterLabel.Name = "DiameterLabel";
      this.DiameterLabel.Size = new Size(29, 13);
      this.DiameterLabel.TabIndex = 3;
      this.DiameterLabel.Text = "D(m)";
      this.DiameterNumericUpDown.DecimalPlaces = 3;
      this.DiameterNumericUpDown.Increment = new Decimal(new int[4]
      {
        1,
        0,
        0,
        196608
      });
      this.DiameterNumericUpDown.Location = new Point(99, 46);
      this.DiameterNumericUpDown.Name = "DiameterNumericUpDown";
      this.DiameterNumericUpDown.Size = new Size(60, 20);
      this.DiameterNumericUpDown.TabIndex = 2;
      this.DiameterNumericUpDown.Value = new Decimal(new int[4]
      {
        203,
        0,
        0,
        196608
      });
      this.DiameterNumericUpDown.ValueChanged += new EventHandler(this.DiameterNumericUpDown_ValueChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(64, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(27, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "f (m)";
      this.FocalLengthNumericUpDown.DecimalPlaces = 3;
      this.FocalLengthNumericUpDown.Increment = new Decimal(new int[4]
      {
        1,
        0,
        0,
        196608
      });
      this.FocalLengthNumericUpDown.Location = new Point(99, 20);
      this.FocalLengthNumericUpDown.Name = "FocalLengthNumericUpDown";
      this.FocalLengthNumericUpDown.Size = new Size(60, 20);
      this.FocalLengthNumericUpDown.TabIndex = 0;
      this.FocalLengthNumericUpDown.Value = new Decimal(new int[4]
      {
        2032,
        0,
        0,
        196608
      });
      this.FocalLengthNumericUpDown.ValueChanged += new EventHandler(this.FocalLengthNumericUpDown_ValueChanged);
      this.SoundCheckBox.AutoSize = true;
      this.SoundCheckBox.Location = new Point(11, 105);
      this.SoundCheckBox.Name = "SoundCheckBox";
      this.SoundCheckBox.Size = new Size(57, 17);
      this.SoundCheckBox.TabIndex = 8;
      this.SoundCheckBox.Text = "Sound";
      this.SoundCheckBox.UseVisualStyleBackColor = true;
      this.SoundCheckBox.CheckedChanged += new EventHandler(this.SoundCheckBox_CheckedChanged);
      this.FocusErrorLabel.AutoSize = true;
      this.FocusErrorLabel.Location = new Point(11, 150);
      this.FocusErrorLabel.Name = "FocusErrorLabel";
      this.FocusErrorLabel.Size = new Size(60, 13);
      this.FocusErrorLabel.TabIndex = 9;
      this.FocusErrorLabel.Text = "Focus error";
      this.AverageFocusErrorLabel.AutoSize = true;
      this.AverageFocusErrorLabel.Location = new Point(11, 175);
      this.AverageFocusErrorLabel.Name = "AverageFocusErrorLabel";
      this.AverageFocusErrorLabel.Size = new Size(100, 13);
      this.AverageFocusErrorLabel.TabIndex = 10;
      this.AverageFocusErrorLabel.Text = "Average focus error";
      this.WithinCriticalFocusLabel.AutoSize = true;
      this.WithinCriticalFocusLabel.Location = new Point(11, 229);
      this.WithinCriticalFocusLabel.Name = "WithinCriticalFocusLabel";
      this.WithinCriticalFocusLabel.Size = new Size(99, 13);
      this.WithinCriticalFocusLabel.TabIndex = 11;
      this.WithinCriticalFocusLabel.Text = "Within critical focus";
      this.AbsFocusErrorLabel.AutoSize = true;
      this.AbsFocusErrorLabel.Location = new Point(11, 202);
      this.AbsFocusErrorLabel.Name = "AbsFocusErrorLabel";
      this.AbsFocusErrorLabel.Size = new Size(101, 13);
      this.AbsFocusErrorLabel.TabIndex = 12;
      this.AbsFocusErrorLabel.Text = "Absolute focus error";
      this.MaskTypeLabel.AutoSize = true;
      this.MaskTypeLabel.Location = new Point(11, 257);
      this.MaskTypeLabel.Name = "MaskTypeLabel";
      this.MaskTypeLabel.Size = new Size(56, 13);
      this.MaskTypeLabel.TabIndex = 13;
      this.MaskTypeLabel.Text = "Mask type";
      this.MaskAnglesLabel.AutoSize = true;
      this.MaskAnglesLabel.Location = new Point(11, 282);
      this.MaskAnglesLabel.Name = "MaskAnglesLabel";
      this.MaskAnglesLabel.Size = new Size(67, 13);
      this.MaskAnglesLabel.TabIndex = 14;
      this.MaskAnglesLabel.Text = "Mask angles";
      this.BlueCheckBox.AutoSize = true;
      this.BlueCheckBox.Checked = true;
      this.BlueCheckBox.CheckState = CheckState.Checked;
      this.BlueCheckBox.Location = new Point(14, 65);
      this.BlueCheckBox.Name = "BlueCheckBox";
      this.BlueCheckBox.Size = new Size(47, 17);
      this.BlueCheckBox.TabIndex = 16;
      this.BlueCheckBox.Text = "Blue";
      this.BlueCheckBox.UseVisualStyleBackColor = true;
      this.RedCheckBox.AutoSize = true;
      this.RedCheckBox.Checked = true;
      this.RedCheckBox.CheckState = CheckState.Checked;
      this.RedCheckBox.Location = new Point(14, 19);
      this.RedCheckBox.Name = "RedCheckBox";
      this.RedCheckBox.Size = new Size(46, 17);
      this.RedCheckBox.TabIndex = 15;
      this.RedCheckBox.Text = "Red";
      this.RedCheckBox.UseVisualStyleBackColor = true;
      this.GreenCheckBox.AutoSize = true;
      this.GreenCheckBox.Checked = true;
      this.GreenCheckBox.CheckState = CheckState.Checked;
      this.GreenCheckBox.Location = new Point(14, 42);
      this.GreenCheckBox.Name = "GreenCheckBox";
      this.GreenCheckBox.Size = new Size(55, 17);
      this.GreenCheckBox.TabIndex = 17;
      this.GreenCheckBox.Text = "Green";
      this.GreenCheckBox.UseVisualStyleBackColor = true;
      this.RGBgroupBox.Controls.Add((Control) this.GreenCheckBox);
      this.RGBgroupBox.Controls.Add((Control) this.BlueCheckBox);
      this.RGBgroupBox.Controls.Add((Control) this.RedCheckBox);
      this.RGBgroupBox.Location = new Point(9, 307);
      this.RGBgroupBox.Name = "RGBgroupBox";
      this.RGBgroupBox.Size = new Size(134, 91);
      this.RGBgroupBox.TabIndex = 18;
      this.RGBgroupBox.TabStop = false;
      this.RGBgroupBox.Text = "RGB channels used";
      this.RotatingFocusserCheckBox.AutoSize = true;
      this.RotatingFocusserCheckBox.Location = new Point(204, 12);
      this.RotatingFocusserCheckBox.Name = "RotatingFocusserCheckBox";
      this.RotatingFocusserCheckBox.Size = new Size(107, 17);
      this.RotatingFocusserCheckBox.TabIndex = 7;
      this.RotatingFocusserCheckBox.Text = "Rotating Focuser";
      this.RotatingFocusserCheckBox.UseVisualStyleBackColor = true;
      this.RotatingFocusserCheckBox.CheckedChanged += new EventHandler(this.RotatingFocusserCheckBox_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.RGBgroupBox);
      this.Controls.Add((Control) this.RotatingFocusserCheckBox);
      this.Controls.Add((Control) this.MaskAnglesLabel);
      this.Controls.Add((Control) this.MaskTypeLabel);
      this.Controls.Add((Control) this.AbsFocusErrorLabel);
      this.Controls.Add((Control) this.WithinCriticalFocusLabel);
      this.Controls.Add((Control) this.AverageFocusErrorLabel);
      this.Controls.Add((Control) this.FocusErrorLabel);
      this.Controls.Add((Control) this.SoundCheckBox);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.pictureBox);
      this.Controls.Add((Control) this.resultBox);
      this.Controls.Add((Control) this.StartButton);
      this.MinimumSize = new Size(610, 470);
      this.Name = "MainForm";
      this.Text = "Bahtinov Grabber  V18.0 December 14 2019";
      this.Load += new EventHandler(this.MainForm_Load);
      ((ISupportInitialize) this.pictureBox).EndInit();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.PixelSizeNumericUpDown.EndInit();
      this.DiameterNumericUpDown.EndInit();
      this.FocalLengthNumericUpDown.EndInit();
      this.RGBgroupBox.ResumeLayout(false);
      this.RGBgroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void GetRegistryValues()
    {
      try
      {
        this.FocalLengthNumericUpDown.Value = Convert.ToDecimal(Application.UserAppDataRegistry.GetValue("FocalLength"));
        this.DiameterNumericUpDown.Value = Convert.ToDecimal(Application.UserAppDataRegistry.GetValue("Diameter"));
        this.PixelSizeNumericUpDown.Value = Convert.ToDecimal(Application.UserAppDataRegistry.GetValue("PixelSize"));
        this.RotatingFocusserCheckBox.Checked = Convert.ToBoolean(Application.UserAppDataRegistry.GetValue("RotatingFocusser"));
        this.SoundCheckBox.Checked = Convert.ToBoolean(Application.UserAppDataRegistry.GetValue("Sound"));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
        this.FocalLengthNumericUpDown.Value = new Decimal(2032, 0, 0, false, (byte) 3);
        this.DiameterNumericUpDown.Value = new Decimal(2032, 0, 0, false, (byte) 4);
        this.PixelSizeNumericUpDown.Value = new Decimal(56, 0, 0, false, (byte) 1);
        this.RotatingFocusserCheckBox.Checked = false;
        this.SoundCheckBox.Checked = false;
        Application.UserAppDataRegistry.SetValue("FocalLength", (object) this.FocalLengthNumericUpDown.Value);
        Application.UserAppDataRegistry.SetValue("Diameter", (object) this.DiameterNumericUpDown.Value);
        Application.UserAppDataRegistry.SetValue("PixelSize", (object) this.PixelSizeNumericUpDown.Value);
        Application.UserAppDataRegistry.SetValue("RotatingFocusser", (object) this.RotatingFocusserCheckBox.Checked);
        Application.UserAppDataRegistry.SetValue("Sound", (object) this.SoundCheckBox.Checked);
      }
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      this.centered = false;
      if (this.bahtinov_grabber == null)
      {
        this.bahtinov_grabber = new Grabber();
        this.bahtinov_grabber.picture = new Bitmap(128, 128);
        this.startTimer();
      }
      else
      {
        this.update_timer.Stop();
        this.bahtinov_angles = new float[3] { 0.0f, 0.0f, 0.0f };
        this.bahtinov_grabber.ReStart();
        this.update_timer.Start();
      }
      this.errorcounter = 0;
    }

    private void logMessage(string logtext)
    {
      try
      {
        if (!this.logging_enabled)
          return;
        File.AppendAllText("bahtinovgrabber.log", DateTime.Now.ToString() + " " + logtext + "\r\n");
      }
      catch
      {
      }
    }

    private void startTimer()
    {
      this.update_timer = new Timer();
      this.update_timer.Interval = this.updateinterval;
      this.update_timer.Start();
      this.update_timer.Tick += new EventHandler(this.Timer_Tick);
    }

    private bool equal(Bitmap bmp1, Bitmap bmp2)
    {
      bool flag;
      if (bmp1.Size != bmp2.Size)
      {
        flag = true;
      }
      else
      {
        flag = false;
        for (int x = 0; x < bmp1.Width & !flag; ++x)
        {
          for (int y = 0; y < bmp1.Height & !flag; ++y)
          {
            if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
              flag = true;
          }
        }
      }
      return !flag;
    }

    private void Timer_Tick(object sender, EventArgs eArgs)
    {
      if (!(this.bahtinov_grabber.activated & this.bahtinov_grabber.picture != null))
        return;
      if (this.buffered_picture == null)
        this.buffered_picture = new Bitmap(this.bahtinov_grabber.picture.Width, this.bahtinov_grabber.picture.Height);
      if (this.equal(this.bahtinov_grabber.picture, this.buffered_picture))
        return;
      this.pictureBox.Image = (Image) this.bahtinov_grabber.picture;
      this.buffered_picture = new Bitmap((Image) this.bahtinov_grabber.picture);
      this.ShowLines(ref this.bahtinov_grabber.picture, ref this.bahtinov_angles);
      this.resultBox.Location = new Point(this.pictureBox.Location.X + this.pictureBox.Size.Width + 25, this.resultBox.Location.Y);
      this.Size = new Size(Math.Max(610, this.resultBox.Location.X + this.resultBox.Size.Width + 50), Math.Max(470, this.pictureBox.Location.Y + this.pictureBox.Size.Height + 50));
    }

    private Bitmap createBitmapFromArray(float[,] imgData, int width, int height)
    {
      byte[] bytes = new byte[width * height * 3];
      int idx = 0;
      for (int indexY = 0; indexY < height; ++indexY)
      {
        for (int indexX = 0; indexX < width; ++indexX)
        {
          byte byteValue = (byte)Math.Floor(imgData[indexX, indexY] * 255);
          bytes[idx++] = byteValue; // R
          bytes[idx++] = byteValue; // G
          bytes[idx++] = byteValue; // B
        }
      }

      var columns = width;
      var rows = height;
      var stride = columns * 4;
      var newbytes = PadLines(bytes, rows, columns);
      var im = new Bitmap(columns, rows, stride,
                          PixelFormat.Format24bppRgb,
                          Marshal.UnsafeAddrOfPinnedArrayElement(newbytes, 0));
      return im;
    }

    static byte[] PadLines(byte[] bytes, int rows, int columns)
    {
      //The old and new offsets could be passed through parameters,
      //but I hardcoded them here as a sample.
      var currentStride = columns * 3;
      var newStride = columns * 4;
      var newBytes = new byte[newStride * rows];
      for (var i = 0; i < rows; i++)
        Buffer.BlockCopy(bytes, currentStride * i, newBytes, newStride * i, currentStride);
      return newBytes;
    }

    private void ShowLines(ref Bitmap bmp, ref float[] bahtinov_angles)
    {
      // *** COPY DATA ***
      Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
      BitmapData bitmapdata = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
      IntPtr scan0 = bitmapdata.Scan0;
      int dataSize = bitmapdata.Stride * bmp.Height;
      byte[] rawByteStream = new byte[dataSize];
      Marshal.Copy(scan0, rawByteStream, 0, dataSize);
      Marshal.Copy(rawByteStream, 0, scan0, dataSize);
      bmp.UnlockBits(bitmapdata);

      int width = bmp.Width;
      int height = bmp.Height;
      float outerCircleRadius = (width < height ? 0.5f * (float) Math.Sqrt(2.0) * (float) width : 0.5f * (float) Math.Sqrt(2.0) * (float) height) - 8f;
      int leftEdge = (int) (0.5 * ((double) width - (double) outerCircleRadius));
      int rightEdge = (int) (0.5 * ((double) width + (double) outerCircleRadius));
      int topEdge = (int) (0.5 * ((double) height - (double) outerCircleRadius));
      int bottomEdge = (int) (0.5 * ((double) height + (double) outerCircleRadius));
      int incX = 1;
      int incY = 1;

      // *** GET DATA PART OF SQUARE THAT MATCHES INNER CIRCLE IN IMAGE AND STORE GRAYSCALE VALUE ***
      // NOTE: Make sure at least 1 channel is switched on
      float useRedChannel = this.RedCheckBox.Checked ? 1f : 0.0f;
      float useGreenChannel = this.GreenCheckBox.Checked ? 1f : 0.0f;
      float useBlueChannel = this.BlueCheckBox.Checked ? 1f : 0.0f;
      float divideFactor = Math.Max(1.0f, useRedChannel + useGreenChannel + useBlueChannel);
      float[,] normalizedImageData = new float[width, height];
      //float num11 = 0.0f;
      //float num12 = 0.0f;
      for (int indexX = leftEdge; indexX < rightEdge; ++indexX)
      {
        for (int indexY = topEdge; indexY < bottomEdge; ++indexY)
        {
          int index3 = (indexX + indexY * width) * bitmapdata.Stride / width;
          normalizedImageData[indexX, indexY] = (float) ((double) useRedChannel * (double) rawByteStream[index3] + (double) useGreenChannel * (double) rawByteStream[index3 + 1] + (double) useBlueChannel * (double) rawByteStream[index3 + 2]);
          normalizedImageData[indexX, indexY] /= divideFactor; // 3f; // NOTE: Is incorrect, if some channels are switched off, then those should not be encounted for, divide by less then 3
          normalizedImageData[indexX, indexY] /= (float) byte.MaxValue;
          normalizedImageData[indexX, indexY] = (float) Math.Sqrt((double) normalizedImageData[indexX, indexY]);
          //num11 += numArray2[index1, index2];
          //++num12;
        }
      }
      //try
      //{
      //  float num13 = num11 / num12;
      //}
      //catch
      //{
      //}

      float halfWidth = (float) (((double) width + 1.0) / 2.0);
      float halfHeight = (float) (((double) height + 1.0) / 2.0);
      float[] numArray3 = new float[3]; // Only used for non rotating focus (RotatingFocusserCheckBox not checked)
      float[] numArray4 = new float[3]; // Only used for non rotating focus (RotatingFocusserCheckBox not checked)
      bool bahtinov_angles_empty = (float.Equals(bahtinov_angles[0], 0.0f) & float.Equals(bahtinov_angles[1], 0.0f) & float.Equals(bahtinov_angles[2], 0.0f));
      if (bahtinov_angles_empty) // OR this.RotatingFocusserCheckBox.Checked
      {
        // *** ROTATE DATA PART 180 DEGREES AND CHECK FOR EACH ANGLE FOR A LINE ***
        int steps = 180;
        float radPerStep = (float) Math.PI / (float) steps;
        float[] averageRowIndexPerAngle = new float[steps];
        float[] highestRowAveragePerAngle = new float[steps];
        float[,] numArray7 = new float[width, height];
        for (int index1 = 0; index1 < steps; ++index1)
        {
          // *** ROTATE DATA PART 1 STEP ***
          float num18 = radPerStep * (float) index1;
          float num19 = (float) Math.Sin((double) num18);
          float num20 = (float) Math.Cos((double) num18);
          int index2 = leftEdge;
          while (index2 < rightEdge)
          {
            int index3 = topEdge;
            while (index3 < bottomEdge)
            {
              float num21 = (float) index2 - halfWidth;
              float num22 = (float) index3 - halfHeight;
              float num23 = (float) ((double) halfWidth + (double) num21 * (double) num20 + (double) num22 * (double) num19);
              float num24 = (float) ((double) halfHeight - (double) num21 * (double) num19 + (double) num22 * (double) num20);
              int index4 = (int) Math.Floor((double) num23);
              int index5 = (int) Math.Ceiling((double) num23);
              int index6 = (int) Math.Floor((double) num24);
              int index7 = (int) Math.Ceiling((double) num24);
              float num25 = num23 - (float) index4;
              float num26 = num24 - (float) index6;
              numArray7[index2, index3] = (float) ((double) normalizedImageData[index4, index6] * (1.0 - (double) num25) * (1.0 - (double) num26) + (double) normalizedImageData[index5, index6] * (double) num25 * (1.0 - (double) num26) + (double) normalizedImageData[index5, index7] * (double) num25 * (double) num26 + (double) normalizedImageData[index4, index7] * (1.0 - (double) num25) * (double) num26);
              index3 += incY;
            }
            index2 += incX;
          }

          //Bitmap intermediate = createBitmapFromArray(numArray7, width, height);
          //this.resultBox.Image = (Image)intermediate;
          //this.resultBox.Size = new Size(intermediate.Width, intermediate.Height);

          // *** CALCULATE AVERAGE PIXEL VALUE OF EACH ROW AND STORE HIGHEST VALUE PER ANGLE/STEP ***
          float[] rowAverage = new float[height];
          // Clear numArray8
          for (int index3 = 0; index3 < height; ++index3)
          {
            rowAverage[index3] = 0.0f;
          }
          // Loop over each row
          int index8 = topEdge;
          while (index8 < bottomEdge)
          {
            int numEdges = 0;
            // Loop over each column
            int index3 = leftEdge;
            while (index3 < rightEdge)
            {
              rowAverage[index8] += numArray7[index3, index8];
              ++numEdges;
              index3 += incX;
            }
            rowAverage[index8] /= (float)numEdges;
            index8 += incY;
          }

          // Calculate row average over multiple rows (if num27 > 1)
          int num27 = 1; // If values is greater then 1, then below code has effect
          if (num27 > 1)
          {
            // Copy numArray8 into numArray9
            float[] numArray9 = new float[height];
            for (int index3 = 0; index3 < height; ++index3)
            {
              numArray9[index3] = rowAverage[index3];
            }

            // Determine new average over multiple rows
            for (int index3 = topEdge; index3 < bottomEdge; ++index3)
            {
              numArray9[index3] = 0.0f;
              for (int index4 = -(num27 - 1) / 2; index4 <= (num27 - 1) / 2; ++index4)
              {
                numArray9[index3] += rowAverage[index3 + index4] / (float)num27;
              }
            }

            // Copy new average back to numArray8
            for (int index3 = 0; index3 < height; ++index3)
            {
              rowAverage[index3] = numArray9[index3];
            }
          }

          // Store higest row average and corresponding row index per anle/step
          float highestRowAverage = -1f;
          float rowAverageIndex = -1f;
          for (int index3 = topEdge; index3 < bottomEdge; ++index3)
          {
            if ((double) rowAverage[index3] > (double) highestRowAverage)
            {
              rowAverageIndex = (float) index3;
              highestRowAverage = rowAverage[index3];
            }
          }
          try
          {
            averageRowIndexPerAngle[index1] = rowAverageIndex;
            highestRowAveragePerAngle[index1] = highestRowAverage;
          }
          catch
          {
            MessageBox.Show("Foutje bedankt!");
          }
        }

        // *** CALCULATE BAHTINOV ANGLES ***
        int num30 = 0;
        int num50 = (int)(0.0872664600610733 / (double)radPerStep); // 0.0872664600610733 = 5 degrees in rad
        for (int index1 = 0; index1 < 3; ++index1)
        {
          float num18 = -1f;
          float num19 = -1f;
          float num20 = -1f;
          for (int index2 = 0; index2 < steps; ++index2)
          {
            if ((double) highestRowAveragePerAngle[index2] > (double) num18)
            {
              num18 = highestRowAveragePerAngle[index2];
              num20 = averageRowIndexPerAngle[index2];
              num19 = (float) index2 * radPerStep;
              num30 = index2;
            }
          }
          numArray4[index1] = num20; // Only used for non rotating focus
          numArray3[index1] = num19; // Only used for non rotating focus
          bahtinov_angles[index1] = num19;
          for (int index2 = num30 - num50; index2 < num30 + num50; ++index2)
          {
            int index3 = (index2 + steps) % steps;
            try
            {
              highestRowAveragePerAngle[index3] = 0.0f;
            }
            catch
            {
              MessageBox.Show("Kwalitatief uitermate teleurstellend");
            }
          }
        }

        if (this.RotatingFocusserCheckBox.Checked)
        {
          bahtinov_angles = new float[3] { 0.0f, 0.0f, 0.0f };
        }
      }
      else
      {
        int length2 = 3;
        float[] numArray5 = new float[length2];
        float[] numArray6 = new float[length2];
        float[,] numArray7 = new float[width, height];
        for (int index1 = 0; index1 < length2; ++index1)
        {
          float num13 = bahtinov_angles[index1];
          float num18 = (float) Math.Sin((double) num13);
          float num19 = (float) Math.Cos((double) num13);
          int index2 = leftEdge;
          while (index2 < rightEdge)
          {
            int index3 = topEdge;
            while (index3 < bottomEdge)
            {
              float num20 = (float) index2 - halfWidth;
              float num21 = (float) index3 - halfHeight;
              float num22 = (float) ((double) halfWidth + (double) num20 * (double) num19 + (double) num21 * (double) num18);
              float num23 = (float) ((double) halfHeight - (double) num20 * (double) num18 + (double) num21 * (double) num19);
              int index4 = (int) Math.Floor((double) num22);
              int index5 = (int) Math.Ceiling((double) num22);
              int index6 = (int) Math.Floor((double) num23);
              int index7 = (int) Math.Ceiling((double) num23);
              float num24 = num22 - (float) index4;
              float num25 = num23 - (float) index6;
              numArray7[index2, index3] = (float) ((double) normalizedImageData[index4, index6] * (1.0 - (double) num24) * (1.0 - (double) num25) + (double) normalizedImageData[index5, index6] * (double) num24 * (1.0 - (double) num25) + (double) normalizedImageData[index5, index7] * (double) num24 * (double) num25 + (double) normalizedImageData[index4, index7] * (1.0 - (double) num24) * (double) num25);
              index3 += incY;
            }
            index2 += incX;
          }

          float[] yvals = new float[height];
          for (int index3 = 0; index3 < height; ++index3)
            yvals[index3] = 0.0f;
          int index8 = topEdge;
          while (index8 < bottomEdge)
          {
            int num20 = 0;
            int index3 = leftEdge;
            while (index3 < rightEdge)
            {
              yvals[index8] += numArray7[index3, index8];
              ++num20;
              index3 += incX;
            }
            yvals[index8] /= (float) num20;
            index8 += incY;
          }
          float num26 = -1f;
          float num27 = -1f;
          for (int index3 = topEdge; index3 < bottomEdge; ++index3)
          {
            if ((double) yvals[index3] > (double) num26)
            {
              num27 = (float) index3;
              num26 = yvals[index3];
            }
          }
          float num28 = new LSQcalculator().peakposition(yvals, (int) num27, 2);
          try
          {
            numArray5[index1] = num28;
            numArray6[index1] = num26;
          }
          catch
          {
            int num20 = (int) MessageBox.Show("foutje");
          }
        }
        for (int index = 0; index < 3; ++index)
        {
          numArray4[index] = numArray5[index];
          numArray3[index] = bahtinov_angles[index];
        }
      }
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = index1; index2 < 3; ++index2)
        {
          if ((double) numArray3[index2] < (double) numArray3[index1])
          {
            float num13 = numArray3[index1];
            numArray3[index1] = numArray3[index2];
            numArray3[index2] = num13;
            float num18 = numArray4[index1];
            numArray4[index1] = numArray4[index2];
            numArray4[index2] = num18;
          }
        }
      }
      if ((double) numArray3[1] - (double) numArray3[0] > Math.PI / 2.0)
      {
        numArray3[1] -= 3.141593f;
        numArray3[2] -= 3.141593f;
        numArray4[1] = (float) height - numArray4[1];
        numArray4[2] = (float) height - numArray4[2];
      }
      if ((double) numArray3[2] - (double) numArray3[1] > 1.57079637050629)
      {
        numArray3[2] -= 3.141593f;
        numArray4[2] = (float) height - numArray4[2];
      }
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = index1; index2 < 3; ++index2)
        {
          if ((double) numArray3[index2] < (double) numArray3[index1])
          {
            float num13 = numArray3[index1];
            numArray3[index1] = numArray3[index2];
            numArray3[index2] = num13;
            float num18 = numArray4[index1];
            numArray4[index1] = numArray4[index2];
            numArray4[index2] = num18;
          }
        }
      }

      DrawLines(halfWidth, halfHeight, numArray3, numArray4);

      this.centered = false;
      if (!this.RotatingFocusserCheckBox.Checked)
        return;
      bahtinov_angles = new float[3];
    }

    private void DrawLines(float val1, float val2, float[] bahtinovAngles, float[] numArray4)
    {
      // Take bahtinov angles and draw the 3 angle lines
      int yzero = 1;
      int penWidth = 1;
      Pen pen = new Pen(Color.Yellow, (float)penWidth);
      pen.DashStyle = DashStyle.Dash;
      Graphics graphics = Graphics.FromImage(this.pictureBox.Image);
      int imageHeight = this.pictureBox.Height;
      float num14 = 0.0f;
      float num15 = 0.0f;
      float num16 = 0.0f;
      float num17 = 0.0f;
      float num32 = 0.0f;
      float num33 = 0.0f;
      float num34 = 0.0f;
      float num35 = 0.0f;
      for (int index = 0; index < 3; ++index)
      {
        float num13 = Math.Min(val1, val2);
        float x1 = val1 + -num13 * (float)Math.Cos((double)bahtinovAngles[index]) + (numArray4[index] - val2) * (float)Math.Sin((double)bahtinovAngles[index]);
        float x2 = val1 + num13 * (float)Math.Cos((double)bahtinovAngles[index]) + (numArray4[index] - val2) * (float)Math.Sin((double)bahtinovAngles[index]);
        float num18 = val2 + -num13 * (float)Math.Sin((double)bahtinovAngles[index]) + (float)-((double)numArray4[index] - (double)val2) * (float)Math.Cos((double)bahtinovAngles[index]);
        float num19 = val2 + num13 * (float)Math.Sin((double)bahtinovAngles[index]) + (float)-((double)numArray4[index] - (double)val2) * (float)Math.Cos((double)bahtinovAngles[index]);
        if (index == 0)
        {
          float num20 = x1;
          float num21 = x2;
          float num22 = num18;
          float num23 = num19;
          num14 = (float)(((double)num23 - (double)num22) / ((double)num21 - (double)num20));
          num16 = (float)(-(double)num20 * (((double)num23 - (double)num22) / ((double)num21 - (double)num20))) + num22;
        }
        else if (index == 1)
        {
          num32 = x1;
          num34 = x2;
          num33 = num18;
          num35 = num19;
          double num20 = ((double)num35 - (double)num33) / ((double)num34 - (double)num32);
          double num21 = ((double)num35 - (double)num33) / ((double)num34 - (double)num32);
        }
        else if (index == 2)
        {
          float num20 = x1;
          float num21 = x2;
          float num22 = num18;
          float num23 = num19;
          num15 = (float)(((double)num23 - (double)num22) / ((double)num21 - (double)num20));
          num17 = (float)(-(double)num20 * (((double)num23 - (double)num22) / ((double)num21 - (double)num20))) + num22;
        }
        switch (index)
        {
          case 0:
            pen.Color = Color.Red;
            break;
          case 1:
            pen.Color = Color.Yellow;
            break;
          default:
            pen.Color = Color.Green;
            break;
        }
        graphics.DrawLine(pen, x1, (float)imageHeight - num18 + (float)yzero, x2, (float)imageHeight - num19 + (float)yzero);
      }


      float x3 = (float)(-((double)num16 - (double)num17) / ((double)num14 - (double)num15));
      float num36 = num14 * x3 + num16;
      pen.Color = Color.Blue;
      int num37 = penWidth * 4;
      graphics.DrawEllipse(pen, x3 - (float)num37, (float)imageHeight - num36 - (float)num37 + (float)yzero, (float)(num37 * 2), (float)(num37 * 2));
      float num38 = (float)(((double)x3 - (double)num32) * ((double)num34 - (double)num32) + ((double)num36 - (double)num33) * ((double)num35 - (double)num33)) / (float)(((double)num34 - (double)num32) * ((double)num34 - (double)num32) + ((double)num35 - (double)num33) * ((double)num35 - (double)num33));
      float num39 = num32 + num38 * (num34 - num32);
      float num40 = num33 + num38 * (num35 - num33);
      float num41 = (float)Math.Sqrt(((double)x3 - (double)num39) * ((double)x3 - (double)num39) + ((double)num36 - (double)num40) * ((double)num36 - (double)num40));
      float num42 = 0.0f;
      float num43 = x3 - num39;
      float num44 = num36 - num40;
      float num45 = num34 - num32;
      float num46 = num35 - num33;
      try
      {
        num42 = (float)-Math.Sign((float)((double)num43 * (double)num46 - (double)num44 * (double)num45));
      }
      catch
      {
        MessageBox.Show("Oops!");
      }
      this.error = num42 * num41;
      this.errorvalues[this.errorcounter % MainForm.num_errorvalues] = this.error;
      ++this.errorcounter;
      float num47 = 0.0f;
      int num48 = 0;
      if (this.errorcounter >= MainForm.num_errorvalues)
      {
        for (int index = 0; index < MainForm.num_errorvalues; ++index)
        {
          num47 += this.errorvalues[index];
          ++num48;
        }
      }
      else
      {
        for (int index = 0; index < this.errorcounter; ++index)
        {
          num47 += this.errorvalues[index];
          ++num48;
        }
      }
      double num49 = (double)num47;
      int num50 = num48;
      int num51 = 1;
      int num52 = num50 + num51;
      double num53 = (double)num50;
      float num54 = (float)(num49 / num53);
      float x4 = x3 + (float)(((double)num39 - (double)x3) * 20.0);
      float num55 = num36 + (float)(((double)num40 - (double)num36) * 20.0);
      pen.Color = Color.Red;
      pen.Width = (float)penWidth;
      int num56 = penWidth * 4;
      graphics.DrawEllipse(pen, x4 - (float)num56, (float)imageHeight - num55 - (float)num56 + (float)yzero, (float)(num56 * 2), (float)(num56 * 2));
      pen.Width = (float)penWidth;
      graphics.DrawLine(pen, new PointF(x4, (float)imageHeight - num55 + (float)yzero), new PointF(x3, (float)imageHeight - num36 + (float)yzero));
      Font font = new Font("Arial", 8f);
      SolidBrush solidBrush = new SolidBrush(Color.White);
      string str1 = "focus error: " + (num42 * num41).ToString("F2") + " pixels";
      this.logMessage(str1);
      if (this.text_on_bitmap)
        graphics.DrawString(str1, font, (Brush)solidBrush, (PointF)new Point(10, 10));
      this.FocusErrorLabel.Text = str1;
      string str2 = (MainForm.num_errorvalues / (1000 / this.updateinterval)).ToString() + "s average: " + num54.ToString("F2") + " pixels";
      this.logMessage(str2);
      if (this.text_on_bitmap)
        graphics.DrawString(str2, font, (Brush)solidBrush, (PointF)new Point(10, 20));
      this.AverageFocusErrorLabel.Text = str2;
      float num57 = 57.29578f; // 57.29578 = 180 / PI = 1 Rad in degrees
      float num58 = (float)Math.PI / 180f;
      float num59 = Math.Abs((float)(((double)bahtinovAngles[2] - (double)bahtinovAngles[0]) / 2.0));
      string str3 = (num59 * num57).ToString("F0") + " degree Bahtinov";
      float num60 = (float)(9.0 / 32.0 * ((double)(float)this.DiameterNumericUpDown.Value / ((double)(float)this.FocalLengthNumericUpDown.Value *
        (double)(float)this.PixelSizeNumericUpDown.Value)) * (1.0 + Math.Cos(45.0 * (double)num58) * (1.0 + Math.Tan((double)num59))));
      this.MaskTypeLabel.Text = str3;
      this.MaskAnglesLabel.Text = "angles: " + (num57 * bahtinovAngles[0]).ToString("F1") + " " + (num57 * bahtinovAngles[1]).ToString("F1") + " " +
        (num57 * bahtinovAngles[2]).ToString("F1");
      float num61 = num42 * num41 / num60;
      string str4 = "calculated absolute focus error: " + num61.ToString("F2") + " microns";
      this.logMessage(str4);
      if (this.text_on_bitmap)
        graphics.DrawString(str4, font, (Brush)solidBrush, (PointF)new Point(10, 30));
      this.AbsFocusErrorLabel.Text = str4;
      float num62 = (float)(8.99999974990351E-07 * ((double)(float)this.FocalLengthNumericUpDown.Value / (double)(float)this.DiameterNumericUpDown.Value) * ((double)(float)this.FocalLengthNumericUpDown.Value / (double)(float)this.DiameterNumericUpDown.Value));
      bool flag = Math.Abs((double)num61 * 1E-06) < (double)Math.Abs(num62);
      string str5 = "within critical focus: " + (flag ? "YES" : "NO");
      this.logMessage(str5);
      if (this.text_on_bitmap)
        graphics.DrawString(str5, font, (Brush)solidBrush, (PointF)new Point(10, 40));
      this.WithinCriticalFocusLabel.Text = str5;
      if (flag)
      {
        pen.Color = Color.Yellow;
        pen.Width = (float)penWidth;
        int num13 = 32;
        while (num13 < 128)
        {
          int num18 = num13;
          graphics.DrawEllipse(pen, x3 - (float)num18, (float)imageHeight - num36 - (float)num18 + (float)yzero, (float)(num18 * 2), (float)(num18 * 2));
          num13 += 32;
        }
        if (this.SoundCheckBox.Checked)
          SystemSounds.Exclamation.Play();
      }
      if (!this.centered)
      {
        this.bahtinov_grabber.areaForm.CurrentTopLeft.X -= (int)((double)(this.pictureBox.Width / 2) - ((double)x3 + (double)num39) / 2.0);
        this.bahtinov_grabber.areaForm.CurrentTopLeft.Y += (int)((double)(this.pictureBox.Height / 2) - ((double)num36 + (double)num40) / 2.0);
        this.bahtinov_grabber.areaForm.CurrentBottomRight.X -= (int)((double)(this.pictureBox.Width / 2) - ((double)x3 + (double)num39) / 2.0);
        this.bahtinov_grabber.areaForm.CurrentBottomRight.Y += (int)((double)(this.pictureBox.Height / 2) - ((double)num36 + (double)num40) / 2.0);
        this.centered = true;
      }
    }

    private void center_button_Click(object sender, EventArgs e)
    {
      this.centered = false;
    }

    private void D_trackBar_Scroll(object sender, EventArgs e)
    {
    }

    private void FocalLengthNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      Application.UserAppDataRegistry.SetValue("FocalLength", (object) this.FocalLengthNumericUpDown.Value);
    }

    private void DiameterNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      Application.UserAppDataRegistry.SetValue("Diameter", (object) this.DiameterNumericUpDown.Value);
    }

    private void PixelSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      Application.UserAppDataRegistry.SetValue("PixelSize", (object) this.PixelSizeNumericUpDown.Value);
    }

    private void RotatingFocusserCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      Application.UserAppDataRegistry.SetValue("RotatingFocusser", (object) this.RotatingFocusserCheckBox.Checked);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }

    private void donate_button_Click(object sender, EventArgs e)
    {
      string str1 = "";
      string str2 = "njnoordhoek@hotmail.com";
      string str3 = "Donation%20in%20appreciation%20of%20Bahtinov%20Grabber.";
      string str4 = "US";
      string str5 = "EUR";
      Process.Start(str1 + "https://www.paypal.com/cgi-bin/webscr?cmd=\"_donations\"&business=\"" + str2 + "\"&lc=\"" + str4 + "\"&item_name=\"" + str3 + "\"&currency_code=\"" + str5 + "\"&bn=\"PP%2dDonationsBF\"");
    }

    private void SoundCheckBox_CheckedChanged(object sender, EventArgs e)
    {
    }
  }
}
