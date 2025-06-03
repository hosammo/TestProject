using Newtonsoft.Json;
using Shopify.IO;
using Shopify.IO.helpers;
using Shopify.IO.Types;
using Shopify_Manager;
using ShopifyHelper.IO;
using ShopifyHelper.IO.ODAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopify_Manager.UI
{
    public partial class ShopifyQtyUpdate_frm : Form
    {
        Variants_DAL vdal = new Variants_DAL();
        Locations_DAL ldal = new Locations_DAL();

        public ShopifyQtyUpdate_frm()
        {
            InitializeComponent();
        }

        #region UpdateZeroQtys
        //private void updateQty_btn_Click(object sender, EventArgs e)
        //{
        //    QtyAvailable_grd.Rows.Clear();

        //    List<Variant> v = General.montaniashop.Products.ZeroQtyVariants;

        //    foreach (Variant vr in v)
        //    {
        //        int OceanQty = General.GetBarcodeQty(vr.barcode);
        //        if (OceanQty != (int)vr.inventory_quantity)
        //        {
        //            if (OceanQty > 0)
        //            {
        //                Variant tv = General.montaniashop.Products.UpdateInvetorey(vr, OceanQty);
        //                if (tv != null)
        //                {
        //                    QtyAvailable_grd.Rows.Add(vr.barcode, vr.inventory_quantity, OceanQty, "", "", "", "", "OK");
        //                    QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
        //                }
        //                else
        //                {
        //                    QtyAvailable_grd.Rows.Add(vr.barcode, vr.inventory_quantity, OceanQty, "", "", "", "", "Failed To Update Qty");
        //                    QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
        //                }
        //            }
        //            else if (OceanQty == 0)
        //            {
        //                QtyAvailable_grd.Rows.Add(vr.barcode, vr.inventory_quantity, OceanQty, "", "", "", "", "No Qty.");
        //                QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
        //            }
        //            else
        //            {
        //                NoQty_grd.Rows.Add(vr.barcode, vr.inventory_quantity, OceanQty);
        //            }
        //        }
        //    }
        //}
        #endregion

        private void ShopifyQtyUpdate_frm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }



        private void UpdateAllQty_btn_Click(object sender, EventArgs e)
        {
            UpdateStockV2();
        }

        private void Legacy_UpdateStockV2()
        {

            //start
            //clear grid rows
            QtyAvailable_grd.Rows.Clear();

            // get all products from shopify api.
            List<Product> lp = Fields.CurrentStore.Products.GetList();

            // get all locations from caching db.
            List<LocationConfig> l = ldal.GetLocationConfig();

            // get total number of variants to populate the max value of the progress bar.
            int pbln = Fields.CurrentStore.Products.GetVariantsCount(lp) * l.Count;
            progressBar1.Maximum = pbln;
            progressBar1.Value = 0;


            int counter = 1;
            // loop all locations configrations.
            foreach (LocationConfig lc in l)
            {
                // loop per product
                foreach (Product pr in lp)
                {
                    // loop on each variant of the current product.
                    foreach (Variant vr in pr.variants)
                    {
                        int OceanQty = vdal.GetVariantQtyFromOcean(vr.barcode, lc);
                        
                        if (OceanQty >= 0)
                        {
                            Variant tv = Fields.CurrentStore.Products.UpdateInvetorey(vr, OceanQty, lc.Id);
                            if (tv.inventory_Level != null)
                            {
                                QtyAvailable_grd.Rows.Add(vr.barcode, lc.Name, vr.inventory_quantity, OceanQty, "", "", "", "", "OK");
                                QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                QtyAvailable_grd.Rows.Add(vr.barcode, lc.Name, vr.inventory_quantity, OceanQty,"", "", "", "", tv.LastError.title[0]);
                                QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                            }
                        }
                        else if (OceanQty == -1)
                        {
                            QtyAvailable_grd.Rows.Add(vr.barcode, vr.inventory_quantity, OceanQty, "", "", "", "", "Barcdode not exist in ocean.");
                            QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                        }
                        this.Refresh();
                        progressBar1.Value += 1;
                    }
                    counter += 1;
                }
            }
            MessageBox.Show("Update Completed");
            //end

        }

        private void ParallelUpdateStockV2()
        {
            // Clear grid rows - must be on UI thread
            if (QtyAvailable_grd.InvokeRequired)
            {
                QtyAvailable_grd.Invoke(new MethodInvoker(() => QtyAvailable_grd.Rows.Clear()));
            }
            else
            {
                QtyAvailable_grd.Rows.Clear();
            }

            // Get data
            List<Product> lp = Fields.CurrentStore.Products.GetList();
            List<LocationConfig> l = ldal.GetLocationConfig();

            int totalVariants = Fields.CurrentStore.Products.GetVariantsCount(lp) * l.Count;
            progressBar1.Maximum = totalVariants;
            progressBar1.Value = 0;

            // Use thread-safe counter
            int progressCounter = 0;
            object lockObj = new object();

            // Flatten the workload into a list of (location, variant) pairs
            var tasks = new List<Tuple<LocationConfig, Variant>>();
            foreach (LocationConfig lc in l)
            {
                foreach (Product pr in lp)
                {
                    foreach (Variant vr in pr.variants)
                    {
                        tasks.Add(new Tuple<LocationConfig, Variant>(lc, vr));
                    }
                }
            }

            Parallel.ForEach(tasks, task =>
            {
                LocationConfig lc = task.Item1;
                Variant vr = task.Item2;

                int OceanQty = vdal.GetVariantQtyFromOcean(vr.barcode, lc);

                if (OceanQty >= 0)
                {
                    Variant tv = Fields.CurrentStore.Products.UpdateInvetorey(vr, OceanQty, lc.Id);

                    Action uiUpdate = () =>
                    {
                        if (tv.inventory_Level != null)
                        {
                            int rowIndex = QtyAvailable_grd.Rows.Add(vr.barcode, lc.Name, vr.inventory_quantity, OceanQty, "", "", "", "", "OK");
                            QtyAvailable_grd.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            int rowIndex = QtyAvailable_grd.Rows.Add(vr.barcode, lc.Name, vr.inventory_quantity, OceanQty, "", "", "", "", tv.LastError.title[0]);
                            QtyAvailable_grd.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                        }
                    };

                    if (QtyAvailable_grd.InvokeRequired)
                        QtyAvailable_grd.Invoke(uiUpdate);
                    else
                        uiUpdate();
                }
                else if (OceanQty == -1)
                {
                    Action uiUpdate = () =>
                    {
                        int rowIndex = QtyAvailable_grd.Rows.Add(vr.barcode, vr.inventory_quantity, OceanQty, "", "", "", "", "Barcode not exist in ocean.");
                        QtyAvailable_grd.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    };

                    if (QtyAvailable_grd.InvokeRequired)
                        QtyAvailable_grd.Invoke(uiUpdate);
                    else
                        uiUpdate();
                }

                // Update progress bar safely
                lock (lockObj)
                {
                    if (progressBar1.InvokeRequired)
                        progressBar1.Invoke(new Action(() => progressBar1.Value++));
                    else
                        progressBar1.Value++;
                }
            });

            MessageBox.Show("Update Completed");
        }

        private async void UpdateStockV2()
        {
            // Clear grid rows - must be on UI thread
            if (QtyAvailable_grd.InvokeRequired)
            {
                QtyAvailable_grd.Invoke(new MethodInvoker(() => QtyAvailable_grd.Rows.Clear()));
            }
            else
            {
                QtyAvailable_grd.Rows.Clear();
            }

            // Get data
            List<Product> lp = Fields.CurrentStore.Products.GetList();
            List<LocationConfig> l = ldal.GetLocationConfig();

            int totalVariants = Fields.CurrentStore.Products.GetVariantsCount(lp) * l.Count;

            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() =>
                {
                    progressBar1.Maximum = totalVariants;
                    progressBar1.Value = 0;
                }));
            }
            else
            {
                progressBar1.Maximum = totalVariants;
                progressBar1.Value = 0;
            }

            object lockObj = new object();

            // Run multithreaded processing off UI thread
            await Task.Run(() =>
            {
                var tasks = new List<Tuple<LocationConfig, Variant>>();
                foreach (LocationConfig lc in l)
                {
                    foreach (Product pr in lp)
                    {
                        foreach (Variant vr in pr.variants)
                        {
                            tasks.Add(new Tuple<LocationConfig, Variant>(lc, vr));
                        }
                    }
                }

                Parallel.ForEach(tasks, task =>
                {
                    LocationConfig lc = task.Item1;
                    Variant vr = task.Item2;

                    int OceanQty = vdal.GetVariantQtyFromOcean(vr.barcode, lc);

                    if (OceanQty >= 0)
                    {
                        Variant tv = Fields.CurrentStore.Products.UpdateInvetorey(vr, OceanQty, lc.Id);

                        Action uiUpdate = () =>
                        {
                            if (tv.inventory_Level != null)
                            {
                                int rowIndex = QtyAvailable_grd.Rows.Add(vr.barcode, lc.Name, vr.inventory_quantity, OceanQty, "", "", "", "", "OK");
                                QtyAvailable_grd.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                int rowIndex = QtyAvailable_grd.Rows.Add(vr.barcode, lc.Name, vr.inventory_quantity, OceanQty, "", "", "", "", tv.LastError.title[0]);
                                QtyAvailable_grd.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                            }
                        };

                        QtyAvailable_grd.Invoke(uiUpdate);
                    }
                    else if (OceanQty == -1)
                    {
                        Action uiUpdate = () =>
                        {
                            int rowIndex = QtyAvailable_grd.Rows.Add(vr.barcode, lc.Name, vr.inventory_quantity, OceanQty, "", "", "", "", "Barcode not exist in ocean.");
                            QtyAvailable_grd.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                        };

                        QtyAvailable_grd.Invoke(uiUpdate);
                    }

                    // Update progress bar safely
                    lock (lockObj)
                    {
                        progressBar1.Invoke(new Action(() => progressBar1.Value++));
                    }
                });
            });

            // Create logs directory
            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "StockUpdates");
            Directory.CreateDirectory(logDir);

            // Build file name
            string logFileName = $"StockUpdate_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string logFilePath = Path.Combine(logDir, logFileName);

            // Export grid
            ExportGridToJson(QtyAvailable_grd, logFilePath);

            MessageBox.Show("Update Completed. Log saved to:\n" + logFilePath);
        }


        private void UpateStockV1()
        {
            QtyAvailable_grd.Rows.Clear();

            int counter = 1;

            List<Product> p = Fields.CurrentStore.Products.GetList();
            List<Location> l = Fields.CurrentStore.Locations.GetList();

            foreach (Product pr in p)
            {
                foreach (Variant vr in pr.variants)
                {
                    int OceanQty = 0;//vdal.GetVariantQtyFromOcean(vr.barcode,"");

                    if (OceanQty != (int)vr.inventory_quantity)
                    {
                        if (OceanQty >= 0)
                        {
                            Variant tv = Fields.CurrentStore.Products.UpdateInvetorey(vr, OceanQty, 6019590);
                            if (tv.inventory_Level != null)
                            {
                                QtyAvailable_grd.Rows.Add(vr.barcode, vr.inventory_quantity, OceanQty, "", "", "", "", "OK");
                                QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                QtyAvailable_grd.Rows.Add(vr.barcode, vr.inventory_quantity, OceanQty, "", "", "", "", tv.LastError.title[0]);
                                QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                            }
                        }
                        else if (OceanQty == -1)
                        {
                            QtyAvailable_grd.Rows.Add(vr.barcode, vr.inventory_quantity, OceanQty, "", "", "", "", "Barcdode not exist in ocean.");
                            QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;

                        }
                    }
                }
                counter += 1;
            }
            //setRowNumber(NoQty_grd); setRowNumber(nonExistBarcodes_grd); setRowNumber(QtyAvailable_grd);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QtyAvailable_grd.Rows.Clear();


            List<Variant> v = Fields.CurrentStore.Products.ZeroWeightVariants;

            foreach (Variant vr in v)
            {
                if ((int)vr.inventory_quantity != 0 && (int)vr.weight == 0)
                {
                    QtyAvailable_grd.Rows.Add(vr.barcode, vr.weight, 0);
                }
            }


        }

        private void updatePrices_btn_Click(object sender, EventArgs e)
        {
            UpdatePrices();
        }

        private void UpdatePrices()
        {
            QtyAvailable_grd.Rows.Clear();

            int counter = 1;

            List<Product> p = Fields.CurrentStore.Products.GetList();

            int pbln = p.Count;
            progressBar1.Maximum = pbln;
            progressBar1.Value = 0;

            foreach (Product pr in p)
            {
                foreach (Variant vr in pr.variants)
                {
                    decimal price = vdal.GetVariantPriceFromOcean(vr);

                    decimal compareAtPrice = vdal.GetVariantComparedAtPriceFromOcean(vr);
                    #region old code
                    //if (price != Convert.ToDecimal(vr.price))
                    //{
                    //    if (price > 0M)
                    //    {
                    //        Variant tv = General.montaniashop.Products.UpdatePrices(vr,price, comparedToPrice);
                    //        if (tv != null)
                    //        {
                    //            QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, comparedToPrice, "OK");
                    //            QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                    //        }
                    //        else
                    //        {
                    //            QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, comparedToPrice, "Failed To Update Prices.");
                    //            QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                    //        }
                    //    }
                    //    else if (price == 0M)
                    //    {
                    //        QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, comparedToPrice, "Cannot Update Price to 0");
                    //        QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    //    }
                    //    else if (price == -1M)
                    //    {
                    //        QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, comparedToPrice, "Barcode does not exist in ocean.");
                    //        QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    //    }
                    //}
                    #endregion

                    if (price > 0M)
                    {
                        if (price != Convert.ToDecimal(vr.price) || (compareAtPrice > 0 && compareAtPrice != Convert.ToDecimal(vr.compare_at_price)))
                        {
                            Variant tv = Fields.CurrentStore.Products.UpdatePrices(vr, price, compareAtPrice);
                            if (tv != null)
                            {
                                QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, compareAtPrice, "OK");
                                QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, compareAtPrice, "Failed To Update Prices.");
                                QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                            }
                        }
                        else if (compareAtPrice == 0M && Convert.ToDecimal(vr.price) != 0M && Convert.ToDecimal(vr.price) == Convert.ToDecimal(vr.compare_at_price))
                        {
                            Variant tv = Fields.CurrentStore.Products.UpdatePrices(vr, Convert.ToDecimal(vr.price), 0M);
                            if (tv != null)
                            {
                                QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, compareAtPrice, "Compare at price set to 0");
                                QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, compareAtPrice, "Failed To Update Prices.");
                                QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                            }

                        }
                        else
                        {
                            QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, compareAtPrice, "Prices are the same, no update needed.");
                            QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                        }
                    }
                    else if (price == 0M)
                    {
                        QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, compareAtPrice, "Cannot Update Price to 0");
                        QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    }
                    else if (price == -1M)
                    {
                        QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, compareAtPrice, "Barcode does not exist in ocean.");
                        QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        QtyAvailable_grd.Rows.Add(vr.barcode, "", "", vr.price, price, vr.compare_at_price, compareAtPrice, "this case has never handled.");
                        QtyAvailable_grd.Rows[QtyAvailable_grd.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    }
                    this.Refresh();
                }
                counter += 1;
                progressBar1.Value += 1;

            }
            MessageBox.Show("Price Update Completed.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QtyAvailable_grd.Rows.Clear();


            List<Variant> v = Fields.CurrentStore.Products.ZeroPriceVariants;

            foreach (Variant vr in v)
            {
                if ((int)vr.inventory_quantity != 0)
                {
                    QtyAvailable_grd.Rows.Add(vr.barcode, vr.weight, 0);
                }
            }
        }

        private void ExportGridToJson(DataGridView grid, string filePath)
        {
            var rows = new List<Dictionary<string, object>>();

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (!row.IsNewRow)
                {
                    var rowData = new Dictionary<string, object>();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string columnName = grid.Columns[cell.ColumnIndex].HeaderText;
                        rowData[columnName] = cell.Value;
                    }
                    rows.Add(rowData);
                }
            }

            string json = JsonConvert.SerializeObject(rows, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void ShopifyQtyUpdate_frm_Load(object sender, EventArgs e)
        {

        }
    }
}
