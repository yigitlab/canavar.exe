using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ProductWizardApp.Data;
using ProductWizardApp.Models;

namespace ProductWizardApp.UI;

public class ProductDetailForm : Form
{
    private readonly Product _product;

    private PictureBox _picImage;

    private Label _lblTitle;

    private Label _lblPrice;

    private FlowLayoutPanel _pnlSpecs;

    public ProductDetailForm(Product product)
    {
        _product = product;
        InitializeUI();
        LoadProductData();
    }

    private void InitializeUI()
    {
        AppTheme current = ThemeManager.Current;
        Text = _product.Brand + " " + _product.Name + " - Detaylar";
        base.Size = new Size(600, 700);
        base.StartPosition = FormStartPosition.CenterParent;
        BackColor = current.Light;
        Font = new Font("Segoe UI", 10f);
        base.FormBorderStyle = FormBorderStyle.FixedDialog;
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        Panel panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 250,
            Padding = new Padding(20)
        };
        _picImage = new PictureBox
        {
            Dock = DockStyle.Left,
            Width = 220,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.White
        };
        Panel panel2 = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20, 10, 0, 10)
        };
        _lblTitle = new Label
        {
            Dock = DockStyle.Top,
            Height = 80,
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            ForeColor = current.Dark,
            Text = _product.Brand + " " + _product.Name
        };
        _lblPrice = new Label
        {
            Dock = DockStyle.Top,
            Height = 40,
            Font = new Font("Segoe UI", 20f, FontStyle.Bold),
            ForeColor = current.Primary,
            Text = $"{_product.Price:N0} TL"
        };
        panel2.Controls.Add(_lblPrice);
        panel2.Controls.Add(_lblTitle);
        panel.Controls.Add(panel2);
        panel.Controls.Add(_picImage);
        Label value = new Label
        {
            Text = "Teknik Özellikler",
            Dock = DockStyle.Top,
            Height = 40,
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            ForeColor = current.Dark,
            TextAlign = ContentAlignment.BottomLeft,
            Padding = new Padding(20, 0, 0, 5)
        };
        _pnlSpecs = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(20, 10, 20, 20),
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };
        Button button = new Button
        {
            Text = "Kapat",
            Dock = DockStyle.Bottom,
            Height = 50,
            FlatStyle = FlatStyle.Flat,
            BackColor = current.Primary,
            ForeColor = current.TextOnPrimary,
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        button.Click += delegate
        {
            Close();
        };
        base.Controls.Add(_pnlSpecs);
        base.Controls.Add(value);
        base.Controls.Add(panel);
        base.Controls.Add(button);
    }

    private void LoadProductData()
    {
        DataService.LoadProductImage(_picImage, _product);
        PropertyInfo[] properties = _product.GetType().GetProperties();
        AppTheme current = ThemeManager.Current;
        PropertyInfo[] array = properties;
        foreach (PropertyInfo propertyInfo in array)
        {
            if (propertyInfo.Name == "Id" || propertyInfo.Name == "Image" || propertyInfo.Name == "Name" || propertyInfo.Name == "Brand" || propertyInfo.Name == "Price")
            {
                continue;
            }
            object value = propertyInfo.GetValue(_product);
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                string text = propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? propertyInfo.Name;
                string text2 = ((!(value is bool)) ? value.ToString() : (((bool)value) ? "Evet" : "Hayır"));
                Panel panel = new Panel
                {
                    Width = 540,
                    Height = 35,
                    Margin = new Padding(0, 0, 0, 2)
                };
                Label value2 = new Label
                {
                    Text = text,
                    Width = 200,
                    Dock = DockStyle.Left,
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                Label value3 = new Label
                {
                    Text = text2,
                    Dock = DockStyle.Fill,
                    ForeColor = current.Dark,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                panel.Controls.Add(value3);
                panel.Controls.Add(value2);
                if (_pnlSpecs.Controls.Count % 2 == 0)
                {
                    panel.BackColor = Color.FromArgb(245, 245, 245);
                }
                _pnlSpecs.Controls.Add(panel);
            }
        }
    }
}
