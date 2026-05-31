using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ProductWizardApp.Data;
using ProductWizardApp.Models;

namespace ProductWizardApp.UI;

public class CompareWizardControl : UserControl
{
    private Panel topBar = null;

    private ComboBox cbCategory = null;

    private Panel cardArea = null;

    private FlowLayoutPanel cardFlow = null;

    private Panel selectionStrip = null;

    private Label lblSel1 = null;

    private Label lblVs = null;

    private Label lblSel2 = null;

    private Button btnCompare = null;
    private Button btnClearSelection = null;

    private Panel compareArea = null;

    private Button btnBack = null;

    private DataGridView gridCompare = null;

    private Panel pnlCompareResultHeader = null;

    private readonly DataService _dataService;

    private readonly List<Product> _selectedProducts = new List<Product>();

    private readonly Dictionary<Product, Panel> _cardPanels = new Dictionary<Product, Panel>();

    private static readonly Dictionary<string, (string prop, string icon)[]> CategorySpecs = new Dictionary<string, (string, string)[]>
    {
        ["CPU"] = new (string, string)[4]
        {
            ("Cores", "⚙\ufe0f"),
            ("BaseClockGHz", "\ud83d\udcf6"),
            ("Socket", "\ud83d\udd0c"),
            ("TuketimDegeri", "\ud83c\udf21\ufe0f")
        },
        ["GPU"] = new (string, string)[4]
        {
            ("VRAM_GB", "\ud83d\udda5\ufe0f"),
            ("CoreClockMHz", "⚡"),
            ("FanAdedi", "\ud83d\udca8"),
            ("GPUBellekArayuzu", "\ud83d\udd17")
        },
        ["RAM"] = new (string, string)[4]
        {
            ("KapasiteGB", "\ud83d\udcbe"),
            ("HizMHz", "\ud83d\udcf6"),
            ("RamTipi", "\ud83d\udd22"),
            ("Voltaj", "⚡")
        },
        ["Anakart"] = new (string, string)[4]
        {
            ("Soket", "\ud83d\udd0c"),
            ("RamTipi", "\ud83d\udcbe"),
            ("M2Yuvasi", "\ud83d\uddc4\ufe0f"),
            ("KasaTipi", "\ud83d\udcd0")
        },
        ["Telefon"] = new (string, string)[4]
        {
            ("ScreenResolution", "\ud83d\udcfa"),
            ("RAM_GB", "\ud83d\udcbe"),
            ("Camera_MP", "\ud83d\udcf7"),
            ("Battery_mAh", "\ud83d\udd0b")
        },
        ["Laptop"] = new (string, string)[4]
        {
            ("ScreenSize", "\ud83d\udcd0"),
            ("RAM_GB", "\ud83d\udcbe"),
            ("Storage_GB", "\ud83d\uddc4\ufe0f"),
            ("Processor", "⚙\ufe0f")
        },
        ["Tablet"] = new (string, string)[4]
        {
            ("ScreenSize", "\ud83d\udcd0"),
            ("Storage_GB", "\ud83d\uddc4\ufe0f"),
            ("Battery_mAh", "\ud83d\udd0b"),
            ("HasStylus", "✏\ufe0f")
        },
        ["SSD"] = new (string, string)[4]
        {
            ("Capacity_GB", "\ud83d\uddc4\ufe0f"),
            ("Interface", "\ud83d\udcf6"),
            ("", ""),
            ("", "")
        },
        ["Monitör"] = new (string, string)[4]
        {
            ("ScreenSize", "\ud83d\udcd0"),
            ("Resolution", "\ud83d\udcfa"),
            ("RefreshRate", "\ud83d\udd04"),
            ("PanelType", "\ud83d\udda5\ufe0f")
        },
        ["Televizyon"] = new (string, string)[4]
        {
            ("ScreenSize", "\ud83d\udcd0"),
            ("Resolution", "\ud83d\udcfa"),
            ("RefreshRate_Hz", "\ud83d\udd04"),
            ("PanelType", "\ud83d\udda5\ufe0f")
        },
        ["Bilgisayar Kasası"] = new (string, string)[4]
        {
            ("FormFactor", "\ud83d\udcd0"),
            ("IncludesPowerSupply", "⚡"),
            ("", ""),
            ("", "")
        }
    };

    private Panel filterPanel = null;

    private Button btnApplyFilter = null;

    private Dictionary<string, object> activeFilters = new Dictionary<string, object>();

    private List<Product> currentRawProducts = new List<Product>();

    public CompareWizardControl(DataService dataService)
    {
        _dataService = dataService;
        InitializeUI();
        LoadCategories();
        ThemeManager.ThemeChanged += delegate (object? s, AppTheme t)
        {
            if (base.InvokeRequired)
            {
                Invoke(delegate
                {
                    ApplyTheme(t);
                });
            }
            else
            {
                ApplyTheme(t);
            }
        };
        ApplyTheme(ThemeManager.Current);
    }

    private void InitializeUI()
    {
        Dock = DockStyle.Fill;
        base.Padding = new Padding(0);
        topBar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 56,
            Padding = new Padding(14, 10, 14, 10)
        };
        FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };
        Label value = new Label
        {
            Text = "Kategori:",
            AutoSize = false,
            Width = 80,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold)
        };
        cbCategory = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 11f),
            Width = 200,
            Height = 36
        };
        cbCategory.SelectedIndexChanged += OnCategoryChanged;
        flowLayoutPanel.Controls.Add(value);
        flowLayoutPanel.Controls.Add(cbCategory);
        topBar.Controls.Add(flowLayoutPanel);
        cardArea = new Panel
        {
            Dock = DockStyle.Fill,
            Visible = true
        };
        filterPanel = new Panel
        {
            Dock = DockStyle.Right,
            Width = 230,
            BackColor = ThemeManager.Current.Light,
            Padding = new Padding(12),
            AutoScroll = true
        };
        filterPanel.Paint += delegate (object? s, PaintEventArgs e)
        {
            using Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1f);
            e.Graphics.DrawLine(pen, 0, 0, 0, filterPanel.Height);
        };
        cardFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            AutoScroll = true,
            Padding = new Padding(15, 12, 15, 12)
        };
        cardArea.Controls.Add(cardFlow);
        cardArea.Controls.Add(filterPanel);
        cardFlow.BringToFront();
        selectionStrip = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 52,
            Padding = new Padding(14, 8, 14, 8),
            Visible = false
        };
        FlowLayoutPanel flowLayoutPanel2 = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };
        lblSel1 = MakeStripLabel("—");
        lblVs = MakeStripLabel("vs", bold: true);
        lblSel2 = MakeStripLabel("—");
        btnCompare = new Button
        {
            Text = "Karşılaştır ▶",
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Width = 150,
            Height = 36,
            Cursor = Cursors.Hand,
            Enabled = false
        };
        btnCompare.FlatAppearance.BorderSize = 0;
        btnCompare.Click += OnCompareClicked;

        btnClearSelection = new Button
        {
            Text = "✕ Seçenekleri Kaldır",
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Width = 180,
            Height = 36,
            Cursor = Cursors.Hand,
            BackColor = Color.FromArgb(200, 50, 50),
            ForeColor = Color.White
        };
        btnClearSelection.FlatAppearance.BorderSize = 0;
        btnClearSelection.Click += OnClearSelectionClicked;

        flowLayoutPanel2.Controls.AddRange(lblSel1, lblVs, lblSel2, btnCompare, btnClearSelection);
        selectionStrip.Controls.Add(flowLayoutPanel2);
        compareArea = new Panel
        {
            Dock = DockStyle.Fill,
            Visible = false
        };
        Panel panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 48,
            Padding = new Padding(10, 8, 10, 8)
        };
        btnBack = new Button
        {
            Text = "← Geri Dön",
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f),
            Width = 130,
            Height = 34,
            Cursor = Cursors.Hand
        };
        btnBack.FlatAppearance.BorderSize = 0;
        btnBack.Click += OnBackClicked;
        panel.Controls.Add(btnBack);
        gridCompare = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            BorderStyle = BorderStyle.None,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToResizeRows = false,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        gridCompare.DefaultCellStyle.Font = new Font("Segoe UI", 11f);
        gridCompare.DefaultCellStyle.Padding = new Padding(6);
        gridCompare.RowTemplate.Height = 42;
        gridCompare.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
        gridCompare.ColumnHeadersDefaultCellStyle.Padding = new Padding(6);
        gridCompare.ColumnHeadersHeight = 46;
        gridCompare.EnableHeadersVisualStyles = false;
        gridCompare.Columns.Add("Ozellik", "Özellik");
        gridCompare.Columns.Add("Urun1", "1. Ürün");
        gridCompare.Columns.Add("Urun2", "2. Ürün");
        pnlCompareResultHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 180,
            BackColor = ThemeManager.Current.Light,
            Padding = new Padding(10)
        };
        compareArea.Controls.Add(gridCompare);
        compareArea.Controls.Add(pnlCompareResultHeader);
        compareArea.Controls.Add(panel);
        base.Controls.Add(compareArea);
        base.Controls.Add(cardArea);
        base.Controls.Add(selectionStrip);
        base.Controls.Add(topBar);

        // Z-order ve Dock düzenini sağlamak için (üst üste binmeyi önler)
        // WinForms'ta Dock.Fill olan kontrol en önde (Z-order index 0) olmalıdır ki barların üzerinde kalmasın.
        cardArea.BringToFront();
        compareArea.SendToBack();
    }

    private static Label MakeStripLabel(string text, bool bold = false)
    {
        return new Label
        {
            Text = text,
            AutoSize = false,
            Width = 200,
            Height = 36,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 10f, bold ? FontStyle.Bold : FontStyle.Regular)
        };
    }

    private void LoadCategories()
    {
        cbCategory.Items.AddRange("CPU", "GPU", "RAM", "Anakart", "SSD", "Bilgisayar Kasası", "Telefon", "Tablet", "Laptop", "Televizyon", "Monitör");
        cbCategory.SelectedIndex = 0;
    }

    private void OnCategoryChanged(object? sender, EventArgs e)
    {
        ShowCardView();
        _selectedProducts.Clear();
        _cardPanels.Clear();
        cardFlow.Controls.Clear();
        UpdateStrip();
        string text = cbCategory.SelectedItem?.ToString() ?? "";
        List<Product> list = text switch
        {
            "CPU" => _dataService.GetProductsByType<CPU>().Cast<Product>().ToList(),
            "GPU" => _dataService.GetProductsByType<GPU>().Cast<Product>().ToList(),
            "RAM" => _dataService.GetProductsByType<RAM>().Cast<Product>().ToList(),
            "Anakart" => _dataService.GetProductsByType<Motherboard>().Cast<Product>().ToList(),
            "SSD" => _dataService.GetProductsByType<Storage>().Cast<Product>().ToList(),
            "Bilgisayar Kasası" => _dataService.GetProductsByType<PC_Case>().Cast<Product>().ToList(),
            "Telefon" => _dataService.GetProductsByType<Phone>().Cast<Product>().ToList(),
            "Tablet" => _dataService.GetProductsByType<Tablet>().Cast<Product>().ToList(),
            "Laptop" => _dataService.GetProductsByType<Laptop>().Cast<Product>().ToList(),
            "Televizyon" => _dataService.GetProductsByType<Television>().Cast<Product>().ToList(),
            "Monitör" => _dataService.GetProductsByType<ProductWizardApp.Models.Monitor>().Cast<Product>().ToList(),
            _ => new List<Product>(),
        };
        currentRawProducts = list;
        BuildFilterUI(text);
        ApplyFilters();
    }

    private void BuildFilterUI(string cat)
    {
        filterPanel.Controls.Clear();
        activeFilters.Clear();
        Button button = new Button
        {
            Text = "FİLTRELE",
            Dock = DockStyle.Top,
            Height = 38,
            BackColor = ThemeManager.Current.Primary,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 0, 20)
        };
        button.FlatAppearance.BorderSize = 0;
        button.Click += delegate
        {
            ApplyFilters();
        };
        filterPanel.Controls.Add(button);
        button.BringToFront();
        AddHeader("Marka");
        AddCombo("Marka", currentRawProducts.Select((Product p) => p.Brand));
        switch (cat)
        {
            case "CPU":
                AddHeader("Çekirdek");
                AddCombo("Cores", from p in currentRawProducts.OfType<CPU>()
                                  select p.Cores.ToString());
                AddHeader("Soket");
                AddCombo("Socket", from p in currentRawProducts.OfType<CPU>()
                                   select p.Socket);
                break;
            case "GPU":
                AddHeader("VRAM (GB)");
                AddCombo("VRAM", from p in currentRawProducts.OfType<GPU>()
                                 select p.VRAM_GB.ToString());
                AddHeader("Ray Tracing");
                AddCombo("RT", new string[2] { "Evet", "Hayır" });
                break;
            case "SSD":
                AddHeader("Kapasite (GB)");
                AddCombo("Capacity", from p in currentRawProducts.OfType<Storage>()
                                     select p.Capacity_GB.ToString());
                AddHeader("Disk Türü");
                AddCombo("DiskType", from p in currentRawProducts.OfType<Storage>()
                                     select p.StorageType);
                break;
            case "Anakart":
                AddHeader("Soket");
                AddCombo("Socket", from p in currentRawProducts.OfType<Motherboard>()
                                   select p.Soket);
                AddHeader("RAM Tipi");
                AddCombo("RamType", from p in currentRawProducts.OfType<Motherboard>()
                                    select p.RamTipi);
                break;
            case "Bilgisayar Kasası":
                AddHeader("Kasa Tipi");
                AddCombo("Form", from p in currentRawProducts.OfType<PC_Case>()
                                 select p.FormFactor);
                AddHeader("Sıvı Soğutma");
                AddCombo("Liquid", new string[2] { "Evet", "Hayır" });
                break;
            case "Laptop":
                AddHeader("RAM (GB)");
                AddCombo("RAM", from p in currentRawProducts.OfType<Laptop>()
                                select p.RAM_GB.ToString());
                AddHeader("Ekran Boyutu");
                AddCombo("Screen", from p in currentRawProducts.OfType<Laptop>()
                                   select p.ScreenSize.ToString());
                break;
            case "Telefon":
            case "Tablet":
                {
                    AddHeader("RAM (GB)");
                    AddCombo("RAM", (cat == "Telefon") ? (from p in currentRawProducts.OfType<Phone>()
                                                          select p.RAM_GB.ToString()) : (from p in currentRawProducts.OfType<Tablet>()
                                                                                         select p.Storage_GB.ToString()));
                    AddHeader("İşletim Sistemi");
                    IEnumerable<string> items;
                    if (!(cat == "Telefon"))
                    {
                        IEnumerable<string> enumerable = new string[2] { "Android", "iPadOS" };
                        items = enumerable;
                    }
                    else
                    {
                        items = from p in currentRawProducts.OfType<Phone>()
                                select p.OS_Version;
                    }
                    AddCombo("OS", items);
                    break;
                }
            case "RAM":
                AddHeader("Kapasite (GB)");
                AddCombo("Capacity", from p in currentRawProducts.OfType<RAM>()
                                     select p.KapasiteGB.ToString());
                AddHeader("RAM Tipi");
                AddCombo("RamType", from p in currentRawProducts.OfType<RAM>()
                                    select p.RamTipi);
                break;
        }
        AddHeader("Max Fiyat (TL)");
        TextBox textBox = new TextBox
        {
            Dock = DockStyle.Top,
            Text = ""
        };
        filterPanel.Controls.Add(textBox);
        textBox.BringToFront();
        activeFilters["Price"] = textBox;
        AddHeader("Min Puan (Epey)");
        TrackBar trackBar = new TrackBar
        {
            Dock = DockStyle.Top,
            Minimum = 0,
            Maximum = 100,
            Value = 0,
            TickFrequency = 10,
            Height = 45
        };
        filterPanel.Controls.Add(trackBar);
        trackBar.BringToFront();
        activeFilters["Score"] = trackBar;

        Button btnClearFilters = new Button
        {
            Text = "Filtreleri Kaldır",
            Dock = DockStyle.Top,
            Height = 32,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            BackColor = Color.FromArgb(200, 50, 50),
            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 10, 0, 0)
        };
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.Click += delegate
        {
            ResetFilters();
        };
        filterPanel.Controls.Add(btnClearFilters);
        btnClearFilters.BringToFront();
        ComboBox AddCombo(string key, IEnumerable<string> source)
        {
            ComboBox comboBox = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 0, 5)
            };
            comboBox.Items.Add("Tümü");
            comboBox.Items.AddRange((from x in source.Distinct()
                                     orderby x
                                     select x).Cast<object>().ToArray());
            comboBox.SelectedIndex = 0;
            filterPanel.Controls.Add(comboBox);
            comboBox.BringToFront();
            activeFilters[key] = comboBox;
            return comboBox;
        }
        void AddHeader(string text)
        {
            Label label = new Label
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomLeft,
                Margin = new Padding(0, 10, 0, 0)
            };
            filterPanel.Controls.Add(label);
            label.BringToFront();
        }
    }

    private void ResetFilters()
    {
        foreach (var kvp in activeFilters)
        {
            if (kvp.Value is ComboBox cb)
            {
                cb.SelectedIndex = 0;
            }
            else if (kvp.Value is TextBox tb)
            {
                tb.Text = "";
            }
            else if (kvp.Value is TrackBar tr)
            {
                tr.Value = 0;
            }
        }
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        IEnumerable<Product> enumerable = currentRawProducts.AsEnumerable();
        if (activeFilters.TryGetValue("Marka", out object value))
        {
            ComboBox cbM = value as ComboBox;
            if (cbM != null && cbM.SelectedIndex > 0)
            {
                enumerable = enumerable.Where((Product p) => p.Brand == cbM.SelectedItem.ToString());
            }
        }
        string text = cbCategory.SelectedItem?.ToString() ?? "";
        switch (text)
        {
            case "CPU":
                {
                    if (activeFilters.TryGetValue("Cores", out object value12))
                    {
                        ComboBox cb = value12 as ComboBox;
                        if (cb != null && cb.SelectedIndex > 0)
                        {
                            enumerable = (from p in enumerable.OfType<CPU>()
                                          where p.Cores.ToString() == cb.SelectedItem.ToString()
                                          select p).Cast<Product>();
                        }
                    }
                    if (!activeFilters.TryGetValue("Socket", out object value13))
                    {
                        break;
                    }
                    ComboBox cbs2 = value13 as ComboBox;
                    if (cbs2 != null && cbs2.SelectedIndex > 0)
                    {
                        enumerable = (from p in enumerable.OfType<CPU>()
                                      where p.Socket == cbs2.SelectedItem.ToString()
                                      select p).Cast<Product>();
                    }
                    break;
                }
            case "GPU":
                {
                    if (activeFilters.TryGetValue("VRAM", out object value14))
                    {
                        ComboBox cbv3 = value14 as ComboBox;
                        if (cbv3 != null && cbv3.SelectedIndex > 0)
                        {
                            enumerable = (from p in enumerable.OfType<GPU>()
                                          where p.VRAM_GB.ToString() == cbv3.SelectedItem.ToString()
                                          select p).Cast<Product>();
                        }
                    }
                    if (!activeFilters.TryGetValue("RT", out object value15))
                    {
                        break;
                    }
                    ComboBox cbr3 = value15 as ComboBox;
                    if (cbr3 != null && cbr3.SelectedIndex > 0)
                    {
                        enumerable = (from p in enumerable.OfType<GPU>()
                                      where p.HasRayTracing == (cbr3.SelectedItem.ToString() == "Evet")
                                      select p).Cast<Product>();
                    }
                    break;
                }
            case "SSD":
                {
                    if (activeFilters.TryGetValue("Capacity", out object value6))
                    {
                        ComboBox cbv2 = value6 as ComboBox;
                        if (cbv2 != null && cbv2.SelectedIndex > 0)
                        {
                            enumerable = (from p in enumerable.OfType<Storage>()
                                          where p.Capacity_GB.ToString() == cbv2.SelectedItem.ToString()
                                          select p).Cast<Product>();
                        }
                    }
                    if (!activeFilters.TryGetValue("DiskType", out object value7))
                    {
                        break;
                    }
                    ComboBox cbd2 = value7 as ComboBox;
                    if (cbd2 != null && cbd2.SelectedIndex > 0)
                    {
                        enumerable = (from p in enumerable.OfType<Storage>()
                                      where p.StorageType == cbd2.SelectedItem.ToString()
                                      select p).Cast<Product>();
                    }
                    break;
                }
            case "Anakart":
                {
                    if (activeFilters.TryGetValue("Socket", out object value16))
                    {
                        ComboBox cbs3 = value16 as ComboBox;
                        if (cbs3 != null && cbs3.SelectedIndex > 0)
                        {
                            enumerable = (from p in enumerable.OfType<Motherboard>()
                                          where p.Soket == cbs3.SelectedItem.ToString()
                                          select p).Cast<Product>();
                        }
                    }
                    if (!activeFilters.TryGetValue("RamType", out object value17))
                    {
                        break;
                    }
                    ComboBox cbr4 = value17 as ComboBox;
                    if (cbr4 != null && cbr4.SelectedIndex > 0)
                    {
                        enumerable = (from p in enumerable.OfType<Motherboard>()
                                      where p.RamTipi == cbr4.SelectedItem.ToString()
                                      select p).Cast<Product>();
                    }
                    break;
                }
            case "Bilgisayar Kasası":
                {
                    if (activeFilters.TryGetValue("Form", out object value8))
                    {
                        ComboBox cbf = value8 as ComboBox;
                        if (cbf != null && cbf.SelectedIndex > 0)
                        {
                            enumerable = (from p in enumerable.OfType<PC_Case>()
                                          where p.FormFactor == cbf.SelectedItem.ToString()
                                          select p).Cast<Product>();
                        }
                    }
                    if (!activeFilters.TryGetValue("Liquid", out object value9))
                    {
                        break;
                    }
                    ComboBox cbl = value9 as ComboBox;
                    if (cbl != null && cbl.SelectedIndex > 0)
                    {
                        enumerable = (from p in enumerable.OfType<PC_Case>()
                                      where p.HasLiquidCoolingSupport == (cbl.SelectedItem.ToString() == "Evet")
                                      select p).Cast<Product>();
                    }
                    break;
                }
            case "Laptop":
                {
                    if (activeFilters.TryGetValue("RAM", out object value4))
                    {
                        ComboBox cbr = value4 as ComboBox;
                        if (cbr != null && cbr.SelectedIndex > 0)
                        {
                            enumerable = (from p in enumerable.OfType<Laptop>()
                                          where p.RAM_GB.ToString() == cbr.SelectedItem.ToString()
                                          select p).Cast<Product>();
                        }
                    }
                    if (!activeFilters.TryGetValue("Screen", out object value5))
                    {
                        break;
                    }
                    ComboBox cbs = value5 as ComboBox;
                    if (cbs != null && cbs.SelectedIndex > 0)
                    {
                        enumerable = (from p in enumerable.OfType<Laptop>()
                                      where p.ScreenSize.ToString() == cbs.SelectedItem.ToString()
                                      select p).Cast<Product>();
                    }
                    break;
                }
            case "Telefon":
                {
                    if (activeFilters.TryGetValue("RAM", out object value10))
                    {
                        ComboBox cbr2 = value10 as ComboBox;
                        if (cbr2 != null && cbr2.SelectedIndex > 0)
                        {
                            enumerable = (from p in enumerable.OfType<Phone>()
                                          where p.RAM_GB.ToString() == cbr2.SelectedItem.ToString()
                                          select p).Cast<Product>();
                        }
                    }
                    if (!activeFilters.TryGetValue("OS", out object value11))
                    {
                        break;
                    }
                    ComboBox cbo = value11 as ComboBox;
                    if (cbo != null && cbo.SelectedIndex > 0)
                    {
                        enumerable = (from p in enumerable.OfType<Phone>()
                                      where p.OS_Version == cbo.SelectedItem.ToString()
                                      select p).Cast<Product>();
                    }
                    break;
                }
            case "RAM":
                {
                    if (activeFilters.TryGetValue("Capacity", out object value2))
                    {
                        ComboBox cbv = value2 as ComboBox;
                        if (cbv != null && cbv.SelectedIndex > 0)
                        {
                            enumerable = (from p in enumerable.OfType<RAM>()
                                          where p.KapasiteGB.ToString() == cbv.SelectedItem.ToString()
                                          select p).Cast<Product>();
                        }
                    }
                    if (!activeFilters.TryGetValue("RamType", out object value3))
                    {
                        break;
                    }
                    ComboBox cbd = value3 as ComboBox;
                    if (cbd != null && cbd.SelectedIndex > 0)
                    {
                        enumerable = (from p in enumerable.OfType<RAM>()
                                      where p.RamTipi == cbd.SelectedItem.ToString()
                                      select p).Cast<Product>();
                    }
                    break;
                }
        }
        if (activeFilters.TryGetValue("Price", out object value18) && value18 is TextBox textBox && decimal.TryParse(textBox.Text, out var maxPrice))
        {
            enumerable = enumerable.Where((Product p) => p.Price <= maxPrice);
        }
        if (activeFilters.TryGetValue("Score", out object value19))
        {
            TrackBar trS = value19 as TrackBar;
            if (trS != null && trS.Value > 0)
            {
                enumerable = enumerable.Where((Product p) => Math.Abs(p.Name.GetHashCode()) % 30 + 70 >= trS.Value);
            }
        }
        cardFlow.Controls.Clear();
        _cardPanels.Clear();
        if (!CategorySpecs.TryGetValue(text, out var specs))
        {
            specs = Array.Empty<(string, string)>();
        }
        foreach (Product item in enumerable)
        {
            Panel value21 = BuildCard(item, specs);
            _cardPanels[item] = value21;
            cardFlow.Controls.Add(value21);
        }
    }

    private Panel BuildCard(Product product, (string prop, string icon)[] specs)
    {
        AppTheme theme = ThemeManager.Current;
        Panel card = new Panel
        {
            Width = 300,
            Height = 420,
            Margin = new Padding(0, 0, 15, 15),
            Cursor = Cursors.Hand,
            Tag = product
        };
        card.Paint += delegate (object? s, PaintEventArgs e)
        {
            DrawCardBorder(e.Graphics, card, theme);
        };
        PictureBox pictureBox = new PictureBox
        {
            Width = 280,
            Height = 200,
            Left = 10,
            Top = 10,
            SizeMode = PictureBoxSizeMode.Zoom
        };
        DataService.LoadProductImage(pictureBox, product);
        Panel panel = new Panel
        {
            Left = 10,
            Top = 215,
            Width = 280,
            Height = 195
        };
        Label value = new Label
        {
            Text = product.Brand + " " + product.Name,
            Left = 0,
            Top = 0,
            Width = 230,
            Height = 40,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            ForeColor = theme.TextOnLight,
            AutoEllipsis = true,
            UseMnemonic = false
        };
        panel.Controls.Add(value);
        Label value2 = new Label
        {
            Text = product.Price.ToString("N0") + " TL",
            Left = 140,
            Top = 160,
            Width = 130,
            Height = 32,
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            ForeColor = Color.Black,
            TextAlign = ContentAlignment.MiddleRight,
            AutoEllipsis = true,
            UseMnemonic = false
        };
        panel.Controls.Add(value2);
        int epeyScore = Math.Abs(product.Name.GetHashCode()) % 30 + 70;
        Panel panel2 = new Panel
        {
            Left = 235,
            Top = 0,
            Width = 40,
            Height = 40,
            Cursor = Cursors.Hand
        };
        panel2.Paint += delegate (object? s, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Color color = ((epeyScore >= 85) ? Color.FromArgb(76, 175, 80) : Color.FromArgb(255, 152, 0));
            using SolidBrush brush = new SolidBrush(color);
            e.Graphics.FillEllipse(brush, 4, 4, 32, 32);
            using Pen pen = new Pen(Color.FromArgb(220, 220, 220), 3f);
            e.Graphics.DrawEllipse(pen, 2, 2, 36, 36);
            using Pen pen2 = new Pen(color, 3f);
            pen2.StartCap = LineCap.Round;
            pen2.EndCap = LineCap.Round;
            float sweepAngle = (float)epeyScore / 100f * 360f;
            e.Graphics.DrawArc(pen2, 2f, 2f, 36f, 36f, -90f, sweepAngle);
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            using Font font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            e.Graphics.DrawString(epeyScore.ToString(), font, Brushes.White, new RectangleF(4f, 4f, 32f, 32f), format);
        };
        panel.Controls.Add(panel2);
        PropertyInfo[] properties = product.GetType().GetProperties();
        int xi = 0;
        for (xi = 0; xi < Math.Min(specs.Length, 4); xi++)
        {
            if (string.IsNullOrEmpty(specs[xi].prop))
            {
                continue;
            }
            PropertyInfo propertyInfo = properties.FirstOrDefault((PropertyInfo p) => p.Name == specs[xi].prop);
            if (!(propertyInfo == null))
            {
                object value3 = propertyInfo.GetValue(product);
                if (value3 != null)
                {
                    string text = ((!(value3 is bool)) ? (value3.ToString() ?? "") : (((bool)value3) ? "Evet" : "Hayır"));
                    string text2 = specs[xi].icon + " " + text;
                    Label value4 = new Label
                    {
                        Text = text2,
                        AutoSize = false,
                        Left = 0,
                        Top = 45 + xi * 28,
                        Width = 270,
                        Height = 24,
                        Font = new Font("Segoe UI", 9f),
                        ForeColor = Color.FromArgb(80, 80, 80),
                        AutoEllipsis = true,
                        UseMnemonic = false
                    };
                    panel.Controls.Add(value4);
                }
            }
        }
        Button button = new Button
        {
            Text = "Seç",
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Width = 140,
            Height = 32,
            Left = 0,
            Top = 160,
            Cursor = Cursors.Hand,
            BackColor = theme.Primary,
            ForeColor = theme.TextOnPrimary,
            Name = "btnSec",
            TabStop = false,
            Margin = new Padding(0)
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        button.Click += delegate
        {
            ToggleSelect(product);
        };
        panel.Controls.Add(button);
        card.Controls.Add(pictureBox);
        card.Controls.Add(panel);
        card.Click += OpenDetailHandler;
        foreach (Control control in panel.Controls)
        {
            if (!(control is Button))
            {
                control.Click += OpenDetailHandler;
            }
        }
        pictureBox.Click += OpenDetailHandler;
        return card;
        void OpenDetailHandler(object? s, EventArgs e)
        {
            if (!(s is Button { Name: "btnSec" }))
            {
                ProductDetailForm productDetailForm = new ProductDetailForm(product);
                productDetailForm.ShowDialog(this);
            }
        }
    }

    private void DrawCardBorder(Graphics g, Panel card, AppTheme theme)
    {
        bool flag = _selectedProducts.Contains((Product)card.Tag);
        using SolidBrush brush = new SolidBrush(flag ? Color.FromArgb(30, theme.Primary.R, theme.Primary.G, theme.Primary.B) : theme.Light);
        g.FillRectangle(brush, card.ClientRectangle);
        using Pen pen = new Pen(flag ? theme.Primary : Color.FromArgb(210, 210, 210), flag ? 2.5f : 1f);
        g.DrawRectangle(pen, 1, 1, card.Width - 3, card.Height - 3);
    }

    private void ToggleSelect(Product product)
    {
        if (_selectedProducts.Contains(product))
        {
            _selectedProducts.Remove(product);
        }
        else
        {
            if (_selectedProducts.Count >= 2)
            {
                return;
            }
            _selectedProducts.Add(product);
        }
        foreach (var (item, panel) in _cardPanels)
        {
            bool flag = _selectedProducts.Contains(item);
            bool enabled = _selectedProducts.Count < 2 || flag;
            if (panel.Controls.Find("btnSec", searchAllChildren: true).FirstOrDefault() is Button button)
            {
                button.Text = (flag ? "✔ Seçildi" : "Seç");
                button.Enabled = enabled;
                button.BackColor = (flag ? ThemeManager.Current.Secondary : ThemeManager.Current.Primary);
            }
            panel.Invalidate();
        }
        UpdateStrip();
    }

    private void UpdateStrip()
    {
        lblSel1.Text = ((_selectedProducts.Count > 0) ? (_selectedProducts[0].Brand + " " + _selectedProducts[0].Name) : "—");
        lblSel2.Text = ((_selectedProducts.Count > 1) ? (_selectedProducts[1].Brand + " " + _selectedProducts[1].Name) : "—");
        btnCompare.Enabled = _selectedProducts.Count == 2;
        selectionStrip.Visible = _selectedProducts.Count > 0;
    }

    private void OnClearSelectionClicked(object? sender, EventArgs e)
    {
        _selectedProducts.Clear();
        foreach (var (item, panel) in _cardPanels)
        {
            if (panel.Controls.Find("btnSec", searchAllChildren: true).FirstOrDefault() is Button button)
            {
                button.Text = "Seç";
                button.Enabled = true;
                button.BackColor = ThemeManager.Current.Primary;
                button.ForeColor = ThemeManager.Current.TextOnPrimary;
            }
            panel.Invalidate();
        }
        UpdateStrip();
    }

    private void OnCompareClicked(object? sender, EventArgs e)
    {
        if (_selectedProducts.Count < 2)
        {
            return;
        }
        Product product = _selectedProducts[0];
        Product product2 = _selectedProducts[1];
        pnlCompareResultHeader.Controls.Clear();
        pnlCompareResultHeader.Height = 350;
        AppTheme theme = ThemeManager.Current;

        // Build a TableLayoutPanel with 3 columns matching the grid layout
        TableLayoutPanel headerTable = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1,
            BackColor = Color.Transparent
        };
        headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        headerTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        // Column 0: empty (matches "Özellik" column)
        headerTable.Controls.Add(new Panel { BackColor = Color.Transparent }, 0, 0);

        // Column 1: Product 1 summary (centered above its comparison column)
        headerTable.Controls.Add(BuildProductSummaryPanel(product, theme), 1, 0);

        // Column 2: Product 2 summary (centered above its comparison column)
        headerTable.Controls.Add(BuildProductSummaryPanel(product2, theme), 2, 0);

        pnlCompareResultHeader.Controls.Add(headerTable);

        gridCompare.Columns[1].HeaderText = product.Brand + " " + product.Name;
        gridCompare.Columns[2].HeaderText = product2.Brand + " " + product2.Name;
        gridCompare.Rows.Clear();
        CultureInfo c = new CultureInfo("tr-TR");
        PropertyInfo[] properties = product.GetType().GetProperties();
        foreach (PropertyInfo propertyInfo in properties)
        {
            if (propertyInfo.Name == "Id" || propertyInfo.Name == "ImageUrl" || propertyInfo.Name == "Image")
            {
                continue;
            }
            object value = propertyInfo.GetValue(product);
            object value2 = propertyInfo.GetValue(product2);
            string text = propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? propertyInfo.Name;
            string text2 = FormatVal(propertyInfo.Name, value, c);
            string text3 = FormatVal(propertyInfo.Name, value2, c);
            int index = gridCompare.Rows.Add(text, text2, text3);
            DataGridViewRow dataGridViewRow = gridCompare.Rows[index];
            if (value is IComparable comp1 && value2 is IComparable comp2 && value.GetType() == value2.GetType())
            {
                if (value is int || value is decimal || value is double || value is float)
                {
                    int num2 = comp1.CompareTo(comp2);
                    if (num2 != 0)
                    {
                        bool flag4 = propertyInfo.Name == "Price";
                        bool flag5 = (num2 > 0 && !flag4) || (num2 < 0 && flag4);
                        dataGridViewRow.Cells[1].Style.BackColor = (flag5 ? Color.FromArgb(200, 255, 200) : Color.FromArgb(255, 200, 200));
                        dataGridViewRow.Cells[1].Style.ForeColor = (flag5 ? Color.DarkGreen : Color.DarkRed);
                        dataGridViewRow.Cells[2].Style.BackColor = (flag5 ? Color.FromArgb(255, 200, 200) : Color.FromArgb(200, 255, 200));
                        dataGridViewRow.Cells[2].Style.ForeColor = (flag5 ? Color.DarkRed : Color.DarkGreen);
                    }
                }
            }
        }
        gridCompare.ClearSelection();
        cardArea.Visible = false;
        selectionStrip.Visible = false;
        compareArea.Visible = true;
        compareArea.BringToFront();
    }

    private Panel BuildProductSummaryPanel(Product p, AppTheme theme)
    {
        Panel panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent
        };

        // Product image — centered horizontally, at top
        PictureBox pictureBox = new PictureBox
        {
            Width = 200,
            Height = 200,
            SizeMode = PictureBoxSizeMode.Zoom,
            Anchor = AnchorStyles.Top
        };
        DataService.LoadProductImage(pictureBox, p);

        // Product name label — centered below image
        Label lblName = new Label
        {
            Text = p.Brand + "\n" + p.Name,
            Width = 200,
            Height = 42,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = theme.Dark,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoEllipsis = true,
            UseMnemonic = false,
            Anchor = AnchorStyles.Top
        };

        // Epey score badge
        int score = Math.Abs(p.Name.GetHashCode()) % 30 + 70;
        Panel badgePanel = new Panel
        {
            Width = 36,
            Height = 36,
            Anchor = AnchorStyles.Top
        };
        badgePanel.Paint += delegate (object? s, PaintEventArgs ev)
        {
            ev.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Color color = ((score >= 85) ? Color.FromArgb(76, 175, 80) : Color.FromArgb(255, 152, 0));
            using SolidBrush brush = new SolidBrush(color);
            ev.Graphics.FillEllipse(brush, 2, 2, 32, 32);
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            ev.Graphics.DrawString(score.ToString(), new Font("Segoe UI", 9f, FontStyle.Bold), Brushes.White, new RectangleF(2f, 2f, 32f, 32f), format);
        };

        Label lblScore = new Label
        {
            Text = "Epey Puanı",
            AutoSize = true,
            Font = new Font("Segoe UI", 8f),
            ForeColor = Color.Gray,
            Anchor = AnchorStyles.Top
        };

        panel.Controls.Add(pictureBox);
        panel.Controls.Add(lblName);
        panel.Controls.Add(badgePanel);
        panel.Controls.Add(lblScore);

        // Use Resize to center controls dynamically within the panel
        panel.Resize += delegate
        {
            int cx = panel.Width / 2;
            pictureBox.Left = cx - pictureBox.Width / 2;
            pictureBox.Top = 2;

            lblName.Left = cx - lblName.Width / 2;
            lblName.Top = pictureBox.Bottom + 4;

            badgePanel.Left = cx - badgePanel.Width / 2 - 30;
            badgePanel.Top = lblName.Bottom + 2;

            lblScore.Left = badgePanel.Right + 4;
            lblScore.Top = badgePanel.Top + (badgePanel.Height - lblScore.Height) / 2;
        };

        return panel;
    }

    private void OnBackClicked(object? sender, EventArgs e)
    {
        ShowCardView();
    }

    private void ShowCardView()
    {
        compareArea.Visible = false;
        cardArea.Visible = true;
        cardArea.BringToFront();
        selectionStrip.Visible = _selectedProducts.Count > 0;
    }

    private static string FormatVal(string name, object? v, CultureInfo c)
    {
        if (v == null)
        {
            return "—";
        }
        string result = name switch
        {
            "Price" => string.Format(c, "{0:N2} ₺", v),
            "IncludesPowerSupply" => ((bool)v) ? "Evet" : "Hayır",
            "HasStylus" => ((bool)v) ? "Evet" : "Hayır",
            "BaseClockGHz" => string.Format(c, "{0:N2} GHz", v),
            "CoreClockMHz" => string.Format(c, "{0:N0} MHz", v),
            "ScreenSize" => string.Format(c, "{0:N1}\"", v),
            _ => v.ToString() ?? "—",
        };
        return result;
    }

    private void ApplyTheme(AppTheme t)
    {
        Panel panel = topBar;
        Panel panel2 = cardArea;
        Color color = (cardFlow.BackColor = t.Light);
        Color color2 = (panel2.BackColor = color);
        Color backColor = (panel.BackColor = color2);
        BackColor = backColor;
        filterPanel.BackColor = t.Light;
        selectionStrip.BackColor = Darken(t.Light, 10);
        compareArea.BackColor = t.Light;
        foreach (Control control5 in topBar.Controls[0].Controls)
        {
            if (control5 is Label label)
            {
                label.ForeColor = t.Dark;
                label.BackColor = Color.Transparent;
            }
        }
        foreach (Control control6 in filterPanel.Controls)
        {
            if (control6 is Label label2)
            {
                label2.ForeColor = t.Dark;
                label2.BackColor = Color.Transparent;
            }
            if (control6 is Button { Text: "FİLTRELE" } button)
            {
                button.BackColor = t.Primary;
                button.ForeColor = t.TextOnPrimary;
            }
            if (control6 is Button btn && btn.Text == "Filtreleri Kaldır")
            {
                btn.BackColor = Color.FromArgb(200, 50, 50);
                btn.ForeColor = Color.White;
            }
        }
        btnBack.BackColor = t.Primary;
        btnBack.ForeColor = t.TextOnPrimary;
        btnBack.FlatAppearance.BorderSize = 0;
        btnCompare.BackColor = t.Primary;
        btnCompare.ForeColor = t.TextOnPrimary;
        if (btnClearSelection != null)
        {
            btnClearSelection.BackColor = Color.FromArgb(200, 50, 50);
            btnClearSelection.ForeColor = Color.White;
        }
        Label label3 = lblSel1;
        backColor = (lblSel2.ForeColor = t.TextOnLight);
        label3.ForeColor = backColor;
        lblVs.ForeColor = t.Primary;
        gridCompare.BackgroundColor = t.Light;
        gridCompare.GridColor = Darken(t.Light, 15);
        gridCompare.DefaultCellStyle.BackColor = t.Light;
        gridCompare.DefaultCellStyle.ForeColor = t.TextOnLight;
        gridCompare.DefaultCellStyle.SelectionBackColor = t.Secondary;
        gridCompare.DefaultCellStyle.SelectionForeColor = t.TextOnSecondary;
        gridCompare.ColumnHeadersDefaultCellStyle.BackColor = t.Primary;
        gridCompare.ColumnHeadersDefaultCellStyle.ForeColor = t.TextOnPrimary;
        foreach (Panel value in _cardPanels.Values)
        {
            value.Invalidate();
            foreach (Control control7 in value.Controls)
            {
                if (!(control7 is Panel panel3))
                {
                    continue;
                }
                foreach (Control control8 in panel3.Controls)
                {
                    if (control8 is Label label4)
                    {
                        label4.ForeColor = t.TextOnLight;
                    }
                    if (control8 is Button { Name: "btnSec" } button2)
                    {
                        bool flag = _selectedProducts.Contains((Product)value.Tag);
                        button2.BackColor = (flag ? t.Secondary : t.Primary);
                        button2.ForeColor = (flag ? t.TextOnSecondary : t.TextOnPrimary);
                    }
                }
            }
        }
    }

    private static Color Darken(Color c, int a)
    {
        return Color.FromArgb(Math.Max(0, c.R - a), Math.Max(0, c.G - a), Math.Max(0, c.B - a));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ThemeManager.ThemeChanged -= delegate (object? s, AppTheme t)
            {
                ApplyTheme(t);
            };
        }
        base.Dispose(disposing);
    }
}
